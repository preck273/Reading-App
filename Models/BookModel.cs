using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReaderApp.Models
{
    public class BookModel
    {
        public int Bid { get; set; }
        public byte[] Image { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
