using BibleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleModel
{
    public class ReadingListData
    {
        public int CurrentListIndex { get; set; }
        public List<ReadingList> Lists { get; set; }

        public ReadingListData()
        {
            Lists = new List<ReadingList>();
        }
    }

    public class ReadingList
    {
        public string Name { get; set; }
        public List<ReadingChapterHeader> ReadingChapters { get; set; }
        public int CurrentChapterIndex { get; set; }
    }
}
