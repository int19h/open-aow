using System.Runtime.CompilerServices;
using Aow.Rtl;

namespace Aow.Formats.Tree;

/// <summary>
/// The load's <see cref="LoadErrorHandler"/>, with the same stop flag as the original stream.
/// A null handler continues. Once a handler stops the load, later exceptions stop too.
/// An empty message is reported as "Error Reading EngineObject".
/// </summary>
sealed class LoadErrorPolicy
{
    private readonly LoadErrorHandler? _handler;
    private bool _stopped;

    internal LoadErrorPolicy(LoadErrorHandler? handler) => this._handler = handler;

    internal bool Continue(System.Exception error)
    {
        if (_stopped) return false;
        bool continueLoad = true;
        if (_handler is not null)
        {
            string message = error.Message.Length == 0 ? "Error Reading EObject" : error.Message;
            _handler(message, out continueLoad);
        }
        if (!continueLoad) _stopped = true;
        return !_stopped;
    }
}

/// <summary>Writes one object into a tree. The root carries no class name. Created objects and children do.</summary>
public static class TreeWriter
{
    public static ObjectNode Write(IPersistable objectValue, IPersistableFactory factory)
    {
        var node = new ObjectNode(null, null);
        var persister = new MemberPersister(node, false, factory, DocumentNodePath.Root);
        objectValue.Persist(persister);
        return node;
    }
}

/// <summary>
/// Reads a tree into an object. After <see cref="IPersistable.Persist"/> returns, a member the
/// serializer did not consume is an error. The path names that member.
/// </summary>
public static class TreeReader
{
    public static void Read(ObjectNode node, IPersistable objectValue, IPersistableFactory factory, LoadErrorHandler? onError = null)
    {
        var persister = new MemberPersister(node, true, factory, DocumentNodePath.Root, new LoadErrorPolicy(onError));
        objectValue.Persist(persister);
        persister.Finish();
    }
}

/// <summary>The persister over one object or one record.</summary>
sealed class MemberPersister : IPersister
{
    private readonly IReadOnlyList<(string Name, TreeValue Value)> _existing;
    private readonly Action<string, TreeValue>? _add;
    // The members a load has read. A save reads none, so the set is made by the first read.
    private HashSet<string>? _consumed;
    private readonly DocumentNodePath _path;
    private readonly LoadErrorPolicy? _errors;

    // The keyed dynamic members this object has taken on load or started on save.
    private Dictionary<string, SlotListNode?>? _slotMembers;

    public MemberPersister(ObjectNode node, bool loading, IPersistableFactory factory, DocumentNodePath path, LoadErrorPolicy? errors = null)
        : this(node.Members, loading ? null : node.Add, loading, factory, path, errors)
    {
    }

    public MemberPersister(RecordNode node, bool loading, IPersistableFactory factory, DocumentNodePath path, LoadErrorPolicy? errors = null)
        : this(node.Members, loading ? null : node.Add, loading, factory, path, errors)
    {
    }

    private MemberPersister(IReadOnlyList<(string Name, TreeValue Value)> existing, Action<string, TreeValue>? add, bool loading, IPersistableFactory factory, DocumentNodePath path, LoadErrorPolicy? errors)
    {
        this._existing = existing;
        this._add = add;
        Loading = loading;
        Factory = factory;
        this._path = path;
        this._errors = errors;
    }

    public bool Loading { get; }
    public IPersistableFactory Factory { get; }

    public bool Has(string name) => Loading && (Find(name) ?? FindOld(name).Value) is not null;

    public void Finish()
    {
        if (!Loading) return;
        foreach (var (name, _) in _existing)
            if (_consumed is null || !_consumed.Contains(name))
                throw TreeErrors.Fail(Codes.Required, $"Member \"{name}\" was not read.", _path.Member(name));
    }

    public int Int32(string name, int value, int fallback = 0) =>
        (int)Integer(name, value, fallback, 4, true);

    public byte Byte(string name, byte value, byte fallback = 0) =>
        (byte)Integer(name, value, fallback, 1, false);

    public sbyte SByte(string name, sbyte value, sbyte fallback = 0) =>
        (sbyte)Integer(name, value, fallback, 1, true);

    public ushort UInt16(string name, ushort value, ushort fallback = 0) =>
        (ushort)Integer(name, value, fallback, 2, false);

    public short Int16(string name, short value, short fallback = 0) =>
        (short)Integer(name, value, fallback, 2, true);

    public uint UInt32(string name, uint value, uint fallback = 0) =>
        (uint)Integer(name, value, fallback, 4, false);

