using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models.enums;
using MiliNeu.Models.Services.Interfaces;
using MiliNeu.Models.ViewModels;
using PaymentGateway;
using System.Security.Claims;

namespace MiliNeu.Models.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRazorpayPaymentService _razorpayPaymentService;
        private readonly IHttpContextAccessor _httpcontextAccessor;


        public OrderService(ApplicationDbContext context, IRazorpayPaymentService razorpayPaymentService, IHttpContextAccessor httpcontextAccessor)
        {
            _context = context;
            _razorpayPaymentService = razorpayPaymentService;
            _httpcontextAccessor = httpcontextAccessor;
        }

        public async Task<PagerVM<Order>> GetFilteredOrdersAsync(string filter, string searchTerm, int pageNumber, int pageSize)
        {
            IQueryable<Order> query = _context.Orders.Include(o => o.Items).Include(o => o.User);

            if (Enum.TryParse(filter, out DeliveryStatus deliveryStatus))
            {
                query = query.Where(o => o.DeliveryStatus == deliveryStatus);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(o => o.Id.ToString().Contains(searchTerm) ||
                                         o.User.NormalizedUserName.Contains(searchTerm) ||
                                         o.User.Email.Contains(searchTerm) ||
                                         o.RazorReceiptId.Contains(searchTerm) ||
                                         o.RazorOrderId.Contains(searchTerm) ||
                                         o.RazorPaymentId.Contains(searchTerm));
            }
            query = query.OrderByDescending(o => o.Id);

            int totalOrders = await query.CountAsync();
            var orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagerVM<Order>
            {
                Items = orders,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalOrders / pageSize)
            };
        }
        public async Task<bool> UpdateFulfilmentStatusAsync(List<int> selectedOrders, string newStatus)
        {
            if (selectedOrders == null || selectedOrders.Count == 0 || string.IsNullOrEmpty(newStatus))
            {
                // Handle case where no orders were selected or no status was selected
                return false; // Or return an error message
            }

            // Attempt to parse the new status string to DeliveryStatus enum
            if (!Enum.TryParse(newStatus, out DeliveryStatus deliveryStatus))
            {
                // If parsing fails, return with error or default action
                return false;
            }

            foreach (var orderId in selectedOrders)
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order != null)
                {
                    // Update the order's status
                    if (order.DeliveryStatus != deliveryStatus)
                    {
                        order.DeliveryStatus = deliveryStatus;
                    }

                }
            }

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Redirect to the order list or display a success message
            return true;
        }
        public async Task<bool> UpdatePaymentStatusAsync(List<int> selectedOrders, string newStatus, string changedBy)
        {
            if (selectedOrders == null || selectedOrders.Count == 0 || string.IsNullOrEmpty(newStatus))
            {
                // Handle case where no orders were selected or no status was selected
                return false; // Or return an error message
            }

            // Attempt to parse the new status string to DeliveryStatus enum
            if (!Enum.TryParse(newStatus, out PaymentStatus paymentStatus))
            {
                // If parsing fails, return with error or default action
                return false;
            }

            foreach (var orderId in selectedOrders)
            {
                Order? order = await _context.Orders.FindAsync(orderId);
                if (order != null && order.PaymentStatus != paymentStatus)
                {



                    // Create the audit entry
                    var auditEntry = new PaymentStatusAudit
                    {
                        OrderId = order.Id,
                        OldStatus = order.PaymentStatus,
                        NewStatus = paymentStatus,
                        DateChanged = DateTime.UtcNow,
                        ChangedBy = changedBy // Capture the current admin user

                    };

                    // Add the audit entry to the PaymentStatusAudit table
                    _context.PaymentStatusAudits.Add(auditEntry);

                    // Update the order's status
                    order.PaymentStatus = paymentStatus;
                }
            }

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Redirect to the order list or display a success message
            return true;
        }
        public async Task<CheckoutVM> GetCheckoutDataAsync()
        {
            ApplicationUser? user = await getUserWithCart();
            if (user == null || user.Cart == null || user.Cart.Items.Count < 1)
            {
                return null;
            }
            // Just calculate and show the cart total, no need to create an order here
            List<CartItem> cartItems = user.Cart.Items;


            decimal totalDiscount = 0, subTotal = 0, total = 0, shipping = 500;

            foreach (var item in cartItems)
            {
                if (item.Product.DiscountedPrice > 0)
                {
                    totalDiscount += ((decimal)item.Product.Price - (decimal)item.Product.DiscountedPrice) * item.Quantity;
                    subTotal += ((decimal)item.Product.DiscountedPrice) * item.Quantity;
                }
                else
                {
                    subTotal += (decimal)item.Product.Price;
                }
            }
            total = subTotal + shipping;

            /*Save address always false to prevent saving address*/
            CheckoutVM checkoutVM = new CheckoutVM
            {
                CartItems = cartItems,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                EstimatedDelivery = DateTime.UtcNow.AddDays(7).ToString("dddd, dd MMMM, yyyy"),
                Subtotal = subTotal,
                Total = total,
                Discount = totalDiscount,
                Shipping = 500,
                AddressVM = new AddressVM { Addresses = user.Addresses }
            };

            return checkoutVM;
        }
        public DateTime getEstimatedDeliveryDate()
        {
            return DateTime.UtcNow.AddDays(7);

        }
        public async Task<Order> CreateOrderAsync(CheckoutVM checkoutVM)
        {
            ApplicationUser? user = await getUserWithCart();

            if (user == null || user.Cart == null || user.Cart.Items.Count < 1)
            {
                return null;
            }
            if (checkoutVM.EstimatedDelivery != getEstimatedDeliveryDate().ToString("dddd, dd MMMM, yyyy"))
            {
                return null;
            }
            OrderAddress? orderAddress;
            // Create the new order
            if (checkoutVM.AddressVM.SelectedAddressId > 0)
            {
                Address? address = await _context.Address.SingleOrDefaultAsync(c => c.Id == checkoutVM.AddressVM.SelectedAddressId);

                if (address != null && address.User.Id == user.Id)
                {
                    orderAddress = MapToOrderAddress(address);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                orderAddress = new OrderAddress
                {
                    FirstName = checkoutVM.AddressVM.NewAddress.FirstName,
                    LastName = checkoutVM.AddressVM.NewAddress.LastName,
                    City = checkoutVM.AddressVM.NewAddress.City,
                    PostalCode = checkoutVM.AddressVM.NewAddress.PostalCode,
                    ApartmentSuite = checkoutVM.AddressVM.NewAddress.ApartmentSuite,
                    Email = user.Email,/* Temproary Fix Temproary Fix Temproary Fix Temproary Fix Temproary Fix */
                    PhoneNumber = checkoutVM.AddressVM.NewAddress.PhoneNumber,
                    State = checkoutVM.AddressVM.NewAddress.State,
                    Country = "India",
                    StreetAddress = checkoutVM.AddressVM.NewAddress.StreetAddress
                };

                if (checkoutVM.AddressVM.SaveAddress == true && false)
                {

                    Address address = new Address()
                    {
                        User = user,
                        UserId = user.Id,
                        FirstName = checkoutVM.AddressVM.NewAddress.FirstName,
                        LastName = checkoutVM.AddressVM.NewAddress.LastName,
                        City = checkoutVM.AddressVM.NewAddress.City,
                        PostalCode = checkoutVM.AddressVM.NewAddress.PostalCode,
                        ApartmentSuite = checkoutVM.AddressVM.NewAddress.ApartmentSuite,
                        Email = user.Email,/* Temproary Fix Temproary Fix Temproary Fix Temproary Fix Temproary Fix */
                        PhoneNumber = checkoutVM.AddressVM.NewAddress.PhoneNumber,
                        State = checkoutVM.AddressVM.NewAddress.State,
                        Country = "India",
                        StreetAddress = checkoutVM.AddressVM.NewAddress.StreetAddress,
                        IsPrimary = false
                    };

                    if (user.Addresses.Count == 0)
                    {
                        address.IsPrimary = true;
                    }
                    _context.Address.Add(address);
                }

            }
            Order order = new Order
            {
                User = user,
                Items = new List<OrderItem>(),
                EstimatedDeliveryBy = getEstimatedDeliveryDate(),
                OrderStatus = OrderStatus.Active,
                PaymentStatus = PaymentStatus.Pending,
                DeliveryStatus = DeliveryStatus.OrderPlaced,
                Currency = "INR",
                CustomerNotes = checkoutVM.CustomerNotes,
                ShippingProvider = "Delhivery",
                ShippingAddress = orderAddress,
                //BillingAddress = null,
            };


            order = MapOrderItems(order, user.Cart);
            order.CalculatePrice();
            string receiptId = GenerateReceiptId();

            var razorOrder = _razorpayPaymentService.CreateOrder(order.Amount, order.Currency, receiptId);

            order.RazorOrderId = razorOrder["id"];
            order.RazorReceiptId = receiptId;


            //order.RazorOrderId = "RazorOrderId 4543dfvd";
            //order.RazorReceiptId = "RazorReceiptId 76543fdvf";


            user.UserOrders.Add(order);         // Add the order to user's orders and clear the cart
            user.Cart.Items.Clear();            //Empty cart after checked out

            await _context.SaveChangesAsync();  // Persist changes to the database

            return order;

        }

        public async Task<PaymentVM> GetPaymentDetailsAsync(int orderId, string callbackUrl)
        {


            string? userId = _httpcontextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return null;
            }


            // Retrieve the order by orderId from the database
            Order? order = await _context.Orders
                .Include(c => c.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null || order.User.Id != userId)
            {
                return null;
            }
            PaymentVM paymentVM = new PaymentVM
            {
                Email = order.User.Email,
                PhoneNumber = order.User.PhoneNumber,
                Amount = order.Amount,
                Currency = order.Currency,
                UserName = order.User.UserName,
                RazorOrderId = order.RazorOrderId,
                RazorpayKey = _razorpayPaymentService.GetKey(),
                /*CallbackUrl = Passed from Controller*/
            };



            return paymentVM;
        }
        public async Task<List<Order>> GetUserOrdersAsync(string userId)
        {
            if (userId == null || userId != _httpcontextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return null;
            }

            ApplicationUser? user = await _context.Users
                    .IgnoreQueryFilters()
                .Include(o => o.UserOrders)
                .ThenInclude(c => c.ShippingAddress)
                .Include(o => o.UserOrders)
                .ThenInclude(i => i.Items)
                .ThenInclude(p => p.Product)
                  .ThenInclude(i => i.Variants)
                .ThenInclude(i => i.Images)
                .SingleAsync(u => u.Id == userId);



            return user.UserOrders;
        }

        public async Task<OrderDisplayVM> GetUserOrderDetailsAsync(int orderId)
        {
            if (orderId < 1)
            {
                return null;
            }


            Order? order = await _context.Orders
                .IgnoreQueryFilters()
           .Include(u => u.User)
           .ThenInclude(u => u.Addresses)
           .Include(u => u.ShippingAddress)
           .Include(i => i.OrderIssues)
           .Include(i => i.Items)
                .ThenInclude(p => p.Product)
                .ThenInclude(i => i.Variants)
                .ThenInclude(i => i.Images)
           .Include(i => i.Items)
                .ThenInclude(p => p.Product)
                .ThenInclude(i => i.Variants)
                .ThenInclude(i => i.Color)

           .SingleAsync(o => o.Id == orderId);

            if (order == null || order.User.Id != _httpcontextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return null;
            }

            OrderDisplayVM viewModel = new OrderDisplayVM
            {
                OrderId = orderId,
                TrackingNumber = "1900020",
                BillingAddress = order.ShippingAddress,
                EstimatedDeliveryDate = order.EstimatedDeliveryBy.ToString("dddd, dd MMMM, yyyy"),
                DeliveryStatus = order.DeliveryStatus,
                ReturnStatus = order.ReturnStatus,
                ReturnInitiatedDate = order.ReturnInitiatedDate,
                OrderDate = order.CreatedAt,
                PaymentStatus = order.PaymentStatus.ToString(),
                Shipping = 500,
                AddressVM = new AddressVM { Addresses = order.User.Addresses, SelectedAddressSnapshot = order.ShippingAddress },
                ShippingAddress = order.ShippingAddress,
                AmountCharged = order.Amount,
                Email = order.User.Email,
                PhoneNumber = order.User.PhoneNumber,
                TotalDiscount = order.DiscountApplied,
                Username = order.User.UserName,
                Items = new List<OrderItemDisplayVM>()
            };
            foreach (var item in order.Items)
            {
                OrderItemDisplayVM orderItemDisplayVM = new OrderItemDisplayVM
                {
                    ProductName = item.Product.Name,
                    VariantName = item.ProductVariant.Color.Name,
                    Size = item.SelectedSize,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    DiscountedPrice = item.DiscountedPrice != null ? (decimal)item.DiscountedPrice : 0,
                    Subtotal = item.DiscountedPrice > 0 || item.DiscountedPrice != null ? ((decimal)item.DiscountedPrice * item.Quantity) : (item.Price * item.Quantity),
                    Images = item.ProductVariant.Images

                };
                viewModel.Items.Add(orderItemDisplayVM);

            }


            return viewModel;
        }

        public OrderAddress MapToOrderAddress(Address address)
        {
            return new OrderAddress
            {
                FirstName = address.FirstName,
                LastName = address.LastName,
                StreetAddress = address.StreetAddress,
                ApartmentSuite = address.ApartmentSuite,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country,
                PhoneNumber = address.PhoneNumber,
                Email = address.Email

            };
        }

        public async Task<ApplicationUser> getUserWithCart()
        {
            string? userId = _httpcontextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                // Find the user again
                return await _context.Users
                    .Include(c => c.Addresses)
                    .Include(c => c.Cart)
                    .ThenInclude(i => i.Items)
                        .ThenInclude(p => p.Product)
                        .ThenInclude(p => p.Variants)
                        .ThenInclude(v => v.Images)
                    .Include(c => c.Cart)
                        .ThenInclude(i => i.Items)
                        .ThenInclude(p => p.Product)
                        .ThenInclude(p => p.Variants)
                        .ThenInclude(v => v.Color)      // Include Color separately, without nesting under Variants
                    .SingleOrDefaultAsync(u => u.Id == userId);
            }
            else
            {
                return null;
            }


        }
        public string GenerateReceiptId()
        {
            return $"MIL_{Guid.NewGuid().ToString()}";
        }
        public Order MapOrderItems(Order order, Cart cart)
        {

            order.Items = new List<OrderItem>();


            foreach (var item in cart.Items)
            {
                OrderItem orderItem = new OrderItem();
                orderItem.Product = item.Product;
                orderItem.Quantity = item.Quantity;
                orderItem.Price = item.Product.Price;
                orderItem.DiscountedPrice = item.Product.DiscountedPrice;
                orderItem.SelectedSize = item.SelectedSize;
                orderItem.ProductVariant = item.ProductVariant;
                orderItem.ProductVariantId = item.ProductVariantId;

                orderItem.calculatePrice();
                order.Items.Add(orderItem);

                //Update SalesCount of products bought
                orderItem.Product.SalesCount += orderItem.Quantity;

            }
            return order;
        }
        public async void HandleAbandonedOrders()
        {
            var pendingOrders = await _context.Orders.Where(o => o.PaymentStatus == PaymentStatus.Pending && o.CreatedAt >= DateTime.UtcNow.AddDays(-3)).ToListAsync();

            foreach (var order in pendingOrders)
            {
                order.OrderStatus = OrderStatus.Expired;
            }

            _context.SaveChanges();
        }

        public async void CleanupExpiredOrders()
        {
            var expiredOrders = await _context.Orders.Where(o => o.PaymentStatus == PaymentStatus.Pending && o.OrderStatus == OrderStatus.Expired && o.CreatedAt <= DateTime.UtcNow.AddMonths(-1)).ToListAsync();

            foreach (var order in expiredOrders)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
        }
    }


}
