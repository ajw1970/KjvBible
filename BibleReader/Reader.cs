using BibleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader
{
    public class Reader
    {
        private List<ReadingList> readingLists;
        private int currentIndex;
        private List<BookData> books;

        public Reader(List<BookData> books, List<ReadingList> readingLists)
        {
            this.books = books;
            this.readingLists = readingLists;
        }

        public Reader(List<BookData> books)
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

        public List<BookData> AddReadingList(string bookName, int currentChapter)
        {
            var book = (from b in books
                        where b.Name.StartsWith(bookName) || b.AbbreviatedName.StartsWith(bookName)
                        select b).FirstOrDefault();
            if (book != null)
            {
                readingLists.Add(new ReadingList
                {
                    Name = bookName,
                    ReadingChapters = buildBookChapterList(book),
                    currentIndex = currentChapter - 1,
                });
                return new List<BookData> { book };
            }
            return new List<BookData>();
        }

        public void SetCurrentListIndex(int index)
        {
            if (readingLists.Count >= index + 1)
            {
                currentIndex = index;
            }
        }

        public List<BookData> AddReadingList(string firstBookname, string lastBookname, string currentBookname, int currentChapterNumber)
        {
            var addedBooks = new List<BookData>();
            var range = new List<ReadingChapterHeader>();
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
                var currentChapter = (from c in range
                                      where c.BookName == currentBook.Name && c.Number == currentChapterNumber
                                      select c).FirstOrDefault();
                readingLists.Add(new ReadingList
                {
                    Name = String.Format("{0}-{1}", firstBookname, lastBookname),
                    ReadingChapters = range,
                    currentIndex = range.IndexOf(currentChapter),
                });
                return addedBooks;
            }

            return new List<BookData>();
        }

        public ReadingChapterHeader CurrentChapterHeader
        {
            get
            {
                return currentReadingListItem;
            }
        }

        public ReadingChapterHeader NextChapterHeader
        {
            get
            {
                if (currentReadingList.ReadingChapters.Count > currentReadingList.currentIndex + 1)
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

        private List<ReadingChapterHeader> buildBookChapterList(BookData book)
        {
            var listing = new List<ReadingChapterHeader>();
            for (int i = 1; i <= book.ChapterCount; i++)
            {
                listing.Add(new ReadingChapterHeader
                {
                    BookName = book.Name,
                    Number = i,
                });
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

        private ReadingChapterHeader currentReadingListItem
        {
            get
            {
                return currentReadingList.ReadingChapters[currentReadingList.currentIndex];
            }
        }
    }
}
