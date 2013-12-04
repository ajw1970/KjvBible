using System;
using System.Collections.Generic;
using System.Linq;

namespace BibleModel
{
    public class VerseText
    {
        private string value = string.Empty;
        public string Value
        {
            get
            {
                return value.Replace(Environment.NewLine,"");
            }
            set
            {
                this.value = value;
            }
        }
        public VerseTextModifier Modifier { get; set; }

        private string type = string.Empty;
        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
                switch (type)
                {
                    case null:
                        Modifier = Modifier | VerseTextModifier.None;
                        break;
                    case "added":
                        Modifier = Modifier | VerseTextModifier.TranslatorAdded;
                        break;
                    //default:
                    //    throw new NotImplementedException();
                }
            }
        }
    }
}
