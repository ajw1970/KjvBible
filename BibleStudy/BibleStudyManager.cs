using BibleModel;
using KjvBible;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibleStudy
{
    public abstract class BibleStudyManager
    {
        private Binder bible;
        protected List<BookData> books;
        protected static BibleStudyManager bibleStudyManager;

        public abstract ReadingChapter GetCurrentChapter(string userName);
        public abstract ReadingChapter GetNextChapter(string userName);

        protected BibleStudyManager()
        {
            bible = Service.GetBible();
            books = Service.GetCannonizedBookData();
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
