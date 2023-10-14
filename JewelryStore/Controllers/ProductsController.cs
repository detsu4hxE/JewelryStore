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
    public class ProductsController : Controller
    {
        private readonly JewelryStoreContext _context;

        public ProductsController(JewelryStoreContext context)
        {
            _context = context;
        }

        // GET: Products

        private void UpdateDropDownList(object selectedProductType = null, object selectedMaterial = null)
        {
            var productTypesQuery = from p in _context.Product_types
                             orderby p.name
                             select p;
            ViewBag.Product_typesID = new SelectList(productTypesQuery.AsNoTracking(), "Id", "name", selectedProductType);

            var materialsQuery = from m in _context.Materials
                                    orderby m.name
                                    select m;
            ViewBag.MaterialsID = new SelectList(materialsQuery.AsNoTracking(), "Id", "name", selectedMaterial);
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

        // GET: Products/Details/5
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

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["MaterialsID"] = new SelectList(_context.Materials, "Id", "Id");
            ViewData["Product_typesID"] = new SelectList(_context.Product_types, "Id", "Id");
            UpdateDropDownList();
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,Product_typesID,MaterialsID,price,weight,image")] Products products)
        {
            //if (ModelState.IsValid)
            {
                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaterialsID"] = new SelectList(_context.Materials, "Id", "Id", products.MaterialsID);
            ViewData["Product_typesID"] = new SelectList(_context.Product_types, "Id", "Id", products.Product_typesID);
            UpdateDropDownList();
            return View(products);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["MaterialsID"] = new SelectList(_context.Materials, "Id", "Id", products.MaterialsID);
            ViewData["Product_typesID"] = new SelectList(_context.Product_types, "Id", "Id", products.Product_typesID);
            UpdateDropDownList(products.Product_typesID, products.MaterialsID);
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,Product_typesID,MaterialsID,price,weight,image")] Products products)
        {
            if (id != products.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.Id))
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
            ViewData["MaterialsID"] = new SelectList(_context.Materials, "Id", "Id", products.MaterialsID);
            ViewData["Product_typesID"] = new SelectList(_context.Product_types, "Id", "Id", products.Product_typesID);
            UpdateDropDownList(products.Product_typesID, products.MaterialsID);
            return View(products);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'JewelryStoreContext.Products'  is null.");
            }
            var products = await _context.Products.FindAsync(id);
            if (products != null)
            {
                _context.Products.Remove(products);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
