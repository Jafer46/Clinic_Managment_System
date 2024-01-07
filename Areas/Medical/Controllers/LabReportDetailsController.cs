using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BALibrary.Medical;
using CMS.Data;

namespace CMS.Areas.Medical.Controllers
{
    [Area("Medical")]
    public class LabReportDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LabReportDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Medical/LabReportDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LabReportDetails.Include(l => l.LabReport);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Medical/LabReportDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LabReportDetails == null)
            {
                return NotFound();
            }

            var labReportDetail = await _context.LabReportDetails
                .Include(l => l.LabReport)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (labReportDetail == null)
            {
                return NotFound();
            }

            return View(labReportDetail);
        }

        // GET: Medical/LabReportDetails/Create
        public IActionResult Create(int id)
        {
            var labReport = _context.LabReports.Find(id);
            var labReports = _context.LabReports.Where(l => l.Id == id);
            var labRequestDetails = _context.LabRequestDetails
                             .Where(l => l.LabRequestId == labReport.LabRequestId);
            ViewData["LabReportId"] = new SelectList(labReports, "Id", "Id");
            ViewData["LabRequestDetails"] = new SelectList(labRequestDetails, "Case", "Case");
            ViewData["LabReportError"] = "";
            return View();
        }

        // POST: Medical/LabReportDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LabReportId,Case,CaseDetail")] LabReportDetail labReportDetail)
        {
            if (sameReportExists(labReportDetail.LabReportId, labReportDetail.Case))
            {
                var lr = _context.LabReports.Find(labReportDetail.LabReportId);
                var lrs = _context.LabReports.Where(l => l.Id == labReportDetail.LabReportId);
                var lrds= _context.LabRequestDetails
                                 .Where(l => l.LabRequestId == lr.LabRequestId);
                ViewData["LabReportId"] = new SelectList(lrs, "Id", "Id");
                ViewData["LabRequestDetails"] = new SelectList(lrds, "Case", "Case");
                ViewData["LabReportError"] = "A report with the smae case exists";
                return View(labReportDetail);
            }
            if (ModelState.IsValid)
            {
                _context.Add(labReportDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "LabReports");
            }
            var labReport = _context.LabReports.Find(labReportDetail.LabReportId);
            var labReports = _context.LabReports.Where(l => l.Id == labReportDetail.LabReportId);
            var labRequestDetails = _context.LabRequestDetails
                             .Where(l => l.LabRequestId == labReport.LabRequestId);
            ViewData["LabReportId"] = new SelectList(labReports, "Id", "Id");
            ViewData["LabRequestDetails"] = new SelectList(labRequestDetails, "Case", "Case");
            ViewData["LabReportError"] = "";
            return View(labReportDetail);
        }

        // GET: Medical/LabReportDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LabReportDetails == null)
            {
                return NotFound();
            }

            var labReportDetail = await _context.LabReportDetails.FindAsync(id);
            if (labReportDetail == null)
            {
                return NotFound();
            }
            ViewData["LabReportId"] = new SelectList(_context.LabReports, "Id", "Id", labReportDetail.LabReportId);
            return View(labReportDetail);
        }

        // POST: Medical/LabReportDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LabReportId,Case,CaseDetail")] LabReportDetail labReportDetail)
        {
            if (id != labReportDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(labReportDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabReportDetailExists(labReportDetail.Id))
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
            ViewData["LabReportId"] = new SelectList(_context.LabReports, "Id", "Id", labReportDetail.LabReportId);
            return View(labReportDetail);
        }

        // GET: Medical/LabReportDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LabReportDetails == null)
            {
                return NotFound();
            }

            var labReportDetail = await _context.LabReportDetails
                .Include(l => l.LabReport)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (labReportDetail == null)
            {
                return NotFound();
            }

            return View(labReportDetail);
        }

        // POST: Medical/LabReportDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LabReportDetails == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LabReportDetails'  is null.");
            }
            var labReportDetail = await _context.LabReportDetails.FindAsync(id);
            if (labReportDetail != null)
            {
                _context.LabReportDetails.Remove(labReportDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabReportDetailExists(int id)
        {
          return (_context.LabReportDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool sameReportExists(int labReportId, string reportCase)
        {
            return (_context.LabReportDetails?
                .Any(e => e.LabReportId == labReportId && e.Case == reportCase))
                .GetValueOrDefault();
        }
    }
}
