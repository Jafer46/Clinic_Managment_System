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
    public class LabPaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LabPaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Payment/LabPayments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LabPayments
                        .Include(l => l.LabRequest)
                        .Include(l => l.LabRequest.Session.Patient)
                        .Include(l => l.LabRequest.Session.Docter)
                        .Where(l => l.LabRequest.Session.SessionStatus.Name.ToLower() != "ended");
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Payment/LabPayments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LabPayments == null)
            {
                return NotFound();
            }

            var labPayment = await _context.LabPayments
                .Include(l => l.LabRequest)
                .Include(l => l.LabRequest.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (labPayment == null)
            {
                return NotFound();
            }
            object toL = _context.LabPaymentDetails.Where(l => l.LabPaymentId == id).ToList();
            var labPaymentDetail = toL;
            ViewData["Detail"] = labPaymentDetail;

            return View(labPayment);
        }

        // GET: Payment/LabPayments/Create
        public IActionResult Create(int id)
        {
            if (samePaymentExists(id))
            {
                var payment = _context.LabPayments.FirstOrDefault(l => l.LabRequestId == id);
                return RedirectToAction("Details", "LabPayments", new{ id = payment.Id });
            }
            var labRequests = _context.LabRequests.Where(l => l.Id == id);
            ViewData["LabRequestId"] = new SelectList(labRequests, "Id", "Id");
            return View();
        }

        // POST: Payment/LabPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LabRequestId,Date,Checked")] LabPayment labPayment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(labPayment);
                await _context.SaveChangesAsync();
                var lr = await _context.LabRequests.FindAsync(labPayment.LabRequestId);
                return RedirectToAction("Details", "Sessions", new {area = "Medical", id = lr.SessionId});
            }
            ViewData["LabRequestId"] = new SelectList(_context.LabRequests, "Id", "Id", labPayment.LabRequestId);
            return View(labPayment);
        }

        // GET: Payment/LabPayments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LabPayments == null)
            {
                return NotFound();
            }

            var labPayment = await _context.LabPayments.FindAsync(id);
            if (labPayment == null)
            {
                return NotFound();
            }
            ViewData["LabRequestId"] = new SelectList(_context.LabRequests, "Id", "Id", labPayment.LabRequestId);
            return View(labPayment);
        }

        // POST: Payment/LabPayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LabRequestId,Date,Checked")] LabPayment labPayment)
        {
            if (id != labPayment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(labPayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabPaymentExists(labPayment.Id))
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
            ViewData["LabRequestId"] = new SelectList(_context.LabRequests, "Id", "Id", labPayment.LabRequestId);
            return View(labPayment);
        }

        // GET: Payment/LabPayments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LabPayments == null)
            {
                return NotFound();
            }

            var labPayment = await _context.LabPayments
                .Include(l => l.LabRequest)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (labPayment == null)
            {
                return NotFound();
            }

            return View(labPayment);
        }

        // POST: Payment/LabPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LabPayments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LabPayments'  is null.");
            }
            var labPayment = await _context.LabPayments.FindAsync(id);
            if (labPayment != null)
            {
                _context.LabPayments.Remove(labPayment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabPaymentExists(int id)
        {
          return (_context.LabPayments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool samePaymentExists(int id)
        {
            return (_context.LabPayments?
                .Any(e => e.LabRequestId == id))
                .GetValueOrDefault();
        }
    }
}
