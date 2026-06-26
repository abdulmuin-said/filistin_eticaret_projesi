using FilistinProje.Data;
using FilistinProje.Core.Varliklar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace FilistinProje.Service.Services
{
    public interface IFirebaseNotificationService
    {
        /// <summary>Push bildirimi gönder (tek kullanıcı/token)</summary>
        Task<bool> SendPushAsync(string token, string title, string body, string? clickUrl = null);

        /// <summary>Tüm abonelere push bildirimi gönder (kampanya)</summary>
        Task<int> SendBulkPushAsync(string title, string body, string? clickUrl = null);

        /// <summary>Token kaydet / güncelle</summary>
        Task SubscribeAsync(string token, string? appUserId, string? tarayici, string? platform);

        /// <summary>Token abonelikten çıkar</summary>
        Task UnsubscribeAsync(string token);

        /// <summary>Aktif token sayısı</summary>
        Task<int> GetActiveSubscriptionCountAsync();

        /// <summary>Servis kullanılabilir mi? (Firebase yapılandırılmış mı)</summary>
        bool IsAvailable { get; }
    }

    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        private readonly KanvasDbContext _db;
        private readonly IConfiguration _config;
        private readonly ILogger<FirebaseNotificationService> _logger;
        private readonly HttpClient _httpClient;
        private readonly bool _isConfigured;

        public bool IsAvailable => _isConfigured;

        public FirebaseNotificationService(
            KanvasDbContext db,
            IConfiguration config,
            ILogger<FirebaseNotificationService> logger,
            IHttpClientFactory httpClientFactory)
        {
            _db = db;
            _config = config;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("FirebaseFCM");

            var serverKey = _config["Firebase:ServerKey"];
            _isConfigured = !string.IsNullOrWhiteSpace(serverKey) && serverKey.Length > 20;
        }

        /// <summary>
        /// FCM HTTP v1 API ile push bildirimi gönderir.
        /// Degrade: ServerKey yoksa veya geçersizse sessizce başarısız olur (crash yok).
        /// </summary>
        public async Task<bool> SendPushAsync(string token, string title, string body, string? clickUrl = null)
        {
            if (!_isConfigured)
            {
                _logger.LogWarning("Firebase yapılandırılmamış. Push gönderilemedi.");
                return false;
            }

            try
            {
                var serverKey = _config["Firebase:ServerKey"];
                var projectId = _config["Firebase:ProjectId"] ?? "7anrps48-firebase";

                var payload = new
                {
                    to = token,
                    notification = new
                    {
                        title,
                        body,
                        click_action = clickUrl ?? "/",
                        icon = "/favicon.svg",
                        badge = "/favicon.svg"
                    },
                    data = new
                    {
                        click_url = clickUrl ?? "/",
                        title,
                        body
                    },
                    priority = "high"
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send")
                {
                    Content = new StringContent(
                        JsonSerializer.Serialize(payload),
                        Encoding.UTF8,
                        "application/json")
                };
                request.Headers.TryAddWithoutValidation("Authorization", $"key={serverKey}");

                var response = await _httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Başarısız token'ları temizle (cihaz abonelikten çıkmış)
                    var result = JsonSerializer.Deserialize<FcmResponse>(responseBody);
                    if (result?.failure > 0 && result.results != null)
                    {
                        for (int i = 0; i < result.results.Length; i++)
                        {
                            if (result.results[i].error == "NotRegistered" || result.results[i].error == "InvalidRegistration")
                            {
                                await MarkTokenInvalidAsync(token);
                            }
                        }
                    }
                    return true;
                }

                _logger.LogWarning("FCM hatası: {StatusCode} - {Body}", response.StatusCode, responseBody);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Push bildirimi gönderilirken hata oluştu.");
                return false;
            }
        }

        /// <summary>
        /// Tüm aktif abonelere toplu push bildirimi gönderir.
        /// </summary>
        public async Task<int> SendBulkPushAsync(string title, string body, string? clickUrl = null)
        {
            if (!_isConfigured) return 0;

            var tokens = await _db.PushAbonelikleri
                .Where(p => p.AktifMi && !p.SilindiMi)
                .Select(p => p.Token)
                .Distinct()
                .ToListAsync();

            if (!tokens.Any()) return 0;

            var successCount = 0;
            foreach (var token in tokens)
            {
                var sent = await SendPushAsync(token, title, body, clickUrl);
                if (sent) successCount++;
            }

            _logger.LogInformation("Toplu push: {sent}/{total} başarılı.", successCount, tokens.Count);
            return successCount;
        }

        /// <summary>Token kaydet (varsa güncelle).</summary>
        public async Task SubscribeAsync(string token, string? appUserId, string? tarayici, string? platform)
        {
            if (string.IsNullOrWhiteSpace(token)) return;

            var existing = await _db.PushAbonelikleri
                .FirstOrDefaultAsync(p => p.Token == token && !p.SilindiMi);

            if (existing != null)
            {
                existing.AppUserId = appUserId ?? existing.AppUserId;
                existing.Tarayici = tarayici ?? existing.Tarayici;
                existing.Platform = platform ?? existing.Platform;
                existing.AktifMi = true;
                existing.SonGorulmeTarihi = DateTime.UtcNow;
            }
            else
            {
                _db.PushAbonelikleri.Add(new PushAbonelik
                {
                    Token = token,
                    AppUserId = appUserId,
                    Tarayici = tarayici,
                    Platform = platform,
                    AktifMi = true,
                    SonGorulmeTarihi = DateTime.UtcNow,
                    OlusturulmaTarihi = DateTime.UtcNow
                });
            }

            await _db.SaveChangesAsync();
        }

        /// <summary>Token'ı pasife al / sil.</summary>
        public async Task UnsubscribeAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return;

            var subscription = await _db.PushAbonelikleri
                .FirstOrDefaultAsync(p => p.Token == token && !p.SilindiMi);

            if (subscription != null)
            {
                subscription.AktifMi = false;
                subscription.SilindiMi = true;
                await _db.SaveChangesAsync();
            }
        }

        /// <summary>Aktif token sayısı.</summary>
        public async Task<int> GetActiveSubscriptionCountAsync()
        {
            return await _db.PushAbonelikleri
                .CountAsync(p => p.AktifMi && !p.SilindiMi);
        }

        private async Task MarkTokenInvalidAsync(string token)
        {
            var sub = await _db.PushAbonelikleri
                .FirstOrDefaultAsync(p => p.Token == token && !p.SilindiMi);
            if (sub != null)
            {
                sub.AktifMi = false;
                sub.SilindiMi = true;
                await _db.SaveChangesAsync();
            }
        }

        private class FcmResponse
        {
            public long success { get; set; }
            public long failure { get; set; }
            public FcmResult[]? results { get; set; }
        }

        private class FcmResult
        {
            public string? error { get; set; }
            public string? message_id { get; set; }
        }
    }
}
