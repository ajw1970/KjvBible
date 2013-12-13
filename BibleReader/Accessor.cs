﻿using Newtonsoft.Json;
using System;
using System.IO;

namespace BibleStudy
{
    public interface Accessor
    {
        void SaveReadingListData(string userId, ReadingListData data);
        ReadingListData LoadReadingListData(string userId);
    }

    public class FileAccessor : Accessor
    {
        public void SaveReadingListData(string userId, ReadingListData data)
        {
            try
            {
                using (var tr = new StreamWriter(String.Format("{0}-{1}.{2}", userId, "ReadingListData", "json")))
                {
                    tr.Write(JsonConvert.SerializeObject(data));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ReadingListData LoadReadingListData(string userId)
        {
            try
            {
                using (var tr = new StreamReader(String.Format("{0}-{1}.{2}", userId, "ReadingListData", "json")))
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
