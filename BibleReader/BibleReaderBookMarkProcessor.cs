using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibleModel;
using ScriptureReferenceParser;

namespace BibleStudy
{
    public class BibleReaderBookMarkProcessor
    {
        public BibleReaderBookMarkProcessor(IBibleReferenceParser parser, IEnumerable<BookData> books)
        {
            _parser = parser;
            _books = books;
        }

        public IEnumerable<BookData> GetBooksInRange(BibleReaderBookMarkData bookMark)
        {
            var booksInRange = new List<BookData>();

            var range = _parser.ParseBookRange(bookMark.Range);

            var firstBook = _books.FirstOrDefault(b => b.Name.StartsWith(range.First, StringComparison.CurrentCultureIgnoreCase) ||
                                                      b.AbbreviatedName.StartsWith(range.First, StringComparison.CurrentCultureIgnoreCase));

            if (firstBook == null) return booksInRange;
            if (string.IsNullOrEmpty(range.Last)) return new BookData[] { firstBook };

            var lastBook = _books.FirstOrDefault(b => b.Name.StartsWith(range.Last, StringComparison.CurrentCultureIgnoreCase) ||
                                                     b.AbbreviatedName.StartsWith(range.Last, StringComparison.CurrentCultureIgnoreCase));

            if (lastBook == null) return booksInRange;

            return _books.Where(b => b.Id >= firstBook.Id && b.Id <= lastBook.Id);
        }

        public int GetChapterCountInRange(BibleReaderBookMarkData bookMark)
        {
            return GetBooksInRange(bookMark).Sum(b => b.ChapterCount);
        }

        public int GetVerseCountInRange(BibleReaderBookMarkData bookMark)
        {
            return GetBooksInRange(bookMark).Sum(b => b.VerseCount);
        }

        public BibleReaderBookMarkData AdvanceToNextChapter(BibleReaderBookMarkData bookMark)
        {
            var current = _parser.ParseChapter(bookMark.Current);
            var currentBook = GetBook(current.Book);
            if (currentBook == null) throw new ApplicationException("Unable to find book: " + current.Book);
            if (current.Chapter < currentBook.ChapterCount)
            {
                //we can just go to the next verse in the current book
                return new BibleReaderBookMarkData(bookMark.Range, $"{current.Book} {current.Chapter + 1}");
            }

            var booksInRange = GetBooksInRange(bookMark);
            var nextBook = booksInRange.FirstOrDefault(b => b.Id > currentBook.Id) ?? booksInRange.First();
            return new BibleReaderBookMarkData(bookMark.Range, $"{nextBook.Name} {1}");
        }

        private BookData GetBook(string name)
        {
            return _books.FirstOrDefault(b => b.Name.StartsWith(name, StringComparison.CurrentCultureIgnoreCase) ||
                                       b.AbbreviatedName.StartsWith(name, StringComparison.CurrentCultureIgnoreCase));
        }

        private readonly IBibleReferenceParser _parser;
        private readonly IEnumerable<BookData> _books;

        public BibleReaderBookMarksData AdvanceToNextBookMark(BibleReaderBookMarksData bookMarksData)
        {
            var bookMarks = bookMarksData.BookMarks.ToList();
            var currentBookMark = bookMarks.First(bm =>
                bm.Range.Equals(bookMarksData.CurrentBookMark, StringComparison.CurrentCultureIgnoreCase));
            var index = bookMarks.IndexOf(currentBookMark);
            if (bookMarks.Count > ++index)
            {
                return new BibleReaderBookMarksData(bookMarks[index].Range, bookMarks);
            }
            return new BibleReaderBookMarksData(bookMarks.First().Range,bookMarks);
        }
    }
}
