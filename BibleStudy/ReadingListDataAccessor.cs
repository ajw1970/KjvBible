using Newtonsoft.Json;
using System;
using System.IO;

namespace BibleStudy
{
    public interface ReadingListDataAccessor
    {
        void SaveReadingListData(string userId, ReadingListData data);
        ReadingListData LoadReadingListData(string userName);
    }

    public class ReadingListDataFileAccessor : ReadingListDataAccessor
    {
        public void SaveReadingListData(string userName, ReadingListData data)
        {
            try
            {
                using (var tr = new StreamWriter(String.Format("{0}-{1}.{2}", userName, "ReadingListData", "json")))
                {
                    tr.Write(JsonConvert.SerializeObject(data));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ReadingListData LoadReadingListData(string userName)
        {
            try
            {
                using (var tr = new StreamReader(String.Format("{0}-{1}.{2}", userName, "ReadingListData", "json")))
                {
                    return JsonConvert.DeserializeObject<ReadingListData>(tr.ReadToEnd());
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
