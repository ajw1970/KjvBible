using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections;

namespace BibleReader.Test
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void BibleReaderIsSingleton()
        {
            var bibleReader = BibleReader.Instance;
            Assert.IsNotNull(bibleReader);

            var bibleReader2 = BibleReader.Instance;
            Assert.AreSame(bibleReader, bibleReader2);
        }

        [TestMethod]
        public void BibleReaderReturnsCurrentBookChapterVerse()
        {
            var bibleReader = BibleReader.Instance;
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

        private static BibleReader bibleReader;
        public static BibleReader Instance
        {
            get
            {
                if (bibleReader == null)
                {
                    bibleReader = new BibleReader();
                }
                return bibleReader;
            }
        }

        private BibleReader()
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
