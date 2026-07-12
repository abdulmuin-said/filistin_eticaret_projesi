using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
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
            var env = serviceScope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            var loggerFactory = serviceScope.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("FilistinProje.Web.Data.DbSeeder");
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<KanvasDbContext>();

            var roleNames = AdminSecurityRoles.AllRoles
                .Distinct(StringComparer.OrdinalIgnoreCase);

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var seedDefaultAdmin = configuration.GetValue<bool>("AdminSettings:SeedDefaultAdmin");
            var adminEmail = configuration["AdminSettings:Email"];
            var adminPassword = configuration["AdminSettings:Password"];

            if (seedDefaultAdmin)
            {
                if (env.IsProduction())
                {
                    logger.LogWarning("AdminSettings:SeedDefaultAdmin=true, ancak uretim ortaminda sabit yonetici parolasi olusturulmasi reddedildi. Lutfen AdminSettings:Email ve AdminSettings:Password degerlerini guvenli sekilde (ortam degiskenleri / secrets store) temin edin.");
                }
                else if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
                {
                    logger.LogWarning("AdminSettings:SeedDefaultAdmin=true, ancak AdminSettings:Email veya AdminSettings:Password bos. Development'ta acilir sekilde uyari veriliyor; uretimde bu durum sessizce gecilecekti. Lutfen .NET User Secrets veya ortam degiskeni uzerinden tanimlayin.");
                }
                else
                {
                    logger.LogWarning("Development ortaminda AdminSettings:SeedDefaultAdmin aktif. Bu ozellik sadece gelistirme kolayligi icindir; uretime deploy edilirken SeedDefaultAdmin=false yapin veya ortam degiskeni ile override edin.");

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
                            logger.LogWarning("Seed admin kullanicisi olusturulamadi. ASP.NET Identity hatalarini kontrol edin.");
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
            }

            foreach (var user in userManager.Users.ToList())
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Count == 0)
                {
                    await userManager.AddToRoleAsync(user, AdminSecurityRoles.Uye);
                }
            }

            await SeedKargoVeBankaAsync(dbContext, logger);
        }

        private static async Task SeedKargoVeBankaAsync(KanvasDbContext db, Microsoft.Extensions.Logging.ILogger logger)
        {
            var anyFirma = await db.KargoFirmalari.IgnoreQueryFilters().AnyAsync(x => !x.SilindiMi);
            if (!anyFirma)
            {
                db.KargoFirmalari.Add(new KargoFirmasi
                {
                    Ad = "United Express",
                    Kod = "united-express",
                    Telefon = "+970 000 000 000",
                    TakipUrl = "https://tracking.unitedexpress.ps/?track=",
                    GondericiUnvan = "7ANRPS48",
                    GondericiAdres = "Ramallah, Filistin",
                    GondericiTelefon = "+970 000 000 000",
                    AktifMi = true,
                    VarsayilanMi = true,
                    Fiyat = 0,
                    OlusturulmaTarihi = DateTime.UtcNow,
                    SilindiMi = false
                });
                await db.SaveChangesAsync();
                logger.LogInformation("[Seed] United Express kargo firmasi eklendi (takip URL ve fiyat gercek değil, admin duzeltmeli).");
            }

            // Kargo bölgeleri (48 Bölge – 3 bölge: İç/Kuzey/Merkez, Batı Şeria alt bölgeleri, Kudüs)
            var bolgeAdlari = new (string Ad, string? Ulke, string? Aciklama, int Sira)[]
            {
                ("48 İç Bölge (Kuzey)", "Filistin", "Hayfa - Nasıra - Akka - Ümmü'l-Fahm (48 kuzey)", 1),
                ("48 İç Bölge (Merkez)", "Filistin", "Yafa - Lydda - Ramla - Taybe (48 merkez)", 2),
                ("Batı Şeria (Kuzey / Merkez)", "Filistin", "Cenin - Nablus - Ramallah - El-Halil - Beytüllahim - Salfit - Tubas - Tulkarim - Kalkilya - Eriha", 3),
                ("Kudüs", "Filistin", "El-Kudüs (ayrı bölge)", 4),
            };

            var bolgeLookup = await db.KargoBolgeler.IgnoreQueryFilters().ToDictionaryAsync(x => x.Ad, x => x);
            foreach (var (ad, ulke, aciklama, sira) in bolgeAdlari)
            {
                if (!bolgeLookup.ContainsKey(ad))
                {
                    db.KargoBolgeler.Add(new KargoBolge
                    {
                        Ad = ad,
                        Ulke = ulke,
                        Aciklama = aciklama,
                        Sira = sira,
                        Fiyat = 0,
                        OlusturulmaTarihi = DateTime.UtcNow,
                        SilindiMi = false
                    });
                }
                else
                {
                    var mevcut = bolgeLookup[ad];
                    mevcut.Ulke ??= ulke;
                    mevcut.Aciklama ??= aciklama;
                    mevcut.Sira = sira;
                    mevcut.SilindiMi = false;
                }
            }
            await db.SaveChangesAsync();

            // Şehirler
            var sehirBolgeMap = new Dictionary<string, string>
            {
                ["Hayfa (Haifa)"] = "48 İç Bölge (Kuzey)",
                ["Nasıra (Nazareth)"] = "48 İç Bölge (Kuzey)",
                ["Akka (Acre)"] = "48 İç Bölge (Kuzey)",
                ["Ümmü'l-Fahm"] = "48 İç Bölge (Kuzey)",
                ["Yafa (Jaffa)"] = "48 İç Bölge (Merkez)",
                ["Lydda"] = "48 İç Bölge (Merkez)",
                ["Ramla"] = "48 İç Bölge (Merkez)",
                ["Taybe"] = "48 İç Bölge (Merkez)",
                ["Cenin (Jenin)"] = "Batı Şeria (Kuzey / Merkez)",
                ["Nablus"] = "Batı Şeria (Kuzey / Merkez)",
                ["Ramallah & El-Bireh"] = "Batı Şeria (Kuzey / Merkez)",
                ["El-Halil (Hebron)"] = "Batı Şeria (Kuzey / Merkez)",
                ["Beytüllahim (Bethlehem)"] = "Batı Şeria (Kuzey / Merkez)",
                ["Salfit"] = "Batı Şeria (Kuzey / Merkez)",
                ["Tubas"] = "Batı Şeria (Kuzey / Merkez)",
                ["Tulkarim"] = "Batı Şeria (Kuzey / Merkez)",
                ["Kalkilya (Qalqilya)"] = "Batı Şeria (Kuzey / Merkez)",
                ["Eriha (Jericho)"] = "Batı Şeria (Kuzey / Merkez)",
                ["El-Kudüs (Jerusalem)"] = "Kudüs",
            };

            var sehirArMap = new Dictionary<string, string>
            {
                ["Hayfa (Haifa)"] = "حيفا",
                ["Nasıra (Nazareth)"] = "الناصرة",
                ["Akka (Acre)"] = "عكا",
                ["Ümmü'l-Fahm"] = "أم الفحم",
                ["Yafa (Jaffa)"] = "يافا",
                ["Lydda"] = "اللد",
                ["Ramla"] = "الرملة",
                ["Taybe"] = "الطيبة",
                ["Cenin (Jenin)"] = "جنين",
                ["Nablus"] = "نابلس",
                ["Ramallah & El-Bireh"] = "رام الله والبيرة",
                ["El-Halil (Hebron)"] = "الخليل",
                ["Beytüllahim (Bethlehem)"] = "بيت لحم",
                ["Salfit"] = "سلفيت",
                ["Tubas"] = "طوباس",
                ["Tulkarim"] = "طولكرم",
                ["Kalkilya (Qalqilya)"] = "قلقيلية",
                ["Eriha (Jericho)"] = "أريحا",
                ["El-Kudüs (Jerusalem)"] = "القدس",
            };

            var bolgeler = await db.KargoBolgeler.IgnoreQueryFilters().Where(x => !x.SilindiMi).ToListAsync();
            var sehirLookup = await db.KargoBolgeSehirler.IgnoreQueryFilters().Include(x => x.Bolge).ToDictionaryAsync(x => x.SehirAdi, x => x);

            foreach (var (sehirAdi, bolgeAdi) in sehirBolgeMap)
            {
                var bolge = bolgeler.FirstOrDefault(x => x.Ad == bolgeAdi);
                if (bolge == null) continue;

                var sehirAr = sehirArMap.TryGetValue(sehirAdi, out var ar) ? ar : null;

                if (sehirLookup.TryGetValue(sehirAdi, out var mevcutSehir))
                {
                    mevcutSehir.BolgeId = bolge.Id;
                    mevcutSehir.SilindiMi = false;
                    if (string.IsNullOrWhiteSpace(mevcutSehir.SehirAdiAr)) mevcutSehir.SehirAdiAr = sehirAr;
                }
                else
                {
                    db.KargoBolgeSehirler.Add(new KargoBolgeSehir
                    {
                        BolgeId = bolge.Id,
                        SehirAdi = sehirAdi,
                        SehirAdiEn = sehirAdi,
                        SehirAdiAr = sehirAr,
                        OlusturulmaTarihi = DateTime.UtcNow,
                        SilindiMi = false
                    });
                }
            }

            // Gazze şehirleri ayrı bölgeye eklenmedi - proje sahibi kararı bekleniyor.
            // ponytail: Gazze bölge+şehir eklemesi, proje sahibi "Gazze dahil" dediğinde yapılacak.
            await db.SaveChangesAsync();
            logger.LogInformation("[Seed] {N} sehir {R} bolgeye eklendi.", sehirBolgeMap.Count, bolgeAdlari.Length);
        }
    }
}