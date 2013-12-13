using BibleModel;
using System;
using System.Collections.Generic;
namespace BibleStudy
{
    public abstract class BibleStudyManager
    {
        protected Binder bible;
        protected List<BookData> books;
        protected BibleReader reader;

        public abstract ReadingChapter CurrentChapter { get; }
        public abstract ReadingChapter GetNextChapter();
    }
}
