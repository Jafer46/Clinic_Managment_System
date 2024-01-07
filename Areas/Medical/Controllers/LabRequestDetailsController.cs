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
    public class LabRequestDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LabRequestDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Medical/LabRequestDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LabRequestDetails.Include(l => l.LabRequest);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Medical/LabRequestDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LabRequestDetails == null)
            {
                return NotFound();
            }

            var labRequestDetail = await _context.LabRequestDetails
                .Include(l => l.LabRequest)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (labRequestDetail == null)
            {
                return NotFound();
            }

            return View(labRequestDetail);
        }

        // GET: Medical/LabRequestDetails/Create
        public IActionResult Create(int id)
        {
            var labRequest = _context.LabRequests.Where(l => l.Id == id);
            ViewData["LabRequestId"] = new SelectList(labRequest, "Id", "Id");
            ViewData["LabRequestError"] = "";
            return View();
        }

        // POST: Medical/LabRequestDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LabRequestId,Case,CaseDetail")] LabRequestDetail labRequestDetail)
        {
            if(sameRequestExists(labRequestDetail.LabRequestId, labRequestDetail.Case))
            {
                ViewData["LabRequestId"] = new SelectList(_context.LabRequests, "Id", "Id", labRequestDetail.LabRequestId);
                ViewData["LabRequestError"] = "There is a request with the same case";
                return View(labRequestDetail);
            }
            if (ModelState.IsValid)
            {
                _context.Add(labRequestDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details" , "LabRequests", new {id = labRequestDetail.LabRequestId});
            }
            ViewData["LabRequestId"] = new SelectList(_context.LabRequests, "Id", "Id", labRequestDetail.LabRequestId);
            ViewData["LabRequestError"] = "";
            return View(labRequestDetail);
        }

        // GET: Medical/LabRequestDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LabRequestDetails == null)
            {
                return NotFound();
            }

            var labRequestDetail = await _context.LabRequestDetails.FindAsync(id);
            if (labRequestDetail == null)
            {
                return NotFound();
            }
            ViewData["LabRequestId"] = new SelectList(_context.LabRequests, "Id", "Id", labRequestDetail.LabRequestId);
            return View(labRequestDetail);
        }

        // POST: Medical/LabRequestDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LabRequestId,Case,CaseDetail")] LabRequestDetail labRequestDetail)
        {
            if (id != labRequestDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(labRequestDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabRequestDetailExists(labRequestDetail.Id))
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
            ViewData["LabRequestId"] = new SelectList(_context.LabRequests, "Id", "Id", labRequestDetail.LabRequestId);
            return View(labRequestDetail);
        }

        // GET: Medical/LabRequestDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LabRequestDetails == null)
            {
                return NotFound();
            }

            var labRequestDetail = await _context.LabRequestDetails
                .Include(l => l.LabRequest)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (labRequestDetail == null)
            {
                return NotFound();
            }

            return View(labRequestDetail);
        }

        // POST: Medical/LabRequestDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LabRequestDetails == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LabRequestDetails'  is null.");
            }
            var labRequestDetail = await _context.LabRequestDetails.FindAsync(id);
            if (labRequestDetail != null)
            {
                _context.LabRequestDetails.Remove(labRequestDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabRequestDetailExists(int id)
        {
          return (_context.LabRequestDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool sameRequestExists(int labRequestId, string requestCase)
        {
            return (_context.LabRequestDetails?
                .Any(e => e.LabRequestId == labRequestId && e.Case == requestCase))
                .GetValueOrDefault();
        }
    }
}
