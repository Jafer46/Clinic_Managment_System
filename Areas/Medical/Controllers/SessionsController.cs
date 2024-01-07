using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BALibrary.Medical;
using BALibrary.Patient;
using CMS.Data;

namespace CMS.Areas.Medical.Controllers
{
    [Area("Medical")]
    public class SessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Medical/Sessions
        public async Task<IActionResult> Index(int? patientId)
        {
            
            if (patientId != null) {
                var applicationDbContext = _context.Sessions
                                           .Include(s => s.Docter)
                                           .Include(s => s.Patient)
                                           .Include(s => s.SessionStatus)
                                           .Where(s => s.PatientId == patientId && s.SessionStatus.Name.ToLower() != "ended");
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.Sessions
                                           .Include(s => s.Docter)
                                           .Include(s => s.Patient)
                                           .Include(s => s.SessionStatus)
                                           .Where  (s => s.SessionStatus.Name.ToLower() != "ended");
                return View(await applicationDbContext.ToListAsync());
            }            
        }

        

        // GET: Medical/Sessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sessions == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.Docter)
                .Include(s => s.Patient)
                .Include(s => s.SessionStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (session == null)
            {
                return NotFound();
            }
            object toL = _context.LabRequests.Where(l => l.SessionId == id).ToList();
            ViewData["Detail"] = toL;
            var patientRecords = _context.PatientRecords
                .Where(p => p.PatientId == session.PatientId)
                .OrderByDescending(p => p.LeaveDate);
            var prescription = _context.Prescription
                .Where(p => p.SessionId == session.Id);
            ViewData["PatientRecord"] = patientRecords;
            ViewData["Prescription"] = prescription;

            return View(session);
        }

        // GET: Medical/Sessions/Create
        public IActionResult Create(int id)
        {
            if (PatientHasSession(id))
            {
                return RedirectToAction("Index", new { patientId = id });
            }
            var patient = _context.Patients.Where(p => p.Id == id).ToList();
            var docter = _context.Users.Where(u => u.Role.Name == "Docter");

            ViewData["DocterId"] = new SelectList(docter, "Id", "FirstName");
            ViewData["PatientId"] = new SelectList(patient, "Id", "FirstName");
            ViewData["Status"] = new SelectList(_context.SessionStatuses, "Id", "Name");

            return View();
        }

        // POST: Medical/Sessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId,SartDateTime,DocterId,Status")] Session session)
        {
            if (ModelState.IsValid)
            {
                _context.Add(session);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "CardPayments", new {area = "Payment", SessionId = session.Id});
            }
            ViewData["DocterId"] = new SelectList(_context.Users, "Id", "FirstName", session.DocterId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FirstName", session.PatientId);
            ViewData["Status"] = new SelectList(_context.SessionStatuses, "Id", "Name", session.Status);
            return View(session);
        }

        // GET: Medical/Sessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sessions == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }
            ViewData["DocterId"] = new SelectList(_context.Users, "Id", "FirstName", session.DocterId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FirstName", session.PatientId);
            ViewData["Status"] = new SelectList(_context.SessionStatuses, "Id", "Name", session.Status);
            return View(session);
        }
        public async Task<IActionResult> Edit2(int? id)
        {
            if (id == null || _context.Sessions == null)
            {
                return NotFound();
            }
            var session = await _context.Sessions.FindAsync(id);
            session.Status = 3;
            try
            {
                _context.Update(session);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionExists(session.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Create" , "RecordSummaries" ,new {area = "Patient" ,SessionId = session.Id});

        }

        // POST: Medical/Sessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PatientId,SartDateTime,DocterId,Status")] Session session)
        {
            if (id != session.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(session);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessionExists(session.Id))
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
            ViewData["DocterId"] = new SelectList(_context.Users, "Id", "FirstName", session.DocterId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FirstName", session.PatientId);
            ViewData["Status"] = new SelectList(_context.SessionStatuses, "Id", "Name", session.Status);
            return View(session);
        }

        // GET: Medical/Sessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sessions == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.Docter)
                .Include(s => s.Patient)
                .Include(s => s.SessionStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // POST: Medical/Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, [Bind("PatientId,LeaveDate,SessionId")]PatientRecord patientRecord)
        {
            if (_context.Sessions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sessions'  is null.");
            }
            var session = await _context.Sessions.FindAsync(id);
            var sessionStatus = await _context.SessionStatuses
                       .FirstOrDefaultAsync(s => s.Name.ToLower() == "ended");
            if (session != null && sessionStatus != null)
            {
                session.Status = sessionStatus.Id;
                _context.Update(session);
                await _context.SaveChangesAsync();
                _context.Add(patientRecord);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SessionExists(int id)
        {
          return (_context.Sessions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool PatientHasSession(int PatientId)
        {
            return (_context.Sessions?.Any(e => e.PatientId == PatientId && (e.SessionStatus.Name.ToLower() != "ended"))).GetValueOrDefault();
        }
    }
}
