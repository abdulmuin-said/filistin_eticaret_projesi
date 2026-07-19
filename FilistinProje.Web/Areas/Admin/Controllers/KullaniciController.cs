using System.Text;
using FilistinProje.Core.Models;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Services;
using FilistinProje.Web.Security;
using FilistinProje.Web.Services;
using FilistinProje.Web.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KullaniciController : AdminBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAdminSessionStateService _adminSessionStateService;
        private readonly IAdminSecurityAuditService _auditService;
        private readonly KanvasDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public KullaniciController(
            UserManager<AppUser> userManager,
            IAdminSessionStateService adminSessionStateService,
            IAdminSecurityAuditService auditService,
            KanvasDbContext context,
            IStringLocalizer<SharedResource> localizer)
        {
            _userManager = userManager;
            _adminSessionStateService = adminSessionStateService;
            _auditService = auditService;
            _context = context;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 20)
        {
            page = Math.Max(page, 1);
            pageSize = pageSize is 20 or 50 or 100 ? pageSize : 20;

            var query = _userManager.Users.AsQueryable();
            var term = search?.Trim();

            if (!string.IsNullOrWhiteSpace(term))
            {
                var normalized = term.ToLower();
                query = query.Where(u =>
                    (u.AdSoyad ?? string.Empty).ToLower().Contains(normalized) ||
                    (u.Email ?? string.Empty).ToLower().Contains(normalized) ||
                    (u.PhoneNumber ?? string.Empty).Contains(normalized));
            }

            var totalCount = await query.CountAsync();
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
            page = Math.Min(page, totalPages);

            var adminRoleIds = await _context.Roles
                .AsNoTracking()
                .Where(x => x.Name != null && AdminSecurityRoles.AllAdminRoles.Contains(x.Name))
                .Select(x => x.Id)
                .ToListAsync();

            var adminCount = adminRoleIds.Count == 0
                ? 0
                : await _context.UserRoles
                    .AsNoTracking()
                    .Where(ur => adminRoleIds.Contains(ur.RoleId) && query.Any(u => u.Id == ur.UserId))
                    .Select(ur => ur.UserId)
                    .Distinct()
                    .CountAsync();

            var users = await query
                .OrderByDescending(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var states = await _adminSessionStateService.GetStatesAsync(users.Select(x => x.Id));
            var currentUserId = _userManager.GetUserId(User);
            var canManageSuperAdmin = User.IsInRole(AdminSecurityRoles.SuperAdmin);
            var items = new List<KullaniciListItemViewModel>(users.Count);
            var userIds = users.Select(x => x.Id).ToList();
            var userRoles = await (from userRole in _context.UserRoles.AsNoTracking()
                                   join role in _context.Roles.AsNoTracking() on userRole.RoleId equals role.Id
                                   where userIds.Contains(userRole.UserId) && role.Name != null
                                   select new { userRole.UserId, RoleName = role.Name! })
                .ToListAsync();

            var roleLookup = userRoles
                .GroupBy(x => x.UserId)
                .ToDictionary(x => x.Key, x => (ICollection<string>)x.Select(v => v.RoleName).ToList());

            foreach (var user in users)
            {
                roleLookup.TryGetValue(user.Id, out var roles);
                roles ??= Array.Empty<string>();
                var primaryRole = roles.Count == 0
                    ? AdminSecurityRoles.Uye
                    : AdminSecurityRoles.GetPrimaryRole(roles);

                states.TryGetValue(user.Id, out var sessionState);

                items.Add(new KullaniciListItemViewModel
                {
                    Id = user.Id,
                    AdSoyad = string.IsNullOrWhiteSpace(user.AdSoyad) ? (user.Email ?? _localizer["Admin_UnnamedUser"]) : user.AdSoyad,
                    Email = user.Email ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    Sehir = user.Sehir ?? string.Empty,
                    PrimaryRole = primaryRole,
                    RoleLabel = GetLocalizedRoleOption(primaryRole).Label,
                    IsAdmin = AdminSecurityRoles.IsAdminRole(primaryRole),
                    IsBanned = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow,
                    IsCurrentUser = string.Equals(user.Id, currentUserId, StringComparison.Ordinal),
                    CanManage = canManageSuperAdmin || !IsProtectedAdminRole(primaryRole),
                    LastAdminLoginUtc = sessionState?.CurrentLoginUtc,
                    PreviousAdminLoginUtc = sessionState?.PreviousLoginUtc
                });
            }

            var model = new KullaniciIndexViewModel
            {
                Search = term ?? string.Empty,
                TotalCount = totalCount,
                AdminCount = adminCount,
                CustomerCount = totalCount - adminCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                Kullanicilar = items
                    .OrderByDescending(x => x.IsAdmin)
                    .ThenBy(x => x.AdSoyad)
                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DurumDegistir(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (!await CanManageUserAsync(user))
            {
                return Forbid();
            }

            if (string.Equals(user.Id, _userManager.GetUserId(User), StringComparison.Ordinal))
            {
                TempData["Hata"] = _localizer["Admin_CannotChangeOwnStatus"].Value;
                return RedirectToAction(nameof(Index));
            }

            var isBanned = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow;
            user.LockoutEnd = isBanned ? null : DateTimeOffset.UtcNow.AddYears(100);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                TempData["Hata"] = string.Join(" ", result.Errors.Select(x => x.Description));
                return RedirectToAction(nameof(Index));
            }

            await _adminSessionStateService.ClearSessionAsync(user.Id);

            await _auditService.LogAsync(
                HttpContext,
                isBanned ? "user_unblocked" : "user_blocked",
                isBanned ? _localizer["Admin_AuditUserUnblocked"] : _localizer["Admin_AuditUserBlocked"],
                target: user.Id);

            TempData["Basari"] = isBanned
                ? _localizer["Admin_UserUnblocked"].Value
                : _localizer["Admin_UserBlocked"].Value;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SifreSifirla(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["Hata"] = _localizer["Admin_PasswordUserNotSelected"].Value;
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Hata"] = _localizer["Admin_UserNotFound"].Value;
                return RedirectToAction(nameof(Index));
            }

            if (!await CanManageUserAsync(user))
            {
                return Forbid();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SifreSifirla(string id, string yeniSifre, string sifreTekrar)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Hata"] = _localizer["Admin_UserNotFound"].Value;
                return RedirectToAction(nameof(Index));
            }

            if (!await CanManageUserAsync(user))
            {
                return Forbid();
            }

            if (string.IsNullOrWhiteSpace(yeniSifre))
            {
                TempData["Hata"] = _localizer["Admin_NewPasswordRequired"].Value;
                return View(user);
            }

            if (!string.Equals(yeniSifre, sifreTekrar, StringComparison.Ordinal))
            {
                TempData["Hata"] = _localizer["Admin_PasswordsMustMatch"].Value;
                return View(user);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, yeniSifre);

            if (!result.Succeeded)
            {
                TempData["Hata"] = string.Join(" ", result.Errors.Select(x => x.Description));
                return View(user);
            }

            await _adminSessionStateService.ClearSessionAsync(user.Id);

            await _auditService.LogAsync(
                HttpContext,
                "user_password_reset",
                _localizer["Admin_AuditPasswordReset"],
                target: user.Id);

            TempData["Basari"] = _localizer["Admin_PasswordUpdated"].Value;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Duzenle(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["Hata"] = _localizer["Admin_EditUserNotSelected"].Value;
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Hata"] = _localizer["Admin_UserNotFound"].Value;
                return RedirectToAction(nameof(Index));
            }

            if (!await CanManageUserAsync(user))
            {
                return Forbid();
            }

            return View(await BuildEditModelAsync(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(KullaniciDuzenleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var currentPrimaryRole = currentRoles.Count == 0
                ? AdminSecurityRoles.Uye
                : AdminSecurityRoles.GetPrimaryRole(currentRoles);
            var currentEditableRole = NormalizeEditableRole(currentPrimaryRole);

            if (IsProtectedAdminRole(currentPrimaryRole) && !User.IsInRole(AdminSecurityRoles.SuperAdmin))
            {
                return Forbid();
            }

            var selectedRole = string.IsNullOrWhiteSpace(model.SelectedRole)
                ? AdminSecurityRoles.Uye
                : model.SelectedRole;
            var allowedRoles = GetAssignableRoleOptions()
                .Select(x => x.Value)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (!allowedRoles.Contains(selectedRole))
            {
                ModelState.AddModelError(string.Empty, _localizer["Admin_InvalidRole"]);
                var invalidRoleModel = await BuildEditModelAsync(user);
                invalidRoleModel.SelectedRole = currentEditableRole;
                return View(invalidRoleModel);
            }

            if (IsProtectedAdminRole(selectedRole) && !User.IsInRole(AdminSecurityRoles.SuperAdmin))
            {
                return Forbid();
            }

            var currentUserId = _userManager.GetUserId(User);
            if (string.Equals(user.Id, currentUserId, StringComparison.Ordinal) &&
                !string.Equals(selectedRole, currentEditableRole, StringComparison.OrdinalIgnoreCase))
            {
                TempData["Hata"] = _localizer["Admin_CannotChangeOwnRole"].Value;
                return RedirectToAction(nameof(Duzenle), new { id = user.Id });
            }


            if (string.IsNullOrWhiteSpace(model.AdSoyad))
            {
                ModelState.AddModelError(nameof(model.AdSoyad), _localizer["Admin_NameRequired"]);
                var invalidNameModel = await BuildEditModelAsync(user);
                invalidNameModel.AdSoyad = model.AdSoyad;
                invalidNameModel.PhoneNumber = model.PhoneNumber;
                invalidNameModel.Sehir = model.Sehir;
                invalidNameModel.SelectedRole = selectedRole;
                return View(invalidNameModel);
            }

            string? normalizedPhone = null;
            if (!string.IsNullOrWhiteSpace(model.PhoneNumber) && !PhoneNumberNormalizer.TryNormalize(model.PhoneNumber, out normalizedPhone))
            {
                ModelState.AddModelError(nameof(model.PhoneNumber), _localizer["Admin_InvalidPalestinePhone"]);
                var invalidPhoneModel = await BuildEditModelAsync(user);
                invalidPhoneModel.AdSoyad = model.AdSoyad;
                invalidPhoneModel.PhoneNumber = model.PhoneNumber;
                invalidPhoneModel.Sehir = model.Sehir;
                invalidPhoneModel.SelectedRole = selectedRole;
                return View(invalidPhoneModel);
            }

            user.AdSoyad = model.AdSoyad?.Trim() ?? string.Empty;
            user.PhoneNumber = string.IsNullOrWhiteSpace(model.PhoneNumber) ? null : normalizedPhone;
            user.Sehir = model.Sehir?.Trim() ?? string.Empty;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                AddErrors(updateResult);
                var failedModel = await BuildEditModelAsync(user);
                failedModel.SelectedRole = selectedRole;
                return View(failedModel);
            }

            var rolesToRemove = currentRoles
                .Where(x => AdminSecurityRoles.IsAdminRole(x) || string.Equals(x, AdminSecurityRoles.Uye, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (rolesToRemove.Count > 0)
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                {
                    AddErrors(removeResult);
                    var failedModel = await BuildEditModelAsync(user);
                    failedModel.SelectedRole = currentEditableRole;
                    return View(failedModel);
                }
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, selectedRole);
            if (!addRoleResult.Succeeded)
            {
                AddErrors(addRoleResult);
                var failedModel = await BuildEditModelAsync(user);
                failedModel.SelectedRole = currentEditableRole;
                return View(failedModel);
            }

            var roleChanged = !string.Equals(currentEditableRole, selectedRole, StringComparison.OrdinalIgnoreCase);
            if (roleChanged)
            {
                await _adminSessionStateService.ClearSessionAsync(user.Id);

                await _auditService.LogAsync(
                    HttpContext,
                    "user_role_updated",
                    _localizer["Admin_AuditUserRoleUpdated", GetLocalizedRoleOption(currentEditableRole).Label, GetLocalizedRoleOption(selectedRole).Label],
                    target: user.Id);
            }

            TempData["Basari"] = roleChanged
                ? _localizer["Admin_UserAndRoleUpdated"].Value
                : _localizer["Admin_UserUpdated"].Value;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            if (!await CanManageUserAsync(user))
            {
                return Forbid();
            }

            if (string.Equals(user.Id, _userManager.GetUserId(User), StringComparison.Ordinal))
            {
                TempData["Hata"] = _localizer["Admin_CannotDeleteOwnAccount"].Value;
                return RedirectToAction(nameof(Index));
            }

            await _adminSessionStateService.ClearSessionAsync(user.Id);
            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                TempData["Hata"] = string.Join(" ", result.Errors.Select(x => x.Description));
                return RedirectToAction(nameof(Index));
            }

            await _auditService.LogAsync(
                HttpContext,
                "user_deleted",
                _localizer["Admin_AuditUserDeleted"],
                target: user.Id);

            TempData["Basari"] = _localizer["Admin_UserDeleted"].Value;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ExcelExport()
        {
            var users = await _userManager.Users.ToListAsync();
            var states = await _adminSessionStateService.GetStatesAsync(users.Select(x => x.Id));
            var builder = new StringBuilder();

            builder.AppendLine(string.Join(",", new[]
            {
                "Id",
                EscapeCsv(_localizer["Admin_Ad_Soyad"]),
                EscapeCsv(_localizer["Admin_E_posta"]),
                EscapeCsv(_localizer["Admin_Telefon"]),
                EscapeCsv(_localizer["Admin_Sehir"]),
                EscapeCsv(_localizer["Admin_Role"]),
                EscapeCsv(_localizer["Admin_LastAdminLogin"])
            }));

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var primaryRole = roles.Count == 0
                    ? AdminSecurityRoles.Uye
                    : AdminSecurityRoles.GetPrimaryRole(roles);

                states.TryGetValue(user.Id, out var sessionState);
                var lastLogin = sessionState?.CurrentLoginUtc.HasValue == true
                    ? ToPalestineTime(sessionState.CurrentLoginUtc.Value).ToString("yyyy-MM-dd HH:mm:ss")
                    : string.Empty;

                builder.AppendLine(string.Join(",",
                    EscapeCsv(user.Id),
                    EscapeCsv(user.AdSoyad),
                    EscapeCsv(user.Email),
                    EscapeCsv(user.PhoneNumber),
                    EscapeCsv(user.Sehir),
                    EscapeCsv(GetLocalizedRoleOption(primaryRole).Label),
                    EscapeCsv(lastLogin)));
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", _localizer["Admin_UsersCsvFileName"].Value);
        }

        private async Task<KullaniciDuzenleViewModel> BuildEditModelAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var primaryRole = roles.Count == 0
                ? AdminSecurityRoles.Uye
                : AdminSecurityRoles.GetPrimaryRole(roles);
            var editableRole = NormalizeEditableRole(primaryRole);

            var roleOption = GetLocalizedRoleOption(editableRole);
            var sessionState = await _adminSessionStateService.GetStateAsync(user.Id);

            return new KullaniciDuzenleViewModel
            {
                Id = user.Id,
                AdSoyad = user.AdSoyad,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                Sehir = user.Sehir ?? string.Empty,
                SelectedRole = editableRole,
                CurrentRoleLabel = roleOption.Label,
                CurrentRoleDescription = roleOption.Description,
                IsCurrentUser = string.Equals(user.Id, _userManager.GetUserId(User), StringComparison.Ordinal),
                CanManageSuperAdmin = User.IsInRole(AdminSecurityRoles.SuperAdmin),
                IsBanned = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow,
                LastAdminLoginUtc = sessionState?.CurrentLoginUtc,
                PreviousAdminLoginUtc = sessionState?.PreviousLoginUtc,
                RoleOptions = GetAssignableRoleOptions().ToList()
            };
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private static string NormalizeEditableRole(string roleName)
        {
            return string.Equals(roleName, AdminSecurityRoles.LegacyAdmin, StringComparison.OrdinalIgnoreCase)
                ? AdminSecurityRoles.SuperAdmin
                : roleName;
        }

        private static string EscapeCsv(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var safeValue = value;
            var firstNonWhitespace = safeValue.FirstOrDefault(c => !char.IsWhiteSpace(c));
            if (firstNonWhitespace is '=' or '+' or '-' or '@' || safeValue[0] is '\t' or '\r' or '\n')
            {
                safeValue = "'" + safeValue;
            }

            return $"\"{safeValue.Replace("\"", "\"\"")}\"";
        }

        private async Task<bool> CanManageUserAsync(AppUser user)
        {
            if (User.IsInRole(AdminSecurityRoles.SuperAdmin))
            {
                return true;
            }

            var roles = await _userManager.GetRolesAsync(user);
            return !roles.Any(IsProtectedAdminRole);
        }

        private IReadOnlyList<AdminRoleOption> GetAssignableRoleOptions()
        {
            return AdminSecurityRoles.GetAssignableRoleOptions()
                .Where(x => User.IsInRole(AdminSecurityRoles.SuperAdmin) || !IsProtectedAdminRole(x.Value))
                .Select(x => GetLocalizedRoleOption(x.Value))
                .ToList();
        }

        private AdminRoleOption GetLocalizedRoleOption(string roleName)
        {
            var key = roleName switch
            {
                AdminSecurityRoles.LegacyAdmin => "LegacyAdmin",
                AdminSecurityRoles.SuperAdmin => "SuperAdmin",
                AdminSecurityRoles.Yonetici => "Manager",
                AdminSecurityRoles.SiparisYoneticisi => "OrderManager",
                AdminSecurityRoles.UrunYoneticisi => "ProductManager",
                AdminSecurityRoles.IcerikYoneticisi => "ContentManager",
                AdminSecurityRoles.KargoYoneticisi => "ShippingManager",
                AdminSecurityRoles.Goruntuleyici => "Viewer",
                AdminSecurityRoles.Wholesale => "Wholesale",
                _ => "Member"
            };
            var option = AdminSecurityRoles.GetRoleOption(roleName);
            return new AdminRoleOption(roleName, _localizer[$"Admin_Role_{key}"], _localizer[$"Admin_Role_{key}_Description"], option.SortOrder);
        }

        private static bool IsProtectedAdminRole(string? roleName)
        {
            return string.Equals(roleName, AdminSecurityRoles.SuperAdmin, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(roleName, AdminSecurityRoles.LegacyAdmin, StringComparison.OrdinalIgnoreCase);
        }

        private static DateTime ToPalestineTime(DateTime utc)
        {
            var utcValue = DateTime.SpecifyKind(utc, DateTimeKind.Utc);
            foreach (var id in new[] { "Asia/Hebron", "West Bank Standard Time" })
            {
                try
                {
                    return TimeZoneInfo.ConvertTimeFromUtc(utcValue, TimeZoneInfo.FindSystemTimeZoneById(id));
                }
                catch (TimeZoneNotFoundException) { }
                catch (InvalidTimeZoneException) { }
            }

            return utcValue.AddHours(3);
        }
    }
}


