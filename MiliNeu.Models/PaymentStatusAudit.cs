using MiliNeu.Models.enums;

namespace MiliNeu.Models
{
    public class PaymentStatusAudit
    {
        public int Id { get; set; }

        // Foreign Key for Order
        public int OrderId { get; set; }
        public Order Order { get; set; } // Navigation Property

        public PaymentStatus OldStatus { get; set; }
        public PaymentStatus NewStatus { get; set; }
        public DateTime DateChanged { get; set; }
        public string ChangedBy { get; set; } // Optionally track the admin user
    }

}
