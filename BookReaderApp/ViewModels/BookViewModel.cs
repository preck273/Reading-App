using BookReaderApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReaderApp.ViewModels
{
    internal class BookViewModel
    {
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\g3k01\source\repos\BookReaderApp\BookReader.mdf;Trusted_Connection=true;encrypt=false";

        public static bool AddBook(BookModel book)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Book (Bid, Image, Title, Content) VALUES (@Bid, @Image, @Title, @Content)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@Bid", book.Bid);
                        command.Parameters.AddWithValue("@Image", book.Image);
                        command.Parameters.AddWithValue("@Title", book.Title);
                        command.Parameters.AddWithValue("@Content", book.Content);

                        connection.Open();

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding book: {ex.Message}");
                return false;
            }
        }

        public static List<BookModel> GetAllBooks()
        {
            List<BookModel> books = new List<BookModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT Bid, Image, Title, Content FROM Book";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BookModel book = new BookModel
                                {
                                    Bid = reader.GetInt32(reader.GetOrdinal("Bid")),
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    Content = reader.GetString(reader.GetOrdinal("Content"))
                                };

                                byte[] imageData = (byte[])reader["Image"];
                                book.Image = imageData;

                                books.Add(book);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting books: {ex.Message}");
            }

            return books;
        }
    }
}

