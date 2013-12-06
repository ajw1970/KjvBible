using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections;
using BibleModel;
using KjvBible;

namespace BibleReader.Tests
{
    [TestClass]
    public class UnitTests
    {
        List<Book> books;

        [TestInitialize]
        public void Init()
        {
            var bible = Service.GetBible();
            books = new List<Book>(bible.BookGroups[0].Books);
            books.AddRange(bible.BookGroups[2].Books);
        }

        [TestMethod]
        public void BibleHas_66_Books()
        {
            Assert.AreEqual(66, books.Count);
        }

        [TestMethod]
        public void CanAddReadingListsByRange()
        {
            var reader = new BibleReader(books);
            var list1 = reader.AddReadingList("Gen", "Deut", "Ex", 7);
            Assert.AreEqual(5, list1.Count);
            Assert.AreEqual(50, list1[0].Chapters.Count);
            Assert.AreEqual(40, list1[1].Chapters.Count);
            Assert.AreEqual(27, list1[2].Chapters.Count);
            Assert.AreEqual(36, list1[3].Chapters.Count);
            Assert.AreEqual(34, list1[4].Chapters.Count);
            Assert.AreEqual(1, reader.ReadingLists.Count);
            var chapterCount = list1.Sum(c => c.Chapters.Count);
            Assert.AreEqual(187, chapterCount);
            Assert.AreEqual(chapterCount, reader.ReadingLists.First().ReadingItems.Count);

            var bookLists = new List<Book>(list1);

            var list2 = reader.AddReadingList("Joshua", "2 Chron", "Judges", 19);
            Assert.AreEqual(9, list2.Count);
            bookLists.AddRange(list2);

            var list3 = reader.AddReadingList("Ezra", "Job", "Job", 42);
            Assert.AreEqual(4, list3.Count);
            bookLists.AddRange(list3);

            var list4 = reader.AddReadingList("Psalm", 44);
            Assert.AreEqual(1, list4.Count);
            bookLists.AddRange(list4);

            var list5 = reader.AddReadingList("Prov", "Song", "Prov",  22);
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

            Assert.AreEqual("1 Corinthians 5", reader.Current);
            Assert.AreEqual("2 Timothy 3", reader.Next);
            Assert.AreEqual("Exodus 7", reader.Next);
            Assert.AreEqual("Judges 19", reader.Next);
        }

        [TestMethod]
        public void BibleReaderReturnsCurrentBookChapterVerse()
        {
            var bibleReader = new BibleReader(books, new List<ReadingList>
            {
                new ReadingList { ReadingItems = new List<string> { "Genesis 1", "Genesis 2", "Genesis 3" }},
                new ReadingList { ReadingItems = new List<string> { "John 1", "John 2", "John 3" }},
            });
            Assert.AreEqual("Genesis 1", bibleReader.Current, "CurrentReturnsCurrentBookChapterVerse");

            Assert.AreEqual("John 1", bibleReader.Next, "NextReturnsNextBookChapterVerse");

            Assert.AreEqual("John 1", bibleReader.Current, "CurrentAfterNextReturnsLastBookChapterVerse");

            Assert.AreEqual("Genesis 2", bibleReader.Next, "NextReturnsNextBookChapterVerse");

            Assert.AreEqual("John 2", bibleReader.Next, "NextReturnsNextBookChapterVerse");

            Assert.AreEqual("Genesis 3", bibleReader.Next, "NextReturnsNextBookChapterVerse");

            Assert.AreEqual("John 3", bibleReader.Next, "NextReturnsNextBookChapterVerse");

            Assert.AreEqual("Genesis 1", bibleReader.Next, "NextReturnsNextBookChapterVerse");
            Assert.AreEqual("John 1", bibleReader.Next, "NextReturnsNextBookChapterVerse");
        }

    }
}
