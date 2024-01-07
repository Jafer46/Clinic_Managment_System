using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BALibrary.Triage;
using CMS.Data;

namespace CMS.Areas.Triage.Controllers
{
    [Area("Triage")]
    public class TriageDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TriageDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Triage/TriageDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TriageDetails.Include(t => t.PatientTriage);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Triage/TriageDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TriageDetails == null)
            {
                return NotFound();
            }

            var triageDetail = await _context.TriageDetails
                .Include(t => t.PatientTriage)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (triageDetail == null)
            {
                return NotFound();
            }

            return View(triageDetail);
        }

        // GET: Triage/TriageDetails/Create
        public IActionResult Create(int id)
        {
            var patientTriage = _context.PatientTriages.Where(p => p.Id == id);
            ViewData["PatientTriageId"] = new SelectList(patientTriage, "Id", "Id");
            return View();
        }

        // POST: Triage/TriageDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientTriageId,Case,CaseDetail")] TriageDetail triageDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(triageDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "PatientTriages", new {id = triageDetail.PatientTriageId});
            }
            ViewData["PatientTriageId"] = new SelectList(_context.PatientTriages, "Id", "Id", triageDetail.PatientTriageId);
            return View(triageDetail);
        }

        // GET: Triage/TriageDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TriageDetails == null)
            {
                return NotFound();
            }

            var triageDetail = await _context.TriageDetails.FindAsync(id);
            if (triageDetail == null)
            {
                return NotFound();
            }
            ViewData["PatientTriageId"] = new SelectList(_context.PatientTriages, "Id", "Id", triageDetail.PatientTriageId);
            return View(triageDetail);
        }

        // POST: Triage/TriageDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PatientTriageId,Case,CaseDetail")] TriageDetail triageDetail)
        {
            if (id != triageDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(triageDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TriageDetailExists(triageDetail.Id))
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
            ViewData["PatientTriageId"] = new SelectList(_context.PatientTriages, "Id", "Id", triageDetail.PatientTriageId);
            return View(triageDetail);
        }

        // GET: Triage/TriageDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TriageDetails == null)
            {
                return NotFound();
            }

            var triageDetail = await _context.TriageDetails
                .Include(t => t.PatientTriage)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (triageDetail == null)
            {
                return NotFound();
            }

            return View(triageDetail);
        }

        // POST: Triage/TriageDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TriageDetails == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TriageDetails'  is null.");
            }
            var triageDetail = await _context.TriageDetails.FindAsync(id);
            if (triageDetail != null)
            {
                _context.TriageDetails.Remove(triageDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TriageDetailExists(int id)
        {
          return (_context.TriageDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
