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
    public override void TranslateStrings(ref StringCollection Strings)
    {
        string S;
        int EndOfLine;
        int LineFeed;
        if (Strings.Count != 0)
        {
            S = "";
            for (int I = 0, last = Strings.Count - 1; I <= last; I++)
            {
                S = S + Strings[I];
                if (I != Strings.Count - 1)
                {
                    S = S + "<eol>";
                }
            }

            S = FDictionary.Translate(S);
            Strings.Clear();
            do
            {
                EndOfLine = Pos("<eol>", S);
                LineFeed = Pos("\n", S);
                if ((EndOfLine < LineFeed || LineFeed == 0) && EndOfLine != 0)
                {
                    Strings.Add(Copy(S, 1, EndOfLine - 1));
                    S = Copy(S, EndOfLine + 5, S.Length - EndOfLine - 4);
                }
                else
                {
                    if ((LineFeed < EndOfLine || EndOfLine == 0) && LineFeed != 0)
                    {
                        Strings.Add(Copy(S, 1, LineFeed - 1));
                        // { Original reverses the length comparison before reading the following CR. }
                        if (S.Length < LineFeed + 1 && S[LineFeed] == 13)
                        {
                            LineFeed += 1;
                        }

                        S = Copy(S, LineFeed + 1, S.Length - LineFeed);
                    }
                    else
                    {
                        Strings.Add(S);
                        S = "";
                    }
                }
            }
            while (!IsEmpty(S));
        }
    }

    // Army.PAS:5609
    public override void TranslateString(ref string S)
    {
        S = FDictionary.Translate(S);
    }

    // Engine.pas:1343
    public TranslateTextStream(GameEngine AEngine) : base(AEngine)
    { }
}
