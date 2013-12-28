using BibleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudy
{
    public class BibleStudyManagerImp : BibleStudyManager
    {
        private ReadingListDataAccessor accessor;

        public BibleStudyManagerImp(ReadingListDataAccessor accessor)
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
            try
            {
                var data = accessor.LoadReadingListData(userName);
                return new BibleReader(books, data);
            }
            catch (Exception e)
            {
                //todo: log error
                var reader = new BibleReader(books, new ReadingListData());
                reader.AddReadingList("Gen", "Rev", "Gen", 1);
                reader.SetCurrentListIndex(0);
                return reader;
            }
        }
    }
}
