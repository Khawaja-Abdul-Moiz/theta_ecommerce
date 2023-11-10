using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using theta_ecommerce.Models;

namespace theta_ecommerce.Controllers
{
    public class PurchaseHistoriesController : Controller
    {
        private readonly theta_ecommerceContext _context;

        public PurchaseHistoriesController(theta_ecommerceContext context)
        {
            _context = context;
        }

        // GET: PurchaseHistories
        public async Task<IActionResult> Index()
        {
            var theta_ecommerceContext = _context.PurchaseHistories.Include(p => p.Product).Include(p => p.Vendor).Include(p => p.Seller);
            return View(await theta_ecommerceContext.ToListAsync());
        }

        // GET: PurchaseHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PurchaseHistories == null)
            {
                return NotFound();
            }

            var purchaseHistory = await _context.PurchaseHistories
                .Include(p => p.Product)
                .Include(p => p.Vendor)
                .Include(p => p.Seller)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchaseHistory == null)
            {
                return NotFound();
            }

            return View(purchaseHistory);
        }

        // GET: PurchaseHistories/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "Id");
            ViewData["SellerId"] = new SelectList(_context.Sellers, "Id", "Id");
            return View();
        }

        // POST: PurchaseHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SellerId,VendorId,ProductId,PurchaseDate,Quantity,PurchasePrice")] PurchaseHistory purchaseHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchaseHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", purchaseHistory.ProductId);
            ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "Id", purchaseHistory.VendorId);
            ViewData["SellerId"] = new SelectList(_context.Sellers, "Id", "Id", purchaseHistory.SellerId);
            return View(purchaseHistory);
        }

        // GET: PurchaseHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PurchaseHistories == null)
            {
                return NotFound();
            }

            var purchaseHistory = await _context.PurchaseHistories.FindAsync(id);
            if (purchaseHistory == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", purchaseHistory.ProductId);
            ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "Id", purchaseHistory.VendorId);
            ViewData["SellerId"] = new SelectList(_context.Sellers, "Id", "Id", purchaseHistory.SellerId);
            return View(purchaseHistory);
        }

        // POST: PurchaseHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SellerId,VendorId,ProductId,PurchaseDate,Quantity,PurchasePrice")] PurchaseHistory purchaseHistory)
        {
            if (id != purchaseHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseHistoryExists(purchaseHistory.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", purchaseHistory.ProductId);
            ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "Id", purchaseHistory.VendorId);
            ViewData["SellerId"] = new SelectList(_context.Sellers, "Id", "Id", purchaseHistory.SellerId);
            return View(purchaseHistory);
        }

        // GET: PurchaseHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PurchaseHistories == null)
            {
                return NotFound();
            }

            var purchaseHistory = await _context.PurchaseHistories
                .Include(p => p.Product)
                .Include(p => p.Vendor)
                .Include(p => p.Seller)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchaseHistory == null)
            {
                return NotFound();
            }

            return View(purchaseHistory);
        }

        // POST: PurchaseHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PurchaseHistories == null)
            {
                return Problem("Entity set 'theta_ecommerceContext.PurchaseHistories'  is null.");
            }
            var purchaseHistory = await _context.PurchaseHistories.FindAsync(id);
            if (purchaseHistory != null)
            {
                _context.PurchaseHistories.Remove(purchaseHistory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseHistoryExists(int id)
        {
          return (_context.PurchaseHistories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
