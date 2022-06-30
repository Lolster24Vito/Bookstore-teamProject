using Knjizara.Models.Authentication;
using Knjizara.Models.Books;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knjizara.Models.Transactions
{
    public class UserBookItem
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        [ForeignKey("BookId")]
        public Book  Book { get; set; }
    }
}
