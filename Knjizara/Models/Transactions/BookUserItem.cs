using Knjizara.Models.Authentication;
using Knjizara.Models.Books;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knjizara.Models.Transactions
{
    public class BookUserItem
    {
        [Key]
        public int Id { get; set; }

        
        public Book Book { get; set; }
        public AppUser User { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
