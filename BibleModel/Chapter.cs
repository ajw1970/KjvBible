using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BibleModel
{
    public class Chapter
    {
        public byte Number { get; set; }

        private List<VerseText> subHeadingTexts;
        public List<VerseText> SubHeadingTexts {
            get {
                if (subHeadingTexts == null)
                {
                    subHeadingTexts = new List<VerseText>();
                }
                return subHeadingTexts; 
            }
        }

        public void AddSubHeadingTexts(VerseText vText)
        {
            if (SubHeadingTexts.Count > 0)
            {
                var lastText = SubHeadingTexts.Last();
                if (lastText.Modifier == vText.Modifier)
                {
                    lastText.Value += vText.Value;
                    if (lastText.Type.Length == 0 && vText.Type.Length > 0)
                    {
                        lastText.Type = vText.Type;
                    }
                }
                else
                {
                    SubHeadingTexts.Add(vText);
                }
            }
            else
            {
                SubHeadingTexts.Add(vText);
            }
        }

        public List<Verse> Verses { get; set; }

        public Chapter()
        {
            Verses = new List<Verse>();
        }
    }
}
