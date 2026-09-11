using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FilistinProje.Data;
using FilistinProje.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace FilistinProje.Service.Services
{
    /// <summary>
    /// Favori ürünlerde fiyat düşüşü olduğunda kullanıcılara e-posta bildirimi gönderen arka plan servisi.
    /// Her 6 saatte bir çalışır.
    /// </summary>
    public class FavoriPriceDropService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<FavoriPriceDropService> _logger;
        private readonly IConfiguration _config;

        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(6);
        // Aynı ürün için en az 24 saat aralıkla bildirim gönder
        private readonly TimeSpan _minNotificationInterval = TimeSpan.FromHours(24);

        public FavoriPriceDropService(
            IServiceScopeFactory scopeFactory,
            ILogger<FavoriPriceDropService> logger,
            IConfiguration config)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Favorite Price Drop Notification Service starting.");

            // Delay 2 minutes on first run to ensure app startup completes
            try
            {
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Checking for favorite price drops...");

                if (!IsSmtpConfigured())
                {
                    _logger.LogWarning("SMTP settings not configured, skipping favorite price drop notification check.");
                    try
                    {
                        await Task.Delay(_checkInterval, stoppingToken);
                    }
                    catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                    {
                        break;
                    }
                    continue;
                }

                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<KanvasDbContext>();
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    var siteSettingsService = scope.ServiceProvider.GetRequiredService<ISiteSettingsService>();
                    var siteSettings = siteSettingsService.GetSettings();
                    var currencySymbol = string.IsNullOrWhiteSpace(siteSettings.ParaBirimi) ? "₪" : siteSettings.ParaBirimi;

                    // Fetch all favorites with price drop alerts enabled
                    var favoriler = await context.Favoriler
                        .Include(f => f.Urun)
                        .Include(f => f.AppUser)
                        .Where(f => f.FiyatDustugundaBildir
                                    && !f.SilindiMi
                                    && f.EskiFiyat.HasValue)
                        .ToListAsync(stoppingToken);

                    if (!favoriler.Any())
                    {
                        _logger.LogInformation("No favorites with price alert enabled found.");
                    }
                    else
                    {
                        _logger.LogInformation("Found {Count} favorites with price drop tracking.", favoriler.Count);

                        var bildirimSayisi = 0;

                        foreach (var favori in favoriler)
                        {
                            if (favori.Urun == null || favori.AppUser == null) continue;
                            if (string.IsNullOrWhiteSpace(favori.AppUser.Email)) continue;
                            if (!IsValidEmail(favori.AppUser.Email)) continue;

                            // Son bildirimden bu yana yeterli süre geçmiş mi?
                            if (favori.SonBildirimTarihi.HasValue &&
                                DateTime.UtcNow - favori.SonBildirimTarihi.Value < _minNotificationInterval)
                            {
                                continue;
                            }

                            var mevcutFiyat = favori.Urun.EtkinFiyat;
                            var eskiFiyat = favori.EskiFiyat!.Value;

                            // Fiyat düştü mü?
                            if (mevcutFiyat < eskiFiyat)
                            {
                                var indirimOrani = Math.Round((1 - mevcutFiyat / eskiFiyat) * 100, 0);
                                var indirimMiktari = eskiFiyat - mevcutFiyat;

                                try
                                {
                                    var urunUrl = siteSettingsService.BuildAbsoluteUrl(
                                        $"/Urun/Detay/{favori.Urun.Slug}-{favori.Urun.Id}");

                                    var subject = $"Favorinizdeki ürünün fiyatı düştü! 🎉";
                                    var body = $@"
                                        <p>Favori listenize eklediğiniz bir ürünün fiyatı düştü!</p>
                                        <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='border:1px solid #e5e2dc; border-radius:12px; background:#fffaf0; margin:18px 0;'>
                                            <tr>
                                                <td style='padding:16px; border-bottom:1px solid #e5e2dc;'>
                                                    <strong style='color:#313511; font-size:16px;'>{System.Net.WebUtility.HtmlEncode(favori.Urun.Baslik)}</strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style='padding:16px;'>
                                                    <div style='display:flex; gap:24px; align-items:center;'>
                                                        <div>
                                                            <span style='color:#999; text-decoration:line-through; font-size:14px;'>{eskiFiyat:N2} {currencySymbol}</span>
                                                            <br/>
                                                            <span style='color:#b58735; font-size:24px; font-weight:700;'>{mevcutFiyat:N2} {currencySymbol}</span>
                                                        </div>
                                                        <div style='background:#27ae60; color:white; padding:6px 14px; border-radius:999px; font-size:14px; font-weight:700;'>
                                                            %{indirimOrani} indirim
                                                        </div>
                                                    </div>
                                                    <p style='margin-top:8px; color:#47473d; font-size:14px;'>
                                                        <strong>{indirimMiktari:N2} {currencySymbol}</strong> tasarruf edin!
                                                    </p>
                                                </td>
                                            </tr>
                                        </table>
                                        <p>Bu fırsatı kaçırmamak için hemen sipariş verin!</p>";

                                    await emailService.SendTemplateMailAsync(
                                        favori.AppUser.Email,
                                        subject,
                                        favori.AppUser.AdSoyad,
                                        body,
                                        urunUrl,
                                        "Ürünü İncele"
                                    );

                                    // Bildirim bilgilerini güncelle
                                    favori.SonBildirimTarihi = DateTime.UtcNow;
                                    favori.EskiFiyat = mevcutFiyat; // Yeni fiyatı kaydet ki bir sonraki düşüşte tekrar bildirim gelsin

                                    bildirimSayisi++;
                                    _logger.LogInformation(
                                        "Price drop notification sent. User={Email}, Product={UrunId}, OldPrice={EskiFiyat}, NewPrice={YeniFiyat}",
                                        favori.AppUser.Email, favori.UrunId, eskiFiyat, mevcutFiyat);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex, "Failed to send price drop notification. FavoriteId={FavoriId}", favori.Id);
                                }
                            }
                            else if (mevcutFiyat != eskiFiyat)
                            {
                                // If price went up, update EskiFiyat so next drop is accurately compared
                                favori.EskiFiyat = mevcutFiyat;
                            }
                        }

                        await context.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation("Price drop check completed. {Count} notifications sent.", bildirimSayisi);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in favorite price drop service.");
                }

                // Bir sonraki kontrol için bekle
                try
                {
                    await Task.Delay(_checkInterval, stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        private bool IsSmtpConfigured()
        {
            if (!_config.GetValue<bool>("EmailSettings:Enabled"))
            {
                return false;
            }

            var username = _config["EmailSettings:Username"];
            var password = _config["EmailSettings:Password"];
            return IsValidEmail(username) && !string.IsNullOrWhiteSpace(password);
        }

        private static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
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
