using CMS.Data;
using CMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login(string emlAddress, string passWrd)
        {
            if((_context.Users?.Any(u => u.Email == emlAddress && u.Password == passWrd)).GetValueOrDefault())
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == emlAddress && u.Password == passWrd);
                HttpContext.Session.SetString("SessionKeyUserId", user.Id.ToString());
                HttpContext.Session.SetString("SessionKeyUserEmail", user.Email.ToString());
                HttpContext.Session.SetString("SessionKeyUserRoleId", user.RoleId.ToString());
                HttpContext.Session.SetString("SessionKeySessionId", Guid.NewGuid().ToString());
                TempData["MessageType"] = "success";
                TempData["Message"] = "Successfully Logged in";
                return RedirectToAction("Index", "Home", new {area = "Dashboard"});
            }
            TempData["MessageType"] = "error";
            TempData["Message"] = "unsuccessfully Log in";
            return View(nameof(Index));
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionVariable.SessionKeyUserId);
            HttpContext.Session.Remove(SessionVariable.SessionKeyUserEmail);
            HttpContext.Session.Remove(SessionVariable.SessionKeyUserRoleId);
            HttpContext.Session.Remove(SessionVariable.SessionKeySessionId);

            TempData["MessageType"] = "success";
            TempData["Message"] = "Successfully Logged Out";
            return RedirectToAction("Index", "Home", new { area = "Default" });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}