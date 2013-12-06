using BibleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader
{
    public class BibleReader
    {
        private List<ReadingList> readingLists;
        private int currentIndex;
        private List<Book> books;

        public BibleReader(List<Book> books, List<ReadingList> readingLists)
        {
            this.books = books;
            this.readingLists = readingLists;
        }

        public BibleReader(List<Book> books)
            : this(books, new List<ReadingList>())
        {
        }

        public List<ReadingList> ReadingLists
        {
            get
            {
                return readingLists;
            }
        }

        public List<Book> AddReadingList(string bookname, int currentChapter)
        {
            var book = (from b in books
                        where b.Name.StartsWith(bookname) || b.AbbreviatedName.StartsWith(bookname)
                        select b).FirstOrDefault();
            if (book != null)
            {
                readingLists.Add(new ReadingList
                {
                    ReadingItems = buildBookChapterList(book),
                    currentIndex = currentChapter - 1,
                });
                return new List<Book> { book };
            }
            return new List<Book>();
        }

        public void SetCurrentListIndex(int index)
        {
            if (readingLists.Count >= index + 1)
            {
                currentIndex = index;
            }
        }

        public List<Book> AddReadingList(string firstBookname, string lastBookname, string currentBookname, int currentChapter)
        {
            var addedBooks = new List<Book>();
            var range = new List<string>();
            var inRange = false;

            foreach (var book in books)
            {
                if (!inRange && (book.Name.StartsWith(firstBookname) || book.AbbreviatedName.StartsWith(firstBookname)))
                {
                    range.AddRange(buildBookChapterList(book));
                    addedBooks.Add(book);
                    inRange = true;
                }
                else if (book.Name.StartsWith(lastBookname) || book.AbbreviatedName.StartsWith(lastBookname))
                {
                    range.AddRange(buildBookChapterList(book));
                    addedBooks.Add(book);
                    inRange = false;
                    break;
                }
                else if (inRange)
                {
                    range.AddRange(buildBookChapterList(book));
                    addedBooks.Add(book);
                }
            }
            if (!inRange)
            {
                var currentBook = (from b in addedBooks
                                   where b.Name.StartsWith(currentBookname) || b.AbbreviatedName.StartsWith(currentBookname)
                                   select b).FirstOrDefault();
                readingLists.Add(new ReadingList
                {
                    ReadingItems = range,
                    currentIndex = range.IndexOf(String.Format("{0} {1}", currentBook.Name, currentChapter)),
                });
                return addedBooks;
            }

            return new List<Book>();
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

        private List<string> buildBookChapterList(Book book)
        {
            var listing = new List<string>();
            for (int i = 1; i <= book.Chapters.Count; i++)
            {
                listing.Add(String.Format("{0} {1}", book.Name, i));
            }
            return listing;
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
