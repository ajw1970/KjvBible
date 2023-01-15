using System.Collections.Generic;

namespace BibleModel
{
    public class BookData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AbbreviatedName { get; set; }
        public int ChapterCount { get; set; }
        public int BookVerseCount { get; set; }
        public List<int> ChapterVerseCounts { get; set; }
    }
}