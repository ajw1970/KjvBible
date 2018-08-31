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
        public string Name { get; set; }
        public string Position { get; set; }

        //public BibleReaderBookMarkData(string name, string position)
        //{
        //    Name = name;
        //    Position = position;
        //}

        //public BibleReaderBookMarkData(string firstBookname, string lastBookname, string currentBookname, int currentChapterNumber)
        //{
        //    if (string.IsNullOrEmpty(firstBookname)) throw new ArgumentException(nameof(firstBookname));
        //    if (string.IsNullOrEmpty(lastBookname)) throw new ArgumentException(nameof(lastBookname));
        //    if (string.IsNullOrEmpty(currentBookname)) throw new ArgumentException(nameof(currentBookname));
        //    if (currentChapterNumber <= 0) throw new ArgumentException(nameof(currentChapterNumber));

        //    Name = $"{firstBookname}-{lastBookname}";
        //    Position = $"{currentBookname} {currentChapterNumber}";
        //}

        //public BibleReaderBookMarkData(string bookName, int chapterNumber)
        //{
        //    if (string.IsNullOrEmpty(bookName)) throw new ArgumentException(nameof(bookName));
        //    if (chapterNumber <= 0) throw new ArgumentException(nameof(chapterNumber));

        //    Name = bookName;
        //    Position = $"{bookName} {chapterNumber}";
        //}
    }
}
