namespace MiliNeu.Models
{
    public class OrderIssue
    {
        public int Id { get; set; }


        public string IssueType { get; set; } // Example: "Return Item", "Not Delivered"
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public string Status { get; set; } // Example: "Pending", "In Progress", "Resolved"
        public DateTime? ResolvedDate { get; set; }
        public string? AdditionalData { get; set; } // Optional, for storing extra information


        //Nav Props
        public int OrderId { get; set; }
        public Order Order { get; set; } // Navigation property
        public string? UserId { get; set; } // ID of the user reporting the issue
        public ApplicationUser User { get; set; } // Navigation property for the user

    }

}
