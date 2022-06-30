using JsonParseToDatabase.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonParseToDatabase.Dal
{
    public class JsonManager
    {
        public IList<Book> GetDeserializedBooksData(string booksRawData)
        {
            return JsonConvert.DeserializeObject<IList<Book>>(booksRawData);
        }

        //MALE
        public string GetRawBooksData()
        {

            string jsonClient = JsonConstants.BOOKS;
            string jsonString = File.ReadAllText(jsonClient);
            return jsonString;

        }

        public IList<Book> GetAllBooksData()
        {


            IList<Book> books = new List<Book>();
            var booksRawData = GetRawBooksData();
            var booksData = GetDeserializedBooksData(booksRawData);

            foreach (var book in booksData)
            {
                books.Add(book);
            }



            return books;

        }

    }
}
