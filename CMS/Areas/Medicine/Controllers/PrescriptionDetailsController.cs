using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BALibrary.Prescription;
using CMS.Data;

namespace CMS.Areas.Medicine.Controllers
{
    [Area("Medicine")]
    public class PrescriptionDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Medicine/PrescriptionDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PrescriptionDetails.Include(p => p.Prescription);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Medicine/PrescriptionDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PrescriptionDetails == null)
            {
                return NotFound();
            }

            var prescriptionDetail = await _context.PrescriptionDetails
                .Include(p => p.Prescription)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prescriptionDetail == null)
            {
                return NotFound();
            }
            

            return View(prescriptionDetail);
        }

        // GET: Medicine/PrescriptionDetails/Create
        public IActionResult Create(int id)
        {
            var prescription = _context.Prescription.Where(p => p.Id == id);
            ViewData["PrescriptionId"] = new SelectList(prescription, "Id", "Id");
            return View();
        }

        // POST: Medicine/PrescriptionDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PrescriptionId,MedicineName,Brand,PrescribedAmount,OtherDetail")] PrescriptionDetail prescriptionDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prescriptionDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction("Detail", "Prescriptions");
            }
            ViewData["PrescriptionId"] = new SelectList(_context.Prescription, "Id", "Id", prescriptionDetail.PrescriptionId);
            return View(prescriptionDetail);
        }

        // GET: Medicine/PrescriptionDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PrescriptionDetails == null)
            {
                return NotFound();
            }

            var prescriptionDetail = await _context.PrescriptionDetails.FindAsync(id);
            if (prescriptionDetail == null)
            {
                return NotFound();
            }
            ViewData["PrescriptionId"] = new SelectList(_context.Prescription, "Id", "Id", prescriptionDetail.PrescriptionId);
            return View(prescriptionDetail);
        }

        // POST: Medicine/PrescriptionDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PrescriptionId,MedicineName,Brand,PrescribedAmount,OtherDetail")] PrescriptionDetail prescriptionDetail)
        {
            if (id != prescriptionDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prescriptionDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrescriptionDetailExists(prescriptionDetail.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Detail", "Prescriptions");
            }
            ViewData["PrescriptionId"] = new SelectList(_context.Prescription, "Id", "Id", prescriptionDetail.PrescriptionId);
            return View(prescriptionDetail);
        }

        // GET: Medicine/PrescriptionDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PrescriptionDetails == null)
            {
                return NotFound();
            }

            var prescriptionDetail = await _context.PrescriptionDetails
                .Include(p => p.Prescription)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prescriptionDetail == null)
            {
                return NotFound();
            }

            return View(prescriptionDetail);
        }

        // POST: Medicine/PrescriptionDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PrescriptionDetails == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PrescriptionDetails'  is null.");
            }
            var prescriptionDetail = await _context.PrescriptionDetails.FindAsync(id);
            if (prescriptionDetail != null)
            {
                _context.PrescriptionDetails.Remove(prescriptionDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrescriptionDetailExists(int id)
        {
          return (_context.PrescriptionDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
