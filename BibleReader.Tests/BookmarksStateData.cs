using System.Collections.Generic;

namespace BibleStudy.Tests
{
    public partial class BibleReaderInterractionTests
    {
        public class BookmarksStateData
        {
            public string Current { get; set; }
            public IList<BookmarkStateData> List { get; set; }
        }
    }
}
