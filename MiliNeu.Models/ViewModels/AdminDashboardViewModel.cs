namespace MiliNeu.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        // Sales Statistics
        public decimal TotalSales { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int TotalOrders { get; set; }
        public int OrdersProcessing { get; set; }
        public int OrdersPending { get; set; }

        // Customer Insights
        public int TotalCustomers { get; set; }
        public int NewCustomers { get; set; }
        public int ReturningCustomers { get; set; }

        // Inventory Management
        public int TotalProducts { get; set; }
        public List<string> LowStockProducts { get; set; } = new List<string>();
        public List<string> TopSellingProducts { get; set; } = new List<string>();

        // Site Traffic and Performance
        public int TotalVisitors { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal BounceRate { get; set; }

        // User Feedback and Reviews
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }

        // Marketing Performance
        public int DiscountCodeUsage { get; set; }
        public decimal RevenueFromPromotions { get; set; }
    }
}
