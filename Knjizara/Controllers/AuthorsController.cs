using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Knjizara.Data;
using Knjizara.Models.BaseEntities;
using Knjizara.Models.Books;
using Microsoft.AspNetCore.Authorization;

namespace Knjizara.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Authorss
        public async Task<IActionResult> Index()
        {
              return _context.Authors != null ? 
                          View(await _context.Authors.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Authors'  is null.");
        }

        /* 
        private async Task<Author> GetAuthorAsync(int id)
        {
             if (id == null || _context.Authors == null)
              {
                  return null;
              }

              var author = await _context.Authors
                  .FirstOrDefaultAsync(m => m.Id == id);
              author.Books = new List<Book>();
              // author.Books = await _context.Books.FirstOrDefaultAsync(t => author.Id == t.AuthorId);


              if (author == null)
              {
                  return null;
              }
              var bookQuery = author.Books.Where(x => x.AuthorId == author.Id);


              author.Books = bookQuery.ToList();
              return author;


          }  */

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            author.Books = new List<Book>();

            if (author == null)
            {
                return NotFound();
            }
            var bookQuery = _context.Books.Where(x => x.AuthorId==author.Id);


            author.Books = bookQuery.ToList();

            return View(author);
        }



        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id))
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
            return View(author);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Authors == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Authors'  is null.");
            }
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
          return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
