using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models;
using System.Security.Claims;

namespace MiliNeu.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Carts   
        [Authorize]
        public async Task<IActionResult> Index()
        {


            return _context.Cart != null ?
                        View(await _context.Cart
                        .ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Cart'  is null.");
        }

        // GET: Carts/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }
            Cart cart;
            ApplicationUser user;
            try
            {
                user = await _context.Users.SingleAsync(u => u.CartId == id);
                cart = await _context.Cart
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            }
            catch (Exception)
            {

                return NotFound();
            }


            if (cart == null)
            {
                return NotFound();
            }
            if (user.Id != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid(); // Or redirect to an access denied page
            }



            return View(cart);
        }

        // GET: Carts/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,ApplictaionUserId")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

        // GET: Carts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApplictaionUserId")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

        // GET: Carts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int cartId, int productId)
        {
            if (cartId == 0 || productId == 0)
            {
                return NotFound();
            }

            // Retrieve the cart from the database
            var cart = await _context.Cart
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null)
            {
                return NotFound();
            }

            // Find the CartItem associated with the productId in the cart
            var cartItem = cart.Items.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem == null)
            {
                return NotFound();
            }

            // Remove the CartItem from the cart
            cart.Items.Remove(cartItem);

            // Check if there are changes before saving
            if (_context.ChangeTracker.HasChanges())
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
            }

            // Redirect to the appropriate action or view
            return RedirectToAction("Details", "Carts", new { id = cartId });
        }


        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cart == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cart'  is null.");
            }
            var cart = await _context.Cart.FindAsync(id);
            if (cart != null)
            {
                _context.Cart.Remove(cart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Cart/AddToCart/5
        [Authorize]
        public async Task<IActionResult> AddToCart(int productId, string sizeSelected)
        {

            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {

                // Retrieve the product from the database based on the given ID
                var product = await _context.Product.FindAsync(productId);
                var cart = new Cart();
                if (product == null)
                {
                    return NotFound();
                }
                // Get the current user's ID
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ApplicationUser user = _context.Users.SingleOrDefault(u => u.Id == userId);

                // Retrieve the user's cart from the database

                cart = await _context.Cart
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.User == user);

                // If the user doesn't have a cart, create a new one
                if (cart == null)
                {
                    cart = new Cart { User = user };
                    _context.Cart.Add(cart);
                }



                var existingCartItem = cart?.Items.FirstOrDefault(ci =>
                    ci.ProductId == product.Id &&
                    ci.SelectedSize == sizeSelected);
                // Add the product to the cart
                if (existingCartItem == null)
                {
                    CartItem newCartItem = new CartItem();
                    newCartItem.Cart = cart;
                    newCartItem.Product = product;
                    newCartItem.ProductId = product.Id;
                    newCartItem.Quantity = 1;
                    newCartItem.CartId = cart.Id;
                    newCartItem.SelectedSize = sizeSelected;

                    cart.Items.Add(newCartItem);
                }
                else
                {
                    existingCartItem.Quantity++;
                }




                // Save changes to the database
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Carts", new { id = cart.Id }); // Redirect to the home page after adding the product to the cart

            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });

            }


        }

        private bool CartExists(int id)
        {
            return (_context.Cart?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
