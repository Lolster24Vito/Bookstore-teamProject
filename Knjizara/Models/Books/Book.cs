using Knjizara.Models.BaseEntities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knjizara.Models.Books
{
    public class Book
    {
        [Key]
        [Display(Name = "Id")]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        public int Id { get; set; }


        [Display(Name = "Naslov")]
        [StringLength(1000)]
        [Required(ErrorMessage = "Ovo polje je obavezno")]
        [JsonProperty("TITLE")]
        public string Title { get; set; }


      

        [Display(Name = "ISBN")]
        [StringLength(maximumLength:15, MinimumLength = 10)]

        [JsonProperty("ISBN")]
        public string Isbn { get; set; }

        [JsonProperty("PRICE")]
        public Decimal PriceForBorrowing { get; set; }
        public Decimal PriceForBuying { get; set; }

        [JsonProperty("STOCK AVAILABILTY")]
        public int StockAvailabilty { get; set; }

        [JsonProperty("COVER")]
        public string? CoverURL { get; set; }

        [JsonProperty("DESCRIPTION")]
        [StringLength(5000)]

        public string? LongDescription { get; set; }
        [StringLength(5000)]

        public string? ShortDescription { get; set; }

        //navigation properties
        [ForeignKey("AuthorId")]
        public int AuthorId { get; set; }

        [Display(Name = "Autor")]

        [JsonProperty("AUTHOR")]
        public Author? Author { get; set; }
    }
}
