using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibleModel;
using ScriptureReferenceParser;

namespace BibleStudy
{
    public class BibleReaderBookMarkData
    {
        public string Range { get; }
        public string Current { get; }

        public BibleReaderBookMarkData(string range, string current)
        {
            Range = range;
            Current = current;
        }

        public BibleReaderBookMarkData(string firstBookname, string lastBookname, string currentBookname, int currentChapterNumber)
        {
            if (string.IsNullOrEmpty(firstBookname)) throw new ArgumentException(nameof(firstBookname));
            if (string.IsNullOrEmpty(lastBookname)) throw new ArgumentException(nameof(lastBookname));
            if (string.IsNullOrEmpty(currentBookname)) throw new ArgumentException(nameof(currentBookname));
            if (currentChapterNumber <= 0) throw new ArgumentException(nameof(currentChapterNumber));

            Range = $"{firstBookname}-{lastBookname}";
            Current = $"{currentBookname} {currentChapterNumber}";
        }

        public BibleReaderBookMarkData(string bookName, int chapterNumber)
        {
            if (string.IsNullOrEmpty(bookName)) throw new ArgumentException(nameof(bookName));
            if (chapterNumber <= 0) throw new ArgumentException(nameof(chapterNumber));

            Range = bookName;
            Current = $"{bookName} {chapterNumber}";
        }

        public IEnumerable<BookData> GetBooksInRange(IBibleReferenceParser parser, IEnumerable<BookData> books)
        {
            var booksInRange = new List<BookData>();

            var range = parser.ParseBookRange(Range);

            var firstBook = books.FirstOrDefault(b => b.Name.StartsWith(range.First, StringComparison.CurrentCultureIgnoreCase) || 
                                             b.AbbreviatedName.StartsWith(range.First, StringComparison.CurrentCultureIgnoreCase));

            if (firstBook == null) return booksInRange;
            if (string.IsNullOrEmpty(range.Last)) return new BookData[] { firstBook };

            var lastBook = books.FirstOrDefault(b => b.Name.StartsWith(range.Last, StringComparison.CurrentCultureIgnoreCase) ||
                                                      b.AbbreviatedName.StartsWith(range.Last, StringComparison.CurrentCultureIgnoreCase));

            if (lastBook == null) return booksInRange;

            return books.Where(b => b.Id >= firstBook.Id && b.Id <= lastBook.Id);
        }

        public int GetChapterCountInRange(IBibleReferenceParser parser, IEnumerable<BookData> books)
        {
            return GetBooksInRange(parser, books).Sum(b => b.ChapterCount);
        }

        public int GetVerseCountInRange(IBibleReferenceParser parser, IEnumerable<BookData> books)
        {
            return GetBooksInRange(parser, books).Sum(b => b.VerseCount);
        }
    }
}