    public TEnum Enum<TEnum>(string name, TEnum value, TEnum fallback = default) where TEnum : struct, Enum
    {
        var (width, signed) = EnumTypeInfo.Layout(typeof(TEnum));
        string typeName = NameTable.Stored(typeof(TEnum));
        if (Loading)
        {
            var found = Take(name);
            if (found is null) return fallback;
            if (found is not EnumNode node || !NameTable.Names(typeof(TEnum), node.TypeName) || node.Width != width || node.Signed != signed)
                throw Type(name, "a " + typeName);
            return (TEnum)System.Enum.ToObject(typeof(TEnum), node.Value);
        }
        Store(name, new EnumNode(typeName, width, signed, Convert.ToInt64(value)));
        return value;
    }

    public Set<E> Set<E>(string name, Set<E> value) where E : unmanaged, Enum
    {
        int width = default(Set<E>).StorageSize;
        string typeName = NameTable.Stored(typeof(E));
        if (width is < 1 or > 8)
            throw new InvalidOperationException("A set image is 1 to 8 bytes.");
        if (Loading)
        {
            var found = Take(name);
            if (found is null) return value;
            if (found is not SetNode node || !NameTable.Names(typeof(E), node.TypeName) || node.Width != width)
                throw Type(name, "a set of " + typeName);
            return Rtl.Set<E>.FromMask(node.Mask);
        }
        Span<byte> image = stackalloc byte[width];
        value.CopyImageTo(image);
        ulong mask = 0;
        for (int i = 0; i < width; i++) mask |= (ulong)image[i] << (8 * i);
        Store(name, new SetNode(typeName, width, mask));
        return value;
    }

    public bool Boolean(string name, bool value)
    {
        if (Loading)
        {
            return Take(name) switch
            {
                null => false,
                BooleanNode node => node.Value,
                _ => throw Type(name, "a boolean"),
            };
        }
        if (value) Store(name, new BooleanNode(true));
        return value;
    }

    public string String(string name, string value)
    {
        if (Loading)
        {
            return Take(name) switch
            {
                null => "",
                StringNode node => node.Value,
                _ => throw Type(name, "a string"),
            };
        }
        if (!string.IsNullOrEmpty(value)) Store(name, new StringNode(value));
        return value;
    }

    public void Lines(string name, IList<string> lines)
    {
        if (Loading)
        {
            lines.Clear();
            var found = Take(name);
            if (found is null) return;
            if (found is not LinesNode node) throw Type(name, "lines");
            foreach (var line in node.Lines ?? Split(node.RawText ?? "")) lines.Add(line);
            return;
        }
        if (lines.Count > 0) Store(name, LinesNode.FromLines(lines is List<string> list ? list : [.. lines]));
    }

    public void LinesWithObjects(string name, IList<string> lines, IList<int> objects)
    {
        if (Loading)
        {
            lines.Clear();
            objects.Clear();
            var found = Take(name);
            if (found is null) return;
            if (found is not LinesWithValuesNode node) throw Type(name, "lines with objects");
            foreach (var line in node.Lines ?? Split(node.RawText ?? "")) lines.Add(line);
            if (lines.Count != node.Objects.Count)
                throw TreeErrors.Fail(Codes.Corrupt, $"The text has {lines.Count} lines and {node.Objects.Count} object values.", _path.Member(name));
            foreach (var item in node.Objects) objects.Add(item);
            return;
        }
        if (lines.Count == 0) return;
        if (lines.Count != objects.Count)
            throw TreeErrors.Fail(Codes.Corrupt, $"The text has {lines.Count} lines and {objects.Count} object values.", _path.Member(name));
        Store(name, LinesWithValuesNode.FromLines(lines is List<string> list ? list : [.. lines], objects is List<int> ids ? ids : [.. objects]));
    }

    public void Bytes(string name, Span<byte> data)
    {
        if (Loading)
        {
            var found = Take(name);
            if (found is null) return;
            if (found is not BytesNode node)
                throw Type(name, "bytes");
            if (node.Data.Length != data.Length)
                throw TreeErrors.Fail(Codes.Range, $"{name} holds exactly {data.Length} bytes.", _path.Member(name), value: node.Data.Length.ToString(System.Globalization.CultureInfo.InvariantCulture), constraint: $"{data.Length} bytes");
            node.Data.CopyTo(data);
            return;
        }
        if (data.Length == 0) return;
        Store(name, new BytesNode(data.ToArray()));
    }

    public int RawInt32(string name, int value) => (int)RawInteger(name, value, 4, true);

    public byte RawByte(string name, byte value) => (byte)RawInteger(name, value, 1, false);

    public sbyte RawSByte(string name, sbyte value) => (sbyte)RawInteger(name, value, 1, true);

