using Microsoft.Extensions.Configuration;
using Razorpay.Api;
namespace PaymentGateway
{
    public class RazorpayPaymentService : IRazorpayPaymentService
    {
        private readonly IConfiguration _configuration;
        public RazorpayPaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public Order CreateOrder(decimal amount, string currency, string receiptId)
        {
            // Retrieve API key and secret from configuration
            string key = GetKey();
            string secret = GetSecret();

            // Initialize Razorpay client with the correct key and secret
            var client = new RazorpayClient(key, secret);

            // Create order options
            var options = new Dictionary<string, object>
                {
                    { "amount", (amount * 100) }, // Amount in paise (1 INR = 100 paise)
                    { "currency", currency },
                    { "receipt", receiptId }, // Use "receipt" instead of "receiptId"
                    { "payment_capture", "1" } // Auto capture the payment
                };

            // Create and return the order
            return client.Order.Create(options);
        }


        /*public bool CapturePayment(string paymentId, decimal amount)
        {
            var client = new RazorpayClient(GetKey(), GetSecret());
            var payment = client.Payment.Fetch(paymentId);
            var options = new Dictionary<string, object>
        {
            { "amount", amount * 100 }
        };
            payment.Capture(options);
            return true;
        }*/



        public string GetKey()
        {
            return _configuration["RazorPay:Key"];
        }
        public string GetSecret()
        {
            return _configuration["RazorPay:Secret"];
        }

        public bool VerifyPaymentSignature(string razorpay_payment_id, string razorpay_order_id, string razorpay_signature)
        {
            RazorpayClient client = new RazorpayClient(GetKey(), GetSecret());

            Dictionary<string, string> attributes = new Dictionary<string, string>();

            attributes.Add("razorpay_payment_id", razorpay_payment_id);
            attributes.Add("razorpay_order_id", razorpay_order_id);
            attributes.Add("razorpay_signature", razorpay_signature);


            try
            {
                Utils.verifyPaymentSignature(attributes);
                return true;
            }
            catch (Exception)
            {

                return false;

            }

        }
        public string CheckPaymentStatus(string razorpayPaymentId)
        {
            var client = new RazorpayClient(GetKey(), GetSecret());


            // Fetch payment details
            Payment payment = client.Payment.Fetch(razorpayPaymentId);

            if (payment != null)
            {
                return payment["status"].ToString();
            }
            else
            {
                return null;
            }

        }

    }
}
