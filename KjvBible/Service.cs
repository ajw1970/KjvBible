﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BibleModel;
using System.IO;

namespace KjvBible
{
    public static class Service
    {
        private static Binder binder;

        public static Binder GetBible()
        {
            if (binder == null)
                binder = Osis.Service.GetBible();

            return binder;
        }

        public static Binder GetBible(string osisXml)
        {
            if (binder == null)
                binder = Osis.Service.GetBible(osisXml);

            return binder;
        }

        private static List<BookData> GetCannonizedBookData()
        {
            if (binder == null)
                binder = GetBible();

            return GetCannonizedBookData(binder);
        }

        public static List<BookData> GetCannonizedBookData(this Binder binder)
        {
            var books = (getBooks("Old Testament").Union(getBooks("New Testament"))).ToList();
            return getBookData(books);
        }

        private static List<Book> getBooks(string collectionName)
        {
            return (from c in binder.BookGroups
                    where c.Name == collectionName
                    from b in c.Books
                    select b).ToList();
        }

        private static List<BookData> getBookData(List<Book> books, int startingId = 1)
        {
            return (from b in books
                    select new BookData
                    {
                        Id = startingId++,
                        Name = b.Name,
                        AbbreviatedName = b.AbbreviatedName,
                        ChapterCount = b.Chapters.Count(),
                        VerseCount = b.Chapters.Sum(c2 => c2.Verses.Count),
                    }).ToList();
        }
    }
}
