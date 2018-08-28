using System.Collections.Generic;
using System.Linq;

namespace BibleStudy
{
    public class BibleReaderBookMarksData
    {
        public string CurrentName { get; }
        public IEnumerable<BibleReaderBookMarkData> BookMarks { get; }

        public BibleReaderBookMarksData()
        {
            CurrentName = null;
            BookMarks = new List<BibleReaderBookMarkData>();
        }

        public BibleReaderBookMarksData(string range, string current)
        {
            BookMarks = new List<BibleReaderBookMarkData>()
            {
                new BibleReaderBookMarkData(range, current)
            };
            CurrentName = range;
        }

        public BibleReaderBookMarksData(string currentName, IEnumerable<BibleReaderBookMarkData> bookMarks)
        {
            CurrentName = currentName;
            BookMarks = bookMarks;
        }

        public BibleReaderBookMarksData AddBookMark(string name, string position)
        {
            if (string.IsNullOrWhiteSpace(name)) return this;
            if (string.IsNullOrWhiteSpace(position)) return this;

            var bookMarks = BookMarks.ToList();
            bookMarks.Add(new BibleReaderBookMarkData(name, position));

            var currentBookMark = CurrentName ?? name;
            
            return new BibleReaderBookMarksData(currentBookMark, bookMarks);
        }

        public BibleReaderBookMarksData AddBookMark(BibleReaderBookMarkData bookMark)
        {
            return bookMark == null ? this : AddBookMark(bookMark.Name, bookMark.Position);
        }

        public BibleReaderBookMarksData AddBookMark(string bookName, int currentChapter)
        {
            if (string.IsNullOrWhiteSpace(bookName)) return this;
            if (currentChapter <= 0) return this;

            return AddBookMark(bookName, $"{bookName} {currentChapter}");
        }
    }
}