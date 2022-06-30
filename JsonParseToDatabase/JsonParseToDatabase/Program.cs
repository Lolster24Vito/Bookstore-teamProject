using JsonParseToDatabase.Dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonParseToDatabase
{
    public class Program
    {
        static string connectionString = "Server = (localdb)\\mssqllocaldb; Database = aspnet-Knjizara-A684A6DA-A4A6-4CD9-93B4-2142EB287C06; Trusted_Connection = True; MultipleActiveResultSets = true";

        static void DbInsert(string upit)
        {

           
                using (DbConnection oConnection = new SqlConnection(connectionString))
                using (DbCommand oCommand = oConnection.CreateCommand())
                {
                    oCommand.CommandText = upit;
                    oConnection.Open();
                    using (DbDataReader oReader = oCommand.ExecuteReader())
                    {
                    }
                }
            
        }

        static bool CheckExistingTable(string upit)
        {

            
            
            using (DbConnection oConnection = new SqlConnection(connectionString))
            using (DbCommand oCommand = oConnection.CreateCommand())
            {

                oCommand.CommandText = upit;
                oConnection.Open();
                using (DbDataReader oReader = oCommand.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        if (oReader["Id"] != null)
                        {
                            return true;
                        }
                    }
                    
                }
            }
            return false;
        }
        static bool CheckTitleNameTable(string upit)
        {



            using (DbConnection oConnection = new SqlConnection(connectionString))
            using (DbCommand oCommand = oConnection.CreateCommand())
            {

                oCommand.CommandText = upit;
                oConnection.Open();
                using (DbDataReader oReader = oCommand.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        if (oReader["Title"] != null)
                        {
                            return true;
                        }
                    }

                }
            }
            return false;
        }
        static int GetId(string upit)
        {



            using (DbConnection oConnection = new SqlConnection(connectionString))
            using (DbCommand oCommand = oConnection.CreateCommand())
            {

                oCommand.CommandText = upit;
                oConnection.Open();
                using (DbDataReader oReader = oCommand.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        
                            return (int)oReader["Id"];
                        
                    }

                }
            }
            return 0;
        }


        static void Main(string[] args)
        {
            JsonManager json = new JsonManager();
            var data = json.GetAllBooksData();

            IList<Book> books = new List<Book>();

            foreach (var book in data)
            {
                books.Add(book);
                
            }
          
           


            IList<string> autori = new List<string>();
            foreach (var item in books)
            {
                autori.Add(item.Author);
            }

            autori = autori.Distinct().ToList();






            foreach (var item in autori)
            {

                if (!CheckExistingTable("select * from Authors where Name ='" + item + "'"))
                {

                    DbInsert("INSERT INTO Authors (Name) VALUES ('" + item + "')");

                }
               

            }

            foreach (var item in books)
            {
                string stocks = item.StockAvailabilty.Substring(10, 2);


                int stock = Convert.ToInt32(stocks);

                int id = GetId("select Id from authors");

                double priceforborrowing = item.Price - 5;
                double priceforbuying = item.Price;

                if (!CheckTitleNameTable("select * from Books where Title ='" + item.Title + "'"))
                {

                    DbInsert("INSERT INTO Books (Title,AuthorId,Isbn,PriceForBorrowing,PriceForBuying,StockAvailabilty,CoverURL) VALUES (" + "'" + item.Title + "'" + "," + id + "," + "'" + item.Isbn + "'" + "," + priceforborrowing + "," + priceforbuying + "," + stock + "," + "'" + item.Cover + "'" + ")");

                }
               

               

               
            }




        }
    }

}
