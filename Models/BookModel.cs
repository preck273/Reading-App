using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public string Author { get; set; }
		public byte[] BookImage { get; set; } 
		public byte[] PdfFile { get; set; } 
		public string Category { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Description { get; set; }


		public ImageSource GetImageSource()
		{
			if (BookImage != null && BookImage.Length > 0)
			{
				return ImageSource.FromStream(() => new MemoryStream(BookImage));
			}
			return null;
		}
	}
}
