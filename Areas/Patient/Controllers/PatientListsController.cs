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
    public class PatientListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Patient/PatientLists
        public async Task<IActionResult> Index()
        {
              return _context.Patients != null ? 
                          View(await _context.Patients.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Patients'  is null.");
        }

        // GET: Patient/PatientLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patientList = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patientList == null)
            {
                return NotFound();
            }

            return View(patientList);
        }

        // GET: Patient/PatientLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patient/PatientLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Gender,Contact,DateOfBirth")] PatientList patientList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patientList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patientList);
        }

        // GET: Patient/PatientLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patientList = await _context.Patients.FindAsync(id);
            if (patientList == null)
            {
                return NotFound();
            }
            return View(patientList);
        }

        // POST: Patient/PatientLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Gender,Contact,DateOfBirth")] PatientList patientList)
        {
            if (id != patientList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientListExists(patientList.Id))
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
            return View(patientList);
        }

        // GET: Patient/PatientLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patientList = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patientList == null)
            {
                return NotFound();
            }

            return View(patientList);
        }

        // POST: Patient/PatientLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Patients == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Patients'  is null.");
            }
            var patientList = await _context.Patients.FindAsync(id);
            if (patientList != null)
            {
                _context.Patients.Remove(patientList);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientListExists(int id)
        {
          return (_context.Patients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
