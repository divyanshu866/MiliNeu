using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models;
using MiliNeu.Models.Services.Interfaces;
using MiliNeu.Models.ViewModels;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MiliNeu.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ICartService _cartService;


        public CartsController(ApplicationDbContext context, IConfiguration configuration, ICartService cartService)
        {
            _context = context;
            _configuration = configuration;
            _cartService = cartService;
        }

        // GET: Carts   
        [Authorize]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {


            return _context.Carts != null ?
                        View(await _context.Carts
                        .ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Carts'  is null.");
        }
        /*public async Task<string> GetOrCreateCartIdentifier()
        {
            var userId = User.Identity?.IsAuthenticated == true ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : null;

            if (userId != null)
            {
                // Logged-in user: return cart ID
                return await GetLoggedInCartId(userId);
            }
            else
            {
                // Guest user: return session-based identifier
                return GetGuestCartId();
            }
        }
        // Helper method to get logged-in cart ID
        private async Task<string> GetLoggedInCartId(string userId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

            if (cart == null)
            {
                cart = new Cart { ApplicationUserId = userId, Items = new List<CartItem>() };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart.Id.ToString();
        }

        // Helper method to get or create a guest cart ID
        private string GetGuestCartId()
        {
            var sessionCart = HttpContext.Session.GetString("GuestCartId");

            if (string.IsNullOrEmpty(sessionCart))
            {
                // Generate a new GUID for the guest cart
                var newGuestCartId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("GuestCartId", newGuestCartId);

                // Save an empty cart object in session
                var emptyCart = new Cart { Items = new List<CartItem>() };
                HttpContext.Session.SetString("GuestCart", JsonConvert.SerializeObject(emptyCart));

                return newGuestCartId;
            }

            return sessionCart;
        }

        // Fetches cart details dynamically as needed
        public async Task<Cart> GetCartDetails(string cartIdentifier, bool includeItems = false, bool includeVariants = false, bool includeImages = false, bool includeColor = false)
        {
            var userId = User.Identity?.IsAuthenticated == true ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : null;

            if (userId != null)
            {
                // Base query for fetching the cart
                var query = _context.Carts.AsQueryable();

                // Use a single Include chain to avoid duplicate includes
                if (includeItems || includeVariants || includeImages || includeColor)
                {
                    query = query.Include(c => c.Items).ThenInclude(c => c.Product);
                }

                if (includeVariants || includeImages || includeColor)
                {
                    query = query.Include(c => c.Items).ThenInclude(ci => ci.ProductVariant);
                }

                if (includeImages)
                {
                    query = query.Include(c => c.Items).ThenInclude(ci => ci.ProductVariant).ThenInclude(pv => pv.Images);
                }

                if (includeColor)
                {
                    query = query.Include(c => c.Items).ThenInclude(ci => ci.ProductVariant).ThenInclude(pv => pv.Color);
                }

                return await query.FirstOrDefaultAsync(c => c.ApplicationUserId == userId);
            }
            else
            {
                // Fetch guest user cart from session
                var sessionCart = HttpContext.Session.GetString("GuestCart");
                if (string.IsNullOrEmpty(sessionCart))
                {
                    return new Cart { Items = new List<CartItem>() };
                }

                return JsonConvert.DeserializeObject<Cart>(sessionCart) ?? new Cart { Items = new List<CartItem>() };
            }
        }*/
        public Cart? getSessionCart()
        {
            // Fetch guest user cart from session
            var sessionCart = HttpContext.Session.GetString("GuestCart");
            if (string.IsNullOrEmpty(sessionCart))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Cart>(sessionCart);

        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details()
        {
            /*if (id == null || _context.Carts == null)
            {
                return NotFound();
            }*/
            Cart? cart;
            try
            {
                string cartIdentifier = await _cartService.GetOrCreateCartIdentifier();

                cart = await _cartService.GetCartDetails(cartIdentifier, true, true, true, true);

            }
            catch (Exception)
            {

                return NotFound();
            }


            if (cart == null)
            {
                return NotFound();
            }
            if (cart.ApplicationUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid(); // Or redirect to an access denied page
            }


            string basepath = _configuration["BasePaths:ProductImageBasePath"];
            ViewData["ImageBasePath"] = basepath;

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
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityVM model)
        {
            if (model == null || model.Quantity < 1 || model.ProductId == 0 || string.IsNullOrEmpty(model.cartItemSize))
            {
                return BadRequest("Invalid quantity.");
            }

            string cartIdentifier = await _cartService.GetOrCreateCartIdentifier();

            Cart cart = await _cartService.GetCartDetails(cartIdentifier, true);


            // Fetch the cart item
            CartItem? cartItem = cart.Items
                .SingleOrDefault(ci => ci.Product.Id == model.ProductId && ci.ProductVariantId == model.variantId && ci.SelectedSize == model.cartItemSize);

            if (cartItem == null)
            {
                return NotFound("Cart item not found.");
            }

            //update Quantity
            cartItem.Quantity = model.Quantity;

            //For cart stored in cookie (Guest cart)
            if (cart.Id == 0)
            {
                UpdateSessionCart(cart);
            }
            //For cart stored in database (logged in user)
            else
            {
                // Save changes
                await _context.SaveChangesAsync();
            }





            // Calculate updated totals (optional)
            var subtotal = cartItem.Quantity * (cartItem.Product.DiscountedPrice > 0
                ? cartItem.Product.DiscountedPrice
                : cartItem.Product.Price);
            var total = cart.Items
                 .Where(ci => ci.Product != null)
                .Sum(ci => ci.Quantity * (ci.Product.DiscountedPrice > 0 ? ci.Product.DiscountedPrice : ci.Product.Price));


            return Json(new { subtotal, total });
        }

        // GET: Carts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
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
        public async Task<IActionResult> Delete(int cartId, int productId, int variantId, string selectedSize)
        {
            if (productId == 0)
            {
                return NotFound();
            }

            // Retrieve the cart from the database
            string cartIdentifier = await _cartService.GetOrCreateCartIdentifier();

            Cart cart = await _cartService.GetCartDetails(cartIdentifier, true, true);
            /*var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cartId);*/

            if (cart == null)
            {
                return NotFound();
            }

            // Find the CartItem associated with the productId in the cart
            var cartItem = cart.Items.FirstOrDefault(ci => ci.ProductId == productId && ci.ProductVariant.Id == variantId && ci.SelectedSize == selectedSize);

            if (cartItem == null)
            {
                return NotFound();
            }

            // Remove the CartItem from the cart
            cart.Items.Remove(cartItem);


            if (cart.Id == 0)
            {
                //If GuestCart
                UpdateSessionCart(cart);
            }
            else
            {
                // Check if there are changes before saving
                if (_context.ChangeTracker.HasChanges())
                {
                    // Save changes to the database
                    await _context.SaveChangesAsync();
                }
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
            if (_context.Carts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Carts'  is null.");
            }
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Carts/AddToCart/5

        public async Task<IActionResult> AddToCart(int productId, string SelectedSize, int selectedVariantId, int quantity)
        {



            // Retrieve the product from the database based on the given ID
            var product = await _context.Products
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Color)  // Include related Color entity
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Images) // Include related Images
                .Where(p => p.Id == productId && p.Variants.Any(v => v.Id == selectedVariantId))
                .SingleOrDefaultAsync();






            if (product == null)
            {
                return NotFound();
            }
            // Get the current user's ID
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);

            // Retrieve the user's cart from the database

            string cartIdentifier = await _cartService.GetOrCreateCartIdentifier();

            Cart cart = await _cartService.GetCartDetails(cartIdentifier, true, true);

            // Retrieve the user's cart from the database

            /*Cart? cart = await _context.Carts
                .Include(c => c.Items)
                *//*  .ThenInclude(c=>c.ProductVariant)*//*
                .SingleOrDefaultAsync(c => c.Id == cartId);*/

            // If the user doesn't have a cart, create a new one
            if (cart == null)
            {
                return NotFound();
            }




            CartItem? existingCartItem = cart.Items.SingleOrDefault(ci =>
                ci.ProductId == product.Id &&
                ci.SelectedSize == SelectedSize &&
                ci.ProductVariant.Id == selectedVariantId
                );

            // Add the product to the cart
            if (existingCartItem == null)
            {
                CartItem newCartItem = new CartItem();
                newCartItem.Cart = cart;
                newCartItem.Product = product;
                newCartItem.ProductId = product.Id;
                newCartItem.Quantity = quantity;
                newCartItem.CartId = cart.Id;
                newCartItem.SelectedSize = SelectedSize;
                // newCartItem.ProductVariantId = selectedVariantId;
                newCartItem.ProductVariantId = selectedVariantId;
                newCartItem.ProductVariant = product.Variants.SingleOrDefault(p => p.Id == selectedVariantId);
                cart.Items.Add(newCartItem);
            }
            else
            {
                existingCartItem.Quantity = existingCartItem.Quantity + quantity;
            }




            // Save changes to the database
            if (cart.Id == 0)
            {


                // Update the session cart for GuestCart
                if (UpdateSessionCart(cart))
                {
                    return RedirectToAction("Details", "Carts", new { id = cart.Id });
                }
                else
                {
                    return StatusCode(500, "Failed to update the session cart.");
                }

            }
            else
            {
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Carts", new { id = cart.Id }); // Redirect to the home page after adding the product to the cart




        }
        public async Task<bool> MergeGuestCart(string userId)
        {
            string cartIdentifier = await _cartService.GetOrCreateCartIdentifier();
            Cart? guestCart = getSessionCart();

            if (guestCart == null || guestCart.Items == null)
            {
                return true;
            }
            Cart? cart = await _context.Carts.SingleOrDefaultAsync(c => c.ApplicationUserId == userId);

            if (cart == null)
            {
                return true;
            }




            return true;
        }
        public bool UpdateSessionCart(Cart cart)
        {
            try
            {
                HttpContext.Session.SetString("GuestCart",
                JsonConvert.SerializeObject(cart, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                })
                );
                return true;
            }
            catch (Exception)
            {
                // Log the exception here
                Console.WriteLine($"Error updating session cart");
                return false;
            }

        }
        private bool CartExists(int id)
        {
            return (_context.Carts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
