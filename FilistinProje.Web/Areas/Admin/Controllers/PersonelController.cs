using FilistinProje.Core.Models;
using FilistinProje.Core.Varliklar;
using FilistinProje.Service.Services;
using FilistinProje.Web.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PersonelController : AdminBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAdminSessionStateService _sessionStateService;
        private readonly IAdminSecurityAuditService _auditService;

        public PersonelController(
            UserManager<AppUser> userManager,
            IAdminSessionStateService sessionStateService,
            IAdminSecurityAuditService auditService)
        {
            _userManager = userManager;
            _sessionStateService = sessionStateService;
            _auditService = auditService;
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
                    AdSoyad = string.IsNullOrWhiteSpace(user.AdSoyad) ? (user.Email ?? "Adsız") : user.AdSoyad,
                    Email = user.Email ?? string.Empty,
                    Telefon = user.PhoneNumber ?? string.Empty,
                    RolAdi = primaryRole,
                    RolLabel = AdminSecurityRoles.GetRoleLabel(primaryRole),
                    SonGirisUtc = sessionState?.CurrentLoginUtc,
                    OncekiGirisUtc = sessionState?.PreviousLoginUtc,
                    EngelliMi = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow,
                    KendisiMi = string.Equals(user.Id, currentUserId, StringComparison.Ordinal),
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
                        var roleOption = AdminSecurityRoles.GetRoleOption(kv.Key);
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
                    Label = AdminSecurityRoles.GetRoleLabel(rol),
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
                new() { ControllerAdi = "Home", DisplayAdi = "Dashboard", Grup = "Genel", Ikon = "fa-chart-pie" },
                new() { ControllerAdi = "Rapor", DisplayAdi = "Raporlar", Grup = "Genel", Ikon = "fa-chart-bar" },
                new() { ControllerAdi = "Ziyaretci", DisplayAdi = "Ziyaretçi Logları", Grup = "Genel", Ikon = "fa-eye" },
                new() { ControllerAdi = "Search", DisplayAdi = "Arama", Grup = "Genel", Ikon = "fa-search" },
                new() { ControllerAdi = "Siparis", DisplayAdi = "Siparişler", Grup = "Operasyon", Ikon = "fa-truck" },
                new() { ControllerAdi = "Iade", DisplayAdi = "İade", Grup = "Operasyon", Ikon = "fa-rotate-left" },
                new() { ControllerAdi = "Kargo", DisplayAdi = "Kargo Yönetimi", Grup = "Operasyon", Ikon = "fa-shipping-fast" },
                new() { ControllerAdi = "Urun", DisplayAdi = "Ürünler", Grup = "Katalog", Ikon = "fa-box" },
                new() { ControllerAdi = "Kategori", DisplayAdi = "Kategoriler", Grup = "Katalog", Ikon = "fa-sitemap" },
                new() { ControllerAdi = "Kupon", DisplayAdi = "Kuponlar", Grup = "İçerik", Ikon = "fa-tags" },
                new() { ControllerAdi = "Yorum", DisplayAdi = "Yorumlar", Grup = "İçerik", Ikon = "fa-comments" },
                new() { ControllerAdi = "Sayfa", DisplayAdi = "Sayfalar", Grup = "İçerik", Ikon = "fa-file" },
                new() { ControllerAdi = "Slayt", DisplayAdi = "Slaytlar", Grup = "İçerik", Ikon = "fa-images" },
                new() { ControllerAdi = "AnaSayfa", DisplayAdi = "Anasayfa", Grup = "İçerik", Ikon = "fa-palette" },
                new() { ControllerAdi = "Bulten", DisplayAdi = "Bülten", Grup = "İçerik", Ikon = "fa-envelope-open-text" },
                new() { ControllerAdi = "Iletisim", DisplayAdi = "İletişim Mesajları", Grup = "İçerik", Ikon = "fa-inbox" },
                new() { ControllerAdi = "CarkOdul", DisplayAdi = "Çark Ödülleri", Grup = "İçerik", Ikon = "fa-spinner" },
                new() { ControllerAdi = "PushBildirim", DisplayAdi = "Bildirimler", Grup = "İçerik", Ikon = "fa-bell" },
                new() { ControllerAdi = "HomeSections", DisplayAdi = "Anasayfa Bölümleri", Grup = "İçerik", Ikon = "fa-layer-group" },
                new() { ControllerAdi = "Toptanci", DisplayAdi = "Toptancı Yönetimi", Grup = "Yönetim", Ikon = "fa-warehouse" },
                new() { ControllerAdi = "ToptanciUrunGrubu", DisplayAdi = "Toptan Ürün Grubu", Grup = "Yönetim", Ikon = "fa-cubes" },
                new() { ControllerAdi = "Kullanici", DisplayAdi = "Kullanıcı Yönetimi", Grup = "Yönetim", Ikon = "fa-users" },
                new() { ControllerAdi = "Ayarlar", DisplayAdi = "Site Ayarları", Grup = "Yönetim", Ikon = "fa-gear" },
                new() { ControllerAdi = "Bankalar", DisplayAdi = "Banka Hesapları", Grup = "Yönetim", Ikon = "fa-building-columns" },
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

            await _sessionStateService.ClearSessionAsync(user.Id);

            await _auditService.LogAsync(
                HttpContext,
                "admin_session_cleared",
                $"Personel oturumu admin tarafından temizlendi.",
                "Personel",
                user.Id,
                user.UserName ?? user.Email);

            TempData["Basari"] = $"{(string.IsNullOrWhiteSpace(user.AdSoyad) ? user.Email : user.AdSoyad)} için oturum temizlendi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
