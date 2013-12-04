using System;
using System.Collections.Generic;
using System.Linq;

namespace KjvBible.Osis
{
    public class Quote
    {
        public static int Count { get; set; }
        public string Text { get; set; }
        public string Who { get; set; }
        public string Type { get; set; }
        public Quote()
        {
            Count++;
        }
    }
}
