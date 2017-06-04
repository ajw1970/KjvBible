using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using KjvBible;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var bible = KjvBible.Service.GetBible();

            foreach (var group in bible.BookGroups)
            {
                Console.WriteLine($"{group.Name}: {group.Books.Count()} Book(s)");
            }

            Console.WriteLine();
            foreach (var book in bible.Books)
            {
                Console.WriteLine("{0} {1} ({2} Chapters)", book.Parent.Name, book.Name, book.Chapters.Count());
            }

            //var bibleNotes = WordXml.Service.GetBibleNotes(bible);
            //foreach (var book in bibleNotes.Books)
            //{
            //    Console.WriteLine("{0} ({1} Chapters)", book.Name, book.Chapters.Count());
            //}

            Console.Read();
        }
    }
}
