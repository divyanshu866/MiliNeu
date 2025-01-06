namespace MiliNeu.Models.ViewModels
{
    public class PaymentVM
    {
        public string RazorpayKey { get; set; }
        public string RazorOrderId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public String UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CallbackUrl { get; set; }
    }
}
