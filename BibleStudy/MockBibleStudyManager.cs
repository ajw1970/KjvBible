using BibleModel;
using BibleReader;
using KjvBible;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudy
{
    public class MockBibleStudyManager : BibleStudyManager
    {
        private Binder bible;
        private List<BookData> books;
        private Reader reader;
        private static BibleStudyManager bibleStudyManager;

        public static BibleStudyManager Instance
        {
            get
            {
                if (bibleStudyManager == null)
                    bibleStudyManager = new MockBibleStudyManager();
                return bibleStudyManager;
            }
        }

        private MockBibleStudyManager()
        {
            bible = Service.GetBible();
            books = Service.GetCannonizedBookData();
            reader = new Reader(books, new SaveOnlyFileAccessor(new FileAccessor()), "ajw1970");

            reader.AddReadingList("Gen", "Deut", "Ex", 7);
            reader.AddReadingList("Joshua", "2 Chron", "Judges", 19);
            reader.AddReadingList("Ezra", "Job", "Job", 42);
            reader.AddReadingList("Psalm", 44);
            reader.AddReadingList("Prov", "Song", "Prov", 22);
            reader.AddReadingList("Isaiah", "Daniel", "Jer", 6);
            reader.AddReadingList("Hosea", "Malachi", "Jon", 2);
            reader.AddReadingList("Matt", "John", "Matt", 4);
            reader.AddReadingList("Acts", "2 Cor", "1 Cor", 5);
            reader.AddReadingList("Gal", "Rev", "2 Tim", 3);
            reader.SetCurrentListIndex(8);
        }

        public ReadingChapter CurrentChapter
        {
            get
            {
                return ConvertHeaderToChapter(reader.CurrentChapterHeader);
            }
        }

        public ReadingChapter GetNextChapter()
        {
            return ConvertHeaderToChapter(reader.NextChapterHeader);
        }

        private ReadingChapter ConvertHeaderToChapter(ReadingChapterHeader header)
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
