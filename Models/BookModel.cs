using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReaderApp.Models
{
	public class BookModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public byte[] CoverImage { get; set; }
		public string Author { get; set; }
		public string Description { get; set; }
		//public List<BookImageModel> BookImages { get; set; }
	}
}
