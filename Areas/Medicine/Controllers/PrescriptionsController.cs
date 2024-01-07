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
    public class PrescriptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Prescription/Prescriptions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Prescription.Include(p => p.Session).Include(p => p.Session.Patient).Include(p => p.Session.Docter);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Prescription/Prescriptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Prescription == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescription
                .Include(p => p.Session).Include(p => p.Session.Patient).Include(p => p.Session.Docter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prescription == null)
            {
                return NotFound();
            }
            object toL = _context.PrescriptionDetails.Where(l => l.PrescriptionId == id).ToList();
            var prescriptionDetail = toL;
            ViewData["Detail"] = prescriptionDetail;

            return View(prescription);
        }

        // GET: Prescription/Prescriptions/Create
        public IActionResult Create(int id)
        {
            var session = _context.Sessions.Where(s => s.Id == id);
            ViewData["SessionId"] = new SelectList(session, "Id", "Id");
            return View();
        }

        // POST: Prescription/Prescriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,SessionId")] Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prescription);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details","Sessions", new {area = "Medical", id = prescription.SessionId});
            }
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", prescription.SessionId);
            return View(prescription);
        }

        // GET: Prescription/Prescriptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Prescription == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescription.FindAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", prescription.SessionId);
            return View(prescription);
        }

        // POST: Prescription/Prescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,SessionId")] Prescription prescription)
        {
            if (id != prescription.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prescription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrescriptionExists(prescription.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Sessions", new { area = "Medical", id = prescription.SessionId });
            }
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", prescription.SessionId);
            return View(prescription);
        }

        // GET: Prescription/Prescriptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Prescription == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescription
                .Include(p => p.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prescription == null)
            {
                return NotFound();
            }

            return View(prescription);
        }

        // POST: Prescription/Prescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Prescription == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Prescription'  is null.");
            }
            var prescription = await _context.Prescription.FindAsync(id);
            if (prescription != null)
            {
                _context.Prescription.Remove(prescription);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrescriptionExists(int id)
        {
          return (_context.Prescription?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
    }
}
