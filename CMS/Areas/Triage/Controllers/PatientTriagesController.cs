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
    public class PatientTriagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientTriagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Triage/PatientTriages
        public async Task<IActionResult> Index()
        {
            
            var tiagre = _context.PatientTriages.Include(p => p.Session).Include(p => p.Session.Patient).Where(p => p.Session.SessionStatus.Name.ToLower() != "ended");
            var triage = (from t in tiagre
                          join c in _context.CardPayment.Where(c => c.Checked) on t.SessionId equals c.SessionId
                          select new
                          {
                              t.Id,
                              t.SessionId,
                              t.Priority,
                              t.Session
                          });
            ViewData["triage"] = await triage.ToListAsync();
            return View();
        }

        // GET: Triage/PatientTriages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PatientTriages == null)
            {
                return NotFound();
            }

            var patientTriage = await _context.PatientTriages
                .Include(p => p.Session)
                .Include(p => p.Session.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patientTriage == null)
            {
                return NotFound();
            }
            var traigeDetails = _context.TriageDetails.Where(t => t.PatientTriageId == id).ToList();
            ViewData["Detail"] = traigeDetails;

            return View(patientTriage);
        }

        // GET: Triage/PatientTriages/Create
        public IActionResult Create(int id)
        {
            var session = _context.Sessions.Where(s => s.Id == id);
            ViewData["SessionId"] = new SelectList(session, "Id", "Id");
            return View();
        }

        // POST: Triage/PatientTriages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SessionId,Priority")] PatientTriage patientTriage)
        {
            if (sameTriageExists(patientTriage.SessionId))
            {
                var pt = _context.PatientTriages
                    .FirstOrDefault(p => p.SessionId == patientTriage.SessionId);
                return RedirectToAction("Details", new { id = pt.Id });
            }
            if (ModelState.IsValid)
            {
                _context.Add(patientTriage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", patientTriage.SessionId);
            return View(patientTriage);
        }

        // GET: Triage/PatientTriages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PatientTriages == null)
            {
                return NotFound();
            }

            var patientTriage = await _context.PatientTriages.FindAsync(id);
            if (patientTriage == null)
            {
                return NotFound();
            }
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", patientTriage.SessionId);
            return View(patientTriage);
        }

        // POST: Triage/PatientTriages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SessionId,Priority")] PatientTriage patientTriage)
        {
            if (id != patientTriage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientTriage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientTriageExists(patientTriage.Id))
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
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", patientTriage.SessionId);
            return View(patientTriage);
        }

        // GET: Triage/PatientTriages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PatientTriages == null)
            {
                return NotFound();
            }

            var patientTriage = await _context.PatientTriages
                .Include(p => p.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patientTriage == null)
            {
                return NotFound();
            }

            return View(patientTriage);
        }

        // POST: Triage/PatientTriages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PatientTriages == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PatientTriages'  is null.");
            }
            var patientTriage = await _context.PatientTriages.FindAsync(id);
            if (patientTriage != null)
            {
                _context.PatientTriages.Remove(patientTriage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientTriageExists(int id)
        {
          return (_context.PatientTriages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool sameTriageExists(int sessionId)
        {
            return (_context.PatientTriages?
                .Any(e => e.SessionId == sessionId)).GetValueOrDefault();
        }
    }
}
