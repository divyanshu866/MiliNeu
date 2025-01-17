using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models.enums;
using MiliNeu.Models.Services.Interfaces;
using MiliNeu.Models.ViewModels;

namespace MiliNeu.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;

        public DashboardController(ApplicationDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        // GET: DashboardController
        public async Task<IActionResult> Dashboard()
        {
            var orderTable = await _orderService.GetFilteredOrdersAsync("All", "", 1, 10);
            // Dropdown options for status
            ViewBag.StatusOptions = new List<string> {
                DeliveryStatus.Processing.ToString(),
                DeliveryStatus.Shipped.ToString(),
                DeliveryStatus.Delivered.ToString(),
                DeliveryStatus.Canceled.ToString(),
                DeliveryStatus.Failed.ToString(),
                };

            ViewBag.filter = "All";
            ViewBag.searchTerm = "";















            var model = new SalesStatisticsVM();

            // Total Sales
            model.TotalSales = Math.Round(await _context.Orders.SumAsync(o => o.Amount - o.DiscountApplied), 2);

            // Sales by Product
            var salesByProduct = _context.OrderItems
                .GroupBy(oi => oi.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalSales = g.Sum(oi => (oi.DiscountedPrice > 0 ? oi.DiscountedPrice : oi.Price) * oi.Quantity)
                }).ToList();

            foreach (var item in salesByProduct)
            {
                // Assuming you have a way to get product name from ProductId
                var productName = GetProductName(item.ProductId); // Implement this method to get the product name
                model.SalesByProduct[productName] = (decimal)item.TotalSales;
            }
            model.TrafficEngagementVM = await GetTrafficAndEngagementAsync();
            model.CurrentMonthTraffic = await GetCurrentMonthTrafficAsync();
            model.LifeTimeUnitSold = await GetLifetimeUnitSoldAsync();
            model.TrafficToday = await GetTrafficTodayAsync();
            model.OrderTable = orderTable;
            // Average Order Value
            var totalOrders = await _context.Orders.CountAsync();
            model.AverageOrderValue = totalOrders > 0 ? Math.Round(model.TotalSales / totalOrders, 2) : 0;

            return View(model);
        }
        public async Task<int> GetLifetimeUnitSoldAsync()
        {
            int totalUnitsSold = 0;
            List<int> quantities = await _context.OrderItems.Select(g => g.Quantity).ToListAsync();
            if (quantities != null && quantities.Count > 0)
            {
                foreach (var quantity in quantities)
                {
                    totalUnitsSold += quantity;
                }
            }
            return totalUnitsSold;
        }
        public async Task<MonthlyTrafficVM> GetCurrentMonthTrafficAsync()
        {
            // Get the current year and month
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            // Query to get the traffic for the current month
            var currentMonthTraffic = await _context.VisitorLogs
                .Where(v => v.VisitDate.Year == currentYear && v.VisitDate.Month == currentMonth) // Filter for current year and month
                .GroupBy(v => new { v.VisitDate.Year, v.VisitDate.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    TotalVisits = g.Count(),
                    TotalConverted = g.Count(v => v.IsConverted)
                })
                .FirstOrDefaultAsync(); // Get only the current month's data

            // Return the view model formatted for the current month
            var monthlyTraffic = new MonthlyTrafficVM
            {
                Month = $"{currentYear}-{currentMonth:D2}", // Format as "yyyy-MM"
                TotalVisits = currentMonthTraffic?.TotalVisits ?? 0, // Handle null case if no visits
                TotalConverted = currentMonthTraffic?.TotalConverted ?? 0
            };

            return monthlyTraffic;
        }

        public async Task<List<MonthlyTrafficVM>> GetMonthlyTrafficAsync()
        {

            // Query the data and group by Year and Month
            var rawMonthlyTraffic = await _context.VisitorLogs
                .GroupBy(v => new { Year = v.VisitDate.Year, Month = v.VisitDate.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    TotalVisits = g.Count(),
                    TotalConverted = g.Count(v => v.IsConverted)
                })
                .OrderBy(e => e.Year).ThenBy(e => e.Month) // Order by Year and Month
                .ToListAsync();

            // Project to MonthlyTrafficViewModel and format the Month string
            var monthlyTraffic = rawMonthlyTraffic
                .Select(r => new MonthlyTrafficVM
                {
                    Month = $"{r.Year}-{r.Month:D2}", // Format as "yyyy-MM"
                    TotalVisits = r.TotalVisits,
                    TotalConverted = r.TotalConverted
                })
                .ToList();

            return monthlyTraffic;

        }
        public async Task<TrafficStatsVM> GetTrafficTodayAsync()
        {
            // Get today's date
            var today = DateTime.Today;

            // Query to get today's traffic
            var todayTraffic = await _context.VisitorLogs
                .Where(v => v.VisitDate.Date == today) // Filter for today's date
                .GroupBy(v => new { v.VisitDate.Year, v.VisitDate.Month, v.VisitDate.Day })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    g.Key.Day,
                    TotalVisits = g.Count(),
                    TotalConverted = g.Count(v => v.IsConverted)
                })
                .FirstOrDefaultAsync(); // Get only today's data

            // Return the traffic statistics formatted for today
            var trafficStats = new TrafficStatsVM
            {
                Date = today.ToString("yyyy-MM-dd"), // Format as "yyyy-MM-dd"
                TotalVisits = todayTraffic?.TotalVisits ?? 0, // Handle null case if no visits
                TotalConverted = todayTraffic?.TotalConverted ?? 0
            };

            return trafficStats;
        }


        public async Task<TrafficEngagementVM> GetTrafficAndEngagementAsync()
        {
            var model = new TrafficEngagementVM();

            // Calculate total distinct visitors using unique UserIdentifier
            model.TotalVisitors = await _context.VisitorLogs.CountAsync();

            // Total Conversions
            model.TotalConversions = await _context.VisitorLogs.CountAsync(v => v.IsConverted);

            // Conversion Rate
            model.ConversionRate = model.TotalVisitors > 0
                ? Math.Round((decimal)model.TotalConversions / model.TotalVisitors * 100, 2)
                : 0;

            // Bounce Rate: Users with only one visit
            var singlePageUsers = await _context.VisitorLogs
                .GroupBy(v => v.UserIdentifier)
                .CountAsync(g => g.Count() == 1);

            model.BounceRate = model.TotalVisitors > 0
                ? (decimal)singlePageUsers / model.TotalVisitors * 100
                : 0;

            // Total Page Views
            model.TotalPageViews = await _context.VisitorLogs.CountAsync();

            // Unique Sessions
            model.UniqueSessions = await _context.VisitorLogs
                .Select(v => v.UserIdentifier)
                .Distinct()
                .CountAsync();

            return model;
        }
        public string GetProductName(int id)
        {
            string productName = _context.Products.SingleOrDefault(i => i.Id == id).Name;

            return productName;
        }
        // GET: DashboardController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DashboardController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DashboardController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DashboardController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DashboardController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DashboardController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DashboardController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public IActionResult AdminDashboard()
        {
            // Sample data (replace with real data from your database)
            var dashboardData = new AdminDashboardViewModel
            {
                TotalSales = 15200.50m,
                AverageOrderValue = 120.75m,
                TotalOrders = 200,
                OrdersProcessing = 15,
                OrdersPending = 10,
                TotalCustomers = 500,
                NewCustomers = 50,
                ReturningCustomers = 450,
                TotalProducts = 300,
                LowStockProducts = new List<string> { "Product A", "Product B", "Product C" },
                TopSellingProducts = new List<string> { "Product X", "Product Y", "Product Z" },
                TotalVisitors = 1500,
                ConversionRate = 2.5m,
                BounceRate = 35.0m,
                TotalReviews = 120,
                AverageRating = 4.5,
                DiscountCodeUsage = 30,
                RevenueFromPromotions = 3200.00m
            };

            return View(dashboardData);
        }
    }
}
