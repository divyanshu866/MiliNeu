namespace MiliNeu.Models.Services.Interfaces
{
    public interface ICartService
    {
        public Task<string> GetOrCreateCartIdentifier();
        public Task<string> GetLoggedInCartId(string userId);
        public string GetGuestCartId();
        public Task<Cart> GetCartDetails(string cartIdentifier, bool includeItems = false, bool includeVariants = false, bool includeImages = false, bool includeColor = false);
        public Cart? getSessionCart();


    }
}
