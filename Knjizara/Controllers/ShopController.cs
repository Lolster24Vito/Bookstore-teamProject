using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Knjizara.Data;
using Knjizara.Models.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Knjizara.Models.Authentication;

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
                var applicationDbContext = _context.Books.Include(b => b.Author).Where(b=>b.Title.Contains(searchString));
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




        public async Task<IActionResult> Buy(int? id)
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Buy(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Books'  is null.");
            }
            var book = await _context.Books.Include(b => b.UsersPurchased)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            if (book != null)
            {
                //ADD BOOK TO USER,AND USER TO THE BOOK
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return Challenge();
                }
                var userWithContent = _context.Users.Include(c => c.PurchasedBooks).FirstOrDefault(u => currentUser.Id == u.Id);

                if (userWithContent.PurchasedBooks == null) userWithContent.PurchasedBooks = new List<Book>();

                userWithContent.PurchasedBooks.Add(book);

                if (book.UsersPurchased == null) book.UsersPurchased = new List<AppUser>();
                book.UsersPurchased.Add(currentUser);

                _context.Update(userWithContent);
                _context.Update(book);

                await _context.SaveChangesAsync();
            }

            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Borrow(int? id)
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
                //Borrow?
            }

            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
