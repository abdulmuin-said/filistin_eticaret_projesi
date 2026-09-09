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
                            AdSoyad = "مدير 7ANRPS48",
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
            await SeedKurumsalSayfalarAsync(dbContext, logger);
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
                    GondericiAdres = "Ramallah, Palestine",
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

            // Kargo bölgeleri (48 Bölge – 3 bölge: İç/Kuzey/Merkez, Batı Şeria alt bölgeleri, Kudüs, Gazze)
            var bolgeAdlari = new (string Ad, string? Ulke, string? Aciklama, int Sira)[]
            {
                ("المناطق الداخلية 48 (شمال)", "Palestine", "حيفا - الناصرة - عكا - أم الفحم", 1),
                ("المناطق الداخلية 48 (وسط)", "Palestine", "يافا - اللد - الرملة - الطيبة", 2),
                ("الضفة الغربية (شمال / وسط)", "Palestine", "جنين - نابلس - رام الله - الخليل - بيت لحم - سلفيت - طوباس - طولكرم - قلقيلية - أريحا", 3),
                ("القدس", "Palestine", "القدس وضواحيها", 4),
                ("قطاع غزة", "Palestine", "غزة - خان يونس - رفح - شمال غزة - دير البلح", 5),
            };

            var existingBolgeler = await db.KargoBolgeler.IgnoreQueryFilters().ToListAsync();
            foreach (var (ad, ulke, aciklama, sira) in bolgeAdlari)
            {
                var mevcut = existingBolgeler.FirstOrDefault(x => x.Sira == sira || x.Ad == ad);
                if (mevcut == null)
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
                    mevcut.Ad = ad;
                    mevcut.Ulke = ulke;
                    mevcut.Aciklama = aciklama;
                    mevcut.Sira = sira;
                    mevcut.SilindiMi = false;
                }
            }
            await db.SaveChangesAsync();

            // Şehirler
            var sehirler = new (string SehirAdi, string SehirAdiEn, string SehirAdiAr, string BolgeAdi)[]
            {
                ("حيفا", "Haifa", "حيفا", "المناطق الداخلية 48 (شمال)"),
                ("الناصرة", "Nazareth", "الناصرة", "المناطق الداخلية 48 (شمال)"),
                ("عكا", "Acre", "عكا", "المناطق الداخلية 48 (شمال)"),
                ("أم الفحم", "Umm al-Fahm", "أم الفحم", "المناطق الداخلية 48 (شمال)"),
                ("يافا", "Jaffa", "يافا", "المناطق الداخلية 48 (وسط)"),
                ("اللد", "Lydda", "اللد", "المناطق الداخلية 48 (وسط)"),
                ("الرملة", "Ramla", "الرملة", "المناطق الداخلية 48 (وسط)"),
                ("الطيبة", "Tayibe", "الطيبة", "المناطق الداخلية 48 (وسط)"),
                ("جنين", "Jenin", "جنين", "الضفة الغربية (شمال / وسط)"),
                ("نابلس", "Nablus", "نابلس", "الضفة الغربية (شمال / وسط)"),
                ("رام الله والبيرة", "Ramallah and al-Bireh", "رام الله والبيرة", "الضفة الغربية (شمال / وسط)"),
                ("الخليل", "Hebron", "الخليل", "الضفة الغربية (شمال / وسط)"),
                ("بيت لحم", "Bethlehem", "بيت لحم", "الضفة الغربية (شمال / وسط)"),
                ("سلفيت", "Salfit", "سلفيت", "الضفة الغربية (شمال / وسط)"),
                ("طوباس", "Tubas", "طوباس", "الضفة الغربية (شمال / وسط)"),
                ("طولكرم", "Tulkarm", "طولكرم", "الضفة الغربية (شمال / وسط)"),
                ("قلقيلية", "Qalqilya", "قلقيلية", "الضفة الغربية (شمال / وسط)"),
                ("أريحا", "Jericho", "أريحا", "الضفة الغربية (شمال / وسط)"),
                ("القدس", "Jerusalem", "القدس", "القدس"),
                ("غزة", "Gaza", "غزة", "قطاع غزة"),
                ("خان يونس", "Khan Yunis", "خان يونس", "قطاع غزة"),
                ("رفح", "Rafah", "رفح", "قطاع غزة"),
                ("شمال غزة", "North Gaza", "شمال غزة", "قطاع غزة"),
                ("دير البلح", "Deir al-Balah", "دير البلح", "قطاع غزة"),
            };

            var bolgeler = await db.KargoBolgeler.IgnoreQueryFilters().Where(x => !x.SilindiMi).ToListAsync();
            var mevcutSehirler = await db.KargoBolgeSehirler.IgnoreQueryFilters().ToListAsync();

            foreach (var (sehirAdi, sehirAdiEn, sehirAdiAr, bolgeAdi) in sehirler)
            {
                var bolge = bolgeler.FirstOrDefault(x => x.Ad == bolgeAdi);
                if (bolge == null) continue;

                var mevcutSehir = mevcutSehirler.FirstOrDefault(x =>
                    x.BolgeId == bolge.Id &&
                    (x.SehirAdiEn == sehirAdiEn || x.SehirAdiAr == sehirAdiAr || x.SehirAdi == sehirAdi));
                if (mevcutSehir != null)
                {
                    mevcutSehir.SehirAdi = sehirAdi;
                    mevcutSehir.SehirAdiEn = sehirAdiEn;
                    mevcutSehir.SehirAdiAr = sehirAdiAr;
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

        private static async Task SeedKurumsalSayfalarAsync(KanvasDbContext db, Microsoft.Extensions.Logging.ILogger logger)
        {
            var anySayfa = await db.KurumsalSayfalar.AnyAsync();
            if (anySayfa) return;

            var sayfalar = new List<KurumsalSayfa>
            {
                new()
                {
                    UrlSlug = "hakkimizda",
                    Sira = 1,
                    Baslik = "من نحن",
                    BaslikAr = "من نحن",
                    BaslikEn = "About Us",
                    Icerik = "<p>نصلكم بأجود المنتجات الفلسطينية والعالمية المختارة بعناية فائقة، ملتزمين بأعلى معايير الجودة والأصالة.</p>",
                    IcerikAr = "<p>نصلكم بأجود المنتجات الفلسطينية والعالمية المختارة بعناية فائقة، ملتزمين بأعلى معايير الجودة والأصالة.</p>",
                    IcerikEn = "<p>Connecting you with premier products chosen with utmost care, committed to high quality and authenticity.</p>",
                    OlusturulmaTarihi = DateTime.UtcNow,
                    SilindiMi = false
                },
                new()
                {
                    UrlSlug = "gizlilik",
                    Sira = 2,
                    Baslik = "سياسة الخصوصية والأمان",
                    BaslikAr = "سياسة الخصوصية والأمان",
                    BaslikEn = "Privacy and Security Policy",
                    Icerik = "<p>نولي في منصة 7ANRPS48 أقصى درجات الاهتمام لحماية خصوصية بياناتكم وأمان معاملاتكم الإلكترونية.</p>",
                    IcerikAr = "<p>نولي في منصة 7ANRPS48 أقصى درجات الاهتمام لحماية خصوصية بياناتكم وأمان معاملاتكم الإلكترونية.</p>",
                    IcerikEn = "<p>At 7ANRPS48, we place paramount importance on safeguarding your privacy and ensuring the security of your transactions.</p>",
                    OlusturulmaTarihi = DateTime.UtcNow,
                    SilindiMi = false
                },
                new()
                {
                    UrlSlug = "kullanici-sozlesmesi",
                    Sira = 3,
                    Baslik = "اتفاقية المستخدم والشروط",
                    BaslikAr = "اتفاقية المستخدم والشروط",
                    BaslikEn = "Terms of Service",
                    Icerik = "<p>مرحباً بكم في منصة 7ANRPS48. استخدامكم لهذا الموقع وإتمام أي عملية شراء يعد موافقة صريحة على الالتزام بالشروط والأحكام.</p>",
                    IcerikAr = "<p>مرحباً بكم في منصة 7ANRPS48. استخدامكم لهذا الموقع وإتمام أي عملية شراء يعد موافقة صريحة على الالتزام بالشروط والأحكام.</p>",
                    IcerikEn = "<p>Welcome to 7ANRPS48. Accessing our website and placing orders constitutes your acceptance of these terms and conditions.</p>",
                    OlusturulmaTarihi = DateTime.UtcNow,
                    SilindiMi = false
                },
                new()
                {
                    UrlSlug = "mesafeli-satis",
                    Sira = 4,
                    Baslik = "عقد البيع عن بُعد",
                    BaslikAr = "عقد البيع عن بُعد",
                    BaslikEn = "Distance Selling Agreement",
                    Icerik = "<p>يوضّح هذا العقد شروط البيع عن بُعد بين البائع (7ANRPS48) والمشتري للطلبات المُقدَّمة عبر الموقع الإلكتروني.</p>",
                    IcerikAr = "<p>يوضّح هذا العقد شروط البيع عن بُعد بين البائع (7ANRPS48) والمشتري للطلبات المُقدَّمة عبر الموقع الإلكتروني.</p>",
                    IcerikEn = "<p>This agreement outlines the distance selling terms between the seller (7ANRPS48) and the buyer.</p>",
                    OlusturulmaTarihi = DateTime.UtcNow,
                    SilindiMi = false
                },
                new()
                {
                    UrlSlug = "iade-kosullari",
                    Sira = 5,
                    Baslik = "سياسة الشحن والإرجاع والاستبدال",
                    BaslikAr = "سياسة الشحن والإرجاع والاستبدال",
                    BaslikEn = "Shipping & Return Policy",
                    Icerik = "<p>نحرص في 7ANRPS48 على رضاكم التام عن كل عملية شراء. توضح هذه السياسة شروط ومواعيد الشحن، بالإضافة إلى إجراءات الإرجاع.</p>",
                    IcerikAr = "<p>نحرص في 7ANRPS48 على رضاكم التام عن كل عملية شراء. توضح هذه السياسة شروط ومواعيد الشحن، بالإضافة إلى إجراءات الإرجاع.</p>",
                    IcerikEn = "<p>At 7ANRPS48, customer satisfaction is our prime commitment. This policy outlines shipping timelines and returns.</p>",
                    OlusturulmaTarihi = DateTime.UtcNow,
                    SilindiMi = false
                },
                new()
                {
                    UrlSlug = "sss",
                    Sira = 6,
                    Baslik = "الأسئلة الشائعة",
                    BaslikAr = "الأسئلة الشائعة",
                    BaslikEn = "Frequently Asked Questions",
                    Icerik = "<p>إليك إجابات لأكثر الأسئلة تكراراً حول التسوق والطلب والتوصيل في متجر 7ANRPS48.</p>",
                    IcerikAr = "<p>إليك إجابات لأكثر الأسئلة تكراراً حول التسوق والطلب والتوصيل في متجر 7ANRPS48.</p>",
                    IcerikEn = "<p>Here are answers to the most common questions regarding shopping, ordering, and delivery at 7ANRPS48.</p>",
                    OlusturulmaTarihi = DateTime.UtcNow,
                    SilindiMi = false
                }
            };

            db.KurumsalSayfalar.AddRange(sayfalar);
            await db.SaveChangesAsync();
            logger.LogInformation("[Seed] 6 kurumsal sayfa basariyla eklendi.");
        }
    }
}
