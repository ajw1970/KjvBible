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
        private ReadingListData data;
        private List<BookData> books;

        public BibleReader(List<BookData> books, ReadingListData data)
        {
            this.books = books;
            this.data = data;
        }

        public ReadingListData ReadingListData
        {
            get
            {
                return data;
            }
        }

        public int ReadingListCount
        {
            get
            {
                return data.Lists.Count;
            }
        }

        public List<BookData> AddReadingList(string bookName, int currentChapter)
        {
            var book = (from b in books
                        where b.Name.StartsWith(bookName) || b.AbbreviatedName.StartsWith(bookName)
                        select b).FirstOrDefault();
            if (book != null)
            {
                data.Lists.Add(new ReadingList
                {
                    Name = bookName,
                    ReadingChapters = buildBookChapterList(book),
                    CurrentChapterIndex = currentChapter - 1,
                });
                return new List<BookData> { book };
            }
            return new List<BookData>();
        }

        public void SetCurrentListIndex(int index)
        {
            if (ReadingListCount >= index + 1)
            {
                data.CurrentListIndex = index;
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
                data.Lists.Add(new ReadingList
                {
                    Name = String.Format("{0}-{1}", firstBookname, lastBookname),
                    ReadingChapters = range,
                    CurrentChapterIndex = range.IndexOf(currentChapter),
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
                if (currentReadingList.ReadingChapters.Count > currentReadingList.CurrentChapterIndex + 1)
                {
                    currentReadingList.CurrentChapterIndex++;
                }
                else
                {
                    currentReadingList.CurrentChapterIndex = 0;
                }

                if (ReadingListCount > data.CurrentListIndex + 1)
                {
                    data.CurrentListIndex++;
                }
                else
                {
                    data.CurrentListIndex = 0;
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
                return data.Lists[data.CurrentListIndex];
            }
        }

        private ReadingChapterHeader currentReadingListItem
        {
            get
            {
                return currentReadingList.ReadingChapters[currentReadingList.CurrentChapterIndex];
            }
        }
    }

    public class ReadingListData
    {
        public int CurrentListIndex { get; set; }
        public List<ReadingList> Lists { get; set; }

        public ReadingListData()
        {
            Lists = new List<ReadingList>();
        }
    }

    public class ReadingList
    {
        public string Name { get; set; }
        public List<ReadingChapterHeader> ReadingChapters { get; set; }
        public int CurrentChapterIndex { get; set; }
    }
}
