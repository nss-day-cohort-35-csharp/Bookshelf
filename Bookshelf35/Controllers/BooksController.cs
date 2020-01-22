using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bookshelf35.Data;
using Bookshelf35.Models;
using Microsoft.AspNetCore.Identity;
using Bookshelf35.Models.ViewModels;

namespace Bookshelf35.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BooksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            // This is how we GET related entities with a many to many
            var applicationDbContext = _context.Book
                .Include(b => b.Author)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.ApplicationUser)
                .Include(b => b.Author)
                .Include(b => b.BookGenres)
                    .ThenInclude(g => g.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public async Task<IActionResult> Create()
        {
            var user = await GetCurrentUserAsync();
            var authors = _context.Author.Where(a => a.ApplicationUserId == user.Id);
            ViewData["AuthorId"] = new SelectList(authors, "Id", "Name");
            ViewData["Genres"] = new SelectList(_context.Genre, "Id", "Description");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,AuthorId,YearPublished,Rating,GenreIds")] BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();

                // Add the book to the database
                var bookDataModel = new Book
                {
                    Title = bookViewModel.Title,
                    AuthorId = bookViewModel.AuthorId,
                    YearPublished = bookViewModel.YearPublished,
                    Rating = bookViewModel.Rating,
                    ApplicationUserId = user.Id
                };
                _context.Add(bookDataModel);
                await _context.SaveChangesAsync();

                // After saving, the Book data model now has an Id. Add genres now
                bookDataModel.BookGenres = bookViewModel.GenreIds.Select(genreId => new BookGenre
                {
                    BookId = bookDataModel.Id,
                    GenreId = genreId
                }).ToList();

                // Save again to database
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Genre, "Id", "Name", bookViewModel.AuthorId);
            ViewData["Genres"] = new SelectList(_context.Genre, "Id", "Description", bookViewModel.GenreIds);
            return View(bookViewModel);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.BookGenres)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            // convert data model to view model
            var bookViewModel = new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                AuthorId = book.AuthorId,
                Rating = book.Rating,
                YearPublished = book.YearPublished,
                GenreIds = book.BookGenres.Select(bg => bg.GenreId).ToList()
            };

            ViewData["Genres"] = new SelectList(_context.Genre, "Id", "Description", bookViewModel.GenreIds);
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "Name", bookViewModel.AuthorId);
            return View(bookViewModel);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,AuthorId,YearPublished,Rating,ApplicationUserId,GenreIds")] BookViewModel bookViewModel)
        {
            if (id != bookViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Get existing data
                var bookDataModel = await _context.Book
                    .Include(b => b.BookGenres)
                    .FirstOrDefaultAsync(b => b.Id == id);

                // Update data
                bookDataModel.Id = bookViewModel.Id;
                bookDataModel.Title = bookViewModel.Title;
                bookDataModel.Rating = bookViewModel.Rating;
                bookDataModel.YearPublished = bookViewModel.YearPublished;
                bookDataModel.AuthorId = bookViewModel.AuthorId;
                bookDataModel.ApplicationUserId = bookViewModel.ApplicationUserId;
                bookDataModel.BookGenres = bookViewModel.GenreIds.Select(gid => new BookGenre
                {
                    BookId = bookViewModel.Id,
                    GenreId = gid
                }).ToList();

                try
                {
                    // Save changes
                    _context.Update(bookDataModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(bookViewModel.Id))
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
            
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "Name", bookViewModel.AuthorId);
            ViewData["Genres"] = new SelectList(_context.Genre, "Id", "Description", bookViewModel.GenreIds);
            return View(bookViewModel);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.ApplicationUser)
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
