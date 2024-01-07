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
    public class LabRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LabRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Medical/LabRequests
        public async Task<IActionResult> Index()
        {
            var labr = _context.LabRequests
                       .Include(l => l.Session)
                       .Include(l => l.Session.Patient)
                       .Include(l => l.Session.Docter)
                       .Where(l => l.Session.SessionStatus.Name.ToLower() != "ended");
            var labp = _context.LabPayments.Where(l => l.Checked);
            var LabRequests = (from t in labr
                               join p in labp on t.Id equals p.LabRequestId
                               select new
                               {
                                   t.Id,
                                   t.SessionId,
                                   t.Session
                               });
            ViewData["Req"] = LabRequests;
            return View();
        }

        // GET: Medical/LabRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LabRequests == null)
            {
                return NotFound();
            }

            var labRequest = await _context.LabRequests
                .Include(l => l.Session).Include(l => l.Session.Patient).Include(l => l.Session.Docter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (labRequest == null)
            {
                return NotFound();
            }

            object toL = _context.LabRequestDetails.Where(l => l.LabRequestId == id).ToList();
            var labRequestDetail = toL;
            ViewData["Detail"] = labRequestDetail;
            return View(labRequest);
        }

        // GET: Medical/LabRequests/Create
        public IActionResult Create(int id)
        {
            var session = _context.Sessions.Where(s => s.Id == id);
            ViewData["SessionId"] = new SelectList(session, "Id", "Id");
            return View();
        }

        // POST: Medical/LabRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SessionId")] LabRequest labRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(labRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Sessions");
            }
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", labRequest.SessionId);
            return View(labRequest);
        }

        // GET: Medical/LabRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LabRequests == null)
            {
                return NotFound();
            }

            var labRequest = await _context.LabRequests.FindAsync(id);
            if (labRequest == null)
            {
                return NotFound();
            }
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", labRequest.SessionId);
            return View(labRequest);
        }

        // POST: Medical/LabRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SessionId")] LabRequest labRequest)
        {
            if (id != labRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(labRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabRequestExists(labRequest.Id))
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
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", labRequest.SessionId);
            return View(labRequest);
        }

        // GET: Medical/LabRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LabRequests == null)
            {
                return NotFound();
            }

            var labRequest = await _context.LabRequests
                .Include(l => l.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (labRequest == null)
            {
                return NotFound();
            }

            return View(labRequest);
        }

        // POST: Medical/LabRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LabRequests == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LabRequests'  is null.");
            }
            var labRequest = await _context.LabRequests.FindAsync(id);
            if (labRequest != null)
            {
                _context.LabRequests.Remove(labRequest);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabRequestExists(int id)
        {
          return (_context.LabRequests?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
