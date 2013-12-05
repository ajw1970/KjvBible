using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections;
using BibleModel;
using KjvBible;

namespace BibleReader.Test
{
    public class BookHeader
    {
        public string Name { get; set; }
        public string LongName { get; set; }
        public string AbbreviatedName { get; set; }
        public int ChapterCount { get; set; }
    }

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
        public void CanSearchForRangeOfBooks()
        {
            var bookTitles = from b in books
                             select new BookHeader {
                                Name = b.Name,
                                LongName = b.LongName,
                                AbbreviatedName = b.AbbreviatedName,
                                ChapterCount = b.Chapters.Count,
                             };
            var searchedTitles = BibleReader.SearchBooks("Gen-Deut", bookTitles);
            Assert.AreEqual(5, searchedTitles.Count);
        }

        [TestMethod]
        public void BibleReaderReturnsCurrentBookChapterVerse()
        {
            var bibleReader = new BibleReader();
            Assert.AreEqual("Genesis 1:1", bibleReader.Current, "CurrentReturnsCurrentBookChapterVerse");

            Assert.AreEqual("John 1:1", bibleReader.Next, "NextReturnsNextBookChapterVerse");

            Assert.AreEqual("John 1:1", bibleReader.Current, "CurrentAfterNextReturnsLastBookChapterVerse");

            Assert.AreEqual("Genesis 2:1", bibleReader.Next, "NextReturnsNextBookChapterVerse");

            Assert.AreEqual("John 2:1", bibleReader.Next, "NextReturnsNextBookChapterVerse");

            Assert.AreEqual("Genesis 3:1", bibleReader.Next, "NextReturnsNextBookChapterVerse");

            Assert.AreEqual("John 3:1", bibleReader.Next, "NextReturnsNextBookChapterVerse");

            Assert.AreEqual("Genesis 1:1", bibleReader.Next, "NextReturnsNextBookChapterVerse");
            Assert.AreEqual("John 1:1", bibleReader.Next, "NextReturnsNextBookChapterVerse");
        }

    }

    public class ReadingList
    {
        public int currentIndex { get; set; }
        public List<string> ReadingItems { get; set; }
    }

    public class BibleReader
    {
        private List<ReadingList> readingLists;
        private int currentIndex;

        public BibleReader()
        {
            readingLists = new List<ReadingList> {
                new ReadingList{
                ReadingItems = new List<string> {
                    "Genesis 1:1",
                    "Genesis 2:1",
                    "Genesis 3:1"
                }},
                new ReadingList{
                ReadingItems = new List<string> {
                    "John 1:1",
                    "John 2:1",
                    "John 3:1"
                }},
            };
        }

        public static List<string> SearchBooks(string searchFor, IEnumerable<BookHeader> bookTitles)
        {
            var searchTerms = new List<string>(searchFor.Split(';'));
            var retList = new List<string>();
            foreach (var term in searchTerms)
            {
                if (term.Contains('-'))
                {
                    var foundRange = new List<string>();
                    var inRange = false;
                    var range = term.Split('-');
                    foreach (var title in bookTitles)
                    {
                        if (!inRange && (title.Name.StartsWith(range[0]) || title.AbbreviatedName.StartsWith(range[0])))
                        {
                            foundRange.Add(title.Name);
                            inRange = true;
                        } else if (title.Name.StartsWith(range[1]) || title.AbbreviatedName.StartsWith(range[1]))
                        {
                            foundRange.Add(title.Name);
                            break;
                        } else if (inRange)
                        {
                            foundRange.Add(title.LongName);
                        }
                    }
                    if (foundRange.Any())
                    {
                        retList.AddRange(foundRange);
                    }
                }
            }
            return retList;
        }

        public string Current
        {
            get
            {
                return currentReadingListItem;
            }
        }

        public string Next
        {
            get
            {
                if (currentReadingList.ReadingItems.Count > currentReadingList.currentIndex + 1)
                {
                    currentReadingList.currentIndex++;
                }
                else
                {
                    currentReadingList.currentIndex = 0;
                }

                if (readingLists.Count > currentIndex + 1)
                {
                    currentIndex++;
                }
                else
                {
                    currentIndex = 0;
                }

                return currentReadingListItem;
            }
        }

        private ReadingList currentReadingList
        {
            get
            {
                return readingLists[currentIndex];
            }
        }

        private string currentReadingListItem
        {
            get
            {
                return currentReadingList.ReadingItems[currentReadingList.currentIndex];
            }
        }
    }
}
