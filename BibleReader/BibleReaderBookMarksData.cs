using System.Collections.Generic;
using System.Linq;

namespace BibleStudy
{
    public class BibleReaderBookMarksData
    {
        public string CurrentBookMark { get; }
        public IEnumerable<BibleReaderBookMarkData> BookMarks { get; }

        public BibleReaderBookMarksData()
        {
            CurrentBookMark = null;
            BookMarks = new List<BibleReaderBookMarkData>();
        }

        public BibleReaderBookMarksData(string range, string current)
        {
            BookMarks = new List<BibleReaderBookMarkData>()
            {
                new BibleReaderBookMarkData(range, current)
            };
            CurrentBookMark = range;
        }

        public BibleReaderBookMarksData(string currentBookMark, IEnumerable<BibleReaderBookMarkData> bookMarks)
        {
            CurrentBookMark = currentBookMark;
            BookMarks = bookMarks;
        }

        public BibleReaderBookMarksData AddBookMark(string name, string position)
        {
            if (string.IsNullOrWhiteSpace(name)) return this;
            if (string.IsNullOrWhiteSpace(position)) return this;

            var bookMarks = BookMarks.ToList();
            bookMarks.Add(new BibleReaderBookMarkData(name, position));

            var currentBookMark = CurrentBookMark ?? name;
            
            return new BibleReaderBookMarksData(currentBookMark, bookMarks);
        }

        public BibleReaderBookMarksData AddBookMark(BibleReaderBookMarkData bookMark)
        {
            return bookMark == null ? this : AddBookMark(bookMark.Range, bookMark.Current);
        }

        public BibleReaderBookMarksData AddBookMark(string bookName, int currentChapter)
        {
            if (string.IsNullOrWhiteSpace(bookName)) return this;
            if (currentChapter <= 0) return this;

            return AddBookMark(bookName, $"{bookName} {currentChapter}");
        }
    }
}