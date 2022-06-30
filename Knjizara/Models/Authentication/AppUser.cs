using Knjizara.Models.Books;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Knjizara.Models.Authentication
{
    public class AppUser : IdentityUser<Guid>
    {
        [Display(Name = "Ime")]
        [StringLength(250)]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        public string? FirstName { get; set; }

        [Display(Name = "Prezime")]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        [StringLength(250)]
        public string? LastName { get; set; }

        [Display(Name = "Adresa")]
        [StringLength(350)]
        public string? Address { get; set; }

        [Display(Name ="Posuđene knjige")]
        IList<Book>? BorrowedBooks { get; set; }

        [Display(Name = "Kupljene knjige")]
        IList<Book>? PurchasedBooks { get; set; }


    }
}
