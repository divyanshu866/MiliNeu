

using Razorpay.Api;

namespace PaymentGateway
{
    public interface IRazorpayPaymentService
    {
        public Order CreateOrder(decimal amount, string currency, string receiptId);
        public bool VerifyPaymentSignature(string razorpay_payment_id, string razorpay_order_id, string razorpay_signature);
        public string CheckPaymentStatus(string razorpayPaymentId);
        public string GetKey();
        public string GetSecret();

    }

}
