using Knjizara.Models.BaseEntities;
using Knjizara.Models.Books;

namespace Knjizara.Models.ViewModels
{
    public class CollectionBookAuthor
    {
        public Author Author { get; set; }
        public Book Book { get; set; }


        public CollectionBookAuthor(Book book,Author author)
        {
            Book = book;  
            Author = author;
        }
    }
}
