using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibleModel;
using FluentAssertions;
using Newtonsoft.Json;
using ScriptureReferenceParser;
using Xunit;

namespace BibleStudy.Tests
{
    public partial class BibleReaderInterractionTests
    {
        private List<BookData> _books;

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

        [Fact]
        public void CanManageSimpleBookmarkStateData()
        {
            var bookmarksJson = "{\"Current\":\"Genesis-Deuteronomy\",\"List\":" +
                                "[" +
                                "{\"Name\":\"Genesis-Deuteronomy\",\"Position\":\"Exodus 38\"}," +
                                "{\"Name\":\"Joshua-2 Chronicles\",\"Position\":\"1 Samuel 25\"}," +
                                "{\"Name\":\"Ezra-Jacob\",\"Position\":\"Esther 9\"}," +
                                "{\"Name\":\"Psalm\",\"Position\":\"Psalm 81\"}," +
                                "{\"Name\":\"Proverbs-Song of Solomon\",\"Position\":\"Proverbs 4\"}," +
                                "{\"Name\":\"Isaiah-Daniel\",\"Position\":\"Jeremiah 37\"}," +
                                "{\"Name\":\"Hosea-Malachi\",\"Position\":\"Zechariah 11\"}," +
                                "{\"Name\":\"Matthew-John\",\"Position\":\"Mark 7\"}," +
                                "{\"Name\":\"Acts-2 Corinthians\",\"Position\":\"Acts 7\"}," +
                                "{\"Name\":\"Galatians-Revelation\",\"Position\":\"1 John 2\"}" +
                                "]}";
            var bookmarks = JsonConvert.DeserializeObject<BookmarksStateData>(bookmarksJson);

            bookmarks.List.Count().Should().Be(10);
            bookmarks.Current.Should().Be("Genesis-Deuteronomy");

            var bookmarkManager = new BookmarkManager(Books, bookmarks, new BibleReferenceParser());

            bookmarkManager.CurrentReadingChapter.Should().Be("Exodus 38", "Initially loaded current position");
            bookmarkManager.MoveToNextBookmark();
            bookmarkManager.CurrentReadingChapter.Should().Be("1 Samuel 25", "Next bookmark position");

            var managedState = bookmarkManager.State;
            managedState.Current.Should().Be("Joshua-2 Chronicles", "State reflects change to current");
            managedState.List.First().Position.Should().Be("Exodus 39", "State should reflect updated position of previous list");
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
