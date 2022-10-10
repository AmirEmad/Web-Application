using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using New_Pharmacy.Models;
using System.Diagnostics;

namespace New_Pharmacy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly New_PharmacyContext _context;

        public HomeController(ILogger<HomeController> logger , New_PharmacyContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var cat = _context.Categories.Include(p => p.Products);
            return View(cat);
        }

        public IActionResult Privacy()
        {
            return View();
        } 
        public IActionResult Shop()
        {
            var cat = _context.Categories.Include(p => p.Products);
            return View(cat);
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(ContactU contact)
        {
            _context.ContactUs.Add(contact);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}