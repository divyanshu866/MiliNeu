using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models;
using MiliNeu.Models.ViewModels;
using MiliNeu.Utility;
using System.Diagnostics;

namespace MiliNeu.Controllers
{
    /*[Authorize(Roles ="Customer")]*/
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ISendGridService _SendGridService;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, ISendGridService sendGridService, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            this._SendGridService = sendGridService;
            _configuration = configuration;
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {

            //Send Test Email
            /*            var reciever = "divyanshusharma866@gmail.com";
                        var subject = "Test Subject";
                        var message = "Hello from MiliNeu Studios!";

                        await _SendGridService.SendEmailAsync("DevUser", "divyanshusharma2307@gmail.com", "Test Subject", "Hello from Milineu!");
            */
            /*Collections collection = new Collections();*/

            List<HeroSection> HeroSections = await _context.HeroSections
                .Include(c => c.Image).Where(c => c.IsActive == true).ToListAsync();

            List<Collection> AllCollections = await _context.Collections
                .IgnoreQueryFilters()
                .Include(i => i.Images)
                .Include(p => p.Products)
                .ThenInclude(c => c.Variants)
                .ThenInclude(i => i.Images)
                .Take(2)
                .ToListAsync();

            List<Collection> TopCollections = AllCollections
                .OrderByDescending(p => p.Name) // Sort by sales
                .Take(2).ToList(); // Fetch top 'count' best sellers


            List<Product> BestSellers = await _context.Products
                .IgnoreQueryFilters()
                .Include(c => c.Collection)
                .Include(c => c.Variants)
                .ThenInclude(c => c.Images)
                         .OrderByDescending(p => p.SalesCount)
                         .Take(4)
                         .ToListAsync();

            HomeViewModel homeViewModel = new HomeViewModel
            {
                HeroSections = HeroSections,
                AllCollections = AllCollections,
                TopCollections = TopCollections,
                BestSellers = BestSellers,
            };
            string basepath = _configuration["BasePaths:ThumbnailImageBasePath"];
            ViewData["ImageBasePath"] = basepath;


            return View(homeViewModel);

        }
        public async Task<IActionResult> About()
        {
            string basepath = _configuration["BasePaths:ThumbnailImageBasePath"];
            ViewData["ImageBasePath"] = basepath;

            return View();

        }
        [HttpPost]
        public async Task<JsonResult> Subscribe(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { success = false, message = "An error occurred. Please try again later." });

            }


            try
            {
                Subscriber? subscriber = await _context.Subscribers.SingleOrDefaultAsync(s => s.Email == email);

                if (subscriber == null)
                {
                    subscriber = new Subscriber()
                    {
                        Email = email,
                        SubscribedAt = DateTime.Now,
                        IsActive = true
                    };
                    _context.Subscribers.Add(subscriber);
                }
                else
                {
                    subscriber.IsActive = true;
                }
                await _context.SaveChangesAsync();
                // Return success response
                return Json(new { success = true, message = "Thank you for subscribing!" });

            }
            catch (Exception)
            {

                // Return error response
                return Json(new { success = false, message = "An error occurred. Please try again later." });

            }

        }
        [HttpPost]
        public async Task<IActionResult> UnSubscribe(string email)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            Subscriber? subscriber = await _context.Subscribers.SingleOrDefaultAsync(c => c.Email == email.Trim());
            if (subscriber != null)
            {
                subscriber.IsActive = false;
                await _context.SaveChangesAsync();
            }

            return View();
        }
        public async Task<IActionResult> Dashboard()
        {

            return View();

        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Manage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}