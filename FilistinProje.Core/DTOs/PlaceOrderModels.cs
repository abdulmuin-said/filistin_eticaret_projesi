using FilistinProje.Core.Varliklar;

namespace FilistinProje.Core.DTOs
{
    public enum PlaceOrderStatus
    {
        Success = 0,
        PriceChanged = 1,
        StockShortage = 2,
        ShippingNotConfigured = 3,
        InvalidCoupon = 4,
        NoActiveBankAccount = 5,
        CodLimitExceeded = 6,
        WholesaleMinimumNotMet = 7,
        ValidationError = 8,
        BusinessError = 9
    }

    public sealed class PlaceOrderRequest
    {
        public CheckoutRequestDto Checkout { get; init; } = new();
        public IReadOnlyList<SepetItem> SepetItems { get; init; } = Array.Empty<SepetItem>();
        public string? AppUserId { get; init; }
        public string SessionId { get; init; } = string.Empty;
        public bool IsWholesale { get; init; }
        public string? KuponKodu { get; init; }

        public string PaymentPendingMessage { get; init; } = string.Empty;
        public string PayOnDeliveryPendingMessage { get; init; } = string.Empty;
        public string StorePickupCity { get; init; } = string.Empty;
        public string StorePickupDistrict { get; init; } = string.Empty;
        public string StorePickupAddress { get; init; } = string.Empty;
    }

    public sealed class PlaceOrderResult
    {
        public PlaceOrderStatus Status { get; init; }
        public int? SiparisId { get; init; }
        public string? SiparisNo { get; init; }
        public OrderPricingResult? Pricing { get; init; }
        public IReadOnlyList<StockShortageEntry> StockShortages { get; init; } = Array.Empty<StockShortageEntry>();
        public IReadOnlyList<PriceChangedEntry> PriceChanges { get; init; } = Array.Empty<PriceChangedEntry>();
        public string? MessageKey { get; init; }
        public object[] MessageArgs { get; init; } = Array.Empty<object>();

        public bool Succeeded => Status == PlaceOrderStatus.Success || Status == PlaceOrderStatus.PriceChanged;

        public static PlaceOrderResult SuccessResult(int siparisId, string siparisNo, OrderPricingResult pricing)
        {
            return new PlaceOrderResult
            {
                Status = pricing.FiyatDegistiMi ? PlaceOrderStatus.PriceChanged : PlaceOrderStatus.Success,
                SiparisId = siparisId,
                SiparisNo = siparisNo,
                Pricing = pricing,
                PriceChanges = pricing.FiyatDegisiklikleri
            };
        }
    }
}
