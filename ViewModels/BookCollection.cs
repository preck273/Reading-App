using BookReaderApp.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReaderApp.ViewModels
{
	public class BookCollection
	{
		private readonly IMongoCollection<BookModel> collection;


		public BookCollection()
		{
			var client = new MongoClient("mongodb://localhost:27017/");
			var database = client.GetDatabase("ReadingApp");
			var collection = database.GetCollection<BookModel>("Book");
		}

		public BookModel FetchBookDetailsFromDatabase()
		{
			try
			{
				// Query the database to retrieve a book
				return collection.Find(_ => true).FirstOrDefault();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while fetching book details: {ex.Message}");
				return null;
			}
		}

	}
}
