using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using MiliNeu.DataAccess.Data;
using MiliNeu.Models;

namespace MiliNeu.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Product.Include(p => p.Collection);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Collection)

                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Collections/Upsert
        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            Product product = new Product();
            Collection col = new Collection();
            product.Collection = col;
            if (id == null || id == 0)
            {

                //Create
                ViewBag.CollectionId = new SelectList(_context.Collection, "Id", "Name");

                return View(product);
            }
            else
            {
                //Update
                product = await _context.Product.Include(c => c.Collection).SingleAsync(i => i.Id == id);
                if (product == null)
                {
                    return NotFound();
                }
                /*                ViewData["CollectionId"] = new SelectList(_context.Collection, "Id", "Id", product.Collection.Id);*/
                ViewBag.CollectionId = new SelectList(_context.Collection, "Id", "Name", product.Collection.Id);
                return View(product);


            }


        }
        // POST: Collections/Upsert/Id,file
        [HttpPost]
        public async Task<IActionResult> Upsert(Product product, IFormFile? file)
        {
            //Check if model is valid
            if (ModelState.IsValid)
            {
                //Saving Image File
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Resources\Images\Products");
                    //Check if an image is already available
                    if (!string.IsNullOrEmpty(product.Path))
                    {
                        //Delete old image
                        string oldImagePath = Path.Combine(wwwRootPath, product.Path.TrimStart('\\'));

                        //Check if file exists
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            //Delete file
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    product.Path = @"\Resources\Images\Products\" + filename;
                }

                if (product.Id == null || product.Id == 0)
                {
                    //Create Collection in database
                    _context.Add(product);
                }
                else
                {
                    _context.Update(product);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Create
        /*   public IActionResult Create()
           {
               //ViewData["CollectionId"] = new SelectList(_context.Collection, "Id", "Id");
               ViewData["CollectionName"] = new SelectList(_context.Collection, "Name", "Name");
               return View();
           }

           // POST: Products/Create
           // To protect from overposting attacks, enable the specific properties you want to bind to.
           // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
           [HttpPost]
           [ValidateAntiForgeryToken]
           public async Task<IActionResult> Create([Bind("Id,Name,Category,Description,Price,Path,CollectionId,CollectionName")] Product product)
           {
               Collection col = new Collection();
               col = _context.Collection.Single(p => p.Id == product.CollectionId);
               product.CollectionId = col.Id;
               if (ModelState.IsValid)
               {
                   _context.Add(product);
                   await _context.SaveChangesAsync();
                   return RedirectToAction(nameof(Index));
               }
               ViewData["CollectionId"] = new SelectList(_context.Collection, "Id", "Id", product.Collection.Name);
               return View(product);
           }

           // GET: Products/Edit/5
           public async Task<IActionResult> Edit(int? id)
           {
               if (id == null || _context.Product == null)
               {
                   return NotFound();
               }

               var product = await _context.Product.Include(c => c.Collection).SingleAsync(p => p.Id == id);
               if (product == null)
               {
                   return NotFound();
               }
               ViewData["CollectionId"] = new SelectList(_context.Collection, "Id", "Id", product.Collection.Id);
               return View(product);
           }

           // POST: Products/Edit/5
           // To protect from overposting attacks, enable the specific properties you want to bind to.
           // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
           [HttpPost]
           [ValidateAntiForgeryToken]
           public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Category,Description,Price,Path,CollectionId,CollectionName")] Product product)
           {
               if (id != product.Id)
               {
                   return NotFound();
               }

               if (ModelState.IsValid)
               {
                   try
                   {
                       _context.Update(product);
                       await _context.SaveChangesAsync();
                   }
                   catch (DbUpdateConcurrencyException)
                   {
                       if (!ProductExists(product.Id))
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
               ViewData["CollectionId"] = new SelectList(_context.Collection, "Id", "Id", product.Collection.Id);
               return View(product);
           }*/

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Collection)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Collection'  is null.");
            }
            var product = await _context.Product.FindAsync(id);

            if (product != null)
            {
                //Delete File
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                string productPath = Path.Combine(wwwRootPath, @"Resources\Images\Products");
                //Check if an image is present
                if (!string.IsNullOrEmpty(product.Path))
                {
                    //Delete old image
                    string oldImagePath = Path.Combine(wwwRootPath, product.Path.TrimStart('\\'));

                    //Check if file exists
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        //Delete file
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
































            /*

                        if (_context.Product == null)
                        {
                            return Problem("Entity set 'ApplicationDbContext.Product'  is null.");
                        }
                        var product = await _context.Product.FindAsync(id);
                        if (product != null)
                        {
                            _context.Product.Remove(product);
                        }

                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));*/
        }

        private bool ProductExists(int id)
        {
            return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
