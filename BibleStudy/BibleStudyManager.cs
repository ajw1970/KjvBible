using BibleModel;
using KjvBible;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BibleStudy
{
    public abstract class BibleStudyManager
    {
        private Binder bible;
        protected List<BookData> books;
        protected static BibleStudyManager bibleStudyManager;

        public abstract ReadingChapter GetCurrentChapter(string userId);
        public abstract ReadingChapter GetNextChapter(string userId);

        protected BibleStudyManager(string osisXml)
        {
            bible = Service.GetBible(osisXml);
            books = bible.GetCannonizedBookData();
        }

        protected BibleStudyManager()
        {
            bible = Service.GetBible();
            books = bible.GetCannonizedBookData();
        }

        protected ReadingChapter ConvertHeaderToChapter(ReadingChapterHeader header)
        {
            var verses = (from b in bible.Books
                          where b.Name == header.BookName
                          from c in b.Chapters
                          where c.Number == header.Number
                          from v in c.Verses
                          select new ReadingVerse
                          {
                              Number = v.Number,
                              Text = v.Text,
                          }).ToList();

            return new ReadingChapter
            {
                BookName = header.BookName,
                Number = header.Number,
                Verses = verses,
            };
        }
    }
}
