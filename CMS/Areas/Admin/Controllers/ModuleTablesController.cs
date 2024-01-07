using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BALibrary.Admin;
using CMS.Data;

namespace CMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ModuleTablesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModuleTablesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ModuleTables
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ModuleTables.Include(m => m.Module);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/ModuleTables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ModuleTables == null)
            {
                return NotFound();
            }

            var moduleTable = await _context.ModuleTables
                .Include(m => m.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moduleTable == null)
            {
                return NotFound();
            }

            return View(moduleTable);
        }

        // GET: Admin/ModuleTables/Create
        public IActionResult Create()
        {
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Name");
            return View();
        }

        // POST: Admin/ModuleTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TableName,ModuleId")] ModuleTable moduleTable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(moduleTable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Name", moduleTable.ModuleId);
            return View(moduleTable);
        }

        // GET: Admin/ModuleTables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ModuleTables == null)
            {
                return NotFound();
            }

            var moduleTable = await _context.ModuleTables.FindAsync(id);
            if (moduleTable == null)
            {
                return NotFound();
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Name", moduleTable.ModuleId);
            return View(moduleTable);
        }

        // POST: Admin/ModuleTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TableName,ModuleId")] ModuleTable moduleTable)
        {
            if (id != moduleTable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moduleTable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleTableExists(moduleTable.Id))
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
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Name", moduleTable.ModuleId);
            return View(moduleTable);
        }

        // GET: Admin/ModuleTables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ModuleTables == null)
            {
                return NotFound();
            }

            var moduleTable = await _context.ModuleTables
                .Include(m => m.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moduleTable == null)
            {
                return NotFound();
            }

            return View(moduleTable);
        }

        // POST: Admin/ModuleTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ModuleTables == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ModuleTables'  is null.");
            }
            var moduleTable = await _context.ModuleTables.FindAsync(id);
            if (moduleTable != null)
            {
                _context.ModuleTables.Remove(moduleTable);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuleTableExists(int id)
        {
          return (_context.ModuleTables?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
