using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BALibrary.Payment;
using CMS.Data;

namespace CMS.Areas.Payment.Controllers
{
    [Area("Payment")]
    public class CardPaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CardPaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Payment/CardPayments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CardPayment.Include(c => c.Session);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Payment/CardPayments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CardPayment == null)
            {
                return NotFound();
            }

            var cardPayment = await _context.CardPayment
                .Include(c => c.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cardPayment == null)
            {
                return NotFound();
            }

            return View(cardPayment);
        }

        // GET: Payment/CardPayments/Create
        public IActionResult Create(int SessionId)
        {
            var session = _context.Sessions.Where(s => s.Id == SessionId);
            ViewData["SessionId"] = new SelectList(session, "Id", "Id");
            return View();
        }

        // POST: Payment/CardPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Amount,Checked,SessionId")] CardPayment cardPayment)
        {
            if (sameCardPaymentExists(cardPayment.SessionId))
            {
                var c = _context.CardPayment
                    .FirstOrDefault(c => c.SessionId == cardPayment.SessionId);
                return RedirectToAction("Details", new { id = c.Id });
            }
            if (ModelState.IsValid)
            {
                _context.Add(cardPayment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", cardPayment.SessionId);
            return View(cardPayment);
        }

        // GET: Payment/CardPayments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CardPayment == null)
            {
                return NotFound();
            }

            var cardPayment = await _context.CardPayment.FindAsync(id);
            if (cardPayment == null)
            {
                return NotFound();
            }
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", cardPayment.SessionId);
            return View(cardPayment);
        }

        // POST: Payment/CardPayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Amount,Checked,SessionId")] CardPayment cardPayment)
        {
            if (id != cardPayment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cardPayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardPaymentExists(cardPayment.Id))
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
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", cardPayment.SessionId);
            return View(cardPayment);
        }

        // GET: Payment/CardPayments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CardPayment == null)
            {
                return NotFound();
            }

            var cardPayment = await _context.CardPayment
                .Include(c => c.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cardPayment == null)
            {
                return NotFound();
            }

            return View(cardPayment);
        }

        // POST: Payment/CardPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CardPayment == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CardPayment'  is null.");
            }
            var cardPayment = await _context.CardPayment.FindAsync(id);
            if (cardPayment != null)
            {
                _context.CardPayment.Remove(cardPayment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CardPaymentExists(int id)
        {
          return (_context.CardPayment?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool sameCardPaymentExists(int sessionId)
        {
            return (_context.CardPayment?.Any(e => e.SessionId == sessionId)).GetValueOrDefault();
        }
    }
}
