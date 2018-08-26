using System.Linq;
using System.Collections.Generic;
using BibleModel;
using System.IO;
using FluentAssertions;
using Newtonsoft.Json;
using ScriptureReferenceParser;
using Xunit;

namespace BibleStudy.Tests
{
    public class BibleReaderConfigurationTests
    {
        readonly List<BookData> _books;
        private readonly IBibleReferenceParser _parser;

        public BibleReaderConfigurationTests()
        {
            //var bible = Service.GetBible();
            //books = bible.GetCannonizedBookData();
            //var booksJson = JsonConvert.SerializeObject(books);

            //using (var stream = new StreamWriter("BookDataList.json"))
            //{
            //    stream.WriteLine(booksJson);
            //}

            using (var stream = new StreamReader(@"Data\BookDataList.json"))
            {
                var booksJson = stream.ReadLine();
                _books = JsonConvert.DeserializeObject<List<BookData>>(booksJson);
            }

            _parser = new BibleReferenceParser();
        }

        [Fact]
        public void BibleHas_66_Books()
        {
            _books.Count.Should().Be(66);
        }

        [Fact]
        public void CanAddReadingListsByRangeStringAndCurrentString()
        {
            var data = new BibleReaderBookMarkData(range: "Gen-Deut", current: "Ex 7");

            data.GetBooksInRange(_parser, _books).Count().Should().Be(5, "Chapter Count");
            data.GetChapterCountInRange(_parser, _books).Should().Be(50 + 40 + 27 + 36 + 34);
        }

        [Fact]
        public void CanAddReadingListsByBookStringAndCurrentString()
        {
            var data = new BibleReaderBookMarkData(range: "Psalm", current: "Psalm 44");
            data.GetBooksInRange(_parser, _books).Count().Should().Be(1);
        }

        [Fact]
        public void CanAddReadingListsByRange()
        {
            var counts = new List<int>();
            var bookMarks = new BibleReaderBookMarksData();

            var data = new BibleReaderBookMarkData(firstBookname: "Gen", lastBookname: "Deut",
                currentBookname: "Ex", currentChapterNumber: 7);
            var count = data.GetBooksInRange(_parser, _books).Count();
            count.Should().Be(5, "Gen-Deut");
            counts.Add(count);
            data.GetChapterCountInRange(_parser, _books).Should().Be(187, "Gen-Deut Total Chapters");
            bookMarks = bookMarks.AddBookMark(data);

            data = new BibleReaderBookMarkData(firstBookname: "Joshua", lastBookname: "2 Chron",
                currentBookname: "Judges", currentChapterNumber: 19);
            count = data.GetBooksInRange(_parser, _books).Count();
            count.Should().Be(9, "Joshua-2 Chron");
            counts.Add(count);
            bookMarks = bookMarks.AddBookMark(data);

            data = new BibleReaderBookMarkData("Ezra", "Job", "Job", 42);
            count = data.GetBooksInRange(_parser, _books).Count();
            count.Should().Be(4, "Ezra-Job");
            counts.Add(count);
            bookMarks = bookMarks.AddBookMark(data);

            data = new BibleReaderBookMarkData("Psalm", 44);
            count = data.GetBooksInRange(_parser, _books).Count();
            count.Should().Be(1, "Psalm");
            counts.Add(count);
            bookMarks = bookMarks.AddBookMark(data);

            data = new BibleReaderBookMarkData("Prov", "Song", "Prov", 22);
            count = data.GetBooksInRange(_parser, _books).Count();
            count.Should().Be(3, "Prov-Song");
            counts.Add(count);
            bookMarks = bookMarks.AddBookMark(data);

            data = new BibleReaderBookMarkData("Isaiah", "Daniel", "Jer", 6);
            count = data.GetBooksInRange(_parser, _books).Count();
            count.Should().Be(5, "Isaiah-Daniel");
            counts.Add(count);
            bookMarks = bookMarks.AddBookMark(data);

            data = new BibleReaderBookMarkData("Hosea", "Malachi", "Jon", 2);
            count = data.GetBooksInRange(_parser, _books).Count();
            count.Should().Be(12, "Hosea-Malachi");
            counts.Add(count);
            bookMarks = bookMarks.AddBookMark(data);

            data = new BibleReaderBookMarkData("Matt", "John", "Matt", 4);
            count = data.GetBooksInRange(_parser, _books).Count();
            count.Should().Be(4, "Matt-John");
            counts.Add(count);
            bookMarks = bookMarks.AddBookMark(data);

            data = new BibleReaderBookMarkData("Acts", "2 Cor", "1 Cor", 5);
            count = data.GetBooksInRange(_parser, _books).Count();
            count.Should().Be(4, "Acts-2 Cor");
            counts.Add(count);
            bookMarks = bookMarks.AddBookMark(data);

            data = new BibleReaderBookMarkData("Gal", "Rev", "2 Tim", 3);
            count = data.GetBooksInRange(_parser, _books).Count();
            count.Should().Be(19, "Gal-Rev");
            counts.Add(count);
            bookMarks = bookMarks.AddBookMark(data);

            counts.Sum().Should().Be(66);

            var reader = new BibleReader(_books, bookMarks);
            reader.SetCurrentListIndex(8);

            reader.CurrentChapterHeader.ToString().Should().Be("1 Corinthians 5", "Pick up after loading from file");
            reader.AdvanceToNext();
            reader.CurrentChapterHeader.ToString().Should().Be("2 Timothy 3");

            reader.AdvanceToNext();
            reader.CurrentChapterHeader.ToString().Should().Be("Exodus 7", "Pick up after loading again");
            reader.AdvanceToNext();
            reader.CurrentChapterHeader.ToString().Should().Be("Judges 19");
        }

