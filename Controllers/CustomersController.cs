using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using theta_ecommerce.Models;

namespace theta_ecommerce.Controllers
{
    public class CustomersController : Controller
    {
        private readonly theta_ecommerceContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public CustomersController(theta_ecommerceContext context, IWebHostEnvironment webHostEnvironment)

        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
              return _context.Customers != null ? 
                          View(await _context.Customers.ToListAsync()) :
                          Problem("Entity set 'theta_ecommerceContext.Customers'  is null.");
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,PhoneNo,Email,Address,SystemUserId,CreatedBy,CreatedDate,ModifyDate,ModifiedBy,Status")] Customer customer, IList<IFormFile> FileImages)
        {
            var CommmaSeperatedString = "";
            if (FileImages != null)
            {
                foreach (IFormFile item in FileImages)
                {
                    var Imagepath = "/images/" + Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                    using (FileStream dd = new FileStream(_webHostEnvironment.WebRootPath + Imagepath, FileMode.Create))
                    {
                        item.CopyTo(dd);
                    }
                    //product.Images += "," + Imagepath;
                    CommmaSeperatedString += "," + Imagepath;
                }

            }
            if (ModelState.IsValid)
            {
                if (CommmaSeperatedString.StartsWith(","))
                {
                    customer.Image = CommmaSeperatedString.Remove(0, 1);
                }
                customer.CreatedDate = DateTime.Now;
                customer.CreatedBy = "System";
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
        //    foreach (IFormFile item in FileImages)
        //    {
        //        var Imagepath = "/images/" + Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
        //        using (FileStream dd = new FileStream(_webHostEnvironment.WebRootPath + Imagepath, FileMode.Create))
        //        {
        //            item.CopyTo(dd);
        //        }
        //        customer.Image += "'" + Imagepath;
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(customer);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(customer);
        //}

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,PhoneNo,Email,Address,SystemUserId,CreatedBy,CreatedDate,ModifyDate,ModifiedBy,Status")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'theta_ecommerceContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
          return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
