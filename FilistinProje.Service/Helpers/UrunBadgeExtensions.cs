using FilistinProje.Core.DTOs;
using FilistinProje.Core.Varliklar;

namespace FilistinProje.Service.Helpers
{
    public static class UrunBadgeExtensions
    {
        public static List<ProductBadge> ToBadges(
            this Urun urun,
            bool stoktaYokSatisIzni = false,
            decimal? eskiFiyat = null,
            decimal? etkinFiyat = null) =>
            ProductBadgeBuilder.Build(new ProductBadgeContext
            {
                StoktaVarMi = urun.StoktaVarMi,
                ToplamStok = urun.ToplamStok,
                KampanyaliMi = urun.KampanyaliMi,
                KampanyaBitisTarihi = urun.KampanyaBitisTarihi,
                FiyatGizliMi = urun.FiyatGizliMi,
                EskiFiyat = eskiFiyat ?? urun.Fiyat,
                EtkinFiyat = etkinFiyat ?? urun.EtkinFiyat,
                ToptanFiyatVarMi = urun.TopFiyat.HasValue && urun.TopFiyat.Value > 0,
                YeniUrunMu = urun.YeniUrunMu,
                OneCikanMi = urun.OneCikanMi,
                WhatsappSiparisModu = urun.FiyatGizliMi || urun.WhatsappSiparisVarMi,
                OneCikanEtiketRengi = urun.OneCikanEtiketRengi,
                YeniUrunEtiketRengi = urun.YeniUrunEtiketRengi,
                KampanyaEtiketRengi = urun.KampanyaEtiketRengi,
                IndirimEtiketRengi = urun.IndirimEtiketRengi
            }, stoktaYokSatisIzni);
    }
}
