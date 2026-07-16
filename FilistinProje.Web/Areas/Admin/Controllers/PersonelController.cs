using FilistinProje.Core.Models;
using FilistinProje.Core.Varliklar;
using FilistinProje.Service.Services;
using FilistinProje.Web.Security;
using FilistinProje.Web.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PersonelController : AdminBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAdminSessionStateService _sessionStateService;
        private readonly IAdminSecurityAuditService _auditService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public PersonelController(
            UserManager<AppUser> userManager,
            IAdminSessionStateService sessionStateService,
            IAdminSecurityAuditService auditService,
            IStringLocalizer<SharedResource> localizer)
        {
            _userManager = userManager;
            _sessionStateService = sessionStateService;
            _auditService = auditService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var tumKullanicilar = await _userManager.Users
                .OrderByDescending(x => x.BasvuruTarihi ?? x.LockoutEnd ?? DateTime.MaxValue)
                .ToListAsync();

            var tumPersonel = new List<PersonelListItemViewModel>();
            var rolGruplari = new Dictionary<string, List<PersonelListItemViewModel>>(StringComparer.OrdinalIgnoreCase);

            var currentUserId = _userManager.GetUserId(User);
            var son7Gun = DateTime.UtcNow.AddDays(-7);
            var tumKullaniciIdler = tumKullanicilar.Select(x => x.Id).ToList();
            var states = await _sessionStateService.GetStatesAsync(tumKullaniciIdler);

            foreach (var user in tumKullanicilar)
            {
                var roller = await _userManager.GetRolesAsync(user);
                var primaryRole = roller.Count == 0
                    ? AdminSecurityRoles.Uye
                    : AdminSecurityRoles.GetPrimaryRole(roller);

                if (!AdminSecurityRoles.IsAdminRole(primaryRole))
                    continue;

                states.TryGetValue(user.Id, out var sessionState);

                var item = new PersonelListItemViewModel
                {
                    Id = user.Id,
                    AdSoyad = string.IsNullOrWhiteSpace(user.AdSoyad) ? (user.Email ?? _localizer["Admin_UnnamedUser"]) : user.AdSoyad,
                    Email = user.Email ?? string.Empty,
                    Telefon = user.PhoneNumber ?? string.Empty,
                    RolAdi = primaryRole,
                    RolLabel = GetLocalizedRoleOption(primaryRole).Label,
                    SonGirisUtc = sessionState?.CurrentLoginUtc,
                    OncekiGirisUtc = sessionState?.PreviousLoginUtc,
                    EngelliMi = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow,
                    KendisiMi = string.Equals(user.Id, currentUserId, StringComparison.Ordinal),
                    YonetilebilirMi = User.IsInRole(AdminSecurityRoles.SuperAdmin) || !IsProtectedAdminRole(primaryRole),
                    KayitTarihi = user.BasvuruTarihi ?? user.LockoutEnd?.UtcDateTime ?? DateTime.MinValue
                };

                tumPersonel.Add(item);

                if (!rolGruplari.ContainsKey(primaryRole))
                    rolGruplari[primaryRole] = new List<PersonelListItemViewModel>();

                rolGruplari[primaryRole].Add(item);
            }

            var rolSira = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                [AdminSecurityRoles.SuperAdmin] = 0,
                [AdminSecurityRoles.LegacyAdmin] = 1,
                [AdminSecurityRoles.Yonetici] = 2,
                [AdminSecurityRoles.SiparisYoneticisi] = 3,
                [AdminSecurityRoles.UrunYoneticisi] = 4,
                [AdminSecurityRoles.IcerikYoneticisi] = 5,
                [AdminSecurityRoles.KargoYoneticisi] = 6,
                [AdminSecurityRoles.Goruntuleyici] = 7
            };

            var model = new PersonelIndexViewModel
            {
                Stats = new PersonelStatViewModel
                {
                    ToplamPersonel = tumPersonel.Count,
                    SuanAktif = tumPersonel.Count(x =>
                    {
                        if (x.EngelliMi) return false;
                        if (!x.SonGirisUtc.HasValue) return false;
                        return (DateTime.UtcNow - x.SonGirisUtc.Value).TotalHours < 24;
                    }),
                    Son7GunGiris = tumPersonel.Count(x =>
                        x.SonGirisUtc.HasValue && x.SonGirisUtc.Value >= son7Gun),
                    BlokeEdilen = tumPersonel.Count(x => x.EngelliMi)
                },
                Personel = tumPersonel
                    .OrderBy(x => rolSira.GetValueOrDefault(x.RolAdi, 99))
                    .ThenBy(x => x.AdSoyad)
                    .ToList(),
                RolGruplari = rolGruplari
                    .Select(kv =>
                    {
                        var roleOption = GetLocalizedRoleOption(kv.Key);
                        return new PersonelRoleGroupViewModel
                        {
                            RolAdi = kv.Key,
                            RolLabel = roleOption.Label,
                            RolAciklamasi = roleOption.Description,
                            PersonelSayisi = kv.Value.Count,
                            Sira = rolSira.GetValueOrDefault(kv.Key, 99),
                            Personeller = kv.Value
                                .OrderBy(x => x.AdSoyad)
                                .ToList()
                        };
                    })
                    .OrderBy(x => x.Sira)
                    .ToList()
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult YetkiMatrisi()
        {
            var allRoles = AdminSecurityRoles.AllAdminRoles;
            var rollers = allRoles
                .Select((rol, idx) => new YetkiMatrisiRoleItem
                {
                    RolAdi = rol,
                    Label = GetLocalizedRoleOption(rol).Label,
                    Aciklama = GetLocalizedRoleOption(rol).Description,
                    Renk = rol switch
                    {
                        AdminSecurityRoles.SuperAdmin or AdminSecurityRoles.LegacyAdmin => "danger",
                        AdminSecurityRoles.Yonetici => "warning",
                        AdminSecurityRoles.SiparisYoneticisi => "primary",
                        AdminSecurityRoles.UrunYoneticisi => "success",
                        AdminSecurityRoles.IcerikYoneticisi => "info",
                        AdminSecurityRoles.KargoYoneticisi => "secondary",
                        AdminSecurityRoles.Goruntuleyici => "dark",
                        _ => "secondary"
                    },
                    Sira = idx
                })
                .OrderBy(x => x.Sira)
                .ToList();

            var controllerlar = new List<YetkiMatrisiControllerItem>
            {
                MatrixItem("Home", "Dashboard", "General", "fa-chart-pie"),
                MatrixItem("Rapor", "Reports", "General", "fa-chart-bar"),
                MatrixItem("Ziyaretci", "VisitorLogs", "General", "fa-eye"),
                MatrixItem("Search", "Search", "General", "fa-search"),
                MatrixItem("Siparis", "Orders", "Operations", "fa-truck"),
                MatrixItem("Iade", "Returns", "Operations", "fa-rotate-left"),
                MatrixItem("Kargo", "ShippingManagement", "Operations", "fa-shipping-fast"),
                MatrixItem("Urun", "Products", "Catalog", "fa-box"),
                MatrixItem("Kategori", "Categories", "Catalog", "fa-sitemap"),
                MatrixItem("Kupon", "Coupons", "Content", "fa-tags"),
                MatrixItem("Yorum", "Reviews", "Content", "fa-comments"),
                MatrixItem("Sayfa", "Pages", "Content", "fa-file"),
                MatrixItem("Slayt", "Slides", "Content", "fa-images"),
                MatrixItem("AnaSayfa", "Homepage", "Content", "fa-palette"),
                MatrixItem("Bulten", "Newsletter", "Content", "fa-envelope-open-text"),
                MatrixItem("Iletisim", "ContactMessages", "Content", "fa-inbox"),
                MatrixItem("HomeSections", "HomepageSections", "Content", "fa-layer-group"),
                MatrixItem("Toptanci", "WholesaleManagement", "Management", "fa-warehouse"),
                MatrixItem("ToptanciUrunGrubu", "WholesaleProductGroups", "Management", "fa-cubes"),
                MatrixItem("Kullanici", "UserManagement", "Management", "fa-users"),
                MatrixItem("Ayarlar", "SiteSettings", "Management", "fa-gear"),
                MatrixItem("Bankalar", "BankAccounts", "Management", "fa-building-columns"),
            };

            var matris = new Dictionary<string, Dictionary<string, bool>>();
            foreach (var rol in rollers)
            {
                var rolMatrisi = new Dictionary<string, bool>();
                foreach (var ctrl in controllerlar)
                {
                    rolMatrisi[ctrl.ControllerAdi] = AdminPermissionMatrix.CanAccess(
                        new System.Security.Claims.ClaimsPrincipal(
                            new System.Security.Claims.ClaimsIdentity(new[]
                            {
                                new System.Security.Claims.Claim(
                                    System.Security.Claims.ClaimTypes.Role, rol.RolAdi)
                            }, "test")),
                        ctrl.ControllerAdi,
                        "GET");
                }
                matris[rol.RolAdi] = rolMatrisi;
            }

            var model = new YetkiMatrisiViewModel
            {
                Roller = rollers,
                Controllerlar = controllerlar,
                Matris = matris
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OturumTemizle(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any(IsProtectedAdminRole) && !User.IsInRole(AdminSecurityRoles.SuperAdmin))
                return Forbid();

            await _sessionStateService.ClearSessionAsync(user.Id);

            await _auditService.LogAsync(
                HttpContext,
                "admin_session_cleared",
                _localizer["Admin_AuditStaffSessionCleared"],
                "Personel",
                user.Id,
                user.UserName ?? user.Email);

            var displayName = string.IsNullOrWhiteSpace(user.AdSoyad)
                ? user.Email ?? _localizer["Admin_UnnamedUser"].Value
                : user.AdSoyad;
            TempData["Basari"] = _localizer["Admin_StaffSessionCleared", displayName].Value;
            return RedirectToAction(nameof(Index));
        }

        private YetkiMatrisiControllerItem MatrixItem(string controller, string displayKey, string groupKey, string icon)
        {
            return new YetkiMatrisiControllerItem
            {
                ControllerAdi = controller,
                DisplayAdi = _localizer[$"Admin_Matrix_{displayKey}"],
                Grup = _localizer[$"Admin_MatrixGroup_{groupKey}"],
                Ikon = icon
            };
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
                _ => "Viewer"
            };
            var option = AdminSecurityRoles.GetRoleOption(roleName);
            return new AdminRoleOption(roleName, _localizer[$"Admin_Role_{key}"], _localizer[$"Admin_Role_{key}_Description"], option.SortOrder);
        }

        private static bool IsProtectedAdminRole(string? roleName)
        {
            return string.Equals(roleName, AdminSecurityRoles.SuperAdmin, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(roleName, AdminSecurityRoles.LegacyAdmin, StringComparison.OrdinalIgnoreCase);
        }
    }
}
