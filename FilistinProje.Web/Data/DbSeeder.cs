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
                ("Gazze Şeridi", "Filistin", "Gazze - Han Yunus - Refah - Kuzey Gazze - Deyr el-Balah", 5),
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
            var sehirler = new (string SehirAdi, string SehirAdiEn, string SehirAdiAr, string BolgeAdi)[]
            {
                ("Hayfa", "Haifa", "حيفا", "48 İç Bölge (Kuzey)"),
                ("Nasıra", "Nazareth", "الناصرة", "48 İç Bölge (Kuzey)"),
                ("Akka", "Acre", "عكا", "48 İç Bölge (Kuzey)"),
                ("Ümmü'l-Fahm", "Umm al-Fahm", "أم الفحم", "48 İç Bölge (Kuzey)"),
                ("Yafa", "Jaffa", "يافا", "48 İç Bölge (Merkez)"),
                ("Lydda", "Lydda", "اللد", "48 İç Bölge (Merkez)"),
                ("Ramla", "Ramla", "الرملة", "48 İç Bölge (Merkez)"),
                ("Taybe", "Tayibe", "الطيبة", "48 İç Bölge (Merkez)"),
                ("Cenin", "Jenin", "جنين", "Batı Şeria (Kuzey / Merkez)"),
                ("Nablus", "Nablus", "نابلس", "Batı Şeria (Kuzey / Merkez)"),
                ("Ramallah ve El-Bireh", "Ramallah and al-Bireh", "رام الله والبيرة", "Batı Şeria (Kuzey / Merkez)"),
                ("El-Halil", "Hebron", "الخليل", "Batı Şeria (Kuzey / Merkez)"),
                ("Beytüllahim", "Bethlehem", "بيت لحم", "Batı Şeria (Kuzey / Merkez)"),
                ("Salfit", "Salfit", "سلفيت", "Batı Şeria (Kuzey / Merkez)"),
                ("Tubas", "Tubas", "طوباس", "Batı Şeria (Kuzey / Merkez)"),
                ("Tulkarim", "Tulkarm", "طولكرم", "Batı Şeria (Kuzey / Merkez)"),
                ("Kalkilya", "Qalqilya", "قلقيلية", "Batı Şeria (Kuzey / Merkez)"),
                ("Eriha", "Jericho", "أريحا", "Batı Şeria (Kuzey / Merkez)"),
                ("El-Kudüs", "Jerusalem", "القدس", "Kudüs"),
                ("Gazze", "Gaza", "غزة", "Gazze Şeridi"),
                ("Han Yunus", "Khan Yunis", "خان يونس", "Gazze Şeridi"),
                ("Refah", "Rafah", "رفح", "Gazze Şeridi"),
                ("Kuzey Gazze", "North Gaza", "شمال غزة", "Gazze Şeridi"),
                ("Deyr el-Balah", "Deir al-Balah", "دير البلح", "Gazze Şeridi"),
            };

            var bolgeler = await db.KargoBolgeler.IgnoreQueryFilters().Where(x => !x.SilindiMi).ToListAsync();
            var mevcutSehirler = await db.KargoBolgeSehirler.IgnoreQueryFilters().ToListAsync();

            foreach (var (sehirAdi, sehirAdiEn, sehirAdiAr, bolgeAdi) in sehirler)
            {
                var bolge = bolgeler.FirstOrDefault(x => x.Ad == bolgeAdi);
                if (bolge == null) continue;

                var mevcutSehir = mevcutSehirler.FirstOrDefault(x =>
                    x.BolgeId == bolge.Id &&
                    (x.SehirAdi == sehirAdi || x.SehirAdiEn == sehirAdiEn));
                if (mevcutSehir != null)
                {
                    mevcutSehir.SehirAdi = sehirAdi;
                    mevcutSehir.SehirAdiEn = sehirAdiEn;
                    mevcutSehir.SehirAdiAr = sehirAdiAr;
                    mevcutSehir.SilindiMi = false;
                }
                else
                {
                    db.KargoBolgeSehirler.Add(new KargoBolgeSehir
                    {
                        BolgeId = bolge.Id,
                        SehirAdi = sehirAdi,
                        SehirAdiEn = sehirAdiEn,
                        SehirAdiAr = sehirAdiAr,
                        OlusturulmaTarihi = DateTime.UtcNow,
                        SilindiMi = false
                    });
                }
            }

            // Gazze şehirleri ayrı bölgeye eklendi.

            await db.SaveChangesAsync();
            logger.LogInformation("[Seed] {N} sehir {R} bolge icin dogrulandi.", sehirler.Length, bolgeAdlari.Length);
        }
    }
}