    // The bool keeps its stored byte, so a byte other than 0 or 1 loads as that byte and saves back.
    public bool RawBool(string name, bool value)
    {
        if (Loading)
        {
            var found = Take(name);
            if (found is null) return value;
            byte loaded;
            if (found is BooleanNode node) loaded = node.Value ? (byte)1 : (byte)0;
            else if (found is IntegerNode { Width: 1, Signed: false } number && number.Value is >= 0 and <= 255)
                loaded = (byte)number.Value;
            else throw Type(name, "a boolean");
            return Unsafe.As<byte, bool>(ref loaded);
        }
        byte stored = Unsafe.As<bool, byte>(ref value);
        if (stored is 0 or 1) Store(name, new BooleanNode(stored != 0));
        else Store(name, new IntegerNode(1, false, stored));
        return value;
    }

    public void BitArray(string name, int count, Span<byte> bits)
    {
        int length = count < 0 ? -1 : (count + 7) >> 3;
        if (length < 0)
            throw TreeErrors.Fail(Codes.Range, name + " has a negative count.", _path.Member(name), value: count.ToString(System.Globalization.CultureInfo.InvariantCulture));
        if (bits.Length != length)
            throw TreeErrors.Fail(Codes.Range, name + " holds exactly " + length.ToString(System.Globalization.CultureInfo.InvariantCulture) + " bytes.", _path.Member(name), value: bits.Length.ToString(System.Globalization.CultureInfo.InvariantCulture), constraint: length.ToString(System.Globalization.CultureInfo.InvariantCulture) + " bytes");
        if (Loading)
        {
            var found = Take(name);
            if (length == 0)
            {
                if (found is not null) throw Type(name, "omitted when the count is zero");
                return;
            }
            if (found is null) return;
            if (found is not BitArrayNode node || node.Count != count) throw Type(name, "a bit array");
            bits.Clear();
            foreach (var index in node.Indices)
            {
                if ((uint)index >= (uint)count)
                    throw TreeErrors.Fail(Codes.Range, name + " is outside its storage range.", _path.Member(name), value: index.ToString(System.Globalization.CultureInfo.InvariantCulture));
                bits[index >> 3] |= (byte)(1 << (index & 7));
            }
            if ((count & 7) != 0) bits[length - 1] |= node.Residue;
            return;
        }
        if (length == 0) return;
        var indices = new List<int>();
        for (int i = 0; i < count; i++)
            if ((bits[i >> 3] & (1 << (i & 7))) != 0) indices.Add(i);
        byte residue = 0;
        if ((count & 7) != 0) residue = (byte)(bits[length - 1] & (byte)(0xFF << (count & 7)));
        Store(name, new BitArrayNode(count, indices.ToArray(), residue));
    }

    public BitListImage? BitList(string name, BitListImage value)
    {
        if (Loading)
        {
            var found = Take(name);
            if (found is null) return null;
            if (found is not BitListNode node) throw Type(name, "a bit list");
            var bits = new byte[(node.Count + 7) >> 3];
            foreach (int index in node.Indices) bits[index >> 3] |= (byte)(1 << (index & 7));
            if ((node.Count & 7) != 0 && bits.Length != 0) bits[^1] |= node.Residue;
            return new BitListImage(node.Default, node.Count, bits);
        }
        if (value.Count < 0 || value.Bits.Length != (value.Count + 7L) >> 3)
            throw TreeErrors.Fail(Codes.Range, name + " of " + value.Count.ToString(System.Globalization.CultureInfo.InvariantCulture) + " bits holds " + ((value.Count + 7L) >> 3).ToString(System.Globalization.CultureInfo.InvariantCulture) + " bytes.", _path.Member(name));
        var indices = new List<int>();
        for (int i = 0; i < value.Count; i++)
            if ((value.Bits[i >> 3] & (1 << (i & 7))) != 0) indices.Add(i);
        byte residue = (value.Count & 7) != 0 ? (byte)(value.Bits[^1] & (byte)(0xFF << (value.Count & 7))) : (byte)0;
        Store(name, new BitListNode(value.Default, value.Count, [.. indices], residue));
        return value;
    }

    public float Single(string name, float value)
    {
        if (Loading)
        {
            var found = Take(name);
            if (found is null) return value;
            if (found is not SingleNode node) throw Type(name, "a single");
            return node.Value;
        }
        Store(name, new SingleNode(BitConverter.SingleToInt32Bits(value)));
        return value;
    }

    public byte[]? Blob(string name, byte[] data)
    {
        if (Loading)
        {
            var found = Take(name);
            if (found is null) return null;
            if (found is not BytesNode node) throw Type(name, "bytes");
            return node.Data;
        }
        Store(name, new BytesNode(data));
        return data;
    }

