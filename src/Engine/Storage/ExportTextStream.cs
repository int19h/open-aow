// Translated from AoWE.pas.
using Aow.Game;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Storage;

// { Interface declaration run owned by P2. }
public partial class ExportTextStream : EWriteTextStorageStream
{
    // Army.PAS:5685
    public override void rwString(ref string S, int ID)
    {
        string Buffer;
        if (!PropertyTable.PropertyExist(ID))
        {
            if (!IsEmpty(S))
            {
                Buffer = "[" + IntToStr(ID) + "]=" + "|" + S + "|" + "\r" + "\n";
                FBuffer.Write(Cp1252.Encode(Buffer).AsSpan().Slice(1 - 1, Buffer.Length));
                PropertyTable.Add(ID, 0);
            }
        }
        else
        {
            throw new DoublePrioriyIDException("Double priorityID found");
        }
    }

    // Army.PAS:5700
    public override void rwStrings(StringCollection Strings, int ID)
    {
        string S = "";
        string Buffer;
        if (!PropertyTable.PropertyExist(ID))
        {
            if (Strings.Count != 0)
            {
                for (int I = 0, last = Strings.Count - 1; I <= last; I++)
                {
                    S = S + Strings[I];
                    if (I != Strings.Count - 1)
                    {
                        S = S + "<eol>";
                    }
                }
                {
                    Buffer = "[" + IntToStr(ID) + "]=";
                    if (!IsEmpty(S))
                    {
                        Buffer = Buffer + "|" + S + "|" + "\r" + "\n";
                    }
                    else
                    {
                        Buffer = Buffer + "\r" + "\n";
                    }

                    FBuffer.Write(Cp1252.Encode(Buffer).AsSpan().Slice(1 - 1, Buffer.Length));
                    PropertyTable.Add(ID, 0);
                }
            }
        }
        else
        {
            throw new DoublePrioriyIDException("Double priorityID found");
        }
    }

    // Engine.pas:1327
    public ExportTextStream(Stream AStream, GameEngine AEngine) : base(AStream, AEngine)
    { }
}
