using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BibleModel;

namespace WordXml
{
    class NotesState
    {
        public bool InChapterHeading { get; set; }
        public bool InVerseHeading { get; set; }
        public bool IsVerseText { get; set; }
        public bool IsNewParagraph { get; set; }
        public VerseTextModifier ParagraphModifier { get; set; }
        public VerseTextModifier RunModifier { get; set; }
    }
}
