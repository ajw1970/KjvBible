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
            catch (Exception e)
            {
                //todo: log error
                var reader = new BibleReader(books, new ReadingListData());

                //todo: need to put this back to a sane default. 
                reader.AddReadingList("Gen", "Rev", "Gen", 1);
                reader.SetCurrentListIndex(0);

                //reader.AddReadingList("Gen", "Deut", "Ex", 11);
                //reader.AddReadingList("Joshua", "2 Chron", "Ruth", 1);
                //reader.AddReadingList("Ezra", "Job", "Ezra", 3);
                //reader.AddReadingList("Psalm", 47);
                //reader.AddReadingList("Prov", "Song", "Prov", 25);
                //reader.AddReadingList("Isaiah", "Daniel", "Jer", 9);
                //reader.AddReadingList("Hosea", "Malachi", "Micah", 1);
                //reader.AddReadingList("Matt", "John", "Matt", 7);
                //reader.AddReadingList("Acts", "2 Cor", "1 Cor", 8);
                //reader.AddReadingList("Gal", "Rev", "Titus", 3);
                //reader.SetCurrentListIndex(1);

                return reader;
            }
        }
    }
}
