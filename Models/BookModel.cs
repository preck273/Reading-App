using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookReaderApp.Models
{
	public class BookModel
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		public string Title { get; set; }
		public string Category { get; set; }
		public DateTime PublishedDate { get; set; }
		public string Description { get; set; }
		public string PdfFile { get; set; }
		public string Author { get; set; }
		public string PdfImage { get; set; }

	}
}
