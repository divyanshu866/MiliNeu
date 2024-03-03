using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MiliNeu.DataAccess.Data;
using MiliNeu.Models;

namespace MiliNeu.Controllers
{
    public class CollectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CollectionsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Collections
        public async Task<IActionResult> Index()
        {
            /*Collection collection = new Collection();*/

            return _context.Collection != null ?
                        View(await _context.Collection.Include(c => c.Products).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Collection'  is null.");
        }

        // GET: Collections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Collection == null)
            {
                return NotFound();
            }

            var collection = await _context.Collection
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collection == null)
            {
                return NotFound();
            }

            return View(collection);
        }

        // GET: Collections/Create
        /* public IActionResult Create()
         {
             return View();
         }*/

        // GET: Collections/Upsert
        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            Collection collection = new Collection();
            if (id == null || id == 0)
            {
                //Create
                return View(collection);
            }
            else
            {
                //Update
                collection = await _context.Collection.FindAsync(id);
                if (collection == null)
                {
                    return NotFound();
                }
                return View(collection);

            }

        }

        // POST: Collections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /* public async Task<IActionResult> Create(Collection collection, IFormFile? file)
         {
             if (ModelState.IsValid)
             {
                 _context.Add(collection);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
             }
             return View(collection);
         }


 */


        // POST: Collections/Upsert/Id,file
        [HttpPost]
        public async Task<IActionResult> Upsert(Collection collection, IFormFile? file)
        {

            //Create
            if (ModelState.IsValid)
            {
                //Saving Image File
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Resources\Images\Collections");
                    //Check if an image is already available
                    if (!string.IsNullOrEmpty(collection.Path))
                    {
                        //Delete old image
                        string oldImagePath = Path.Combine(wwwRootPath, collection.Path.TrimStart('\\'));

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
                    collection.Path = @"\Resources\Images\Collections\" + filename;
                }

                if (collection.Id == null || collection.Id == 0)
                {
                    //Create Collection in database
                    _context.Add(collection);
                }
                else
                {
                    _context.Update(collection);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(collection);
        }

        // GET: Collections/Edit/5
        /* public async Task<IActionResult> Edit(int? id)
         {
             if (id == null || _context.Collection == null)
             {
                 return NotFound();
             }

             var collection = await _context.Collection.FindAsync(id);
             if (collection == null)
             {
                 return NotFound();
             }
             return View(collection);
         }*/

        // POST: Collections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*        [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Category,Description,Price,Path")] Collection collection)
                {
                    if (id != collection.Id)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(collection);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!CollectionExists(collection.Id))
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
                    return View(collection);
                }*/

        // GET: Collections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Collection == null)
            {
                return NotFound();
            }

            var collection = await _context.Collection
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collection == null)
            {
                return NotFound();
            }

            return View(collection);
        }

        // POST: Collections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Collection == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Collection'  is null.");
            }
            var collection = await _context.Collection.FindAsync(id);

            if (collection != null)
            {
                //Delete File
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                string productPath = Path.Combine(wwwRootPath, @"Resources\Images\Collections");
                //Check if an image is present
                if (!string.IsNullOrEmpty(collection.Path))
                {
                    //Delete old image
                    string oldImagePath = Path.Combine(wwwRootPath, collection.Path.TrimStart('\\'));

                    //Check if file exists
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        //Delete file
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _context.Collection.Remove(collection);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CollectionExists(int id)
        {
            return (_context.Collection?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
