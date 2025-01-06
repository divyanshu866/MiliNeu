using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models.Services.Interfaces;
using Newtonsoft.Json;
using System.Security.Claims;


namespace MiliNeu.Models.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetOrCreateCartIdentifier()
        {
            var User = _httpContextAccessor.HttpContext.User;
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
        public async Task<string> GetLoggedInCartId(string userId)
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
        public string GetGuestCartId()
        {

            var sessionCart = _httpContextAccessor.HttpContext.Session.GetString("GuestCartId");

            if (string.IsNullOrEmpty(sessionCart))
            {
                // Generate a new GUID for the guest cart
                var newGuestCartId = Guid.NewGuid().ToString();
                _httpContextAccessor.HttpContext.Session.SetString("GuestCartId", newGuestCartId);

                // Save an empty cart object in session
                var emptyCart = new Cart { Items = new List<CartItem>() };
                _httpContextAccessor.HttpContext.Session.SetString("GuestCart", JsonConvert.SerializeObject(emptyCart));

                return newGuestCartId;
            }

            return sessionCart;
        }

        // Fetches cart details dynamically as needed
        public async Task<Cart> GetCartDetails(string cartIdentifier, bool includeItems = false, bool includeVariants = false, bool includeImages = false, bool includeColor = false)
        {
            var User = _httpContextAccessor.HttpContext.User;

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
                var sessionCart = _httpContextAccessor.HttpContext.Session.GetString("GuestCart");
                if (string.IsNullOrEmpty(sessionCart))
                {
                    return new Cart { Items = new List<CartItem>() };
                }

                return JsonConvert.DeserializeObject<Cart>(sessionCart) ?? new Cart { Items = new List<CartItem>() };
            }
        }
        public Cart? getSessionCart()
        {
            // Fetch guest user cart from session
            var sessionCart = _httpContextAccessor.HttpContext.Session.GetString("GuestCart");
            if (string.IsNullOrEmpty(sessionCart))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Cart>(sessionCart);

        }
    }
}
