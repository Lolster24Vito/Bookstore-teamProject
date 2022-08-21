using Knjizara.Models.Authentication;
using Knjizara.Models.Books;
using System.ComponentModel.DataAnnotations;

namespace Knjizara.Models.Transactions
{
    //ef wont create two  tables from the same class.That is why these classes are empty

    public class BookUserBorrow
    {
        [Key]
        public int Id { get; set; }

        public Book Book { get; set; }
        public AppUser User { get; set; }
        [Display(Name = "Posuđeno datuma:")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Treba vratiti datuma:")]

        public DateTime? ReturnOnDate { get; set; }

        public bool IsReturned { get; set; }
    }
}
