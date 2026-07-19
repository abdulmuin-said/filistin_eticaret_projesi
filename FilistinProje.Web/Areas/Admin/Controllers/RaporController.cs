using FilistinProje.Core.Helpers;
using FilistinProje.Core.Models;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FilistinProje.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RaporController : AdminBaseController
    {
        private readonly KanvasDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public RaporController(
            KanvasDbContext context,
            IWebHostEnvironment webHostEnvironment,
            IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index(DateTime? baslangic, DateTime? bitis)
        {
            var (startUtc, endUtc, startLocal, endLocal) = ResolveDateRange(baslangic, bitis);
            var model = await BuildReportAsync(startUtc, endUtc, startLocal, endLocal);
            return View(model);
        }

        public async Task<IActionResult> ExcelExport(DateTime? baslangic, DateTime? bitis)
        {
            var (startUtc, endUtc, startLocal, endLocal) = ResolveDateRange(baslangic, bitis);
            var model = await BuildReportAsync(startUtc, endUtc, startLocal, endLocal);

            using var package = new ExcelPackage();

            AddSummarySheet(package, model);
            AddDailySheet(package, model);
            AddHourlySheet(package, model);
            AddStatusSheet(package, model);
            AddProductSheet(package, L("Admin_ReportSheetBestSellingProducts"), model.EnCokSatanUrunler);
            AddProductSheet(package, L("Admin_ReportSheetMostClickedProducts"), model.EnCokTiklananUrunler);
            AddConversionSheet(package, model);
            AddCustomerSheet(package, model);
            AddReturnReasonSheet(package, model);
            AddCategorySheet(package, model);
            AddCitySheet(package, model);
            AddCouponSheet(package, model);
            AddTrafficSheet(package, L("Admin_ReportSheetTrafficSources"), model.TrafikKaynaklari);
            AddTrafficSheet(package, L("Admin_ReportSheetVisitedPages"), model.EnCokGezilenSayfalar);
            AddTrafficSheet(package, L("Admin_ReportSheetDeviceDistribution"), model.CihazDagilimi);
            AddCargoSheet(package, model);

            var bytes = package.GetAsByteArray();
            return File(
                bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                L("Admin_ReportExcelFileName", startLocal, endLocal));
        }

        public async Task<IActionResult> PdfExport(DateTime? baslangic, DateTime? bitis)
        {
            var (startUtc, endUtc, startLocal, endLocal) = ResolveDateRange(baslangic, bitis);
            var model = await BuildReportAsync(startUtc, endUtc, startLocal, endLocal);
            var logoPath = Path.Combine(_webHostEnvironment.WebRootPath, "74anrps48logo2.svg");

            QuestPDF.Settings.License = LicenseType.Community;
            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(28);
                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));

                    page.Header().Column(column =>
                    {
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Column(header =>
                            {
                                header.Item().Text(L("Admin_ReportPdfTitle")).FontSize(18).Bold().FontColor("#313511");
                                header.Item().Text(model.AralikEtiketi).FontSize(9).FontColor("#6b6f45");
                            });

                            row.ConstantItem(120).AlignRight().Element(box =>
                            {
                                if (System.IO.File.Exists(logoPath))
                                {
                                    box.Height(44).Image(logoPath).FitArea();
                                }
                                else
                                {
                                    box.Text("7ANRPS48").FontSize(14).Bold().FontColor("#313511");
                                }
                            });
                        });

                        column.Item().PaddingTop(12).LineHorizontal(1).LineColor("#e5e2dc");
                    });

                    page.Content().PaddingVertical(16).Column(column =>
                    {
                        column.Spacing(14);

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Element(box => PdfKpi(box, L("Admin_Revenue"), Money(model.Ciro), L("Admin_ReportRevenueSubtitle")));
                            row.RelativeItem().Element(box => PdfKpi(box, L("Admin_Order"), model.SiparisSayisi.ToString(), L("Admin_ReportAverageCartSubtitle", Money(model.OrtalamaSepet))));
                        });
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Element(box => PdfKpi(box, L("Admin_Conversion"), $"%{model.DonusumOrani:N2}", L("Admin_UniqueVisitorsCount", model.TekilZiyaretciSayisi)));
                            row.RelativeItem().Element(box => PdfKpi(box, L("Admin_AbandonedCarts"), Money(model.TerkEdilenSepetTutari), L("Admin_CartsCount", model.TerkEdilenSepetSayisi)));
                        });
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Element(box => PdfKpi(box, L("Admin_NewCustomer"), model.YeniMusteriSayisi.ToString(), L("Admin_ReportRepeatCustomerSubtitle", model.TekrarMusteriSayisi)));
                            row.RelativeItem().Element(box => PdfKpi(box, L("Admin_ReturnCancel"), $"{model.IadeTalebiSayisi} / {model.IptalSiparisSayisi}", L("Admin_ReportOperationsTrackingSubtitle")));
                        });

                        PdfSection(column, L("Admin_ActionInsights"), table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(3);
                            });
                            PdfHeader(table, L("Admin_ReportTitleColumn"));
                            PdfHeader(table, L("Admin_Aciklama"));
                            foreach (var item in model.Oneriler.Take(5))
                            {
                                PdfCell(table, item.Baslik);
                                PdfCell(table, item.Aciklama);
                            }
                        });

                        PdfSection(column, L("Admin_HighClicksLowSales"), table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });
                            PdfHeader(table, L("Admin_Urun"));
                            PdfHeader(table, L("Admin_Clicks"));
                            PdfHeader(table, L("Admin_Sales"));
                            PdfHeader(table, L("Admin_Risk"));
                            foreach (var item in model.UrunDonusumSorunlari.Take(8))
                            {
                                PdfCell(table, item.UrunAdi);
                                PdfCell(table, item.Goruntulenme.ToString());
                                PdfCell(table, item.SatisAdedi.ToString());
                                PdfCell(table, item.RiskNotu);
                            }
                        });

                        PdfSection(column, L("Admin_MostValuableCustomers"), table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });
                            PdfHeader(table, L("Admin_Musteri"));
                            PdfHeader(table, L("Admin_Email"));
                            PdfHeader(table, L("Admin_Order"));
                            PdfHeader(table, L("Admin_Revenue"));
                            foreach (var item in model.EnDegerliMusteriler.Take(8))
                            {
                                PdfCell(table, item.Musteri);
                                PdfCell(table, item.Eposta);
                                PdfCell(table, item.SiparisAdedi.ToString());
                                PdfCell(table, Money(item.Ciro));
                            }
                        });

                        PdfSection(column, L("Admin_ReturnCancelReasons"), table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });
                            PdfHeader(table, L("Admin_Reason"));
                            PdfHeader(table, L("Admin_Type"));
                            PdfHeader(table, L("Admin_Quantity"));
                            PdfHeader(table, L("Admin_Amount"));
                            foreach (var item in model.IadeIptalNedenleri.Take(8))
                            {
                                PdfCell(table, item.Neden);
                                PdfCell(table, item.Tip);
                                PdfCell(table, item.Adet.ToString());
                                PdfCell(table, Money(item.Tutar));
                            }
                        });
                    });

                    page.Footer()
                        .AlignRight()
                        .Text(text =>
                        {
                            text.Span(L("Admin_ReportPagePrefix"));
                            text.CurrentPageNumber();
                            text.Span(" / ");
                            text.TotalPages();
                        });
                });
            }).GeneratePdf();

            return File(pdfBytes, "application/pdf", L("Admin_ReportPdfFileName", startLocal, endLocal));
        }

        private async Task<RaporIndexViewModel> BuildReportAsync(DateTime startUtc, DateTime endUtc, DateTime startLocal, DateTime endLocal)
        {
            var periodDays = Math.Max(1, (endUtc - startUtc).Days);
            var previousStartUtc = startUtc.AddDays(-periodDays);
            var previousEndUtc = startUtc;

            var orders = await _context.Siparisler
                .AsNoTracking()
                .Include(x => x.SiparisDetaylari)
                    .ThenInclude(x => x.Urun)
                        .ThenInclude(x => x.Kategori)
                .Where(x => x.OlusturulmaTarihi >= startUtc && x.OlusturulmaTarihi < endUtc)
                .ToListAsync();

            var previousOrders = await _context.Siparisler
                .AsNoTracking()
                .Where(x => x.OlusturulmaTarihi >= previousStartUtc && x.OlusturulmaTarihi < previousEndUtc)
                .ToListAsync();

            var allCustomerOrders = await _context.Siparisler
                .AsNoTracking()
                .Where(x => x.Durum != SiparisDurumHelper.IptalEdildi &&
                            x.Durum != SiparisDurumHelper.IadeTalebi &&
                            x.Durum != SiparisDurumHelper.IadeOnaylandi &&
                            x.Durum != SiparisDurumHelper.IadeTamamlandi)
                .Select(x => new
                {
                    x.Id,
                    x.AppUserId,
                    x.Eposta,
                    x.MusteriAdSoyad,
                    x.Sehir,
                    x.ToplamTutar,
                    x.OlusturulmaTarihi
                })
                .ToListAsync();

            var visitorLogs = await _context.ZiyaretciLoglari
                .AsNoTracking()
                .Where(x => x.OlusturulmaTarihi >= startUtc && x.OlusturulmaTarihi < endUtc)
                .ToListAsync();

            var abandonedCarts = await _context.Sepetler
                .AsNoTracking()
                .Include(x => x.SepetItems)
                .Where(x => x.TerkEdildi && x.TerkEdilmeTarihi >= startUtc && x.TerkEdilmeTarihi < endUtc)
                .ToListAsync();

            var returns = await _context.IadeTalepleri
                .AsNoTracking()
                .Include(x => x.Siparis)
                .Where(x => x.OlusturulmaTarihi >= startUtc && x.OlusturulmaTarihi < endUtc)
                .ToListAsync();

            var allProducts = await _context.Urunler
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Where(x => !x.SilindiMi)
                .Select(x => new
                {
                    x.Id,
                    x.Baslik,
                    x.AnaGorselUrl,
                    x.GoruntulenmeSayisi,
                    x.FavoriSayisi
                })
                .ToListAsync();

            var revenueOrders = orders.Where(IsRevenueOrder).ToList();
            var previousRevenueOrders = previousOrders.Where(IsRevenueOrder).ToList();
            var allDetails = revenueOrders.SelectMany(x => x.SiparisDetaylari).ToList();
            var salesByProduct = allDetails
                .GroupBy(x => x.UrunId)
                .ToDictionary(
                    x => x.Key,
                    x => new
                    {
                        Adet = x.Sum(i => i.Adet),
                        Ciro = x.Sum(i => i.BirimFiyat * i.Adet)
                    });

            var periodCustomerKeys = revenueOrders.Select(GetCustomerKey).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            var firstOrderByCustomer = allCustomerOrders
                .GroupBy(x => !string.IsNullOrWhiteSpace(x.AppUserId)
                    ? $"u:{x.AppUserId.Trim()}"
                    : string.IsNullOrWhiteSpace(x.Eposta)
                        ? string.Empty
                        : $"e:{x.Eposta.Trim().ToLowerInvariant()}")
                .Where(x => !string.IsNullOrWhiteSpace(x.Key))
                .ToDictionary(x => x.Key, x => x.Min(o => o.OlusturulmaTarihi), StringComparer.OrdinalIgnoreCase);
            var periodOrderCountByCustomer = revenueOrders
                .GroupBy(GetCustomerKey)
                .Where(x => !string.IsNullOrWhiteSpace(x.Key))
                .ToDictionary(x => x.Key, x => x.Count(), StringComparer.OrdinalIgnoreCase);
            var newCustomerKeys = periodCustomerKeys
                .Where(x => firstOrderByCustomer.TryGetValue(x, out var firstOrderDate) && firstOrderDate >= startUtc && firstOrderDate < endUtc)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            var repeatCustomerKeys = periodCustomerKeys
                .Where(x => (firstOrderByCustomer.TryGetValue(x, out var firstOrderDate) && firstOrderDate < startUtc) ||
                            (periodOrderCountByCustomer.TryGetValue(x, out var count) && count > 1))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var topSelling = allDetails
                .GroupBy(x => new
                {
                    x.UrunId,
                    UrunAdi = string.IsNullOrWhiteSpace(x.Urun?.Baslik) ? L("Admin_ReportProductFallback", x.UrunId) : x.Urun.Baslik,
                    Gorsel = x.Urun?.AnaGorselUrl ?? string.Empty
                })
                .Select(g => new RaporProductMetric
                {
                    UrunId = g.Key.UrunId,
                    UrunAdi = g.Key.UrunAdi,
                    GorselUrl = g.Key.Gorsel,
                    Adet = g.Sum(x => x.Adet),
                    Ciro = g.Sum(x => x.BirimFiyat * x.Adet),
                    Goruntulenme = allProducts.FirstOrDefault(p => p.Id == g.Key.UrunId)?.GoruntulenmeSayisi ?? 0,
                    Favori = allProducts.FirstOrDefault(p => p.Id == g.Key.UrunId)?.FavoriSayisi ?? 0
                })
                .OrderByDescending(x => x.Ciro)
                .ThenByDescending(x => x.Adet)
                .Take(10)
                .ToList();

            var topClicked = allProducts
                .OrderByDescending(x => x.GoruntulenmeSayisi)
                .ThenByDescending(x => x.FavoriSayisi)
                .Take(10)
                .Select(x =>
                {
                    salesByProduct.TryGetValue(x.Id, out var sales);
                    return new RaporProductMetric
                    {
                        UrunId = x.Id,
                        UrunAdi = x.Baslik,
                        GorselUrl = x.AnaGorselUrl,
                        Goruntulenme = x.GoruntulenmeSayisi,
                        Favori = x.FavoriSayisi,
                        Adet = sales?.Adet ?? 0,
                        Ciro = sales?.Ciro ?? 0
                    };
                })
                .ToList();

            var conversionRisk = allProducts
                .Where(x => x.GoruntulenmeSayisi > 0)
                .Select(x =>
                {
                    salesByProduct.TryGetValue(x.Id, out var sales);
                    var sold = sales?.Adet ?? 0;
                    var conversion = x.GoruntulenmeSayisi == 0 ? 0 : Math.Round(sold * 100m / x.GoruntulenmeSayisi, 2);
                    return new RaporProductConversionMetric
                    {
                        UrunId = x.Id,
                        UrunAdi = x.Baslik,
                        GorselUrl = x.AnaGorselUrl,
                        Goruntulenme = x.GoruntulenmeSayisi,
                        SatisAdedi = sold,
                        Ciro = sales?.Ciro ?? 0,
                        DonusumOrani = conversion,
                        RiskNotu = sold == 0
                            ? L("Admin_ReportRiskClicksNoSales")
                            : conversion < 1
                                ? L("Admin_ReportRiskLowConversion")
                                : L("Admin_ReportRiskMonitor")
                    };
                })
                .Where(x => x.SatisAdedi == 0 || x.DonusumOrani < 1)
                .OrderByDescending(x => x.Goruntulenme)
                .ThenBy(x => x.DonusumOrani)
                .Take(12)
                .ToList();

            var customerReport = revenueOrders
                .GroupBy(GetCustomerKey)
                .Where(x => !string.IsNullOrWhiteSpace(x.Key))
                .Select(g =>
                {
                    var lastOrder = g.OrderByDescending(x => x.OlusturulmaTarihi).First();
                    var key = g.Key;
                    return new RaporCustomerMetric
                    {
                        Musteri = string.IsNullOrWhiteSpace(lastOrder.MusteriAdSoyad) ? L("Admin_ReportUnnamedCustomer") : lastOrder.MusteriAdSoyad,
                        Eposta = string.IsNullOrWhiteSpace(lastOrder.Eposta) ? "-" : lastOrder.Eposta,
                        Sehir = string.IsNullOrWhiteSpace(lastOrder.Sehir) ? "-" : lastOrder.Sehir,
                        SiparisAdedi = g.Count(),
                        Ciro = g.Sum(x => x.ToplamTutar),
                        SonSiparisTarihi = g.Max(x => x.OlusturulmaTarihi),
                        YeniMusteri = newCustomerKeys.Contains(key)
                    };
                })
                .OrderByDescending(x => x.Ciro)
                .Take(12)
                .ToList();

            var cancelledReasons = orders
                .Where(x => x.Durum == SiparisDurumHelper.IptalEdildi)
                .GroupBy(x => NormalizeReason(x.Aciklama, L("Admin_ReportReasonNotProvided")))
                .Select(g => new RaporReturnReasonMetric
                {
                    Neden = g.Key,
                    Tip = L("Admin_ReportCancellationType"),
                    IadeMi = false,
                    Adet = g.Count(),
                    Tutar = g.Sum(x => x.ToplamTutar)
                });
            var returnReasons = returns
                .GroupBy(x => NormalizeReason(x.Neden, L("Admin_ReportReasonNotProvided")))
                .Select(g => new RaporReturnReasonMetric
                {
                    Neden = g.Key,
                    Tip = L("Admin_ReportReturnType"),
                    IadeMi = true,
                    Adet = g.Count(),
                    Tutar = g.Sum(x => x.Siparis?.ToplamTutar ?? 0)
                });

            var model = new RaporIndexViewModel
            {
                Baslangic = startLocal,
                Bitis = endLocal.AddDays(-1),
                AralikEtiketi = $"{startLocal:dd.MM.yyyy} - {endLocal.AddDays(-1):dd.MM.yyyy}",
                Ciro = revenueOrders.Sum(x => x.ToplamTutar),
                OncekiCiro = previousRevenueOrders.Sum(x => x.ToplamTutar),
                IndirimToplami = revenueOrders.Sum(x => x.IndirimTutari),
                OrtalamaSepet = revenueOrders.Count == 0 ? 0 : revenueOrders.Average(x => x.ToplamTutar),
                SiparisSayisi = revenueOrders.Count,
                OncekiSiparisSayisi = previousRevenueOrders.Count,
                SatilanUrunAdedi = allDetails.Sum(x => x.Adet),
                TekilMusteriSayisi = periodCustomerKeys.Count,
                YeniMusteriSayisi = newCustomerKeys.Count,
                TekrarMusteriSayisi = repeatCustomerKeys.Count,
                TekrarMusteriCirosu = revenueOrders.Where(x => repeatCustomerKeys.Contains(GetCustomerKey(x))).Sum(x => x.ToplamTutar),
                ZiyaretSayisi = visitorLogs.Count,
                TekilZiyaretciSayisi = visitorLogs.Select(x => x.IpAdresi).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().Count(),
                BekleyenSiparisSayisi = orders.Count(x => x.Durum == SiparisDurumHelper.SiparisAlindi),
                KargoyaHazirSiparisSayisi = orders.Count(x => x.Durum == SiparisDurumHelper.Paketleniyor),
                IadeTalebiSayisi = returns.Count,
                IptalSiparisSayisi = orders.Count(x => x.Durum == SiparisDurumHelper.IptalEdildi),
                TerkEdilenSepetSayisi = abandonedCarts.Count,
                TerkEdilenSepetTutari = abandonedCarts.Sum(x => x.SepetItems.Sum(i => i.Fiyat * i.Adet)),
                GunlukMetrikler = BuildDailyMetrics(startLocal, endLocal, revenueOrders, visitorLogs),
                SaatlikMetrikler = BuildHourlyMetrics(revenueOrders, visitorLogs),
                DurumDagilimi = orders
                    .GroupBy(x => x.Durum)
                    .Select(g => new RaporStatusMetric
                    {
                        Durum = g.Key,
                        Etiket = GetOrderStatusLabel(g.Key),
                        Adet = g.Count(),
                        Tutar = g.Where(IsRevenueOrder).Sum(x => x.ToplamTutar)
                    })
                    .OrderByDescending(x => x.Adet)
                    .ToList(),
                EnCokSatanUrunler = topSelling,
                EnCokTiklananUrunler = topClicked,
                UrunDonusumSorunlari = conversionRisk,
                EnDegerliMusteriler = customerReport,
                IadeIptalNedenleri = returnReasons.Concat(cancelledReasons)
                    .OrderByDescending(x => x.Adet)
                    .ThenByDescending(x => x.Tutar)
                    .Take(12)
                    .ToList(),
                KategoriPerformansi = allDetails
                    .GroupBy(x => new
                    {
                        KategoriId = x.Urun?.KategoriId ?? 0,
                        KategoriAdi = x.Urun?.Kategori?.Ad ?? L("Admin_ReportUncategorized")
                    })
                    .Select(g => new RaporCategoryMetric
                    {
                        KategoriId = g.Key.KategoriId,
                        KategoriAdi = g.Key.KategoriAdi,
                        UrunAdedi = g.Select(x => x.UrunId).Distinct().Count(),
                        SiparisAdedi = g.Sum(x => x.Adet),
                        Ciro = g.Sum(x => x.BirimFiyat * x.Adet)
                    })
                    .OrderByDescending(x => x.Ciro)
                    .Take(10)
                    .ToList(),
                SehirPerformansi = revenueOrders
                    .GroupBy(x => string.IsNullOrWhiteSpace(x.Sehir) ? L("Admin_Belirtilmemis") : x.Sehir.Trim())
                    .Select(g => new RaporCityMetric
                    {
                        Sehir = g.Key,
                        SiparisAdedi = g.Count(),
                        Ciro = g.Sum(x => x.ToplamTutar)
                    })
                    .OrderByDescending(x => x.Ciro)
                    .Take(10)
                    .ToList(),
                KuponPerformansi = revenueOrders
                    .Where(x => !string.IsNullOrWhiteSpace(x.KuponKodu))
                    .GroupBy(x => x.KuponKodu!.Trim().ToUpperInvariant())
                    .Select(g => new RaporCouponMetric
                    {
                        Kod = g.Key,
                        Kullanim = g.Count(),
                        Indirim = g.Sum(x => x.IndirimTutari),
                        Ciro = g.Sum(x => x.ToplamTutar)
                    })
                    .OrderByDescending(x => x.Kullanim)
                    .Take(10)
                    .ToList(),
                TrafikKaynaklari = visitorLogs
                    .GroupBy(x => NormalizeReferer(x.ReferansUrl))
                    .Select(g => new RaporTrafficMetric
                    {
                        Etiket = g.Key,
                        Adet = g.Count(),
                        Tekil = g.Select(x => x.IpAdresi).Distinct().Count()
                    })
                    .OrderByDescending(x => x.Adet)
                    .Take(10)
                    .ToList(),
                EnCokGezilenSayfalar = visitorLogs
                    .Where(x => string.Equals(x.Metod, "GET", StringComparison.OrdinalIgnoreCase))
                    .GroupBy(x => string.IsNullOrWhiteSpace(x.Url) ? "/" : x.Url)
                    .Select(g => new RaporTrafficMetric
                    {
                        Etiket = g.Key,
                        Adet = g.Count(),
                        Tekil = g.Select(x => x.IpAdresi).Distinct().Count()
                    })
                    .OrderByDescending(x => x.Adet)
                    .Take(12)
                    .ToList(),
                CihazDagilimi = visitorLogs
                    .GroupBy(x => NormalizeDevice(x.CihazModeli, x.IsletimSistemi))
                    .Select(g => new RaporTrafficMetric
                    {
                        Etiket = g.Key,
                        Adet = g.Count(),
                        Tekil = g.Select(x => x.IpAdresi).Distinct().Count()
                    })
                    .OrderByDescending(x => x.Adet)
                    .ToList(),
                KargoPerformansi = orders
                    .GroupBy(x => string.IsNullOrWhiteSpace(x.KargoFirmasi) ? L("Admin_ReportShippingCompanyNotSelected") : x.KargoFirmasi.Trim())
                    .Select(g => new RaporKargoMetric
                    {
                        Firma = g.Key,
                        SiparisAdedi = g.Count(),
                        Kargoda = g.Count(x => x.Durum == SiparisDurumHelper.KargoyaVerildi),
                        Teslim = g.Count(x => x.Durum == SiparisDurumHelper.TeslimEdildi)
                    })
                    .OrderByDescending(x => x.SiparisAdedi)
                    .ToList()
            };

            model.DonusumOrani = model.TekilZiyaretciSayisi == 0
                ? 0
                : Math.Round(model.SiparisSayisi * 100m / model.TekilZiyaretciSayisi, 2);
            model.Oneriler = BuildInsights(model);

            return model;
        }

        private static IReadOnlyList<RaporDailyMetric> BuildDailyMetrics(DateTime startLocal, DateTime endLocal, IReadOnlyList<Siparis> orders, IReadOnlyList<ZiyaretciLog> visitorLogs)
        {
            var days = new List<RaporDailyMetric>();
            for (var day = startLocal.Date; day < endLocal.Date; day = day.AddDays(1))
            {
                var nextDay = day.AddDays(1);
                days.Add(new RaporDailyMetric
                {
                    Tarih = day,
                    Ciro = orders.Where(x => ToPalestineLocal(x.OlusturulmaTarihi) >= day && ToPalestineLocal(x.OlusturulmaTarihi) < nextDay).Sum(x => x.ToplamTutar),
                    Siparis = orders.Count(x => ToPalestineLocal(x.OlusturulmaTarihi) >= day && ToPalestineLocal(x.OlusturulmaTarihi) < nextDay),
                    Ziyaret = visitorLogs.Count(x => ToPalestineLocal(x.OlusturulmaTarihi) >= day && ToPalestineLocal(x.OlusturulmaTarihi) < nextDay)
                });
            }

            return days;
        }

        private static IReadOnlyList<RaporHourlyMetric> BuildHourlyMetrics(IReadOnlyList<Siparis> orders, IReadOnlyList<ZiyaretciLog> visitorLogs)
        {
            return Enumerable.Range(0, 24)
                .Select(hour => new RaporHourlyMetric
                {
                    Saat = hour,
                    Siparis = orders.Count(x => ToPalestineLocal(x.OlusturulmaTarihi).Hour == hour),
                    Ciro = orders.Where(x => ToPalestineLocal(x.OlusturulmaTarihi).Hour == hour).Sum(x => x.ToplamTutar),
                    Ziyaret = visitorLogs.Count(x => ToPalestineLocal(x.OlusturulmaTarihi).Hour == hour)
                })
                .ToList();
        }

        private IReadOnlyList<RaporInsightItem> BuildInsights(RaporIndexViewModel model)
        {
            var insights = new List<RaporInsightItem>();

            if (model.BekleyenSiparisSayisi > 0)
            {
                insights.Add(new RaporInsightItem
                {
                    Seviye = "warning",
                    Baslik = L("Admin_ReportInsightPendingOrdersTitle"),
                    Aciklama = L("Admin_ReportInsightPendingOrdersDescription", model.BekleyenSiparisSayisi),
                    Link = "/Admin/Siparis?durum=0"
                });
            }

            if (model.UrunDonusumSorunlari.Count > 0)
            {
                var risk = model.UrunDonusumSorunlari.First();
                insights.Add(new RaporInsightItem
                {
                    Seviye = "warning",
                    Baslik = L("Admin_ReportInsightConversionRiskTitle"),
                    Aciklama = L("Admin_ReportInsightConversionRiskDescription", risk.UrunAdi),
                    Link = $"/Admin/Urun/Duzenle/{risk.UrunId}"
                });
            }

            if (model.TerkEdilenSepetSayisi > 0)
            {
                insights.Add(new RaporInsightItem
                {
                    Seviye = "info",
                    Baslik = L("Admin_ReportInsightAbandonedCartTitle"),
                    Aciklama = L("Admin_ReportInsightAbandonedCartDescription", model.TerkEdilenSepetSayisi, Money(model.TerkEdilenSepetTutari)),
                    Link = "/Admin/Rapor"
                });
            }

            if (model.IadeTalebiSayisi + model.IptalSiparisSayisi > 0)
            {
                insights.Add(new RaporInsightItem
                {
                    Seviye = "info",
                    Baslik = L("Admin_ReportInsightReturnCancelTitle"),
                    Aciklama = L("Admin_ReportInsightReturnCancelDescription", model.IadeTalebiSayisi, model.IptalSiparisSayisi),
                    Link = "/Admin/Iade"
                });
            }

            if (model.DonusumOrani > 0 && model.DonusumOrani < 1)
            {
                insights.Add(new RaporInsightItem
                {
                    Seviye = "info",
                    Baslik = L("Admin_ReportInsightLowConversionTitle"),
                    Aciklama = L("Admin_ReportInsightLowConversionDescription", model.DonusumOrani),
                    Link = "/Admin/Kupon"
                });
            }

            if (insights.Count == 0)
            {
                insights.Add(new RaporInsightItem
                {
                    Seviye = "success",
                    Baslik = L("Admin_ReportInsightNormalTitle"),
                    Aciklama = L("Admin_ReportInsightNormalDescription"),
                    Link = "/Admin/Siparis"
                });
            }

            return insights;
        }

        private static (DateTime StartUtc, DateTime EndUtc, DateTime StartLocal, DateTime EndLocal) ResolveDateRange(DateTime? baslangic, DateTime? bitis)
        {
            var todayLocal = DateTime.UtcNow.AddHours(3).Date;
            var startLocal = baslangic?.Date ?? todayLocal.AddDays(-29);
            var endLocalInclusive = bitis?.Date ?? todayLocal;

            if (endLocalInclusive < startLocal)
            {
                (startLocal, endLocalInclusive) = (endLocalInclusive, startLocal);
            }

            var endLocalExclusive = endLocalInclusive.AddDays(1);
            var startUtc = DateTime.SpecifyKind(startLocal.AddHours(-3), DateTimeKind.Utc);
            var endUtc = DateTime.SpecifyKind(endLocalExclusive.AddHours(-3), DateTimeKind.Utc);
            return (startUtc, endUtc, startLocal, endLocalExclusive);
        }

        private static bool IsRevenueOrder(Siparis order)
        {
            return !SiparisDurumHelper.IsCancelled(order.Durum) && !SiparisDurumHelper.IsReturn(order.Durum);
        }

        private static DateTime ToPalestineLocal(DateTime value)
        {
            return value.Kind == DateTimeKind.Utc ? value.AddHours(3) : value;
        }

        private static string GetCustomerKey(Siparis order)
        {
            if (!string.IsNullOrWhiteSpace(order.AppUserId))
            {
                return $"u:{order.AppUserId.Trim()}";
            }

            return string.IsNullOrWhiteSpace(order.Eposta) ? string.Empty : $"e:{order.Eposta.Trim().ToLowerInvariant()}";
        }

        private string NormalizeReferer(string? referer)
        {
            if (string.IsNullOrWhiteSpace(referer))
            {
                return L("Admin_ReportDirectUnknown");
            }

            if (Uri.TryCreate(referer, UriKind.Absolute, out var uri))
            {
                return uri.Host.Replace("www.", string.Empty);
            }

            return referer.Length > 40 ? referer[..40] : referer;
        }

        private string NormalizeDevice(string? model, string? os)
        {
            var text = $"{model} {os}".ToLowerInvariant();
            if (text.Contains("iphone") || text.Contains("android") || text.Contains("mobile"))
            {
                return L("Admin_ReportMobile");
            }

            if (text.Contains("ipad") || text.Contains("tablet"))
            {
                return L("Admin_ReportTablet");
            }

            return L("Admin_ReportDesktop");
        }

        private static string NormalizeReason(string? value, string fallback)
        {
            var text = value?.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                return fallback;
            }

            return text.Length > 80 ? text[..80] : text;
        }

        private string GetOrderStatusLabel(int status) => status switch
        {
            SiparisDurumHelper.SiparisAlindi => L("Admin_ReportStatusOrderReceived"),
            SiparisDurumHelper.UretimHazirlaniyor => L("Admin_ReportStatusPreparing"),
            SiparisDurumHelper.Paketleniyor => L("Admin_ReportStatusPacking"),
            SiparisDurumHelper.KargoyaVerildi => L("Admin_ReportStatusShipped"),
            SiparisDurumHelper.TeslimEdildi => L("Admin_ReportStatusDelivered"),
            SiparisDurumHelper.IptalEdildi => L("Admin_ReportStatusCancelled"),
            SiparisDurumHelper.IadeTalebi => L("Admin_ReportStatusReturnRequested"),
            SiparisDurumHelper.IadeOnaylandi => L("Admin_ReportStatusReturnApproved"),
            SiparisDurumHelper.IadeTamamlandi => L("Admin_ReportStatusReturnCompleted"),
            _ => L("Admin_ReportStatusUpdated")
        };

        private string L(string key, params object[] arguments) => _localizer[key, arguments].Value;

        private static string Money(decimal value)
        {
            return $"{value:N2} ?";
        }

        private void AddSummarySheet(ExcelPackage package, RaporIndexViewModel model)
        {
            var summary = package.Workbook.Worksheets.Add(L("Admin_ReportSheetSummary"));
            var summaryRows = new (string Label, object Value)[]
            {
                (L("Admin_ReportRange"), model.AralikEtiketi),
                (L("Admin_Revenue"), model.Ciro),
                (L("Admin_ReportOrderCount"), model.SiparisSayisi),
                (L("Admin_AverageCartValue"), model.OrtalamaSepet),
                (L("Admin_ReportProductsSoldCount"), model.SatilanUrunAdedi),
                (L("Admin_ReportUniqueCustomers"), model.TekilMusteriSayisi),
                (L("Admin_NewCustomer"), model.YeniMusteriSayisi),
                (L("Admin_RepeatCustomer"), model.TekrarMusteriSayisi),
                (L("Admin_RepeatCustomerRevenue"), model.TekrarMusteriCirosu),
                (L("Admin_ReportVisits"), model.ZiyaretSayisi),
                (L("Admin_ReportUniqueVisitor"), model.TekilZiyaretciSayisi),
                (L("Admin_ReportConversionRate"), $"{model.DonusumOrani:N2}%"),
                (L("Admin_TotalDiscount"), model.IndirimToplami),
                (L("Admin_ReportReturnRequests"), model.IadeTalebiSayisi),
                (L("Admin_ReportCancelledOrders"), model.IptalSiparisSayisi),
                (L("Admin_AbandonedCarts"), model.TerkEdilenSepetSayisi),
                (L("Admin_ReportAbandonedCartAmount"), model.TerkEdilenSepetTutari)
            };

            for (var i = 0; i < summaryRows.Length; i++)
            {
                summary.Cells[i + 1, 1].Value = summaryRows[i].Label;
                summary.Cells[i + 1, 2].Value = summaryRows[i].Value;
            }

            StyleKeyValueSheet(summary, summaryRows.Length);
        }

        private void AddDailySheet(ExcelPackage package, RaporIndexViewModel model)
        {
            var ws = package.Workbook.Worksheets.Add(L("Admin_ReportSheetDaily"));
            WriteHeaders(ws, L("Admin_Date"), L("Admin_Revenue"), L("Admin_Order"), L("Admin_ReportVisits"));
            var row = 2;
            foreach (var item in model.GunlukMetrikler)
            {
                ws.Cells[row, 1].Value = item.Tarih;
                ws.Cells[row, 2].Value = item.Ciro;
                ws.Cells[row, 3].Value = item.Siparis;
                ws.Cells[row, 4].Value = item.Ziyaret;
                row++;
            }
            ws.Column(1).Style.Numberformat.Format = "dd.mm.yyyy";
            ws.Column(2).Style.Numberformat.Format = "#,##0.00";
            AutoFit(ws);
        }

        private void AddHourlySheet(ExcelPackage package, RaporIndexViewModel model)
        {
            var ws = package.Workbook.Worksheets.Add(L("Admin_ReportSheetHourlyAnalysis"));
            WriteHeaders(ws, L("Admin_ReportHour"), L("Admin_Order"), L("Admin_Revenue"), L("Admin_ReportVisits"));
            var row = 2;
            foreach (var item in model.SaatlikMetrikler)
            {
                ws.Cells[row, 1].Value = $"{item.Saat:00}:00";
                ws.Cells[row, 2].Value = item.Siparis;
                ws.Cells[row, 3].Value = item.Ciro;
                ws.Cells[row, 4].Value = item.Ziyaret;
                row++;
            }
            ws.Column(3).Style.Numberformat.Format = "#,##0.00";
            AutoFit(ws);
        }

        private void AddStatusSheet(ExcelPackage package, RaporIndexViewModel model)
        {
            var ws = package.Workbook.Worksheets.Add(L("Admin_ReportSheetOrderStatuses"));
            WriteHeaders(ws, L("Admin_Durum"), L("Admin_Quantity"), L("Admin_Amount"));
            var row = 2;
            foreach (var item in model.DurumDagilimi)
            {
                ws.Cells[row, 1].Value = item.Etiket;
                ws.Cells[row, 2].Value = item.Adet;
                ws.Cells[row, 3].Value = item.Tutar;
                row++;
            }
            ws.Column(3).Style.Numberformat.Format = "#,##0.00";
            AutoFit(ws);
        }

        private void AddProductSheet(ExcelPackage package, string title, IReadOnlyList<RaporProductMetric> items)
        {
            var ws = package.Workbook.Worksheets.Add(title);
            WriteHeaders(ws, L("Admin_ReportProductId"), L("Admin_Urun"), L("Admin_ReportSalesQuantity"), L("Admin_Revenue"), L("Admin_ReportViews"), L("Admin_Favorite"));
            var row = 2;
            foreach (var item in items)
            {
                ws.Cells[row, 1].Value = item.UrunId;
                ws.Cells[row, 2].Value = item.UrunAdi;
                ws.Cells[row, 3].Value = item.Adet;
                ws.Cells[row, 4].Value = item.Ciro;
                ws.Cells[row, 5].Value = item.Goruntulenme;
                ws.Cells[row, 6].Value = item.Favori;
                row++;
            }
            ws.Column(4).Style.Numberformat.Format = "#,##0.00";
            AutoFit(ws);
        }

        private void AddConversionSheet(ExcelPackage package, RaporIndexViewModel model)
        {
            var ws = package.Workbook.Worksheets.Add(L("Admin_ReportSheetProductConversionRisk"));
            WriteHeaders(ws, L("Admin_ReportProductId"), L("Admin_Urun"), L("Admin_ReportViews"), L("Admin_Sales"), L("Admin_Revenue"), L("Admin_ReportConversionPercent"), L("Admin_ReportRiskNote"));
            var row = 2;
            foreach (var item in model.UrunDonusumSorunlari)
            {
                ws.Cells[row, 1].Value = item.UrunId;
                ws.Cells[row, 2].Value = item.UrunAdi;
                ws.Cells[row, 3].Value = item.Goruntulenme;
                ws.Cells[row, 4].Value = item.SatisAdedi;
                ws.Cells[row, 5].Value = item.Ciro;
                ws.Cells[row, 6].Value = item.DonusumOrani;
                ws.Cells[row, 7].Value = item.RiskNotu;
                row++;
            }
            ws.Column(5).Style.Numberformat.Format = "#,##0.00";
            ws.Column(6).Style.Numberformat.Format = "0.00";
            AutoFit(ws);
        }

        private void AddCustomerSheet(ExcelPackage package, RaporIndexViewModel model)
        {
            var ws = package.Workbook.Worksheets.Add(L("Admin_ReportSheetCustomers"));
            WriteHeaders(ws, L("Admin_Musteri"), L("Admin_Email"), L("Admin_Sehir"), L("Admin_Order"), L("Admin_Revenue"), L("Admin_ReportLastOrder"), L("Admin_Type"));
            var row = 2;
            foreach (var item in model.EnDegerliMusteriler)
            {
                ws.Cells[row, 1].Value = item.Musteri;
                ws.Cells[row, 2].Value = item.Eposta;
                ws.Cells[row, 3].Value = item.Sehir;
                ws.Cells[row, 4].Value = item.SiparisAdedi;
                ws.Cells[row, 5].Value = item.Ciro;
                ws.Cells[row, 6].Value = ToPalestineLocal(item.SonSiparisTarihi);
                ws.Cells[row, 7].Value = item.YeniMusteri ? L("Admin_New") : L("Admin_Repeat");
                row++;
            }
            ws.Column(5).Style.Numberformat.Format = "#,##0.00";
            ws.Column(6).Style.Numberformat.Format = "dd.mm.yyyy hh:mm";
            AutoFit(ws);
        }

        private void AddReturnReasonSheet(ExcelPackage package, RaporIndexViewModel model)
        {
            var ws = package.Workbook.Worksheets.Add(L("Admin_ReportSheetReturnCancelReasons"));
            WriteHeaders(ws, L("Admin_Reason"), L("Admin_Type"), L("Admin_Quantity"), L("Admin_Amount"));
            var row = 2;
            foreach (var item in model.IadeIptalNedenleri)
            {
                ws.Cells[row, 1].Value = item.Neden;
                ws.Cells[row, 2].Value = item.Tip;
                ws.Cells[row, 3].Value = item.Adet;
                ws.Cells[row, 4].Value = item.Tutar;
                row++;
            }
            ws.Column(4).Style.Numberformat.Format = "#,##0.00";
            AutoFit(ws);
        }

        private void AddCategorySheet(ExcelPackage package, RaporIndexViewModel model)
        {
            var ws = package.Workbook.Worksheets.Add(L("Admin_ReportSheetCategoryPerformance"));
            WriteHeaders(ws, L("Admin_ReportCategoryId"), L("Admin_Kategori"), L("Admin_ReportProductCount"), L("Admin_ReportSalesQuantity"), L("Admin_Revenue"));
            var row = 2;
            foreach (var item in model.KategoriPerformansi)
            {
                ws.Cells[row, 1].Value = item.KategoriId;
                ws.Cells[row, 2].Value = item.KategoriAdi;
                ws.Cells[row, 3].Value = item.UrunAdedi;
                ws.Cells[row, 4].Value = item.SiparisAdedi;
                ws.Cells[row, 5].Value = item.Ciro;
                row++;
            }
            ws.Column(5).Style.Numberformat.Format = "#,##0.00";
            AutoFit(ws);
        }

        private void AddCitySheet(ExcelPackage package, RaporIndexViewModel model)
        {
            var ws = package.Workbook.Worksheets.Add(L("Admin_ReportSheetCities"));
            WriteHeaders(ws, L("Admin_Sehir"), L("Admin_Order"), L("Admin_Revenue"));
            var row = 2;
            foreach (var item in model.SehirPerformansi)
            {
                ws.Cells[row, 1].Value = item.Sehir;
                ws.Cells[row, 2].Value = item.SiparisAdedi;
                ws.Cells[row, 3].Value = item.Ciro;
                row++;
            }
            ws.Column(3).Style.Numberformat.Format = "#,##0.00";
            AutoFit(ws);
        }

        private void AddCouponSheet(ExcelPackage package, RaporIndexViewModel model)
        {
            var ws = package.Workbook.Worksheets.Add(L("Admin_ReportSheetCoupons"));
            WriteHeaders(ws, L("Admin_ReportCoupon"), L("Admin_Usage"), L("Admin_Discount"), L("Admin_Revenue"));
            var row = 2;
            foreach (var item in model.KuponPerformansi)
            {
                ws.Cells[row, 1].Value = item.Kod;
                ws.Cells[row, 2].Value = item.Kullanim;
                ws.Cells[row, 3].Value = item.Indirim;
                ws.Cells[row, 4].Value = item.Ciro;
                row++;
            }
            ws.Column(3).Style.Numberformat.Format = "#,##0.00";
            ws.Column(4).Style.Numberformat.Format = "#,##0.00";
            AutoFit(ws);
        }

        private void AddTrafficSheet(ExcelPackage package, string title, IReadOnlyList<RaporTrafficMetric> items)
        {
            var ws = package.Workbook.Worksheets.Add(title);
            WriteHeaders(ws, L("Admin_ReportTitleColumn"), L("Admin_Toplam"), L("Admin_Unique"));
            var row = 2;
            foreach (var item in items)
            {
                ws.Cells[row, 1].Value = item.Etiket;
                ws.Cells[row, 2].Value = item.Adet;
                ws.Cells[row, 3].Value = item.Tekil;
                row++;
            }
            AutoFit(ws);
        }

        private void AddCargoSheet(ExcelPackage package, RaporIndexViewModel model)
        {
            var ws = package.Workbook.Worksheets.Add(L("Admin_ReportSheetShipping"));
            WriteHeaders(ws, L("Admin_ReportCompany"), L("Admin_Order"), L("Admin_ReportInTransit"), L("Admin_Delivery"));
            var row = 2;
            foreach (var item in model.KargoPerformansi)
            {
                ws.Cells[row, 1].Value = item.Firma;
                ws.Cells[row, 2].Value = item.SiparisAdedi;
                ws.Cells[row, 3].Value = item.Kargoda;
                ws.Cells[row, 4].Value = item.Teslim;
                row++;
            }
            AutoFit(ws);
        }

        private static void WriteHeaders(ExcelWorksheet worksheet, params string[] headers)
        {
            for (var i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            using var range = worksheet.Cells[1, 1, 1, headers.Length];
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(49, 53, 17));
            range.Style.Font.Color.SetColor(System.Drawing.Color.White);
        }

        private static void StyleKeyValueSheet(ExcelWorksheet worksheet, int rows)
        {
            using var firstColumn = worksheet.Cells[1, 1, rows, 1];
            firstColumn.Style.Font.Bold = true;
            firstColumn.Style.Fill.PatternType = ExcelFillStyle.Solid;
            firstColumn.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(242, 238, 229));
            worksheet.Column(2).Style.Numberformat.Format = "#,##0.00";
            AutoFit(worksheet);
        }

        private static void AutoFit(ExcelWorksheet worksheet)
        {
            if (worksheet.Dimension != null)
            {
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            }
        }

        private static void PdfKpi(IContainer container, string title, string value, string subtitle)
        {
            container.Padding(4).Border(1).BorderColor("#e5e2dc").Background("#fcf9f3").Padding(10).Column(column =>
            {
                column.Item().Text(title).FontSize(8).FontColor("#6b6f45");
                column.Item().Text(value).FontSize(14).Bold().FontColor("#313511");
                column.Item().Text(subtitle).FontSize(7).FontColor("#777");
            });
        }

        private static void PdfSection(ColumnDescriptor column, string title, Action<TableDescriptor> tableBuilder)
        {
            column.Item().Text(title).FontSize(12).Bold().FontColor("#313511");
            column.Item().Table(tableBuilder);
        }

        private static void PdfHeader(TableDescriptor table, string text)
        {
            table.Cell().Background("#313511").Padding(5).Text(text).FontColor(Colors.White).Bold().FontSize(8);
        }

        private static void PdfCell(TableDescriptor table, string text)
        {
            table.Cell().BorderBottom(0.5f).BorderColor("#e5e2dc").Padding(5).Text(string.IsNullOrWhiteSpace(text) ? "-" : text).FontSize(7);
        }
    }
}


