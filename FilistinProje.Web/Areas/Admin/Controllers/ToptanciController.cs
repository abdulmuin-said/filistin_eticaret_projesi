using FilistinProje.Core.Enums;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Web.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ToptanciController : AdminBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly KanvasDbContext _db;

        public ToptanciController(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            KanvasDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
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
                    BasvuruTarihi = user.BasvuruTarihi
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
                TempData["Hata"] = "Kullanıcı bulunamadı.";
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
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                TempData["Hata"] = "Onaylama sırasında hata oluştu.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Basari"] = $"{user.AdSoyad} toptancı olarak onaylandı.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reddet(string id, string? redSebebi = null)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Hata"] = "Kullanıcı bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            if (await _userManager.IsInRoleAsync(user, AdminSecurityRoles.Wholesale))
            {
                await _userManager.RemoveFromRoleAsync(user, AdminSecurityRoles.Wholesale);
            }

            user.WholesaleStatus = WholesaleStatus.Rejected;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                TempData["Hata"] = "Reddetme sırasında hata oluştu.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Basari"] = $"{user.AdSoyad} toptancı başvurusu reddedildi.";
            return RedirectToAction(nameof(Index));
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
                user.KimlikFotografYolu,
                user.Adres,
                user.Sehir,
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
                TempData["Hata"] = "Kullanıcı bulunamadı.";
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
                TempData["Hata"] = "Durum güncellenirken hata oluştu.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Basari"] = $"{user.AdSoyad} toptancı durumu beklemedeye alındı.";
            return RedirectToAction(nameof(Index));
        }

        #region Ürün Grupları

        public async Task<IActionResult> UrunGruplari()
        {
            var gruplar = await _db.ToptanciUrunGruplari
                .Include(g => g.IskontoOranlari.Where(i => !i.SilindiMi))
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
                TempData["Hata"] = "Grup adı boş olamaz.";
                return RedirectToAction(nameof(UrunGruplari));
            }

            if (model.Id > 0)
            {
                var existing = await _db.ToptanciUrunGruplari.FindAsync(model.Id);
                if (existing == null)
                {
                    TempData["Hata"] = "Grup bulunamadı.";
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
            TempData["Basari"] = "Ürün grubu kaydedildi.";
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

            TempData["Basari"] = "Ürün grubu silindi.";
            return RedirectToAction(nameof(UrunGruplari));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IskontoKaydet(ToptanciIskontoOrani model)
        {
            if (model.ToptanciUrunGrubuId <= 0)
            {
                TempData["Hata"] = "Geçersiz grup.";
                return RedirectToAction(nameof(UrunGruplari));
            }

            if (model.MinAdet < 1)
            {
                TempData["Hata"] = "Minimum adet en az 1 olmalıdır.";
                return RedirectToAction(nameof(UrunGruplari));
            }

            if (model.IskontoYuzdesi < 0 || model.IskontoYuzdesi > 100)
            {
                TempData["Hata"] = "İskonto oranı 0-100 arasında olmalıdır.";
                return RedirectToAction(nameof(UrunGruplari));
            }

            if (model.Id > 0)
            {
                var existing = await _db.ToptanciIskontoOranlari.FindAsync(model.Id);
                if (existing == null)
                {
                    TempData["Hata"] = "İskonto kaydı bulunamadı.";
                    return RedirectToAction(nameof(UrunGruplari));
                }
                existing.MinAdet = model.MinAdet;
                existing.IskontoYuzdesi = model.IskontoYuzdesi;
                existing.AktifMi = model.AktifMi;
                _db.ToptanciIskontoOranlari.Update(existing);
            }
            else
            {
                model.SilindiMi = false;
                model.OlusturulmaTarihi = DateTime.UtcNow;
                await _db.ToptanciIskontoOranlari.AddAsync(model);
            }

            await _db.SaveChangesAsync();
            TempData["Basari"] = "İskonto oranı kaydedildi.";
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

            TempData["Basari"] = "İskonto oranı silindi.";
            return RedirectToAction(nameof(UrunGruplari));
        }

        #endregion

        private static string GetStatusLabel(WholesaleStatus status) => status switch
        {
            WholesaleStatus.Pending => "Beklemede",
            WholesaleStatus.Approved => "Onaylı",
            WholesaleStatus.Rejected => "Reddedildi",
            _ => "Bilinmiyor"
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
    }
}
