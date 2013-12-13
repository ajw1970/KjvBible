using BibleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudy
{
    public class BibleReader
    {
        private ReadingLists readingLists;
        private List<BookData> books;
        private Accessor accessor;
        private string userId;

        public BibleReader(List<BookData> books, Accessor accessor, string userId)
        {
            this.books = books;
            this.accessor = accessor;
            this.userId = userId;
            loadLists();
        }

        public void SaveLists()
        {
            accessor.SaveLists(userId, readingLists);
        }

        private void loadLists()
        {
            readingLists = accessor.LoadLists(userId);
        }

        public ReadingLists ReadingLists
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
                readingLists.AddList(new ReadingList
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
                readingLists.CurrentIndex = index;
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
                readingLists.AddList(new ReadingList
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

                if (readingLists.Count > readingLists.CurrentIndex + 1)
                {
                    readingLists.CurrentIndex++;
                }
                else
                {
                    readingLists.CurrentIndex = 0;
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
                return readingLists.Lists[readingLists.CurrentIndex];
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
