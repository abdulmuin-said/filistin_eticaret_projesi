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
    /// <summary>
    /// QuestPDF ile sipariş verisinden profesyonel PDF faturası oluşturur.
    /// </summary>
    public interface IFaturaPdfService
    {
        Task<byte[]> GenerateInvoicePdfAsync(int siparisId);
    }

    public class FaturaPdfService : IFaturaPdfService
    {
        private readonly KanvasDbContext _context;
        private readonly ISiteSettingsService _siteSettings;
        private readonly ILogger<FaturaPdfService> _logger;

        public FaturaPdfService(
            KanvasDbContext context,
            ISiteSettingsService siteSettings,
            ILogger<FaturaPdfService> logger)
        {
            _context = context;
            _siteSettings = siteSettings;
            _logger = logger;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]> GenerateInvoicePdfAsync(int siparisId)
        {
            // Sipariş ve ilişkili verileri yükle
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

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(35, Unit.Millimetre);
                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));

                    // ========== HEADER ==========
                    page.Header().Element(c => ComposeHeader(c, settings, brandName, siparis, siparisNo));

                    // ========== CONTENT ==========
                    page.Content().Element(c => ComposeContent(c, settings, siparis, brandName));

                    // ========== FOOTER ==========
                    page.Footer().Element(c => ComposeFooter(c, settings, brandName, siparisNo));
                });
            });

            var pdfBytes = document.GeneratePdf();
            _logger.LogInformation("PDF fatura oluşturuldu. SiparisId={SiparisId}, Boyut={Size} bytes", siparisId, pdfBytes.Length);
            return pdfBytes;
        }

        private void ComposeHeader(IContainer container, SiteAyarlari settings, string brandName, Siparis siparis, string siparisNo)
        {
            container.Column(col =>
            {
                // Üst şerit — marka adı + "FATURA" başlığı
                col.Item().Row(row =>
                {
                    // Sol: Logo ve marka
                    row.RelativeItem().Column(logoCol =>
                    {
                        logoCol.Item().Text(brandName)
                            .FontSize(20).Bold().FontColor(Colors.Black);

                        if (!string.IsNullOrWhiteSpace(settings.SiteAciklamasi))
                        {
                            logoCol.Item().Text(settings.SiteAciklamasi)
                                .FontSize(8).FontColor(Colors.Grey.Medium);
                        }
                    });

                    // Sağ: FATURA etiketi
                    row.ConstantItem(120).Column(right =>
                    {
                        right.Item().Text("FATURA")
                            .FontSize(28).Bold().FontColor(Colors.Black).AlignRight();
                        right.Item().Text(siparisNo)
                            .FontSize(11).Bold().FontColor(Colors.Grey.Darken3).AlignRight();
                    });
                });

                // Ayırıcı çizgi
                col.Item().PaddingVertical(8).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                // İletişim bilgileri + müşteri bilgileri
                col.Item().Row(row =>
                {
                    // Sol: Şirket bilgileri
                    row.RelativeItem().Column(companyCol =>
                    {
                        if (!string.IsNullOrWhiteSpace(settings.Adres))
                            companyCol.Item().Text(settings.Adres).FontSize(8).FontColor(Colors.Grey.Darken1);
                        if (!string.IsNullOrWhiteSpace(settings.Telefon))
                            companyCol.Item().Text($"Tel: {settings.Telefon}").FontSize(8).FontColor(Colors.Grey.Darken1);
                        if (!string.IsNullOrWhiteSpace(settings.Email))
                            companyCol.Item().Text($"E-posta: {settings.Email}").FontSize(8).FontColor(Colors.Grey.Darken1);
                        if (!string.IsNullOrWhiteSpace(settings.BaseUrl))
                            companyCol.Item().Text(settings.BaseUrl).FontSize(8).FontColor(Colors.Blue.Darken2);
                    });

                    // Sağ: Müşteri bilgileri
                    row.ConstantItem(220).Column(custCol =>
                    {
                        custCol.Item().Background(Colors.Grey.Lighten5).Border(1)
                            .BorderColor(Colors.Grey.Lighten2).Padding(8).Column(c =>
                            {
                                c.Item().Text("MÜŞTERİ BİLGİLERİ")
                                    .FontSize(7).Bold().FontColor(Colors.Grey.Darken2);
                                c.Item().PaddingTop(4).Text(siparis.MusteriAdSoyad)
                                    .FontSize(10).Bold().FontColor(Colors.Black);
                                if (!string.IsNullOrWhiteSpace(siparis.Telefon))
                                    c.Item().Text($"Tel: {siparis.Telefon}").FontSize(8).FontColor(Colors.Grey.Darken1);
                                if (!string.IsNullOrWhiteSpace(siparis.Eposta))
                                    c.Item().Text(siparis.Eposta).FontSize(8).FontColor(Colors.Grey.Darken1);
                                c.Item().PaddingTop(4).Text($"{siparis.AcikAdres}")
                                    .FontSize(8).FontColor(Colors.Grey.Darken1);
                                c.Item().Text($"{siparis.Ilce} / {siparis.Sehir}")
                                    .FontSize(8).FontColor(Colors.Grey.Darken1);
                            });
                    });
                });

                // Sipariş özeti
                col.Item().PaddingTop(8).Row(row =>
                {
                    var durumLabel = siparis.Durum switch
                    {
                        0 => "Sipariş Alındı",
                        1 => "Hazırlanıyor",
                        2 => "Kargoya Verildi",
                        3 => "Teslim Edildi",
                        4 => "İptal Edildi",
                        8 => "Paketleniyor",
                        _ => "Diğer"
                    };

                    row.RelativeItem().Column(metaCol =>
                    {
                        metaCol.Item().Row(r =>
                        {
                            r.ConstantItem(130).Text("Sipariş No:").FontSize(8).Bold();
                            r.RelativeItem().Text(siparisNo).FontSize(8);
                        });
                        metaCol.Item().Row(r =>
                        {
                            r.ConstantItem(130).Text("Sipariş Tarihi:").FontSize(8).Bold();
                            r.RelativeItem().Text(siparis.OlusturulmaTarihi.ToString("dd.MM.yyyy HH:mm")).FontSize(8);
                        });
                        metaCol.Item().Row(r =>
                        {
                            r.ConstantItem(130).Text("Durum:").FontSize(8).Bold();
                            r.RelativeItem().Text(durumLabel).FontSize(8);
                        });
                        metaCol.Item().Row(r =>
                        {
                            r.ConstantItem(130).Text("Ödeme Yöntemi:").FontSize(8).Bold();
                            r.RelativeItem().Text(siparis.OdemeYontemi).FontSize(8);
                        });
                    });

                    row.ConstantItem(100); // boşluk
                });
            });
        }

        private void ComposeContent(IContainer container, SiteAyarlari settings, Siparis siparis, string brandName)
        {
            container.Column(col =>
            {
                // ========== ÜRÜN TABLOSU ==========
                col.Item().PaddingVertical(12).Table(table =>
                {
                    table.ColumnsDefinition(c =>
                    {
                        c.RelativeColumn(3);  // Ürün adı
                        c.RelativeColumn(1);  // Varyant
                        c.ConstantColumn(60); // Adet
                        c.ConstantColumn(90); // Birim Fiyat
                        c.ConstantColumn(90); // Toplam
                    });

                    // Tablo başlıkları
                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Black).Padding(6)
                            .Text("Ürün").FontSize(8).Bold().FontColor(Colors.White);
                        header.Cell().Background(Colors.Black).Padding(6)
                            .Text("Varyant").FontSize(8).Bold().FontColor(Colors.White);
                        header.Cell().Background(Colors.Black).Padding(6)
                            .Text("Adet").FontSize(8).Bold().FontColor(Colors.White).AlignCenter();
                        header.Cell().Background(Colors.Black).Padding(6)
                            .Text("Birim Fiyat").FontSize(8).Bold().FontColor(Colors.White).AlignRight();
                        header.Cell().Background(Colors.Black).Padding(6)
                            .Text("Toplam").FontSize(8).Bold().FontColor(Colors.White).AlignRight();
                    });

                    // Tablo satırları
                    var detaylar = siparis.SiparisDetaylari
                        .Where(d => !d.SilindiMi)
                        .ToList();

                    foreach (var item in detaylar)
                    {
                        var urunAdi = item.Urun?.Baslik ?? "Ürün";
                        var varyant = item.UrunSecenek?.VaryantBasligi
                                      ?? item.UrunSecenek?.Olcu
                                      ?? "Standart";
                        var birimFiyat = item.BirimFiyat;
                        var toplam = (item.Adet * birimFiyat) + (item.HediyePaketi ? item.HediyePaketFiyati * item.Adet : 0);

                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(4)
                            .Text(urunAdi).FontSize(8);
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(4)
                            .Text(varyant).FontSize(7).FontColor(Colors.Grey.Darken1);
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(4)
                            .Text(item.Adet.ToString()).FontSize(8).AlignCenter();
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(4)
                            .Text($"{birimFiyat:N2} ₪").FontSize(8).AlignRight();
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(4)
                            .Text($"{toplam:N2} ₪").FontSize(8).AlignRight().Bold();
                    }
                });

                // ========== ÖZET ==========
                col.Item().AlignRight().Width(250).PaddingTop(8).Column(summary =>
                {
                    var araToplam = siparis.SiparisDetaylari
                        .Where(d => !d.SilindiMi)
                        .Sum(d => (d.Adet * d.BirimFiyat) + (d.HediyePaketi ? d.HediyePaketFiyati * d.Adet : 0));

                    summary.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Ara Toplam:").FontSize(9).FontColor(Colors.Grey.Darken2);
                        r.ConstantItem(90).AlignRight().Text($"{araToplam:N2} ₪").FontSize(9);
                    });

                    if (siparis.IndirimTutari > 0)
                    {
                        summary.Item().PaddingTop(4).Row(r =>
                        {
                            r.RelativeItem().Text("İndirim:").FontSize(9).FontColor(Colors.Red.Medium);
                            r.ConstantItem(90).AlignRight().Text($"-{siparis.IndirimTutari:N2} ₪").FontSize(9).FontColor(Colors.Red.Medium);
                        });
                    }

                    if (siparis.KargoUcreti > 0)
                    {
                        summary.Item().PaddingTop(4).Row(r =>
                        {
                            r.RelativeItem().Text("Kargo:").FontSize(9).FontColor(Colors.Grey.Darken2);
                            r.ConstantItem(90).AlignRight().Text($"{siparis.KargoUcreti:N2} ₪").FontSize(9);
                        });
                    }

                    // Kesintisiz çizgi
                    summary.Item().PaddingVertical(4).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                    // Genel Toplam
                    summary.Item().Row(r =>
                    {
                        r.RelativeItem().Text("GENEL TOPLAM:").FontSize(11).Bold();
                        r.ConstantItem(90).AlignRight().Text($"{siparis.ToplamTutar:N2} ₪")
                            .FontSize(14).Bold().FontColor(Colors.Black);
                    });
                });

                // Ödeme bilgisi
                if (siparis.OdemeYontemi == "KapidaOdeme")
                {
                    col.Item().PaddingTop(16).Background(Colors.Grey.Lighten5)
                        .Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text(text =>
                        {
                            text.Span("Ödeme Şekli: ").FontSize(8).Bold();
                            text.Span("Kapıda Ödeme").FontSize(8);
                            if (siparis.KapidaOdemeHizmetBedeli > 0)
                            {
                                text.Span($" (Hizmet Bedeli: {siparis.KapidaOdemeHizmetBedeli:N2} ₪)").FontSize(7).FontColor(Colors.Grey.Darken1);
                            }
                        });
                }
                else if (siparis.OdemeYontemi == "BankaHavalesi")
                {
                    col.Item().PaddingTop(16).Background(Colors.Grey.Lighten5)
                        .Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text(text =>
                        {
                            text.Span("Ödeme Şekli: ").FontSize(8).Bold();
                            text.Span("Banka Havalesi / EFT").FontSize(8);
                        });
                }
                else
                {
                    col.Item().PaddingTop(16).Background(Colors.Grey.Lighten5)
                        .Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text(text =>
                        {
                            text.Span("Ödeme Şekli: ").FontSize(8).Bold();
                            text.Span(siparis.OdemeYontemi).FontSize(8);
                        });
                }

                // Teslimat bilgisi
                if (!string.IsNullOrWhiteSpace(siparis.KargoFirmasi))
                {
                    col.Item().PaddingTop(4).Background(Colors.Grey.Lighten5)
                        .Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text(text =>
                        {
                            text.Span("Kargo: ").FontSize(8).Bold();
                            text.Span(siparis.KargoFirmasi).FontSize(8);
                            if (!string.IsNullOrWhiteSpace(siparis.KargoTakipNo))
                            {
                                text.Span($" | Takip No: {siparis.KargoTakipNo}").FontSize(7).FontColor(Colors.Grey.Darken1);
                            }
                        });
                }
            });
        }

        private void ComposeFooter(IContainer container, SiteAyarlari settings, string brandName, string siparisNo)
        {
            container.Column(col =>
            {
                col.Item().PaddingTop(8).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                col.Item().PaddingTop(8).Row(row =>
                {
                    row.RelativeItem().Column(footerCol =>
                    {
                        footerCol.Item().Text($"Bu fatura {brandName} tarafından oluşturulmuştur.")
                            .FontSize(7).FontColor(Colors.Grey.Medium);

                        footerCol.Item().Text($"Fatura No: {siparisNo} | Tarih: {DateTime.Now:dd.MM.yyyy HH:mm}")
                            .FontSize(7).FontColor(Colors.Grey.Medium);
                    });

                    row.ConstantItem(100).AlignRight().Column(pageCol =>
                    {
                        pageCol.Item().Text("Sayfa {page} / {totalPages}")
                            .FontSize(7).FontColor(Colors.Grey.Medium).AlignRight();
                    });
                });
            });
        }
    }
}
