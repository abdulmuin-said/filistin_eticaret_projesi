using FilistinProje.Core.Varliklar;
using FilistinProje.Core.Enums;
using FilistinProje.Data;
using FilistinProje.Service.Helpers;
using FilistinProje.Web.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FilistinProje.Web.Data
{
    public static class DbSeeder
    {
        public static async Task VerileriYukle(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var configuration = serviceScope.ServiceProvider.GetRequiredService<IConfiguration>();
            var context = serviceScope.ServiceProvider.GetRequiredService<KanvasDbContext>();

            var roleNames = AdminSecurityRoles.AllRoles
                .Distinct(StringComparer.OrdinalIgnoreCase);

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminEmail = configuration["AdminSettings:Email"];
            var adminPassword = configuration["AdminSettings:Password"];
            var seedDefaultAdmin = configuration.GetValue<bool>("AdminSettings:SeedDefaultAdmin");

            if (seedDefaultAdmin && !string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
            {
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    adminUser = new AppUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        AdSoyad = "Sistem Yöneticisi",
                        Sehir = "Ramallah",
                        EmailConfirmed = true
                    };

                    var createResult = await userManager.CreateAsync(adminUser, adminPassword);
                    if (!createResult.Succeeded)
                    {
                        adminUser = null;
                    }
                }

                if (adminUser != null)
                {
                    if (!await userManager.IsInRoleAsync(adminUser, AdminSecurityRoles.LegacyAdmin))
                    {
                        await userManager.AddToRoleAsync(adminUser, AdminSecurityRoles.LegacyAdmin);
                    }
                }
            }

            foreach (var user in userManager.Users.ToList())
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Count == 0)
                {
                    await userManager.AddToRoleAsync(user, AdminSecurityRoles.Uye);
                }
            }

            if (!await context.KargoFirmalari.IgnoreQueryFilters().AnyAsync(x => x.Kod == "filistin-kargo"))
            {
                context.KargoFirmalari.Add(new KargoFirmasi
                {
                    Ad = "Filistin Kargo",
                    Kod = "filistin-kargo",
                    LogoUrl = "/filistin-kargo-logo.svg",
                    Telefon = "0599 123 456",
                    TakipUrl = "https://track.7anrps48.com/kargo/",
                    GondericiUnvan = "7ANRPS48",
                    GondericiAdres = "Ramallah, El-Bireh, Palestine",
                    GondericiTelefon = "+970 59 912 3456",
                    AktifMi = true,
                    VarsayilanMi = true
                });
            }

            var mevcutKodlar = await context.UrunOzellikTanimlari
                .IgnoreQueryFilters()
                .Select(x => x.Kod)
                .ToListAsync();

            foreach (var definition in UrunOzellikCatalog.GetDefaultDefinitions())
            {
                if (mevcutKodlar.Contains(definition.Kod, StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }

                var entity = new UrunOzellikTanimi
                {
                    OlusturulmaTarihi = DateTime.UtcNow,
                    SilindiMi = false
                };

                UrunOzellikCatalog.ApplySeed(entity, definition);
                context.UrunOzellikTanimlari.Add(entity);
            }

            await context.SaveChangesAsync();

            var jsonToCategoryMap = new Dictionary<string, string>
            {
                { "Åehir ve Mimari", "Sehir ve Mimari" },
                { "Manzara TemalÄ±", "Manzara Temali" },
                { "Ã‡iÃ§ekli ve Dekoratif", "Cicekli ve Dekoratif" },
                { "Hayvanlar Alemi", "Hayvanlar Alemi" },
                { "Modern & Soyut", "Modern ve Soyut" },
                { "Ã‡ocuk OdasÄ±", "Cocuk Odasi" },
                { "FarklÄ± TasarÄ±mlar", "Farkli Tasarimlar" },
                { "Dini ve Hat SanatÄ±", "Dini ve Hat Sanati" },
                { "Klasik Eserler", "Klasik Eserler" },
                { "AtatÃ¼rk ve TÃ¼rkiye", "Ataturk ve Turkiye" },
                { "KadÄ±n TemalÄ±", "Kadin Temali" },
                { "Yiyecek ve Ä°Ã§ecek", "Yiyecek ve Icecek" },
                { "OsmanlÄ± ve TuÄŸra", "Osmanli ve Tugra" },
                { "Soyut ve Sanatsal", "Soyut ve Sanatsal" }
            };

            var existingKategoriler = await context.Kategoriler.IgnoreQueryFilters().ToListAsync();
            if (!existingKategoriler.Any(x => x.Ad == "Åehir ve Mimari"))
            {
                int sira = 1;
                foreach (var kvp in jsonToCategoryMap)
                {
                    var kategori = new Kategori
                    {
                        Ad = kvp.Key,
                        Slug = kvp.Value.ToLower().Replace(" ", "-"),
                        KisaAciklama = "",
                        Aciklama = "",
                        SeoTitle = kvp.Key + " - 7ANRPS48",
                        SeoDescription = kvp.Key + " kategorisindeki kanvas tablolar",
                        Sira = sira,
                        AktifMi = true,
                        SilindiMi = false,
                        OlusturulmaTarihi = DateTime.UtcNow
                    };
                    context.Kategoriler.Add(kategori);
                    sira++;
                }
                await context.SaveChangesAsync();
            }

            await TestUrunleriSeedle(context);

            var existingSlaytlar = await context.Slaytlar.IgnoreQueryFilters().ToListAsync();
            if (!existingSlaytlar.Any())
            {
                context.Slaytlar.AddRange(new List<Slayt>
                {
                    new Slayt
                    {
                        Baslik = "YaÅŸam AlanÄ±nÄ±za Sanat KatÄ±n",
                        AltBaslik = "SeÃ§kin Tablo KoleksiyonlarÄ±",
                        Aciklama = "Evinizin ve ofisinizin havasÄ±nÄ± deÄŸiÅŸtirecek kanvas ve cam tablolar.",
                        ResimUrl = "/img/banner/slider-1.webp",
                        VideoUrl = "/video/slider1.mp4",
                        Tur = "Video",
                        Sira = 1,
                        AktifMi = true,
                        OlusturmaTarihi = DateTime.UtcNow
                    },
                    new Slayt
                    {
                        Baslik = "Modern ve Minimalist Ã‡izgiler",
                        AltBaslik = "Yeni Nesil Duvar SanatÄ±",
                        Aciklama = "GÃ¶z alÄ±cÄ± detaylar ve yÃ¼ksek kaliteli baskÄ± teknolojisi.",
                        ResimUrl = "/img/banner/slider-2.webp",
                        VideoUrl = "/video/slider2.mp4",
                        Tur = "Video",
                        Sira = 2,
                        AktifMi = true,
                        OlusturmaTarihi = DateTime.UtcNow
                    },
                    new Slayt
                    {
                        Baslik = "DoÄŸanÄ±n CanlÄ± Renkleri",
                        AltBaslik = "Manzara Eserleri",
                        Aciklama = "DoÄŸanÄ±n en gÃ¼zel anlarÄ±nÄ± yansÄ±tan tablolar.",
                        ResimUrl = "/img/banner/slider-3.webp",
                        VideoUrl = null,
                        Tur = "GÃ¶rsel",
                        Sira = 3,
                        AktifMi = true,
                        OlusturmaTarihi = DateTime.UtcNow
                    }
                });
                await context.SaveChangesAsync();
            }
        }

        private static async Task TestUrunleriSeedle(KanvasDbContext context)
        {
            var mevcutTestUrunleri = await context.Urunler.IgnoreQueryFilters()
                .AnyAsync(x => x.SKU == "TEST-MODERN-001");

            if (mevcutTestUrunleri)
            {
                return;
            }

            var kategoriModern = await context.Kategoriler.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Ad == "Modern & Soyut");
            var kategoriCicek = await context.Kategoriler.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Ad == "Ã‡iÃ§ekli ve Dekoratif");
            var kategoriSehir = await context.Kategoriler.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Ad == "Åehir ve Mimari");

            var now = DateTime.UtcNow;

            // ÃœrÃ¼n 1: Modern Soyut Tablo
            var urun1 = new Urun
            {
                Baslik = "Modern Soyut Kanvas Tablo",
                BaslikEn = "Modern Abstract Canvas Painting",
                BaslikAr = "Ù„ÙˆØ­Ø© ÙƒØ§Ù†ÙØ§Ø³ ØªØ¬Ø±ÙŠØ¯ÙŠØ© Ø­Ø¯ÙŠØ«Ø©",
                KisaAd = "Soyut Sanat",
                Slug = "modern-soyut-kanvas-tablo",
                SKU = "TEST-MODERN-001",
                Barkod = "8690000000011",
                Marka = "7ANRPS48",
                KisaAciklama = "Modern sanat akÄ±mÄ±ndan ilham alan soyut kanvas tablo.",
                KisaAciklamaEn = "Abstract canvas painting inspired by modern art movements.",
                KisaAciklamaAr = "Ù„ÙˆØ­Ø© ÙƒØ§Ù†ÙØ§Ø³ ØªØ¬Ø±ÙŠØ¯ÙŠØ© Ù…Ø³ØªÙˆØ­Ø§Ø© Ù…Ù† Ø§Ù„Ø­Ø±ÙƒØ§Øª Ø§Ù„ÙÙ†ÙŠØ© Ø§Ù„Ø­Ø¯ÙŠØ«Ø©.",
                Aciklama = "Bu soyut kanvas tablo, modern sanatÄ±n enerjisini evinize taÅŸÄ±yor. YÃ¼ksek kaliteli baskÄ± ve premium kanvas malzeme.",
                AciklamaEn = "This abstract canvas brings the energy of modern art into your home. High quality print and premium canvas material.",
                AciklamaAr = "ØªØ¬Ù„Ø¨ Ù„ÙˆØ­Ø© Ø§Ù„ÙƒØ§Ù†ÙØ§Ø³ Ø§Ù„ØªØ¬Ø±ÙŠØ¯ÙŠØ© Ù‡Ø°Ù‡ Ø·Ø§Ù‚Ø© Ø§Ù„ÙÙ† Ø§Ù„Ø­Ø¯ÙŠØ« Ø¥Ù„Ù‰ Ù…Ù†Ø²Ù„Ùƒ. Ø·Ø¨Ø§Ø¹Ø© Ø¹Ø§Ù„ÙŠØ© Ø§Ù„Ø¬ÙˆØ¯Ø© ÙˆÙ…Ø§Ø¯Ø© ÙƒØ§Ù†ÙØ§Ø³ ÙØ§Ø®Ø±Ø©.",
                Fiyat = 550m,
                TopFiyat = 410m,
                Maliyet = 200m,
                KdvOrani = 20,
                KategoriId = kategoriModern?.Id ?? 5,
                AktifMi = true,
                YeniUrunMu = true,
                OneCikanMi = true,
                AnaSayfadaGoster = true,
                Sira = 1,
                MinSiparisAdedi = 1,
                UretimSuresiGun = 3,
                KargoyaVerilisSuresiGun = 2,
                TahminiTeslimSuresiGun = 5,
                StokDurumu = "Stokta",
                OlusturulmaTarihi = now,
                SeoTitle = "Modern Soyut Kanvas Tablo",
                SeoDescription = "Modern soyut kanvas tablo koleksiyonumuzu keÅŸfedin."
            };
            context.Urunler.Add(urun1);
            await context.SaveChangesAsync();

            // Varyantlar: Modern Soyut
            var varyantlar1 = new[]
            {
                new UrunSecenek { UrunId = urun1.Id, Olcu = "30x40 cm", SatisFiyati = 200m, MaliyetFiyati = 80m, StokAdedi = 50, VarsayilanMi = true, Sira = 1, OlusturulmaTarihi = now },
                new UrunSecenek { UrunId = urun1.Id, Olcu = "50x70 cm", SatisFiyati = 350m, MaliyetFiyati = 140m, StokAdedi = 30, Sira = 2, OlusturulmaTarihi = now },
                new UrunSecenek { UrunId = urun1.Id, Olcu = "70x100 cm", SatisFiyati = 550m, MaliyetFiyati = 220m, StokAdedi = 15, Sira = 3, OlusturulmaTarihi = now }
            };
            context.UrunSecenekleri.AddRange(varyantlar1);

            // ÃœrÃ¼n 2: Ã‡iÃ§ekli Dekoratif Poster
            var urun2 = new Urun
            {
                Baslik = "Ã‡iÃ§ekli Dekoratif Poster",
                BaslikEn = "Floral Decorative Poster",
                BaslikAr = "Ø¨ÙˆØ³ØªØ± Ø²Ø®Ø±ÙÙŠ Ø¨Ø§Ù„Ø£Ø²Ù‡Ø§Ø±",
                KisaAd = "Ã‡iÃ§ekli Poster",
                Slug = "cicekli-dekoratif-poster",
                SKU = "TEST-CICEK-002",
                Barkod = "8690000000028",
                Marka = "7ANRPS48",
                KisaAciklama = "DoÄŸanÄ±n renklerini yansÄ±tan Ã§iÃ§ekli dekoratif poster.",
                KisaAciklamaEn = "Floral decorative poster reflecting the colors of nature.",
                KisaAciklamaAr = "Ø¨ÙˆØ³ØªØ± Ø²Ø®Ø±ÙÙŠ Ø¨Ø§Ù„Ø£Ø²Ù‡Ø§Ø± ÙŠØ¹ÙƒØ³ Ø£Ù„ÙˆØ§Ù† Ø§Ù„Ø·Ø¨ÙŠØ¹Ø©.",
                Aciklama = "CanlÄ± renkler ve zarif desenlerle hazÄ±rlanmÄ±ÅŸ Ã§iÃ§ekli poster. Her ortama doÄŸal bir hava katar.",
                AciklamaEn = "Floral poster prepared with vibrant colors and elegant patterns. Adds a natural touch to any environment.",
                AciklamaAr = "Ø¨ÙˆØ³ØªØ± Ø²Ù‡ÙˆØ± Ø¨Ø£Ù„ÙˆØ§Ù† Ø²Ø§Ù‡ÙŠØ© ÙˆØ£Ù†Ù…Ø§Ø· Ø£Ù†ÙŠÙ‚Ø©. ÙŠØ¶ÙÙŠ Ù„Ù…Ø³Ø© Ø·Ø¨ÙŠØ¹ÙŠØ© Ø¹Ù„Ù‰ Ø£ÙŠ Ù…ÙƒØ§Ù†.",
                Fiyat = 320m,
                TopFiyat = 240m,
                Maliyet = 100m,
                KdvOrani = 20,
                KategoriId = kategoriCicek?.Id ?? 3,
                AktifMi = true,
                AnaSayfadaGoster = true,
                Sira = 2,
                MinSiparisAdedi = 1,
                UretimSuresiGun = 2,
                KargoyaVerilisSuresiGun = 1,
                TahminiTeslimSuresiGun = 4,
                StokDurumu = "Stokta",
                OlusturulmaTarihi = now,
                SeoTitle = "Ã‡iÃ§ekli Dekoratif Poster",
                SeoDescription = "Ã‡iÃ§ekli dekoratif poster Ã§eÅŸitleri."
            };
            context.Urunler.Add(urun2);
            await context.SaveChangesAsync();

            var varyantlar2 = new[]
            {
                new UrunSecenek { UrunId = urun2.Id, Olcu = "20x30 cm", SatisFiyati = 80m, MaliyetFiyati = 30m, StokAdedi = 100, VarsayilanMi = true, Sira = 1, OlusturulmaTarihi = now },
                new UrunSecenek { UrunId = urun2.Id, Olcu = "40x60 cm", SatisFiyati = 180m, MaliyetFiyati = 70m, StokAdedi = 60, Sira = 2, OlusturulmaTarihi = now },
                new UrunSecenek { UrunId = urun2.Id, Olcu = "60x90 cm", SatisFiyati = 320m, MaliyetFiyati = 130m, StokAdedi = 25, Sira = 3, OlusturulmaTarihi = now }
            };
            context.UrunSecenekleri.AddRange(varyantlar2);

            // ÃœrÃ¼n 3: Premium Åehir SilÃ¼eti
            var urun3 = new Urun
            {
                Baslik = "Premium Åehir SilÃ¼eti Kanvas",
                BaslikEn = "Premium City Skyline Canvas",
                BaslikAr = "ÙƒØ§Ù†ÙØ§Ø³ Ø£ÙÙ‚ Ø§Ù„Ù…Ø¯ÙŠÙ†Ø© Ø§Ù„ÙØ§Ø®Ø±",
                KisaAd = "Åehir SilÃ¼eti",
                Slug = "premium-sehir-silueti-kanvas",
                SKU = "TEST-SEHIR-003",
                Barkod = "8690000000035",
                Marka = "7ANRPS48",
                KisaAciklama = "Modern ÅŸehir silÃ¼etini yansÄ±tan premium kanvas tablo.",
                KisaAciklamaEn = "Premium canvas painting reflecting the modern city skyline.",
                KisaAciklamaAr = "Ù„ÙˆØ­Ø© ÙƒØ§Ù†ÙØ§Ø³ ÙØ§Ø®Ø±Ø© ØªØ¹ÙƒØ³ Ø£ÙÙ‚ Ø§Ù„Ù…Ø¯ÙŠÙ†Ø© Ø§Ù„Ø­Ø¯ÙŠØ«Ø©.",
                Aciklama = "Ä°stanbul, Londra ve Dubai silÃ¼etlerinden ilham alan bu premium kanvas tablo, ÅŸehir yaÅŸamÄ±nÄ±n enerjisini yansÄ±tÄ±r.",
                AciklamaEn = "Inspired by the skylines of Istanbul, London and Dubai, this premium canvas reflects the energy of city life.",
                AciklamaAr = "Ù…Ø³ØªÙˆØ­Ø§Ø© Ù…Ù† Ø£ÙÙ‚ Ø¥Ø³Ø·Ù†Ø¨ÙˆÙ„ ÙˆÙ„Ù†Ø¯Ù† ÙˆØ¯Ø¨ÙŠØŒ ØªØ¹ÙƒØ³ Ù„ÙˆØ­Ø© Ø§Ù„ÙƒØ§Ù†ÙØ§Ø³ Ø§Ù„ÙØ§Ø®Ø±Ø© Ù‡Ø°Ù‡ Ø·Ø§Ù‚Ø© Ø§Ù„Ø­ÙŠØ§Ø© Ø§Ù„Ø­Ø¶Ø±ÙŠØ©.",
                Fiyat = 950m,
                TopFiyat = 710m,
                Maliyet = 350m,
                KdvOrani = 20,
                KategoriId = kategoriSehir?.Id ?? 1,
                AktifMi = true,
                OneCikanMi = true,
                Sira = 3,
                MinSiparisAdedi = 1,
                UretimSuresiGun = 5,
                KargoyaVerilisSuresiGun = 2,
                TahminiTeslimSuresiGun = 7,
                StokDurumu = "Stokta",
                OlusturulmaTarihi = now,
                SeoTitle = "Premium Åehir SilÃ¼eti Kanvas",
                SeoDescription = "Premium ÅŸehir silÃ¼eti kanvas tablo koleksiyonu."
            };
            context.Urunler.Add(urun3);
            await context.SaveChangesAsync();

            var varyantlar3 = new[]
            {
                new UrunSecenek { UrunId = urun3.Id, Olcu = "50x70 cm", SatisFiyati = 400m, MaliyetFiyati = 160m, StokAdedi = 40, VarsayilanMi = true, Sira = 1, OlusturulmaTarihi = now },
                new UrunSecenek { UrunId = urun3.Id, Olcu = "70x100 cm", SatisFiyati = 650m, MaliyetFiyati = 260m, StokAdedi = 20, Sira = 2, OlusturulmaTarihi = now },
                new UrunSecenek { UrunId = urun3.Id, Olcu = "100x140 cm", SatisFiyati = 950m, MaliyetFiyati = 380m, StokAdedi = 10, Sira = 3, OlusturulmaTarihi = now }
            };
            context.UrunSecenekleri.AddRange(varyantlar3);

            await context.SaveChangesAsync();

            // --- KARGO BÖLGE VE ŞEHİRLERİ — Filistin ---
            if (!await context.KargoBolgeler.IgnoreQueryFilters().AnyAsync(x => x.Ulke == "Filistin"))
            {
                var filistinBolgelerSeed = new (string Ad, string Aciklama, int Sira, string[] Sehirler)[]
                {
                    ("Gazze", "Kuzey Gazze ve Gazze Şehri bölgesi", 1, new[] { "Gazze", "Şimali Gazze", "Beyt Hanun", "Beyt Lahiya", "Cablus" }),
                    ("Ramallah", "Orta Batı Şeria bölgesi", 2, new[] { "Ramallah", "El-Bireh", "Birzeit", "Abu Dis", "Yabrud" }),
                    ("Nablus", "Kuzey Batı Şeria bölgesi", 3, new[] { "Nablus", "Tubas", "Salfit", "Asira" }),
                    ("El Halil", "Güney Batı Şeria bölgesi", 4, new[] { "El Halil", "Yatta", "Dura", "Halhul", "Sa'ir" }),
                    ("Beytüllahim", "Batı Şeria - Kudüs'e yakın", 5, new[] { "Beytüllahim", "Beyt Jala", "Artas", "Husan" }),
                    ("Cenin", "Kuzey Filistin bölgesi", 6, new[] { "Cenin", "Ajjah", "Kabatiya", "Ya'bad" }),
                    ("Eriha", "Ürdün Vadisi ve Eriha bölgesi", 7, new[] { "Eriha", "El-Auja", "Fasayil", "Nabi Musa" }),
                    ("Tulkerim", "Kuzeybatı Batı Şeria bölgesi", 8, new[] { "Tulkerim", "Anabta", "Nur Şems", "Kafr Sur" }),
                    ("Kalkilya", "Kuzeybatı Batı Şeria - Kalkilya bölgesi", 9, new[] { "Kalkilya", "Azzun", "Kafr Thulth", "Hable" }),
                    ("Kudüs", "Kudüs ve çevresi", 10, new[] { "Kudüs", "Şarkiyye Kudüs", "Al-Eizariya", "Al-Ram", "Abu Dis" })
                };
                var fNow = DateTime.UtcNow;

                foreach (var (ad, aciklama, sira, sehirler) in filistinBolgelerSeed)
                {
                    var bolge = new KargoBolge
                    {
                        Ad = ad,
                        Ulke = "Filistin",
                        Aciklama = aciklama,
                        Sira = sira,
                        OlusturulmaTarihi = fNow
                    };
                    context.KargoBolgeler.Add(bolge);
                    await context.SaveChangesAsync();

                    foreach (var sehir in sehirler)
                    {
                        context.KargoBolgeSehirler.Add(new KargoBolgeSehir
                        {
                            BolgeId = bolge.Id,
                            SehirAdi = sehir,
                            OlusturulmaTarihi = fNow
                        });
                    }
                }
                await context.SaveChangesAsync();

                var kargoList = await context.KargoFirmalari.IgnoreQueryFilters().Where(x => !x.SilindiMi).ToListAsync();
                var bolgeIds = await context.KargoBolgeler.IgnoreQueryFilters().Where(x => x.Ulke == "Filistin" && !x.SilindiMi).Select(x => x.Id).ToListAsync();
                foreach (var kargo in kargoList)
                {
                    foreach (var bolgeId in bolgeIds)
                    {
                        context.KargoBolgeFiyatlari.Add(new KargoBolgeFiyat
                        {
                            KargoFirmasiId = kargo.Id,
                            BolgeId = bolgeId,
                            Fiyat = Math.Round(kargo.Fiyat * 1.8m, 2),
                            OlusturulmaTarihi = fNow
                        });
                    }
                }
                await context.SaveChangesAsync();
            }

            if (!await context.BankaHesaplari.IgnoreQueryFilters().AnyAsync())
            {
                var bankaTime = DateTime.UtcNow;
                context.BankaHesaplari.Add(new BankaHesap
                {
                    BankaAdi = "Bank of Palestine",
                    HesapSahibi = "7ANRPS48 Trading Co.",
                    IBAN = "PS99000102030405060708090",
                    SubeKodu = "001",
                    HesapNo = "102030",
                    AktifMi = true,
                    Sira = 1,
                    OlusturulmaTarihi = bankaTime
                });
                context.BankaHesaplari.Add(new BankaHesap
                {
                    BankaAdi = "Palestine National Bank",
                    HesapSahibi = "7ANRPS48 Trading Co.",
                    IBAN = "PS88010203040506070809100",
                    SubeKodu = "010",
                    HesapNo = "304050",
                    AktifMi = true,
                    Sira = 2,
                    OlusturulmaTarihi = bankaTime
                });
                await context.SaveChangesAsync();
            }

            if (!await context.ToptanciUrunGruplari.IgnoreQueryFilters().AnyAsync())
            {
                var grupTime = DateTime.UtcNow;
                var gruplar = new[]
                {
                    new ToptanciUrunGrubu { Ad = "Tekstil ve Giyim", Aciklama = "KÄ±yafet, kumaÅŸ ve tekstil Ã¼rÃ¼nleri", AktifMi = true, Sira = 1, OlusturulmaTarihi = grupTime },
                    new ToptanciUrunGrubu { Ad = "Elektronik", Aciklama = "Elektronik cihazlar ve aksesuarlar", AktifMi = true, Sira = 2, OlusturulmaTarihi = grupTime },
                    new ToptanciUrunGrubu { Ad = "GÄ±da ve Ä°Ã§ecek", Aciklama = "GÄ±da Ã¼rÃ¼nleri ve iÃ§ecekler", AktifMi = true, Sira = 3, OlusturulmaTarihi = grupTime },
                    new ToptanciUrunGrubu { Ad = "Kozmetik ve BakÄ±m", Aciklama = "Kozmetik, kiÅŸisel bakÄ±m ve temizlik Ã¼rÃ¼nleri", AktifMi = true, Sira = 4, OlusturulmaTarihi = grupTime }
                };
                await context.ToptanciUrunGruplari.AddRangeAsync(gruplar);
                await context.SaveChangesAsync();

                var tekstil = gruplar[0];
                var elektronik = gruplar[1];
                var gida = gruplar[2];

                var iskontoTime = DateTime.UtcNow;
                var iskontolar = new[]
                {
                    new ToptanciIskontoOrani { ToptanciUrunGrubuId = tekstil.Id, MinAdet = 10, IskontoYuzdesi = 5, AktifMi = true, OlusturulmaTarihi = iskontoTime },
                    new ToptanciIskontoOrani { ToptanciUrunGrubuId = tekstil.Id, MinAdet = 50, IskontoYuzdesi = 10, AktifMi = true, OlusturulmaTarihi = iskontoTime },
                    new ToptanciIskontoOrani { ToptanciUrunGrubuId = tekstil.Id, MinAdet = 100, IskontoYuzdesi = 15, AktifMi = true, OlusturulmaTarihi = iskontoTime },
                    new ToptanciIskontoOrani { ToptanciUrunGrubuId = elektronik.Id, MinAdet = 5, IskontoYuzdesi = 3, AktifMi = true, OlusturulmaTarihi = iskontoTime },
                    new ToptanciIskontoOrani { ToptanciUrunGrubuId = elektronik.Id, MinAdet = 20, IskontoYuzdesi = 8, AktifMi = true, OlusturulmaTarihi = iskontoTime },
                    new ToptanciIskontoOrani { ToptanciUrunGrubuId = gida.Id, MinAdet = 20, IskontoYuzdesi = 7, AktifMi = true, OlusturulmaTarihi = iskontoTime },
                    new ToptanciIskontoOrani { ToptanciUrunGrubuId = gida.Id, MinAdet = 100, IskontoYuzdesi = 12, AktifMi = true, OlusturulmaTarihi = iskontoTime }
                };
                await context.ToptanciIskontoOranlari.AddRangeAsync(iskontolar);
                await context.SaveChangesAsync();
            }
        }
    }
}

