using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BALibrary.Patient;
using CMS.Data;

namespace CMS.Areas.Patient.Controllers
{
    [Area("Patient")]
    public class RecordSummariesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecordSummariesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Patient/RecordSummaries
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RecordSummaries.Include(r => r.Session);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Patient/RecordSummaries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RecordSummaries == null)
            {
                return NotFound();
            }

            var recordSummary = await _context.RecordSummaries
                .Include(r => r.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recordSummary == null)
            {
                return NotFound();
            }

            return View(recordSummary);
        }

        // GET: Patient/RecordSummaries/Create
        public IActionResult Create(int SessionId)
        {
            var Session = _context.Sessions.Where(s => s.Id == SessionId);
            ViewData["SessionId"] = new SelectList(Session, "Id", "Id");
            return View();
        }

        // POST: Patient/RecordSummaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,SessionId")] RecordSummary recordSummary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recordSummary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", recordSummary.SessionId);
            return View(recordSummary);
        }

        // GET: Patient/RecordSummaries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RecordSummaries == null)
            {
                return NotFound();
            }

            var recordSummary = await _context.RecordSummaries.FindAsync(id);
            if (recordSummary == null)
            {
                return NotFound();
            }
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", recordSummary.SessionId);
            return View(recordSummary);
        }

        // POST: Patient/RecordSummaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,SessionId")] RecordSummary recordSummary)
        {
            if (id != recordSummary.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recordSummary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecordSummaryExists(recordSummary.Id))
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
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", recordSummary.SessionId);
            return View(recordSummary);
        }

        // GET: Patient/RecordSummaries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RecordSummaries == null)
            {
                return NotFound();
            }

            var recordSummary = await _context.RecordSummaries
                .Include(r => r.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recordSummary == null)
            {
                return NotFound();
            }

            return View(recordSummary);
        }

        // POST: Patient/RecordSummaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RecordSummaries == null)
            {
                return Problem("Entity set 'ApplicationDbContext.RecordSummaries'  is null.");
            }
            var recordSummary = await _context.RecordSummaries.FindAsync(id);
            if (recordSummary != null)
            {
                _context.RecordSummaries.Remove(recordSummary);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecordSummaryExists(int id)
        {
          return (_context.RecordSummaries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