    public void SBytePairs(string name, IList<(sbyte First, sbyte Second)> pairs)
    {
        if (Loading)
        {
            var found = Take(name);
            if (found is null) return;
            if (found is not PairListNode node) throw Type(name, "pairs of signed bytes");
            pairs.Clear();
            foreach (var pair in node.Pairs) pairs.Add(pair);
            return;
        }
        Store(name, new PairListNode([.. pairs]));
    }

    public void ImageSequences(string name, IList<IReadOnlyList<SequenceLayer>?> entries)
    {
        var at = _path.Member(name);
        if (Loading)
        {
            var found = Take(name);
            if (found is null) return;
            if (found is not ImageSequenceListNode node) throw Type(name, "an image sequence list");
            entries.Clear();
            foreach (var entry in node.Entries) entries.Add(entry);
            return;
        }
        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i] is not { } layers) continue;
            if (layers.Count == 0)
                throw TreeErrors.Fail(Codes.Type, "A sequence of " + name + " has at least one layer.", at.Item(i));
            for (int k = 0; k < layers.Count; k++)
                if ((layers[k].Frames is not null) != SequenceLayer.HasFrameTable(layers[k].Mode))
                    throw TreeErrors.Fail(Codes.Type, "A layer stores a frame table exactly when its mode is FrameTable or RandomFrameTable.", at.Item(i).Item(k));
        }
        Store(name, new ImageSequenceListNode([.. entries]));
    }

    public void Object(string name, IPersistable objectValue)
    {
        if (Loading)
        {
            var found = Take(name);
            ObjectNode nested;
            if (found is null) nested = new ObjectNode(null, null);
            else if (found is ObjectNode node) nested = node;
            else throw Type(name, "an object");
            var child = new MemberPersister(nested, true, Factory, _path.Member(name), _errors);
            // rwEObject wraps MainReadWrite and leaves the caller's instance in place.
            try
            {
                objectValue.Persist(child);
                child.Finish();
            }
            catch (System.Exception error)
            {
                if (!Continue(error)) throw;
            }
            return;
        }
        var written = new ObjectNode(null, null);
        objectValue.Persist(new MemberPersister(written, false, Factory, _path.Member(name), _errors));
        Store(name, written);
    }

    public IPersistable? Created(string name, IPersistable? objectValue)
    {
        if (Loading)
        {
            var found = Take(name);
            if (found is null || found is NullNode) return null;
            if (found is not ObjectNode node || node.ClassName is not string className)
                throw Type(name, "a created object");
            return Create(node, className, _path.Member(name));
        }
        if (objectValue is null)
        {
            Store(name, NullNode.Instance);
            return null;
        }
        var written = new ObjectNode(Factory.ClassName(objectValue), null);
        objectValue.Persist(new MemberPersister(written, false, Factory, _path.Member(name), _errors));
        Store(name, written);
        return objectValue;
    }

    public T? Created<T>(string name, T? objectValue) where T : class, IPersistable
    {
        var box = Created(name, (IPersistable?)objectValue);
        if (box is not null && box is not T)
            throw TreeErrors.Fail(Codes.ClassFamily, $"Created class {Factory.ClassName(box)} is not a {typeof(T).Name}.", _path.Member(name), value: Factory.ClassName(box), constraint: typeof(T).Name);
        return (T?)box;
    }

    public IPersistable? CreatedSlot(string name, int slot, IPersistable? objectValue)
    {
        var at = _path.Member(name).Item(slot);
        if (slot < 0)
            throw TreeErrors.Fail(Codes.Range, "A slot of " + name + " is negative.", at, value: slot.ToString(System.Globalization.CultureInfo.InvariantCulture));
        _slotMembers ??= new Dictionary<string, SlotListNode?>(StringComparer.Ordinal);
        if (Loading)
        {
            if (!_slotMembers.TryGetValue(name, out var stored))
            {
                var found = Take(name);
                if (found is not null and not SlotListNode) throw Type(name, "created objects at slots");
                stored = (SlotListNode?)found;
                _slotMembers[name] = stored;
            }
            var value = stored?.Find(slot);
            if (value is null || value is NullNode) return null;
            if (value is not ObjectNode node || node.ClassName is not string className)
                throw TreeErrors.Fail(Codes.Type, "A slot of " + name + " is a created object.", at);
            return Create(node, className, at);
        }
        if (!_slotMembers.TryGetValue(name, out var list) || list is null)
        {
            list = new SlotListNode();
            Store(name, list);
            _slotMembers[name] = list;
        }
        if (list.Find(slot) is not null)
            throw TreeErrors.Fail(Codes.Slot, "Slot " + slot.ToString(System.Globalization.CultureInfo.InvariantCulture) + " of " + name + " is stored twice.", at);
        if (objectValue is null)
        {
            list.Add(slot, NullNode.Instance);
            return null;
        }
        var written = new ObjectNode(Factory.ClassName(objectValue), slot);
        objectValue.Persist(new MemberPersister(written, loading: false, factory: Factory, path: at, errors: _errors));
        list.Add(slot, written);
        return objectValue;
    }

    public T? CreatedSlot<T>(string name, int slot, T? objectValue) where T : class, IPersistable
    {
        var box = CreatedSlot(name, slot, (IPersistable?)objectValue);
        if (box is not null && box is not T)
            throw TreeErrors.Fail(Codes.ClassFamily, $"Created class {Factory.ClassName(box)} is not a {typeof(T).Name}.", _path.Member(name).Item(slot), value: Factory.ClassName(box), constraint: typeof(T).Name);
        return (T?)box;
    }

    public void ObjectSlot(string name, int slot, IPersistable objectValue)
    {
        var at = _path.Member(name).Item(slot);
        if (slot < 0)
            throw TreeErrors.Fail(Codes.Range, "A slot of " + name + " is negative.", at, value: slot.ToString(System.Globalization.CultureInfo.InvariantCulture));
        _slotMembers ??= new Dictionary<string, SlotListNode?>(StringComparer.Ordinal);
        if (Loading)
        {
            if (!_slotMembers.TryGetValue(name, out var stored))
            {
                var found = Take(name);
                if (found is not null and not SlotListNode) throw Type(name, "fixed objects at slots");
                stored = (SlotListNode?)found;
                _slotMembers[name] = stored;
            }
            var value = stored?.Find(slot);
            if (value is null) return;
            if (value is not ObjectNode { ClassName: null } node)
                throw TreeErrors.Fail(Codes.Type, "A slot of " + name + " is a fixed object.", at);
            var reader = new MemberPersister(node, loading: true, factory: Factory, path: at, errors: _errors);
            // rwEObject wraps MainReadWrite and leaves the caller's instance in place.
            try
            {
                objectValue.Persist(reader);
                reader.Finish();
            }
            catch (System.Exception error)
            {
                if (!Continue(error)) throw;
            }
            return;
        }
        if (!_slotMembers.TryGetValue(name, out var list) || list is null)
        {
            list = new SlotListNode();
            Store(name, list);
            _slotMembers[name] = list;
        }
        if (list.Find(slot) is not null)
            throw TreeErrors.Fail(Codes.Slot, "Slot " + slot.ToString(System.Globalization.CultureInfo.InvariantCulture) + " of " + name + " is stored twice.", at);
        var written = new ObjectNode(null, slot);
        objectValue.Persist(new MemberPersister(written, loading: false, factory: Factory, path: at, errors: _errors));
        list.Add(slot, written);
    }

    public void Skip(string name)
    {
        if (Loading && (_consumed is null || !_consumed.Contains(name))) Take(name);
    }

    public int Int32List(string name, IList<int> items, int defaultValue)
    {
        if (Loading)
        {
            var found = Take(name);
            if (found is null) return defaultValue;
            if (found is not DefaultListNode { Width: 4 } node) throw Type(name, "a list of 32-bit integers with a default");
            items.Clear();
            foreach (var item in node.Items)
                items.Add(checked((int)item));
            return checked((int)node.Default);
        }
        var values = new long[items.Count];
        for (int i = 0; i < values.Length; i++) values[i] = items[i];
        Store(name, new DefaultListNode(4, defaultValue, values));
        return defaultValue;
    }

    public byte ByteList(string name, IList<byte> items, byte defaultValue)
    {
        if (Loading)
        {
            var found = Take(name);
            if (found is null) return defaultValue;
            if (found is not DefaultListNode { Width: 1 } node) throw Type(name, "a list of bytes with a default");
            items.Clear();
            foreach (var item in node.Items)
                items.Add(checked((byte)item));
            return checked((byte)node.Default);
        }
        var values = new long[items.Count];
        for (int i = 0; i < values.Length; i++) values[i] = items[i];
        Store(name, new DefaultListNode(1, defaultValue, values));
        return defaultValue;
    }

    public void Children(string name, IChildCollection children)
    {
        if (Loading)
        {
            var found = Take(name);
            IReadOnlyList<ObjectNode> list;
            if (found is null) list = [];
            else if (found is ChildListNode node) list = node.Children;
            else throw Type(name, "children");
            foreach (var child in list)
            {
                if (child.ClassName is not string className || child.Slot is not int slot)
                    throw TreeErrors.Fail(Codes.MissingMember, "A child needs a class and a slot.", _path.Member(name));
                IPersistable? created = null;
                try
                {
                    created = Factory.Create(className);
                    children.Attach(created);
                    var reader = new MemberPersister(child, true, Factory, _path.Member(name).Item(slot), _errors);
                    created.Persist(reader);
                    reader.Finish();
                    children.Place(slot, created);
                }
                catch (System.Exception error)
                {
                    if (created is not null) Factory.Release(created);
                    if (!Continue(error)) throw;
                }
            }
            return;
        }
        var written = new List<ObjectNode>();
        for (int i = 0; i < children.Count; i++)
        {
            if (!children.IsStored(i)) continue;
            var child = children.Get(i) ?? throw TreeErrors.Fail(Codes.Corrupt, "A stored child is missing.", _path.Member(name).Item(i));
            var node = new ObjectNode(Factory.ClassName(child), children.Slot(i));
            child.Persist(new MemberPersister(node, false, Factory, _path.Member(name).Item(children.Slot(i)), _errors));
            written.Add(node);
        }
        Store(name, new ChildListNode(written));
    }

    public void Grid(string name, int width, int height, IReadOnlyList<IPersistable> fields, Action<int>? visited = null)
    {
        var at = _path.Member(name);
        if (width < 0 || height < 0 || (long)width * height != fields.Count)
            throw TreeErrors.Fail(Codes.Dimensions, $"{name} holds {fields.Count} fields for a {width} by {height} grid.", at,
                value: fields.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), constraint: $"{(long)width * height} fields");
        if (Loading)
        {
            var found = Take(name);
            if (found is null) return;
            if (found is not GridNode node) throw Type(name, "a grid of fields");
            if (node.Width != width || node.Height != height)
                throw TreeErrors.Fail(Codes.Dimensions, $"{name} is a {node.Width} by {node.Height} grid and the object is {width} by {height}.", at,
                    value: $"{node.Width} by {node.Height}", constraint: $"{width} by {height}");
            for (int i = 0; i < fields.Count; i++)
            {
                // TEReadStorageStream.rwEObjectStream wraps MainReadWrite and leaves the field in place.
                var reader = new MemberPersister(node.Fields[i], loading: true, factory: Factory, path: FieldPath(at, i, width), errors: _errors);
                try
                {
                    fields[i].Persist(reader);
                    reader.Finish();
                }
                catch (System.Exception error)
                {
                    if (!Continue(error)) throw;
                }
                visited?.Invoke(i);
            }
            return;
        }
        var written = new ObjectNode[fields.Count];
        for (int i = 0; i < fields.Count; i++)
        {
            var field = new ObjectNode(null, null);
            fields[i].Persist(new MemberPersister(field, loading: false, factory: Factory, path: FieldPath(at, i, width), errors: _errors));
            written[i] = field;
            visited?.Invoke(i);
        }
        Store(name, new GridNode(width, height, written));
    }

    // The field at an index sits at [y][x] of the document's rows.
    private static DocumentNodePath FieldPath(DocumentNodePath grid, int index, int width)
    {
        int y = Math.DivRem(index, width, out int x);
        return grid.Item(y).Item(x);
    }

    public void CreatedSequence(string name, IChildCollection items)
    {
        var at = _path.Member(name);
        if (Loading)
        {
            var found = Take(name);
            if (found is null) return;
            if (found is not SequenceNode node) throw Type(name, "a sequence of created objects");
            for (int k = 0; k < node.Items.Count; k++)
            {
                // TEReadStorageStream.rwCreateEObject with an owner: a stored null creates nothing.
                if (node.Items[k] is NullNode) continue;
                if (node.Items[k] is not ObjectNode { ClassName: string className } item)
                    throw TreeErrors.Fail(Codes.Type, "An item of " + name + " is a created object.", at.Item(k));
                IPersistable? created = null;
                try
                {
                    created = Factory.Create(className);
                    items.Attach(created);
                    var reader = new MemberPersister(item, loading: true, factory: Factory, path: at.Item(k), errors: _errors);
                    created.Persist(reader);
                    reader.Finish();
                    items.Place(k, created);
                }
                catch (System.Exception error)
                {
                    if (created is not null) Factory.Release(created);
                    if (!Continue(error)) throw;
                }
            }
            return;
        }
        List<TreeValue>? written = null;
        for (int i = 0; i < items.Count; i++)
        {
            if (!items.IsStored(i)) continue;
            var item = items.Get(i) ?? throw TreeErrors.Fail(Codes.Corrupt, "A stored item is missing.", at.Item(i));
            written ??= [];
            var node = new ObjectNode(Factory.ClassName(item), null);
            item.Persist(new MemberPersister(node, loading: false, factory: Factory, path: at.Item(written.Count), errors: _errors));
            written.Add(node);
        }
        if (written is not null) Store(name, new SequenceNode(written));
    }

    public byte[] NestedDocument(string name, string profile, byte[] image)
    {
        if (Loading)
        {
            var found = Take(name);
            if (found is null) return [];
            if (found is not NestedDocumentNode node || node.Profile != profile) throw Type(name, "a nested " + profile + " document");
            return node.Image;
        }
        if (image.Length != 0) Store(name, new NestedDocumentNode(profile, image));
        return image;
    }

    public int Reference(string name, string domain, int key, int fallback = 0)
    {
        if (Loading)
        {
            var found = Take(name);
            if (found is null) return fallback;
            if (found is ReferenceNode reference)
            {
                if (reference.Domain != domain)
                    throw TreeErrors.Fail(Codes.Reference, $"Reference \"{name}\" is in domain {reference.Domain}.", _path.Member(name), value: reference.Domain, constraint: domain);
                return reference.Key;
            }
            if (found is IntegerNode integer && integer.Width == 4 && integer.Signed) return (int)integer.Value;
            throw Type(name, "a reference");
        }
        if (key != fallback) Store(name, new ReferenceNode(domain, key));
        return key;
    }

    public void Record(string name, Action<IPersister> body)
    {
        if (Loading)
        {
            var found = Take(name);
            RecordNode record;
            if (found is null) record = new RecordNode();
            else if (found is RecordNode node) record = node;
            else throw Type(name, "a record");
            var child = new MemberPersister(record, true, Factory, _path.Member(name), _errors);
            body(child);
            child.Finish();
            return;
        }
        var written = new RecordNode();
        body(new MemberPersister(written, false, Factory, _path.Member(name), _errors));
        Store(name, written);
    }

    public void List<T>(string name, IList<T> items, ItemTransfer<T> transfer)
    {
        if (Loading)
        {
            items.Clear();
            var found = Take(name);
            if (found is null) return;
            if (found is not ListNode list) throw Type(name, "a list");
            for (int i = 0; i < list.Items.Count; i++)
            {
                var slot = new ItemSlot(list.Items[i], true, Factory, _path.Member(name).Item(i), _errors);
                T value = transfer(slot, default!);
                slot.Finish();
                items.Add(value);
            }
            return;
        }
        if (items.Count == 0) return;
        var written = new ListNode();
        for (int i = 0; i < items.Count; i++)
        {
            var slot = new ItemSlot(null, false, Factory, _path.Member(name).Item(i), _errors);
            items[i] = transfer(slot, items[i]);
            written.Items.Add(slot.Finish());
        }
        Store(name, written);
    }

    private long Integer(string name, long current, long fallback, int width, bool signed)
    {
        if (Loading)
        {
            var found = Take(name);
            if (found is null) return fallback;
            if (found is IntegerNode node && node.Width == width && node.Signed == signed) return node.Value;
            if (found is ReferenceNode reference && width == 4 && signed) return reference.Key;
            throw Type(name, "an integer");
        }
        if (current != fallback) Store(name, new IntegerNode(width, signed, current));
        return current;
    }

    private TreeValue? Find(string name)
    {
        foreach (var (member, value) in _existing)
            if (member == name) return value;
        return null;
    }

    // An older document stores a renamed member under its old name (schema.json oldNames).
    private (string? Name, TreeValue? Value) FindOld(string name)
    {
        foreach (string old in Schema.FormatSchema.Current.OldNamesOf(name))
            if (Find(old) is { } value) return (old, value);
        return (null, null);
    }

    private TreeValue? Take(string name)
    {
        _consumed ??= new HashSet<string>(StringComparer.Ordinal);
        if (!_consumed.Add(name))
            throw TreeErrors.Fail(Codes.Duplicate, $"Member \"{name}\" is read twice.", _path.Member(name));
        if (Find(name) is { } found) return found;
        var (old, value) = FindOld(name);
        if (old is not null) _consumed.Add(old);
        return value;
    }

    private void Store(string name, TreeValue value)
    {
        if (Find(name) is not null)
            throw TreeErrors.Fail(Codes.Duplicate, $"Member \"{name}\" is stored twice.", _path.Member(name));
        _add!(name, value);
    }

    private ConversionException Type(string name, string expected) =>
        TreeErrors.Fail(Codes.Type, $"{name} is {expected}.", _path.Member(name));

    private long RawInteger(string name, long current, int width, bool signed)
    {
        if (Loading)
        {
            var found = Take(name);
            if (found is null) return current;
            if (found is IntegerNode node && node.Width == width && node.Signed == signed) return node.Value;
            throw Type(name, "an integer");
        }
        Store(name, new IntegerNode(width, signed, current));
        return current;
    }

    private bool Continue(System.Exception error) => _errors is null || _errors.Continue(error);

    // rwCreateEObject creates the object and runs its load. A failure releases the object, and the
    // load's handler decides whether the parent continues with null.
    private IPersistable? Create(ObjectNode node, string className, DocumentNodePath at)
    {
        IPersistable? created = null;
        try
        {
            created = Factory.Create(className);
            var child = new MemberPersister(node, loading: true, factory: Factory, path: at, errors: _errors);
            created.Persist(child);
            child.Finish();
            return created;
        }
        catch (System.Exception error)
        {
            if (created is not null) Factory.Release(created);
            if (!Continue(error)) throw;
            return null;
        }
    }

    private static List<string> Split(string text)
    {
        var lines = new List<string>();
        int i = 0;
        while (i < text.Length && text[i] != '\0')
        {
            int start = i;
            while (i < text.Length && text[i] is not ('\0' or '\r' or '\n')) i++;
            lines.Add(text[start..i]);
            if (i < text.Length && text[i] == '\r') i++;
            if (i < text.Length && text[i] == '\n') i++;
        }
        return lines;
    }
}

