using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Knjizara.Data;
using Knjizara.Models.Books;
using Knjizara.Models.BaseEntities;

namespace Knjizara.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
              return (_context.Books != null ? 
                          View(await _context.Books.Include(x => x.Author).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Books'  is null."));
        }



        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.Include(_x => _x.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {


            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Isbn,PriceForBorrowing,PriceForBuying,StockAvailabilty,CoverURL,LongDescription,ShortDescription")] Book book,string authorName)
        {
            if (ModelState.IsValid)
            {
                //MISSING LOGIC
                //if(autor exists dont add new author to database) Add Book To authors works
                var dbAuthor = _context.Authors
                 .Where(x => x.Name == authorName)
                 .FirstOrDefault();
                if (dbAuthor != null)
                {
                    book.Author = dbAuthor;
                }
                else
                {
                    Author editedAuthor = new Author(authorName);
                    book.Author = editedAuthor;
                    _context.Authors.Add(editedAuthor);
                }

                _context.Add(book);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Isbn,PriceForBorrowing,PriceForBuying,StockAvailabilty,CoverURL,LongDescription,ShortDescription")] Book book,string authorName)
        {
            if (id != book.Id)
            {
                return NotFound();
            }
                    var dbAuthor = _context.Authors
                   .Where(x => x.Name == authorName)
                   .FirstOrDefault();
            if (dbAuthor != null)
            {
                book.Author = dbAuthor;
            }
            else
            {
                Author editedAuthor = new Author(authorName);

                _context.Add(editedAuthor);
                _context.SaveChanges();
                book.AuthorId=editedAuthor.Id;
            }
          
            if (ModelState.IsValid)
            {
                try
                {


                    if (dbAuthor != null)
                    {
                        _context.Update(book.Author);
                        _context.SaveChanges();

                    }

                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
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
            if (_context.Books == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
