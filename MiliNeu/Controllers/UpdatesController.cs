using Microsoft.AspNetCore.Mvc;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models;
using MiliNeu.Utility;

namespace MiliNeu.Controllers
{
    public class UpdatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISendGridService _sendGridService;
        public UpdatesController(ApplicationDbContext dbContext, ISendGridService sendGridService)
        {
            _context = dbContext;
            _sendGridService = sendGridService;
        }
        public IActionResult Index()
        {
            return View();
        }
        // GET: Admin page to create email
        public IActionResult CreateEmail()
        {
            return View();
        }

        // POST: Send manually created email
        [HttpPost]
        public async Task<IActionResult> SendManualEmail(string subject, string body)
        {
            if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body))
            {
                ModelState.AddModelError("", "Subject and Body are required.");
                return View("CreateEmail");
            }
            try
            {
                // Retrieve the list of subscribed emails
                IEnumerable<Subscriber>? subscribers = _context.Subscribers?.Where(c => c.IsActive == true);
                if (subscribers != null && subscribers.Count() > 0)
                {
                    foreach (var subscriber in subscribers)
                    {
                        // Send the email
                        await _sendGridService.SendEmailAsync("Milineu Subscriber", subscriber.Email, subject, body);
                    }
                    // Return a success message
                    ViewData["Message"] = "Email sent successfully!";
                }
                else
                {
                    // Return a success message
                    ViewData["Message"] = "There are no Subscribers";
                }





            }
            catch (Exception)
            {

                ViewData["Message"] = "Error Occured while Sending Emails.";

            }
            return View("CreateEmail");


        }
    }
}
