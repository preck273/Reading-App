using BookReaderApp.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReaderApp.HelperClass
{
	public class RetriveBookHelper
	{
		//private readonly string _connectionString = @"Data Source=DESKTOP-MUE5L5M\SQLEXPRESS06;Initial Catalog=BookReader;Integrated Security=True;encrypt=false";

		public static List<BookModel> GetBooks()
		{
			List<BookModel> books = new List<BookModel>();

			string selectQuery = "SELECT Id, Title, Description, CoverImage FROM BookDetails";

			using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-MUE5L5M\SQLEXPRESS06;Initial Catalog=BookReader;Integrated Security=True;encrypt=false"))
			{
				connection.Open();

				using (SqlCommand command = new SqlCommand(selectQuery, connection))
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							BookModel book = new BookModel
							{
								Id = reader.GetInt32(reader.GetOrdinal("Id")),
								Title = reader.GetString(reader.GetOrdinal("Title")),
								Description = reader.GetString(reader.GetOrdinal("Description")),
								CoverImage = (byte[])reader["CoverImage"] 
							};

							books.Add(book);
						}
					}
				}
			}

			return books;
		}

	}
}