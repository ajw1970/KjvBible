using System;
using System.Collections.Generic;
using System.Linq;

namespace BibleModel
{
    public class BookGroup
    {
        public string Name { get; set; }
        public List<Book> Books { get; set; }

        public BookGroup()
        {
            Books = new List<Book>();
        }
    }
}