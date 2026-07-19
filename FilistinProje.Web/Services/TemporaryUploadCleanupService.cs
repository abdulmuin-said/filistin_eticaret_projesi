using FilistinProje.Service.Interfaces;

namespace FilistinProje.Web.Services;

public sealed class TemporaryUploadCleanupService : BackgroundService
{
    private static readonly TimeSpan CleanupInterval = TimeSpan.FromMinutes(30);
    private static readonly TimeSpan Retention = TimeSpan.FromHours(2);
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TemporaryUploadCleanupService> _logger;

    public TemporaryUploadCleanupService(
        IServiceScopeFactory scopeFactory,
        ILogger<TemporaryUploadCleanupService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await CleanupAsync(stoppingToken);
        using var timer = new PeriodicTimer(CleanupInterval);
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await CleanupAsync(stoppingToken);
        }
    }

    internal async Task CleanupAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var fileService = scope.ServiceProvider.GetRequiredService<IDosyaServisi>();
            var temporaryRoot = Path.GetFullPath(Path.Combine(fileService.GetPrivateStorageRoot(), "gecici"));
            if (!Directory.Exists(temporaryRoot))
            {
                return;
            }

            var cutoff = DateTime.UtcNow - Retention;
            var deleted = 0;
            foreach (var file in Directory.EnumerateFiles(temporaryRoot, "*", SearchOption.AllDirectories))
            {
                cancellationToken.ThrowIfCancellationRequested();
                var fullPath = Path.GetFullPath(file);
                if (!fullPath.StartsWith(temporaryRoot, StringComparison.OrdinalIgnoreCase) ||
                    File.GetLastWriteTimeUtc(fullPath) >= cutoff)
                {
                    continue;
                }

                File.Delete(fullPath);
                deleted++;
            }

            foreach (var directory in Directory.EnumerateDirectories(temporaryRoot, "*", SearchOption.AllDirectories)
                         .OrderByDescending(x => x.Length))
            {
                if (!Directory.EnumerateFileSystemEntries(directory).Any())
                {
                    Directory.Delete(directory);
                }
            }

            if (deleted > 0)
            {
                _logger.LogInformation("Süresi dolan geçici checkout dosyaları temizlendi. Adet={Count}", deleted);
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Geçici checkout dosyaları temizlenemedi.");
        }
    }
}
