// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Game;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Storage;

// _partial, _buffer. Values go into the buffer. the property table records (id, buffer
// position). A nested object gets its own buffer, appended to the outer one as [table][data].
// The destructor writes the root table and buffer
public partial class StorageWriter : StorageStream
{

    private readonly bool _partial;
    private BufferStream _buffer;

    // Engine.pas:9592
    internal void PropertyExistError(int id)
    {
        throw new DoublePriorityIdException("PropertyID already in use (" + IntToStr(id) + " / " + this.GetType().Name + ")");
    }

    // Engine.pas:9565
    // Engine.pas:10489 TDebugWriteStream borrows the supplied stream as its buffer.
    private protected StorageWriter(Stream stream, GameEngine engine, BufferStream buffer) : base(stream, engine)
    {
        _buffer = buffer;
        _propertyTable = new PropertyTable();
        _partial = false;
    }

    public StorageWriter(Stream stream, GameEngine engine, bool partial) : base(stream, engine)
    {
        _engine = engine;
        _stream = stream;
        _buffer = new BufferStream();
        _propertyTable = new PropertyTable();
        _partial = partial;
    }

    // Engine.pas:9575
    public override void Dispose()
    {
        _propertyTable.SaveToStream(_stream);
        _buffer.SaveToStream(_stream);
        _propertyTable?.Dispose();
        _buffer?.Dispose();
        base.Dispose();
    }

    // Engine.pas:9584
    public override Set<StorageModeElement> StorageMode()
    {
        Set<StorageModeElement> result;
        if (_partial)
        {
            result = Set<StorageModeElement>.Of(StorageModeElement.Write, EngineServices.Partial);
        }
        else
        {
            result = Set<StorageModeElement>.Of(StorageModeElement.Write);
        }

        return result;
    }

    // Engine.pas:9597
    public override void Byte(ref byte value, int id)
    {
        if (!_propertyTable.PropertyExist(id))
        {
            if (value != 0)
            {
                _propertyTable.Add(id, (int)(_buffer.Position));
                _buffer.Write(Rtl.Bytes.Of(ref value, 1));
            }
        }
        else
        {
            PropertyExistError(id);
        }
    }

    // Engine.pas:9611
    public override void UInt16(ref ushort value, int id)
    {
        if (!_propertyTable.PropertyExist(id))
        {
            if (value != 0)
            {
                _propertyTable.Add(id, (int)(_buffer.Position));
                _buffer.Write(Rtl.Bytes.Of(ref value, 2));
            }
        }
        else
        {
            PropertyExistError(id);
        }
    }

    // Engine.pas:9625
    public override void SByte(ref sbyte value, int id)
    {
        if (!_propertyTable.PropertyExist(id))
        {
            if (value != 0)
            {
                _propertyTable.Add(id, (int)(_buffer.Position));
                _buffer.Write(Rtl.Bytes.Of(ref value, 1));
            }
        }
        else
        {
            PropertyExistError(id);
        }
    }

    // Engine.pas:9639
    public override void Int32(ref int value, int id)
    {
        if (!_propertyTable.PropertyExist(id))
        {
            if (value != 0)
            {
                _propertyTable.Add(id, (int)(_buffer.Position));
                _buffer.Write(Rtl.Bytes.Of(ref value, 4));
            }
        }
        else
        {
            throw new DoublePriorityIdException("PropertyID already in use");
        }
    }

    // Engine.pas:9653
    public override void Byte(ref byte value, byte fallback, int id)
    {
        if (!_propertyTable.PropertyExist(id))
        {
            if (value != fallback)
            {
                _propertyTable.Add(id, (int)(_buffer.Position));
                _buffer.Write(Rtl.Bytes.Of(ref value, 1));
            }
        }
        else
        {
            PropertyExistError(id);
        }
    }

    // Engine.pas:9667
    public override void UInt16(ref ushort value, ushort fallback, int id)
    {
        if (!_propertyTable.PropertyExist(id))
        {
            if (value != fallback)
            {
                _propertyTable.Add(id, (int)(_buffer.Position));
                _buffer.Write(Rtl.Bytes.Of(ref value, 2));
            }
        }
        else
        {
            PropertyExistError(id);
        }
    }

    // Engine.pas:9681
    public override void SByte(ref sbyte value, sbyte fallback, int id)
    {
        if (!_propertyTable.PropertyExist(id))
        {
            if (value != fallback)
            {
                _propertyTable.Add(id, (int)(_buffer.Position));
                _buffer.Write(Rtl.Bytes.Of(ref value, 1));
            }
        }
        else
        {
            PropertyExistError(id);
        }
    }

    // Engine.pas:9695
    public override void Int32(ref int value, int fallback, int id)
    {
        if (!_propertyTable.PropertyExist(id))
        {
            if (value != fallback)
            {
                _propertyTable.Add(id, (int)(_buffer.Position));
                _buffer.Write(Rtl.Bytes.Of(ref value, 4));
            }
        }
        else
        {
            throw new DoublePriorityIdException("PropertyID already in use");
        }
    }

