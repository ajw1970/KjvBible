using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudy
{
    public interface Accessor
    {
        void SaveLists(string userId, ReadingLists lists);
        ReadingLists LoadLists(string userId);
    }

    public class FileAccessor : Accessor
    {
        public void SaveLists(string userId, ReadingLists lists)
        {
            try
            {
                using (var tr = new StreamWriter(String.Format("{0}-{1}.{2}", userId, "readinglists", "json")))
                {
                    tr.Write(JsonConvert.SerializeObject(lists));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ReadingLists LoadLists(string userId)
        {
            var lists = new ReadingLists();
            try
            {
                using (var tr = new StreamReader(String.Format("{0}-{1}.{2}", userId, "readinglists", "json")))
                {
                    var listsJson = tr.ReadToEnd();
                    lists = JsonConvert.DeserializeObject<ReadingLists>(listsJson);
                }
            }
            catch (Exception e)
            {
                //TODO: log error
            }

            return lists;
        }
    }

    public class SaveOnlyFileAccessor : Accessor
    {
        private Accessor accessor;
        public SaveOnlyFileAccessor(Accessor accessor)
        {
            this.accessor = accessor;
        }

        public ReadingLists LoadLists(string userId)
        {
            return new ReadingLists();
        }

        public void SaveLists(string userId, ReadingLists lists)
        {
            accessor.SaveLists(userId, lists);
        }
    }
}
