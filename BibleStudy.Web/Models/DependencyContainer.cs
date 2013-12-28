using BibleStudy.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;

namespace BibleStudy.Web.Models
{
    //http://www.asp.net/web-api/overview/extensibility/using-the-web-api-dependency-resolver
    public class DependencyContainer : IDependencyResolver
    {
        static readonly BibleStudyManager bibleStudyManager = new MockBibleStudyManager();

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(ChaptersController))
            {
                return new ChaptersController(bibleStudyManager);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        public void Dispose()
        {
            
        }
    }
}