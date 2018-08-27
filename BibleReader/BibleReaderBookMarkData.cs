using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibleModel;
using ScriptureReferenceParser;

namespace BibleStudy
{
    public class BibleReaderBookMarkData
    {
        public string Range { get; }
        public string Current { get; }

        public BibleReaderBookMarkData(string range, string current)
        {
            Range = range;
            Current = current;
        }

        public BibleReaderBookMarkData(string firstBookname, string lastBookname, string currentBookname, int currentChapterNumber)
        {
            if (string.IsNullOrEmpty(firstBookname)) throw new ArgumentException(nameof(firstBookname));
            if (string.IsNullOrEmpty(lastBookname)) throw new ArgumentException(nameof(lastBookname));
            if (string.IsNullOrEmpty(currentBookname)) throw new ArgumentException(nameof(currentBookname));
            if (currentChapterNumber <= 0) throw new ArgumentException(nameof(currentChapterNumber));

            Range = $"{firstBookname}-{lastBookname}";
            Current = $"{currentBookname} {currentChapterNumber}";
        }

        public BibleReaderBookMarkData(string bookName, int chapterNumber)
        {
            if (string.IsNullOrEmpty(bookName)) throw new ArgumentException(nameof(bookName));
            if (chapterNumber <= 0) throw new ArgumentException(nameof(chapterNumber));

            Range = bookName;
            Current = $"{bookName} {chapterNumber}";
        }
    }
}
