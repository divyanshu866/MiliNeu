    using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models;
namespace MiliNeu.Controllers
{
    public class ProductVariantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductVariantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductVariants
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProductVariant.Include(p => p.Color).Include(p => p.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProductVariants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productVariant = await _context.ProductVariant
                .Include(p => p.Color)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productVariant == null)
            {
                return NotFound();
            }

            return View(productVariant);
        }

        // GET: ProductVariants/Create
        public IActionResult Create()
        {
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "HexCode");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Category");
            return View();
        }

        // POST: ProductVariants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ColorId,ProductId,Price,DiscountedPrice,isMain,IsDiscontinued")] ProductVariant productVariant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productVariant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "HexCode", productVariant.ColorId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Category", productVariant.ProductId);
            return View(productVariant);
        }

        // GET: ProductVariants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productVariant = await _context.ProductVariant.FindAsync(id);
            if (productVariant == null)
            {
                return NotFound();
            }
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "HexCode", productVariant.ColorId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Category", productVariant.ProductId);
            return View(productVariant);
        }

        // POST: ProductVariants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ColorId,ProductId,Price,DiscountedPrice,isMain,IsDiscontinued")] ProductVariant productVariant)
        {
            if (id != productVariant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productVariant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductVariantExists(productVariant.Id))
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
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "HexCode", productVariant.ColorId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Category", productVariant.ProductId);
            return View(productVariant);
        }

        // GET: ProductVariants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productVariant = await _context.ProductVariant
                .Include(p => p.Color)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productVariant == null)
            {
                return NotFound();
            }

            return View(productVariant);
        }

        // POST: ProductVariants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productVariant = await _context.ProductVariant.FindAsync(id);
            if (productVariant != null)
            {
                _context.ProductVariant.Remove(productVariant);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductVariantExists(int id)
        {
            return _context.ProductVariant.Any(e => e.Id == id);
        }
    }
}
