// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Game;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Storage;

// FPath, FSize, FSections, FText, FValues. The text form of the storage: "[:path:]" lines open
// the section of an object (its path is the ids of its parents joined with '\'), "[id]=value"
// lines are its properties. the section is split into FValues with the property table pointing
// at them
public partial class EReadTextStorageStream : EStorageStream
{
    internal string FPath = "";
    internal int FSize;
    internal StringList FSections;
    internal StringList FText;
    internal StringList FValues;

    // Engine.pas:10632
    internal void ReadStreamText()
    {
        string Line;
        StringList Text;
        int P = 0;
        int Start;
        int L;
        Text = FText;
        FSections.Clear();
        Text.BeginUpdate();
        try
        {
            if (FSize < 0) throw new ArgumentOutOfRangeException(paramName: null, "Negative string length");
            byte[] buffer = new byte[FSize];
            FStream.ReadBytes(buffer);
            Text.Clear();
            ReadOnlySpan<byte> text = buffer;
            if (!text.IsEmpty)
            {
                while ((P < text.Length ? text[P] : (byte)0) != 0)
                {
                    Start = P;
                    while (!Set<byte>.WithBounds(0, 255).With(0).With(10).With(13).Contains(P < text.Length ? text[P] : (byte)0))
                    {
                        P += 1;
                    }

                    Line = Cp1252.Decode(text[Start..P].ToArray());
                    if (!IsEmpty(Line))
                    {
                        L = Line.Length;
                        if (L >= 4 && Copy(Line, 1, 2) == "[:" && Copy(Line, L - 1, 2) == ":]")
                        {
                            var count = Text.Count;
                            FSections.AddObject(Copy(Line, 3, L - 4), IntToObject(count));
                        }
                        else
                        {
                            Text.Add(Line);
                        }
                    }

                    if ((P < text.Length ? text[P] : (byte)0) == 13)
                    {
                        P += 1;
                    }

                    if ((P < text.Length ? text[P] : (byte)0) == 10)
                    {
                        P += 1;
                    }
                }
            }
        }
        finally
        {
            Text.EndUpdate();
        }
    }

    // Engine.pas:10694
    internal void ReadTextBuffer()
    {
        int Index;
        int First;
        int Last;
        int I;
        int ID;
        int NewID;
        string Value;
        FValues.Clear();
        Index = FSections.IndexOf(FPath);
        if (Index != -1)
        {
            First = ObjectToInt(FSections.Objects[Index]);
            if (Index != FSections.Count - 1)
            {
                Last = ObjectToInt(FSections.Objects[Index + 1]) - 1;
            }
            else
            {
                Last = FText.Count - 1;
            }

            I = First;
            ID = -1;
            Value = "";
            while (I <= Last)
            {
                NewID = GetLineID(FText[I]);
                if (ID != -1)
                {
                    if (NewID != ID && NewID != -1)
                    {
                        var count = FValues.Count;
                        FPropertyTable.Add(ID, count);
                        FValues.Add(Value);
                        ID = NewID;
                        Value = GetLineValue(FText[I]);
                    }
                    else
                    {
                        var item = FText[I];
                        Value = Value + "\r" + "\n" + item;
                    }
                }
                else
                {
                    if (NewID != -1)
                    {
                        if (ID == -1)
                        {
                            ID = NewID;
                        }

                        Value = GetLineValue(FText[I]);
                    }
                }

                I += 1;
            }

            if (ID != -1)
            {
                var count = FValues.Count;
                FPropertyTable.Add(ID, count);
                FValues.Add(Value);
            }
        }
    }

    // Engine.pas:10611
    public EReadTextStorageStream(Stream AStream, GameEngine AEngine, int ASize) : base(AStream, AEngine)
    {
        FPath = "";
        if (ASize <= 0)
        {
            FSize = (int)(FStream.Length - FStream.Position);
        }
        else
        {
            FSize = ASize;
        }

        FValues = new StringList();
        FPropertyTable = new PropertyTable();
        FText = new StringList();
        FSections = new StringList();
        ReadStreamText();
        ReadTextBuffer();
    }

