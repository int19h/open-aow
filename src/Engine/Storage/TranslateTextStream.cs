#nullable disable
// Translated from AoWE.pas.
using Aow.Game;

using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Engine.Storage;

public class TranslateTextStream(GameEngine engine) : TranslateVisit(engine)
{
    public IvDictionary FDictionary { get; internal set; }

    // Army.PAS:5614
    public override void TranslateStrings(DelphiStringCollection lines)
    {
        if (lines.Count != 0)
        {
            string text = "";
            for (int i = 0, last = lines.Count - 1; i <= last; i++)
            {
                text += lines[i];
                if (i != lines.Count - 1)
                {
                    text += "<eol>";
                }
            }

            text = FDictionary.Translate(text);
            lines.Clear();
            do
            {
                int endOfLine = Pos("<eol>", haystack: text);
                int lineFeed = Pos("\n", haystack: text);
                if ((endOfLine < lineFeed || lineFeed == 0) && endOfLine != 0)
                {
                    _ = lines.Add(Copy(text, index: 1, count: endOfLine - 1));
                    text = Copy(text, index: endOfLine + 5, count: text.Length - endOfLine - 4);
                }
                else
                {
                    if ((lineFeed < endOfLine || endOfLine == 0) && lineFeed != 0)
                    {
                        _ = lines.Add(Copy(text, index: 1, count: lineFeed - 1));
                        // { Original reverses the length comparison before reading the following CR. }
                        if (text.Length < lineFeed + 1 && text[lineFeed] == 13)
                        {
                            lineFeed += 1;
                        }

                        text = Copy(text, index: lineFeed + 1, count: text.Length - lineFeed);
                    }
                    else
                    {
                        _ = lines.Add(text);
                        text = "";
                    }
                }
            }
            while (!IsEmpty(text));
        }
    }

    // Army.PAS:5609
    public override void TranslateString(ref string value) => value = FDictionary.Translate(value);
}
