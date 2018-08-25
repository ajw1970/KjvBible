using System.Linq;
using System.Collections.Generic;
using BibleModel;
using System.IO;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace BibleStudy.Tests
{
    public class BibleReaderConfigurationTests
    {
        readonly List<BookData> _books;

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
        }

        [Fact]
        public void BibleHas_66_Books()
        {
            _books.Count.Should().Be(66);
        }

        [Fact]
        public void CanAddReadingListsByRangeStringAndCurrentString()
        {
            var bookMarksData = new BibleReaderBookMarksData();
            var reader = new BibleReader(_books, bookMarksData);
            var list1 = reader.AddReadingList(books: "Gen-Deut", current: "Ex 7");
            list1.Count.Should().Be(5, "Chapter Count");
            list1[0].ChapterCount.Should().Be(50);
            list1[1].ChapterCount.Should().Be(40);
            list1[2].ChapterCount.Should().Be(27);
            list1[3].ChapterCount.Should().Be(36);
            list1[4].ChapterCount.Should().Be(34);
            reader.ReadingListData.Lists.Count.Should().Be(1);
            var chapterCount = list1.Sum(c => c.ChapterCount);
            chapterCount.Should().Be(187);
        }

        [Fact]
        public void CanAddReadingListsByBookStringAndCurrentString()
        {
            var bookMarks = new BibleReaderBookMarksData();
            var reader = new BibleReader(_books, bookMarks);
            var list1 = reader.AddReadingList(books: "Psalm", current: "Psalm 44");
            list1.Count.Should().Be(1);
        }

        [Fact]
        public void CanAddReadingListsByRange()
        {
            var bookMarks = new BibleReaderBookMarksData();
            var reader = new BibleReader(_books, bookMarks);
            var list1 = reader.AddReadingList(
                firstBookname: "Gen", lastBookname: "Deut", 
                currentBookname: "Ex", currentChapterNumber: 7);
            list1.Count.Should().Be(5);
            list1[0].ChapterCount.Should().Be(50);
            list1[1].ChapterCount.Should().Be(40);
            list1[2].ChapterCount.Should().Be(27);
            list1[3].ChapterCount.Should().Be(36);
            list1[4].ChapterCount.Should().Be(34);
            reader.ReadingListData.Lists.Count.Should().Be(1);
            var chapterCount = list1.Sum(c => c.ChapterCount);
             chapterCount.Should().Be(187);

            var bookLists = new List<BookData>(list1);

            List<BookData> list2 = reader.AddReadingList(
                firstBookname: "Joshua", lastBookname: "2 Chron", 
                currentBookname: "Judges", currentChapterNumber: 19);
            list2.Count.Should().Be(9);
            bookLists.AddRange(list2);

            var list3 = reader.AddReadingList("Ezra", "Job", "Job", 42);
            list3.Count.Should().Be(4);
            bookLists.AddRange(list3);

            var list4 = reader.AddReadingList("Psalm", 44);
            list4.Count.Should().Be(1);
            bookLists.AddRange(list4);

            var list5 = reader.AddReadingList("Prov", "Song", "Prov", 22);
            list5.Count.Should().Be(3);
            bookLists.AddRange(list5);

            var list6 = reader.AddReadingList("Isaiah", "Daniel", "Jer", 6);
            list6.Count.Should().Be(5);
            bookLists.AddRange(list6);

            var list7 = reader.AddReadingList("Hosea", "Malachi", "Jon", 2);
            list7.Count.Should().Be(12);
            bookLists.AddRange(list7);

            var list8 = reader.AddReadingList("Matt", "John", "Matt", 4);
            list8.Count.Should().Be(4);
            bookLists.AddRange(list8);

            var list9 = reader.AddReadingList("Acts", "2 Cor", "1 Cor", 5);
            list9.Count.Should().Be(4);
            bookLists.AddRange(list9);

            var list10 = reader.AddReadingList("Gal", "Rev", "2 Tim", 3);
            list10.Count.Should().Be(19);
            bookLists.AddRange(list10);

            bookLists.Count.Should().Be(66);

            reader.SetCurrentListIndex(8);

            reader.CurrentChapterHeader.ToString().Should().Be("1 Corinthians 5", "Pick up after loading from file");
            reader.NextChapterHeader.ToString().Should().Be("2 Timothy 3");

            reader.NextChapterHeader.ToString().Should().Be("Exodus 7", "Pick up after loading again");
            reader.NextChapterHeader.ToString().Should().Be("Judges 19");
        }

        [Fact]
        public void BibleReaderReturnsCurrentBookChapterVerse()
        {
            var bibleReader = new BibleReader(_books, new BibleReaderBookMarksData());
            bibleReader.AddReadingList(bookName: "Gen", currentChapter: 1);
            bibleReader.AddReadingList(bookName: "John", currentChapter: 1);
            bibleReader.AddReadingList(bookName: "Jude", currentChapter: 1);

            bibleReader.CurrentChapterHeader.ToString().Should().Be("Genesis 1", "CurrentReturns Gen 1");

            bibleReader.NextChapterHeader.ToString().Should().Be("John 1", "NextReturns John 1");

           bibleReader.CurrentChapterHeader.ToString().Should().Be("John 1", "CurrentAfterNextReturnsLastBookChapterVerse");

           bibleReader.NextChapterHeader.ToString().Should().Be("Jude 1", "NextReturns Jude 1");

           bibleReader.NextChapterHeader.ToString().Should().Be("Genesis 2", "NextReturns Gen 2");

           bibleReader.NextChapterHeader.ToString().Should().Be("John 2", "NextReturns John 2");

           bibleReader.NextChapterHeader.ToString().Should().Be("Jude 1", "NextReturns Jude 1 (2)");

           bibleReader.NextChapterHeader.ToString().Should().Be("Genesis 3", "NextReturns Gen 3");

           bibleReader.NextChapterHeader.ToString().Should().Be("John 3", "NextReturns John 3");

           bibleReader.NextChapterHeader.ToString().Should().Be("Jude 1", "NextReturns Jude 1 (3)");

           bibleReader.NextChapterHeader.ToString().Should().Be("Genesis 4", "NextReturns Gen 4");
           bibleReader.NextChapterHeader.ToString().Should().Be("John 4", "NextReturns John 4");
        }

        [Fact]
        public void BibleReaderCanStartWithStateData()
        {
            var data = new BibleReaderBookMarksData()
            {
                BookMarks = new List<BibleReaderBookMarkData>()
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
                },
                CurrentBookMark = "Hosea-Malachi"
            };
            var bibleReader = new BibleReader(_books, data);

            bibleReader.CurrentChapterHeader.ToString().Should().Be("Zechariah 11");
        }
    }
}
