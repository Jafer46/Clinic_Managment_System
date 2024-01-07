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
    public class LabPaymentDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LabPaymentDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Payment/LabPaymentDetails
        public async Task<IActionResult> Index()
        { 
            var applicationDbContext = _context.LabPaymentDetails.Include(l => l.LabPayment);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Payment/LabPaymentDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LabPaymentDetails == null)
            {
                return NotFound();
            }

            var labPaymentDetail = await _context.LabPaymentDetails
                .Include(l => l.LabPayment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (labPaymentDetail == null)
            {
                return NotFound();
            }

            return View(labPaymentDetail);
        }

        // GET: Payment/LabPaymentDetails/Create
        public IActionResult Create(int id)
        {
            var LabPayment = _context.LabPayments.Find(id); 
            var LabRequestDetail = _context.LabRequestDetails.Where(l => l.LabRequestId == LabPayment.LabRequestId);
            ViewData["LabPaymentId"] = new SelectList(_context.LabPayments.Where(l => l.Id == id),"Id","Id");
            ViewData["LabRequestDetail"] = new SelectList(LabRequestDetail, "Case", "Case");
            ViewData["LabPaymentError"] = "";
            return View();
        }

        // POST: Payment/LabPaymentDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LabPaymentId,Amount,Reason")] LabPaymentDetail labPaymentDetail)
        {
            if(samePaymentExists(labPaymentDetail.LabPaymentId, labPaymentDetail.Reason))
            {
                var lrd = _context.LabRequestDetails.Where(l => l.LabRequestId == labPaymentDetail.LabPaymentId);
                ViewData["LabPaymentId"] = new SelectList(_context.LabPayments, "Id", "Id", labPaymentDetail.LabPaymentId);
                ViewData["LabRequestDetail"] = new SelectList(lrd, "Case", "Case");
                ViewData["LabPaymentError"] = "The same Payment exists";
                return View(labPaymentDetail);
            }
            if (ModelState.IsValid)
            {
                _context.Add(labPaymentDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "LabPayments", new {id = labPaymentDetail.LabPaymentId});
            }
            var LabRequestDetail = _context.LabRequestDetails.Where(l => l.LabRequestId == labPaymentDetail.LabPaymentId);
            ViewData["LabPaymentId"] = new SelectList(_context.LabPayments, "Id", "Id", labPaymentDetail.LabPaymentId);
            ViewData["LabRequestDetail"] = new SelectList(LabRequestDetail, "Case", "Case");
            ViewData["LabPaymentError"] = "";
            return View(labPaymentDetail);
        }

        // GET: Payment/LabPaymentDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LabPaymentDetails == null)
            {
                return NotFound();
            }

            var labPaymentDetail = await _context.LabPaymentDetails.FindAsync(id);
            if (labPaymentDetail == null)
            {
                return NotFound();
            }
            ViewData["LabPaymentId"] = new SelectList(_context.LabPayments, "Id", "Id", labPaymentDetail.LabPaymentId);
            return View(labPaymentDetail);
        }

        // POST: Payment/LabPaymentDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LabPaymentId,Amount,Reason")] LabPaymentDetail labPaymentDetail)
        {
            if (id != labPaymentDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(labPaymentDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabPaymentDetailExists(labPaymentDetail.Id))
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
            ViewData["LabPaymentId"] = new SelectList(_context.LabPayments, "Id", "Id", labPaymentDetail.LabPaymentId);
            return View(labPaymentDetail);
        }

        // GET: Payment/LabPaymentDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LabPaymentDetails == null)
            {
                return NotFound();
            }

            var labPaymentDetail = await _context.LabPaymentDetails
                .Include(l => l.LabPayment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (labPaymentDetail == null)
            {
                return NotFound();
            }

            return View(labPaymentDetail);
        }

        // POST: Payment/LabPaymentDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LabPaymentDetails == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LabPaymentDetails'  is null.");
            }
            var labPaymentDetail = await _context.LabPaymentDetails.FindAsync(id);
            if (labPaymentDetail != null)
            {
                _context.LabPaymentDetails.Remove(labPaymentDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabPaymentDetailExists(int id)
        {
          return (_context.LabPaymentDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool samePaymentExists(int labPaymentId, string paymentReason)
        {
            return (_context.LabPaymentDetails?
                .Any(e => e.LabPaymentId == labPaymentId && e.Reason == paymentReason))
                .GetValueOrDefault();
        }
    }
}
