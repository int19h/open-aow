// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Formats;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

using Aow.Game;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Storage;

// _partial, _dataStart. The constructor reads the root property table. every property is found
// through the table and read at _dataStart + offset
public partial class StorageReader : StorageStream
{
    private readonly bool _partial;
    private int _dataStart;

    // The image of the object being read for a class whose stream methods are retired: where its
    // property table starts (-1 when the object is not stored) and where its data ends.
    private int _objectStart;
    private int _dataEnd;

    // Engine.pas:9968
    public StorageReader(Stream stream, GameEngine engine, bool partial) : base(stream, engine)
    {
        _propertyTable = new PropertyTable();
        _objectStart = (int)stream.Position;
        _propertyTable.LoadFromStream(stream);
        _dataStart = (int)(_stream.Position);
        _dataEnd = (int)stream.Length;
        _partial = partial;
    }

    // Engine.pas:9977
    public override void Dispose()
    {
        _propertyTable?.Dispose();
        base.Dispose();
    }

    // Engine.pas:9983
    public override Set<StorageModeElement> StorageMode()
    {
        Set<StorageModeElement> result;
        if (_partial)
        {
            result = Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.Partial);
        }
        else
        {
            result = Set<StorageModeElement>.Of(StorageModeElement.Read);
        }

        return result;
    }

    // Engine.pas:9991
    public override void Int32(ref int value, int id)
    {
        int offset;
        offset = _propertyTable.FindOffset(id);
        if (offset != -1)
        {
            if (_stream.Position != _dataStart + offset)
            {
                _stream.Position = _dataStart + offset;
            }

            _stream.ReadExactly(Rtl.Bytes.Of(ref value, 4));
        }
        else
        {
            value = 0;
        }
    }

    // Engine.pas:10005
    public override void Byte(ref byte value, int id)
    {
        int offset;
        offset = _propertyTable.FindOffset(id);
        if (offset != -1)
        {
            if (_stream.Position != _dataStart + offset)
            {
                _stream.Position = _dataStart + offset;
            }

            _stream.ReadExactly(Rtl.Bytes.Of(ref value, 1));
        }
        else
        {
            value = 0;
        }
    }

    // Engine.pas:10019
    public override void SByte(ref sbyte value, int id)
    {
        int offset;
        offset = _propertyTable.FindOffset(id);
        if (offset != -1)
        {
            if (_stream.Position != _dataStart + offset)
            {
                _stream.Position = _dataStart + offset;
            }

            _stream.ReadExactly(Rtl.Bytes.Of(ref value, 1));
        }
        else
        {
            value = 0;
        }
    }

    // Engine.pas:10033
    public override void UInt16(ref ushort value, int id)
    {
        int offset;
        offset = _propertyTable.FindOffset(id);
        if (offset != -1)
        {
            if (_stream.Position != _dataStart + offset)
            {
                _stream.Position = _dataStart + offset;
            }

            _stream.ReadExactly(Rtl.Bytes.Of(ref value, 2));
        }
        else
        {
            value = 0;
        }
    }

    // Engine.pas:10047
    public override void Int32(ref int value, int fallback, int id)
    {
        int offset;
        offset = _propertyTable.FindOffset(id);
        if (offset != -1)
        {
            if (_stream.Position != _dataStart + offset)
            {
                _stream.Position = _dataStart + offset;
            }

            _stream.ReadExactly(Rtl.Bytes.Of(ref value, 4));
        }
        else
        {
            value = fallback;
        }
    }

    // Engine.pas:10061
    public override void Byte(ref byte value, byte fallback, int id)
    {
        int offset;
        offset = _propertyTable.FindOffset(id);
        if (offset != -1)
        {
            if (_stream.Position != _dataStart + offset)
            {
                _stream.Position = _dataStart + offset;
            }

            _stream.ReadExactly(Rtl.Bytes.Of(ref value, 1));
        }
        else
        {
            value = fallback;
        }
    }

    // Engine.pas:10075
    public override void SByte(ref sbyte value, sbyte fallback, int id)
    {
        int offset;
        offset = _propertyTable.FindOffset(id);
        if (offset != -1)
        {
            if (_stream.Position != _dataStart + offset)
            {
                _stream.Position = _dataStart + offset;
            }

            _stream.ReadExactly(Rtl.Bytes.Of(ref value, 1));
        }
        else
        {
            value = fallback;
        }
    }

    // Engine.pas:10089
    public override void UInt16(ref ushort value, ushort fallback, int id)
    {
        int offset;
        offset = _propertyTable.FindOffset(id);
        if (offset != -1)
        {
            if (_stream.Position != _dataStart + offset)
            {
                _stream.Position = _dataStart + offset;
            }

            _stream.ReadExactly(Rtl.Bytes.Of(ref value, 2));
        }
        else
        {
            value = fallback;
        }
    }

    // Engine.pas:10103
    public override void String(ref string value, int id)
    {
        int offset;
        offset = _propertyTable.FindOffset(id);
        if (offset != -1)
        {
            if (_stream.Position != _dataStart + offset)
            {
                _stream.Position = _dataStart + offset;
            }

            LoadStringFromStream(out value, _stream);
        }
        else
        {
            value = "";
        }
    }

    // Engine.pas:10117
    public override void Lines(StringCollection lines, int id)
    {
        int offset;
        offset = _propertyTable.FindOffset(id);
        if (offset != -1)
        {
            if (_stream.Position != _dataStart + offset)
            {
                _stream.Position = _dataStart + offset;
            }

            StringsFromStream(lines, _stream);
        }
        else
        {
            lines.Clear();
        }
    }

    // Engine.pas:10131
    public override void LinesWithObjects(StringCollection lines, int id)
    {
        int offset;
        offset = _propertyTable.FindOffset(id);
        if (offset != -1)
        {
            if (_stream.Position != _dataStart + offset)
            {
                _stream.Position = _dataStart + offset;
            }

            StringsFromStreamEx(lines, _stream);
        }
        else
        {
            lines.Clear();
        }
    }

    // Engine.pas:10145
    public override void Bytes(Span<byte> buffer, int size, int id)
    {
        int offset;
        offset = _propertyTable.FindOffset(id);
        if (offset != -1)
        {
            if (_stream.Position != _dataStart + offset)
            {
                _stream.Position = _dataStart + offset;
            }

            _stream.ReadExactly(buffer.Slice(0, size));
        }
    }

    // Engine.pas:10157
    public override void Payload(int id, StreamProc load, StreamProc save)
    {
        int offset;
        offset = _propertyTable.FindOffset(id);
        if (offset != -1)
        {
            if (_stream.Position != _dataStart + offset)
            {
                _stream.Position = _dataStart + offset;
            }

            try
            {
                load(_stream);
            }
            catch
            {
            }
        }
    }

    // Engine.pas:10172
    public override Stream OpenProperty(int id)
    {
        Stream result;
        int offset;
        offset = _propertyTable.FindOffset(id);
        if (offset != -1)
        {
            if (_stream.Position != _dataStart + offset)
            {
                _stream.Position = _dataStart + offset;
            }

            result = _stream;
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Engine.pas:10186
    public override void Boolean(ref bool value, int id)
    {
        int offset;
        offset = _propertyTable.FindOffset(id);
        value = offset != -1;
    }

    // Engine.pas:10191
    public bool HandleException(Exception error)
    {
        bool canContinue;
        if (!_errorHandled)
        {
            canContinue = true;
            if (HasErrorHandlers)
            {
                if (IsEmpty(error.Message))
                {
                    RaiseError(this, s_readError, ref canContinue);
                }
                else
                {
                    RaiseError(this, error.Message, ref canContinue);
                }
            }

            if (!canContinue)
            {
                _errorHandled = true;
            }
        }

        return !_errorHandled;
    }

    // Engine.pas:10208
    public override void Created(ref EngineObject objectValue, ECustomNode owner, int id)
    {
        int offset;
        int savedStart;
        int classId = default;
        objectValue = null;
        offset = _propertyTable.FindOffset(id);
        if (offset != -1)
        {
            if (_stream.Position != _dataStart + offset)
            {
                _stream.Position = _dataStart + offset;
            }

            _stream.ReadExactly(Rtl.Bytes.Of(ref classId, 4));
            if (classId != 0)
            {
                savedStart = _dataStart;
                var (savedObjectStart, savedEnd) = (_objectStart, _dataEnd);
                _dataEnd = EndOf(offset);
                _objectStart = (int)_stream.Position;
                _propertyTable.BackupSettings();
                _propertyTable.LoadFromStream(_stream);
                _dataStart = (int)(_stream.Position);
                try
                {
                    objectValue = _engine.CreateEngineObject(classId, null);
                    objectValue.SetOwnerWithoutNotify(owner);
                    PersistObject(objectValue);
                    objectValue.SetOwnerWithoutNotify(null);
                    objectValue.SetOwner(owner);
                    _propertyTable.RestoreSettings();
                    _dataStart = savedStart;
                    (_objectStart, _dataEnd) = (savedObjectStart, savedEnd);
                }
                catch (Exception error)
                {
                    objectValue?.Dispose();
                    objectValue = null;
                    _propertyTable.RestoreSettings();
                    _dataStart = savedStart;
                    (_objectStart, _dataEnd) = (savedObjectStart, savedEnd);
                    if (!HandleException(error))
                    {
                        throw;
                    }
                }
            }
        }
    }

    // Engine.pas:10246
    public override void Children(ECustomNode node, int id)
    {
        int offset;
        int savedStart;
        int childId;
        EngineObject child = default;
        offset = _propertyTable.FindOffset(id);
        if (offset != -1)
        {
            savedStart = _dataStart;
            int savedEnd = _dataEnd;
            _dataEnd = EndOf(offset);
            _propertyTable.BackupSettings();
            if (_stream.Position != _dataStart + offset)
            {
                _stream.Position = _dataStart + offset;
            }

            _propertyTable.LoadFromStream(_stream);
            _dataStart = (int)(_stream.Position);
            if (node.GetControlStyle().Contains(EngineServices.KeepIndex))
            {
                for (int i = _propertyTable.First, last = _propertyTable.Count - 1; i <= last; i++)
                {
                    childId = _propertyTable.Items[i].Id;
                    Created(ref child, node, childId);
                    if (child != null)
                    {
                        child.SetArrayOwner(node, childId);
                    }
                }
            }
            else
            {
                for (int i = _propertyTable.First, last = _propertyTable.Count - 1; i <= last; i++)
                {
                    childId = _propertyTable.Items[i].Id;
                    Created(ref child, node, childId);
                }
            }

            _propertyTable.RestoreSettings();
            _dataStart = savedStart;
            _dataEnd = savedEnd;
        }
    }

    // Engine.pas:10277
    public override void Object(EngineObject objectValue, int id)
    {
        int offset;
        int savedStart;
        savedStart = _dataStart;
        var (savedObjectStart, savedEnd) = (_objectStart, _dataEnd);
        offset = _propertyTable.FindOffset(id);
        _objectStart = -1;
        if (offset != -1)
        {
            _dataEnd = EndOf(offset);
            _objectStart = _dataStart + offset;
        }

        _propertyTable.BackupSettings();
        if (offset != -1)
        {
            if (_stream.Position != _dataStart + offset)
            {
                _stream.Position = _dataStart + offset;
            }

            _propertyTable.LoadFromStream(_stream);
            _dataStart = (int)(_stream.Position);
        }

        try
        {
            PersistObject(objectValue);
            _propertyTable.RestoreSettings();
            _dataStart = savedStart;
            (_objectStart, _dataEnd) = (savedObjectStart, savedEnd);
        }
        catch (Exception error)
        {
            _propertyTable.RestoreSettings();
            _dataStart = savedStart;
            (_objectStart, _dataEnd) = (savedObjectStart, savedEnd);
            if (!HandleException(error))
            {
                throw;
            }
        }
    }

    // Engine.pas:10304
    public override void SizedObject(EngineObject objectValue, int id)
    {
        int savedStart;
        int start;
        int size = default;
        start = (int)(_stream.Position);
        _stream.ReadExactly(Rtl.Bytes.Of(ref size, 4));
        if (size != 0)
        {
            savedStart = _dataStart;
            var (savedObjectStart, savedEnd) = (_objectStart, _dataEnd);
            _objectStart = start + 4;
            _dataEnd = start + size + 4;
            _propertyTable.BackupSettings();
            try
            {
                _propertyTable.LoadFromStream(_stream);
                _dataStart = (int)(_stream.Position);
                PersistObject(objectValue);
                _propertyTable.RestoreSettings();
                _dataStart = savedStart;
                (_objectStart, _dataEnd) = (savedObjectStart, savedEnd);
                _stream.Position = start + size + 4;
            }
            catch (Exception error)
            {
                _propertyTable.RestoreSettings();
                _dataStart = savedStart;
                (_objectStart, _dataEnd) = (savedObjectStart, savedEnd);
                _stream.Position = start + size + 4;
                if (!HandleException(error))
                {
                    throw;
                }
            }
        }
    }

    // Where a value stored at an offset of the current table ends: at the next larger offset, or
    // at the end of the current object.
    private int EndOf(int offset) => _propertyTable.NextOffset(offset) is var next and >= 0 ? _dataStart + next : _dataEnd;

    // The original backend reads the object's image, and the tree loads into the object, where
    // MainReadWrite would have read it. A failure inside goes to this stream's handlers.
    private protected override void PersistTree(EngineObject objectValue)
    {
        var factory = new EnginePersistableFactory(_engine ?? objectValue.GetEngine());
        ObjectNode root;
        if (_objectStart < 0)
        {
            root = new ObjectNode(null, null);
        }
        else
        {
            var schema = FormatSchema.Current;
            var classValue = schema.Get(factory.ClassName(objectValue));
            byte[] image = new byte[_dataEnd - _objectStart];
            long position = _stream.Position;
            _stream.Position = _objectStart;
            _stream.ReadExactly(image);
            _stream.Position = position;
            try
            {
                root = OriginalBackend.Read(image, classValue: classValue, schema: schema, engine: ClassTable(classValue));
            }
            catch (ConversionException error) when (error.Diagnostic.Code == Codes.UnknownClass && int.TryParse(error.Diagnostic.Value, out int classId))
            {
                // The stream's own load of a class id its table does not hold failed as
                // TEngine.CreateEObject fails.
                throw new KeyNotFoundException("Unregistered E-ClassID found (" + IntToStr(classId) + ")", error);
            }
        }

        TreeReader.Read(root, objectValue: objectValue, factory: factory, onError: ContinueAfter, preview: _partial);
    }

    private void ContinueAfter(string message, out bool continueLoad) =>
        continueLoad = HandleException(new InvalidOperationException(message));

    public int DataStart { get => _dataStart; }

    private static readonly string s_readError = "Error Reading EObject";

    internal static void StringsFromStream(StringCollection lines, Stream stream)
    {
        int length = 0;
        lines.BeginUpdate();
        try
        {
            stream.ReadExactly(Rtl.Bytes.Of(ref length, 4));
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException((string)null, "Negative string length");
            }
            byte[] array = new byte[length];
            stream.ReadExactly((Span<byte>)(array));
            lines.Text = Cp1252.Decode((ReadOnlySpan<byte>)(array));
        }
        finally
        {
            lines.EndUpdate();
        }
    }

    internal static void LoadStringFromStream(out string value, Stream stream)
    {
        int length = 0;
        byte b = 0;
        value = "";
        stream.ReadExactly(Rtl.Bytes.Of(ref b, 1));
        if (b == 255)
        {
            stream.ReadExactly(Rtl.Bytes.Of(ref length, 4));
        }
        else
        {
            length = b;
        }
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException((string)null, "Negative string length");
        }
        byte[] array = new byte[length];
        Cp1252.Encode(value)[..Math.Min(value.Length, length)].CopyTo(array);
        value = Cp1252.Decode((ReadOnlySpan<byte>)(array));
        stream.ReadExactly((Span<byte>)(array));
        value = Cp1252.Decode((ReadOnlySpan<byte>)(array));
    }

    internal static void StringsFromStreamEx(StringCollection lines, Stream stream)
    {
        int length = 0;
        int objectValue = 0;
        lines.BeginUpdate();
        try
        {
            stream.ReadExactly(Rtl.Bytes.Of(ref length, 4));
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException((string)null, "Negative string length");
            }
            byte[] array = new byte[length];
            stream.ReadExactly((Span<byte>)(array));
            lines.Text = Cp1252.Decode((ReadOnlySpan<byte>)(array));
            int i = 0;
            for (int last = lines.Count - 1; i <= last; i++)
            {
                stream.ReadExactly(Rtl.Bytes.Of(ref objectValue, 4));
                lines.Objects[i] = RtlUnit.IntToObject(objectValue);
            }
        }
        finally
        {
            lines.EndUpdate();
        }
    }

}
