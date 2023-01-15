using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace ScriptureReferenceParser
{
    public class BibleReferenceParser : IBibleReferenceParser
    {
        public (string First, string Last) ParseBookRange(string bookRange)
        {
            var splitBooks = bookRange.Split('-');
            if (splitBooks.Length > 2)
            {
                throw new ArgumentException("Expecting hyphenated set of books: \"first-last\"", nameof(bookRange));
            }
            if (splitBooks.Length == 1)
            {
                return (splitBooks[0], string.Empty);
            }
            return (splitBooks[0], splitBooks[1]);
        }

        public (string Book, int Chapter) ParseChapter(string chapterReference)
        {
            var chapterReferenceBeforeColon = chapterReference.Split(':')[0];

            var splitChapter = chapterReferenceBeforeColon
                .Trim()
                .Split(' ');

            var length = splitChapter.Length;
            if (length == 1)
            {
                throw new ArgumentException("Expecting a space between book and chapterReference", nameof(chapterReference));
            }

            int chapter;
            if (!int.TryParse(splitChapter[length - 1], out chapter))
            {
                throw new ArgumentException("Expecting a whole number after book name", nameof(chapterReference));
            }
            
            //rejoin split strings prior to chapter number to get book reference (ie: "1 John"
            var book = string.Join(" ", splitChapter.Take(length - 1));

            return (book, chapter);
        }
    }
}
