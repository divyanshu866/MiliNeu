namespace MiliNeu.Models.ViewModels
{
    public class SalesStatisticsVM
    {
        public decimal TotalSales { get; set; }
        public int LifeTimeUnitSold { get; set; }
        public Dictionary<string, decimal> SalesByProduct { get; set; } = new Dictionary<string, decimal>();
        public decimal AverageOrderValue { get; set; }
        public List<MonthlyTrafficVM> MonthlyTraffic { get; set; }
        public MonthlyTrafficVM CurrentMonthTraffic { get; set; }
        public TrafficStatsVM TrafficToday { get; set; }

        public TrafficEngagementVM TrafficEngagementVM { get; set; }
        public PagerVM<Order> OrderTable { get; set; }
    }
}