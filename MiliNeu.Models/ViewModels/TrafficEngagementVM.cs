namespace MiliNeu.Models.ViewModels
{
    public class TrafficEngagementVM
    {
        public int TotalVisitors { get; set; } // Total number of distinct visitors

        public int TotalConversions { get; set; } // Total number of conversions (orders placed)

        public decimal ConversionRate { get; set; } // Conversion rate (Total Conversions / Total Visitors * 100)

        public decimal BounceRate { get; set; } // Bounce rate (Single-page sessions / Total Sessions * 100)

        public int TotalPageViews { get; set; } // Total number of page views

        public int UniqueSessions { get; set; } // Number of unique sessions
    }

}
