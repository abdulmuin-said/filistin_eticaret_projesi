using FilistinProje.Core.Models;
using FilistinProje.Core.Varliklar;
using FilistinProje.Data;
using FilistinProje.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FilistinProje.Web.Services
{
    public interface IFaturaPdfService
    {
        Task<byte[]> GenerateInvoicePdfAsync(int siparisId);
    }

    public class FaturaPdfService : IFaturaPdfService
    {
        private readonly KanvasDbContext _context;
        private readonly ISiteSettingsService _siteSettings;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FaturaPdfService> _logger;

        public FaturaPdfService(
            KanvasDbContext context,
            ISiteSettingsService siteSettings,
            IWebHostEnvironment env,
            ILogger<FaturaPdfService> logger)
        {
            _context = context;
            _siteSettings = siteSettings;
            _env = env;
            _logger = logger;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]> GenerateInvoicePdfAsync(int siparisId)
        {
            var siparis = await _context.Siparisler
                .AsNoTracking()
                .Include(s => s.SiparisDetaylari.Where(d => !d.SilindiMi))
                    .ThenInclude(d => d.Urun)
                .Include(s => s.SiparisDetaylari.Where(d => !d.SilindiMi))
                    .ThenInclude(d => d.UrunSecenek)
                .FirstOrDefaultAsync(s => s.Id == siparisId);

            if (siparis == null)
                throw new KeyNotFoundException($"Sipariş bulunamadı: #{siparisId}");

            var settings = _siteSettings.GetSettings();
            var brandName = string.IsNullOrWhiteSpace(settings.MarkaAdi) ? settings.SiteAdi : settings.MarkaAdi;
            var siparisNo = string.IsNullOrWhiteSpace(siparis.SiparisNo) ? $"#{siparis.Id}" : siparis.SiparisNo;

            byte[]? logoBytes = TryLoadLogo(settings.SiteLogoUrl);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30, Unit.Millimetre);
                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));

                    page.Header().Element(c => ComposeHeader(c, settings, brandName, siparis, siparisNo, logoBytes));
                    page.Content().Element(c => ComposeContent(c, siparis));
                    page.Footer().Element(c => ComposeFooter(c, settings, brandName, siparisNo));
                });
            });

            var pdfBytes = document.GeneratePdf();
            _logger.LogInformation("PDF fatura oluşturuldu. SiparisId={SiparisId}, Boyut={Size} bytes", siparisId, pdfBytes.Length);
            return pdfBytes;
        }

        private byte[]? TryLoadLogo(string? logoUrl)
        {
            if (string.IsNullOrWhiteSpace(logoUrl)) return null;
            try
            {
                var relativePath = logoUrl.TrimStart('/');
                var fullPath = Path.Combine(_env.WebRootPath, relativePath);
                var ext = Path.GetExtension(fullPath).ToLowerInvariant();
                if ((ext == ".png" || ext == ".jpg" || ext == ".jpeg") && File.Exists(fullPath))
                    return File.ReadAllBytes(fullPath);
            }
            catch { }
            return null;
        }

        private static string DurumLabel(int durum) => durum switch
        {
            0 => "تم استلام الطلب / Order Received",
            1 => "قيد التحضير / Preparing",
            2 => "تم الشحن / Shipped",
            3 => "تم التسليم / Delivered",
            4 => "ملغي / Cancelled",
            8 => "قيد التعبئة / Packing",
            _ => "أخرى / Other"
        };

        private static string OdemeLabel(string? yontem) => yontem switch
        {
            "KapidaOdeme" => "الدفع عند الاستلام / Cash on Delivery",
            "BankaHavalesi" => "تحويل بنكي / Bank Transfer",
            "Kredi" or "KrediKarti" => "بطاقة ائتمان / Credit Card",
            null or "" => "غير محدد / N/A",
            _ => yontem
        };

        private static string TeslimatLabel(string? tip) => tip switch
        {
            "Magazadan" or "magazadan" => "استلام من المتجر / Store Pickup",
            _ => "توصيل للمنزل / Home Delivery"
        };

        private void ComposeHeader(IContainer container, SiteAyarlari settings, string brandName,
            Siparis siparis, string siparisNo, byte[]? logoBytes)
        {
            container.Column(col =>
            {
                col.Item().Row(row =>
                {
                    row.RelativeItem().Column(left =>
                    {
                        if (logoBytes != null)
                        {
                            left.Item().Height(50).Image(logoBytes).FitHeight();
                        }
                        else
                        {
                            left.Item().Text(brandName)
                                .FontSize(20).Bold().FontColor(Colors.Black);
                        }

                        if (!string.IsNullOrWhiteSpace(settings.SiteAciklamasi))
                            left.Item().PaddingTop(2).Text(settings.SiteAciklamasi)
                                .FontSize(7).FontColor(Colors.Grey.Medium);

                        if (!string.IsNullOrWhiteSpace(settings.Adres))
                            left.Item().PaddingTop(4).Text(settings.Adres)
                                .FontSize(7).FontColor(Colors.Grey.Darken1);
                        if (!string.IsNullOrWhiteSpace(settings.Telefon))
                            left.Item().Text($"📞 {settings.Telefon}")
                                .FontSize(7).FontColor(Colors.Grey.Darken1);
                        if (!string.IsNullOrWhiteSpace(settings.Email))
                            left.Item().Text(settings.Email)
                                .FontSize(7).FontColor(Colors.Grey.Darken1);
                        if (!string.IsNullOrWhiteSpace(settings.BaseUrl))
                            left.Item().Text(settings.BaseUrl)
                                .FontSize(7).FontColor(Colors.Blue.Darken2);
                    });

                    row.ConstantItem(160).Column(right =>
                    {
                        right.Item().Text("فاتورة / INVOICE")
                            .FontSize(24).Bold().FontColor(Colors.Black).AlignRight();
                        right.Item().PaddingTop(4).Text(siparisNo)
                            .FontSize(11).Bold().FontColor(Colors.Grey.Darken3).AlignRight();
                        right.Item().PaddingTop(2).Text(siparis.OlusturulmaTarihi.ToString("dd/MM/yyyy"))
                            .FontSize(9).FontColor(Colors.Grey.Darken1).AlignRight();
                    });
                });

                col.Item().PaddingVertical(8).LineHorizontal(1.5f).LineColor(Colors.Black);

                col.Item().Row(row =>
                {
                    row.RelativeItem().Column(metaCol =>
                    {
                        void MetaRow(string ar, string en, string val)
                        {
                            metaCol.Item().Row(r =>
                            {
                                r.ConstantItem(130).Text($"{ar} / {en}:")
                                    .FontSize(8).Bold().FontColor(Colors.Grey.Darken2);
                                r.RelativeItem().Text(val).FontSize(8);
                            });
                        }

                        MetaRow("رقم الطلب", "Order No", siparisNo);
                        MetaRow("تاريخ الطلب", "Order Date", siparis.OlusturulmaTarihi.ToString("dd/MM/yyyy HH:mm"));
                        MetaRow("الحالة", "Status", DurumLabel(siparis.Durum));
                        MetaRow("طريقة الدفع", "Payment", OdemeLabel(siparis.OdemeYontemi));
                        MetaRow("طريقة التسليم", "Delivery", TeslimatLabel(siparis.TeslimatTipi));
                    });

                    row.ConstantItem(220).Column(custCol =>
                    {
                        custCol.Item()
                            .Background(Colors.Grey.Lighten4)
                            .Border(1).BorderColor(Colors.Grey.Lighten2)
                            .Padding(8).Column(c =>
                            {
                                c.Item().Text("معلومات العميل / Customer Info")
                                    .FontSize(7).Bold().FontColor(Colors.Grey.Darken2);
                                c.Item().PaddingTop(4).Text(siparis.MusteriAdSoyad)
                                    .FontSize(10).Bold();
                                if (!string.IsNullOrWhiteSpace(siparis.Telefon))
                                    c.Item().Text($"📞 {siparis.Telefon}").FontSize(8).FontColor(Colors.Grey.Darken1);
                                if (!string.IsNullOrWhiteSpace(siparis.Eposta))
                                    c.Item().Text(siparis.Eposta).FontSize(8).FontColor(Colors.Grey.Darken1);
                                if (!string.IsNullOrWhiteSpace(siparis.AcikAdres))
                                    c.Item().PaddingTop(4).Text(siparis.AcikAdres)
                                        .FontSize(8).FontColor(Colors.Grey.Darken1);
                                if (!string.IsNullOrWhiteSpace(siparis.Sehir))
                                    c.Item().Text($"{siparis.Ilce} / {siparis.Sehir}")
                                        .FontSize(8).FontColor(Colors.Grey.Darken1);
                            });
                    });
                });
            });
        }

        private void ComposeContent(IContainer container, Siparis siparis)
        {
            container.Column(col =>
            {
                col.Item().PaddingVertical(12).Table(table =>
                {
                    table.ColumnsDefinition(c =>
                    {
                        c.RelativeColumn(3);
                        c.RelativeColumn(1.5f);
                        c.ConstantColumn(50);
                        c.ConstantColumn(80);
                        c.ConstantColumn(85);
                    });

                    table.Header(header =>
                    {
                        void Hdr(string ar, string en, bool right = false)
                        {
                            var cell = header.Cell().Background(Colors.Black).Padding(6);
                            if (right)
                                cell.Text($"{ar}\n{en}").FontSize(7).Bold().FontColor(Colors.White).AlignRight();
                            else
                                cell.Text($"{ar}\n{en}").FontSize(7).Bold().FontColor(Colors.White);
                        }
                        Hdr("المنتج", "Product");
                        Hdr("المتغير", "Variant");
                        Hdr("الكمية", "Qty", true);
                        Hdr("سعر الوحدة", "Unit Price", true);
                        Hdr("المجموع", "Total", true);
                    });

                    var detaylar = siparis.SiparisDetaylari.Where(d => !d.SilindiMi).ToList();
                    bool isEven = false;

                    foreach (var item in detaylar)
                    {
                        isEven = !isEven;
                        var bg = isEven ? Colors.White : Colors.Grey.Lighten5;

                        var urunAdi = item.Urun?.LocalizedBaslik ?? "—";
                        var varyant = item.UrunSecenek?.VaryantBasligi
                                      ?? item.UrunSecenek?.Olcu
                                      ?? "—";
                        var birimFiyat = item.BirimFiyat;
                        var hediyeEk = item.HediyePaketi ? item.HediyePaketFiyati * item.Adet : 0;
                        var satirToplam = (item.Adet * birimFiyat) + hediyeEk;

                        table.Cell().Background(bg).BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5)
                            .Text(urunAdi).FontSize(8);
                        table.Cell().Background(bg).BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5)
                            .Text(varyant).FontSize(7).FontColor(Colors.Grey.Darken1);
                        table.Cell().Background(bg).BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5)
                            .Text(item.Adet.ToString()).FontSize(8).AlignRight();
                        table.Cell().Background(bg).BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5)
                            .Text($"₪ {birimFiyat:N2}").FontSize(8).AlignRight();
                        table.Cell().Background(bg).BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5)
                            .Text($"₪ {satirToplam:N2}").FontSize(8).AlignRight().Bold();

                        if (item.HediyePaketi && item.HediyePaketFiyati > 0)
                        {
                            table.Cell().ColumnSpan(4).Padding(2).PaddingLeft(10)
                                .Text("🎁 تغليف هدية / Gift Wrap").FontSize(7).FontColor(Colors.Grey.Darken1).Italic();
                            table.Cell().Padding(2)
                                .Text($"₪ {hediyeEk:N2}").FontSize(7).FontColor(Colors.Grey.Darken1).AlignRight();
                        }
                    }
                });

                col.Item().AlignRight().Width(260).PaddingTop(8).Column(summary =>
                {
                    var araToplam = siparis.SiparisDetaylari
                        .Where(d => !d.SilindiMi)
                        .Sum(d => (d.Adet * d.BirimFiyat) + (d.HediyePaketi ? d.HediyePaketFiyati * d.Adet : 0));

                    void SummaryRow(string label, decimal amount, string? color = null, bool bold = false)
                    {
                        summary.Item().PaddingVertical(2).Row(r =>
                        {
                            var labelText = r.RelativeItem().Text(label).FontSize(9);
                            if (color != null) labelText.FontColor(color);
                            var valText = r.ConstantItem(90).AlignRight().Text($"₪ {amount:N2}").FontSize(9);
                            if (bold) valText.Bold();
                            if (color != null) valText.FontColor(color);
                        });
                    }

                    SummaryRow("المجموع الفرعي / Subtotal", araToplam);

                    if (siparis.IndirimTutari > 0)
                        SummaryRow("الخصم / Discount", -siparis.IndirimTutari, Colors.Red.Medium);

                    if (siparis.KargoUcreti > 0)
                        SummaryRow("الشحن / Shipping", siparis.KargoUcreti);

                    if (siparis.KapidaOdemeHizmetBedeli > 0)
                        SummaryRow("رسوم الدفع عند الاستلام / COD Fee", siparis.KapidaOdemeHizmetBedeli);

                    summary.Item().PaddingVertical(4).LineHorizontal(1.5f).LineColor(Colors.Black);

                    summary.Item().Row(r =>
                    {
                        r.RelativeItem().Text("المجموع الكلي / GRAND TOTAL")
                            .FontSize(11).Bold();
                        r.ConstantItem(90).AlignRight().Text($"₪ {siparis.ToplamTutar:N2}")
                            .FontSize(14).Bold().FontColor(Colors.Black);
                    });
                });

                col.Item().PaddingTop(16).Row(infoRow =>
                {
                    infoRow.RelativeItem().Column(left =>
                    {
                        left.Item().Background(Colors.Grey.Lighten4).Border(1).BorderColor(Colors.Grey.Lighten2)
                            .Padding(8).Column(c =>
                            {
                                c.Item().Text("طريقة الدفع / Payment Method")
                                    .FontSize(7).Bold().FontColor(Colors.Grey.Darken2);
                                c.Item().PaddingTop(3).Text(OdemeLabel(siparis.OdemeYontemi)).FontSize(8);
                                if (siparis.KapidaOdemeHizmetBedeli > 0)
                                    c.Item().Text($"رسوم / Fee: ₪ {siparis.KapidaOdemeHizmetBedeli:N2}")
                                        .FontSize(7).FontColor(Colors.Grey.Darken1);
                            });
                    });

                    if (!string.IsNullOrWhiteSpace(siparis.KargoFirmasi))
                    {
                        infoRow.ConstantItem(8);
                        infoRow.RelativeItem().Background(Colors.Grey.Lighten4).Border(1).BorderColor(Colors.Grey.Lighten2)
                            .Padding(8).Column(c =>
                            {
                                c.Item().Text("معلومات الشحن / Shipping Info")
                                    .FontSize(7).Bold().FontColor(Colors.Grey.Darken2);
                                c.Item().PaddingTop(3).Text(siparis.KargoFirmasi).FontSize(8);
                                if (!string.IsNullOrWhiteSpace(siparis.KargoTakipNo))
                                    c.Item().Text($"رقم التتبع / Tracking: {siparis.KargoTakipNo}")
                                        .FontSize(7).FontColor(Colors.Grey.Darken1);
                            });
                    }
                });

                if (!string.IsNullOrWhiteSpace(siparis.Aciklama))
                {
                    col.Item().PaddingTop(8).Background(Colors.Yellow.Lighten4).Border(1)
                        .BorderColor(Colors.Yellow.Lighten2).Padding(8).Column(c =>
                        {
                            c.Item().Text("ملاحظات الطلب / Order Notes")
                                .FontSize(7).Bold().FontColor(Colors.Grey.Darken2);
                            c.Item().PaddingTop(3).Text(siparis.Aciklama).FontSize(8);
                        });
                }
            });
        }

        private void ComposeFooter(IContainer container, SiteAyarlari settings, string brandName, string siparisNo)
        {
            container.Column(col =>
            {
                col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                col.Item().PaddingTop(6).Row(row =>
                {
                    row.RelativeItem().Column(left =>
                    {
                        left.Item().Text($"تم إنشاء هذه الفاتورة بواسطة {brandName}")
                            .FontSize(7).FontColor(Colors.Grey.Medium);
                        left.Item().Text($"Generated by {brandName} | رقم الفاتورة / Invoice: {siparisNo}")
                            .FontSize(7).FontColor(Colors.Grey.Medium);
                    });

                    row.ConstantItem(100).AlignRight().Column(right =>
                    {
                        right.Item().AlignRight().Text(x =>
                        {
                            x.Span("صفحة / Page ").FontSize(7).FontColor(Colors.Grey.Medium);
                            x.CurrentPageNumber().FontSize(7).FontColor(Colors.Grey.Medium);
                            x.Span(" / ").FontSize(7).FontColor(Colors.Grey.Medium);
                            x.TotalPages().FontSize(7).FontColor(Colors.Grey.Medium);
                        });
                    });
                });
            });
        }
    }
}
