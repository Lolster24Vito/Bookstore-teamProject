﻿using Knjizara.Models.Books;
using Knjizara.Models.Transactions;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knjizara.Models.Authentication
{
    public class AppUser:IdentityUser<Guid>
    {
        [Display(Name ="Ime")]
        [StringLength(250)]
        [Required(ErrorMessage ="Ovo polje je obavezno")]
        public string FirstName { get; set; }
        [Display(Name = "Prezime")]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        [StringLength(250)]

        public string LastName { get; set; }
        [Display(Name = "Adresa")]
        [StringLength(350)]
        public string? Address { get; set; }

        [Display(Name = "Posuđene knjige")]
        public  IList<Book>? BorrowedBooks { get; set; }

        [Display(Name = "Kupljene knjige")]
        public  IList<Book>? PurchasedBooks { get; set; }


    }
}
