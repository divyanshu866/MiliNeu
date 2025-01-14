using Microsoft.AspNetCore.Mvc;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models;
using MiliNeu.Models.enums;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MiliNeu.Controllers
{
    [ApiController]
    [Route("webhook/razorpay")]
    public class RazorpayWebhookController : Controller
    {
        private readonly ApplicationDbContext _context; // Your database context
        private readonly IConfiguration _configuration; // Your database context

        public RazorpayWebhookController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> HandleRazorpayWebhook()
        {


            // Read the request body
            using (var reader = new StreamReader(Request.Body))
            {
                var requestBody = await reader.ReadToEndAsync();


                // Retrieve the Razorpay Signature header
                var razorpaySignature = Request.Headers["X-Razorpay-Signature"].ToString();

                // Verify the signature
                if (!VerifyWebhookSignature(requestBody, razorpaySignature))
                {
                    // Process the payload (e.g., update payment status)
                    return Unauthorized();
                }
                // Parse the Webhook Payload
                var payload = JObject.Parse(requestBody);
                var eventType = payload["event"].ToString();

                //if (eventType == "payment.captured")
                //{
                var RazorPaymentId = payload["payload"]["payment"]["entity"]["id"].ToString();
                var RazorOrderId = payload["payload"]["payment"]["entity"]["order_id"].ToString();
                var amount = payload["payload"]["payment"]["entity"]["amount"].ToObject<int>();
                var email = payload["payload"]["payment"]["entity"]["email"]?.ToString();
                var contact = payload["payload"]["payment"]["entity"]["contact"]?.ToString();
                var billingAddress = payload["payload"]["payment"]["entity"]["billing_address"];

                // Update the Order Model in Database
                var order = _context.Orders.FirstOrDefault(o => o.RazorOrderId == RazorOrderId);
                if (order != null)
                {
                    order.RazorPaymentId = RazorPaymentId;
                    order.PaymentCaptured = true;
                    order.PaymentStatus = PaymentStatus.Confirmed;
                    order.PaidAt = DateTime.UtcNow;
                    order.Amount = amount / 100.0m; // Razorpay returns amount in paise
                                                    //order.BillingEmail = email;
                                                    //order.BillingContact = contact;

                    // Extract and save billing address (if provided)
                    if (billingAddress != null)
                    {
                        order.BillingAddress = new BillingAddress
                        {
                            Line1 = billingAddress["line1"]?.ToString(),
                            Line2 = billingAddress["line2"]?.ToString(),
                            City = billingAddress["city"]?.ToString(),
                            State = billingAddress["state"]?.ToString(),
                            PostalCode = billingAddress["postal_code"]?.ToString(),
                            Country = billingAddress["country"]?.ToString()
                        };
                    }

                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                //}
            }
            return Ok(); // Acknowledge Razorpay
        }



        private bool VerifyWebhookSignature(string payload, string razorpaySignature)
        {
            try
            {
                string RazorpayWebhookSecret = _configuration["RazorPay:WebhookSecret"];
                // Generate the HMAC SHA256 signature
                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(RazorpayWebhookSecret)))
                {
                    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                    var computedSignature = BitConverter.ToString(computedHash).Replace("-", "").ToLower();

                    // Compare the signatures
                    return computedSignature == razorpaySignature;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error verifying webhook signature: {ex.Message}");
                return false;
            }
        }

    }


}
