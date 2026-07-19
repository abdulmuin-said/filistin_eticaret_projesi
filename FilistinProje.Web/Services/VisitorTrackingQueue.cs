using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using Microsoft.EntityFrameworkCore;

namespace FilistinProje.Web.Services;

public sealed record VisitorTrackingEntry(
    string IpAddress,
    string Url,
    string Method,
    string? Referrer,
    string UserAgent,
    string Browser,
    string OperatingSystem,
    string DeviceModel,
    string? UserName,
    DateTime CreatedAtUtc);

public interface IVisitorTrackingQueue
{
    bool TryEnqueue(VisitorTrackingEntry entry);
}

public sealed class VisitorTrackingQueue : BackgroundService, IVisitorTrackingQueue
{
    private const int BatchSize = 100;
    private readonly Channel<VisitorTrackingEntry> _channel;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<VisitorTrackingQueue> _logger;
    private readonly byte[] _ipHashKey;
    private readonly bool _retentionCleanupEnabled;
    private readonly int _retentionDays;
    private long _droppedEntries;

    public VisitorTrackingQueue(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<VisitorTrackingQueue> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _channel = Channel.CreateBounded<VisitorTrackingEntry>(new BoundedChannelOptions(2_000)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false
        });

        var configuredKey = configuration["VisitorTracking:IpHashKey"];
        _ipHashKey = string.IsNullOrWhiteSpace(configuredKey)
            ? RandomNumberGenerator.GetBytes(32)
            : SHA256.HashData(Encoding.UTF8.GetBytes(configuredKey));
        _retentionCleanupEnabled = configuration.GetValue<bool>("VisitorTracking:EnableRetentionCleanup");
        _retentionDays = Math.Clamp(configuration.GetValue("VisitorTracking:RetentionDays", 90), 7, 365);
    }

    public bool TryEnqueue(VisitorTrackingEntry entry)
    {
        if (_channel.Writer.TryWrite(entry))
        {
            return true;
        }

        var dropped = Interlocked.Increment(ref _droppedEntries);
        if (dropped == 1 || dropped % 100 == 0)
        {
            _logger.LogWarning("Ziyaretci takip kuyrugu dolu; dusurulen kayit sayisi={DroppedCount}", dropped);
        }

        return false;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var lastCleanupUtc = DateTime.MinValue;
        try
        {
            while (await _channel.Reader.WaitToReadAsync(stoppingToken))
            {
                var batch = new List<VisitorTrackingEntry>(BatchSize);
                while (batch.Count < BatchSize && _channel.Reader.TryRead(out var entry))
                {
                    batch.Add(entry);
                }

                if (batch.Count > 0)
                {
                    await PersistBatchAsync(batch, stoppingToken);
                }

                if (_retentionCleanupEnabled && DateTime.UtcNow - lastCleanupUtc >= TimeSpan.FromDays(1))
                {
                    await CleanupExpiredEntriesAsync(stoppingToken);
                    lastCleanupUtc = DateTime.UtcNow;
                }
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Normal host shutdown.
        }
        finally
        {
            while (_channel.Reader.TryPeek(out _))
            {
                var remaining = new List<VisitorTrackingEntry>(BatchSize);
                while (remaining.Count < BatchSize && _channel.Reader.TryRead(out var entry))
                {
                    remaining.Add(entry);
                }

                if (remaining.Count > 0)
                {
                    await PersistBatchAsync(remaining, CancellationToken.None);
                }
            }
        }
    }

    private async Task PersistBatchAsync(
        IReadOnlyCollection<VisitorTrackingEntry> batch,
        CancellationToken cancellationToken)
    {
        try
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<KanvasDbContext>();
            context.ZiyaretciLoglari.AddRange(batch.Select(entry => new ZiyaretciLog
            {
                IpAdresi = HashIpAddress(entry.IpAddress),
                Url = entry.Url,
                Metod = entry.Method,
                ReferansUrl = entry.Referrer,
                CihazBilgisi = entry.UserAgent,
                Tarayici = entry.Browser,
                IsletimSistemi = entry.OperatingSystem,
                CihazModeli = entry.DeviceModel,
                Sehir = "-",
                Ulke = "-",
                OlusturulmaTarihi = entry.CreatedAtUtc,
                KullaniciAdi = entry.UserName
            }));
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ziyaretci takip batch'i yazilamadi. KayitSayisi={Count}", batch.Count);
        }
    }

    private async Task CleanupExpiredEntriesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<KanvasDbContext>();
            var cutoffUtc = DateTime.UtcNow.AddDays(-_retentionDays);
            var deleted = await context.ZiyaretciLoglari
                .Where(x => x.OlusturulmaTarihi < cutoffUtc)
                .ExecuteDeleteAsync(cancellationToken);
            _logger.LogInformation("Ziyaretci retention temizligi tamamlandi. Silinen={DeletedCount}", deleted);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ziyaretci retention temizligi basarisiz.");
        }
    }

    private string HashIpAddress(string ipAddress)
    {
        using var hmac = new HMACSHA256(_ipHashKey);
        return Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(ipAddress)))[..32];
    }
}
