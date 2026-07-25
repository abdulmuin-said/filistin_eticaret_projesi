using System.Collections.Generic;

namespace FilistinProje.Core.DTOs
{
    /// <summary>
    /// Bir sipariş satırının server-side hesaplanmış nihai durumu.
    /// Client-supplied SepetItem.Fiyat dikkate alınmaz — sadece DB'den güncel okunan fiyat geçerlidir.
    /// </summary>
    public class OrderLinePricing
    {
        public int SepetItemId { get; set; }
        public int UrunId { get; set; }
        public int? UrunSecenekId { get; set; }

        /// <summary>Satır başına (adet 1) server-side hesaplanmış birim fiyat.</summary>
        public decimal BirimFiyat { get; set; }

        /// <summary>Hediye paketi satır başına birim bedel.</summary>
        public decimal HediyePaketBirim { get; set; }

        public bool HediyePaketi { get; set; }

        public int Adet { get; set; }

        /// <summary>BirimFiyat * Adet + HediyePaketi ? HediyePaketiBirim * Adet : 0</summary>
        public decimal SatirToplam { get; set; }

        public decimal? OncekiSepetFiyat { get; set; }
        public bool FiyatDegistiMi => OncekiSepetFiyat.HasValue && OncekiSepetFiyat.Value != BirimFiyat;

        /// <summary>
        /// Stok yetersizse açıklama: varyant/kalem + mevcut stok.
        /// Bu olursa sipariş tamamen reddedilir; transaction rollback.
        /// </summary>
        public string? StokSorunu { get; set; }

        public bool StokYetersizMi => !string.IsNullOrEmpty(StokSorunu);
    }

    public class PriceChangedEntry
    {
        public int SepetItemId { get; set; }
        public string UrunBaslik { get; set; } = string.Empty;
        public decimal EskiFiyat { get; set; }
        public decimal YeniFiyat { get; set; }
    }

    public class StockShortageEntry
    {
        public int SepetItemId { get; set; }
        public string UrunBaslik { get; set; } = string.Empty;
        public int? UrunSecenekId { get; set; }
        public int IstenenAdet { get; set; }
        public int MevcutStok { get; set; }
    }

    /// <summary>
    /// Checkout için server-side fiyat/stok doğrulaması sonucu.
    /// </summary>
    public class OrderPricingResult
    {
        public List<OrderLinePricing> Satirlar { get; set; } = new();
        public List<PriceChangedEntry> FiyatDegisiklikleri { get; set; } = new();
        public List<StockShortageEntry> StokYetersizlikleri { get; set; } = new();
        public List<string> LimitAsimlari { get; set; } = new();

        public decimal AraToplam { get; set; }
        public decimal IndirimTutari { get; set; }
        public string? UygulananKuponKodu { get; set; }
        public decimal KargoUcreti { get; set; }
        public decimal KapidaOdemeHizmetBedeli { get; set; }

        /// <summary>ToplamTutar = (AraToplam - IndirimTutari) + KargoÜcreti + Kapıda Ödeme Hizmet Bedeli.</summary>
        public decimal GenelToplam => Math.Max(0m,
            (AraToplam - IndirimTutari)
            + KargoUcreti
            + KapidaOdemeHizmetBedeli);

        public bool StokSorunuVar => StokYetersizlikleri.Count > 0;
        public bool LimitSorunuVar => LimitAsimlari.Count > 0;
        public bool FiyatDegistiMi => FiyatDegisiklikleri.Count > 0;
    }

    /// <summary>
    /// Onaylanmış bir checkout işleminde elde edilen stok düşüm sonuçları.
    /// </summary>
    public class StockDeductionResult
    {
        public bool Basarili { get; set; }
        public int? BasarisizUrunSecenekId { get; set; }
        public string? HataMesaji { get; set; }
    }
}
