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

namespace Knjizara.Areas.Identity.Pages.Account.Manage
{
    public class BooksListModel : PageModel
    {
        private readonly Knjizara.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;


        public BooksListModel(Knjizara.Data.ApplicationDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Book> BorrowedBooks { get;set; } = default!;
        public IList<Book> PurchasedBooks { get; set; } = default!;


        public async Task OnGetAsync()
        {

            var currentUser = await _userManager.GetUserAsync(User);

            if (_context.Books != null)
            {
                var userWithContent = _context.Users.Include(c => c.PurchasedBooks).FirstOrDefault(u => currentUser.Id == u.Id);
                BorrowedBooks = userWithContent.PurchasedBooks;
                PurchasedBooks = userWithContent.BorrowedBooks;

            }
        }
    }
}
