using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Core.Helpers;
using FilistinProje.Service.Interfaces;
using FilistinProje.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SixLabors.ImageSharp;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KategoriController : AdminBaseController
    {
        private readonly KanvasDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };
        private const long MaxImageFileBytes = 10 * 1024 * 1024;

        public KategoriController(
            KanvasDbContext context,
            ICacheService cacheService,
            IWebHostEnvironment webHostEnvironment,
            IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _cacheService = cacheService;
            _webHostEnvironment = webHostEnvironment;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index(string? arama, string? durum, string? tip)
        {
            var kategoriler = await BuildCategoryListQuery(arama, durum, tip)
                .ToListAsync();

            ViewBag.Arama = arama;
            ViewBag.Durum = durum;
            ViewBag.Tip = tip;
            ViewBag.ToplamKategori = kategoriler.Count;
            ViewBag.AktifKategori = kategoriler.Count(x => x.AktifMi && !x.SilindiMi);
            ViewBag.AnaKategori = kategoriler.Count(x => !x.ParentKategoriId.HasValue);
            ViewBag.AltKategori = kategoriler.Count(x => x.ParentKategoriId.HasValue);

            return View(kategoriler);
        }

        public async Task<IActionResult> ExcelExport(string? arama, string? durum, string? tip)
        {
            var kategoriler = await BuildCategoryListQuery(arama, durum, tip).ToListAsync();
            var categoryLookup = kategoriler.ToDictionary(x => x.Id);

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Kategoriler");
            var headers = new[]
            {
                "ID",
                "الفئة",
                "التسلسل الهرمي",
                "الفئة الأم",
                "Slug",
                "وصف مختصر",
                "عدد المنتجات",
                "عدد الفئات الفرعية",
                "الترتيب",
                "الحالة",
                "عنوان SEO",
                "وصف SEO"
            };

            for (var i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            var row = 2;
            foreach (var kategori in kategoriler)
            {
                worksheet.Cells[row, 1].Value = kategori.Id;
                worksheet.Cells[row, 2].Value = kategori.Ad;
                worksheet.Cells[row, 3].Value = CategoryPresentationHelper.BuildHierarchyLabel(kategori, categoryLookup);
                worksheet.Cells[row, 4].Value = kategori.ParentKategori?.Ad ?? "فئة رئيسية";
                worksheet.Cells[row, 5].Value = kategori.Slug;
                worksheet.Cells[row, 6].Value = kategori.KisaAciklama;
                worksheet.Cells[row, 7].Value = kategori.Urunler.Count(x => !x.SilindiMi);
                worksheet.Cells[row, 8].Value = kategori.AltKategoriler.Count(x => !x.SilindiMi);
                worksheet.Cells[row, 9].Value = kategori.Sira;
                worksheet.Cells[row, 10].Value = kategori.AktifMi && !kategori.SilindiMi ? "نشط" : "غير نشط";
                worksheet.Cells[row, 11].Value = kategori.SeoTitle;
                worksheet.Cells[row, 12].Value = kategori.SeoDescription;
                row++;
            }

            using (var range = worksheet.Cells[1, 1, 1, headers.Length])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(49, 53, 17));
                range.Style.Font.Color.SetColor(System.Drawing.Color.White);
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            return File(
                package.GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"kategoriler-{DateTime.Now:yyyyMMdd-HHmm}.xlsx");
        }

        public async Task<IActionResult> PdfExport(string? arama, string? durum, string? tip)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var kategoriler = await BuildCategoryListQuery(arama, durum, tip).ToListAsync();
            var categoryLookup = kategoriler.ToDictionary(x => x.Id);

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(24);
                    page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

                    page.Header().Column(column =>
                    {
                        column.Item().Text("Kategori YÃ¶netimi Raporu").FontSize(18).SemiBold().FontColor("#313511");
                        column.Item().Text($"OluÅŸturulma: {DateTime.Now:dd.MM.yyyy HH:mm} | KayÄ±t: {kategoriler.Count}")
                            .FontSize(9)
                            .FontColor("#6b6b61");
                    });

                    page.Content().PaddingTop(14).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(34);
                            columns.RelativeColumn(2.4f);
                            columns.RelativeColumn(2.8f);
                            columns.RelativeColumn(1.9f);
                            columns.RelativeColumn(2.2f);
                            columns.ConstantColumn(46);
                            columns.ConstantColumn(50);
                            columns.ConstantColumn(46);
                            columns.ConstantColumn(50);
                        });

                        table.Header(header =>
                        {
                            AddPdfHeader(header, "ID");
                            AddPdfHeader(header, "الفئة");
                            AddPdfHeader(header, "التسلسل الهرمي");
                            AddPdfHeader(header, "الفئة الأم");
                            AddPdfHeader(header, "Slug");
                            AddPdfHeader(header, "المنتجات");
                            AddPdfHeader(header, "الفروع");
                            AddPdfHeader(header, "الترتيب");
                            AddPdfHeader(header, "الحالة");
                        });

                        foreach (var kategori in kategoriler)
                        {
                            AddPdfCell(table, kategori.Id.ToString());
                            AddPdfCell(table, kategori.Ad);
                            AddPdfCell(table, CategoryPresentationHelper.BuildHierarchyLabel(kategori, categoryLookup));
                            AddPdfCell(table, kategori.ParentKategori?.Ad ?? "فئة رئيسية");
                            AddPdfCell(table, kategori.Slug ?? "-");
                            AddPdfCell(table, kategori.Urunler.Count(x => !x.SilindiMi).ToString());
                            AddPdfCell(table, kategori.AltKategoriler.Count(x => !x.SilindiMi).ToString());
                            AddPdfCell(table, kategori.Sira.ToString());
                            AddPdfCell(table, kategori.AktifMi && !kategori.SilindiMi ? "نشط" : "غير نشط");
                        }
                    });

                    page.Footer()
                        .AlignRight()
                        .Text(text =>
                        {
                            text.Span("Sayfa ");
                            text.CurrentPageNumber();
                            text.Span(" / ");
                            text.TotalPages();
                        });
                });
            }).GeneratePdf();

            return File(pdfBytes, "application/pdf", $"kategoriler-{DateTime.Now:yyyyMMdd-HHmm}.pdf");
        }

        [HttpGet]
        public async Task<IActionResult> Ekle()
        {
            await PopulateParentCategoriesAsync();
            return View(new Kategori
            {
                AktifMi = true,
                UrunSiralamaTipi = "manual"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(Kategori kategori, IFormFile? gorselDosyasi, IFormFile? bannerDosyasi)
        {
            NormalizeLocalizedCategoryFields(kategori);
            ModelState.Clear();
            await ValidateImageUploadAsync(gorselDosyasi, "gorselDosyasi");
            await ValidateImageUploadAsync(bannerDosyasi, "bannerDosyasi");
            if (!await ValidateCategoryAsync(kategori))
            {
                await PopulateParentCategoriesAsync(kategori.ParentKategoriId);
                return View(kategori);
            }

            kategori.GorselUrl = string.IsNullOrWhiteSpace(kategori.GorselUrl) ? null : kategori.GorselUrl.Trim();
            kategori.BannerUrl = string.IsNullOrWhiteSpace(kategori.BannerUrl) ? null : kategori.BannerUrl.Trim();
            kategori.MenuGorselUrl = string.IsNullOrWhiteSpace(kategori.MenuGorselUrl) ? null : kategori.MenuGorselUrl.Trim();
            kategori.OlusturulmaTarihi = DateTime.UtcNow;
            kategori.Sira = await NormalizeCategoryOrderAsync(kategori.Sira);
            kategori.UrunSiralamaTipi = NormalizeSortType(kategori.UrunSiralamaTipi);
            kategori.Slug = await GenerateUniqueCategorySlugAsync(kategori.Slug, kategori.Ad, null);
            if (gorselDosyasi is { Length: > 0 })
            {
                kategori.GorselUrl = await SaveCategoryImageAsync(gorselDosyasi);
            }
            if (bannerDosyasi is { Length: > 0 })
            {
                kategori.BannerUrl = await SaveCategoryImageAsync(bannerDosyasi);
            }

            _context.Kategoriler.Add(kategori);
            await _context.SaveChangesAsync();
            await InvalidateCategoryCachesAsync();

            TempData["Basari"] = _localizer["Admin_IslemBasarili"].Value;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Duzenle(int id)
        {
            var kategori = await _context.Kategoriler
                .Include(x => x.ParentKategori)
                .Include(x => x.AltKategoriler)
                .Include(x => x.Urunler)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (kategori == null)
            {
                return NotFound();
            }

            PrepareLocalizedCategoryFields(kategori);
            PopulateCategoryEditStats(kategori);
            await PopulateParentCategoriesAsync(kategori.ParentKategoriId, kategori.Id);
            return View(kategori);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(Kategori model, IFormFile? gorselDosyasi, IFormFile? bannerDosyasi)
        {
            var kategori = await _context.Kategoriler.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (kategori == null)
            {
                return NotFound();
            }

            NormalizeLocalizedCategoryFields(model);
            ModelState.Clear();
            await ValidateImageUploadAsync(gorselDosyasi, "gorselDosyasi");
            await ValidateImageUploadAsync(bannerDosyasi, "bannerDosyasi");
            if (!await ValidateCategoryAsync(model))
            {
                PopulateCategoryEditStats(kategori);
                await PopulateParentCategoriesAsync(model.ParentKategoriId, model.Id);
                return View(model);
            }

            kategori.Ad = model.Ad.Trim();
            kategori.AdEn = model.AdEn?.Trim() ?? string.Empty;
            kategori.AdAr = model.AdAr?.Trim() ?? string.Empty;
            kategori.KisaAciklama = model.KisaAciklama?.Trim() ?? string.Empty;
            kategori.KisaAciklamaEn = model.KisaAciklamaEn?.Trim() ?? string.Empty;
            kategori.KisaAciklamaAr = model.KisaAciklamaAr?.Trim() ?? string.Empty;
            kategori.Aciklama = model.Aciklama?.Trim() ?? string.Empty;
            kategori.AciklamaEn = model.AciklamaEn?.Trim() ?? string.Empty;
            kategori.AciklamaAr = model.AciklamaAr?.Trim() ?? string.Empty;
            kategori.GorselUrl = string.IsNullOrWhiteSpace(model.GorselUrl) ? null : model.GorselUrl.Trim();
            kategori.BannerUrl = string.IsNullOrWhiteSpace(model.BannerUrl) ? null : model.BannerUrl.Trim();
            kategori.MenuGorselUrl = string.IsNullOrWhiteSpace(model.MenuGorselUrl) ? null : model.MenuGorselUrl.Trim();
            kategori.ParentKategoriId = model.ParentKategoriId;
            kategori.AktifMi = model.AktifMi;
            kategori.ReceteGerekliMi = model.ReceteGerekliMi;
            kategori.Sira = await NormalizeCategoryOrderAsync(model.Sira);
            kategori.SeoTitle = model.SeoTitle?.Trim() ?? string.Empty;
            kategori.SeoTitleEn = model.SeoTitleEn?.Trim() ?? string.Empty;
            kategori.SeoTitleAr = model.SeoTitleAr?.Trim() ?? string.Empty;
            kategori.SeoDescription = model.SeoDescription?.Trim() ?? string.Empty;
            kategori.SeoDescriptionEn = model.SeoDescriptionEn?.Trim() ?? string.Empty;
            kategori.SeoDescriptionAr = model.SeoDescriptionAr?.Trim() ?? string.Empty;
            kategori.UstMetin = model.UstMetin?.Trim() ?? string.Empty;
            kategori.UstMetinEn = model.UstMetinEn?.Trim() ?? string.Empty;
            kategori.UstMetinAr = model.UstMetinAr?.Trim() ?? string.Empty;
            kategori.AltMetin = model.AltMetin?.Trim() ?? string.Empty;
            kategori.AltMetinEn = model.AltMetinEn?.Trim() ?? string.Empty;
            kategori.AltMetinAr = model.AltMetinAr?.Trim() ?? string.Empty;
            kategori.KampanyaEtiketi = model.KampanyaEtiketi?.Trim() ?? string.Empty;
            kategori.KampanyaEtiketiEn = model.KampanyaEtiketiEn?.Trim() ?? string.Empty;
            kategori.KampanyaEtiketiAr = model.KampanyaEtiketiAr?.Trim() ?? string.Empty;
            kategori.UrunSiralamaTipi = NormalizeSortType(model.UrunSiralamaTipi);
            kategori.Slug = await GenerateUniqueCategorySlugAsync(model.Slug, model.Ad, model.Id);
            if (gorselDosyasi is { Length: > 0 })
            {
                kategori.GorselUrl = await SaveCategoryImageAsync(gorselDosyasi);
            }
            if (bannerDosyasi is { Length: > 0 })
            {
                kategori.BannerUrl = await SaveCategoryImageAsync(bannerDosyasi);
            }

            await _context.SaveChangesAsync();
            await InvalidateCategoryCachesAsync();

            TempData["Basari"] = _localizer["Admin_IslemBasarili"].Value;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(int id)
        {
            var kategori = await _context.Kategoriler.FirstOrDefaultAsync(x => x.Id == id);
            if (kategori == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var altKategoriler = await _context.Kategoriler
                .Where(x => x.ParentKategoriId == id)
                .ToListAsync();

            foreach (var altKategori in altKategoriler)
            {
                altKategori.ParentKategoriId = null;
            }

            kategori.AktifMi = false;
            kategori.SilindiMi = true;

            await _context.SaveChangesAsync();
            await InvalidateCategoryCachesAsync();
            TempData["Basari"] = _localizer["Admin_IslemBasarili"].Value;
            return RedirectToAction(nameof(Index));
        }

        private Task InvalidateCategoryCachesAsync()
        {
            return _cacheService.RemoveByPrefixAsync("category-menu:v1:");
        }

        private static void NormalizeLocalizedCategoryFields(Kategori kategori)
        {
            kategori.AdEn = kategori.AdEn?.Trim() ?? string.Empty;
            kategori.AdAr = kategori.AdAr?.Trim() ?? string.Empty;
            kategori.Ad = FirstNotEmpty(kategori.AdAr, kategori.AdEn, kategori.Ad);
            kategori.KisaAciklamaEn = kategori.KisaAciklamaEn?.Trim() ?? string.Empty;
            kategori.KisaAciklamaAr = kategori.KisaAciklamaAr?.Trim() ?? string.Empty;
            kategori.KisaAciklama = FirstNotEmpty(kategori.KisaAciklamaAr, kategori.KisaAciklamaEn, kategori.KisaAciklama);
            kategori.AciklamaEn = kategori.AciklamaEn?.Trim() ?? string.Empty;
            kategori.AciklamaAr = kategori.AciklamaAr?.Trim() ?? string.Empty;
            kategori.Aciklama = FirstNotEmpty(kategori.AciklamaAr, kategori.AciklamaEn, kategori.Aciklama);
            kategori.SeoTitleEn = kategori.SeoTitleEn?.Trim() ?? string.Empty;
            kategori.SeoTitleAr = kategori.SeoTitleAr?.Trim() ?? string.Empty;
            kategori.SeoTitle = FirstNotEmpty(kategori.SeoTitleAr, kategori.SeoTitleEn, kategori.SeoTitle);
            kategori.SeoDescriptionEn = kategori.SeoDescriptionEn?.Trim() ?? string.Empty;
            kategori.SeoDescriptionAr = kategori.SeoDescriptionAr?.Trim() ?? string.Empty;
            kategori.SeoDescription = FirstNotEmpty(kategori.SeoDescriptionAr, kategori.SeoDescriptionEn, kategori.SeoDescription);
            kategori.UstMetinEn = kategori.UstMetinEn?.Trim() ?? string.Empty;
            kategori.UstMetinAr = kategori.UstMetinAr?.Trim() ?? string.Empty;
            kategori.UstMetin = FirstNotEmpty(kategori.UstMetinAr, kategori.UstMetinEn, kategori.UstMetin);
            kategori.AltMetinEn = kategori.AltMetinEn?.Trim() ?? string.Empty;
            kategori.AltMetinAr = kategori.AltMetinAr?.Trim() ?? string.Empty;
            kategori.AltMetin = FirstNotEmpty(kategori.AltMetinAr, kategori.AltMetinEn, kategori.AltMetin);
            kategori.KampanyaEtiketiEn = kategori.KampanyaEtiketiEn?.Trim() ?? string.Empty;
            kategori.KampanyaEtiketiAr = kategori.KampanyaEtiketiAr?.Trim() ?? string.Empty;
            kategori.KampanyaEtiketi = FirstNotEmpty(kategori.KampanyaEtiketiAr, kategori.KampanyaEtiketiEn, kategori.KampanyaEtiketi);
        }

        private static void PrepareLocalizedCategoryFields(Kategori kategori)
        {
            kategori.AdAr = FirstNotEmpty(kategori.AdAr, kategori.Ad);
            kategori.AdEn = FirstNotEmpty(kategori.AdEn, kategori.Ad);
            kategori.KisaAciklamaAr = FirstNotEmpty(kategori.KisaAciklamaAr, kategori.KisaAciklama);
            kategori.KisaAciklamaEn = FirstNotEmpty(kategori.KisaAciklamaEn, kategori.KisaAciklama);
            kategori.AciklamaAr = FirstNotEmpty(kategori.AciklamaAr, kategori.Aciklama);
            kategori.AciklamaEn = FirstNotEmpty(kategori.AciklamaEn, kategori.Aciklama);
        }

        private static string FirstNotEmpty(params string?[] values) =>
            values.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))?.Trim() ?? string.Empty;

        private IQueryable<Kategori> BuildCategoryListQuery(string? arama, string? durum, string? tip)
        {
            var query = _context.Kategoriler
                .Include(x => x.ParentKategori)
                .Include(x => x.AltKategoriler)
                .Include(x => x.Urunler)
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(arama))
            {
                var term = arama.Trim().ToLower();
                query = query.Where(x =>
                    x.Ad.ToLower().Contains(term) ||
                    (x.Slug != null && x.Slug.ToLower().Contains(term)) ||
                    x.KisaAciklama.ToLower().Contains(term) ||
                    x.SeoTitle.ToLower().Contains(term));
            }

            query = durum switch
            {
                "aktif" => query.Where(x => x.AktifMi && !x.SilindiMi),
                "pasif" => query.Where(x => !x.AktifMi || x.SilindiMi),
                _ => query
            };

            query = tip switch
            {
                "ana" => query.Where(x => x.ParentKategoriId == null),
                "alt" => query.Where(x => x.ParentKategoriId != null),
                _ => query
            };

            return query
                .OrderBy(x => x.Sira)
                .ThenBy(x => x.Ad);
        }

        private static void AddPdfHeader(TableCellDescriptor header, string text)
        {
            header.Cell()
                .Background("#313511")
                .Border(0.5f)
                .BorderColor("#313511")
                .Padding(5)
                .Text(text)
                .FontColor(Colors.White)
                .SemiBold();
        }

        private static void AddPdfCell(TableDescriptor table, string text)
        {
            table.Cell()
                .BorderBottom(0.5f)
                .BorderColor("#e5e2dc")
                .Padding(5)
                .Text(string.IsNullOrWhiteSpace(text) ? "-" : text);
        }

        private async Task PopulateParentCategoriesAsync(int? selectedId = null, int? excludedId = null)
        {
            ViewBag.ParentKategoriler = new SelectList(
                await BuildParentCategoryOptionsAsync(excludedId),
                "Value",
                "Text",
                selectedId?.ToString());
        }

        private void PopulateCategoryEditStats(Kategori kategori)
        {
            ViewBag.UrunSayisi = kategori.Urunler?.Count(x => !x.SilindiMi) ?? 0;
            ViewBag.AltKategoriSayisi = kategori.AltKategoriler?.Count(x => !x.SilindiMi) ?? 0;
            ViewBag.UstKategoriAdi = kategori.ParentKategori?.Ad ?? "فئة رئيسية";
        }

        private async Task<List<SelectListItem>> BuildParentCategoryOptionsAsync(int? excludedId)
        {
            var kategoriler = await _context.Kategoriler
                .OrderBy(x => x.Sira)
                .ThenBy(x => x.Ad)
                .ToListAsync();

            var optionList = new List<SelectListItem>
            {
                new() { Value = string.Empty, Text = "فئة رئيسية" }
            };

            foreach (var (category, depth) in CategoryTreeHelper.FlattenHierarchy(kategoriler, excludedId))
            {
                optionList.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = $"{new string('-', depth * 2)}{(depth > 0 ? " " : string.Empty)}{category.Ad}"
                });
            }

            return optionList;
        }

        private async Task<bool> ValidateCategoryAsync(Kategori kategori)
        {
            if (string.IsNullOrWhiteSpace(kategori.Ad))
            {
                ModelState.AddModelError(nameof(Kategori.Ad), _localizer["Admin_Category_NameRequired"].Value);
            }

            if (kategori.ParentKategoriId == kategori.Id && kategori.Id != 0)
            {
                ModelState.AddModelError(nameof(Kategori.ParentKategoriId), _localizer["Admin_Category_SelfParentError"].Value);
            }

            if (kategori.ParentKategoriId.HasValue)
            {
                var categories = await _context.Kategoriler
                    .IgnoreQueryFilters()
                    .ToListAsync();

                var parentExists = categories.Any(x => x.Id == kategori.ParentKategoriId.Value);
                if (!parentExists)
                {
                    ModelState.AddModelError(nameof(Kategori.ParentKategoriId), _localizer["Admin_Category_ParentNotFoundError"].Value);
                }
                else if (kategori.Id != 0 && CategoryTreeHelper.IsDescendant(categories, kategori.Id, kategori.ParentKategoriId.Value))
                {
                    ModelState.AddModelError(nameof(Kategori.ParentKategoriId), _localizer["Admin_Category_ChildParentError"].Value);
                }
            }

            return ModelState.IsValid;
        }

        private async Task ValidateImageUploadAsync(IFormFile? file, string modelStateKey)
        {
            if (file == null || file.Length == 0)
            {
                return;
            }

            var extension = Path.GetExtension(file.FileName);
            if (!AllowedImageExtensions.Contains(extension) || file.Length > MaxImageFileBytes)
            {
                ModelState.AddModelError(modelStateKey, _localizer["Admin_InvalidCategoryImageUpload"]);
                return;
            }

            try
            {
                await using var stream = file.OpenReadStream();
                if (await SixLabors.ImageSharp.Image.IdentifyAsync(stream) == null)
                {
                    ModelState.AddModelError(modelStateKey, _localizer["Admin_InvalidCategoryImageUpload"]);
                }
            }
            catch (UnknownImageFormatException)
            {
                ModelState.AddModelError(modelStateKey, _localizer["Admin_InvalidCategoryImageUpload"]);
            }
            catch (InvalidImageContentException)
            {
                ModelState.AddModelError(modelStateKey, _localizer["Admin_InvalidCategoryImageUpload"]);
            }
        }

        private async Task<string> SaveCategoryImageAsync(IFormFile file)
        {
            var uploadDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "kategoriler");
            Directory.CreateDirectory(uploadDirectory);

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid():N}{extension}";
            var physicalPath = Path.Combine(uploadDirectory, fileName);

            await using var stream = new FileStream(physicalPath, FileMode.CreateNew);
            await file.CopyToAsync(stream);

            return $"/uploads/kategoriler/{fileName}";
        }

        private async Task<int> NormalizeCategoryOrderAsync(int currentOrder)
        {
            if (currentOrder > 0)
            {
                return currentOrder;
            }

            var lastOrder = await _context.Kategoriler
                .OrderByDescending(x => x.Sira)
                .Select(x => (int?)x.Sira)
                .FirstOrDefaultAsync();

            return (lastOrder ?? 0) + 1;
        }

        private static string NormalizeSortType(string? sortType)
        {
            var value = sortType?.Trim().ToLowerInvariant();
            return value switch
            {
                "price_asc" => "price_asc",
                "price_desc" => "price_desc",
                "newest" => "newest",
                "popular" => "popular",
                _ => "manual"
            };
        }

        private async Task<string> GenerateUniqueCategorySlugAsync(string? requestedSlug, string title, int? excludedId)
        {
            var baseSlug = SlugHelper.GenerateSlug(string.IsNullOrWhiteSpace(requestedSlug) ? title : requestedSlug);
            var existingSlugs = await _context.Kategoriler
                .Where(x => x.Id != excludedId && x.Slug != null)
                .Select(x => x.Slug!)
                .ToListAsync();

            return SlugHelper.EnsureUnique(baseSlug, existingSlugs);
        }
    }
}
