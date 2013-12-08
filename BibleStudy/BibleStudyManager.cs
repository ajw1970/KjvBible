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
    public class BibleStudyManager
    {
        private Binder bible;
        private List<Book> books;
        private Reader reader;
        private static BibleStudyManager bibleStudyManager;

        public static BibleStudyManager Instance
        {
            get
            {
                if (bibleStudyManager == null)
                    bibleStudyManager = new BibleStudyManager();
                return bibleStudyManager;
            }
        }

        private BibleStudyManager()
        {
            bible = Service.GetBible();
            books = new List<Book>(bible.BookGroups[0].Books);
            books.AddRange(bible.BookGroups[2].Books);
            reader = new Reader(books);

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
                return reader.CurrentChapter;
            }
        }

        public ReadingChapter NextChapter
        {
            get
            {
                return reader.NextChapter;
            }
        }
    }
}
