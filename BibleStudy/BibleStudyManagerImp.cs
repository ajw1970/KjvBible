using BibleModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudy
{
    public class BibleStudyManagerImp : BibleStudyManager
    {
        private ReadingListDataAccessor accessor;

        public BibleStudyManagerImp(ReadingListDataAccessor listDataAccessor, string osisXml)
            : base(osisXml)
        {
            this.accessor = listDataAccessor;
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
            catch (Exception)
            {
                //todo: log error
                var reader = new BibleReader(books, new ReadingListData());

                //todo: need to put this back to a sane default. 
                //reader.AddReadingList("Gen", "Rev", "Gen", 1);
                //reader.SetCurrentListIndex(0);

                reader.AddReadingList("Gen", "Deut", "Gen", 1);
                reader.AddReadingList("Joshua", "2 Chron", "Joshua", 1);
                reader.AddReadingList("Ezra", "Job", "Ezra", 1);
                reader.AddReadingList("Psalm", 1);
                reader.AddReadingList("Prov", "Song", "Prov", 1);
                reader.AddReadingList("Isaiah", "Daniel", "Isaiah", 1);
                reader.AddReadingList("Hosea", "Malachi", "Hosea", 1);
                reader.AddReadingList("Matt", "John", "Matt", 1);
                reader.AddReadingList("Acts", "2 Cor", "Acts", 1);
                reader.AddReadingList("Gal", "Rev", "Gal", 1);
                reader.SetCurrentListIndex(0);

                return reader;
            }
        }
    }
}
