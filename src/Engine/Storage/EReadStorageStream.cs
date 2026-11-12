// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Game;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Storage;

// FPartial, FDataBase. The constructor reads the root property table. every property is found
// through the table and read at FDataBase + offset
public partial class EReadStorageStream : EStorageStream
{
    internal bool FPartial;
    internal int FDataBase;

    // Engine.pas:9968
    public EReadStorageStream(Stream AStream, GameEngine AEngine, bool APartial) : base(AStream, AEngine)
    {
        FPropertyTable = new PropertyTable();
        FPropertyTable.LoadFromStream(AStream);
        FDataBase = (int)(FStream.Position);
        FPartial = APartial;
    }

    // Engine.pas:9977
    public override void Dispose()
    {
        FPropertyTable?.Dispose();
        base.Dispose();
    }

    // Engine.pas:9983
    public override Set<StorageModeElement> StorageMode()
    {
        Set<StorageModeElement> Result;
        if (FPartial)
        {
            Result = Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.Partial);
        }
        else
        {
            Result = Set<StorageModeElement>.Of(StorageModeElement.Read);
        }

        return Result;
    }

    // Engine.pas:9991
    public override void rwInteger(ref int I, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.ReadExactly(Bytes.Of(ref I, 4));
        }
        else
        {
            I = 0;
        }
    }

    // Engine.pas:10005
    public override void rwByte(ref byte B, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.ReadExactly(Bytes.Of(ref B, 1));
        }
        else
        {
            B = 0;
        }
    }

    // Engine.pas:10019
    public override void rwShortint(ref sbyte S, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.ReadExactly(Bytes.Of(ref S, 1));
        }
        else
        {
            S = 0;
        }
    }

    // Engine.pas:10033
    public override void rwWord(ref ushort W, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.ReadExactly(Bytes.Of(ref W, 2));
        }
        else
        {
            W = 0;
        }
    }

    // Engine.pas:10047
    public override void rwIntegerEx(ref int I, int Default, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.ReadExactly(Bytes.Of(ref I, 4));
        }
        else
        {
            I = Default;
        }
    }

    // Engine.pas:10061
    public override void rwByteEx(ref byte B, byte Default, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.ReadExactly(Bytes.Of(ref B, 1));
        }
        else
        {
            B = Default;
        }
    }

    // Engine.pas:10075
    public override void rwShortintEx(ref sbyte S, sbyte Default, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.ReadExactly(Bytes.Of(ref S, 1));
        }
        else
        {
            S = Default;
        }
    }

    // Engine.pas:10089
    public override void rwWordEx(ref ushort W, ushort Default, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.ReadExactly(Bytes.Of(ref W, 2));
        }
        else
        {
            W = Default;
        }
    }

    // Engine.pas:10103
    public override void rwString(ref string S, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            LoadStringFromStream(ref S, FStream);
        }
        else
        {
            S = "";
        }
    }

    // Engine.pas:10117
    public override void rwStrings(StringCollection Strings, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            StringsFromStream(Strings, FStream);
        }
        else
        {
            Strings.Clear();
        }
    }

    // Engine.pas:10131
    public override void rwStringsEx(StringCollection Strings, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            StringsFromStreamEx(Strings, FStream);
        }
        else
        {
            Strings.Clear();
        }
    }

    // Engine.pas:10145
    public override void rwData(Span<byte> Buffer, int Size, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.ReadExactly(Buffer.Slice(0, Size));
        }
    }

    // Engine.pas:10157
    public override void rwStreamProc(int ID, StreamProc LoadProc, StreamProc SaveProc)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            try
            {
                LoadProc(FStream);
            }
            catch
            {
            }
        }
    }

    // Engine.pas:10172
    public override Stream OpenPropertyStream(int ID)
    {
        Stream Result;
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            Result = FStream;
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:10186
    public override void rwBool(ref bool B, int ID)
    {
        int Offset;
        Offset = FPropertyTable.FindOffset(ID);
        B = Offset != -1;
    }

    // Engine.pas:10191
    public bool HandleException(Exception E)
    {
        bool Continue;
        if (!FErrorHandled)
        {
            Continue = true;
            if (FOnError != null)
            {
                if (IsEmpty(E.Message))
                {
                    FOnError(this, EStreamReadErrorStr, ref Continue);
                }
                else
                {
                    FOnError(this, E.Message, ref Continue);
                }
            }

            if (!Continue)
            {
                FErrorHandled = true;
            }
        }

        return !FErrorHandled;
    }

    // Engine.pas:10208
    public override void rwCreateEObject(ref EngineObject Obj, ECustomNode AOwner, int ID)
    {
        int Offset;
        int SavedBase;
        int AClassID = default;
        Obj = null;
        Offset = FPropertyTable.FindOffset(ID);
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FStream.ReadExactly(Bytes.Of(ref AClassID, 4));
            if (AClassID != 0)
            {
                SavedBase = FDataBase;
                FPropertyTable.BackupSettings();
                FPropertyTable.LoadFromStream(FStream);
                FDataBase = (int)(FStream.Position);
                try
                {
                    Obj = FEngine.CreateEngineObject(AClassID, null);
                    Obj.SetOwnerWithoutNotify(AOwner);
                    Obj.MainReadWrite(this);
                    Obj.SetOwnerWithoutNotify(null);
                    Obj.SetOwner(AOwner);
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

    // Engine.pas:10246
    public override void rwChildren(ECustomNode Node, int ID)
    {
        int Offset;
        int SavedBase;
        int ChildID;
        EngineObject Child = default;
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
            if (Node.GetControlStyle().Contains(EngineServices.KeepIndex))
            {
                for (int I = FPropertyTable.First, last = FPropertyTable.Count - 1; I <= last; I++)
                {
                    ChildID = FPropertyTable.Items[I].Id;
                    rwCreateEObject(ref Child, Node, ChildID);
                    if (Child != null)
                    {
                        Child.SetArrayOwner(Node, ChildID);
                    }
                }
            }
            else
            {
                for (int I = FPropertyTable.First, last = FPropertyTable.Count - 1; I <= last; I++)
                {
                    ChildID = FPropertyTable.Items[I].Id;
                    rwCreateEObject(ref Child, Node, ChildID);
                }
            }

            FPropertyTable.RestoreSettings();
            FDataBase = SavedBase;
        }
    }

    // Engine.pas:10277
    public override void rwEObject(EngineObject Obj, int ID)
    {
        int Offset;
        int SavedBase;
        SavedBase = FDataBase;
        Offset = FPropertyTable.FindOffset(ID);
        FPropertyTable.BackupSettings();
        if (Offset != -1)
        {
            if (FStream.Position != FDataBase + Offset)
            {
                FStream.Position = FDataBase + Offset;
            }

            FPropertyTable.LoadFromStream(FStream);
            FDataBase = (int)(FStream.Position);
        }

        try
        {
            Obj.MainReadWrite(this);
            FPropertyTable.RestoreSettings();
            FDataBase = SavedBase;
        }
        catch (Exception error)
        {
            FPropertyTable.RestoreSettings();
            FDataBase = SavedBase;
            if (!HandleException(error))
            {
                throw;
            }
        }
    }

    // Engine.pas:10304
    public override void rwEObjectStream(EngineObject Obj, int ID)
    {
        int SavedBase;
        int Start;
        int Size = default;
        Start = (int)(FStream.Position);
        FStream.ReadExactly(Bytes.Of(ref Size, 4));
        if (Size != 0)
        {
            SavedBase = FDataBase;
            FPropertyTable.BackupSettings();
            try
            {
                FPropertyTable.LoadFromStream(FStream);
                FDataBase = (int)(FStream.Position);
                Obj.MainReadWrite(this);
                FPropertyTable.RestoreSettings();
                FDataBase = SavedBase;
                FStream.Position = Start + Size + 4;
            }
            catch (Exception error)
            {
                FPropertyTable.RestoreSettings();
                FDataBase = SavedBase;
                FStream.Position = Start + Size + 4;
                if (!HandleException(error))
                {
                    throw;
                }
            }
        }
    }

    public int DataBase { get => FDataBase; }

    private static readonly string EStreamReadErrorStr = "Error Reading EObject";

    internal static void StringsFromStream(StringCollection Strings, Stream Stream)
    {
        int num = 0;
        Strings.BeginUpdate();
        try
        {
            Stream.ReadExactly(Bytes.Of<int>(ref num, 4));
            if (num < 0)
            {
                throw new ArgumentOutOfRangeException((string)null, "Negative string length");
            }
            byte[] array = new byte[num];
            Stream.ReadExactly((Span<byte>)(array));
            Strings.Text = Cp1252.Decode((ReadOnlySpan<byte>)(array));
        }
        finally
        {
            Strings.EndUpdate();
        }
    }

    internal static void LoadStringFromStream(ref string S, Stream Stream)
    {
        int num = 0;
        byte b = 0;
        S = "";
        Stream.ReadExactly(Bytes.Of<byte>(ref b, 1));
        if (b == 255)
        {
            Stream.ReadExactly(Bytes.Of<int>(ref num, 4));
        }
        else
        {
            num = b;
        }
        if (num < 0)
        {
            throw new ArgumentOutOfRangeException((string)null, "Negative string length");
        }
        byte[] array = new byte[num];
        Cp1252.Encode(S)[..Math.Min(S.Length, num)].CopyTo(array);
        S = Cp1252.Decode((ReadOnlySpan<byte>)(array));
        Stream.ReadExactly((Span<byte>)(array));
        S = Cp1252.Decode((ReadOnlySpan<byte>)(array));
    }

    internal static void StringsFromStreamEx(StringCollection Strings, Stream Stream)
    {
        int num = 0;
        int num2 = 0;
        Strings.BeginUpdate();
        try
        {
            Stream.ReadExactly(Bytes.Of<int>(ref num, 4));
            if (num < 0)
            {
                throw new ArgumentOutOfRangeException((string)null, "Negative string length");
            }
            byte[] array = new byte[num];
            Stream.ReadExactly((Span<byte>)(array));
            Strings.Text = Cp1252.Decode((ReadOnlySpan<byte>)(array));
            int i = 0;
            for (int num3 = Strings.Count - 1; i <= num3; i++)
            {
                Stream.ReadExactly(Bytes.Of<int>(ref num2, 4));
                Strings.Objects[i] = RtlUnit.IntToObject(num2);
            }
        }
        finally
        {
            Strings.EndUpdate();
        }
    }

}
