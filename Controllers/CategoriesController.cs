﻿using System;
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
    public class CategoriesController : Controller
    {
        private readonly theta_ecommerceContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public CategoriesController(theta_ecommerceContext context, IWebHostEnvironment webHostEnvironment)

        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        // GET: Categories
        public async Task<IActionResult> Index()
        {
              return _context.Categories != null ? 
                          View(await _context.Categories.ToListAsync()) :
                          Problem("Entity set 'theta_ecommerceContext.Categories'  is null.");
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,Description,CreatedBy,ModifiedBy,CreatedDate,ModifiedDate,Status")] Category category, IList<IFormFile> FileImages)
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
                    category.Image = CommmaSeperatedString.Remove(0, 1);
                }
                category.CreatedDate = DateTime.Now;
                category.CreatedBy = "System";
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        //    foreach (IFormFile item in FileImages)
        //    {
        //        var Imagepath = "/images/" + Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
        //        using (FileStream dd = new FileStream(_webHostEnvironment.WebRootPath + Imagepath, FileMode.Create))
        //        {
        //            item.CopyTo(dd);
        //        }
        //        category.Image += "'" + Imagepath;
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(category);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(category);
        //}

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,Description,CreatedBy,ModifiedBy,CreatedDate,ModifiedDate,Status")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'theta_ecommerceContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
