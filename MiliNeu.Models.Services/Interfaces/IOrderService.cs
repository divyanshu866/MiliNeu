using MiliNeu.Models.ViewModels;

namespace MiliNeu.Models.Services.Interfaces
{
    public interface IOrderService
    {

        public Task<PagerVM<Order>> GetFilteredOrdersAsync(string filter, string searchTerm, int pageNumber, int pageSize);
        public Task<bool> UpdateFulfilmentStatusAsync(List<int> selectedOrders, string newStatus);
        public Task<bool> UpdatePaymentStatusAsync(List<int> selectedOrders, string newStatus, string changedBy);

        public Task<CheckoutVM> GetCheckoutDataAsync();
        public Task<Order> CreateOrderAsync(CheckoutVM checkoutVM);
        public Task<PaymentVM> GetPaymentDetailsAsync(int orderId);
        public Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        public Task<OrderDisplayVM> GetUserOrderDetailsAsync(int orderId);

    }
}
