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

            reader.AddReadingList("Gen", "Deut", "Ex", 7);
            reader.AddReadingList("Joshua", "2 Chron", "Judges", 19);
            reader.AddReadingList("Ezra", "Job", "Job", 42);
            reader.AddReadingList("Psalm", 44);
            reader.AddReadingList("Prov", "Song", "Prov", 22);
            reader.AddReadingList("Isaiah", "Daniel", "Jer", 6);
            reader.AddReadingList("Hosea", "Malachi", "Jon", 2);
            reader.AddReadingList("Matt", "John", "Matt", 4);
            reader.AddReadingList("Acts", "2 Cor", "1 Cor", 5);
            reader.AddReadingList("Gal", "Rev", "2 Tim", 3);

        }

        // GET api/chapters
        public List<ReadingChapter> Get()
        {
            var current = reader.CurrentChapter;

            return new List<ReadingChapter> { current };
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
