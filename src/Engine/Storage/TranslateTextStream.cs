// Translated from AoWE.pas.
using Aow.Game;

using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Storage;

public partial class TranslateTextStream : TranslateStorageStream
{
    public IvDictionary FDictionary;

    // Army.PAS:5614
    public override void TranslateStrings(ref StringCollection lines)
    {
        string text;
        int endOfLine;
        int lineFeed;
        if (lines.Count != 0)
        {
            text = "";
            for (int i = 0, last = lines.Count - 1; i <= last; i++)
            {
                text = text + lines[i];
                if (i != lines.Count - 1)
                {
                    text = text + "<eol>";
                }
            }

            text = FDictionary.Translate(text);
            lines.Clear();
            do
            {
                endOfLine = Pos("<eol>", text);
                lineFeed = Pos("\n", text);
                if ((endOfLine < lineFeed || lineFeed == 0) && endOfLine != 0)
                {
                    lines.Add(Copy(text, 1, endOfLine - 1));
                    text = Copy(text, endOfLine + 5, text.Length - endOfLine - 4);
                }
                else
                {
                    if ((lineFeed < endOfLine || endOfLine == 0) && lineFeed != 0)
                    {
                        lines.Add(Copy(text, 1, lineFeed - 1));
                        // { Original reverses the length comparison before reading the following CR. }
                        if (text.Length < lineFeed + 1 && text[lineFeed] == 13)
                        {
                            lineFeed += 1;
                        }

                        text = Copy(text, lineFeed + 1, text.Length - lineFeed);
                    }
                    else
                    {
                        lines.Add(text);
                        text = "";
                    }
                }
            }
            while (!IsEmpty(text));
        }
    }

    // Army.PAS:5609
    public override void TranslateString(ref string value)
    {
        value = FDictionary.Translate(value);
    }

    // Engine.pas:1343
    public TranslateTextStream(GameEngine engine) : base(engine)
    { }
}
