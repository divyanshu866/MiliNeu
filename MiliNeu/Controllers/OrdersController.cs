using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models;
using System.Security.Claims;

namespace Milineu.Controllers
{

    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Checkout(int cartId)
        {
            ApplicationUser user = await _context.Users.Include(o => o.UserOrders).ThenInclude(i => i.Items).ThenInclude(p => p.Product).SingleOrDefaultAsync(u => u.CartId == cartId);
            Cart cart = await _context.Cart.Include(i => i.Items).ThenInclude(p => p.Product).SingleOrDefaultAsync(c => c.Id == cartId);

            Order order = new Order();
            order.ApplicationUser = user;
            order.Items = new List<OrderItem>();

            foreach (var item in cart.Items)
            {
                OrderItem orderItem = new OrderItem();
                orderItem.Product = item.Product;
                orderItem.Quantity = item.Quantity;
                orderItem.Price = item.Price;
                orderItem.SelectedSize = item.SelectedSize;
                orderItem.calculatePrice();
                order.Items.Add(orderItem);
            }
            user.UserOrders.Add(order);

            //Empty cart after checked out
            user.Cart.Items.Clear();
            _context.SaveChanges();


            return View("Index", user.UserOrders);
        }
        // GET: OrdersController
        public async Task<IActionResult> Index(string userId)
        {
            ApplicationUser user;
            try
            {
                user = await _context.Users
                .Include(o => o.UserOrders)
                .ThenInclude(i => i.Items)
                .ThenInclude(p => p.Product)
                .SingleAsync(u => u.Id == userId);
            }
            catch (Exception)
            {

                return NotFound();
            }


            if (userId == null || user == null)
            {
                return NotFound();
            }
            if (userId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid(); // Or redirect to an access denied page
            }
            return View("Index", user.UserOrders);
        }


        // GET: OrdersController/Details/5
        public async Task<ActionResult> Details(int orderId)
        {
            if (orderId == null)
            {
                return NotFound();
            }

            Order order = new Order();

            order = await _context.Order
           .Include(u => u.ApplicationUser)
           .Include(i => i.Items)
               .ThenInclude(p => p.Product)
           .SingleAsync(o => o.Id == orderId);

            if (order.ApplicationUser.Id != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid(); // Or redirect to an access denied page
            }





            return View(order);
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
