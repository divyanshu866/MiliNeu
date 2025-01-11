using MiliNeu.Models.enums;
using System.ComponentModel.DataAnnotations;

namespace MiliNeu.Models
{


    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public ApplicationUser User { get; set; }
        public DateTime EstimatedDeliveryBy { get; set; }
        [Required]
        public decimal Amount { get; set; }

        //public decimal ShippingCost { get; set; }
        //public decimal TaxAmount { get; set; }
        [Required]
        public decimal DiscountApplied { get; set; }
        public string Currency { get; set; }
        public bool? PaymentCaptured { get; set; }

        public OrderStatus OrderStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public ReturnStatus? ReturnStatus { get; set; }
        public DateTime? ReturnInitiatedDate { get; set; }
        // public PaymentMethod PaymentMethod { get; set; }     //Enum


        //Payment Information
        public string? RazorOrderId { get; set; } // Store Razorpay's Order ID
        public string? RazorReceiptId { get; set; } // Optional: Store unique receipt ID
        public string? RazorPaymentId { get; set; }
        public string? RazorPaymentSignature { get; set; }


        //Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; } // Null until payment is made
                                              //public DateTime? EstimatedDeliveryDate { get; set; }
                                              //public DateTime? ActualDeliveryDate { get; set; }

        //Address Information
        public int ShippingAddressId { get; set; }
        public OrderAddress ShippingAddress { get; set; }

        //public int? BillingAddressId { get; set; }
        //public OrderAddress? BillingAddress { get; set; }


        // Order and Shipping Information
        public string? TrackingNumber { get; set; }
        public string? ShippingProvider { get; set; }


        // Additional Notes
        public string? CustomerNotes { get; set; }
        public string? InternalNotes { get; set; }

        // Discount Details
        //public string? CouponCode { get; set; }
        //public decimal DiscountAmount { get; set; }

        // Navigation Property
        [Required]
        public List<OrderItem> Items { get; set; }

        public List<OrderIssue>? OrderIssues { get; set; }

        // Navigation property for audits
        public ICollection<PaymentStatusAudit> PaymentStatusAudits { get; set; }

        public Order()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public void CalculatePrice()
        {
            if (Items != null && Items.Count > 0)
            {
                decimal totalPrice = 0;
                decimal totalDiscount = 0;

                foreach (var orderItem in Items)
                {

                    // Check for discounted price or fallback to regular price
                    if (orderItem.DiscountedPrice > 0)
                    {
                        totalPrice = totalPrice + ((decimal)orderItem.DiscountedPrice) * orderItem.Quantity;
                        totalDiscount = totalDiscount + (orderItem.Price - (decimal)orderItem.DiscountedPrice) * orderItem.Quantity;
                    }
                    else
                    {
                        totalPrice = totalPrice + ((decimal)orderItem.Price) * orderItem.Quantity;

                    }
                }

                Amount = totalPrice;
                DiscountApplied = totalDiscount; // Calculate discount amount
            }
        }
    }

}
