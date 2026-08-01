using FilistinProje.Core.Enums;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Web.Resources;
using FilistinProje.Web.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ToptanciController : AdminBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly KanvasDbContext _db;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ToptanciController(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            KanvasDbContext db,
            IStringLocalizer<SharedResource> localizer)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index(WholesaleStatus? durum = null, string? arama = null)
        {
            var query = _userManager.Users.AsQueryable();

            if (durum.HasValue)
                query = query.Where(u => u.WholesaleStatus == durum.Value);

            if (!string.IsNullOrWhiteSpace(arama))
            {
                var term = arama.Trim().ToLowerInvariant();
                query = query.Where(u =>
                    (u.AdSoyad != null && u.AdSoyad.ToLower().Contains(term)) ||
                    (u.Email != null && u.Email.ToLower().Contains(term)) ||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(term)) ||
                    (u.KimlikNo != null && u.KimlikNo.Contains(term)));
            }

            var allUsers = await query
                .OrderByDescending(u => u.WholesaleStatus == WholesaleStatus.Pending)
                .ThenByDescending(u => u.BasvuruTarihi ?? DateTime.MinValue)
                .ToListAsync();

            var items = new List<ToptanciViewModel>(allUsers.Count);
            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var isWholesale = roles.Contains(AdminSecurityRoles.Wholesale, StringComparer.OrdinalIgnoreCase);

                items.Add(new ToptanciViewModel
                {
                    Id = user.Id,
                    AdSoyad = user.AdSoyad,
                    Email = user.Email ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    WholesaleStatus = user.WholesaleStatus,
                    StatusLabel = GetStatusLabel(user.WholesaleStatus),
                    IsWholesaleRole = isWholesale,
                    KimlikNo = user.KimlikNo,
                    DogumTarihi = user.DogumTarihi,
                    KimlikFotografYolu = user.KimlikFotografYolu,
                    Adres = user.Adres,
                    Sehir = user.Sehir,
                    BasvuruTarihi = user.BasvuruTarihi,
                    ToptanciRedSebebi = user.ToptanciRedSebebi
                });
            }

            ViewBag.SeciliDurum = durum;
            ViewBag.AramaTerimi = arama;
            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Onayla(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Hata"] = _localizer["Admin_Sonuc_bulunamadi"].Value;
                return RedirectToAction(nameof(Index));
            }

            if (!await _roleManager.RoleExistsAsync(AdminSecurityRoles.Wholesale))
            {
                await _roleManager.CreateAsync(new IdentityRole(AdminSecurityRoles.Wholesale));
            }

            if (!await _userManager.IsInRoleAsync(user, AdminSecurityRoles.Wholesale))
            {
                await _userManager.AddToRoleAsync(user, AdminSecurityRoles.Wholesale);
            }

            user.WholesaleStatus = WholesaleStatus.Approved;
            user.ToptanciRedSebebi = null;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                TempData["Hata"] = "حدث خطأ أثناء الموافقة.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Basari"] = $"{user.AdSoyad} تم الموافقة.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reddet(string id, string? redSebebi = null)
        {
            var temizRedSebebi = redSebebi?.Trim();
            if (string.IsNullOrWhiteSpace(temizRedSebebi))
            {
                return BadRequest(new { success = false, message = "Red sebebi zorunludur." });
            }

            if (temizRedSebebi.Length > 1000)
            {
                return BadRequest(new { success = false, message = "Red sebebi en fazla 1000 karakter olabilir." });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { success = false, message = _localizer["Admin_Sonuc_bulunamadi"].Value });
            }

            if (await _userManager.IsInRoleAsync(user, AdminSecurityRoles.Wholesale))
            {
                await _userManager.RemoveFromRoleAsync(user, AdminSecurityRoles.Wholesale);
            }

            user.WholesaleStatus = WholesaleStatus.Rejected;
            user.ToptanciRedSebebi = temizRedSebebi;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new { success = false, message = "حدث خطأ أثناء الرفض." });
            }

            return Ok(new { success = true, message = $"{user.AdSoyad} تم الرفض." });
        }

        [HttpGet]
        public async Task<IActionResult> Detay(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var isWholesale = roles.Contains(AdminSecurityRoles.Wholesale, StringComparer.OrdinalIgnoreCase);

            var siparisSayisi = await _db.Siparisler.CountAsync(s => s.AppUserId == user.Id && !s.SilindiMi);

            return Json(new
            {
                user.Id,
                user.AdSoyad,
                user.Email,
                user.PhoneNumber,
                user.KimlikNo,
                DogumTarihi = user.DogumTarihi?.ToString("dd.MM.yyyy"),
                KimlikBelgeUrl = string.IsNullOrWhiteSpace(user.KimlikFotografYolu)
                    ? null
                    : Url.Action("Kimlik", "Belge", new { area = string.Empty, userId = user.Id }),
                user.Adres,
                user.Sehir,
                user.ToptanciRedSebebi,
                BasvuruTarihi = user.BasvuruTarihi?.ToString("dd.MM.yyyy HH:mm"),
                WholesaleStatus = user.WholesaleStatus.ToString(),
                StatusLabel = GetStatusLabel(user.WholesaleStatus),
                IsWholesaleRole = isWholesale,
                SiparisSayisi = siparisSayisi
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Beklemede(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Hata"] = _localizer["Admin_Sonuc_bulunamadi"].Value;
                return RedirectToAction(nameof(Index));
            }

            if (await _userManager.IsInRoleAsync(user, AdminSecurityRoles.Wholesale))
            {
                await _userManager.RemoveFromRoleAsync(user, AdminSecurityRoles.Wholesale);
            }

            user.WholesaleStatus = WholesaleStatus.Pending;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                TempData["Hata"] = "حدث خطأ أثناء تحديث الحالة.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Basari"] = $"{user.AdSoyad} تم وضع المستخدم في قائمة الانتظار.";
            return RedirectToAction(nameof(Index));
        }

        #region ÃœrÃ¼n GruplarÄ±

        public async Task<IActionResult> UrunGruplari()
        {
            var gruplar = await _db.ToptanciUrunGruplari
                .Include(g => g.IskontoOranlari.Where(i => !i.SilindiMi))
                .Include(g => g.Urunler)
                .Where(g => !g.SilindiMi)
                .OrderBy(g => g.Sira)
                .ThenBy(g => g.Ad)
                .ToListAsync();

            return View(gruplar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GrupKaydet(ToptanciUrunGrubu model)
        {
            if (string.IsNullOrWhiteSpace(model.Ad))
            {
                TempData["Hata"] = _localizer["Admin_WholesaleGroupNameRequired"].Value;
                return RedirectToAction(nameof(UrunGruplari));
            }

            if (model.Id > 0)
            {
                var existing = await _db.ToptanciUrunGruplari.FindAsync(model.Id);
                if (existing == null)
                {
                    TempData["Hata"] = _localizer["Admin_WholesaleGroupNotFound"].Value;
                    return RedirectToAction(nameof(UrunGruplari));
                }
                existing.Ad = model.Ad.Trim();
                existing.Aciklama = model.Aciklama?.Trim();
                existing.AktifMi = model.AktifMi;
                existing.Sira = model.Sira;
                _db.ToptanciUrunGruplari.Update(existing);
            }
            else
            {
                model.Ad = model.Ad.Trim();
                model.SilindiMi = false;
                model.OlusturulmaTarihi = DateTime.UtcNow;
                await _db.ToptanciUrunGruplari.AddAsync(model);
            }

            await _db.SaveChangesAsync();
            TempData["Basari"] = _localizer["WholesaleGroupSaved"].Value;
            return RedirectToAction(nameof(UrunGruplari));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GrupSil(int id)
        {
            var grup = await _db.ToptanciUrunGruplari.FindAsync(id);
            if (grup != null)
            {
                grup.SilindiMi = true;
                await _db.SaveChangesAsync();
            }

            TempData["Basari"] = _localizer["WholesaleGroupDeleted"].Value;
            return RedirectToAction(nameof(UrunGruplari));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IskontoKaydet(ToptanciIskontoOrani model)
        {
            if (model.ToptanciUrunGrubuId <= 0)
            {
                TempData["Hata"] = _localizer["Admin_WholesaleInvalidGroup"].Value;
                return RedirectToAction(nameof(UrunGruplari));
            }

            if (model.MinAdet < 1)
            {
                TempData["Hata"] = _localizer["Admin_WholesaleMinimumQuantityInvalid"].Value;
                return RedirectToAction(nameof(UrunGruplari));
            }

            if (model.IskontoYuzdesi < 0 || model.IskontoYuzdesi > 100)
            {
                TempData["Hata"] = _localizer["Admin_WholesaleDiscountRateInvalid"].Value;
                return RedirectToAction(nameof(UrunGruplari));
            }

            if (model.Id > 0)
            {
                var existing = await _db.ToptanciIskontoOranlari.FindAsync(model.Id);
                if (existing == null)
                {
                    TempData["Hata"] = _localizer["Admin_WholesaleDiscountNotFound"].Value;
                    return RedirectToAction(nameof(UrunGruplari));
                }
                existing.MinAdet = model.MinAdet;
                existing.IskontoYuzdesi = model.IskontoYuzdesi;
                existing.AktifMi = model.AktifMi;
                _db.ToptanciIskontoOranlari.Update(existing);
            }
            else
            {
                var groupExists = await _db.ToptanciUrunGruplari
                    .AnyAsync(g => g.Id == model.ToptanciUrunGrubuId && !g.SilindiMi);
                if (!groupExists)
                {
                    TempData["Hata"] = _localizer["Admin_WholesaleGroupNotFound"].Value;
                    return RedirectToAction(nameof(UrunGruplari));
                }

                model.SilindiMi = false;
                model.OlusturulmaTarihi = DateTime.UtcNow;
                await _db.ToptanciIskontoOranlari.AddAsync(model);
            }

            await _db.SaveChangesAsync();
            TempData["Basari"] = _localizer["WholesaleDiscountSaved"].Value;
            return RedirectToAction(nameof(UrunGruplari));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IskontoSil(int id)
        {
            var kayit = await _db.ToptanciIskontoOranlari.FindAsync(id);
            if (kayit != null)
            {
                kayit.SilindiMi = true;
                await _db.SaveChangesAsync();
            }

            TempData["Basari"] = _localizer["WholesaleDiscountDeleted"].Value;
            return RedirectToAction(nameof(UrunGruplari));
        }

        #endregion

        private static string GetStatusLabel(WholesaleStatus status) => status switch
        {
            WholesaleStatus.Pending => "قيد الانتظار",
            WholesaleStatus.Approved => "موافق عليه",
            WholesaleStatus.Rejected => "مرفوض",
            _ => "غير معروف"
        };
    }

    public class ToptanciViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string AdSoyad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public WholesaleStatus WholesaleStatus { get; set; }
        public string StatusLabel { get; set; } = string.Empty;
        public bool IsWholesaleRole { get; set; }
        public string KimlikNo { get; set; } = string.Empty;
        public DateTime? DogumTarihi { get; set; }
        public string KimlikFotografYolu { get; set; } = string.Empty;
        public string Adres { get; set; } = string.Empty;
        public string Sehir { get; set; } = string.Empty;
        public DateTime? BasvuruTarihi { get; set; }
        public string? ToptanciRedSebebi { get; set; }
    }
}


