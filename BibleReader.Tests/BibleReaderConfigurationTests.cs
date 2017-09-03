﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections;
using BibleModel;
using System.IO;
using BibleStudy;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace BibleStudy.Tests
{
    [TestClass]
    public class BibleReaderConfigurationTests
    {
        List<BookData> books;

        [TestInitialize]
        public void Init()
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
                books = JsonConvert.DeserializeObject<List<BookData>>(booksJson);
            }
        }

        [TestMethod]
        public void BibleHas_66_Books()
        {
            Assert.AreEqual(66, books.Count);
        }

        [TestMethod]
        public void CanAddReadingListsByRangeStringAndCurrentString()
        {
            var bookMarksData = new BibleReaderBookMarksData();
            var reader = new BibleReader(books, bookMarksData);
            var list1 = reader.AddReadingList("Gen-Deut", "Ex 7");
            Assert.AreEqual(5, list1.Count, "Chapter Count");
            Assert.AreEqual(50, list1[0].ChapterCount);
            Assert.AreEqual(40, list1[1].ChapterCount);
            Assert.AreEqual(27, list1[2].ChapterCount);
            Assert.AreEqual(36, list1[3].ChapterCount);
            Assert.AreEqual(34, list1[4].ChapterCount);
            Assert.AreEqual(1, reader.ReadingListData.Lists.Count);
            var chapterCount = list1.Sum(c => c.ChapterCount);
            Assert.AreEqual(187, chapterCount);
        }

        [TestMethod]
        public void CanAddReadingListsByBookStringAndCurrentString()
        {
            var bookMarks = new BibleReaderBookMarksData();
            var reader = new BibleReader(books, bookMarks);
            var list1 = reader.AddReadingList("Psalm", "Psalm 44");
            Assert.AreEqual(1, list1.Count);
        }

        [TestMethod]
        public void CanAddReadingListsByRange()
        {
            var bookMarks = new BibleReaderBookMarksData();
            var reader = new BibleReader(books, bookMarks);
            var list1 = reader.AddReadingList("Gen", "Deut", "Ex", 7);
            Assert.AreEqual(5, list1.Count);
            Assert.AreEqual(50, list1[0].ChapterCount);
            Assert.AreEqual(40, list1[1].ChapterCount);
            Assert.AreEqual(27, list1[2].ChapterCount);
            Assert.AreEqual(36, list1[3].ChapterCount);
            Assert.AreEqual(34, list1[4].ChapterCount);
            Assert.AreEqual(1, reader.ReadingListData.Lists.Count);
            var chapterCount = list1.Sum(c => c.ChapterCount);
            Assert.AreEqual(187, chapterCount);

            var bookLists = new List<BookData>(list1);

            var list2 = reader.AddReadingList("Joshua", "2 Chron", "Judges", 19);
            Assert.AreEqual(9, list2.Count);
            bookLists.AddRange(list2);

            var list3 = reader.AddReadingList("Ezra", "Job", "Job", 42);
            Assert.AreEqual(4, list3.Count);
            bookLists.AddRange(list3);

            var list4 = reader.AddReadingList("Psalm", 44);
            Assert.AreEqual(1, list4.Count);
            bookLists.AddRange(list4);

            var list5 = reader.AddReadingList("Prov", "Song", "Prov", 22);
            Assert.AreEqual(3, list5.Count);
            bookLists.AddRange(list5);

            var list6 = reader.AddReadingList("Isaiah", "Daniel", "Jer", 6);
            Assert.AreEqual(5, list6.Count);
            bookLists.AddRange(list6);

            var list7 = reader.AddReadingList("Hosea", "Malachi", "Jon", 2);
            Assert.AreEqual(12, list7.Count);
            bookLists.AddRange(list7);

            var list8 = reader.AddReadingList("Matt", "John", "Matt", 4);
            Assert.AreEqual(4, list8.Count);
            bookLists.AddRange(list8);

            var list9 = reader.AddReadingList("Acts", "2 Cor", "1 Cor", 5);
            Assert.AreEqual(4, list9.Count);
            bookLists.AddRange(list9);

            var list10 = reader.AddReadingList("Gal", "Rev", "2 Tim", 3);
            Assert.AreEqual(19, list10.Count);
            bookLists.AddRange(list10);

            Assert.AreEqual(66, bookLists.Count);

            reader.SetCurrentListIndex(8);

            Assert.AreEqual("1 Corinthians 5", reader.CurrentChapterHeader.ToString(), "Pick up after loading from file");
            Assert.AreEqual("2 Timothy 3", reader.NextChapterHeader.ToString());

            Assert.AreEqual("Exodus 7", reader.NextChapterHeader.ToString(), "Pick up after loading again");
            Assert.AreEqual("Judges 19", reader.NextChapterHeader.ToString());
        }

        [TestMethod]
        public void BibleReaderReturnsCurrentBookChapterVerse()
        {
            var bibleReader = new BibleReader(books, new BibleReaderBookMarksData());
            bibleReader.AddReadingList("Gen", 1);
            bibleReader.AddReadingList("John", 1);
            bibleReader.AddReadingList("Jude", 1);

            Assert.AreEqual("Genesis 1", bibleReader.CurrentChapterHeader.ToString(), "CurrentReturns Gen 1");

            Assert.AreEqual("John 1", bibleReader.NextChapterHeader.ToString(), "NextReturns John 1");

            Assert.AreEqual("John 1", bibleReader.CurrentChapterHeader.ToString(), "CurrentAfterNextReturnsLastBookChapterVerse");

            Assert.AreEqual("Jude 1", bibleReader.NextChapterHeader.ToString(), "NextReturns Jude 1");

            Assert.AreEqual("Genesis 2", bibleReader.NextChapterHeader.ToString(), "NextReturns Gen 2");

            Assert.AreEqual("John 2", bibleReader.NextChapterHeader.ToString(), "NextReturns John 2");

            Assert.AreEqual("Jude 1", bibleReader.NextChapterHeader.ToString(), "NextReturns Jude 1 (2)");

            Assert.AreEqual("Genesis 3", bibleReader.NextChapterHeader.ToString(), "NextReturns Gen 3");

            Assert.AreEqual("John 3", bibleReader.NextChapterHeader.ToString(), "NextReturns John 3");

            Assert.AreEqual("Jude 1", bibleReader.NextChapterHeader.ToString(), "NextReturns Jude 1 (3)");

            Assert.AreEqual("Genesis 4", bibleReader.NextChapterHeader.ToString(), "NextReturns Gen 4");
            Assert.AreEqual("John 4", bibleReader.NextChapterHeader.ToString(), "NextReturns John 4");
        }

        [TestMethod]
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
            var bibleReader = new BibleReader(books, data);

            bibleReader.CurrentChapterHeader.ToString().Should().Be("Zechariah 11");
        }
    }
}
