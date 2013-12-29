using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BibleModel;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;

namespace KjvBible.Osis
{
    public static class Service
    {
        //private static int _QuoteCount;
        //private static string _InQuote;
        private static readonly XNamespace _Namespace = "http://www.bibletechnologies.net/2003/OSIS/namespace";

        public static Binder GetBible()
        {
            var osisXml = File.ReadAllText(@"C:\VS Projects\Personal\KjvBible\Data\kjv.osis.xml");
            return GetBible(osisXml);
        }

        public static Binder GetBible(string osisXml)
        {
            Quote quote = null;
            var bible = new Binder();
            bible.BookGroups.Add(new BookGroup() { Name = "Old Testament"});
            bible.BookGroups.Add(new BookGroup() { Name = "Apocrypha" });
            bible.BookGroups.Add(new BookGroup() { Name = "New Testament" });

            var doc = XDocument.Parse(osisXml);

            //var divs = from b in doc.Descendants("div")
            //            where b.Attribute("type").Value == "book"
            //            select b;

            var divs = doc.Descendants(_Namespace + "div")
                .Where(d => d.Attribute("type").Value == "book");
            foreach (var div in divs)
            {
                var book = new Book() { AbbreviatedName = (string)div.Attribute("osisID") };
                var chapter = new Chapter();
                var verse = new Verse();

                foreach (var element in div.Elements())
                {
                    switch (element.Name.LocalName)
                    {
                        case "title":
                            book.LongName = element.Value;
                            book.Name = (string)element.Attribute("short");
                            break;
                        case "chapter":
                            chapter = EvaluateChapter(book, chapter, element);
                            break;
                        case "p":
                            EvaluateVerseGroup(book, ref chapter, ref verse, ref quote, element);
                            break;
                        case "lg":
                            foreach (var l in element.Descendants(_Namespace + "l"))
                            {
                                EvaluateVerseGroup(book, ref chapter, ref verse, ref quote, l);
                            }
                            break;
                        case "verse":
                            EvaluateVerse(book, ref chapter, ref verse, element);
                            break;
                        default:
                            Console.WriteLine(element.Name);
                            throw new NotImplementedException();
                    }
                }

                //var paragraphs = div.Descendants("p");
                //foreach (var p in paragraphs)
                //{
                //    //Console.WriteLine(paragraph.Value);
                //    
                //}

                if (verse.Number > 0 || verse.Texts.Count > 0)
                {
                    throw new NotImplementedException();
                }

                //book.Chapters.Add(chapter);
                if (chapter.Number > 0)
                {
                    throw new NotImplementedException();
                }

                AddBookToGroup(bible, book);

                if (quote == null)
                {
                    Quote.Count = 0;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            return bible;
        }

        private static void AddBookToGroup(Binder bible, Book book)
        {
            if (bible.BookGroups[0].Books.Count() < 39)
            {
                bible.BookGroups[0].Books.Add(book);
                book.Parent = bible.BookGroups[0];
            }
            else if (bible.BookGroups[1].Books.Count() < 15)
            {
                bible.BookGroups[1].Books.Add(book);
                book.Parent = bible.BookGroups[1];
            }
            else
            {
                bible.BookGroups[2].Books.Add(book);
                book.Parent = bible.BookGroups[1];
            }
        }

        private static void EvaluateVerseGroup(Book book, ref Chapter chapter, ref Verse verse, ref Quote quote, XElement element)
        {
            foreach (var node in element.DescendantNodes())
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (((XElement)node).Name.LocalName)
                        {
                            case "verse":
                                EvaluateVerse(book, ref chapter, ref verse, node);
                                break;
                            case "chapter":
                                chapter = EvaluateChapter(book, chapter, (XElement)node);
                                break;
                            case "transChange":
                                // We pull this in by evaluating node.Parent within XmlNodeType.Text case below
                                break;
                            case "q":
                                var n = (string)((XElement)node).Attribute("n");
                                if (n != "")
                                {
                                    throw new NotImplementedException();
                                }
                                var sID = (string)((XElement)node).Attribute("sID");
                                var eID = (string)((XElement)node).Attribute("eID");
                                if (sID != null && eID == null)
                                {
                                    if (quote == null)
                                    {
                                        //Quote.Count++;
                                        quote = new Quote();
                                        quote.Text = string.Format("{0}.{1}.{2}.{3}", book.AbbreviatedName, chapter.Number, verse.Number, Quote.Count);
                                        if (sID != quote.Text)
                                        {
                                            throw new NotImplementedException();
                                        }
                                        quote.Who = (string)((XElement)node).Attribute("who");
                                        if (quote.Who != "Jesus")
                                        {
                                            throw new NotImplementedException();
                                        }
                                        quote.Type = (string)((XElement)node).Attribute("type");
                                        if (quote.Type != "x-doNotGeneratePunctuation")
                                        {
                                            throw new NotImplementedException();
                                        }

                                        //verse.Texts.Add(new VerseText()
                                        //{
                                        //    Value = "\"",
                                        //    Type = node.ToString(SaveOptions.None)
                                        //});
                                    }
                                    else
                                    {
                                        throw new NotImplementedException();
                                    }
                                }
                                else if (eID != null && sID == null)
                                {
                                    if (eID == quote.Text)
                                    {
                                        quote = null;
                                        //verse.Texts.Add(new VerseText()
                                        //{
                                        //    Value = "\"",
                                        //    Type = node.ToString(SaveOptions.None)
                                        //});
                                    }
                                    else
                                    {
                                        throw new NotImplementedException();
                                    }
                                }
                                else
                                {
                                    throw new NotImplementedException();
                                }

                                //var verseText = new VerseText()
                                //{
                                //    Value = "\"",
                                //    Type = node.ToString(SaveOptions.None)
                                //};

                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    case XmlNodeType.Text:
                        if (verse.Number > 0)
                        {
                            if (Regex.IsMatch(node.Parent.Name.LocalName, "^(p|l|transChange)"))
                            {
                                verse.AddTexts(BuildVerseText(ref quote, node));
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                        else if (verse.Number == 0)
                        {
                            if (Regex.IsMatch(node.Parent.Name.LocalName, "^(p|transChange)"))
                            {
                                chapter.AddSubHeadingTexts(BuildVerseText(ref quote, node));
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private static void EvaluateVerse(Book book, ref Chapter chapter, ref Verse verse, XNode node)
        {
            var sID = (string)((XElement)node).Attribute("sID");
            var eID = (string)((XElement)node).Attribute("eID");
            if (sID != null && eID == null)
            {
                // Starting a verse
                var tID = sID.Split('.');
                if (book.AbbreviatedName == "")
                {
                    book.AbbreviatedName = tID[0];
                }

                var tChapter = byte.Parse(tID[1]);
                if (chapter.Number != tChapter)
                {
                    if (chapter.Number > 0)
                    {
                        book.Chapters.Add(chapter);
                        chapter = new Chapter();
                    }
                    chapter.Number = tChapter;
                }

                if (verse.Number == 0 && verse.Texts.Count == 0)
                {
                    verse.Number = byte.Parse(tID[2]);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else if (eID != null && sID == null)
            {
                // Ended a verse
                if (verse.Number > 0 && verse.Texts.Count > 0)
                {
                    chapter.Verses.Add(verse);
                    verse = new Verse();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static VerseText BuildVerseText(ref Quote quote, XNode node)
        {
            var verseText = new VerseText() { Value = node.ToString().Replace(Environment.NewLine," ") };
            if (node.Parent.Name.LocalName == "transChange")
            {
                verseText.Type = (string)node.Parent.Attribute("type");
                if (!verseText.Modifier.HasFlag(VerseTextModifier.TranslatorAdded))
                {
                    throw new NotImplementedException();
                }
            }
            if (quote != null)
            {
                if (quote.Who == "Jesus")
                {
                    verseText.Modifier = verseText.Modifier | VerseTextModifier.QuotingJesus;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            return verseText;
        }

        private static Chapter EvaluateChapter(Book book, Chapter chapter, XElement element)
        {
            var sID = (string)element.Attribute("sID");
            var eID = (string)element.Attribute("eID");
            if (sID != null && eID == null)
            {
                chapter.Number = byte.Parse(sID.Split('.')[1]);
            }
            else if (eID != null && sID == null)
            {
                book.Chapters.Add(chapter);
                chapter = new Chapter();
            }
            else
            {
                throw new NotImplementedException();
            }

            return chapter;
        }
    }
}
