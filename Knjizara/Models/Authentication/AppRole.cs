using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Knjizara.Models.Authentication
{
    public class AppRole: IdentityRole<Guid>
    {
        [Display(Name = "Opis")]
        [StringLength(250)]
        [Required]
        public string Description { get; set; }
    }
}