        [Fact]
        public void BibleReaderReturnsCurrentBookChapterVerse()
        {

            var bibleReader = new BibleReader(_books, new BibleReaderBookMarksData()
                .AddBookMark(bookName: "Gen", currentChapter: 1)
                .AddBookMark(bookName: "John", currentChapter: 1)
                .AddBookMark(bookName: "Jude", currentChapter: 1)
            );

            bibleReader.CurrentChapterHeader.ToString().Should().Be("Genesis 1", "CurrentReturns Gen 1");

            bibleReader.AdvanceToNext();
            bibleReader.CurrentChapterHeader.ToString().Should().Be("John 1", "NextReturns John 1");

            bibleReader.CurrentChapterHeader.ToString().Should().Be("John 1", "CurrentAfterNextReturnsLastBookChapterVerse");

            bibleReader.AdvanceToNext();
            bibleReader.CurrentChapterHeader.ToString().Should().Be("Jude 1", "NextReturns Jude 1");

            bibleReader.AdvanceToNext();
            bibleReader.CurrentChapterHeader.ToString().Should().Be("Genesis 2", "NextReturns Gen 2");

            bibleReader.AdvanceToNext();
            bibleReader.CurrentChapterHeader.ToString().Should().Be("John 2", "NextReturns John 2");

            bibleReader.AdvanceToNext();
            bibleReader.CurrentChapterHeader.ToString().Should().Be("Jude 1", "NextReturns Jude 1 (2)");

            bibleReader.AdvanceToNext();
            bibleReader.CurrentChapterHeader.ToString().Should().Be("Genesis 3", "NextReturns Gen 3");

            bibleReader.AdvanceToNext();
            bibleReader.CurrentChapterHeader.ToString().Should().Be("John 3", "NextReturns John 3");

            bibleReader.AdvanceToNext();
            bibleReader.CurrentChapterHeader.ToString().Should().Be("Jude 1", "NextReturns Jude 1 (3)");

            bibleReader.AdvanceToNext();
            bibleReader.CurrentChapterHeader.ToString().Should().Be("Genesis 4", "NextReturns Gen 4");

            bibleReader.AdvanceToNext();
            bibleReader.CurrentChapterHeader.ToString().Should().Be("John 4", "NextReturns John 4");
        }

        [Fact]
        public void BibleReaderCanStartWithStateData()
        {
            var data = new BibleReaderBookMarksData("Hosea-Malachi", new List<BibleReaderBookMarkData>()
            {
                new BibleReaderBookMarkData("Genesis-Deuteronomy", "Exodus 38"),
                new BibleReaderBookMarkData("Joshua-2 Chronicles", "1 Samuel 25"),
                new BibleReaderBookMarkData("Ezra-Jacob", "Esther 9"),
                new BibleReaderBookMarkData("Psalm", "Psalm 81"),
                new BibleReaderBookMarkData("Proverbs-Song of Solomon", "Proverbs 4"),
                new BibleReaderBookMarkData("Isaiah-Daniel", "Jeremiah 37"),
                new BibleReaderBookMarkData("Hosea-Malachi", "Zechariah 11"),
                new BibleReaderBookMarkData("Matthew-John", "Mark 7"),
                new BibleReaderBookMarkData("Acts-2 Corinthians", "Acts 7"),
                new BibleReaderBookMarkData("Galatians-Revelation", "1 John 2"),
            });

            var bibleReader = new BibleReader(_books, data);

            bibleReader.CurrentChapterHeader.ToString().Should().Be("Zechariah 11");
        }
    }
}
