// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Game;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Storage;

// FPartial, FBuffer. Values go into the buffer. the property table records (id, buffer
// position). A nested object gets its own buffer, appended to the outer one as [table][data].
// The destructor writes the root table and buffer
public partial class EWriteStorageStream : EStorageStream
{

    internal bool FPartial;
    internal BufferStream FBuffer;

    // Engine.pas:9592
    internal void PropertyExistError(int ID)
    {
        throw new DoublePriorityIdException("PropertyID already in use (" + IntToStr(ID) + " / " + this.GetType().Name + ")");
    }

    // Engine.pas:9565
    // Engine.pas:10489 TDebugWriteStream borrows the supplied stream as its buffer.
    private protected EWriteStorageStream(Stream stream, GameEngine engine, BufferStream buffer) : base(stream, engine)
    {
        FBuffer = buffer;
        FPropertyTable = new PropertyTable();
        FPartial = false;
    }

    public EWriteStorageStream(Stream AStream, GameEngine AEngine, bool APartial) : base(AStream, AEngine)
    {
        FEngine = AEngine;
        FStream = AStream;
        FBuffer = new BufferStream();
        FPropertyTable = new PropertyTable();
        FPartial = APartial;
    }

    // Engine.pas:9575
    public override void Dispose()
    {
        FPropertyTable.SaveToStream(FStream);
        FBuffer.SaveToStream(FStream);
        FPropertyTable?.Dispose();
        FBuffer?.Dispose();
        base.Dispose();
    }

    // Engine.pas:9584
    public override Set<StorageModeElement> StorageMode()
    {
        Set<StorageModeElement> Result;
        if (FPartial)
        {
            Result = Set<StorageModeElement>.Of(StorageModeElement.Write, EngineServices.smPartial);
        }
        else
        {
            Result = Set<StorageModeElement>.Of(StorageModeElement.Write);
        }

        return Result;
    }

