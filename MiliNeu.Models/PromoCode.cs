namespace MiliNeu.Models
{
    public class PromoCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal DiscountAmount { get; set; } // e.g., 10 for $10 off or 0.10 for 10% off
        public bool IsPercentage { get; set; } // If true, apply as percentage; otherwise, fixed amount
        public DateTime ExpiryDate { get; set; } // Promo expiration
        public bool IsActive { get; set; } // To easily toggle promo code
    }

}
