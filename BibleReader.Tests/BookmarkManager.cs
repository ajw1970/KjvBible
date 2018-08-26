using System.Collections.Generic;
using System.Linq;
using BibleModel;
using ScriptureReferenceParser;

namespace BibleStudy.Tests
{
    public partial class BibleReaderInterractionTests
    {
        public class BookmarkManager
        {
            public BookmarkManager(IEnumerable<BookData> books, BookmarksStateData bookmarks, IParser parser)
            {
                _books = books;
                _bookmarks = bookmarks;
                _parser = parser;
            }

            private readonly IEnumerable<BookData> _books;
            private readonly BookmarksStateData _bookmarks;
            private readonly IParser _parser;

            public string CurrentReadingChapter
            {
                get { return _bookmarks.List[CurrentBookmarkIndex].Position; }
            }

            public void MoveToNextBookmark()
            {
                SetCurrentBookmarkPositionToNextChapter();
                SetCurrentBookmarkToNextBookmark();
            }

            private void SetCurrentBookmarkToNextBookmark()
            {
                _bookmarks.Current = _bookmarks.List[CurrentBookmarkIndex + 1].Name;
            }

            private void SetCurrentBookmarkPositionToNextChapter()
            {
                (string Book, int Chapter) currentBookmarkPosition = _parser.ParseChapter(CurrentBookmark.Position);

                _bookmarks.List[CurrentBookmarkIndex].Position =
                    currentBookmarkPosition.Book + " " + (currentBookmarkPosition.Chapter + 1);
            }

            private int CurrentBookmarkIndex => _bookmarks.List.IndexOf(CurrentBookmark);

            private BookmarkStateData CurrentBookmark =>
                _bookmarks.List.First(bm => bm.Name.Equals(_bookmarks.Current));

            public BookmarksStateData State => _bookmarks;
        }
    }
}
