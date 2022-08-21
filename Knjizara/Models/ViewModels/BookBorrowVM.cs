using Knjizara.Models.Books;

namespace Knjizara.Models.ViewModels
{
    public class BookBorrowVM
    {
        public Book Book { get; set; }
        public int WeeksToBorrow { get; set; }
    }
}