    // Engine.pas:10624
    public override void Dispose()
    {
        FValues?.Dispose();
        FPropertyTable?.Dispose();
        FSections?.Dispose();
        FText?.Dispose();
    }

    // Engine.pas:10669
    public override Set<StorageModeElement> StorageMode() => Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smText);

    // Engine.pas:10741
    public override void rwCreateEObject(ref EObject Obj, ECustomNode AOwner, int ID)
    {
        EObject O;
        O = Obj;
        if (O != null)
        {
            rwEObject(O, ID);
        }
    }

    // Engine.pas:10749
    public override void rwEObject(EObject Obj, int ID)
    {
        StringList SavedValues;
        string SavedPath;
        bool Continue;
        SavedPath = FPath;
        if (!IsEmpty(FPath))
        {
            FPath = FPath + "\\" + IntToStr(ID);
        }
        else
        {
            FPath = IntToStr(ID);
        }

        SavedValues = FValues;
        FPropertyTable.BackupSettings();
        FValues = new StringList();
        try
        {
            ReadTextBuffer();
            Obj.MainReadWrite(this);
            FValues?.Dispose();
            FPropertyTable.RestoreSettings();
            FValues = SavedValues;
            FPath = SavedPath;
        }
        catch
        {
            FValues?.Dispose();
            FPropertyTable.RestoreSettings();
            FValues = SavedValues;
            FPath = SavedPath;
            Continue = true;
            if (FOnError != null)
            {
                var classID = Obj.EClass.ClassID;
                FOnError(this, "Error reading object (cid:" + IntToStr(classID) + ")", ref Continue);
            }

            if (!Continue)
            {
                throw;
            }
        }
    }

    // Engine.pas:10779
    public override void rwEObjectStream(EObject Obj, int ID)
    {
        rwEObject(Obj, ID);
    }

    // Engine.pas:10784
    public override void rwString(ref string S, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            S = FValues[Offset];
        }
        else
        {
            S = "";
        }
    }

    // Engine.pas:10792
    public override void rwStrings(StringCollection Strings, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            Strings.Text = NullTerminatedText(FValues[Offset]);
        }
        else
        {
            Strings.Clear();
        }
    }

    // Engine.pas:10800
    public override void rwStringsEx(StringCollection Strings, int ID)
    {
        rwStrings(Strings, ID);
    }

    // Engine.pas:10805
    public override void rwChildren(ECustomNode Node, int ID)
    {
        EObject C;
        // { the original saves the path and never restores it }
        if (!IsEmpty(FPath))
        {
            FPath = FPath + "\\" + IntToStr(ID);
        }
        else
        {
            FPath = IntToStr(ID);
        }

        for (int I = 0, last = Node.Count - 1; I <= last; I++)
        {
            C = Node.Children[I];
            if (C != null && C.Owner == Node && !C.GetControlStyle().Contains(EngineServices.csShared) && C.GetControlStyle().Contains(EngineServices.csStored))
            {
                rwEObject(C, I);
            }
        }
    }

    private static int GetLineID(string S)
    {
        int i;
        for (i = 1; Set<byte>.WithBounds(0, 255).With((byte)91).WithRange((byte)48, (byte)57)
            .Contains(Cp1252.Byte(S[i - 1])); i++)
        {
        }
        if (RtlUnit.Copy(S, i, 2) == "]=")
        {
            return RtlUnit.StrToInt(RtlUnit.Copy(S, 2, i - 2));
        }
        return -1;
    }

    private static string GetLineValue(string S)
    {
        int i;
        for (i = 1; Set<byte>.WithBounds(0, 255).With((byte)91).With((byte)93)
            .WithRange((byte)48, (byte)57)
            .Contains(Cp1252.Byte(S[i - 1])); i++)
        {
        }
        return RtlUnit.Copy(S, i + 1, S.Length - i);
    }

}