    // Engine.pas:9597
    public override void rwByte(ref byte B, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (B != 0)
            {
                FPropertyTable.Add(ID, (int)(FBuffer.Position));
                FBuffer.Write(Bytes.Of(ref B, 1));
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9611
    public override void rwWord(ref ushort W, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (W != 0)
            {
                FPropertyTable.Add(ID, (int)(FBuffer.Position));
                FBuffer.Write(Bytes.Of(ref W, 2));
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9625
    public override void rwShortint(ref sbyte S, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (S != 0)
            {
                FPropertyTable.Add(ID, (int)(FBuffer.Position));
                FBuffer.Write(Bytes.Of(ref S, 1));
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9639
    public override void rwInteger(ref int I, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (I != 0)
            {
                FPropertyTable.Add(ID, (int)(FBuffer.Position));
                FBuffer.Write(Bytes.Of(ref I, 4));
            }
        }
        else
        {
            throw new DoublePriorityIdException("PropertyID already in use");
        }
    }

    // Engine.pas:9653
    public override void rwByteEx(ref byte B, byte Default, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (B != Default)
            {
                FPropertyTable.Add(ID, (int)(FBuffer.Position));
                FBuffer.Write(Bytes.Of(ref B, 1));
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9667
    public override void rwWordEx(ref ushort W, ushort Default, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (W != Default)
            {
                FPropertyTable.Add(ID, (int)(FBuffer.Position));
                FBuffer.Write(Bytes.Of(ref W, 2));
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9681
    public override void rwShortintEx(ref sbyte S, sbyte Default, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (S != Default)
            {
                FPropertyTable.Add(ID, (int)(FBuffer.Position));
                FBuffer.Write(Bytes.Of(ref S, 1));
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9695
    public override void rwIntegerEx(ref int I, int Default, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (I != Default)
            {
                FPropertyTable.Add(ID, (int)(FBuffer.Position));
                FBuffer.Write(Bytes.Of(ref I, 4));
            }
        }
        else
        {
            throw new DoublePriorityIdException("PropertyID already in use");
        }
    }

    // Engine.pas:9709
    public override void rwString(ref string S, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (!IsEmpty(S))
            {
                FPropertyTable.Add(ID, (int)(FBuffer.Position));
                SaveStringToStream(S, FBuffer);
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9723
    public override void rwStrings(StringCollection Strings, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (Strings.Count != 0)
            {
                FPropertyTable.Add(ID, (int)(FBuffer.Position));
                StringsToStream(Strings, FBuffer);
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9737
    public override void rwStringsEx(StringCollection Strings, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (Strings.Count != 0)
            {
                FPropertyTable.Add(ID, (int)(FBuffer.Position));
                StringsToStreamEx(Strings, FBuffer);
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9751
    public override void rwData(Span<byte> Buffer, int Size, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (Size != 0)
            {
                FPropertyTable.Add(ID, (int)(FBuffer.Position));
                FBuffer.Write(Buffer.Slice(0, Size));
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9765
    public override void rwStreamProc(int ID, StreamProc LoadProc, StreamProc SaveProc)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            FPropertyTable.Add(ID, (int)(FBuffer.Position));
            try
            {
                SaveProc(FBuffer);
            }
            catch
            {
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9779
    public override Stream OpenPropertyStream(int ID)
    {
        Stream Result;
        if (!FPropertyTable.PropertyExist(ID))
        {
            FPropertyTable.Add(ID, (int)(FBuffer.Position));
            Result = FBuffer;
        }
        else
        {
            PropertyExistError(ID);
            Result = null;
        }

        return Result;
    }

    // Engine.pas:9793
    public override void rwBool(ref bool B, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (B)
            {
                FPropertyTable.Add(ID, (int)(FBuffer.Position));
            }
        }
        else
        {
            PropertyExistError(ID);
        }
    }

    // Engine.pas:9803
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
                    FOnError(this, EStreamWriteErrorStr, ref Continue);
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

    // Engine.pas:9820
    public override void rwCreateEObject(ref EngineObject Obj, ECustomNode AOwner, int ID)
    {
        BufferStream Outer;
        int Start;
        int AClassID;
        EngineObject O;
        if (!FPropertyTable.PropertyExist(ID))
        {
            Start = (int)(FBuffer.Position);
            O = Obj;
            if (O != null)
            {
                AClassID = O.EClass.ClassID;
            }
            else
            {
                AClassID = 0;
            }

            if (AClassID != 0)
            {
                Outer = FBuffer;
                FBuffer = new BufferStream();
                FPropertyTable.BackupSettings();
                try
                {
                    Obj.MainReadWrite(this);
                    Outer.Write(Bytes.Of(ref AClassID, 4));
                    FPropertyTable.SaveToStream(Outer);
                    FBuffer.SaveToStream(Outer);
                    FBuffer?.Dispose();
                    FPropertyTable.RestoreSettings();
                    FBuffer = Outer;
                    FPropertyTable.Add(ID, Start);
                }
                catch (Exception error)
                {
                    FPropertyTable.RestoreSettings();
                    FBuffer?.Dispose();
                    FBuffer = Outer;
                    Outer.Position = Start;
                    if (!HandleException(error))
                    {
                        throw;
                    }
                }
            }
            else
            {
                FBuffer.Write(Bytes.Of(ref AClassID, 4));
                FPropertyTable.Add(ID, Start);
            }
        }
        else
        {
            throw new DoublePriorityIdException("Double priorityID found (" + IntToStr(ID) + ")");
        }
    }

    // Engine.pas:9866
    public override void rwChildren(ECustomNode Node, int ID)
    {
        BufferStream Outer;
        int Start;
        EngineObject C = default;
        if (!FPropertyTable.PropertyExist(ID))
        {
            Start = (int)(FBuffer.Position);
            Outer = FBuffer;
            FBuffer = new BufferStream();
            FPropertyTable.BackupSettings();
            for (int I = 0, last = Node.Count - 1; I <= last; I++)
            {
                C = Node.Children[I];
                if (C != null && C.Owner == Node && !C.GetControlStyle().Contains(EngineServices.csShared) && C.GetControlStyle().Contains(EngineServices.csStored))
                {
                    rwCreateEObject(ref C, Node, I);
                }
            }

            FPropertyTable.SaveToStream(Outer);
            FBuffer.SaveToStream(Outer);
            FBuffer?.Dispose();
            FPropertyTable.RestoreSettings();
            FBuffer = Outer;
            FPropertyTable.Add(ID, Start);
        }
        else
        {
            throw new DoublePriorityIdException("Double priorityID found (" + IntToStr(ID) + ")");
        }
    }

    // Engine.pas:9897
    public override void rwEObject(EngineObject Obj, int ID)
    {
        BufferStream Outer;
        int Start;
        if (!FPropertyTable.PropertyExist(ID))
        {
            Start = (int)(FBuffer.Position);
            Outer = FBuffer;
            FBuffer = new BufferStream();
            FPropertyTable.BackupSettings();
            try
            {
                Obj.MainReadWrite(this);
                FPropertyTable.SaveToStream(Outer);
                FBuffer.SaveToStream(Outer);
                FPropertyTable.RestoreSettings();
                FBuffer?.Dispose();
                FBuffer = Outer;
                FPropertyTable.Add(ID, Start);
            }
            catch (Exception error)
            {
                FPropertyTable.RestoreSettings();
                FBuffer?.Dispose();
                FBuffer = Outer;
                Outer.Position = Start;
                if (!HandleException(error))
                {
                    throw;
                }
            }
        }
        else
        {
            throw new DoublePriorityIdException("Double priorityID found (" + Obj.GetType().Name + ":" + IntToStr(ID) + ")");
        }
    }

    // Engine.pas:9931
    public override void rwEObjectStream(EngineObject Obj, int ID)
    {
        BufferStream Outer;
        int Start;
        int Len = default;
        Start = (int)(FBuffer.Position);
        Outer = FBuffer;
        FBuffer = new BufferStream();
        FPropertyTable.BackupSettings();
        try
        {
            Obj.MainReadWrite(this);
            Outer.Write(Bytes.Of(ref Len, 4));
            FPropertyTable.SaveToStream(Outer);
            FBuffer.SaveToStream(Outer);
            Len = (int)(Outer.Position) - Start - 4;
            Outer.Position = Start;
            Outer.Write(Bytes.Of(ref Len, 4));
            Outer.Position = Start + Len + 4;
            FPropertyTable.RestoreSettings();
            FBuffer?.Dispose();
            FBuffer = Outer;
        }
        catch (Exception error)
        {
            FBuffer?.Dispose();
            FBuffer = Outer;
            FPropertyTable.RestoreSettings();
            Len = 0;
            Outer.Position = Start;
            Outer.Write(Bytes.Of(ref Len, 4));
            if (!HandleException(error))
            {
                throw;
            }
        }
    }

    private static readonly string EStreamWriteErrorStr = "Error Writing EObject";

    internal static void StringsToStream(StringCollection Strings, Stream Stream)
    {
        ReadOnlySpan<byte> text = (ReadOnlySpan<byte>)(Cp1252.Encode(Strings.Text));
        int num = CStringLength(text);
        Stream.Write((Span<byte>)(Bytes.Of<int>(ref num, 4)));
        StreamExtensions.WriteBuffer(Stream, text.Slice(0, num));
    }

    private static void SaveStringToStream(string S, Stream Stream)
    {
        byte b = 0;
        int length = S.Length;
        if (length < 255)
        {
            b = (byte)length;
            Stream.Write((Span<byte>)(Bytes.Of<byte>(ref b, 1)));
        }
        else
        {
            b = 255;
            Stream.Write((Span<byte>)(Bytes.Of<byte>(ref b, 1)));
            Stream.Write((Span<byte>)(Bytes.Of<int>(ref length, 4)));
        }
        Stream.Write((Span<byte>)(MemoryExtensions.AsSpan<byte>(Cp1252.Encode(S)).Slice(0, length)));
    }

    internal static void StringsToStreamEx(StringCollection Strings, Stream Stream)
    {
        int num = 0;
        ReadOnlySpan<byte> text = (ReadOnlySpan<byte>)(Cp1252.Encode(Strings.Text));
        int num2 = CStringLength(text);
        Stream.Write((Span<byte>)(Bytes.Of<int>(ref num2, 4)));
        StreamExtensions.WriteBuffer(Stream, text.Slice(0, num2));
        int i = 0;
        for (int num3 = Strings.Count - 1; i <= num3; i++)
        {
            num = RtlUnit.ObjectToInt(Strings.Objects[i]);
            Stream.Write((Span<byte>)(Bytes.Of<int>(ref num, 4)));
        }
    }

    private static int CStringLength(ReadOnlySpan<byte> text)
    {
        int num = MemoryExtensions.IndexOf<byte>(text, (byte)0);
        return (num < 0) ? text.Length : num;
    }

}
