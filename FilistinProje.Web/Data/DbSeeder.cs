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

            // NOT: Tüm içerik (kategori, ürün, slayt, kargo, banka, toptancı)
            // admin panelden manuel eklenmektedir. Seed verisi eklenmez.
            // Otomatik seed işlemi: 2026-06-26 itibarıyla devre dışı.
        }
    }
}
