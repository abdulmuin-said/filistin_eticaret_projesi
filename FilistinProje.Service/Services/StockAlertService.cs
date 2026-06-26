using System.Net.Mail;
using FilistinProje.Core.Interfaces;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FilistinProje.Service.Services
{
    /// <summary>
    /// Periyodik olarak stok seviyelerini kontrol eder ve
    /// eşik altına düşen ürünler için admin e-posta uyarısı gönderir.
    /// Tekrarlı bildirimler 24 saat cooldown ile sınırlanır.
    /// </summary>
    public class StockAlertService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<StockAlertService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(30);
        private readonly TimeSpan _resendCooldown = TimeSpan.FromHours(24);

        public StockAlertService(
            IServiceScopeFactory scopeFactory,
            ILogger<StockAlertService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("StockAlertService başlatıldı. Kontrol aralığı: {Interval}", _checkInterval);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(_checkInterval, stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }

                await CheckStockAndNotifyAsync(stoppingToken);
            }

            _logger.LogInformation("StockAlertService durduruldu.");
        }

        private async Task CheckStockAndNotifyAsync(CancellationToken ct)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<KanvasDbContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var siteSettingsService = scope.ServiceProvider.GetRequiredService<ISiteSettingsService>();

                var settings = siteSettingsService.GetSettings();

                // Stok uyarıları kapalıysa veya bildirim alıcısı yoksa atla
                if (!settings.StokUyarisiMailBildirimi)
                {
                    return;
                }

                var recipientEmail = settings.BildirimAliciEmail;
                if (string.IsNullOrWhiteSpace(recipientEmail) || !IsValidEmail(recipientEmail))
                {
                    _logger.LogWarning("Stok uyarisi icin gecerli bir alici e-postasi tanimlanmamis.");
                    return;
                }

                var limit = settings.StokUyariLimiti;
                var cooldownThreshold = DateTime.UtcNow.Subtract(_resendCooldown);

                // 1) Stok uyarı limitinin altındaki varyantları bul
                //    (varyant stok adedi <= limit, aktif ve silinmemiş ürünler)
                var dusukStokVaryantlari = await context.UrunSecenekleri
                    .Include(v => v.Urun)
                    .Where(v => !v.SilindiMi
                             && v.AktifMi
                             && v.StokAdedi <= limit
                             && v.Urun != null
                             && !v.Urun.SilindiMi
                             && v.Urun.AktifMi)
                    .ToListAsync(ct);

                // 2) Her varyant için daha önce bildirim gönderilmiş mi kontrol et
                var uyarilacakVaryantlar = new List<(UrunSecenek Varyant, string Tip)>();

                foreach (var varyant in dusukStokVaryantlari)
                {
                    var tip = varyant.StokAdedi <= 0 ? "Tukendi" : "DusukStok";

                    var sonBildirim = await context.StokBildirimLoglari
                        .Where(l => l.UrunId == varyant.UrunId
                                 && l.UrunSecenekId == varyant.Id
                                 && l.BildirimTipi == tip
                                 && l.GonderildiMi)
                        .OrderByDescending(l => l.OlusturulmaTarihi)
                        .FirstOrDefaultAsync(ct);

                    // Cooldown süresi içinde bildirim gönderilmişse atla
                    if (sonBildirim != null && sonBildirim.OlusturulmaTarihi >= cooldownThreshold)
                    {
                        _logger.LogDebug(
                            "Atlanan bildirim: UrunId={UrunId}, VaryantId={VaryantId}, Tip={Tip}, SonBildirim={Son}",
                            varyant.UrunId, varyant.Id, tip, sonBildirim.OlusturulmaTarihi);
                        continue;
                    }

                    uyarilacakVaryantlar.Add((varyant, tip));
                }

                if (uyarilacakVaryantlar.Count == 0)
                {
                    _logger.LogDebug("Stok kontrolü tamam: bildirim gereken ürün yok.");
                    return;
                }

                // 3) Admin e-postası oluştur ve gönder
                var brandName = string.IsNullOrWhiteSpace(settings.MarkaAdi) ? settings.SiteAdi : settings.MarkaAdi;
                var siteUrl = siteSettingsService.BuildAbsoluteUrl("/Admin/Urun");

                // HTML liste oluştur
                var productRows = string.Concat(uyarilacakVaryantlar.Select(item =>
                {
                    var v = item.Varyant;
                    var tipLabel = item.Tip == "Tukendi"
                        ? "<span style=\"color:#dc2626;font-weight:700;\">TÜKENDİ</span>"
                        : $"<span style=\"color:#b58735;font-weight:600;\">DÜŞÜK STOK ({v.StokAdedi} adet)</span>";
                    var urunAdi = System.Net.WebUtility.HtmlEncode(v.Urun?.Baslik ?? "Bilinmeyen Ürün");
                    var varyantOzet = System.Net.WebUtility.HtmlEncode(string.IsNullOrWhiteSpace(v.Olcu) ? "-" : v.Olcu);
                    var urunLink = siteSettingsService.BuildAbsoluteUrl($"/Admin/Urun/Duzenle/{v.UrunId}");

                    return $@"
                    <tr style=""border-bottom:1px solid #e5e2dc;"">
                        <td style=""padding:12px 8px;""><a href=""{urunLink}"" style=""color:#313511;text-decoration:none;font-weight:600;"">{urunAdi}</a></td>
                        <td style=""padding:12px 8px;text-align:center;"">{varyantOzet}</td>
                        <td style=""padding:12px 8px;text-align:center;"">{tipLabel}</td>
                    </tr>";
                }));

                var subject = $"[{brandName}] Stok Uyarısı — {uyarilacakVaryantlar.Count} ürün";
                var body = $@"
                    <p>Aşağıdaki ürünlerde stok kritik seviyededir:</p>
                    <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='border:1px solid #e5e2dc; border-radius:12px; background:#fffaf0; margin:18px 0;'>
                        <thead>
                            <tr style=""background:#313511;color:#fff;"">
                                <th style=""padding:12px 8px;text-align:left;border-radius:12px 0 0 0;"">Ürün</th>
                                <th style=""padding:12px 8px;text-align:center;"">Varyant</th>
                                <th style=""padding:12px 8px;text-align:center;border-radius:0 12px 0 0;"">Durum</th>
                            </tr>
                        </thead>
                        <tbody>
                            {productRows}
                        </tbody>
                    </table>
                    <p style=""text-align:center;margin-top:20px;"">
                        <a href=""{siteUrl}"" style=""display:inline-block;background:#313511;color:#fff;padding:13px 24px;text-decoration:none;border-radius:999px;font-size:13px;font-weight:700;"">
                            Ürün Yönetimine Git
                        </a>
                    </p>";

                await emailService.SendTemplateMailAsync(
                    recipientEmail,
                    subject,
                    brandName,
                    body,
                    siteUrl,
                    "Ürün Yönetimine Git");

                // 4) Bildirim log'larını kaydet
                var loglar = uyarilacakVaryantlar.Select(item => new StokBildirimLog
                {
                    UrunId = item.Varyant.UrunId,
                    UrunSecenekId = item.Varyant.Id,
                    KalanStok = item.Varyant.StokAdedi,
                    StokEsigi = limit,
                    BildirimTipi = item.Tip,
                    GonderildiMi = true,
                    OlusturulmaTarihi = DateTime.UtcNow
                }).ToList();

                context.StokBildirimLoglari.AddRange(loglar);
                await context.SaveChangesAsync(ct);

                _logger.LogInformation(
                    "Stok uyarisi gonderildi. {Count} urun icin bildirim yapildi. Alici={Email}",
                    uyarilacakVaryantlar.Count, recipientEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "StockAlertService stok kontrolu sirasinda hata.");
            }
        }

        private static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            try
            {
                _ = new MailAddress(email.Trim());
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
