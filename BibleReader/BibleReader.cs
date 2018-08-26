using BibleModel;
using ScriptureReferenceParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudy
{
    public class BibleReader
    {
        public BibleReader(IList<BookData> books, BibleReaderBookMarksData bookMarksData)
        {
            _books = books;
            _data = new ReadingListData();

            _bookMarksData = bookMarksData;
            foreach (var bookMark in _bookMarksData.BookMarks)
            {
                AddReadingList(bookMark);
            }

            for (int i = 0; i < _data.Lists.Count; i++)
            {
                if (_data.Lists[i].Name.Equals(_bookMarksData.CurrentBookMark))
                {
                    SetCurrentListIndex(i);
                    break;
                }
            }
        }

        public ReadingListData ReadingListData
        {
            get
            {
                return _data;
            }
        }

        public void SetCurrentListIndex(int index)
        {
            if (_data.Lists.Count >= index + 1)
            {
                _data.CurrentListIndex = index;
            }
        }

        private List<BookData> AddReadingList(BibleReaderBookMarkData bookMark)
        {
            var parser = new BibleReferenceParser();

            var bookRange = parser.ParseBookRange(bookMark.Range); ;
            var currentChapter = parser.ParseChapter(bookMark.Current);

            if (bookRange.Last == string.Empty && bookRange.First == currentChapter.Book)
            {
                return AddReadingList(currentChapter.Book, currentChapter.Chapter);
            }

            return AddReadingList(bookRange.First, bookRange.Last, currentChapter.Book, currentChapter.Chapter);
        }

        public List<BookData> AddReadingList(string bookName, int currentChapter)
        {
            var book = (from b in _books
                        where b.Name.StartsWith(bookName) || b.AbbreviatedName.StartsWith(bookName)
                        select b).FirstOrDefault();
            if (book != null)
            {
                _data.Lists.Add(new ReadingList
                {
                    Name = bookName,
                    ReadingChapters = buildBookChapterList(book),
                    CurrentChapterIndex = currentChapter - 1,
                });
                return new List<BookData> { book };
            }
            return new List<BookData>();
        }

        public List<BookData> AddReadingList(string books, string current)
        {
            var bookMark = new BibleReaderBookMarkData(books, current);

            var list = AddReadingList(bookMark);
            _bookMarksData.BookMarks.Add(bookMark);

            return list;
        }

        public List<BookData> AddReadingList(string firstBookname, string lastBookname, string currentBookname, int currentChapterNumber)
        {
            var addedBooks = new List<BookData>();
            var range = new List<ReadingChapterHeader>();
            var inRange = false;

            foreach (var book in _books)
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
                _data.Lists.Add(new ReadingList
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

                if (_data.Lists.Count > _data.CurrentListIndex + 1)
                {
                    _data.CurrentListIndex++;
                }
                else
                {
                    _data.CurrentListIndex = 0;
                }

                return currentReadingListItem;
            }
        }

        private ReadingListData _data;
        private IList<BookData> _books;
        private BibleReaderBookMarksData _bookMarksData;

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
                return _data.Lists[_data.CurrentListIndex];
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
}
