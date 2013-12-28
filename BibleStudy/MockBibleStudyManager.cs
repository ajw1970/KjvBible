using BibleModel;
using KjvBible;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibleStudy
{
    public class MockBibleStudyManager : BibleStudyManager
    {
        private BibleReader reader;

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
            reader = new BibleReader(books, new ReadingListData());
            reader.AddReadingList("Gen", "Deut", "Ex", 11);
            reader.AddReadingList("Joshua", "2 Chron", "Ruth", 1);
            reader.AddReadingList("Ezra", "Job", "Ezra", 3);
            reader.AddReadingList("Psalm", 47);
            reader.AddReadingList("Prov", "Song", "Prov", 25);
            reader.AddReadingList("Isaiah", "Daniel", "Jer", 9);
            reader.AddReadingList("Hosea", "Malachi", "Micah", 9);
            reader.AddReadingList("Matt", "John", "Matt", 7);
            reader.AddReadingList("Acts", "2 Cor", "1 Cor", 8);
            reader.AddReadingList("Gal", "Rev", "Titus", 3);
            reader.SetCurrentListIndex(1);
        }

        public override ReadingChapter CurrentChapter
        {
            get
            {
                return ConvertHeaderToChapter(reader.CurrentChapterHeader);
            }
        }

        public override ReadingChapter GetNextChapter()
        {
            return ConvertHeaderToChapter(reader.NextChapterHeader);
        }
    }
}
