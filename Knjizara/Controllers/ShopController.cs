using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Knjizara.Data;
using Knjizara.Models.Books;
using Knjizara.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Knjizara.Models.Transactions;

namespace Knjizara.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ShopController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Shop

        public async Task<IActionResult> IndexAsync(string? searchString)
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


        // GET: Shop/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }


        // GET: Authors/Details/5
        public async Task<IActionResult> AuthorDetails(int? id)
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
            var bookQuery = _context.Books.Where(x => x.AuthorId == author.Id);


            author.Books = bookQuery.ToList();

            return View(author);
        }



        [Authorize]
        public async Task<IActionResult> Buy(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.Include(_x => _x.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            var currentUser = await _userManager.GetUserAsync(User);
            if (book == null)
            {
                return NotFound();
            }


            if (book != null && currentUser != null)
            {
                //ADD TRANSACTION TO DATABASE
                _context.BookUserBuyTransaction.Add(new BookUserBuy { User = currentUser, Book = book, CreatedAt = DateTime.Now });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Buy(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {

            }

            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [Authorize]
        public async Task<IActionResult> Borrow(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.Include(_x => _x.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            var currentUser = await _userManager.GetUserAsync(User);

            if (book == null)
            {
                return NotFound();
            }
            if (book != null && currentUser != null)
            {
                //ADD TRANSACTION TO DATABASE
                _context.BookUserBorrowTransaction.Add(new BookUserBorrow { User = currentUser, Book = book, CreatedAt = DateTime.Now });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Borrow(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {

            }

            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
