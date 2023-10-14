using JewelryStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JewelryStore.Data;
using JewelryStore.Models;

namespace JewelryStore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly JewelryStoreContext _context;

        public HomeController(ILogger<HomeController> logger, JewelryStoreContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var jewelryStoreContext = _context.Products.Include(p => p.Materials).Include(p => p.Product_Types);
            return View(await jewelryStoreContext.ToListAsync());
        }
        public async Task<IActionResult> Index2()
        {
            var jewelryStoreContext = _context.Products.Include(p => p.Materials).Include(p => p.Product_Types);
            return View(await jewelryStoreContext.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.Materials)
                .Include(p => p.Product_Types)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }
        public async Task<IActionResult> Details2(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.Materials)
                .Include(p => p.Product_Types)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Privacy2()
        {
            return View();
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Access");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}