using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models;
using MiliNeu.Models.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Milineu.Controllers
{
    public class ColorsController : Controller
    {
        IConfiguration _configuration;
        ApplicationDbContext _context;
        public ColorsController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<IActionResult> Manage()
        {
            /*Collections collection = new Collections();*/

            return _context.Collections != null ?
                        View(await _context.Colors.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Collections'  is null.");
        }
        // GET: ColorsController
        public ActionResult Index()
        {
            return View(_context.Colors);
        }

        // GET: ColorsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ColorsController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ColorsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: ColorsController/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ColorViewModel model)
        {
            if (ModelState.IsValid)
            {
                Color color = new Color
                {
                    Name = model.ColorName,
                    HexCode = model.HexCode
                };

                _context.Colors.Add(color);
                await _context.SaveChangesAsync(); // Awaiting the async method

                return RedirectToAction(nameof(Index));
            }

            // Return the view again with the invalid model for the user to fix
            return View(model);
        }


        // GET: ColorsController/Edit/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            // Find the color in the database by its ID
            Color color = await _context.Colors.SingleOrDefaultAsync(c => c.Id == id);

            if (color == null)
            {
                return NotFound(); // Return a 404 if the color doesn't exist
            }

            // Populate the ColorViewModel with the data from the color entity
            ColorViewModel model = new ColorViewModel
            {
                ColorID = id,
                ColorName = color.Name,
                HexCode = color.HexCode
            };

            return View(model); // Return the populated model to the view
        }


        // POST: ColorsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(ColorViewModel model)
        {
            // Validate ColorID
            if (model.ColorID <= 0) // Assuming ID is positive
            {
                return BadRequest("Invalid Color ID.");
            }

            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Retrieve the existing color from the database
                var color = await _context.Colors.FindAsync(model.ColorID);

                if (color == null)
                {
                    return NotFound();
                }

                // Update the properties
                color.Name = model.ColorName;
                color.HexCode = model.HexCode;

                // Save changes to the database
                _context.Update(color);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Return the view with the current model if validation fails
            return View(model);
        }


        // GET: ColorsController/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            // Find the color in the database by its ID
            Color color = await _context.Colors.SingleOrDefaultAsync(c => c.Id == id);

            if (color == null)
            {
                return NotFound(); // Return a 404 if the color doesn't exist
            }

            // Populate the ColorViewModel with the data from the color entity
            ColorViewModel model = new ColorViewModel
            {
                ColorName = color.Name,
                HexCode = color.HexCode
            };
            return View(model);

        }

        // POST: ColorsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                Color color = _context.Colors.Single(c => c.Id == id);
                if (color == null)
                {
                    return NotFound();
                }
                _context.Remove(color);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            return View();
        }
    }
}
