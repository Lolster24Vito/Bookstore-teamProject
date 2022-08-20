using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Knjizara.Data;
using Knjizara.Models.Books;
using Knjizara.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Knjizara.Models.Transactions;

namespace Knjizara.Areas.Identity.Pages.Account
{
    public class UserBookListModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserBookListModel(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<BookUserBorrow> BorrowedBooks { get; set; } = default!;
        public IList<BookUserBuy> PurchasedBooks { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Books != null)
            {
                var currentUser = await _userManager.GetUserAsync(User);

                BorrowedBooks = _context.BookUserBorrowTransaction
                   .Include(bu => bu.Book)
                   .Include(bu => bu.Book.Author)
                   .Where(bu => bu.User == currentUser).ToList();
                PurchasedBooks = _context.BookUserBuyTransaction
                   .Include(bu => bu.Book)
                   .Include(bu => bu.Book.Author)
                   .Where(bu => bu.User == currentUser).ToList();
            }
        }
    }
}
