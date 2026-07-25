using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FilistinProje.Tests
{
    public sealed class PostgreSqlIntegrationTests
    {
        private const string TestConnectionString = "Host=localhost;Port=5434;Database=filistindb;Username=kanvasuser;Password=changeme_in_production";

        private static KanvasDbContext? TryCreatePostgresContext()
        {
            try
            {
                var options = new DbContextOptionsBuilder<KanvasDbContext>()
                    .UseNpgsql(TestConnectionString)
                    .Options;

                var context = new KanvasDbContext(options);
                if (context.Database.CanConnect())
                {
                    return context;
                }
            }
            catch
            {
                // PostgreSQL container is not running or unreachable
            }

            return null;
        }

        [Fact]
        public async Task AtomicCoupon_ConcurrentTransactions_ExactlyOneSucceedsOnPostgreSQL()
        {
            using var dbCheck = TryCreatePostgresContext();
            if (dbCheck == null)
            {
                // Skip gracefully if PostgreSQL container is unreachable
                return;
            }

            var couponCode = "PG_TEST_" + Guid.NewGuid().ToString("N")[..8];
            var now = DateTime.UtcNow;

            // Setup test coupon in PostgreSQL
            var testCoupon = new Kupon
            {
                Kod = couponCode,
                Tip = 0,
                Deger = 10m,
                MinSepetTutari = 0m,
                KullanimLimiti = 1,
                KullanilanMiktar = 0,
                BaslangicTarihi = now.AddMinutes(-5),
                SonKullanmaTarihi = now.AddHours(1),
                AktifMi = true,
                SilindiMi = false
            };

            dbCheck.Kuponlar.Add(testCoupon);
            await dbCheck.SaveChangesAsync();

            try
            {
                // Run 2 parallel requests trying to claim the coupon atomically
                var task1 = Task.Run(async () =>
                {
                    using var db1 = TryCreatePostgresContext()!;
                    var currentTime = DateTime.UtcNow;
                    var affected1 = await db1.Kuponlar
                        .Where(x =>
                            x.Kod == couponCode &&
                            !x.SilindiMi &&
                            x.AktifMi &&
                            (!x.BaslangicTarihi.HasValue || x.BaslangicTarihi <= currentTime) &&
                            x.SonKullanmaTarihi > currentTime &&
                            (x.KullanimLimiti <= 0 || x.KullanilanMiktar < x.KullanimLimiti))
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(x => x.KullanilanMiktar, x => x.KullanilanMiktar + 1));

                    return affected1;
                });

                var task2 = Task.Run(async () =>
                {
                    using var db2 = TryCreatePostgresContext()!;
                    var currentTime = DateTime.UtcNow;
                    var affected2 = await db2.Kuponlar
                        .Where(x =>
                            x.Kod == couponCode &&
                            !x.SilindiMi &&
                            x.AktifMi &&
                            (!x.BaslangicTarihi.HasValue || x.BaslangicTarihi <= currentTime) &&
                            x.SonKullanmaTarihi > currentTime &&
                            (x.KullanimLimiti <= 0 || x.KullanilanMiktar < x.KullanimLimiti))
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(x => x.KullanilanMiktar, x => x.KullanilanMiktar + 1));

                    return affected2;
                });

                var results = await Task.WhenAll(task1, task2);
                var totalSuccessCount = results.Count(affected => affected == 1);
                var totalFailCount = results.Count(affected => affected == 0);

                // Assert that exactly 1 task succeeded and 1 task failed
                Assert.Equal(1, totalSuccessCount);
                Assert.Equal(1, totalFailCount);

                // Verify final state in DB
                using var dbVerify = TryCreatePostgresContext()!;
                var updatedCoupon = await dbVerify.Kuponlar.FirstAsync(x => x.Kod == couponCode);
                Assert.Equal(1, updatedCoupon.KullanilanMiktar);
            }
            finally
            {
                // Clean up test coupon
                using var dbCleanup = TryCreatePostgresContext()!;
                var couponToDelete = await dbCleanup.Kuponlar.FirstOrDefaultAsync(x => x.Kod == couponCode);
                if (couponToDelete != null)
                {
                    dbCleanup.Kuponlar.Remove(couponToDelete);
                    await dbCleanup.SaveChangesAsync();
                }
            }
        }
    }
}
