using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BibleModel
{
    public class Verse
    {
        public byte Number { get; set; }
        public List<VerseText> Texts { get; set; }              // From OsisBible
        public List<VerseText> NotesVerseTexts { get; set; }    // From Word Bible (verse with embedded notes)
        private List<List<VerseText>> notesTexts;
        public List<List<VerseText>> NotesTexts                       // From Word Bible (verse sub notes)
        {
            get {
                if (notesTexts == null)
                {
                    notesTexts = new List<List<VerseText>>();
                }
                return notesTexts; 
            }
        }

        public void AddTexts(VerseText vText)
        {
            if (Texts.Count > 0)
            {
                var lastText = Texts.Last();
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
                    Texts.Add(vText);
                }
            }
            else
            {
                Texts.Add(vText);
            }
        }

        public void AddNotesVerseTexts(VerseText vText)
        {
            if (NotesVerseTexts.Count > 0)
            {
                var lastText = NotesVerseTexts.Last();
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
                    NotesVerseTexts.Add(vText);
                }
            }
            else
            {
                NotesVerseTexts.Add(vText);
            }
        }

        public void AddNotesTexts(VerseText vText, bool NewParagraph = false)
        {
            if (NotesTexts.Count > 0)
            {
                if (NewParagraph)
                    NotesTexts.Add(new List<VerseText> { vText });
                else
                {
                    var lastText = NotesTexts.Last().Last();
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
                        NotesTexts.Last().Add(vText);
                    }
                }
            }
            else
            {
                NotesTexts.Add(new List<VerseText> { vText });
            }
        }

        public string Text
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var text in Texts)
                {
                    sb.Append(text.Value);
                }
                return sb.ToString().Trim();
            }
        }

        public Verse()
        {
            Texts = new List<VerseText>();
            NotesVerseTexts = new List<VerseText>();
        }
    }
}