    // Engine.pas:9709
    public override void String(ref string value, int id)
    {
        if (!_propertyTable.PropertyExist(id))
        {
            if (!IsEmpty(value))
            {
                _propertyTable.Add(id, (int)(_buffer.Position));
                SaveStringToStream(value, _buffer);
            }
        }
        else
        {
            PropertyExistError(id);
        }
    }

    // Engine.pas:9723
    public override void Lines(StringCollection lines, int id)
    {
        if (!_propertyTable.PropertyExist(id))
        {
            if (lines.Count != 0)
            {
                _propertyTable.Add(id, (int)(_buffer.Position));
                StringsToStream(lines, _buffer);
            }
        }
        else
        {
            PropertyExistError(id);
        }
    }

    // Engine.pas:9737
    public override void LinesWithObjects(StringCollection lines, int id)
    {
        if (!_propertyTable.PropertyExist(id))
        {
            if (lines.Count != 0)
            {
                _propertyTable.Add(id, (int)(_buffer.Position));
                StringsToStreamEx(lines, _buffer);
            }
        }
        else
        {
            PropertyExistError(id);
        }
    }

    // Engine.pas:9751
    public override void Bytes(Span<byte> buffer, int size, int id)
    {
        if (!_propertyTable.PropertyExist(id))
        {
            if (size != 0)
            {
                _propertyTable.Add(id, (int)(_buffer.Position));
                _buffer.Write(buffer.Slice(0, size));
            }
        }
        else
        {
            PropertyExistError(id);
        }
    }

    // Engine.pas:9765
    public override void Payload(int id, StreamProc load, StreamProc save)
    {
        if (!_propertyTable.PropertyExist(id))
        {
            _propertyTable.Add(id, (int)(_buffer.Position));
            try
            {
                save(_buffer);
            }
            catch
            {
            }
        }
        else
        {
            PropertyExistError(id);
        }
    }

    // Engine.pas:9779
    public override Stream OpenProperty(int id)
    {
        Stream result;
        if (!_propertyTable.PropertyExist(id))
        {
            _propertyTable.Add(id, (int)(_buffer.Position));
            result = _buffer;
        }
        else
        {
            PropertyExistError(id);
            result = null;
        }

        return result;
    }

    // Engine.pas:9793
    public override void Boolean(ref bool value, int id)
    {
        if (!_propertyTable.PropertyExist(id))
        {
            if (value)
            {
                _propertyTable.Add(id, (int)(_buffer.Position));
            }
        }
        else
        {
            PropertyExistError(id);
        }
    }

    // Engine.pas:9803
    public bool HandleException(Exception error)
    {
        bool canContinue;
        if (!_errorHandled)
        {
            canContinue = true;
            if (_onError != null)
            {
                if (IsEmpty(error.Message))
                {
                    _onError(this, s_writeError, ref canContinue);
                }
                else
                {
                    _onError(this, error.Message, ref canContinue);
                }
            }

            if (!canContinue)
            {
                _errorHandled = true;
            }
        }

        return !_errorHandled;
    }

    // Engine.pas:9820
    public override void Created(ref EngineObject objectValue, ECustomNode owner, int id)
    {
        BufferStream outer;
        int start;
        int classId;
        EngineObject current;
        if (!_propertyTable.PropertyExist(id))
        {
            start = (int)(_buffer.Position);
            current = objectValue;
            if (current != null)
            {
                classId = current.EngineClass.ClassId;
            }
            else
            {
                classId = 0;
            }

            if (classId != 0)
            {
                outer = _buffer;
                _buffer = new BufferStream();
                _propertyTable.BackupSettings();
                try
                {
                    objectValue.MainReadWrite(this);
                    outer.Write(Rtl.Bytes.Of(ref classId, 4));
                    _propertyTable.SaveToStream(outer);
                    _buffer.SaveToStream(outer);
                    _buffer?.Dispose();
                    _propertyTable.RestoreSettings();
                    _buffer = outer;
                    _propertyTable.Add(id, start);
                }
                catch (Exception error)
                {
                    _propertyTable.RestoreSettings();
                    _buffer?.Dispose();
                    _buffer = outer;
                    outer.Position = start;
                    if (!HandleException(error))
                    {
                        throw;
                    }
                }
            }
            else
            {
                _buffer.Write(Rtl.Bytes.Of(ref classId, 4));
                _propertyTable.Add(id, start);
            }
        }
        else
        {
            throw new DoublePriorityIdException("Double priorityID found (" + IntToStr(id) + ")");
        }
    }

