using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FilistinProje.Web.Services;

public interface IHeaderCategoryService
{
    Task<IReadOnlyList<Kategori>> GetCategoriesAsync();
}

public sealed class HeaderCategoryService : IHeaderCategoryService
{
    private readonly KanvasDbContext _context;
    private readonly ICacheService _cache;
    private readonly ILogger<HeaderCategoryService> _logger;

    public HeaderCategoryService(
        KanvasDbContext context,
        ICacheService cache,
        ILogger<HeaderCategoryService> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task<IReadOnlyList<Kategori>> GetCategoriesAsync()
    {
        try
        {
            return await _cache.GetOrAddAsync(
                "category-menu:v1:header",
                () => _context.Kategoriler
                    .AsNoTracking()
                    .Where(category => category.AktifMi && !category.SilindiMi)
                    .OrderBy(category => category.Sira)
                    .ThenBy(category => category.Ad)
                    .ToListAsync(),
                TimeSpan.FromMinutes(10));
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Header kategorileri yuklenemedi; bos menu gosteriliyor.");
            return Array.Empty<Kategori>();
        }
    }
}
