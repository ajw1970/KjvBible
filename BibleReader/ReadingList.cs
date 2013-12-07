using BibleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader
{
    public class ReadingList
    {
        public string Name { get; set; }
        public List<ReadingChapterHeader> ReadingChapters { get; set; }
        public int currentIndex { get; set; }
    }
}
