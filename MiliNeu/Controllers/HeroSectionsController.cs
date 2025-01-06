using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models;
using MiliNeu.Models.ViewModels;

namespace MiliNeu.Controllers
{
    public class HeroSectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HeroSectionsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: HeroSections
        public async Task<IActionResult> Index()
        {
            return View(await _context.HeroSections.ToListAsync());
        }

        // GET: HeroSections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var heroSection = await _context.HeroSections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (heroSection == null)
            {
                return NotFound();
            }

            return View(heroSection);
        }

        // GET: HeroSections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HeroSections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HeroSectionVM heroSectionVM)
        {
            if (ModelState.IsValid)
            {
                /*string contentType = heroSectionVM.UploadedImage.ContentType;

                // Check if the file is an image
                if (contentType.StartsWith("image/"))
                {
                    // Handle image


                }
                // Check if the file is a video
                else if (contentType.StartsWith("video/"))
                {
                    // Handle video (you could save it differently or perform additional checks)
                    return View(heroSectionVM);

                }
                else
                {
                    // Not an image or video file
                    ModelState.AddModelError("UploadedImage", "Only image or video files are allowed.");
                    return View(heroSectionVM);
                }*/

                HeroSection heroSection = HeroSectionVMToHeroSection(heroSectionVM);

                string uniqueFileName = SaveHeroImage(heroSectionVM.UploadedImage);
                // Prepare HeroSectionImage Path object for Path List

                HeroSectionImage heroSectionImage = new HeroSectionImage
                {
                    Path = uniqueFileName,
                    HeroSection = heroSection,
                };
                heroSection.Image = heroSectionImage;





                _context.Add(heroSection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(heroSectionVM);
        }
        private string SaveHeroImage(IFormFile photo)
        {

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resources", "Images", "HeroSection");
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
        public HeroSection HeroSectionVMToHeroSection(HeroSectionVM viewModel)
        {
            HeroSection heroSection = new HeroSection
            {
                Link = viewModel.Link,
                Title = viewModel.Title,
                IsActive = viewModel.IsActive,
            };
            if (viewModel.Id != null)
            {
                heroSection.Id = (int)viewModel.Id;
            }

            return heroSection;
        }
        public HeroSectionVM HeroSectionToHeroSectionVM(HeroSection heroSection)
        {
            HeroSectionVM viewModel = new HeroSectionVM
            {
                Id = heroSection.Id,
                Title = heroSection.Title,
                Link = heroSection.Link,
                IsActive = heroSection.IsActive

            };

            return viewModel;
        }
        // GET: HeroSections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var heroSection = await _context.HeroSections.FindAsync(id);
            if (heroSection == null)
            {
                return NotFound();
            }
            HeroSectionVM viewModel = HeroSectionToHeroSectionVM(heroSection);

            return View(viewModel);
        }

        // POST: HeroSections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HeroSectionVM viewModel)
        {
            if (viewModel.Id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                HeroSection? heroSection = _context.HeroSections
                    .Include(c => c.Image)
                    .SingleOrDefault(c => c.Id == viewModel.Id);
                if (heroSection == null)
                {
                    return NotFound();
                }
                heroSection.Title = viewModel.Title;
                heroSection.Link = viewModel.Link;
                heroSection.IsActive = viewModel.IsActive;
                if (viewModel.UploadedImage != null)
                {
                    deleteHeroImage(heroSection.Image);

                    string uniqueFileName = SaveHeroImage(viewModel.UploadedImage);
                    // Prepare HeroSectionImage Path object for Path List

                    heroSection.Image.Path = uniqueFileName;
                }

                try
                {
                    _context.Update(heroSection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HeroSectionExists(heroSection.Id))
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
            return View(viewModel);
        }
        public void deleteHeroImage(HeroSectionImage image)
        {
            string HeroImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Resources", "Images", "HeroSection");



            //Delete old image
            string imagePath = Path.Combine(HeroImagePath, image.Path.Trim());

            //Check if file exists
            if (System.IO.File.Exists(imagePath))
            {
                //Delete file
                System.IO.File.Delete(imagePath);
            }



        }

        // GET: HeroSections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var heroSection = await _context.HeroSections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (heroSection == null)
            {
                return NotFound();
            }

            return View(heroSection);
        }

        // POST: HeroSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var heroSection = await _context.HeroSections.FindAsync(id);
            if (heroSection != null)
            {
                _context.HeroSections.Remove(heroSection);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HeroSectionExists(int id)
        {
            return _context.HeroSections.Any(e => e.Id == id);
        }
    }
}
