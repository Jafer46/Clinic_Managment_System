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
    public class RoleModulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoleModulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/RoleModules
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RoleModules.Include(r => r.Module).Include(r => r.Role);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/RoleModules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RoleModules == null)
            {
                return NotFound();
            }

            var roleModule = await _context.RoleModules
                .Include(r => r.Module)
                .Include(r => r.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roleModule == null)
            {
                return NotFound();
            }

            return View(roleModule);
        }

        // GET: Admin/RoleModules/Create
        public IActionResult Create()
        {
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Name");
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View();
        }

        // POST: Admin/RoleModules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoleId,ModuleId")] RoleModule roleModule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roleModule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Name", roleModule.ModuleId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", roleModule.RoleId);
            return View(roleModule);
        }

        // GET: Admin/RoleModules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RoleModules == null)
            {
                return NotFound();
            }

            var roleModule = await _context.RoleModules.FindAsync(id);
            if (roleModule == null)
            {
                return NotFound();
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Name", roleModule.ModuleId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", roleModule.RoleId);
            return View(roleModule);
        }

        // POST: Admin/RoleModules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoleId,ModuleId")] RoleModule roleModule)
        {
            if (id != roleModule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roleModule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleModuleExists(roleModule.Id))
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
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Name", roleModule.ModuleId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", roleModule.RoleId);
            return View(roleModule);
        }

        // GET: Admin/RoleModules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RoleModules == null)
            {
                return NotFound();
            }

            var roleModule = await _context.RoleModules
                .Include(r => r.Module)
                .Include(r => r.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roleModule == null)
            {
                return NotFound();
            }

            return View(roleModule);
        }

        // POST: Admin/RoleModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RoleModules == null)
            {
                return Problem("Entity set 'ApplicationDbContext.RoleModules'  is null.");
            }
            var roleModule = await _context.RoleModules.FindAsync(id);
            if (roleModule != null)
            {
                _context.RoleModules.Remove(roleModule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoleModuleExists(int id)
        {
          return (_context.RoleModules?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
