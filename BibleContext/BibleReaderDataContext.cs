using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibleModel;
using BibleStudy;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BibleContext
{
    public class BibleReaderDataContext : ReadingListDataAccessor
    {
        ReadingListContext db;

        public BibleReaderDataContext()
        {
            db = new ReadingListContext();
        }

        public void SaveReadingListData(string userName, ReadingListData data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            var graph = getGraph(userName);

            if (graph == null)
            {
                graph = new ReadingListObjectGraph
                {
                    UserName = userName,
                    Data = jsonData
                };
                db.ReadingListObjectGraphs.Add(graph);
            }
            else
            {
                graph.Data = jsonData;
            }
            db.SaveChanges();
        }

        public ReadingListData LoadReadingListData(string userName)
        {
            var jsonData = getGraph(userName).Data;
            var data = JsonConvert.DeserializeObject<ReadingListData>(jsonData);
            return data;
        }

        private ReadingListObjectGraph getGraph(string userName)
        {
            return db.ReadingListObjectGraphs.Where(d => d.UserName == userName).FirstOrDefault();
        }
    }

    public class ReadingListContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public ReadingListContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<ReadingListObjectGraph> ReadingListObjectGraphs { get; set; }

    }

    public class ReadingListObjectGraph
    {
        [Key]
        public string UserName { get; set; }
        public string Data { get; set; }
    }
}
