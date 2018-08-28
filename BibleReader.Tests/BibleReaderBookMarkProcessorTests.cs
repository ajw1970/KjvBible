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
            var bookMark = new BibleReaderBookMarkData(range: "Gen-Deut", current: "Ex 7");

            var processor = new BibleReaderBookMarkProcessor(_parser, _books);
            processor.GetBooksInRange(bookMark).Count().Should().Be(5, "Chapter Count");
            processor.GetChapterCountInRange(bookMark).Should().Be(50 + 40 + 27 + 36 + 34);
        }

        [Fact]
        public void CanAddReadingListsByBookStringAndCurrentString()
        {
            var bookMark = new BibleReaderBookMarkData(range: "Psalm", current: "Psalm 44");

            var processor = new BibleReaderBookMarkProcessor(_parser, _books);
            processor.GetBooksInRange(bookMark).Count().Should().Be(1);
        }

        [Fact]
        public void CanAdvanceToNextChapter()
        {
            var bookMark = new BibleReaderBookMarkData("1 John-3 John","1 John 4");

            var processor = new BibleReaderBookMarkProcessor(_parser, _books);
            bookMark = processor.AdvanceToNextChapter(bookMark);
            bookMark.Current.Should().Be("1 John 5", "Another Chapter");

            bookMark = processor.AdvanceToNextChapter(bookMark);
            bookMark.Current.Should().Be("2 John 1", "Next Book");
            
            bookMark = processor.AdvanceToNextChapter(bookMark);
            bookMark.Current.Should().Be("3 John 1", "Next Book Again");

            bookMark = processor.AdvanceToNextChapter(bookMark);
            bookMark.Current.Should().Be("1 John 1", "Should Wrap back around");
        }

        [Fact]
        public void AdvanceToNextBookMark()
        {
            var bookMarks = new BibleReaderBookMarksData("Gen-Deut", new List<BibleReaderBookMarkData>()
            {
                new BibleReaderBookMarkData("Gen-Deut", "Gen 1"),
                new BibleReaderBookMarkData("Psalm", "Psalm 1")
            });

            var processor = new BibleReaderBookMarkProcessor(_parser, _books);

            bookMarks = processor.AdvanceToNextBookMark(bookMarks);
            bookMarks.CurrentBookMark.Should().Be("Psalm", "First Time");

            bookMarks = processor.AdvanceToNextBookMark(bookMarks);
            bookMarks.CurrentBookMark.Should().Be("Gen-Deut", "Second Time");
        }

        private readonly List<BookData> _books;
        private readonly BibleReferenceParser _parser;
    }
}
