using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BibleModel
{
    public class Book
    {
        public string Name { get; set; }
        public string LongName { get; set; }
        public string AbbreviatedName { get; set; }
        public List<Chapter> Chapters { get; set; }
        public BookGroup Parent { get; set;  }

        public Book()
        {
            Chapters = new List<Chapter>();
        }
    }
}