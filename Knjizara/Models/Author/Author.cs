using System.ComponentModel.DataAnnotations;

namespace Knjizara.Models.BaseEntities
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Ime autora")]
        [StringLength(450)]
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }

        public Author() { }
        public Author(String name)
        {
            Name = name;
        }

    }
}
