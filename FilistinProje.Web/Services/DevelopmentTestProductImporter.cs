using System.Net.Http;
using System.Text.Json;
using FilistinProje.Core.Helpers;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using Microsoft.EntityFrameworkCore;

namespace FilistinProje.Web.Services;

public sealed class DevelopmentTestProductImporter
{
    private readonly KanvasDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<DevelopmentTestProductImporter> _logger;
    private readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(20) };
    private readonly Dictionary<string, string> _translations = new(StringComparer.Ordinal);

    public DevelopmentTestProductImporter(
        KanvasDbContext context,
        IWebHostEnvironment environment,
        ILogger<DevelopmentTestProductImporter> logger)
    {
        _context = context;
        _environment = environment;
        _logger = logger;
    }

    public async Task ImportAsync(CancellationToken cancellationToken = default)
    {
        if (!_environment.IsDevelopment())
        {
            throw new InvalidOperationException("Test product import is available only in Development.");
        }

        var sourcePath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, "..", "test_urunler.json"));
        if (!File.Exists(sourcePath))
        {
            throw new FileNotFoundException("Test product source was not found.", sourcePath);
        }

        await using var source = File.OpenRead(sourcePath);
        var payload = await JsonSerializer.DeserializeAsync<TestProductDocument>(source, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }, cancellationToken)
            ?? throw new InvalidOperationException("Test product source could not be parsed.");

        if (payload.Products.Count == 0)
        {
            throw new InvalidOperationException("Test product source is empty.");
        }

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        var existingProductIds = await _context.Urunler.Select(x => x.Id).ToListAsync(cancellationToken);

        if (existingProductIds.Count > 0)
        {
            _context.HomePageSectionProducts.RemoveRange(await _context.HomePageSectionProducts
                .Where(x => existingProductIds.Contains(x.UrunId))
                .ToListAsync(cancellationToken));
            _context.Favoriler.RemoveRange(await _context.Favoriler
                .Where(x => existingProductIds.Contains(x.UrunId))
                .ToListAsync(cancellationToken));
            _context.SepetItems.RemoveRange(await _context.SepetItems
                .Where(x => existingProductIds.Contains(x.UrunId))
                .ToListAsync(cancellationToken));
            _context.Yorumlar.RemoveRange(await _context.Yorumlar
                .Where(x => existingProductIds.Contains(x.UrunId))
                .ToListAsync(cancellationToken));
            _context.UrunOzellikDegerleri.RemoveRange(await _context.UrunOzellikDegerleri
                .Where(x => existingProductIds.Contains(x.UrunId))
                .ToListAsync(cancellationToken));
            _context.UrunResimleri.RemoveRange(await _context.UrunResimleri
                .Where(x => existingProductIds.Contains(x.UrunId))
                .ToListAsync(cancellationToken));
            _context.UrunSecenekleri.RemoveRange(await _context.UrunSecenekleri
                .Where(x => existingProductIds.Contains(x.UrunId))
                .ToListAsync(cancellationToken));
            _context.Urunler.RemoveRange(await _context.Urunler
                .Where(x => existingProductIds.Contains(x.Id))
                .ToListAsync(cancellationToken));
            await _context.SaveChangesAsync(cancellationToken);
        }

        var oldTestCategories = await _context.Kategoriler
            .Where(x => x.Slug != null && x.Slug.StartsWith("test-"))
            .ToListAsync(cancellationToken);
        _context.Kategoriler.RemoveRange(oldTestCategories);
        await _context.SaveChangesAsync(cancellationToken);

        var categories = new Dictionary<string, Kategori>(StringComparer.OrdinalIgnoreCase);
        foreach (var categoryName in payload.Products.Select(x => x.Category).Distinct(StringComparer.OrdinalIgnoreCase))
        {
            var categoryArabic = await TranslateToArabicAsync(categoryName, cancellationToken);
            var slug = $"test-{SlugHelper.GenerateSlug(categoryName)}";
            var category = new Kategori
            {
                Ad = categoryArabic,
                AdEn = categoryName,
                AdAr = categoryArabic,
                Slug = slug,
                KisaAciklama = categoryArabic,
                KisaAciklamaEn = categoryName,
                KisaAciklamaAr = categoryArabic,
                Aciklama = categoryArabic,
                AciklamaEn = categoryName,
                AciklamaAr = categoryArabic,
                SeoTitle = categoryArabic,
                SeoTitleEn = categoryName,
                SeoTitleAr = categoryArabic,
                AktifMi = true
            };
            categories.Add(categoryName, category);
            _context.Kategoriler.Add(category);
        }

        await _context.SaveChangesAsync(cancellationToken);

        var products = new List<Urun>(payload.Products.Count);
        foreach (var item in payload.Products)
        {
            var titleArabic = await TranslateToArabicAsync(item.Title, cancellationToken);
            var categoryArabic = categories[item.Category].AdAr;
            var descriptionArabic = $"{titleArabic}. منتج ضمن فئة {categoryArabic}.";
            var salePrice = Math.Round(item.Price * (1m - item.DiscountPercentage / 100m), 2, MidpointRounding.AwayFromZero);
            var slug = $"test-{item.Id}-{SlugHelper.GenerateSlug(item.Title)}";
            var localImages = new List<string>();
            foreach (var (image, index) in (item.Images ?? []).Select((image, index) => (image, index)))
            {
                localImages.Add(await DownloadProductImageAsync(image, item.Id, index + 1, cancellationToken));
            }

            var mainImage = localImages.FirstOrDefault()
                ?? await DownloadProductImageAsync(item.Thumbnail ?? string.Empty, item.Id, 1, cancellationToken);
            var product = new Urun
            {
                Baslik = titleArabic,
                BaslikEn = item.Title,
                BaslikAr = titleArabic,
                KisaAd = titleArabic,
                Slug = slug,
                UrlYolu = slug,
                SKU = item.Sku ?? $"TEST-{item.Id:D3}",
                Barkod = item.Meta?.Barcode ?? string.Empty,
                Marka = item.Brand ?? string.Empty,
                UrunTipi = "Genel",
                Etiketler = string.Join(',', item.Tags ?? []),
                KisaAciklama = descriptionArabic,
                KisaAciklamaEn = item.Description,
                KisaAciklamaAr = descriptionArabic,
                Aciklama = descriptionArabic,
                AciklamaEn = item.Description,
                AciklamaAr = descriptionArabic,
                AnaGorselUrl = mainImage,
                StokDurumu = item.Stock > 0 ? "Stokta" : "Tukendi",
                Fiyat = item.Price,
                IndirimliFiyat = salePrice < item.Price ? salePrice : null,
                Maliyet = Math.Round(item.Price * .6m, 2),
                KdvOrani = 0,
                AktifMi = true,
                YayindaMi = true,
                OneCikanMi = item.Rating >= 4,
                YeniUrunMu = true,
                KampanyaliMi = salePrice < item.Price,
                AnaSayfadaGoster = true,
                Sira = item.Id,
                MinSiparisAdedi = 1,
                SeoTitle = titleArabic,
                SeoTitleEn = item.Title,
                SeoTitleAr = titleArabic,
                SeoDescription = descriptionArabic,
                SeoDescriptionEn = item.Description,
                SeoDescriptionAr = descriptionArabic,
                SeoKeywords = string.Join(',', item.Tags ?? []),
                KategoriId = categories[item.Category].Id,
                UrunSecenek =
                [
                    new UrunSecenek
                    {
                        Olcu = "Standard",
                        VaryantSku = $"{item.Sku ?? $"TEST-{item.Id:D3}"}-STD",
                        SatisFiyati = salePrice,
                        MaliyetFiyati = Math.Round(item.Price * .6m, 2),
                        StokAdedi = Math.Max(item.Stock, 0),
                        AktifMi = true,
                        VarsayilanMi = true,
                        Sira = 1
                    }
                ]
            };

            foreach (var (image, index) in localImages.Select((image, index) => (image, index)))
            {
                product.UrunResimleri.Add(new UrunResim
                {
                    ResimYolu = image,
                    ThumbnailYolu = mainImage,
                    Baslik = titleArabic,
                    AltMetin = titleArabic,
                    Sira = index + 1,
                    VarsayilanMi = index == 0
                });
            }

            products.Add(product);
        }

        _context.Urunler.AddRange(products);
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        _logger.LogInformation("Imported {ProductCount} bilingual test products from {SourcePath}.", products.Count, sourcePath);
    }

    private async Task<string> TranslateToArabicAsync(string text, CancellationToken cancellationToken)
    {
        if (_translations.TryGetValue(text, out var translated))
        {
            return translated;
        }

        var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl=en&tl=ar&dt=t&q={Uri.EscapeDataString(text)}";
        using var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        using var document = JsonDocument.Parse(await response.Content.ReadAsStreamAsync(cancellationToken));
        translated = string.Concat(document.RootElement[0].EnumerateArray().Select(x => x[0].GetString()));
        if (string.IsNullOrWhiteSpace(translated))
        {
            throw new InvalidOperationException($"Arabic translation was empty for '{text}'.");
        }

        _translations[text] = translated;
        return translated;
    }

    private async Task<string> DownloadProductImageAsync(string sourceUrl, int productSourceId, int imageOrder, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(sourceUrl))
        {
            return string.Empty;
        }

        var extension = Path.GetExtension(new Uri(sourceUrl).AbsolutePath);
        extension = string.Equals(extension, ".webp", StringComparison.OrdinalIgnoreCase) ? ".webp" : ".jpg";
        var relativeDirectory = Path.Combine("uploads", "test-products");
        var physicalDirectory = Path.Combine(_environment.WebRootPath, relativeDirectory);
        Directory.CreateDirectory(physicalDirectory);

        var fileName = $"test-{productSourceId:D3}-{imageOrder:D2}{extension}";
        var physicalPath = Path.Combine(physicalDirectory, fileName);
        using var response = await _httpClient.GetAsync(sourceUrl, cancellationToken);
        response.EnsureSuccessStatusCode();
        var contentType = response.Content.Headers.ContentType?.MediaType;
        if (contentType is null || !contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Product image response was not an image: {sourceUrl}");
        }

        await using var output = File.Create(physicalPath);
        await response.Content.CopyToAsync(output, cancellationToken);
        return $"/{relativeDirectory.Replace(Path.DirectorySeparatorChar, '/')}/{fileName}";
    }

    private sealed class TestProductDocument
    {
        public List<TestProduct> Products { get; set; } = [];
    }

    private sealed class TestProduct
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal Rating { get; set; }
        public int Stock { get; set; }
        public List<string>? Tags { get; set; }
        public string? Brand { get; set; }
        public string? Sku { get; set; }
        public List<string>? Images { get; set; }
        public string? Thumbnail { get; set; }
        public TestProductMeta? Meta { get; set; }
    }

    private sealed class TestProductMeta
    {
        public string? Barcode { get; set; }
    }
}
