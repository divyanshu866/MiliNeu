
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models;
using MiliNeu.Models.Services.Interfaces;
using MiliNeu.Models.ViewModels;
using SixLabors.ImageSharp;
using Color = MiliNeu.Models.Color;

namespace MiliNeu.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IProductService _productServicce;
        public ProductsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IProductService productService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _productServicce = productService;
        }


        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 8)
        {

            PagerVM<Product> viewModel = await _productServicce.GetProductsAsync(pageNumber, pageSize);
            if (viewModel == null || viewModel.Items.Count() < 1)
            {
                return NotFound();
            }

            string basepath = _configuration["BasePaths:ProductImageBasePath"];
            ViewData["ImageBasePath"] = basepath;
            return View(viewModel);
        }
        // Search action
        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            // Query to search products by name, description, or other criteria
            List<Product>? products = await _productServicce.SearchAsync(searchTerm);
            if (products == null)
            {
                return NotFound();
            }
            string basepath = _configuration["BasePaths:ProductImageBasePath"];
            ViewData["ImageBasePath"] = basepath;
            return View(products);  // Pass the search results to the view
        }
        [HttpGet]
        public async Task<IActionResult> BestSellers(int pageNumber = 1, int pageSize = 8)
        {
            /* return await _productServicce.GetBestSellerProductsAsync(count);//gvujguikhk

 */





            PagerVM<Product> viewModel = await _productServicce.GetBestSellerProductsAsync(pageNumber, pageSize);
            if (viewModel == null || viewModel.Items.Count() < 1)
            {
                return NotFound();
            }

            string basepath = _configuration["BasePaths:ProductImageBasePath"];
            ViewData["ImageBasePath"] = basepath;
            return View(viewModel);

        }
        [HttpGet]
        public async Task<IActionResult> Sale(int pageNumber = 1, int pageSize = 8)
        {
            var viewModel = await _productServicce.GetDicountedProducts(pageNumber, pageSize);


            string basepath = _configuration["BasePaths:ProductImageBasePath"];
            ViewData["ImageBasePath"] = basepath;
            return View("Sale", viewModel);
        }
        //Displays products with crud options
        public async Task<IActionResult> Manage()
        {
            var products = await _productServicce.GetAllProductsAsync();
            return View(products);
        }



        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id, int? selectedColor, string? size)
        {
            ProductDetailsViewModel viewModel = await _productServicce.GetProductDetails(id, selectedColor, size);
            if (viewModel == null)
            {
                return NotFound();
            }
            string basepath = _configuration["BasePaths:ProductImageBasePath"];
            ViewData["ImageBasePath"] = basepath;
            return View(viewModel);
        }




        //Formerly - ColorImages
        public async Task<PartialViewResult> GetProductVariant(int productId, int variantId)
        {



            ProductDetailsViewModel viewModel = await _productServicce.GetProductVariantAsync(productId, variantId);

            if (viewModel == null)
            {
                /*return NotFound;*/
            }

            string basepath = _configuration["BasePaths:ProductImageBasePath"];

            ViewData["ImageBasePath"] = basepath;
            return PartialView("_ProductPreview", viewModel);
        }

        //Formerly ColorProduct
        public async Task<PartialViewResult> EditProductVariant(int productId, int VariantId)
        {



            ProductEditVM viewModel = await _productServicce.GetEditProductVariantAsync(productId, VariantId);

            if (viewModel == null)
            {
                /*return NotFound;*/
            }

            string basepath = _configuration["BasePaths:ProductImageBasePath"];

            ViewData["ImageBasePath"] = basepath;
            ViewBag.AvailableColors = ViewBagColors();
            ViewBag.CollectionId = new SelectList(_context.Collections, "Id", "Name", viewModel.CollectionId);
            return PartialView("_ProductEditPreview", viewModel);
        }







        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .IgnoreQueryFilters()
                .Include(p => p.Collection)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Collections'  is null.");
            }
            var product = await _context.Products
                        .IgnoreQueryFilters()
                        .Include(i => i.Variants)
                        .ThenInclude(i => i.Images)
                        .Include(i => i.Variants)
                        .ThenInclude(i => i.Color)
                        .SingleAsync(p => p.Id == id);


            if (product != null)
            {
                //Delete File



                string productImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Resources", "Images", "Products");

                //delete images from storage
                deleteImages(product.Variants[0].Images);

                _context.Products.Remove(product);

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));







        }
        // GET: Products/DeleteVariant/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVariant(int productId, int variantId)
        {
            if (productId == 0 || variantId == 0 || _context.Products == null || _context.ProductVariant == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .IgnoreQueryFilters()
                .Include(p => p.Collection)
                .Include(p => p.Variants)
                .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(m => m.Id == productId);
            var variant = product.Variants.SingleOrDefault(m => m.Id == variantId);
            if (product == null)
            {
                return NotFound();
            }

            return View(variant);
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVariant(int productId, int? variantId)
        {
            if (productId == 0 || variantId == 0 || _context.Products == null || _context.ProductVariant == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Collections'  is null.");

            }
            var product = await _context.Products
                        .IgnoreQueryFilters()
                        .Include(i => i.Variants)
                        .ThenInclude(i => i.Images)
                        .SingleAsync(p => p.Id == productId);

            var variant = product.Variants.SingleOrDefault(i => i.Id == variantId);

            if (variant != null)
            {
                //Delete File



                string productImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Resources", "Images", "Products");

                //delete images from storage
                deleteImages(variant.Images);
                var c = _context.ProductVariant.Count();
                _context.ProductVariant.Remove(variant);

                if (product.Variants.Count == 1)
                {
                    _context.Products.Remove(product);
                }

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public void deleteImages(List<VariantImage> images)
        {
            string productImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Resources", "Images", "Products");
            string compressedImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Resources", "Images", "Products", "Compressed");




            if (images != null && images.Count > 0)
            {
                foreach (var image in images)
                {
                    //Delete old image
                    string oldImagePath = Path.Combine(productImagePath, image.Path.Trim());
                    string compressedOldImagePath = Path.Combine(compressedImagePath, image.Path.Trim());

                    //Check if file exists
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        //Delete file
                        System.IO.File.Delete(oldImagePath);
                    } //Check if file exists
                    if (System.IO.File.Exists(compressedOldImagePath))
                    {
                        //Delete file
                        System.IO.File.Delete(compressedOldImagePath);
                    }
                }

            }
        }

        public void deleteSizeChartImages(string sizeChartPath)
        {
            string SizeChartImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Resources", "Images", "SizeCharts", sizeChartPath);
            //string SizeChartImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Resources", "Images", "Products", "Compressed");

            if (sizeChartPath != null)
            {

                //Delete Size Chart Image from storage
                if (System.IO.File.Exists(SizeChartImagePath))
                {
                    //Delete file
                    System.IO.File.Delete(SizeChartImagePath);
                }
            }



        }






        /*..................................New........................*/

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {


            ViewBag.CollectionId = new SelectList(_context.Collections, "Id", "Name");
            ViewBag.AvailableColors = ViewBagColors();
            ViewData["ImageBasePath"] = _configuration["BasePaths:ProductImageBasePath"];
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] /* Implement Validation | Fix NewColor and Pre-Existing color selection logic*/
        public async Task<IActionResult> Create(ProductWithVariantCreateVM model)
        {


            if (ModelState.IsValid)
            {


                // Create a new product object
                Product product = new Product
                {
                    Name = model.ProductCreateVM.Name,
                    /*  Collection = model.Collection,*/
                    CollectionId = model.ProductCreateVM.CollectionId,
                    Description = model.ProductCreateVM.Description,
                    Category = model.ProductCreateVM.Category,
                    Price = model.ProductCreateVM.Price,
                    DiscountedPrice = model.ProductCreateVM.DiscountedPrice,
                    IsDiscontinued = model.ProductCreateVM.IsDiscontinued,

                    //SizeChartPath Remaining to be done Later

                };


                string uniqueFileName;
                string filePath = _configuration["BasePaths:ProductImageBasePath"];

                //ProductImage Path list for the product
                List<VariantImage> variantImages = new List<VariantImage>();


                //If New Images Uploaded.
                if (model.ProductCreateVM.UploadedImages != null && model.ProductCreateVM.UploadedImages.Count > 0 && model.ProductCreateVM.SizeChartImage != null)
                {
                    string sizeChartPath = SaveSizeChartImage(model.ProductCreateVM.SizeChartImage);
                    //SizeChartPath added to product
                    product.SizeChartPath = sizeChartPath;

                    //
                    foreach (IFormFile image in model.ProductCreateVM.UploadedImages)
                    {
                        uniqueFileName = SaveProductImage(image);

                        // Prepare variantImage Path object for Path List
                        VariantImage variantImage = new VariantImage
                        {
                            Path = uniqueFileName
                        };
                        if (image.FileName.Trim() == model.mainImageName.Trim())
                        {
                            variantImage.IsMain = true;
                        }
                        else
                        {
                            variantImage.IsMain = false;
                        }
                        //Add variantImage to List
                        variantImages.Add(variantImage);
                    }

                }

                Color colorModel;
                if (model.ProductVariantCreateVM.ColorID == null || model.ProductVariantCreateVM.ColorID == 0)
                {
                    colorModel = new Color
                    {
                        Name = model.ProductVariantCreateVM.ColorName,
                        HexCode = model.ProductVariantCreateVM.HexCode
                    };

                    //Update Color Table
                    _context.Colors.Add(colorModel);
                }
                else
                {
                    colorModel = await _context.Colors.FirstOrDefaultAsync(c => c.Id == model.ProductVariantCreateVM.ColorID);

                }



                ProductVariant productVariant = new ProductVariant
                {
                    Product = product,
                    ProductId = product.Id,
                    Color = colorModel,
                    ColorId = colorModel.Id,

                    // Assign the image paths to the product
                    Images = variantImages,
                    isMain = true,
                    IsDiscontinued = false,

                };




                // Add the product to the context (this will generate the ProductId when saved)
                _context.Add(product);
                _context.Add(productVariant);
                // Save changes to the database
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            return View();

        }









        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProductVariant(int productId)
        {



            if (productId == null || _context.Products == null)
            {
                return NotFound();
            }

            // Retrieve the product from the database based on the given ID
            Product? product = await _context.Products
                .IgnoreQueryFilters()
                .Include(P => P.Collection)
            .Include(p => p.Variants)
            .ThenInclude(pc => pc.Color)   // Include related Color entity
            .Include(p => p.Variants)
            .ThenInclude(pc => pc.Images)  // Include related Images
            .Where(p => p.Id == productId)
            .SingleOrDefaultAsync();


            if (product == null)
            {
                return NotFound();
            }


            /*ProductVariant productVariant = product.Variants.SingleOrDefault(c => c.isMain);
*/


            AddVariantVM addVariantVM = new AddVariantVM
            {
                ProductId = product.Id,
                Name = product.Name,
                Category = product.Category,
                CollectionId = product.CollectionId,
                Price = product.Price,
                DiscountedPrice = product.DiscountedPrice,
                Description = product.Description,
                IsDiscontinued = product.IsDiscontinued,
            };



            ViewBag.AvailableColors = ViewBagColors();
            ViewBag.CollectionId = new SelectList(_context.Collections, "Id", "Name", product.Collection.Id);

            string basepath = _configuration["BasePaths:ProductImageBasePath"];
            ViewData["ImageBasePath"] = basepath;

            return View(addVariantVM);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]/* Implement Validation | Fix NewColor and Pre-Existing color selection logic*/
        public async Task<IActionResult> AddProductVariant(AddVariantVM model)
        {

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);

            if (ModelState.IsValid)
            {




                string uniqueFileName = null;
                string filePath = _configuration["BasePaths:ProductImageBasePath"];

                //variantImages Path list for the product
                List<VariantImage> variantImages = new List<VariantImage>();

                if (model.UploadedImages != null && model.UploadedImages.Count > 0)
                {
                    foreach (IFormFile image in model.UploadedImages)
                    {
                        uniqueFileName = SaveProductImage(image);


                        // Prepare variantImages Path object for Path List
                        VariantImage variantImage = new VariantImage
                        {
                            Path = uniqueFileName
                        };
                        if (image.FileName.Trim() == model.mainImageName.Trim())
                        {
                            variantImage.IsMain = true;
                        }
                        else
                        {
                            variantImage.IsMain = false;
                        }
                        //Add ProductImage to List
                        variantImages.Add(variantImage);
                    }

                }
                Color color;
                if (model.Variant.ColorID == null || model.Variant.ColorID == 0)
                {
                    color = new Color
                    {
                        Name = model.Variant.ColorName,
                        HexCode = model.Variant.HexCode
                    };
                    //Update Color Table
                    _context.Colors.Add(color);
                }
                else
                {
                    color = await _context.Colors.FirstOrDefaultAsync(c => c.Id == model.Variant.ColorID);

                }



                ProductVariant ProductVariant = new ProductVariant
                {


                    ProductId = model.ProductId,
                    Color = color,
                    ColorId = color.Id,
                    Images = variantImages,
                    isMain = (bool)model.isMain,
                    IsDiscontinued = model.IsDiscontinued


                };




                // Add the product to the context (this will generate the ProductId when saved)
                _context.Add(ProductVariant);
                // Save changes to the database
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            return View();

        }










        // GET: Products/Edit/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? productId)
        {
            if (productId == null || _context.Products == null)
            {
                return NotFound();
            }

            // Retrieve the product from the database based on the given ID
            var product = await _context.Products
            .IgnoreQueryFilters()
            .Include(P => P.Collection)
            .Include(p => p.Variants)
            .ThenInclude(pc => pc.Color)   // Include related Color entity
            .Include(p => p.Variants)
            .ThenInclude(pc => pc.Images)  // Include related Images
            .Where(p => p.Id == productId)
            .SingleOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }


            ProductVariant ProductVariant = product.Variants.SingleOrDefault(c => c.isMain);

            ProductVariantEditVM productVariantVM = new ProductVariantEditVM
            {
                VariantId = ProductVariant.Id,
                ProductImages = ProductVariant.Images,
                ColorID = ProductVariant.ColorId,
                ColorName = ProductVariant.Color.Name,
                HexCode = ProductVariant.Color.HexCode,
                isMain = ProductVariant.isMain,
                VariantDiscontinued = ProductVariant.IsDiscontinued


            };

            ProductEditVM editVM = new ProductEditVM
            {
                productId = product.Id,
                Name = product.Name,
                Category = product.Category,
                CollectionId = product.CollectionId,
                Price = product.Price,
                DiscountedPrice = product.DiscountedPrice,
                OriginalProductVariantId = ProductVariant.Id,
                OriginalColorId = ProductVariant.Color.Id,
                Description = product.Description,
                SelectedProductVariant = productVariantVM,
                IsDiscontinued = product.IsDiscontinued,


            };



            ProductEditCombinedVM editCombinedVM = new ProductEditCombinedVM
            {
                ProductEditViewModel = editVM,
                ProductVariants = product.Variants,

            };





            ViewBag.AvailableColors = ViewBagColors();
            ViewBag.CollectionId = new SelectList(_context.Collections, "Id", "Name", product.Collection.Id);

            string basepath = _configuration["BasePaths:ProductImageBasePath"];
            ViewData["ImageBasePath"] = basepath;

            return View(editCombinedVM);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(ProductEditVM model)
        {


            //Check if model has Id and product table is not null
            if (model.productId == 0 || _context.Products == null)
            {
                return NotFound();
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);

            if (ModelState.IsValid)
            {

                //Get product from database
                Product? product = await _context.Products
                        .IgnoreQueryFilters()
                        .Include(i => i.Variants)
                        .ThenInclude(i => i.Images)
                        .Include(i => i.Variants)
                        .ThenInclude(i => i.Color)
                        .SingleOrDefaultAsync(p => p.Id == model.productId);

                if (product == null)
                {
                    return NotFound();
                }

                //Get OG ProductVariant
                ProductVariant ProductVariant = product.Variants.SingleOrDefault(c => c.Id == model.OriginalProductVariantId);
                if (model.OriginalColorId != model.SelectedProductVariant.ColorID)
                {

                    ProductVariant.ColorId = (int)model.SelectedProductVariant.ColorID;


                }

                //MAP New Price

                ProductVariant.IsDiscontinued = model.SelectedProductVariant.VariantDiscontinued;

                _context.ProductVariant.Update(ProductVariant);



                ProcessUploadedFiles(model, product);

                if (model.SizeChartImage != null)
                {
                    string sizeChartPath = SaveSizeChartImage(model.SizeChartImage);
                    deleteSizeChartImages(product.SizeChartPath);
                    //SizeChartPath added to product
                    product.SizeChartPath = sizeChartPath;
                }
                //Map ViewModel properties to product object in database
                product.Name = model.Name;
                product.CollectionId = model.CollectionId;
                product.Description = model.Description;
                product.Category = model.Category;
                product.Price = model.Price;
                product.DiscountedPrice = model.DiscountedPrice;
                product.IsDiscontinued = model.IsDiscontinued;




                //_context.Update(product);


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            }
            string basepath = _configuration["BasePaths:ProductImageBasePath"];
            ViewData["ImageBasePath"] = basepath;
            return View();

        }

        /*if new images uploaded = delete old from storage and database then add new ones
         * if none uploaded = update main image among existing ones*/
        private void ProcessUploadedFiles(ProductEditVM model, Product product)
        {
            if (model.UploadedImages != null && model.UploadedImages.Count > 0)
            {
                ProductVariant ProductVariant = product.Variants.SingleOrDefault(c => c.Id == model.OriginalProductVariantId);

                //ProductImage Path list for the product
                List<VariantImage> images = new List<VariantImage>();

                //Saves new files to storage, sets main image returns VariantImage list
                images = ProcessUploadedFile(model.UploadedImages, model.mainImageName, product);

                //Remove All old images
                foreach (var image in ProductVariant.Images)
                {
                    _context.VariantImages.Remove(image);
                }

                //Delete images from storage
                deleteImages(ProductVariant.Images);

                //Overwrite new images to model
                product.Variants.SingleOrDefault(i => i.Id == model.OriginalProductVariantId).Images = images;



            }
            else
            {

                //if no images are uploaded, update the main image among existing
                foreach (var image in product.Variants.SingleOrDefault(c => c.Id == model.OriginalProductVariantId).Images)
                {
                    if (image.Path == model.mainImageName)
                    {
                        image.IsMain = true;
                    }
                    else
                    {
                        image.IsMain = false;
                    }

                }
            }
        }

        public List<SelectListItem> ViewBagColors()
        {
            var colors = _context.Colors.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name + " (" + c.HexCode + ")"
            }).ToList();
            return colors;
        }


        //Saves new files to storage, sets main image returns VariantImage list
        public List<VariantImage> ProcessUploadedFile(List<IFormFile> formFiles, string mainImageName, Product product)
        {

            List<VariantImage> images = new List<VariantImage>();
            foreach (IFormFile image in formFiles)
            {
                string uniqueFileName = SaveProductImage(image);


                //Prepare ProductImage object for VariantImages List
                VariantImage newImage = new VariantImage();

                newImage.Path = uniqueFileName;

                if (image.FileName.Trim() == mainImageName.Trim())
                {
                    newImage.IsMain = true;
                }
                else
                {
                    newImage.IsMain = false;
                }

                //Add ProductImage to List
                images.Add(newImage);
            }
            return images;
        }

        private string SaveProductImage(IFormFile photo)
        {

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resources", "Images", "Products");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }
            /* CreateLosslessImageVariantsAsync(filePath, uploadsFolder, uniqueFileName);
 */

            return uniqueFileName;
        }
        private string SaveSizeChartImage(IFormFile photo)
        {

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resources", "Images", "SizeCharts");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }
            /* CreateLosslessImageVariantsAsync(filePath, uploadsFolder, uniqueFileName);
 */

            return uniqueFileName;
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

































/*
public static async Task CreateLosslessImageVariantsAsync(string inputPath, string uploadsFolder, string fileName)
{
    using (var originalImage = Image.Load(inputPath))
    {
        // Define paths for each variant
        var thumbnailPath = Path.Combine(uploadsFolder, "Thumbnails", $"thumb_{fileName}");
        var mediumPath = Path.Combine(uploadsFolder, "Medium", $"medium_{fileName}");
        var largePath = Path.Combine(uploadsFolder, "Large", $"large_{fileName}");
        var webpPath = Path.Combine(uploadsFolder, "WebP", $"web_{fileName.Replace(".png", ".webp")}");

        // Create Thumbnail (300px wide) - Lossless PNG
        using (var thumbnailImage = originalImage.Clone(context => context.Resize(new ResizeOptions
        {
            Size = new SixLabors.ImageSharp.Size(580, 870),
            Mode = ResizeMode.Crop,
            Sampler = KnownResamplers.Lanczos3 // High-quality resize
        })))
        {
            // Save as lossless PNG
            await thumbnailImage.SaveAsync(thumbnailPath, new PngEncoder { CompressionLevel = PngCompressionLevel.BestCompression });
            // Save as lossless WebP
            await thumbnailImage.SaveAsync(webpPath, new WebpEncoder { FileFormat = WebpFileFormatType.Lossless, Quality = 100 });
        }

        // Create Medium Size (500px wide) - Lossless PNG
        using (var mediumImage = originalImage.Clone(context => context.Resize(new ResizeOptions
        {
            Size = new SixLabors.ImageSharp.Size(500, 750),
            Mode = ResizeMode.Crop,
            Sampler = KnownResamplers.Lanczos3
        })))
        {
            await mediumImage.SaveAsync(mediumPath, new PngEncoder { CompressionLevel = PngCompressionLevel.BestCompression });
        }

        // Create Large Size (1000px wide) - Lossless PNG
        using (var largeImage = originalImage.Clone(context => context.Resize(new ResizeOptions
        {
            Size = new SixLabors.ImageSharp.Size(1500, 1800),
            Mode = ResizeMode.Crop,
            Sampler = KnownResamplers.Lanczos3
        })))
        {
            await largeImage.SaveAsync(largePath, new PngEncoder { CompressionLevel = PngCompressionLevel.BestCompression });
        }
    }
}*/