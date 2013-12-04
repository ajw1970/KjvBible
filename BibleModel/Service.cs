using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BibleModel;

namespace KjvBible
{
    public static class Service
    {
        public static Binder GetBible()
        {
            return Osis.Service.GetBible();
        }
    }
}
