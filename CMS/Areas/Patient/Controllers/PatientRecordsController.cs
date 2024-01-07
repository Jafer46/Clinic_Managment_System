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
    public class PatientRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Patient/PatientRecords
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PatientRecords.Include(p => p.Session).Include(p => p.Session.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Patient/PatientRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PatientRecords == null)
            {
                return NotFound();
            }

            var patientRecord = await _context.PatientRecords
                .Include(p => p.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patientRecord == null)
            {
                return NotFound();
            }
            var session = _context.Sessions
                .Include(s => s.Patient)
                .Include(s => s.Docter)
                .FirstOrDefault(s => s.Id == patientRecord.SessionId);
            var recordSummary = _context.RecordSummaries.FirstOrDefault(r => r.SessionId == session.Id);
            var labRequest = _context.LabRequests.Where(l => l.SessionId == session.Id);
            var triage = _context.PatientTriages.FirstOrDefault(p => p.SessionId == session.Id);
            var triageDetail = _context.TriageDetails.Where(t => t.PatientTriageId == triage.Id);
            var cardPayment = _context.CardPayment.FirstOrDefault(c => c.SessionId == session.Id);
            var prescription = _context.Prescription.Where(p => p.SessionId == session.Id);
            ViewData["Session"] = session;
            ViewData["RecordSummary"] = recordSummary;
            ViewData["CardPayment"] = cardPayment;
            ViewData["Triage"] = triage;
            ViewData["TriageDetails"] = triageDetail;


            return View(patientRecord);
        }

        // GET: Patient/PatientRecords/Create
        public IActionResult Create()
        {
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id");
            return View();
        }

        // POST: Patient/PatientRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PatientId,LeaveDate,SessionId")] PatientRecord patientRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patientRecord);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", patientRecord.SessionId);
            return View(patientRecord);
        }

        // GET: Patient/PatientRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PatientRecords == null)
            {
                return NotFound();
            }

            var patientRecord = await _context.PatientRecords.FindAsync(id);
            if (patientRecord == null)
            {
                return NotFound();
            }
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", patientRecord.SessionId);
            return View(patientRecord);
        }

        // POST: Patient/PatientRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PatientId,LeaveDate,SessionId")] PatientRecord patientRecord)
        {
            if (id != patientRecord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientRecordExists(patientRecord.Id))
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
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", patientRecord.SessionId);
            return View(patientRecord);
        }

        // GET: Patient/PatientRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PatientRecords == null)
            {
                return NotFound();
            }

            var patientRecord = await _context.PatientRecords
                .Include(p => p.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patientRecord == null)
            {
                return NotFound();
            }

            return View(patientRecord);
        }

        // POST: Patient/PatientRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PatientRecords == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PatientRecords'  is null.");
            }
            var patientRecord = await _context.PatientRecords.FindAsync(id);
            
            if (patientRecord != null)
            {
                var session = await _context.Sessions.FindAsync(patientRecord.SessionId);                
                _context.PatientRecords.Remove(patientRecord);
                if(session != null)
                {
                    _context.Sessions.Remove(session);
                }                
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientRecordExists(int id)
        {
          return (_context.PatientRecords?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