/// <summary>One list item, holding a single tree value.</summary>
sealed class ItemSlot : IItemSlot
{
    private readonly TreeValue? _loaded;
    private readonly bool _loading;
    private readonly IPersistableFactory _factory;
    private readonly DocumentNodePath _path;
    private readonly LoadErrorPolicy? _errors;
    private TreeValue? _stored;
    private bool _used;

    public ItemSlot(TreeValue? loaded, bool loading, IPersistableFactory factory, DocumentNodePath path, LoadErrorPolicy? errors = null)
    {
        this._loaded = loaded;
        this._loading = loading;
        this._factory = factory;
        this._path = path;
        this._errors = errors;
    }

    public TreeValue Finish()
    {
        if (!_used) throw TreeErrors.Fail(Codes.MissingMember, "A list item stores one value.", _path);
        return _loading ? _loaded! : _stored!;
    }

    public int Int32(int value) => (int)Integer(value, 4, true);

    public byte Byte(byte value) => (byte)Integer(value, 1, false);

    public sbyte SByte(sbyte value) => (sbyte)Integer(value, 1, true);

    public ushort UInt16(ushort value) => (ushort)Integer(value, 2, false);

    public short Int16(short value) => (short)Integer(value, 2, true);

    public uint UInt32(uint value) => (uint)Integer(value, 4, false);

