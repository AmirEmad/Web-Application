using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using New_Pharmacy.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace New_Pharmacy.Controllers
{
    public class ProductsController : Controller
    {
        private readonly New_PharmacyContext _context;
        private readonly IHostingEnvironment _hosting;

        public ProductsController(New_PharmacyContext context, IHostingEnvironment hosting)
        {
            _context = context;
            _hosting = hosting;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var new_PharmacyContext = _context.Products.Include(p => p.Cat);
            return View(await new_PharmacyContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cat)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CatId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PubDate,ExpDate,Quantity,Price,Image,CatId,FileUplaod")] Product product)
        {

            //cat return null
            if (ModelState.IsValid)
            {
                Uplaod_File(product);
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatId"] = new SelectList(_context.Categories, "Id", "Name", product.CatId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CatId"] = new SelectList(_context.Categories, "Id", "Name", product.CatId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PubDate,ExpDate,Quantity,Price,Image,CatId,FileUplaod")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    EditUploadFile(product);
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CatId"] = new SelectList(_context.Categories, "Id", "Name", product.CatId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cat)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'New_PharmacyContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        void Uplaod_File(Product product)
        {
            string rootpath = Path.Combine(_hosting.WebRootPath, "Uploads");
            string filename = product.FileUplaod.FileName;
            string fullpath = Path.Combine(rootpath, filename);
            product.FileUplaod.CopyTo(new FileStream(fullpath, FileMode.Create));
            product.Image = filename;
        }

        void EditUploadFile(Product product)
        {

            string rootpath = Path.Combine(_hosting.WebRootPath, "Uploads");
            string filename = product.FileUplaod.FileName;
            string fullpath = Path.Combine(rootpath, filename);

            string oldpath = _context.Products.Find(product.Id).Image;
            string fulloldpath = Path.Combine(rootpath, oldpath);
            if (fullpath != fulloldpath)
            {
                System.IO.File.Delete(fulloldpath);
                product.FileUplaod.CopyTo(new FileStream(fullpath, FileMode.Create));
            }
            product.Image = filename;

        }
       
        [HttpPost]
        public async Task<IActionResult> Search(string term)
        {
            var result = _context.Products.Where(x => x.Name.Contains(term) || x.Cat.Name.Contains(term)).Include(p => p.Cat).ToList();
            return View(nameof(Index) , result);
        }
    }
}
