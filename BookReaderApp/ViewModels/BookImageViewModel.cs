using BookReaderApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReaderApp.ViewModels
{
    internal class BookImageViewModel
    {

        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\g3k01\source\repos\BookReaderApp\BookReader.mdf;Trusted_Connection=true;encrypt=false";
       
        public static bool AddImage(BookImageModel image)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO  BookImage (Bid, Image) VALUES (@Bid, @Image)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@Bid", image.Bid);
                        command.Parameters.AddWithValue("@Image", image.Image);

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

        public static List<BookImageModel> GetAllImages(int id)
        {
            List<BookImageModel> images = new List<BookImageModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT Bid, Image FROM BookImage WHERE Bid = @Bid";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Bid", id);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BookImageModel image = new BookImageModel
                                {
                                    Bid = reader.GetInt32(reader.GetOrdinal("Bid"))
                                };

                                byte[] imageData = (byte[])reader["Image"];
                                image.Image = imageData;

                                images.Add(image);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting images: {ex.Message}");
            }

            return images;
        }


    }
}
