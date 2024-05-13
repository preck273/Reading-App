using BookReaderApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;


namespace BookReaderApp.NewFolder
{
	public class BookUploadHelper
	{
		private readonly string _connectionString = @"Data Source=DESKTOP-MUE5L5M\SQLEXPRESS06;Initial Catalog=BookReader;Integrated Security=True;encrypt=false";

		public BookUploadHelper()
		{
		}

		public void UploadBook(BookModel book, BookImageModel bookImage)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				using (var command = new SqlCommand("INSERT INTO BookDetails (Title, CoverImage, Author, Description) VALUES (@Title, @CoverImage, @Author, @Description); SELECT SCOPE_IDENTITY();", connection))
				{
					command.Parameters.AddWithValue("@Title", book.Title);
					command.Parameters.AddWithValue("@CoverImage", book.CoverImage);
					command.Parameters.AddWithValue("@Author", book.Author);
					command.Parameters.AddWithValue("@Description", book.Description);

					int bookDetailsId = Convert.ToInt32(command.ExecuteScalar());

					// Insert BookImages with the associated BookDetailsId
					using (var imageCommand = new SqlCommand("INSERT INTO BookImage (BookDetailsId, BookImage) VALUES (@BookDetailsId, @BookImage)", connection))
					{
						imageCommand.Parameters.AddWithValue("@BookDetailsId", bookDetailsId);
						imageCommand.Parameters.AddWithValue("@BookImage", bookImage.BookImage);

						imageCommand.ExecuteNonQuery();
					}
				}


			}
		}
	}
}
	



	


