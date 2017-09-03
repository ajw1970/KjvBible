using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudy
{
    public class BibleReaderBookMarkData
    {
        public string Range { get; set; }
        public string Current { get; set; }

        public BibleReaderBookMarkData(string range, string current)
        {
            Range = range;
            Current = current;
        }
    }
}
