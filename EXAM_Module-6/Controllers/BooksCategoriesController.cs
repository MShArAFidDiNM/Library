using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EXAM_Module_6.Data;
using EXAM_Module_6.Models;

namespace EXAM_Module_6.Controllers
{
    public class BooksCategoriesController : Controller
    {
        private readonly BookshopDbContext _context;

        public BooksCategoriesController(BookshopDbContext context)
        {
            _context = context;
        }

        // GET: BooksCategories
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IdSort"] = sortOrder == "Id_asc" ? "Id_desc" : "Id_asc";
            ViewData["NameSort"] = sortOrder == "Name_asc" ? "Name_desc" : "Name_asc";

            var categories = _context.BooksCategories.AsQueryable();

            categories = sortOrder switch
            {
                "Id_asc" => categories.OrderBy(x => x.Id),
                "Id_desc" => categories.OrderByDescending(x => x.Id),
                "Name_asc" => categories.OrderBy(x => x.Name),
                "Name_desc" => categories.OrderByDescending(x => x.Name),
                _ => categories.OrderBy(x => x.Id)
            }; ;
            return View(categories);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string? searchString, string a)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return View(await _context.BooksCategories.ToListAsync());
            }

            var authors = await _context.BooksCategories
                .Where(s => s.Name.ToLower().Contains(searchString.ToLower()))
                .ToListAsync();

            return View(authors);
        }

        // GET: BooksCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BooksCategories == null)
            {
                return NotFound();
            }

            var booksCategory = await _context.BooksCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booksCategory == null)
            {
                return NotFound();
            }

            return View(booksCategory);
        }

        // GET: BooksCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BooksCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] BooksCategory booksCategory)
        {
            if (booksCategory != null)
            {
                _context.Add(booksCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(booksCategory);
        }

        // GET: BooksCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BooksCategories == null)
            {
                return NotFound();
            }

            var booksCategory = await _context.BooksCategories.FindAsync(id);
            if (booksCategory == null)
            {
                return NotFound();
            }
            return View(booksCategory);
        }

        // POST: BooksCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] BooksCategory booksCategory)
        {
            if (id != booksCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booksCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BooksCategoryExists(booksCategory.Id))
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
            return View(booksCategory);
        }

        // GET: BooksCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BooksCategories == null)
            {
                return NotFound();
            }

            var booksCategory = await _context.BooksCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booksCategory == null)
            {
                return NotFound();
            }

            return View(booksCategory);
        }

        // POST: BooksCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BooksCategories == null)
            {
                return Problem("Entity set 'BookshopDbContext.BooksCategories'  is null.");
            }
            var booksCategory = await _context.BooksCategories.FindAsync(id);
            if (booksCategory != null)
            {
                _context.BooksCategories.Remove(booksCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BooksCategoryExists(int id)
        {
            return (_context.BooksCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
