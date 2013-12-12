using BibleModel;
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
        BibleStudyManager studyManager;

        public ChaptersController(BibleStudyManager studyManager)
        {
            this.studyManager = studyManager;
        }
        // GET api/chapters
        [Route("api/chapters/")]
        public List<ReadingChapter> Get()
        {
            return new List<ReadingChapter> { studyManager.CurrentChapter };
        }

        [Route("api/chapters/{id}")]
        public ReadingChapter Get(ReadingChapterHeader id)
        {
            return studyManager.GetNextChapter();
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
