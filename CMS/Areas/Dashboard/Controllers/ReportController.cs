using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMS.Data;

namespace CMS.Areas.Dashboard.Controllers
{
    public class ReportController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Area("Dashboard")]
        public IActionResult Index()
        {
            var allEntries = _context.Sessions.Count();
            var allDocters = _context.Users.Where(u => u.Role.Name.ToLower() == "docter");
            ViewData["AllEntries"] = allEntries;
            ViewData["Docter"] = allDocters;
            return View();
        }
    }
}
