using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Knjizara.Data;
using Knjizara.Models.Authentication;
using Knjizara.Models.ViewModels;
using Knjizara.Models.Transactions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Knjizara.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AppUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<AppRole> _roleManager;


        public AppUsersController(ApplicationDbContext context, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;

        }

        // GET: AppUsers
        public async Task<IActionResult> Index(string? searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                var applicationDbContext = _context.Users;
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.Users.Where(u => u.FirstName.Contains(searchString) || u.LastName.Contains(searchString));
                return View(await applicationDbContext.ToListAsync());
            }
        }

        // GET: AppUsers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id is null || _context.Users is null)
            {
                return NotFound();
            }

            AppUser? appUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            IList<BookUserBorrow> borrowedBooks = _context.BookUserBorrowTransaction
                .Include(bu => bu.Book)
                .Include(bu => bu.Book.Author)
                .Where(b => b.User.Id == appUser.Id).ToList();

            IList<BookUserBuy> purchasedBooks = _context.BookUserBuyTransaction
                .Include(bu => bu.Book)
                .Include(bu => bu.Book.Author)
                .Where(b => b.User.Id == appUser.Id).ToList();

            if (appUser is null)
            {
                return NotFound();
            }

            UserDetailsVM userDetailsVM = new()
            {
                User = appUser,
                BorrowedBooks = borrowedBooks,
                PurchasedBooks = purchasedBooks
            };

            return View(userDetailsVM);
        }

        public async Task<IActionResult> ReturnBookAsync(int id)
        {
            BookUserBorrow? borrowedBook = _context.BookUserBorrowTransaction
                .Include(bu => bu.User)
                .FirstOrDefault(b => b.Book.Id == id);
            if (borrowedBook is null)
            {
                return NotFound();
            }
            _context.BookUserBorrowTransaction.Remove(borrowedBook);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = borrowedBook.User.Id });
        }

        // GET: AppUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Address,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                appUser.Id = Guid.NewGuid();
                _context.Add(appUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appUser);
        }

        // GET: AppUsers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var appUser = await _context.Users.FindAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }
            return View(appUser);
        }

        // POST: AppUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("FirstName,LastName,Address,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] AppUser appUser)
        {
            if (id != appUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppUserExists(appUser.Id))
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
            return View(appUser);
        }

        // GET: AppUsers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var appUser = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // POST: AppUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Users'  is null.");
            }
            var appUser = await _context.Users.FindAsync(id);
            if (appUser != null)
            {
                _context.Users.Remove(appUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppUserExists(Guid id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
