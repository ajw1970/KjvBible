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
    public class BibleReaderBookMarkProcessorTests
    {
        public BibleReaderBookMarkProcessorTests()
        {
            _parser = new BibleReferenceParser();
            using (var stream = new StreamReader(@"Data\BookDataList.json"))
            {
                var booksJson = stream.ReadLine();
                _books = JsonConvert.DeserializeObject<List<BookData>>(booksJson);
            }
        }

        [Fact]
        public void CanAddReadingListsByRangeStringAndCurrentString()
        {
            var bookMark = new BibleReaderBookMarkData()
            {
                Name = "Gen-Deut",
                Position = "Ex 7"
            };

            var processor = new BibleReaderBookMarkProcessor(_parser, _books);
            processor.GetBooksInRange(bookMark).Count().Should().Be(5, "Chapter Count");
            processor.GetChapterCountInRange(bookMark).Should().Be(50 + 40 + 27 + 36 + 34);
        }

        [Fact]
        public void CanAddReadingListsByBookStringAndCurrentString()
        {
            var bookMark = new BibleReaderBookMarkData() { Name = "Psalm", Position = "Psalm 44" };

            var processor = new BibleReaderBookMarkProcessor(_parser, _books);
            processor.GetBooksInRange(bookMark).Count().Should().Be(1);
        }

        [Fact]
        public void CanAdvanceToNextChapter()
        {
            var bookMark = new BibleReaderBookMarkData() { Name = "1 John-3 John", Position = "1 John 4" };

            var processor = new BibleReaderBookMarkProcessor(_parser, _books);
            bookMark = processor.AdvanceToNextChapter(bookMark);
            bookMark.Position.Should().Be("1 John 5", "Another Chapter");

            bookMark = processor.AdvanceToNextChapter(bookMark);
            bookMark.Position.Should().Be("2 John 1", "Next Book");

            bookMark = processor.AdvanceToNextChapter(bookMark);
            bookMark.Position.Should().Be("3 John 1", "Next Book Again");

            bookMark = processor.AdvanceToNextChapter(bookMark);
            bookMark.Position.Should().Be("1 John 1", "Should Wrap back around");
        }

        [Fact]
        public void AdvanceToNext()
        {
            var bookMarksData = new BibleReaderBookMarksData()
            {
                CurrentName = "Gen-Deut",
                BookMarks = new List<BibleReaderBookMarkData>()
                {
                    new BibleReaderBookMarkData() {Name = "Gen-Deut",  Position = "Gen 1"},
                    new BibleReaderBookMarkData() {Name = "Psalm",  Position = "Psalm 1"}
                }
            };

            var processor = new BibleReaderBookMarkProcessor(_parser, _books);

            bookMarksData = processor.AdvanceToNext(bookMarksData);
            bookMarksData.BookMarks.First(bm => bm.Name.Equals("Gen-Deut")).Position.Should().Be("Gen 2", "Bumped on First Time");
            bookMarksData.CurrentName.Should().Be("Psalm", "First Time");

            bookMarksData = processor.AdvanceToNext(bookMarksData);
            bookMarksData.BookMarks.First(bm => bm.Name.Equals("Psalm")).Position.Should().Be("Psalm 2", "Bumped on Second Time");
            bookMarksData.CurrentName.Should().Be("Gen-Deut", "Second Time");
        }

        [Fact]
        public void GetCurrentPosition()
        {
            var bookMarksData = new BibleReaderBookMarksData()
            {
                CurrentName = "Gen-Deut",
                BookMarks = new List<BibleReaderBookMarkData>()
                {
                    new BibleReaderBookMarkData() { Name = "Gen-Deut", Position = "Gen 1"},
                    new BibleReaderBookMarkData() {Name = "Psalm", Position = "Psalm 1"}
                }
            };

            var processor = new BibleReaderBookMarkProcessor(_parser, _books);
            var currentPosition = processor.GetCurrentPosition(bookMarksData);
            currentPosition.Should().Be("Gen 1");
        }

        private readonly List<BookData> _books;
        private readonly BibleReferenceParser _parser;
    }
}
