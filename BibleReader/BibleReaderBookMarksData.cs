using System.Collections.Generic;

namespace BibleStudy
{
    public class BibleReaderBookMarksData
    {
        public string CurrentBookMark { get; set; }
        public List<BibleReaderBookMarkData> BookMarks { get; set; }

        public BibleReaderBookMarksData()
        {
            BookMarks = new List<BibleReaderBookMarkData>();
        }
    }
}