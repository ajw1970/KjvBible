using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibleModel;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace BibleStudy.Tests
{
    public class BibleReaderInterractionTests
    {
        private readonly List<BookData> _books;

        public BibleReaderInterractionTests()
        {
            using (var stream = new StreamReader(@"Data\BookDataList.json"))
            {
                var booksJson = stream.ReadLine();
                _books = JsonConvert.DeserializeObject<List<BookData>>(booksJson);
            }
        }

        [Fact]
        public void CanGetCurrentReadingList()
        {
            TestReader.CurrentChapterHeader.ToString().Should().Be("Exodus 38");
            TestReader.NextChapterHeader.ToString().Should().Be("1 Samuel 25");
        }

        private BibleReader TestReader
        {
            get
            {
                var data = new BibleReaderBookMarksData();
                var testReader = new BibleReader(Books, data);

                testReader.AddReadingList("Genesis-Deuteronomy", "Exodus 38");
                testReader.AddReadingList("Joshua-2 Chronicles", "1 Samuel 25");
                testReader.AddReadingList("Ezra-Jacob", "Esther 9");
                testReader.AddReadingList("Psalm", "Psalm 81");
                testReader.AddReadingList("Proverbs-Song of Solomon", "Proverbs 4");
                testReader.AddReadingList("Isaiah-Daniel", "Jeremiah 37");
                testReader.AddReadingList("Hosea-Malachi", "Zechariah 11");
                testReader.AddReadingList("Matthew-John", "Mark 7");
                testReader.AddReadingList("Acts-2 Corinthians", "Acts 7");
                testReader.AddReadingList("Galatians-Revelation", "1 John 2");

                return testReader;
            }
        }

        private List<BookData> Books
        {
            get =>_books; 
        }
    }
}
