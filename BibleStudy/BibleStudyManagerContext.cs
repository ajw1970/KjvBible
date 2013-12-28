using BibleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudy
{
    class BibleStudyManagerContext : BibleStudyManager
    {
        private Accessor accessor;

        public BibleStudyManagerContext(Accessor accessor)
        {
            this.accessor = accessor;
        }

        public override ReadingChapter GetCurrentChapter(string userName)
        {
            var reader = GetReader(userName);
            var chapter = ConvertHeaderToChapter(reader.CurrentChapterHeader);
            accessor.SaveReadingListData(userName, reader.ReadingListData);
            return chapter;
        }

        public override ReadingChapter GetNextChapter(string userName)
        {
            var reader = GetReader(userName);
            var chapter = ConvertHeaderToChapter(reader.NextChapterHeader);
            accessor.SaveReadingListData(userName, reader.ReadingListData);
            return chapter;
        }

        private BibleReader GetReader(string userName)
        {
            var data = accessor.LoadReadingListData(userName);
            return new BibleReader(books, data);
        }
    }
}
