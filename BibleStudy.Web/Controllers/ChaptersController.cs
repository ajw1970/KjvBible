using BibleModel;
using BibleReader;
using KjvBible;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BibleStudy.Web.Controllers
{
    public class ChaptersController : ApiController
    {
        Binder bible;
        List<Book> books;
        Reader reader;

        public ChaptersController()
        {
            bible = Service.GetBible();
            books = new List<Book>(bible.BookGroups[0].Books);
            books.AddRange(bible.BookGroups[2].Books);
            reader = new Reader(books);
        }

        // GET api/chapters
        public List<ReadingChapterHeader> Get()
        {
            var current = reader.CurrentChapterHeader;

            return new List<ReadingChapterHeader>();
        }

        // GET api/chapters/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/chapters
        public void Post([FromBody]string value)
        {
        }

        // PUT api/chapters/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/chapters/5
        public void Delete(int id)
        {
        }
    }
}
