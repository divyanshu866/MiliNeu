namespace MiliNeu.Models.ViewModels
{
    public class MonthlyTrafficVM
    {
        public string Month { get; set; } // The month in "yyyy-MM" format
        public int TotalVisits { get; set; } // Total visits in that month
        public int TotalConverted { get; set; } // Total conversions in that month
    }

}
