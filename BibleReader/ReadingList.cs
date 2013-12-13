using BibleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudy
{
    public class ReadingLists
    {
        public int CurrentIndex;
        public List<ReadingList> Lists;
        public int Count
        {
            get
            {
                return Lists.Count;
            }
        }
        public void AddList(ReadingList list)
        {
            Lists.Add(list);
        }

        public ReadingLists()
        {
            Lists = new List<ReadingList>();
        }
    }

    public class ReadingList
    {
        public string Name { get; set; }
        public List<ReadingChapterHeader> ReadingChapters { get; set; }
        public int currentIndex { get; set; }
    }


}
