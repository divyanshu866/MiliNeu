using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models;
using MiliNeu.Models.Services.Interfaces;
using MiliNeu.Models.ViewModels;

namespace MiliNeu.Controllers
{
    public class CollectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ICollectionService _collectionService;

        public CollectionsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, ICollectionService collectionService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _collectionService = collectionService;
        }

        // GET: Collections
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
        {
            var viewModel = await _collectionService.getCollectionsPageAsync(pageNumber, pageSize);

            string basepath = _configuration["BasePaths:ProductImageBasePath"];
            ViewData["ImageBasePath"] = basepath;
            return View(viewModel);
        }

        //Displays Collections with crud options
        public async Task<IActionResult> Manage()
        {
            return View(await _collectionService.getAllCollectionsAsync());

            /*return _context.Collections != null ?
                        View(await _context.Collections.Include(c => c.Products).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Collections'  is null.");*/
        }
        // GET: Collections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Collections == null)
            {
                return NotFound();
            }

            var collection = await _context.Collections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collection == null)
            {
                return NotFound();
            }

            return View(collection);
        }

        // GET: Collections/Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {



            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CollectionCreateViewModel model, string mainImageName)
        {


            if (ModelState.IsValid)
            {
                // Create a new Collection object
                Collection newCollection = new Collection
                {
                    Name = model.Name,

                    Description = model.Description,
                    Category = model.Category,
                    Price = model.Price,


                };


                string uniqueFileName = null;
                string filePath = _configuration["BasePaths:ProductImageBasePath"];

                //ProductImage Path list for the product
                List<CollectionImage> collectionImages = new List<CollectionImage>();

                if (model.UploadedImages != null && model.UploadedImages.Count > 0)
                {
                    foreach (IFormFile photo in model.UploadedImages)
                    {
                        uniqueFileName = SaveUploadedFile(photo);


                        // Prepare ProductImage Path object for Path List
                        CollectionImage newImage = new CollectionImage
                        {
                            Path = uniqueFileName
                        };
                        if (photo.FileName.Trim() == mainImageName.Trim())
                        {
                            newImage.IsMain = true;
                        }
                        else
                        {
                            newImage.IsMain = false;
                        }
                        //Add ProductImage to List
                        collectionImages.Add(newImage);
                    }

                }

                // Assign the image paths to the product
                newCollection.Images = collectionImages;


                // Add the product to the context (this will generate the ProductId when saved)
                _context.Add(newCollection);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            return View();

        }

        // GET: Products/Edit/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Collections == null)
            {
                return NotFound();
            }

            var collection = await _context.Collections
                .IgnoreQueryFilters()
                .Include(i => i.Images)
                .SingleAsync(p => p.Id == id);

            if (collection == null)
            {
                return NotFound();
            }

            CollctionEditViewModel model = new CollctionEditViewModel
            {
                Id = collection.Id,
                Name = collection.Name,
                Category = collection.Category,
                Price = collection.Price,
                Description = collection.Description,
                ExistingImages = collection.Images,



            };

            string basepath = _configuration["BasePaths:CollectionImageBasePath"];
            ViewData["ImageBasePath"] = basepath;

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(CollctionEditViewModel model, string mainImageName)
        {


            //Check if model has Id and product table is not null
            if (model.Id < 1 || _context.Collections == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);

                //Get product from database
                Collection collection = _context.Collections
                   .IgnoreQueryFilters()
                   .Include(i => i.Images)
                   .SingleOrDefault(p => p.Id == model.Id);

                if (collection == null)
                {
                    return NotFound();
                }





                if (model.UploadedImages != null && model.UploadedImages.Count > 0)
                {
                    //ProductImage Path list for the product
                    List<CollectionImage> images = new List<CollectionImage>();

                    images = ProcessUploadedFile(model.UploadedImages, mainImageName, collection);

                    //Remove All old images
                    foreach (var image in collection.Images)
                    {
                        _context.CollectionImages.Remove(image);
                    }

                    //Delete images from storage
                    deleteCollectionImages(collection.Images);

                    //Overwrite new images to model
                    collection.Images = images;

                }
                else
                {
                    foreach (var image in collection.Images)
                    {
                        if (image.Path == mainImageName)
                        {
                            image.IsMain = true;
                        }
                        else
                        {
                            image.IsMain = false;
                        }

                    }
                }

                //Map ViewModel properties to product object in database
                collection.Name = model.Name;
                collection.Description = model.Description;
                collection.Category = model.Category;
                collection.Price = model.Price;






                _context.Update(collection);


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            }
            string basepath = _configuration["BasePaths:CollectionImageBasePath"];
            ViewData["ImageBasePath"] = basepath;
            return View();

        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Collections == null)
            {
                return NotFound();
            }

            var collection = await _context.Collections
                .IgnoreQueryFilters()
                .Include(p => p.Images)
                .Include(p => p.Products)
                .ThenInclude(i => i.Variants)
                .ThenInclude(i => i.Images)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collection == null)
            {
                return NotFound();
            }

            return View(collection);
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Collections == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Collections'  is null.");
            }
            var collection = await _context.Collections
                .IgnoreQueryFilters()
                .Include(i => i.Images)
                .Include(p => p.Products)
                .ThenInclude(p => p.Variants)
                        .ThenInclude(i => i.Images)
                .SingleAsync(p => p.Id == id);

            if (collection != null)
            {



                //delete images from storage
                deleteCollectionWithProductImages(collection);

                _context.Collections.Remove(collection);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));







        }
        public void deleteCollectionImages(List<CollectionImage> images)
        {
            string CollectionImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Resources", "Images", "Collections");


            if (images != null && images.Count > 0)
            {
                foreach (var image in images)
                {
                    //Delete old image
                    string oldImagePath = Path.Combine(CollectionImagePath, image.Path.Trim());

                    //Check if file exists
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        //Delete file
                        System.IO.File.Delete(oldImagePath);
                    }
                }

            }
        }
        public void deleteCollectionWithProductImages(Collection model)
        {
            List<CollectionImage> collectionImages = model.Images;
            string CollectionImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Resources", "Images", "Collections");
            string ProductImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Resources", "Images", "Products");


            if (collectionImages != null && collectionImages.Count > 0)
            {
                foreach (var image in collectionImages)
                {
                    //Delete old image
                    string oldImagePath = Path.Combine(CollectionImagePath, image.Path.Trim());

                    //Check if file exists
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        //Delete file
                        System.IO.File.Delete(oldImagePath);
                    }
                }

            }
            if (model.Products != null && model.Products.Count > 0)
            {
                foreach (var product in model.Products)
                {
                    foreach (var productImage in product.Variants[0].Images)
                    {
                        //Delete old image
                        string oldImagePath = Path.Combine(ProductImagePath, productImage.Path.Trim());

                        //Check if file exists
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            //Delete file
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                }

            }
        }
        public List<CollectionImage> ProcessUploadedFile(List<IFormFile> formFiles, string mainImageName, Collection collection)
        {

            List<CollectionImage> images = new List<CollectionImage>();
            foreach (IFormFile image in formFiles)
            {
                string uniqueFileName = SaveUploadedFile(image);


                //Prepare ProductImage object for VariantImages List
                CollectionImage newImage = new CollectionImage();
                newImage.Collection = collection;
                newImage.CollectionId = collection.Id;
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
        private string SaveUploadedFile(IFormFile photo)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resources", "Images", "Collections");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }


            return uniqueFileName;
        }

        private bool CollectionExists(int id)
        {
            return (_context.Collections?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
