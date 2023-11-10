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
    public class AdminsController : Controller
    {
        private readonly theta_ecommerceContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public AdminsController(theta_ecommerceContext context, IWebHostEnvironment webHostEnvironment)

        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admins
        public async Task<IActionResult> Index()
        {
              return _context.Admins != null ? 
                          View(await _context.Admins.ToListAsync()) :
                          Problem("Entity set 'theta_ecommerceContext.Admins'  is null.");
        }

        // GET: Admins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Admins == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // GET: Admins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,PhoneNo,Email,Address,Cnic,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Role,Status,SystemUserId")] Admin admin, IList<IFormFile> FileImages)
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
                    admin.Image = CommmaSeperatedString.Remove(0, 1);
                }
                admin.CreatedDate = DateTime.Now;
                admin.CreatedBy = "System";
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
        }
        //    foreach (IFormFile item in FileImages)
        //    {
        //        var Imagepath = "/images/" + Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
        //        using (FileStream dd = new FileStream(_webHostEnvironment.WebRootPath + Imagepath, FileMode.Create))
        //        {
        //            item.CopyTo(dd);
        //        }
        //        admin.Image += "'" + Imagepath;
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(admin);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(admin);
        //}

        // GET: Admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Admins == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,PhoneNo,Email,Address,Cnic,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Role,Status,SystemUserId")] Admin admin)
        {
            if (id != admin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(admin.Id))
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
            return View(admin);
        }

        // GET: Admins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Admins == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Admins == null)
            {
                return Problem("Entity set 'theta_ecommerceContext.Admins'  is null.");
            }
            var admin = await _context.Admins.FindAsync(id);
            if (admin != null)
            {
                _context.Admins.Remove(admin);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(int id)
        {
          return (_context.Admins?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
