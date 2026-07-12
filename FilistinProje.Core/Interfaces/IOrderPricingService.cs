using System.Collections.Generic;
using System.Threading.Tasks;
using FilistinProje.Core.DTOs;
using FilistinProje.Core.Varliklar;

namespace FilistinProje.Core.Interfaces
{
    /// <summary>
    /// Sipariş kontrolünde fiyatların her zaman güncel DB'den hesaplanmasını sağlayan tek server-side servis.
    /// Client snapshot'larına veya SepetItem.Fiyat'a güvenmez.
    /// </summary>
    public interface IOrderPricingService
    {
        /// <summary>
        /// Checkout başlamadan önce sepeti server-side yeniden hesaplar.
        /// </summary>
        /// <param name="sehir">Adres şehri; boş ise mağazadan teslim olarak değerlendirilir.</param>
        /// <param name="odemeYontemi">"BankaHavalesi" veya "KapidaOdeme".</param>
        /// <param name="isWholesale">Toptancı kullanıcı fiyatı için.</param>
        /// <param name="kuponKodu">Session'dan gelen kupon kodu (null/empty ise atlanır).</param>
        Task<OrderPricingResult> HesaplaAsync(
            IReadOnlyList<SepetItem> sepetItems,
            string? sehir,
            string odemeYontemi,
            bool isWholesale,
            string? kuponKodu);

        /// <summary>
        /// Transaction içinde çağrılır. Her sipariş satırı için UrunSecenekId varsa atomik stok düşümü yapar.
        /// EF'in change tracking'ine dahil etmeden ExecuteSqlInterpolatedAsync ile çalışır.
        /// Detay: B2 — read-then-write yarışını engeller.
        /// </summary>
        Task<StockDeductionResult> StokDusAsync(System.Collections.Generic.List<OrderLinePricing> satirlar);

        /// <summary>
        /// Kupon doğrulama + indirim tutarını hesaplar. Sipariş POST'unda kullanılır.
        /// </summary>
        decimal CalculateCouponDiscount(Kupon kupon, decimal sepetTutari);
    }
}
