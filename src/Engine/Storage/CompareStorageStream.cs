// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Game;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Storage;

// reads a stored copy and calls Mismatch for every property that differs from the object.
// Mismatch raises and swallows its own exception, a place for a breakpoint
public partial class CompareStorageStream : EReadStorageStream
{
    // Engine.pas:11002
    internal void Mismatch(int ID)
    {
        try
        {
            throw new InvalidOperationException("Property Mismatch (PropertyID:" + IntToStr(ID) + ")");
        }
        catch
        {
        }
    }

    // Engine.pas:10918
    public CompareStorageStream(Stream AStream, GameEngine AEngine) : base(AStream, AEngine, false)
    { }

    // Engine.pas:10928
    public override Set<StorageModeElement> StorageMode() => Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.Compare);

    // Engine.pas:10933
    public override void rwCreateEObject(ref EObject Obj, ECustomNode AOwner, int ID)
    {
        int Offset;
        int SavedBase;
        int AClassID = default;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1 && Obj != null)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.ReadBytes(Bytes.Of(ref AClassID, 4));
            if (AClassID != 0)
            {
                SavedBase = FDataBase;
                FPropertyTable.BackupSettings();
                FPropertyTable.LoadFromStream(FStream);
                FDataBase = (int)(FStream.Position);
                // { The original handles read errors by destroying and clearing Obj. }
                try
                {
                    Obj.MainReadWrite(this);
                    FPropertyTable.RestoreSettings();
                    FDataBase = SavedBase;
                }
                catch (Exception error)
                {
                    Obj?.Dispose();
                    Obj = null;
                    FPropertyTable.RestoreSettings();
                    FDataBase = SavedBase;
                    if (!HandleException(error))
                    {
                        throw;
                    }
                }
            }
        }
    }

    // Engine.pas:10958
    public override void rwChildren(ECustomNode Node, int ID)
    {
        int SavedBase;
        int Start;
        EObject C = default;
        int Offset;
        int Index;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            SavedBase = FDataBase;
            FPropertyTable.BackupSettings();
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FPropertyTable.LoadFromStream(FStream);
            FDataBase = (int)(FStream.Position);
            if (Node.GetControlStyle().Contains(EngineServices.csKeepIndex))
            {
                for (int I = FPropertyTable.First, last = FPropertyTable.Count - 1; I <= last; I++)
                {
                    Index = FPropertyTable.Items[I].ID;
                    C = Node.Children[Index];
                    if (C != null)
                    {
                        rwCreateEObject(ref C, Node, Index);
                    }
                }
            }
            else
            {
                Start = 0;
                while (Start < Node.Count && Node.Children[Start] != null && (Node.Children[Start].GetControlStyle() & Set<EControlStyleElement>.Of(EngineServices.csShared)) != Set<EControlStyleElement>.Of())
                {
                    Start += 1;
                }

                for (int I = FPropertyTable.First, last = FPropertyTable.Count - 1; I <= last; I++)
                {
                    Index = FPropertyTable.Items[I].ID;
                    if (Index < Node.Count)
                    {
                        C = Node.Children[I - FPropertyTable.First + Start];
                        if (C != null)
                        {
                            rwCreateEObject(ref C, Node, Index);
                        }
                    }
                }
            }

            FPropertyTable.RestoreSettings();
            FDataBase = SavedBase;
        }
    }

    // Engine.pas:11010
    public override void rwData(Span<byte> Buffer, int Size, int ID)
    {
        byte[] image = new byte[Size];
        base.rwData(image, Size, ID);
        if (!image.AsSpan().SequenceEqual(Buffer[..Size])) Mismatch(ID);
    }

    // Engine.pas:11022
    public override void rwString(ref string S, int ID)
    {
        string Value = "";
        base.rwString(ref Value, ID);
        if (Value != S)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11030
    public override void rwStrings(StringCollection Strings, int ID)
    {
        StringList L;
        // { the original never frees L }
        L = new StringList();
        base.rwStrings(L, ID);
        var text = Strings.Text;
        if (CompareText(L.Text, text) != 0)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11040
    public override void rwStringsEx(StringCollection Strings, int ID)
    {
        StringList L;
        L = new StringList();
        base.rwStrings(L, ID);
        var text = Strings.Text;
        if (CompareText(L.Text, text) != 0)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11049
    public override void rwInteger(ref int I, int ID)
    {
        int Value = default;
        base.rwInteger(ref Value, ID);
        if (I != Value)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11057
    public override void rwByte(ref byte B, int ID)
    {
        byte Value = default;
        base.rwByte(ref Value, ID);
        if (B != Value)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11073
    public override void rwWord(ref ushort W, int ID)
    {
        ushort Value = default;
        base.rwWord(ref Value, ID);
        if (W != Value)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11081
    public override void rwBool(ref bool B, int ID)
    {
        bool Value = default;
        base.rwBool(ref Value, ID);
        if (B != Value)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11065
    public override void rwShortint(ref sbyte S, int ID)
    {
        sbyte Value = default;
        base.rwShortint(ref Value, ID);
        if (S != Value)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11089
    public override void rwIntegerEx(ref int I, int Default, int ID)
    {
        int Value = default;
        base.rwIntegerEx(ref Value, Default, ID);
        if (I != Value)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11097
    public override void rwByteEx(ref byte B, byte Default, int ID)
    {
        byte Value = default;
        base.rwByteEx(ref Value, Default, ID);
        if (B != Value)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11113
    public override void rwWordEx(ref ushort W, ushort Default, int ID)
    {
        ushort Value = default;
        base.rwWordEx(ref Value, Default, ID);
        if (W != Value)
        {
            Mismatch(ID);
        }
    }

    // Engine.pas:11105
    public override void rwShortintEx(ref sbyte S, sbyte Default, int ID)
    {
        sbyte Value = default;
        base.rwShortintEx(ref Value, Default, ID);
        if (S != Value)
        {
            Mismatch(ID);
        }
    }

}
