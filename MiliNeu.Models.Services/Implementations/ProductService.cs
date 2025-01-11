using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models.Services.Interfaces;
using MiliNeu.Models.ViewModels;
using PaymentGateway;

namespace MiliNeu.Models.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IRazorpayPaymentService _razorpayPaymentService;
        private readonly IHttpContextAccessor _httpcontextAccessor;


        public ProductService(ApplicationDbContext context, IConfiguration configuration, IRazorpayPaymentService razorpayPaymentService, IHttpContextAccessor httpcontextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _razorpayPaymentService = razorpayPaymentService;
            _httpcontextAccessor = httpcontextAccessor;
        }
        public async Task<PagerVM<Product>> GetProductsAsync(int pageNumber, int pageSize)
        {
            var AllProducts = await _context.Products
                .IgnoreQueryFilters()
                .Include(c => c.Collection)
                .Include(c => c.Variants)
                .ThenInclude(c => c.Images)
                .Include(c => c.Variants)
                .ThenInclude(c => c.Color)
                .OrderBy(p => p.Name) // You can order by any property
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalProducts = _context.Products.Count();

            PagerVM<Product> viewModel = new PagerVM<Product>
            {
                Items = AllProducts,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize)
            };



            return viewModel;
        }
        public async Task<PagerVM<Product>> GetProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize)
        {
            var AllProducts = await _context.Products
                .IgnoreQueryFilters()
                .Include(c => c.Collection)
                .Include(c => c.Variants)
                .ThenInclude(c => c.Images)
                .Include(c => c.Variants)
                .ThenInclude(c => c.Color)
                .Where(c => c.CategoryId == categoryId)
                .OrderBy(p => p.Name) // You can order by any property
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalProducts = _context.Products.Where(c => c.CategoryId == categoryId).Count();

            PagerVM<Product> viewModel;
            if (totalProducts > 0)
            {
                viewModel = new PagerVM<Product>
                {
                    Items = AllProducts,
                    CurrentPage = pageNumber,
                    TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize)
                };
            }
            else
            {
                viewModel = new PagerVM<Product>
                {
                    Items = new List<Product>(),
                    CurrentPage = pageNumber,
                    TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize)
                };
            }


            return viewModel;
        }
        public async Task<List<Product>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return new List<Product>();  // Return empty view if no search term is provided
            }

            // Query to search products by name, description, or other criteria
            List<Product> products = await _context.Products
                .IgnoreQueryFilters()
                .Include(p => p.Variants)
                .ThenInclude(p => p.Images)
                .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .ToListAsync();

            return products;  // Pass the search results to the view
        }
        public async Task<PagerVM<Product>> GetBestSellerProductsAsync(int pageNumber, int pageSize)
        {
            /* return await _context.Products
                 .Include(p => p.Variants)
                 .OrderByDescending(p => p.SalesCount) // Sort by sales
                 .Take(count) // Fetch top 'count' best sellers
                 .ToListAsync();*/


            var AllBestSellers = await _context.Products
               .IgnoreQueryFilters()
               .Include(c => c.Collection)
               .Include(c => c.Variants)
               .ThenInclude(c => c.Images)
               .Include(c => c.Variants)
               .ThenInclude(c => c.Color)
               .OrderByDescending(p => p.SalesCount) // Sort by SalesCount
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();

            var totalProducts = _context.Products.Count();

            PagerVM<Product> viewModel = new PagerVM<Product>
            {
                Items = AllBestSellers,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize)
            };



            return viewModel;

        }
        public async Task<PagerVM<Product>> GetDicountedProducts(int pageNumber, int pageSize)
        {
            var AllSaleProducts = await _context.Products
                .IgnoreQueryFilters()
                .Include(c => c.Collection)
                .Include(c => c.Variants)
                .ThenInclude(c => c.Images)
                .Include(c => c.Variants)
                .ThenInclude(c => c.Color)
                .OrderBy(p => p.Name).Where(c => c.DiscountedPrice > 0) // You can order by any property
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalProducts = _context.Products.Count();

            return new PagerVM<Product>
            {
                Items = AllSaleProducts,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize)
            };


        }
        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = await _context.Products
                .IgnoreQueryFilters()
                .Include(p => p.Collection)
                .Include(p => p.Variants)
                        .ThenInclude(i => i.Images).ToListAsync();
            return products;
        }
        public async Task<ProductDetailsViewModel> GetProductDetails(int? id, int? selectedColor, string? size)
        {
            if (id == null || _context.Products == null)
            {
                return null;
            }

            var product = await _context.Products
                    .IgnoreQueryFilters()
                    .Include(p => p.Collection)
                    .ThenInclude(p => p.Products)
                    .ThenInclude(p => p.Variants)
                    .ThenInclude(p => p.Images)
                    .Include(p => p.Variants) // Include the ProductVariant collection (ProductVariants)
                    .ThenInclude(pc => pc.Color) // Include the Color property in ProductVariant
                    .Include(p => p.Variants) // Include the ProductVariant collection (Images)
                    .ThenInclude(pc => pc.Images) // Include the VariantImages in ProductVariant
                    .Include(p => p.Collection) // Include the Collections navigation property
                    .FirstOrDefaultAsync(m => m.Id == id);

            ProductDetailsViewModel viewModel = new ProductDetailsViewModel();
            viewModel.Product = product;
            List<Collection> col = new List<Collection>();
            col.Add(product.Collection);
            viewModel.Collection = col;


            if (selectedColor == null || selectedColor == 0)
            {
                viewModel.SelectedVariant = product.Variants.FirstOrDefault(c => c.isMain);
            }
            else
            {
                viewModel.SelectedVariant = product.Variants.FirstOrDefault(c => c.Id == selectedColor);
            }

            if (!string.IsNullOrEmpty(size))
            {
                viewModel.SelectedSize = size;
            }


            viewModel.ProductVariants = product.Variants;

            if (product == null)
            {
                return null;
            }

            return viewModel;
        }
        public async Task<ProductDetailsViewModel> GetProductVariantAsync(int productId, int variantId)
        {


            var product = await _context.Products
                    .IgnoreQueryFilters()
                    .Include(p => p.Variants) // Include the ProductVariant collection (ProductVariants)
                    .ThenInclude(pc => pc.Color) // Include the Color property in ProductVariant
                    .Include(p => p.Variants) // Include the ProductVariant collection (Images)
                    .ThenInclude(pc => pc.Images) // Include the VariantImages in ProductVariant
                    .Include(p => p.Collection) // Include the Collections navigation property
                    .FirstOrDefaultAsync(m => m.Id == productId);


            ProductDetailsViewModel viewModel = new ProductDetailsViewModel();
            viewModel.Product = product;
            viewModel.SelectedVariant = product.Variants.FirstOrDefault(c => c.Id == variantId);
            viewModel.ProductVariants = product.Variants;

            if (product == null || viewModel.SelectedVariant == null)
            {
                return null;
            }

            return viewModel;
        }
        public async Task<ProductEditVM> GetEditProductVariantAsync(int productId, int VariantId)
        {


            var product = await _context.Products
                    .IgnoreQueryFilters()
                    .Include(p => p.Variants) // Include the ProductVariant collection (ProductVariants)
                    .ThenInclude(pc => pc.Color) // Include the Color property in ProductVariant
                    .Include(p => p.Variants) // Include the ProductVariant collection (Images)
                    .ThenInclude(pc => pc.Images) // Include the VariantImages in ProductVariant
                    .Include(p => p.Collection) // Include the Collections navigation property
                    .FirstOrDefaultAsync(m => m.Id == productId);
            ProductVariant ProductVariant = product.Variants.SingleOrDefault(c => c.Id == VariantId);

            if (product == null)
            {
                return null;
            }

            ProductVariantEditVM productVariantViewModel = new ProductVariantEditVM
            {
                VariantId = ProductVariant.Id,
                ProductImages = ProductVariant.Images,
                ColorID = ProductVariant.ColorId,
                ColorName = ProductVariant.Color.Name,
                HexCode = ProductVariant.Color.HexCode,
                isMain = ProductVariant.isMain,

                VariantDiscontinued = ProductVariant.IsDiscontinued

            };
            ProductEditVM viewModel = new ProductEditVM
            {
                CategoryId = product.CategoryId,
                CollectionId = product.CollectionId,
                SelectedProductVariant = productVariantViewModel,
                Description = product.Description,
                Price = product.Price,
                DiscountedPrice = product.DiscountedPrice,
                OriginalProductVariantId = ProductVariant.Id,
                OriginalColorId = ProductVariant.Color.Id,
                IsDiscontinued = product.IsDiscontinued,
                Name = product.Name,
                productId = product.Id
            };

            return viewModel;
        }
        public decimal CalculateTaxAmount(decimal price)
        {
            // Validate price to ensure it's non-negative
            if (price < 0)
            {
                throw new ArgumentException("Price cannot be negative.");
            }

            // Initialize tax rate
            decimal taxRate = 0;

            // Determine tax rate based on price thresholds
            if (price <= 1000)
            {
                taxRate = 0.05m; // 5% tax for products priced up to ₹1000
            }
            else if (price > 1000)
            {
                taxRate = 0.12m; // 12% tax for products priced above ₹1000
            }

            // Calculate and return the tax amount
            decimal taxAmount = price * taxRate;
            return taxAmount;
        }

    }
}
