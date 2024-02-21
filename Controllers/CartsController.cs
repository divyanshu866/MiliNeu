using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models;

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
        public async Task<IActionResult> Index()
        {
            return _context.Cart != null ?
                        View(await _context.Cart
                        .ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Cart'  is null.");
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(x => x.CartItems).ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> AddToCart(int id, string sizeSelected)
        {

            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {

                // Retrieve the product from the database based on the given ID
                var product = await _context.Product.FindAsync(id);
                var cart = new Cart();
                if (product == null)
                {
                    return NotFound();
                }
                // Get the current user's ID
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


                // Retrieve the user's cart from the database

                cart = await _context.Cart
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.ApplictaionUserId == userId);

                // If the user doesn't have a cart, create a new one
                if (cart == null)
                {
                    cart = new Cart { ApplictaionUserId = userId };
                    _context.Cart.Add(cart);
                }



                var existingCartItem = cart?.CartItems.FirstOrDefault(ci =>
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

                    cart.CartItems.Add(newCartItem);
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
