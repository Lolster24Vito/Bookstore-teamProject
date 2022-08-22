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
using Knjizara.Models.ViewModels;
using Newtonsoft.Json;
using Knjizara.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace Knjizara.Controllers
{
    [Authorize(Roles = "Admin")]

    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
            
        }

        //GET: Books
        public async Task<IActionResult> Index(string? searchString)
        {

            if (String.IsNullOrEmpty(searchString))
            {
                var applicationDbContext = _context.Books.Include(b => b.Author);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.Books.Include(b => b.Author).Where(b => b.Title.Contains(searchString) || b.Author.Name.Contains(searchString));
                return View(await applicationDbContext.ToListAsync());
            }
        }
        //GET: Deleted Books
        public async Task<IActionResult> DeletedBooks(string? searchString)
        {
            
            if (String.IsNullOrEmpty(searchString))
            {
                var deletedBooks = _context.Books.FromSqlRaw("SELECT * FROM DeletedBooks");
                return View(await deletedBooks.ToListAsync());
            }
            else
            {
                var deletedBooks = _context.Books.FromSqlRaw("SELECT * FROM DeletedBooks").Include(b => b.Author).Where(b => b.Title.Contains(searchString) || b.Author.Name.Contains(searchString));
                return View(await deletedBooks.ToListAsync());
            }
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
            var author = await _context.Authors.FindAsync(book.AuthorId);
            if (author == null)
            {
                return NotFound();
            }

            return View(new CollectionBookAuthor(book,author));
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



                SqlConnection con = new SqlConnection("Server = (localdb)\\MSSQLLocalDB; Database = aspnet-Knjizara-A684A6DA-A4A6-4CD9-93B4-2142EB287C06; Trusted_Connection = True; MultipleActiveResultSets = true");
                
                SqlCommand cmd = new SqlCommand("delete_Books", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", book.Id);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
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
