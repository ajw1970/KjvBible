using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BibleModel;

namespace WordXml
{
    class ReconciliationVerse : Verse
    {
        public int TextPos { get; set; }

        public string Value
        {
            get
            {
                if (EOF)
                {
                    return string.Empty;
                }
                else
                {
                    return Text.Value;
                }
            }
        }

        public bool IsWord
        {
            get
            {
                return isWord(Value);
            }
        }

        public new VerseText Text
        {
            get
            {
                if (EOF)
                {
                    return null; 
                }
                else
                {
                    return Texts[TextPos];
                }
            }
        }

        public VerseText TakeText()
        {
            if (EOF)
            {
                return null;
            }
            else
            {
                return Texts[TextPos++];
            }
        }

        public bool EOF
        {
            get
            {
                return (TextPos >= Texts.Count);
            }
        }

        public ReconciliationVerse(Verse Verse)
        {
            string str = string.Empty;
            bool? strIsWord = null;
            VerseText strText = null;

            Number = Verse.Number;
            foreach (var text in Verse.Texts)
            {
                throw new NotImplementedException();
                //if (text.Modifier.HasFlag(VerseTextModifier.AjwHighlight) ||
                //        text.Modifier.HasFlag(VerseTextModifier.AjwNote))
                //{
                //    if (strIsWord != null && str.Length > 0 && strText != null)
                //    {
                //        Texts.Add(new VerseText()
                //        {
                //            Value = str,
                //            Type = strText.Type,
                //            Modifier = strText.Modifier
                //        });
                //        str = string.Empty;
                //        strIsWord = null;
                //        strText = null;
                //    }
                //    Texts.Add(text);
                //}
                //else
                //{
                //    strText = text;
                //    foreach (var chr in text.Value)
                //    {
                //        if (isWord(chr.ToString()))
                //        {
                //            ProcessChar(false, ref str, ref strIsWord, strText, chr);
                //        }
                //        else
                //        {
                //            ProcessChar(true, ref str, ref strIsWord, strText, chr);
                //        }
                //    }                    
                //}
            }
            if (strIsWord != null && str.Length > 0)
            {
                Texts.Add(new VerseText()
                {
                    Value = str,
                    Type = strText.Type,
                    Modifier = strText.Modifier
                });
            }
            TextPos = 0;
        }

        private bool isWord(string str)
        {
            return !(System.Text.RegularExpressions.Regex.IsMatch(str, "[ '\",.:;]"));
        }

        private void ProcessChar(bool wantWord, ref string str, ref bool? strIsWord, VerseText text, char chr)
        {
            if (strIsWord == null || strIsWord == wantWord)
            {
                str += chr;
                strIsWord = wantWord;
            }
            else
            {
                Texts.Add(new VerseText()
                {
                    Value = str,
                    Type = text.Type,
                    Modifier = text.Modifier
                });
                str = chr.ToString();
                strIsWord = wantWord;
            }
        }
    }
}
