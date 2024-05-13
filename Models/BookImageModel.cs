using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReaderApp.Models
{
    internal class BookImageModel
    {
        public int Id { get; set; }
        public int Bid { get; set; }
        public byte[] Image { get; set; }
    }
}
