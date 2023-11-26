using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EXAM_Module_6.Data;
using EXAM_Module_6.Models;
using Microsoft.Data.SqlClient;
using EXAM_Module_6.ViewModels;

namespace EXAM_Module_6.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookshopDbContext _context;

        public BooksController(BookshopDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IdSort"] = sortOrder == "Id_asc" ? "Id_desc" : "Id_asc";
            ViewData["NameSort"] = sortOrder == "Name_asc" ? "Name_desc" : "Name_asc";
            ViewData["DescriptionSort"] = sortOrder == "Description_asc" ? "Description_desc" : "Description_asc";
            ViewData["CategoryIdSort"] = sortOrder == "CategoryId_asc" ? "CategoryId_desc" : "CategoryId_asc";
            ViewData["PriceSort"] = sortOrder == "Price_asc" ? "Price_desc" : "Price_asc";
            ViewData["AuthorIdSort"] = sortOrder == "AuthorId_asc" ? "AuthorId_desc" : "AuthorId_asc";

            var books = _context.Books.AsQueryable();

            books = sortOrder switch
            {
                "Id_asc" => books.OrderBy(x => x.Id),
                "Id_desc" => books.OrderByDescending(x => x.Id),
                "Name_asc" => books.OrderBy(x => x.Name),
                "Name_desc" => books.OrderByDescending(x => x.Name),
                "Description_asc" => books.OrderBy(x => x.Description),
                "Description_desc" => books.OrderByDescending(x => x.Description),
                "CategoryId_asc" => books.OrderBy(x => x.BooksCategoryId),
                "CategoryId_desc" => books.OrderByDescending(x => x.BooksCategoryId),
                "Price_asc" => books.OrderBy(x => x.Price),
                "Price_desc" => books.OrderByDescending(x => x.Price),
                "AuthorId_asc" => books.OrderBy(x => x.AuthorId),
                "AuthorId_desc" => books.OrderByDescending(x => x.AuthorId),
                _ => books.OrderBy(x => x.Id)
            };
            var categories = await _context.BooksCategories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToListAsync();

            var bookViewModel = new BookViewModel()
            {
                Books = await books.ToListAsync(),
                Categories = categories
            };

            return View(bookViewModel);

        }

        [HttpPost]
        public async Task<IActionResult> Index(string? searchString, string category)
        {
            int categoryId;

            if (int.TryParse(category, out categoryId))
            {
                var books = await _context.Books
                    .Include(b => b.BooksCategory)
                    .Include(b => b.Author)
                    .Where(b => b.Name.ToLower().Contains(searchString.ToLower()))
                    .Where(c => c.BooksCategoryId == categoryId)
                    .ToListAsync();

                var categories = await _context.BooksCategories
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    })
                    .ToListAsync();

                var bookViewModel = new BookViewModel
                {
                    Books = books,
                    Categories = categories
                };

                return View(bookViewModel);
            }
            else
            {

                var books = await _context.Books
                    .Include(b => b.BooksCategory)
                    .Include(b => b.Author)
                    .Where(b => b.Name.ToLower().Contains(searchString.ToLower()))
                    .ToListAsync();

                var categories = await _context.BooksCategories
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    })
                    .ToListAsync();

                var bookViewModel = new BookViewModel
                {
                    Books = books,
                    Categories = categories
                };

                return View(bookViewModel);
            }
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BooksCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName");
            ViewData["BooksCategoryId"] = new SelectList(_context.BooksCategories, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,AuthorId,BooksCategoryId")] Books books)
        {
            if (books != null)
            {
                _context.Add(books);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName", books.AuthorId);
            ViewData["BooksCategoryId"] = new SelectList(_context.BooksCategories, "Id", "Name", books.BooksCategoryId);
            return View(books);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books.FindAsync(id);
            if (books == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName", books.AuthorId);
            ViewData["BooksCategoryId"] = new SelectList(_context.BooksCategories, "Id", "Name", books.BooksCategoryId);
            return View(books);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,AuthorId,BooksCategoryId")] Books books)
        {
            if (id != books.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(books);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BooksExists(books.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName", books.AuthorId);
            ViewData["BooksCategoryId"] = new SelectList(_context.BooksCategories, "Id", "Name", books.BooksCategoryId);
            return View(books);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BooksCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'BookshopDbContext.Books'  is null.");
            }
            var books = await _context.Books.FindAsync(id);
            if (books != null)
            {
                _context.Books.Remove(books);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BooksExists(int id)
        {
            return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
