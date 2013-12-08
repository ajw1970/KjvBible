using BibleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader
{
    public class ReadingChapterHeader
    {
        public string BookName { get; set; }
        public int Number { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1}", BookName, Number);
        }
    }

    public class ReadingChapter : ReadingChapterHeader
    {
        public List<ReadingVerse> Verses { get; set; }
    }
}
