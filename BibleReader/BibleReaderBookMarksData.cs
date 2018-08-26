using System.Collections.Generic;
using System.Linq;

namespace BibleStudy
{
    public class BibleReaderBookMarksData
    {
        public string CurrentBookMark { get; }
        public List<BibleReaderBookMarkData> BookMarks { get; }

        public BibleReaderBookMarksData(string currentBookMark, IEnumerable<BibleReaderBookMarkData> bookMarks)
        {
            CurrentBookMark = currentBookMark;
            BookMarks = bookMarks.ToList();
        }
    }
}