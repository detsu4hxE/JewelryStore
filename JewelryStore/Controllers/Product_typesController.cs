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
    public class Product_typesController : Controller
    {
        private readonly JewelryStoreContext _context;

        public Product_typesController(JewelryStoreContext context)
        {
            _context = context;
        }

        // GET: Product_types
        public async Task<IActionResult> Index()
        {
              return _context.Product_types != null ? 
                          View(await _context.Product_types.ToListAsync()) :
                          Problem("Entity set 'JewelryStoreContext.Product_types'  is null.");
        }

        // GET: Product_types/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product_types == null)
            {
                return NotFound();
            }

            var product_types = await _context.Product_types
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product_types == null)
            {
                return NotFound();
            }

            return View(product_types);
        }

        // GET: Product_types/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product_types/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name")] Product_types product_types)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product_types);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product_types);
        }

        // GET: Product_types/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product_types == null)
            {
                return NotFound();
            }

            var product_types = await _context.Product_types.FindAsync(id);
            if (product_types == null)
            {
                return NotFound();
            }
            return View(product_types);
        }

        // POST: Product_types/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name")] Product_types product_types)
        {
            if (id != product_types.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product_types);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Product_typesExists(product_types.Id))
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
            return View(product_types);
        }

        // GET: Product_types/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product_types == null)
            {
                return NotFound();
            }

            var product_types = await _context.Product_types
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product_types == null)
            {
                return NotFound();
            }

            return View(product_types);
        }

        // POST: Product_types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product_types == null)
            {
                return Problem("Entity set 'JewelryStoreContext.Product_types'  is null.");
            }
            var product_types = await _context.Product_types.FindAsync(id);
            if (product_types != null)
            {
                _context.Product_types.Remove(product_types);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Product_typesExists(int id)
        {
          return (_context.Product_types?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
