using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BibleModel
{
    [Flags]
    public enum VerseTextModifier
    {
        None = 0,
        // Osis
        TranslatorAdded = 1,
        QuotingJesus = 2,
        // My notes
        AjwNote = 4,
        AjwCorrection = 8,
        AjwBorder = 16,
        AjwBold = 32,
        AjwQuote = 64,
        AjwBullet = 128,
        AjwItalic = 256,
        AjwIndented = 512,
        AjwUnderline = 1024,
        AjwHighlightCyan = 2048,
        AjwHighlightDarkCyan = 4096,
        AjwHighlightGreen = 8192,
        AjwHighlightLightGray = 16384,
        AjwHighlightMagenta = 32768,
        AjwHighlightDarkMagenta = 65536,
        AjwHighlightRed = 131072,
        AjwHighlightYellow = 262144,
        // Microsoft Word Smart Tags
        MstCountryRegion = 524288,
        MstCity = 1048576,
        MstState = 2097152,
        MstPlaceName = 4194304,
        MstPersonName = 8388608
    }
}
