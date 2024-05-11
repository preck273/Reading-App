using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReaderApp.MongoDb
{
	public class CreateDb
	{
		private IMongoDatabase database;

		public CreateDb(string db)
		{

			var client = new MongoClient();
			database = client.GetDatabase(db);
			

			//uncomment to create the collection book if not already created.
			//database.CreateCollection("Book");

		}
	}
}
