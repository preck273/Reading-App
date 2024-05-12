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
		private readonly IMongoCollection<BookModel> booksCollection;


		public BookCollection()
		{
			var client = new MongoClient("mongodb://localhost:27017/");
			var database = client.GetDatabase("ReadingBook");
			 this.booksCollection = database.GetCollection<BookModel>("Book");
		}

		public async Task<List<BookModel>> GetBooksAsync()
		{
			// Fetch book data from MongoDB
			return await booksCollection.Find(_ => true).ToListAsync();
		}
	}
}
