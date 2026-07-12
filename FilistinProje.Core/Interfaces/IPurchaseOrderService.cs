using FilistinProje.Core.DTOs;

namespace FilistinProje.Core.Interfaces
{
    public interface IPurchaseOrderService
    {
        Task<PlaceOrderResult> PlaceOrderAsync(PlaceOrderRequest request);
    }
}
