using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JewelryStore.Data;
using JewelryStore.Models;

namespace JewelryStore.Controllers
{
    public class Order_productsController : Controller
    {
        private readonly JewelryStoreContext _context;

        public Order_productsController(JewelryStoreContext context)
        {
            _context = context;
        }
        private void ProductsDropDownList(object selectedProduct = null)
        {
            var productsQuery = from p in _context.Products
                                   orderby p.name
                                   select p;
            ViewBag.ProductsID = new SelectList(productsQuery.AsNoTracking(), "Id", "name", selectedProduct);
        }
        // GET: Order_products
        public async Task<IActionResult> Index()
        {
            var jewelryStoreContext = _context.Order_products.Include(o => o.Orders).Include(o => o.Products);
            return View(await jewelryStoreContext.ToListAsync());
        }

        // GET: Order_products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order_products == null)
            {
                return NotFound();
            }

            var order_products = await _context.Order_products
                .Include(o => o.Orders)
                .Include(o => o.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order_products == null)
            {
                return NotFound();
            }

            return View(order_products);
        }

        // GET: Order_products/Create
        public IActionResult Create()
        {
            ViewData["OrdersID"] = new SelectList(_context.Orders, "Id", "Id");
            ViewData["ProductsID"] = new SelectList(_context.Products, "Id", "Id");
            ProductsDropDownList();
            return View();
        }

        // POST: Order_products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrdersID,ProductsID,amount,price")] Order_products order_products)
        {
            //if (ModelState.IsValid)
            {
                _context.Add(order_products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrdersID"] = new SelectList(_context.Orders, "Id", "Id", order_products.OrdersID);
            ViewData["ProductsID"] = new SelectList(_context.Products, "Id", "Id", order_products.ProductsID);
            ProductsDropDownList();
            return View(order_products);
        }

        // GET: Order_products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Order_products == null)
            {
                return NotFound();
            }

            var order_products = await _context.Order_products.FindAsync(id);
            if (order_products == null)
            {
                return NotFound();
            }
            ViewData["OrdersID"] = new SelectList(_context.Orders, "Id", "Id", order_products.OrdersID);
            ViewData["ProductsID"] = new SelectList(_context.Products, "Id", "Id", order_products.ProductsID);
            ProductsDropDownList(order_products.ProductsID);
            return View(order_products);
        }

        // POST: Order_products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrdersID,ProductsID,amount,price")] Order_products order_products)
        {
            if (id != order_products.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order_products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Order_productsExists(order_products.Id))
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
            ViewData["OrdersID"] = new SelectList(_context.Orders, "Id", "Id", order_products.OrdersID);
            ViewData["ProductsID"] = new SelectList(_context.Products, "Id", "Id", order_products.ProductsID);
            ProductsDropDownList(order_products.ProductsID);
            return View(order_products);
        }

        // GET: Order_products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order_products == null)
            {
                return NotFound();
            }

            var order_products = await _context.Order_products
                .Include(o => o.Orders)
                .Include(o => o.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order_products == null)
            {
                return NotFound();
            }

            return View(order_products);
        }

        // POST: Order_products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order_products == null)
            {
                return Problem("Entity set 'JewelryStoreContext.Order_products'  is null.");
            }
            var order_products = await _context.Order_products.FindAsync(id);
            if (order_products != null)
            {
                _context.Order_products.Remove(order_products);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Order_productsExists(int id)
        {
          return (_context.Order_products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
