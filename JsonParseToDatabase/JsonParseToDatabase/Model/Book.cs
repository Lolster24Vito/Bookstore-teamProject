using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonParseToDatabase
{
    public class Book
    {
        [JsonProperty("TITLE")]
        public string Title { get; set; }

        [JsonProperty("AUTHOR")]
        public string Author { get; set; }

        [JsonProperty("ISBN")]
        public string Isbn { get; set; }

        [JsonProperty("PRICE")]
        public double Price { get; set; }

        [JsonProperty("STOCK AVAILABILTY")]
        public string StockAvailabilty { get; set; }

        [JsonProperty("COVER")]
        public string Cover { get; set; }

        [JsonProperty("DESCRIPTION")]
        public string Description { get; set; }

        public override string ToString()
        => $"{Title},\n{Author},\n{Isbn},\n{Price},\n{StockAvailabilty},\n{Cover},\n{Description}\n";
    }
}
