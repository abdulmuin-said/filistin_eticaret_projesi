using FilistinProje.Core.DTOs;
using FilistinProje.Core.Varliklar;

namespace FilistinProje.Service.Helpers
{
    public static class UrunBadgeExtensions
    {
        public static List<ProductBadge> ToBadges(this Urun urun, bool stoktaYokSatisIzni = false) =>
            ProductBadgeBuilder.Build(new ProductBadgeContext
            {
                StoktaVarMi = urun.StoktaVarMi,
                ToplamStok = urun.ToplamStok,
                KampanyaliMi = urun.KampanyaliMi,
                KampanyaBitisTarihi = urun.KampanyaBitisTarihi,
                FiyatGizliMi = urun.FiyatGizliMi,
                IndirimVarMi = urun.IndirimVarMi,
                IndirimYuzdesi = urun.IndirimYuzdesi,
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