    public bool Boolean(bool value)
    {
        Use();
        if (_loading)
        {
            if (_loaded is not BooleanNode node) throw TreeErrors.Fail(Codes.Type, "A list item is a boolean.", _path);
            return node.Value;
        }
        _stored = new BooleanNode(value);
        return value;
    }

    public string String(string value)
    {
        Use();
        if (_loading)
        {
            if (_loaded is not StringNode node) throw TreeErrors.Fail(Codes.Type, "A list item is a string.", _path);
            return node.Value;
        }
        // A null item stores as empty and stays null in the list, as the ref call left it.
        _stored = new StringNode(value ?? "");
        return value!;
    }

    public void Record(Action<IPersister> body)
    {
        Use();
        if (_loading)
        {
            if (_loaded is not RecordNode node) throw TreeErrors.Fail(Codes.Type, "A list item is a record.", _path);
            var child = new MemberPersister(node, true, _factory, _path, _errors);
            body(child);
            child.Finish();
            return;
        }
        var written = new RecordNode();
        body(new MemberPersister(written, false, _factory, _path, _errors));
        _stored = written;
    }

    public IPersistable? Object(IPersistable? value)
    {
        Use();
        if (_loading)
        {
            if (_loaded is NullNode || _loaded is null) return null;
            if (_loaded is not ObjectNode node || node.ClassName is not string className)
                throw TreeErrors.Fail(Codes.Type, "A list item is an object.", _path);
            IPersistable? created = null;
            try
            {
                created = _factory.Create(className);
                var child = new MemberPersister(node, true, _factory, _path, _errors);
                created.Persist(child);
                child.Finish();
                return created;
            }
            catch (System.Exception error)
            {
                if (created is not null) _factory.Release(created);
                if (_errors is not null && !_errors.Continue(error)) throw;
                return null;
            }
        }
        if (value is null)
        {
            _stored = NullNode.Instance;
            return null;
        }
        var written = new ObjectNode(_factory.ClassName(value), null);
        value.Persist(new MemberPersister(written, false, _factory, _path, _errors));
        _stored = written;
        return value;
    }

    private long Integer(long current, int width, bool signed)
    {
        Use();
        if (_loading)
        {
            if (_loaded is not IntegerNode node || node.Width != width || node.Signed != signed)
                throw TreeErrors.Fail(Codes.Type, "A list item is an integer.", _path);
            return node.Value;
        }
        _stored = new IntegerNode(width, signed, current);
        return current;
    }

    private void Use()
    {
        if (_used) throw TreeErrors.Fail(Codes.Duplicate, "A list item stores one value.", _path);
        _used = true;
    }
}
