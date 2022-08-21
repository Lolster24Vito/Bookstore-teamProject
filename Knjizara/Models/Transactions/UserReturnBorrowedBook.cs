using Knjizara.Models.Authentication;
using Knjizara.Models.Books;
using System.ComponentModel.DataAnnotations;
namespace Knjizara.Models.Transactions
{
    public class UserReturnBorrowedBook
    {
        [Key]
        public int Id { get; set; }

        public BookUserBorrow Borrow { get; set; }

        public DateTime ReturnedDate { get; set; }
        
    }
}
