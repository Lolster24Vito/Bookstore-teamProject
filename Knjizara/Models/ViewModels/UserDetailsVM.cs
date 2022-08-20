using Knjizara.Models.Authentication;
using Knjizara.Models.Books;
using Knjizara.Models.Transactions;

namespace Knjizara.Models.ViewModels
{
    public class UserDetailsVM
    {
        public AppUser? User { get; set; }
        public IList<BookUserBorrow>? BorrowedBooks { get; set; }
        public IList<BookUserBuy>? PurchasedBooks { get; set; }
    }
}
