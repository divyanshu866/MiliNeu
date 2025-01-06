using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiliNeu.DataAccess.Data;
using MiliNeu.Models;
using System.Security.Claims;

namespace MiliNeu.Controllers
{
    public class OrderIssuesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderIssuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OrderIssues
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OrderIssues.Include(o => o.Order).Include(o => o.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OrderIssues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderIssue = await _context.OrderIssues
                .Include(o => o.Order)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderIssue == null)
            {
                return NotFound();
            }

            return View(orderIssue);
        }
        [HttpPost]
        public async Task<IActionResult> ReportIssue([FromBody] OrderIssue model)
        {
            var orderIssue = new OrderIssue
            {
                OrderId = model.OrderId,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier), // Get the logged-in user
                IssueType = model.IssueType,
                Description = model.Description,
                CreatedDate = DateTime.UtcNow,
                Status = "Pending",
                AdditionalData = model.Description

            };

            _context.OrderIssues.Add(orderIssue);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Issue reported successfully!" });

        }

        // GET: OrderIssues/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Currency");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: OrderIssues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderId,IssueType,Description,CreatedDate,Status,ResolvedDate,UserId,AdditionalData")] OrderIssue orderIssue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderIssue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Currency", orderIssue.OrderId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", orderIssue.UserId);
            return View(orderIssue);
        }

        // GET: OrderIssues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderIssue = await _context.OrderIssues.FindAsync(id);
            if (orderIssue == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Currency", orderIssue.OrderId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", orderIssue.UserId);
            return View(orderIssue);
        }

        // POST: OrderIssues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderId,IssueType,Description,CreatedDate,Status,ResolvedDate,UserId,AdditionalData")] OrderIssue orderIssue)
        {
            if (id != orderIssue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderIssue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderIssueExists(orderIssue.Id))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Currency", orderIssue.OrderId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", orderIssue.UserId);
            return View(orderIssue);
        }

        // GET: OrderIssues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderIssue = await _context.OrderIssues
                .Include(o => o.Order)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderIssue == null)
            {
                return NotFound();
            }

            return View(orderIssue);
        }

        // POST: OrderIssues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderIssue = await _context.OrderIssues.FindAsync(id);
            if (orderIssue != null)
            {
                _context.OrderIssues.Remove(orderIssue);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderIssueExists(int id)
        {
            return _context.OrderIssues.Any(e => e.Id == id);
        }
    }
}
