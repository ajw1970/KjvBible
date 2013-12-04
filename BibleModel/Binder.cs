using System;
using System.Collections.Generic;
using System.Linq;

namespace BibleModel
{
    public class Binder
    {
        public List<BookGroup> BookGroups { get; set; }

        public Binder()
        {
            BookGroups = new List<BookGroup>();
        }

        public List<Book> Books
        {
            get
            {
                var books = new List<Book>();
                foreach (var bookGroup in BookGroups)
                {
                    foreach (var book in bookGroup.Books)
                    {
                        books.Add(book);
                    }
                }
                return books;
            }
        }
    }
}
