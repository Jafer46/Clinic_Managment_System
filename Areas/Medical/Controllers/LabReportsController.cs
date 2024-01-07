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
    public class LabReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LabReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Medical/LabReports
        public async Task<IActionResult> Index(int? id)
        {
            if (id != null)
            {
                var lr = _context.LabReports
                    .Include(l => l.LabRequest)
                    .Where(l => l.LabRequestId == id);
                return View(await lr.ToListAsync());
            }
            var applicationDbContext = _context.LabReports.Include(l => l.LabRequest);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Medical/LabReports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LabReports == null)
            {
                return NotFound();
            }

            var labReport = await _context.LabReports
                .Include(l => l.LabRequest)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (labReport == null)
            {
                return NotFound();
            }
            object toL = _context.LabReportDetails.Where(l => l.LabReportId == id).ToList();
            var labRequestDetail = toL;
            ViewData["Detail"] = labRequestDetail;

            return View(labReport);
        }

        // GET: Medical/LabReports/Create
        public IActionResult Create(int id)
        {
            var labRequest = _context.LabRequests.Where(l => l.Id == id);
            var labDocter = _context.Users.Where(u => u.Role.Name.ToLower() == "labdocter");
            ViewData["LabRequestId"] = new SelectList(labRequest, "Id", "Id");
            ViewData["LabDocterId"] = new SelectList(labDocter, "Id", "FirstName");
            return View();
        }

        // POST: Medical/LabReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LabRequestId,LabDocterId")] LabReport labReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(labReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var labDocter = _context.Users.Where(u => u.Role.Name.ToLower() == "labdocter");
            ViewData["LabRequestId"] = new SelectList(_context.LabRequests, "Id", "Id", labReport.LabRequestId);
            ViewData["LabDocterId"] = new SelectList(labDocter, "Id", "FirstName",labReport.LabDocterId);
            return View(labReport);
        }

        // GET: Medical/LabReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LabReports == null)
            {
                return NotFound();
            }

            var labReport = await _context.LabReports.FindAsync(id);
            if (labReport == null)
            {
                return NotFound();
            }
            ViewData["LabRequestId"] = new SelectList(_context.LabRequests, "Id", "Id", labReport.LabRequestId);
            return View(labReport);
        }

        // POST: Medical/LabReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LabRequestId,LabDocterId")] LabReport labReport)
        {
            if (id != labReport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(labReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabReportExists(labReport.Id))
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
            ViewData["LabRequestId"] = new SelectList(_context.LabRequests, "Id", "Id", labReport.LabRequestId);
            return View(labReport);
        }

        // GET: Medical/LabReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LabReports == null)
            {
                return NotFound();
            }

            var labReport = await _context.LabReports
                .Include(l => l.LabRequest)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (labReport == null)
            {
                return NotFound();
            }

            return View(labReport);
        }

        // POST: Medical/LabReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LabReports == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LabReports'  is null.");
            }
            var labReport = await _context.LabReports.FindAsync(id);
            if (labReport != null)
            {
                _context.LabReports.Remove(labReport);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabReportExists(int id)
        {
          return (_context.LabReports?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
