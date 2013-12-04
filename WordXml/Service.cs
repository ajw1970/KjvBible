using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BibleModel;
using System.Windows.Forms;

namespace WordXml
{
    public class Service
    {
        private static readonly XNamespace nsPkg = "http://schemas.microsoft.com/office/2006/xmlPackage";
        private static readonly XNamespace nsW = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

        public static Binder GetBibleNotes(Binder bible)
        {
            MergeNotes(bible.Books, @"D:\Users\jwelty\Documents\S8W\OTBIBLEKJV-John Custom.xml");
            MergeNotes(bible.Books, @"D:\Users\jwelty\Documents\S8W\NTBIBLEKJV-John Custom.xml");
            return bible;
        }

        private static void MergeNotes(List<Book> bible, string sourceXml)
        {
            var doc = XDocument.Load(sourceXml);
            Book book = null;
            Chapter chapter = null;
            Verse verse = null;
            var state = new NotesState();
            var changes = new Dictionary<string, string>();

            var paragraphs = doc.Descendants(nsPkg + "part")
                .Where(d => d.Attribute(nsPkg + "name").Value == "/word/document.xml")
                .Descendants(nsW + "p");

            foreach (var p in paragraphs)
            {
                state.IsNewParagraph = true;
                state.IsVerseText = false;
                state.ParagraphModifier = VerseTextModifier.None;

                foreach (var element in p.Elements())
                {
                    switch (element.Name.LocalName)
                    {
                        case "bookmarkStart":
                        case "bookmarkEnd":
                            continue;
                        case "hyperlink":
                            ProcessHyperlink(element, bible, ref book, ref chapter, ref verse, ref state);
                            break;
                        case "r":
                            ProcessRun(element, bible, ref book, ref chapter, ref verse, ref state);
                            break;
                        case "rPr":
                        case "pPr":
                            ProcessPr(element, ref book, ref chapter, ref verse, ref state);
                            break;
                        case "smartTag":
                            ProcessSmartTag(element, bible, ref book, ref chapter, ref verse, ref state);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }

        private static void ProcessHyperlink(XElement link, List<Book> bible, ref Book book, ref Chapter chapter, ref Verse verse, ref NotesState state)
        {
            foreach (var attribute in link.Attributes())
            {
                switch (attribute.Name.LocalName)
                {
                    case "history":
                    case "id":
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            foreach (var element in link.Elements())
            {
                switch (element.Name.LocalName)
                {
                    case "r":
                        ProcessRun(element, bible, ref book, ref chapter, ref verse, ref state);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private static void ProcessSmartTag(XElement smartTag, List<Book> bible, ref Book book, ref Chapter chapter, ref Verse verse, ref NotesState state)
        {
            foreach (var attribute in smartTag.Attributes())
            {
                switch (attribute.Name.LocalName)
                {
                    case "element":
                        switch (attribute.Value)
                        {
                            case "address":
                                break;
                            case "City":
                                state.RunModifier = state.RunModifier | VerseTextModifier.MstCity;
                                break;
                            case "country-region":
                                state.RunModifier = state.RunModifier | VerseTextModifier.MstCountryRegion;
                                break;
                            case "date":
                                break;
                            case "PersonName":
                                state.RunModifier = state.RunModifier | VerseTextModifier.MstPersonName;
                                break;
                            case "place":
                            case "PlaceType":
                                break;
                            case "PlaceName":
                                state.RunModifier = state.RunModifier | VerseTextModifier.MstPlaceName;
                                break;
                            case "State":
                                state.RunModifier = state.RunModifier | VerseTextModifier.MstState;
                                break;
                            case "Street":
                            case "time":
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    case "uri":
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            foreach (var element in smartTag.Elements())
            {
                switch (element.Name.LocalName)
                {
                    case "r":
                        ProcessRun(element, bible, ref book, ref chapter, ref verse, ref state);
                        break;
                    case "smartTag":
                        ProcessSmartTag(element, bible, ref book, ref chapter, ref verse, ref state);
                        break;
                    case "smartTagPr":
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private static void ProcessRun(XElement run, List<Book> bible, ref Book book, ref Chapter chapter, ref Verse verse, ref NotesState state)
        {
            state.RunModifier = state.ParagraphModifier;

            foreach (var element in run.Elements())
            {
                switch (element.Name.LocalName)
                {
                    case "br":
                    case "cr":
                        ProcessText(Environment.NewLine, ref chapter, ref verse, ref state);
                        break;
                    case "rPr":
                        ProcessPr(element, ref book, ref chapter, ref verse, ref state);
                        break;
                    case "t":
                        byte vNum;
                        if (state.InChapterHeading)
                        {
                            var tParts = element.Value.Split(' ');
                            bool foundBook = false;
                            bool foundChapter = false;
                            int cNum;
                            int tpUB = tParts.Length;
                            string bookName = String.Empty;
                            if (int.TryParse(tParts.Last(), out cNum))
                            {
                                tpUB--;
                            }
                            else
                            {
                                cNum = 1;
                            }

                            for (int i = 0; i < tpUB; i++)
                            {
                                bookName += tParts[i];
                                if (i < tpUB - 1)
                                    bookName += " ";
                            }

                            if (book == null || book.Name != bookName)
                            {
                                switch (bookName)
                                {
                                    case "Psalm":
                                        bookName = bookName + 's';
                                        break;
                                    case "Song of Songs":
                                        bookName = "Song of Solomon";
                                        break;
                                    default:                                        
                                        break;
                                }
                                var books = bible.Where(b => b.Name == bookName);
                                if (books.Count() == 1)
                                {
                                    book = books.First();
                                    foundBook = true;
                                }
                                else
                                {
                                    throw new NotImplementedException();
                                }
                            }
                            else if (book != null)
                            {
                                foundBook = true;
                            }

                            var chapters = book.Chapters.Where(c => c.Number == cNum);
                            if (chapters.Count() == 1)
                            {
                                chapter = chapters.First();
                                foundChapter = true;
                                verse = null;
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }

                            if (foundBook && foundChapter)
                            {
                                state.InChapterHeading = false;
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                        else if (state.InVerseHeading && byte.TryParse(element.Value, out vNum))
                        {
                            verse = chapter.Verses.Where(v => v.Number == vNum).FirstOrDefault();
                            if (verse != null)
                            {
                                state.InVerseHeading = false;
                                state.IsVerseText = true;
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                        else
                        {
                            ProcessText(element.Value, ref chapter, ref verse, ref state);
                        }
                        break;
                    case "tab":
                        ProcessText("    ", ref chapter, ref verse, ref state);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private static void ProcessText(string val, ref Chapter chapter, ref Verse verse, ref NotesState state)
        {
            if (verse == null)
            {
                chapter.AddSubHeadingTexts(new VerseText()
                {
                    Modifier = state.RunModifier,
                    Value = val
                });
            }
            else if (state.IsVerseText)
            {
                verse.AddNotesVerseTexts(new VerseText()
                {
                    Modifier = state.RunModifier,
                    Value = verse.NotesVerseTexts.Count == 0 ? val.TrimStart() : val
                });
            }
            else
            {
                verse.AddNotesTexts(new VerseText()
                {
                    Modifier = state.RunModifier,
                    Value = val
                }, state.IsNewParagraph);
            }
            state.IsNewParagraph = false;
        }
        private static void ProcessPr(XElement pr, ref Book book, ref Chapter chapter, ref Verse verse, ref NotesState state)
        {
            foreach (var element in pr.Elements())
            {
                switch (element.Name.LocalName)
                {
                    case "b":
                    case "bCs":
                        if (element.Parent.Name.LocalName == "rPr")
                        {
                            state.RunModifier = state.RunModifier | VerseTextModifier.AjwBold;
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                        break;
                    case "bdr":
                        if (element.Parent.Name.LocalName == "rPr")
                        {
                            state.RunModifier = state.RunModifier | VerseTextModifier.AjwBorder;
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                        break;
                    case "color":
                        break;
                    case "highlight":
                        var color = element.Attributes().Where(a => a.Name.LocalName == "val").FirstOrDefault().Value;
                        switch (color)
                        {
                            case "cyan":
                                state.RunModifier = state.RunModifier | VerseTextModifier.AjwHighlightCyan;
                                break;
                            case "darkCyan":
                                state.RunModifier = state.RunModifier | VerseTextModifier.AjwHighlightDarkCyan;
                                break;                            
                            case "green":
                                state.RunModifier = state.RunModifier | VerseTextModifier.AjwHighlightGreen;
                                break;
                            case "lightGray":
                                state.RunModifier = state.RunModifier | VerseTextModifier.AjwHighlightLightGray;
                                break;
                            case "magenta":
                                state.RunModifier = state.RunModifier | VerseTextModifier.AjwHighlightMagenta;
                                break;
                            case "darkMagenta":
                                state.RunModifier = state.RunModifier | VerseTextModifier.AjwHighlightDarkMagenta;
                                break;
                            case "red":
                                state.RunModifier = state.RunModifier | VerseTextModifier.AjwHighlightRed;
                                break;
                            case "yellow":
                                state.RunModifier = state.RunModifier | VerseTextModifier.AjwHighlightYellow;
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    case "i":
                    case "iCs":
                        if (element.Parent.Name.LocalName == "rPr")
                        {
                            state.RunModifier = state.RunModifier | VerseTextModifier.AjwItalic;
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                        break;
                    case "ind":
                        if (element.Parent.Name.LocalName == "pPr")
                        {
                            state.ParagraphModifier = state.ParagraphModifier | VerseTextModifier.AjwIndented;
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                        break;
                    case "jc":
                    case "keepLines":
                    case "keepNext":
                        break;
                    case "lang":
                        break;
                    case "numPr":
                        if (element.Parent.Name.LocalName == "pPr")
                        {
                            state.ParagraphModifier = state.ParagraphModifier | VerseTextModifier.AjwBullet;
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                        break;
                    case "pBdr":
                        if (element.Parent.Name.LocalName == "pPr")
                        {
                            state.ParagraphModifier = state.ParagraphModifier | VerseTextModifier.AjwBorder;
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                        break;
                    case "rFonts":
                        break;
                    case "rPr":
                        ProcessPr(element, ref book, ref chapter, ref verse, ref state);
                        break;
                    case "rStyle":
                    case "pStyle":
                        ProcessStyle(element, ref book, ref chapter, ref verse, ref state);
                        break;
                    case "shd":
                    case "sz":
                    case "szCs":
                    case "sectPr":
                    case "snapToGrid":
                    case "spacing":
                    case "tabs":
                        break;
                    case "u":
                        state.RunModifier = state.RunModifier | VerseTextModifier.AjwUnderline;
                        break;
                    case "vertAlign":
                        ProcessVertAlign(element, ref book, ref chapter, ref verse, ref state);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private static void ProcessVertAlign(XElement vertAlign, ref Book book, ref Chapter chapter, ref Verse verse, ref NotesState state)
        {
            foreach (var attribute in vertAlign.Attributes())
            {
                switch (attribute.Name.LocalName)
                {
                    case "val":
                        switch (attribute.Value)
                        {
                            case "superscript":
                                state.InVerseHeading = true;
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            foreach (var element in vertAlign.Elements())
            {
                switch (element.Name.LocalName)
                {
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private static void ProcessStyle(XElement style, ref Book book, ref Chapter chapter, ref Verse verse, ref NotesState notesState)
        {
            foreach (var attribute in style.Attributes())
            {
                switch (attribute.Name.LocalName)
                {
                    case "val":
                        switch (attribute.Value)
                        {        
                            case "apple-converted-space":
                            case "apple-style-span":
                                break;
                            case "Heading3":
                                notesState.InChapterHeading = true;
                                break;
                            case "HTMLTypewriter":
                            case "Hyperlink":
                                break;
                            case "NormalNote":
                            case "NormalNoteChar":
                            case "Notes":
                            case "NotesChar":
                            case "NotesChar1":
                                if (style.Parent.Name.LocalName == "rPr")
                                {
                                    notesState.RunModifier = notesState.RunModifier | VerseTextModifier.AjwNote;
                                }
                                else if (style.Parent.Name.LocalName == "pPr")
                                {
                                    notesState.ParagraphModifier = notesState.ParagraphModifier | VerseTextModifier.AjwNote;
                                }
                                else
                                {
                                    throw new NotImplementedException();
                                }
                                break;
                            case "Quote":
                                if (style.Parent.Name.LocalName == "rPr")
                                {
                                    notesState.RunModifier = notesState.RunModifier | VerseTextModifier.AjwQuote;
                                }
                                else
                                {
                                    notesState.ParagraphModifier = notesState.ParagraphModifier | VerseTextModifier.AjwQuote;
                                }
                                break;
                            case "StyleQuoteUnderlineChar":
                                if (style.Parent.Name.LocalName == "rPr")
                                {
                                    notesState.RunModifier = notesState.RunModifier | VerseTextModifier.AjwQuote | VerseTextModifier.AjwUnderline;
                                }
                                else
                                {
                                    notesState.ParagraphModifier = notesState.ParagraphModifier | VerseTextModifier.AjwQuote | VerseTextModifier.AjwUnderline;
                                }
                                break;
                            case "StyleQuoteBold":
                            case "StyleQuoteBoldChar":
                            case "StyleQuote8ptBold1Char":
                                if (style.Parent.Name.LocalName == "rPr")
                                {
                                    notesState.RunModifier = notesState.RunModifier | VerseTextModifier.AjwQuote | VerseTextModifier.AjwBold;
                                }
                                else
                                {
                                    notesState.ParagraphModifier = notesState.ParagraphModifier | VerseTextModifier.AjwQuote | VerseTextModifier.AjwBold;
                                }
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            foreach (var element in style.Elements())
            {
                switch (element.Name.LocalName)
                {
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private static List<VerseText> ReconcileVerses(Book book, Chapter chapter, Verse bibleVerse, Verse notesVerse, Dictionary<string, string> changes)
        {
            if (bibleVerse.Text == notesVerse.Text)
            {
                return bibleVerse.Texts;
            }
            else
            {
                var newTexts = new List<VerseText>();
                var biblePos = new ReconciliationVerse(bibleVerse);
                var notesPos = new ReconciliationVerse(notesVerse);

                do
                {
                    throw new NotImplementedException();
                    //if (notesPos.Text != null && (notesPos.Text.Modifier.HasFlag(VerseTextModifier.AjwHighlight) ||
                    //    notesPos.Text.Modifier.HasFlag(VerseTextModifier.AjwNote)))
                    //{
                    //    if (newTexts.Count > 0 && newTexts.Last().Modifier == notesPos.Text.Modifier)
                    //    {
                    //        newTexts.Last().Value += notesPos.TakeText().Value;
                    //    }
                    //    else
                    //    {
                    //        newTexts.Add(notesPos.TakeText());
                    //    }
                    //}
                    //else if (notesPos.Value == biblePos.Value)
                    //{
                    //    if (newTexts.Count > 0 && newTexts.Last().Modifier == notesPos.Texts[notesPos.TextPos].Modifier)
                    //    {
                    //        newTexts.Last().Value += notesPos.TakeText().Value;
                    //    }
                    //    else
                    //    {
                    //        newTexts.Add(notesPos.TakeText());
                    //    }
                    //    biblePos.TextPos++;
                    //}
                    //else if (notesPos.IsWord && biblePos.IsWord)
                    //{
                    //    if (changes.ContainsKey(biblePos.Value))
                    //    {
                    //        if (changes[biblePos.Value] == notesPos.Value)
                    //        {
                    //            var tmpText = notesPos.TakeText();
                    //            tmpText.Modifier = tmpText.Modifier | VerseTextModifier.AjwCorrection;
                    //            tmpText.Type = biblePos.TakeText().Value;
                    //            newTexts.Add(tmpText);
                    //        }
                    //        else
                    //        {
                    //            throw new NotImplementedException();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (MessageBox.Show(String.Format("Replace ({0}) with ({1}) in {2} {3}:{4}?" +
                    //            Environment.NewLine + Environment.NewLine + "Kjv:" + Environment.NewLine + bibleVerse.Text +
                    //            Environment.NewLine + Environment.NewLine + "Notes:" + Environment.NewLine + notesVerse.Text,
                    //            biblePos.Value, notesPos.Value,
                    //            book.Name, chapter.Number, bibleVerse.Number),
                    //            "Confirm Replacment", MessageBoxButtons.YesNo,
                    //            MessageBoxIcon.Question) == DialogResult.Yes)
                    //        {
                    //            changes.Add(biblePos.Value, notesPos.Value);

                    //            var tmpText = notesPos.TakeText();
                    //            tmpText.Modifier = tmpText.Modifier | VerseTextModifier.AjwCorrection;
                    //            tmpText.Type = biblePos.TakeText().Value;
                    //            newTexts.Add(tmpText);
                    //        }
                    //        else
                    //        {
                    //            throw new NotImplementedException();
                    //        }
                    //    }
                    //}
                    //else if (!notesPos.IsWord && !biblePos.IsWord)
                    //{
                    //    if (notesPos.Value.Length >= biblePos.Value.Length)
                    //    {
                    //        newTexts.Add(notesPos.TakeText());
                    //        biblePos.TextPos++;
                    //    }
                    //    else
                    //    {
                    //        newTexts.Add(biblePos.TakeText());
                    //        notesPos.TextPos++;
                    //    }
                    //}
                    //else if (notesPos.EOF && !biblePos.EOF)
                    //{
                    //    if (!biblePos.IsWord)
                    //    {
                    //        if (newTexts.Last().Value.Trim().EndsWith(biblePos.Value.Trim()))
                    //        {
                    //            biblePos.TextPos++;
                    //        }
                    //        else
                    //        {
                    //            throw new NotImplementedException();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        throw new NotImplementedException();
                    //    }
                    //}
                    //else
                    //{
                    //    throw new NotImplementedException();
                    //}
                } while (!notesPos.EOF || !biblePos.EOF);

                return newTexts;
            }
        }

        private static bool CheckVerseHeading(Chapter chapter, ref Verse verse, XElement firstRun)
        {
            var verseNumber = firstRun.Descendants(nsW + "vertAlign")
                .Where(a => a.Attribute(nsW + "val").Value == "superscript")
                .FirstOrDefault();
            if (verseNumber != null)
            {
                verse = chapter.Verses.Where(v => v.Number == byte.Parse(firstRun.Value)).FirstOrDefault();
                if (verse != null)
                {
                    return true;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                return false;
            }
        }

        private static bool CheckChapterHeading(List<Book> bible, ref Book book, ref Chapter chapter, XElement p)
        {
            var head3s = p.Descendants(nsW + "pStyle")
                                .Where(d => d.Attribute(nsW + "val").Value == "Heading3");
            if (head3s.Count() == 1)
            {
                var head3 = head3s.FirstOrDefault();
                if (head3 != null)
                {
                    var texts = p.Descendants(nsW + "t");
                    if (texts.Count() == 1)
                    {
                        var text = texts.FirstOrDefault();
                        //we've got a book chapter heading here
                        var tParts = text.Value.Split(' ');
                        int cNum;
                        int tpUB = tParts.Length;
                        string bookName = String.Empty;
                        if (int.TryParse(tParts.Last(), out cNum))
                        {
                            tpUB--;
                        }
                        else
                        {
                            cNum = 1;
                        }

                        for (int i = 0; i < tpUB; i++)
                        {
                            bookName += tParts[i];
                            if (i < tpUB - 1)
                                bookName += " ";
                        }

                        if (book == null || book.Name != bookName)
                        {
                            var books = bible.Where(b => b.Name == bookName);
                            if (books.Count() == 1)
                            {
                                book = books.First();
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }

                        var chapters = book.Chapters.Where(c => c.Number == cNum);
                        if (chapters.Count() == 1)
                        {
                            chapter = chapters.First();
                            return true;
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                    else if (texts.Count() > 1)
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        //this was an empty header... we'll return true to avoid processing it as a verse.
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (head3s.Count() > 1)
            {
                throw new NotImplementedException();
            }
            else
            {
                return false;
            }
        }
    }
}