    // Engine.pas:9866
    public override void Children(ECustomNode node, int id)
    {
        BufferStream outer;
        int start;
        EngineObject child = default;
        if (!_propertyTable.PropertyExist(id))
        {
            start = (int)(_buffer.Position);
            outer = _buffer;
            _buffer = new BufferStream();
            _propertyTable.BackupSettings();
            for (int i = 0, last = node.Count - 1; i <= last; i++)
            {
                child = node.Children[i];
                if (child != null && child.Owner == node && !child.GetControlStyle().Contains(EngineServices.Shared) && child.GetControlStyle().Contains(EngineServices.Stored))
                {
                    Created(ref child, node, i);
                }
            }

            _propertyTable.SaveToStream(outer);
            _buffer.SaveToStream(outer);
            _buffer?.Dispose();
            _propertyTable.RestoreSettings();
            _buffer = outer;
            _propertyTable.Add(id, start);
        }
        else
        {
            throw new DoublePriorityIdException("Double priorityID found (" + IntToStr(id) + ")");
        }
    }

    // Engine.pas:9897
    public override void Object(EngineObject objectValue, int id)
    {
        BufferStream outer;
        int start;
        if (!_propertyTable.PropertyExist(id))
        {
            start = (int)(_buffer.Position);
            outer = _buffer;
            _buffer = new BufferStream();
            _propertyTable.BackupSettings();
            try
            {
                objectValue.MainReadWrite(this);
                _propertyTable.SaveToStream(outer);
                _buffer.SaveToStream(outer);
                _propertyTable.RestoreSettings();
                _buffer?.Dispose();
                _buffer = outer;
                _propertyTable.Add(id, start);
            }
            catch (Exception error)
            {
                _propertyTable.RestoreSettings();
                _buffer?.Dispose();
                _buffer = outer;
                outer.Position = start;
                if (!HandleException(error))
                {
                    throw;
                }
            }
        }
        else
        {
            throw new DoublePriorityIdException("Double priorityID found (" + objectValue.GetType().Name + ":" + IntToStr(id) + ")");
        }
    }

    // Engine.pas:9931
    public override void SizedObject(EngineObject objectValue, int id)
    {
        BufferStream outer;
        int start;
        int length = default;
        start = (int)(_buffer.Position);
        outer = _buffer;
        _buffer = new BufferStream();
        _propertyTable.BackupSettings();
        try
        {
            objectValue.MainReadWrite(this);
            outer.Write(Rtl.Bytes.Of(ref length, 4));
            _propertyTable.SaveToStream(outer);
            _buffer.SaveToStream(outer);
            length = (int)(outer.Position) - start - 4;
            outer.Position = start;
            outer.Write(Rtl.Bytes.Of(ref length, 4));
            outer.Position = start + length + 4;
            _propertyTable.RestoreSettings();
            _buffer?.Dispose();
            _buffer = outer;
        }
        catch (Exception error)
        {
            _buffer?.Dispose();
            _buffer = outer;
            _propertyTable.RestoreSettings();
            length = 0;
            outer.Position = start;
            outer.Write(Rtl.Bytes.Of(ref length, 4));
            if (!HandleException(error))
            {
                throw;
            }
        }
    }

    private static readonly string s_writeError = "Error Writing EObject";

    internal static void StringsToStream(StringCollection lines, Stream stream)
    {
        ReadOnlySpan<byte> text = (ReadOnlySpan<byte>)(Cp1252.Encode(lines.Text));
        int length = CStringLength(text);
        stream.Write((Span<byte>)(Rtl.Bytes.Of(ref length, 4)));
        StreamExtensions.WriteBuffer(stream, text.Slice(0, length));
    }

    private static void SaveStringToStream(string value, Stream stream)
    {
        byte b = 0;
        int length = value.Length;
        if (length < 255)
        {
            b = (byte)length;
            stream.Write((Span<byte>)(Rtl.Bytes.Of(ref b, 1)));
        }
        else
        {
            b = 255;
            stream.Write((Span<byte>)(Rtl.Bytes.Of(ref b, 1)));
            stream.Write((Span<byte>)(Rtl.Bytes.Of(ref length, 4)));
        }
        stream.Write((Span<byte>)(MemoryExtensions.AsSpan<byte>(Cp1252.Encode(value)).Slice(0, length)));
    }

    internal static void StringsToStreamEx(StringCollection lines, Stream stream)
    {
        int objectValue = 0;
        ReadOnlySpan<byte> text = (ReadOnlySpan<byte>)(Cp1252.Encode(lines.Text));
        int length = CStringLength(text);
        stream.Write((Span<byte>)(Rtl.Bytes.Of(ref length, 4)));
        StreamExtensions.WriteBuffer(stream, text.Slice(0, length));
        int i = 0;
        for (int last = lines.Count - 1; i <= last; i++)
        {
            objectValue = RtlUnit.ObjectToInt(lines.Objects[i]);
            stream.Write((Span<byte>)(Rtl.Bytes.Of(ref objectValue, 4)));
        }
    }

    private static int CStringLength(ReadOnlySpan<byte> text)
    {
        int index = MemoryExtensions.IndexOf<byte>(text, (byte)0);
        return (index < 0) ? text.Length : index;
    }

}
