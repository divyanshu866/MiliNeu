using MiliNeu.Models.enums;

namespace MiliNeu.Models.ViewModels
{
    public class OrderDisplayVM
    {
        public int OrderId { get; set; }

        public List<OrderItemDisplayVM> Items { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal Shipping { get; set; }
        public decimal AmountCharged { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public ReturnStatus? ReturnStatus { get; set; }
        public DateTime? ReturnInitiatedDate { get; set; }

        //Contact/Customer Info
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public AddressVM AddressVM { get; set; }
        public string EstimatedDeliveryDate { get; set; }

        public DateTime OrderDate { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public ShippingAddress? BillingAddress { get; set; }
        public string TrackingNumber { get; set; }

        public IEnumerable<OrderIssue> OrderIssues { get; set; }
    }

    public class OrderItemDisplayVM
    {
        public IEnumerable<VariantImage> Images { get; set; }
        public string ProductName { get; set; }
        public string VariantName { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal Subtotal { get; set; }
    }

}
