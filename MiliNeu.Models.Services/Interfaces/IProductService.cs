using MiliNeu.Models.ViewModels;

namespace MiliNeu.Models.Services.Interfaces
{
    public interface IProductService
    {
        public Task<PagerVM<Product>> GetProductsAsync(int pageNumber, int pageSize);
        public Task<PagerVM<Product>> GetProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize);

        public Task<List<Product>> SearchAsync(string searchTerm);
        public Task<PagerVM<Product>> GetBestSellerProductsAsync(int pageNumber, int pageSize);

        public Task<PagerVM<Product>> GetDicountedProducts(int pageNumber, int pageSize);
        public Task<ProductDetailsViewModel> GetProductDetails(int? id, int? selectedColor, string? size);
        public Task<List<Product>> GetAllProductsAsync();

        public Task<ProductDetailsViewModel> GetProductVariantAsync(int productId, int variantId);

        public Task<ProductEditVM> GetEditProductVariantAsync(int productId, int VariantId);

        public decimal CalculateTaxAmount(decimal price);

    }
}
