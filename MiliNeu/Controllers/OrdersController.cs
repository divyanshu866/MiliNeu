using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models.enums;
using MiliNeu.Models.Services.Interfaces;
using MiliNeu.Models.ViewModels;
using PaymentGateway;
using Order = MiliNeu.Models.Order;

namespace Milineu.Controllers
{

    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IRazorpayPaymentService _razorpayPaymentService;
        private readonly IOrderService _orderService;

        public OrdersController(ApplicationDbContext context, IConfiguration configuration, IRazorpayPaymentService razorpayPaymentService, IOrderService orderService)
        {
            _context = context;
            _configuration = configuration;
            _razorpayPaymentService = razorpayPaymentService;
            _orderService = orderService;
        }
        // GET: Manage/Orders
        // Action to list all orders
        public async Task<IActionResult> Manage(string filter = "All", string searchTerm = "", int pageNumber = 1, int pageSize = 50)
        {
            PagerVM<Order> viewModel = await _orderService.GetFilteredOrdersAsync(filter, searchTerm, pageNumber, pageSize);

            // Dropdown options for status
            ViewBag.FulfilmentStatusOptions = new List<string> {
                            DeliveryStatus.OrderPlaced.ToString(),
                            DeliveryStatus.Processing.ToString(),
                            DeliveryStatus.Dispatched.ToString(),
                            DeliveryStatus.Shipped.ToString(),
                            DeliveryStatus.OutForDelivery.ToString(),
                            DeliveryStatus.Delivered.ToString(),

                            DeliveryStatus.Canceled.ToString(),
                            DeliveryStatus.Failed.ToString(),
                            };

            // Dropdown options for status
            ViewBag.PaymentStatusOptions = new List<string> {
                            PaymentStatus.Pending.ToString(),
                            PaymentStatus.Confirmed.ToString(),
                            PaymentStatus.Refunded.ToString(),
                            };
            ViewBag.filter = filter;
            ViewBag.searchTerm = searchTerm;


            return View(viewModel);

        }



        [HttpPost]
        public async Task<IActionResult> UpdateStatus(List<int> selectedOrders, string newStatus)
        {
            bool updateSuccess = await _orderService.UpdateFulfilmentStatusAsync(selectedOrders, newStatus);

            if (!updateSuccess)
            {
                // Handle the failure case (e.g., redirect to an error page or display a message)
                return RedirectToAction("Index");
            }

            // Redirect to the Manage view with a success message or other context as needed
            return RedirectToAction(nameof(Manage), new { filter = newStatus });
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePaymentStatus(List<int> selectedOrders, string newStatus)
        {
            var changedBy = User.Identity.Name ?? "System"; // Capture user name or fallback

            bool updateSuccess = await _orderService.UpdatePaymentStatusAsync(selectedOrders, newStatus, changedBy);

            if (!updateSuccess)
            {
                // Handle the failure case (e.g., redirect to an error page or display a message)
                return RedirectToAction("Index");
            }

            // Redirect to the Manage view with a success message or other context as needed
            return RedirectToAction(nameof(Manage));
        }

        public async Task<IActionResult> AuditLog()
        {
            var auditLogs = await _context.PaymentStatusAudits
                                    .OrderByDescending(a => a.DateChanged)
                                    .ToListAsync();

            return View(auditLogs);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCheckout()
        {

            CheckoutVM checkoutVM = await _orderService.GetCheckoutDataAsync();
            if (checkoutVM == null) return NotFound();

            string basepath = _configuration["BasePaths:ThumbnailImageBasePath"];
            ViewData["ImageBasePath"] = basepath;
            ViewData["HideNavbarLinks"] = true; // Hide navbar links for this page
            ViewData["NavGrid"] = "grid-template-columns:1fr 1fr";
            return View("Checkout", checkoutVM);
        }

        [HttpPost]
        public async Task<IActionResult> PostCheckout(CheckoutVM checkoutVM)
        {

            Order order = await _orderService.CreateOrderAsync(checkoutVM);
            if (order == null)
            {
                return NotFound();
            }

            string basepath = _configuration["BasePaths:ThumbnailImageBasePath"];
            ViewData["ImageBasePath"] = basepath;

            // Redirect to a confirmation page or another action
            return RedirectToAction("GetInitiatePayment", new { orderId = order.Id });
            /* return Ok();*/

        }
        [HttpGet]
        public async Task<IActionResult> GetInitiatePayment(int orderId)
        {
            // Generate callback URL
            PaymentVM paymentVM = await _orderService.GetPaymentDetailsAsync(orderId);

            paymentVM.CallbackUrl = Url.Action("ThankYou", "Orders", null, Request.Scheme); ;
            if (paymentVM == null)
            {
                return NotFound();
            }


            string basepath = _configuration["BasePaths:ThumbnailImageBasePath"];
            ViewData["ImageBasePath"] = basepath;

            return View("PayNow", paymentVM);
        }
        // Action for successful payment


        public async Task<IActionResult> ThankYou()
        {

            return View();
        }






        public async Task updateUserConversion()
        {
            // Mark the visitor's session as converted
            string userId = Request.Cookies["UserId"]; // Get the user identifier from the cookie
            VisitorLog? visitorLog = await _context.VisitorLogs.SingleOrDefaultAsync(v => v.UserIdentifier == userId && !v.IsConverted);
            if (visitorLog != null)
            {
                visitorLog.IsConverted = true;

            }


        }
        // GET: OrdersController
        public async Task<IActionResult> Index(string userId)
        {
            IEnumerable<Order>? orders = await _orderService.GetUserOrdersAsync(userId);

            if (orders == null)
            {
                return NotFound();
            }

            string basepath = _configuration["BasePaths:ThumbnailImageBasePath"];
            ViewData["ImageBasePath"] = basepath;
            return View("Index", orders);
        }



        // GET: OrdersController/Details/5
        public async Task<ActionResult> Details(int orderId)
        {
            OrderDisplayVM? viewModel = await _orderService.GetUserOrderDetailsAsync(orderId);

            if (viewModel == null)
            {
                return NotFound();
            }

            // Pass order details to the view
            string? basepath = _configuration["BasePaths:ThumbnailImageBasePath"];
            ViewData["ImageBasePath"] = basepath;

            return View(viewModel);
        }
        // GET: OrdersController/OrderConfirmed
        public async Task<ActionResult> OrderConfirmed(int orderId)
        {
            return View();
        }
        // GET: OrdersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrdersController/Create
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

        // GET: OrdersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrdersController/Edit/5
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

        // GET: OrdersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrdersController/Delete/5
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


    }
}