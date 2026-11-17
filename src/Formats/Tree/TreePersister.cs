using System.Runtime.CompilerServices;
using Aow.Rtl;

namespace Aow.Formats.Tree;

/// <summary>
/// The load's <see cref="LoadErrorHandler"/>, with the same stop flag as the original stream.
/// A null handler continues. Once a handler stops the load, later exceptions stop too.
/// An empty message is reported as "Error Reading EngineObject".
/// </summary>
internal sealed class LoadErrorPolicy
{
    private readonly LoadErrorHandler? _handler;
    private bool _stopped;

    internal LoadErrorPolicy(LoadErrorHandler? handler) => _handler = handler;

    internal bool Continue(Exception error)
    {
        if (_stopped)
        {
            return false;
        }

        bool continueLoad = true;
        if (_handler is not null)
        {
            string message = error.Message.Length == 0 ? "Error Reading EObject" : error.Message;
            _handler(message, out continueLoad);
        }
        if (!continueLoad)
        {
            _stopped = true;
        }

        return !_stopped;
    }
}

/// <summary>
/// Writes one object into a tree. The root carries no class name. Created objects and children do.
/// A preview write sets <see cref="IPersister.Preview"/>, as the partial storage writer sets the
/// preview mode.
/// </summary>
public static class TreeWriter
{
    public static ObjectNode Write(IPersistable objectValue, IPersistableFactory factory, bool preview = false)
    {
        ObjectNode node = new(null, null);
        MemberPersister persister = new(node, false, factory, DocumentNodePath.Root, preview: preview);
        objectValue.Persist(persister);
        return node;
    }

    /// <summary>
    /// Writes one object the way <c>TEngine.WriteEObject</c> writes it (<c>rwEObject</c> with id
    /// 0): a failure of the root leaves the root out of the image, which a load then reads as an
    /// object with no stored member. The tree of such a write is an empty object node. A failure
    /// inside a nested object still throws (persistence-tree.md 13).
    /// </summary>
    public static ObjectNode WriteObject(IPersistable objectValue, IPersistableFactory factory)
    {
        try
        {
            return Write(objectValue, factory: factory);
        }
        catch (Exception)
        {
            return new ObjectNode(null, null);
        }
    }
}

/// <summary>
/// Reads a tree into an object. After <see cref="IPersistable.Persist"/> returns, a member the
/// serializer did not consume is an error. The path names that member. A preview load sets
/// <see cref="IPersister.Preview"/>, as the partial storage reader sets the preview mode.
/// </summary>
public static class TreeReader
{
    public static void Read(ObjectNode node, IPersistable objectValue, IPersistableFactory factory, LoadErrorHandler? onError = null, bool preview = false)
    {
        MemberPersister persister = new(node, true, factory, DocumentNodePath.Root, new LoadErrorPolicy(onError), preview: preview);
        objectValue.Persist(persister);
        persister.Finish();
    }

    /// <summary>
    /// Reads a tree into an object the caller holds the way <c>TEngine.ReadEObject</c> reads a
    /// stream (<c>rwEObject</c> with id 0): a failure of the root goes to the load's handler like a
    /// failure of a nested object. A handler that continues, or no handler, leaves the object as the
    /// load left it, and a handler that stops makes the load throw the original exception.
    /// </summary>
    public static void ReadObject(ObjectNode node, IPersistable objectValue, IPersistableFactory factory, LoadErrorHandler? onError = null, bool preview = false)
    {
        LoadErrorPolicy errors = new(onError);
        MemberPersister persister = new(node, loading: true, factory: factory, path: DocumentNodePath.Root, errors: errors, preview: preview);
        try
        {
            objectValue.Persist(persister);
            persister.Finish();
        }
        catch (Exception error)
        {
            if (!errors.Continue(error))
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Creates the root of a tree and reads it the way <c>TEngine.RetrieveEObject</c> reads a
    /// stream (<c>rwCreateEObject</c> with id 0): the factory creates the class, and a failure
    /// releases the object and goes to the load's handler. A handler that continues, or no
    /// handler, returns null. A handler that stops makes the load throw the original exception.
    /// </summary>
    public static IPersistable? ReadCreated(ObjectNode node, string className, IPersistableFactory factory, LoadErrorHandler? onError = null, bool preview = false)
    {
        LoadErrorPolicy errors = new(onError);
        IPersistable? created = null;
        try
        {
            created = factory.Create(className);
            MemberPersister persister = new(node, loading: true, factory: factory, path: DocumentNodePath.Root, errors: errors, preview: preview);
            created.Persist(persister);
            persister.Finish();
            return created;
        }
        catch (Exception error)
        {
            if (created is not null)
            {
                factory.Release(created);
            }

            if (!errors.Continue(error))
            {
                throw;
            }

            return null;
        }
    }
}

/// <summary>The persister over one object or one record.</summary>
internal sealed class MemberPersister : IPersister
{
    private readonly IReadOnlyList<(string Name, TreeValue Value)> _existing;
    private readonly Action<string, TreeValue>? _add;
    // The members a load has read. A save reads none, so the set is made by the first read.
    private HashSet<string>? _consumed;
    private readonly DocumentNodePath _path;
    private readonly LoadErrorPolicy? _errors;

    // The keyed dynamic members this object has taken on load or started on save.
    private Dictionary<string, SlotListNode?>? _slotMembers;

    public MemberPersister(ObjectNode node, bool loading, IPersistableFactory factory, DocumentNodePath path, LoadErrorPolicy? errors = null, bool preview = false)
        : this(node.Members, loading ? null : node.Add, loading, factory, path, errors, preview)
    {
    }

    public MemberPersister(RecordNode node, bool loading, IPersistableFactory factory, DocumentNodePath path, LoadErrorPolicy? errors = null, bool preview = false)
        : this(node.Members, loading ? null : node.Add, loading, factory, path, errors, preview)
    {
    }

    private MemberPersister(IReadOnlyList<(string Name, TreeValue Value)> existing, Action<string, TreeValue>? add, bool loading, IPersistableFactory factory, DocumentNodePath path, LoadErrorPolicy? errors, bool preview)
    {
        _existing = existing;
        _add = add;
        Loading = loading;
        Preview = preview;
        Factory = factory;
        _path = path;
        _errors = errors;
    }

    public bool Loading { get; }
    public bool Preview { get; }
    public IPersistableFactory Factory { get; }

    public bool Has(string name) => Loading && Find(name) is not null;

    public void Finish()
    {
        if (!Loading)
        {
            return;
        }

        foreach ((string? name, TreeValue _) in _existing)
        {
            if (_consumed is null || !_consumed.Contains(name))
            {
                throw TreeErrors.Fail(Codes.Required, $"Member \"{name}\" was not read.", _path.Member(name));
            }
        }
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
        (int width, bool signed) = EnumTypeInfo.Layout(typeof(TEnum));
        string typeName = NameTable.Stored(typeof(TEnum));
        if (Loading)
        {
            TreeValue? found = Take(name);
            return found is null
                ? fallback
                : found is not EnumNode node || !NameTable.Names(typeof(TEnum), node.TypeName) || node.Width != width || node.Signed != signed
                ? throw Type(name, "a " + typeName)
                : (TEnum)System.Enum.ToObject(typeof(TEnum), node.Value);
        }
        Store(name, new EnumNode(typeName, width, signed, Convert.ToInt64(value)));
        return value;
    }

    public Set<E> Set<E>(string name, Set<E> value) where E : unmanaged, Enum
    {
        int width = default(Set<E>).StorageSize;
        string typeName = NameTable.Stored(typeof(E));
        if (width is < 1 or > 8)
        {
            throw new InvalidOperationException("A set image is 1 to 8 bytes.");
        }

        if (Loading)
        {
            TreeValue? found = Take(name);
            return found is null
                ? value
                : found is not SetNode node || !NameTable.Names(typeof(E), node.TypeName) || node.Width != width
                ? throw Type(name, "a set of " + typeName)
                : Rtl.Set<E>.FromMask(node.Mask);
        }
        Span<byte> image = stackalloc byte[width];
        value.CopyImageTo(image);
        ulong mask = 0;
        for (int i = 0; i < width; i++)
        {
            mask |= (ulong)image[i] << (8 * i);
        }

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
        if (value)
        {
            Store(name, new BooleanNode(true));
        }

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
        if (!string.IsNullOrEmpty(value))
        {
            Store(name, new StringNode(value));
        }

        return value;
    }

    public void Lines(string name, IList<string> lines)
    {
        if (Loading)
        {
            lines.Clear();
            TreeValue? found = Take(name);
            if (found is null)
            {
                return;
            }

            if (found is not LinesNode node)
            {
                throw Type(name, "lines");
            }

            foreach (string line in node.Lines ?? Split(node.RawText ?? ""))
            {
                lines.Add(line);
            }

            return;
        }
        if (lines.Count > 0)
        {
            Store(name, LinesNode.FromLines(lines is List<string> list ? list : [.. lines]));
        }
    }

    public void LinesWithObjects(string name, IList<string> lines, IList<int> objects)
    {
        if (Loading)
        {
            lines.Clear();
            objects.Clear();
            TreeValue? found = Take(name);
            if (found is null)
            {
                return;
            }

            if (found is not LinesWithValuesNode node)
            {
                throw Type(name, "lines with objects");
            }

            foreach (string line in node.Lines ?? Split(node.RawText ?? ""))
            {
                lines.Add(line);
            }

            if (lines.Count != node.Objects.Count)
            {
                throw TreeErrors.Fail(Codes.Corrupt, $"The text has {lines.Count} lines and {node.Objects.Count} object values.", _path.Member(name));
            }

            foreach (int item in node.Objects)
            {
                objects.Add(item);
            }

            return;
        }
        if (lines.Count == 0)
        {
            return;
        }

        if (lines.Count != objects.Count)
        {
            throw TreeErrors.Fail(Codes.Corrupt, $"The text has {lines.Count} lines and {objects.Count} object values.", _path.Member(name));
        }

        Store(name, LinesWithValuesNode.FromLines(lines is List<string> list ? list : [.. lines], objects is List<int> ids ? ids : [.. objects]));
    }

    public void Bytes(string name, Span<byte> data)
    {
        if (Loading)
        {
            TreeValue? found = Take(name);
            if (found is null)
            {
                return;
            }

            if (found is not BytesNode node)
            {
                throw Type(name, "bytes");
            }

            if (node.Data.Length != data.Length)
            {
                throw TreeErrors.Fail(Codes.Range, $"{name} holds exactly {data.Length} bytes.", _path.Member(name), value: node.Data.Length.ToString(System.Globalization.CultureInfo.InvariantCulture), constraint: $"{data.Length} bytes");
            }

            node.Data.CopyTo(data);
            return;
        }
        if (data.Length == 0)
        {
            return;
        }

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
            TreeValue? found = Take(name);
            if (found is null)
            {
                return value;
            }

            byte loaded = found is BooleanNode node
                ? node.Value ? (byte)1 : (byte)0
                : found is IntegerNode { Width: 1, Signed: false } number && number.Value is >= 0 and <= 255
                    ? (byte)number.Value
                    : throw Type(name, "a boolean");
            return Unsafe.As<byte, bool>(ref loaded);
        }
        byte stored = Unsafe.As<bool, byte>(ref value);
        if (stored is 0 or 1)
        {
            Store(name, new BooleanNode(stored != 0));
        }
        else
        {
            Store(name, new IntegerNode(1, false, stored));
        }

        return value;
    }

    public void BitArray(string name, int count, Span<byte> bits)
    {
        int length = count < 0 ? -1 : (count + 7) >> 3;
        if (length < 0)
        {
            throw TreeErrors.Fail(Codes.Range, name + " has a negative count.", _path.Member(name), value: count.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        if (bits.Length != length)
        {
            throw TreeErrors.Fail(Codes.Range, name + " holds exactly " + length.ToString(System.Globalization.CultureInfo.InvariantCulture) + " bytes.", _path.Member(name), value: bits.Length.ToString(System.Globalization.CultureInfo.InvariantCulture), constraint: length.ToString(System.Globalization.CultureInfo.InvariantCulture) + " bytes");
        }

        if (Loading)
        {
            TreeValue? found = Take(name);
            if (length == 0)
            {
                if (found is not null)
                {
                    throw Type(name, "omitted when the count is zero");
                }

                return;
            }
            if (found is null)
            {
                return;
            }

            if (found is not BitArrayNode node || node.Count != count)
            {
                throw Type(name, "a bit array");
            }

            bits.Clear();
            foreach (int index in node.Indices)
            {
                if ((uint)index >= (uint)count)
                {
                    throw TreeErrors.Fail(Codes.Range, name + " is outside its storage range.", _path.Member(name), value: index.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }

                bits[index >> 3] |= (byte)(1 << (index & 7));
            }
            if ((count & 7) != 0)
            {
                bits[length - 1] |= node.Residue;
            }

            return;
        }
        if (length == 0)
        {
            return;
        }

        List<int> indices = [];
        for (int i = 0; i < count; i++)
        {
            if ((bits[i >> 3] & (1 << (i & 7))) != 0)
            {
                indices.Add(i);
            }
        }

        byte residue = 0;
        if ((count & 7) != 0)
        {
            residue = (byte)(bits[length - 1] & (byte)(0xFF << (count & 7)));
        }

        Store(name, new BitArrayNode(count, [.. indices], residue));
    }

    public BitListImage? BitList(string name, BitListImage value)
    {
        if (Loading)
        {
            TreeValue? found = Take(name);
            if (found is null)
            {
                return null;
            }

            if (found is not BitListNode node)
            {
                throw Type(name, "a bit list");
            }

            byte[] bits = new byte[(node.Count + 7) >> 3];
            foreach (int index in node.Indices)
            {
                bits[index >> 3] |= (byte)(1 << (index & 7));
            }

            if ((node.Count & 7) != 0 && bits.Length != 0)
            {
                bits[^1] |= node.Residue;
            }

            return new BitListImage(node.Default, node.Count, bits);
        }
        if (value.Count < 0 || value.Bits.Length != (value.Count + 7L) >> 3)
        {
            throw TreeErrors.Fail(Codes.Range, name + " of " + value.Count.ToString(System.Globalization.CultureInfo.InvariantCulture) + " bits holds " + ((value.Count + 7L) >> 3).ToString(System.Globalization.CultureInfo.InvariantCulture) + " bytes.", _path.Member(name));
        }

        List<int> indices = [];
        for (int i = 0; i < value.Count; i++)
        {
            if ((value.Bits[i >> 3] & (1 << (i & 7))) != 0)
            {
                indices.Add(i);
            }
        }

        Store(name, new BitListNode(residue: (value.Count & 7) != 0 ? (byte)(value.Bits[^1] & (byte)(0xFF << (value.Count & 7))) : (byte)0, defaultValue: value.Default, count: value.Count, indices: [.. indices]));
        return value;
    }

    public float Single(string name, float value)
    {
        if (Loading)
        {
            TreeValue? found = Take(name);
            return found is null ? value : found is not SingleNode node ? throw Type(name, "a single") : node.Value;
        }
        Store(name, new SingleNode(BitConverter.SingleToInt32Bits(value)));
        return value;
    }

    public byte[]? Blob(string name, byte[] data)
    {
        if (Loading)
        {
            TreeValue? found = Take(name);
            return found is null ? null : found is not BytesNode node ? throw Type(name, "bytes") : node.Data;
        }
        Store(name, new BytesNode(data));
        return data;
    }

    public void SBytePairs(string name, IList<(sbyte First, sbyte Second)> pairs)
    {
        if (Loading)
        {
            TreeValue? found = Take(name);
            if (found is null)
            {
                return;
            }

            if (found is not PairListNode node)
            {
                throw Type(name, "pairs of signed bytes");
            }

            pairs.Clear();
            foreach ((sbyte First, sbyte Second) pair in node.Pairs)
            {
                pairs.Add(pair);
            }

            return;
        }
        Store(name, new PairListNode([.. pairs]));
    }

    public void ImageSequences(string name, IList<IReadOnlyList<SequenceLayer>?> entries)
    {
        DocumentNodePath at = _path.Member(name);
        if (Loading)
        {
            TreeValue? found = Take(name);
            if (found is null)
            {
                return;
            }

            if (found is not ImageSequenceListNode node)
            {
                throw Type(name, "an image sequence list");
            }

            entries.Clear();
            foreach (IReadOnlyList<SequenceLayer>? entry in node.Entries)
            {
                entries.Add(entry);
            }

            return;
        }
        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i] is not { } layers)
            {
                continue;
            }

            if (layers.Count == 0)
            {
                throw TreeErrors.Fail(Codes.Type, "A sequence of " + name + " has at least one layer.", at.Item(i));
            }

            for (int k = 0; k < layers.Count; k++)
            {
                if ((layers[k].Frames is not null) != SequenceLayer.HasFrameTable(layers[k].Mode))
                {
                    throw TreeErrors.Fail(Codes.Type, "A layer stores a frame table exactly when its mode is FrameTable or RandomFrameTable.", at.Item(i).Item(k));
                }
            }
        }
        Store(name, new ImageSequenceListNode([.. entries]));
    }

    public void Object(string name, IPersistable objectValue)
    {
        if (Loading)
        {
            TreeValue? found = Take(name);
            ObjectNode nested = found is null ? new ObjectNode(null, null) : found is ObjectNode node ? node : throw Type(name, "an object");
            MemberPersister child = new(nested, true, Factory, _path.Member(name), _errors, preview: Preview);
            // rwEObject wraps the main visit and leaves the caller's instance in place.
            try
            {
                objectValue.Persist(child);
                child.Finish();
            }
            catch (Exception error)
            {
                if (!Continue(error))
                {
                    throw;
                }
            }
            return;
        }
        ObjectNode written = new(null, null);
        objectValue.Persist(new MemberPersister(written, false, Factory, _path.Member(name), _errors, preview: Preview));
        Store(name, written);
    }

    public IPersistable? Created(string name, IPersistable? objectValue)
    {
        if (Loading)
        {
            TreeValue? found = Take(name);
            return found is null or NullNode
                ? null
                : found is not ObjectNode node || node.ClassName is not string className
                ? throw Type(name, "a created object")
                : Create(node, className, _path.Member(name));
        }
        if (objectValue is null)
        {
            Store(name, NullNode.Instance);
            return null;
        }
        ObjectNode written = new(Factory.ClassName(objectValue), null);
        objectValue.Persist(new MemberPersister(written, false, Factory, _path.Member(name), _errors, preview: Preview));
        Store(name, written);
        return objectValue;
    }

    public T? Created<T>(string name, T? objectValue) where T : class, IPersistable
    {
        IPersistable? box = Created(name, (IPersistable?)objectValue);
        return box is not null and not T
            ? throw TreeErrors.Fail(Codes.ClassFamily, $"Created class {Factory.ClassName(box)} is not a {typeof(T).Name}.", _path.Member(name), value: Factory.ClassName(box), constraint: typeof(T).Name)
            : (T?)box;
    }

    public IPersistable? CreatedSlot(string name, int slot, IPersistable? objectValue)
    {
        DocumentNodePath at = _path.Member(name).Item(slot);
        if (slot < 0)
        {
            throw TreeErrors.Fail(Codes.Range, "A slot of " + name + " is negative.", at, value: slot.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        _slotMembers ??= [with(StringComparer.Ordinal)];
        if (Loading)
        {
            if (!_slotMembers.TryGetValue(name, out SlotListNode? stored))
            {
                TreeValue? found = Take(name);
                if (found is not null and not SlotListNode)
                {
                    throw Type(name, "created objects at slots");
                }

                stored = (SlotListNode?)found;
                _slotMembers[name] = stored;
            }
            TreeValue? value = stored?.Find(slot);
            return value is null or NullNode
                ? null
                : value is not ObjectNode node || node.ClassName is not string className
                ? throw TreeErrors.Fail(Codes.Type, "A slot of " + name + " is a created object.", at)
                : Create(node, className, at);
        }
        if (!_slotMembers.TryGetValue(name, out SlotListNode? list) || list is null)
        {
            list = new SlotListNode();
            Store(name, list);
            _slotMembers[name] = list;
        }
        if (list.Find(slot) is not null)
        {
            throw TreeErrors.Fail(Codes.Slot, "Slot " + slot.ToString(System.Globalization.CultureInfo.InvariantCulture) + " of " + name + " is stored twice.", at);
        }

        if (objectValue is null)
        {
            list.Add(slot, NullNode.Instance);
            return null;
        }
        ObjectNode written = new(Factory.ClassName(objectValue), slot);
        objectValue.Persist(new MemberPersister(written, loading: false, factory: Factory, path: at, errors: _errors, preview: Preview));
        list.Add(slot, written);
        return objectValue;
    }

    public T? CreatedSlot<T>(string name, int slot, T? objectValue) where T : class, IPersistable
    {
        IPersistable? box = CreatedSlot(name, slot, (IPersistable?)objectValue);
        return box is not null and not T
            ? throw TreeErrors.Fail(Codes.ClassFamily, $"Created class {Factory.ClassName(box)} is not a {typeof(T).Name}.", _path.Member(name).Item(slot), value: Factory.ClassName(box), constraint: typeof(T).Name)
            : (T?)box;
    }

    public void ObjectSlot(string name, int slot, IPersistable objectValue)
    {
        DocumentNodePath at = _path.Member(name).Item(slot);
        if (slot < 0)
        {
            throw TreeErrors.Fail(Codes.Range, "A slot of " + name + " is negative.", at, value: slot.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        _slotMembers ??= [with(StringComparer.Ordinal)];
        if (Loading)
        {
            if (!_slotMembers.TryGetValue(name, out SlotListNode? stored))
            {
                TreeValue? found = Take(name);
                if (found is not null and not SlotListNode)
                {
                    throw Type(name, "fixed objects at slots");
                }

                stored = (SlotListNode?)found;
                _slotMembers[name] = stored;
            }
            TreeValue? value = stored?.Find(slot);
            if (value is null)
            {
                return;
            }

            if (value is not ObjectNode { ClassName: null } node)
            {
                throw TreeErrors.Fail(Codes.Type, "A slot of " + name + " is a fixed object.", at);
            }

            MemberPersister reader = new(node, loading: true, factory: Factory, path: at, errors: _errors, preview: Preview);
            // rwEObject wraps the main visit and leaves the caller's instance in place.
            try
            {
                objectValue.Persist(reader);
                reader.Finish();
            }
            catch (Exception error)
            {
                if (!Continue(error))
                {
                    throw;
                }
            }
            return;
        }
        if (!_slotMembers.TryGetValue(name, out SlotListNode? list) || list is null)
        {
            list = new SlotListNode();
            Store(name, list);
            _slotMembers[name] = list;
        }
        if (list.Find(slot) is not null)
        {
            throw TreeErrors.Fail(Codes.Slot, "Slot " + slot.ToString(System.Globalization.CultureInfo.InvariantCulture) + " of " + name + " is stored twice.", at);
        }

        ObjectNode written = new(null, slot);
        objectValue.Persist(new MemberPersister(written, loading: false, factory: Factory, path: at, errors: _errors, preview: Preview));
        list.Add(slot, written);
    }

    public void Skip(string name)
    {
        if (Loading && (_consumed is null || !_consumed.Contains(name)))
        {
            _ = Take(name);
        }
    }

    public int Int32List(string name, IList<int> items, int defaultValue)
    {
        if (Loading)
        {
            TreeValue? found = Take(name);
            if (found is null)
            {
                return defaultValue;
            }

            if (found is not DefaultListNode { Width: 4 } node)
            {
                throw Type(name, "a list of 32-bit integers with a default");
            }

            items.Clear();
            foreach (long item in node.Items)
            {
                items.Add(checked((int)item));
            }

            return checked((int)node.Default);
        }
        long[] values = new long[items.Count];
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = items[i];
        }

        Store(name, new DefaultListNode(4, defaultValue, values));
        return defaultValue;
    }

    public byte ByteList(string name, IList<byte> items, byte defaultValue)
    {
        if (Loading)
        {
            TreeValue? found = Take(name);
            if (found is null)
            {
                return defaultValue;
            }

            if (found is not DefaultListNode { Width: 1 } node)
            {
                throw Type(name, "a list of bytes with a default");
            }

            items.Clear();
            foreach (long item in node.Items)
            {
                items.Add(checked((byte)item));
            }

            return checked((byte)node.Default);
        }
        long[] values = new long[items.Count];
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = items[i];
        }

        Store(name, new DefaultListNode(1, defaultValue, values));
        return defaultValue;
    }

    public void Children(string name, IChildCollection children)
    {
        if (Loading)
        {
            TreeValue? found = Take(name);
            IReadOnlyList<ObjectNode> list = found is null ? [] : found is ChildListNode node ? node.Children : throw Type(name, "children");
            foreach (ObjectNode child in list)
            {
                if (child.ClassName is not string className || child.Slot is not int slot)
                {
                    throw TreeErrors.Fail(Codes.MissingMember, "A child needs a class and a slot.", _path.Member(name));
                }

                IPersistable? created = null;
                try
                {
                    created = Factory.Create(className);
                    children.Attach(created);
                    MemberPersister reader = new(child, true, Factory, _path.Member(name).Item(slot), _errors, preview: Preview);
                    created.Persist(reader);
                    reader.Finish();
                    children.Place(slot, created);
                }
                catch (Exception error)
                {
                    if (created is not null)
                    {
                        Factory.Release(created);
                    }

                    if (!Continue(error))
                    {
                        throw;
                    }
                }
            }
            return;
        }
        List<ObjectNode> written = [];
        for (int i = 0; i < children.Count; i++)
        {
            if (!children.IsStored(i))
            {
                continue;
            }

            IPersistable child = children.Get(i) ?? throw TreeErrors.Fail(Codes.Corrupt, "A stored child is missing.", _path.Member(name).Item(i));
            ObjectNode node = new(Factory.ClassName(child), children.Slot(i));
            child.Persist(new MemberPersister(node, false, Factory, _path.Member(name).Item(children.Slot(i)), _errors, preview: Preview));
            written.Add(node);
        }
        Store(name, new ChildListNode(written));
    }

    public void Grid(string name, int width, int height, IReadOnlyList<IPersistable> fields, Action<int>? visited = null)
    {
        DocumentNodePath at = _path.Member(name);
        if (width < 0 || height < 0 || (long)width * height != fields.Count)
        {
            throw TreeErrors.Fail(Codes.Dimensions, $"{name} holds {fields.Count} fields for a {width} by {height} grid.", at,
                value: fields.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), constraint: $"{(long)width * height} fields");
        }

        if (Loading)
        {
            TreeValue? found = Take(name);
            if (found is null)
            {
                return;
            }

            if (found is not GridNode node)
            {
                throw Type(name, "a grid of fields");
            }

            if (node.Width != width || node.Height != height)
            {
                throw TreeErrors.Fail(Codes.Dimensions, $"{name} is a {node.Width} by {node.Height} grid and the object is {width} by {height}.", at,
                    value: $"{node.Width} by {node.Height}", constraint: $"{width} by {height}");
            }

            for (int i = 0; i < fields.Count; i++)
            {
                // The original object read wraps the main visit and leaves the field in place.
                MemberPersister reader = new(node.Fields[i], loading: true, factory: Factory, path: FieldPath(at, i, width), errors: _errors, preview: Preview);
                try
                {
                    fields[i].Persist(reader);
                    reader.Finish();
                }
                catch (Exception error)
                {
                    if (!Continue(error))
                    {
                        throw;
                    }
                }
                visited?.Invoke(i);
            }
            return;
        }
        ObjectNode[] written = new ObjectNode[fields.Count];
        for (int i = 0; i < fields.Count; i++)
        {
            ObjectNode field = new(null, null);
            fields[i].Persist(new MemberPersister(field, loading: false, factory: Factory, path: FieldPath(at, i, width), errors: _errors, preview: Preview));
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
        DocumentNodePath at = _path.Member(name);
        if (Loading)
        {
            TreeValue? found = Take(name);
            if (found is null)
            {
                return;
            }

            if (found is not SequenceNode node)
            {
                throw Type(name, "a sequence of created objects");
            }

            for (int k = 0; k < node.Items.Count; k++)
            {
                // The original created-object read with an owner: a stored null creates nothing.
                if (node.Items[k] is NullNode)
                {
                    continue;
                }

                if (node.Items[k] is not ObjectNode { ClassName: string className } item)
                {
                    throw TreeErrors.Fail(Codes.Type, "An item of " + name + " is a created object.", at.Item(k));
                }

                IPersistable? created = null;
                try
                {
                    created = Factory.Create(className);
                    items.Attach(created);
                    MemberPersister reader = new(item, loading: true, factory: Factory, path: at.Item(k), errors: _errors, preview: Preview);
                    created.Persist(reader);
                    reader.Finish();
                    items.Place(k, created);
                }
                catch (Exception error)
                {
                    if (created is not null)
                    {
                        Factory.Release(created);
                    }

                    if (!Continue(error))
                    {
                        throw;
                    }
                }
            }
            return;
        }
        List<TreeValue>? written = null;
        for (int i = 0; i < items.Count; i++)
        {
            if (!items.IsStored(i))
            {
                continue;
            }

            IPersistable item = items.Get(i) ?? throw TreeErrors.Fail(Codes.Corrupt, "A stored item is missing.", at.Item(i));
            written ??= [];
            ObjectNode node = new(Factory.ClassName(item), null);
            item.Persist(new MemberPersister(node, loading: false, factory: Factory, path: at.Item(written.Count), errors: _errors, preview: Preview));
            written.Add(node);
        }
        if (written is not null)
        {
            Store(name, new SequenceNode(written));
        }
    }

    public byte[] NestedDocument(string name, string profile, byte[] image)
    {
        if (Loading)
        {
            TreeValue? found = Take(name);
            return found is null
                ? []
                : found is not NestedDocumentNode node || node.Profile != profile
                ? throw Type(name, "a nested " + profile + " document")
                : node.Image;
        }
        if (image.Length != 0)
        {
            Store(name, new NestedDocumentNode(profile, image));
        }

        return image;
    }

    public int Reference(string name, string domain, int key, int fallback = 0)
    {
        if (Loading)
        {
            TreeValue? found = Take(name);
            return found is null
                ? fallback
                : found is ReferenceNode reference
                ? reference.Domain != domain
                    ? throw TreeErrors.Fail(Codes.Reference, $"Reference \"{name}\" is in domain {reference.Domain}.", _path.Member(name), value: reference.Domain, constraint: domain)
                    : reference.Key
                : found is IntegerNode integer && integer.Width == 4 && integer.Signed ? (int)integer.Value : throw Type(name, "a reference");
        }
        if (key != fallback)
        {
            Store(name, new ReferenceNode(domain, key));
        }

        return key;
    }

    public void Record(string name, Action<IPersister> body)
    {
        if (Loading)
        {
            TreeValue? found = Take(name);
            RecordNode record = found is null ? new RecordNode() : found is RecordNode node ? node : throw Type(name, "a record");
            MemberPersister child = new(record, true, Factory, _path.Member(name), _errors, preview: Preview);
            body(child);
            child.Finish();
            return;
        }
        RecordNode written = new();
        body(new MemberPersister(written, false, Factory, _path.Member(name), _errors, preview: Preview));
        Store(name, written);
    }

    public void List<T>(string name, IList<T> items, ItemTransfer<T> transfer)
    {
        if (Loading)
        {
            items.Clear();
            TreeValue? found = Take(name);
            if (found is null)
            {
                return;
            }

            if (found is not ListNode list)
            {
                throw Type(name, "a list");
            }

            for (int i = 0; i < list.Items.Count; i++)
            {
                ItemSlot slot = new(list.Items[i], true, Factory, _path.Member(name).Item(i), _errors, preview: Preview);
                T value = transfer(slot, default!);
                _ = slot.Finish();
                items.Add(value);
            }
            return;
        }
        if (items.Count == 0)
        {
            return;
        }

        ListNode written = new();
        for (int i = 0; i < items.Count; i++)
        {
            ItemSlot slot = new(null, false, Factory, _path.Member(name).Item(i), _errors, preview: Preview);
            items[i] = transfer(slot, items[i]);
            written.Items.Add(slot.Finish());
        }
        Store(name, written);
    }

    private long Integer(string name, long current, long fallback, int width, bool signed)
    {
        if (Loading)
        {
            TreeValue? found = Take(name);
            return found is null
                ? fallback
                : found is IntegerNode node && node.Width == width && node.Signed == signed
                ? node.Value
                : found is ReferenceNode reference && width == 4 && signed ? reference.Key : throw Type(name, "an integer");
        }
        if (current != fallback)
        {
            Store(name, new IntegerNode(width, signed, current));
        }

        return current;
    }

    private TreeValue? Find(string name)
    {
        foreach ((string? member, TreeValue? value) in _existing)
        {
            if (member == name)
            {
                return value;
            }
        }

        return null;
    }

    private TreeValue? Take(string name)
    {
        _consumed ??= [with(StringComparer.Ordinal)];
        return !_consumed.Add(name)
            ? throw TreeErrors.Fail(Codes.Duplicate, $"Member \"{name}\" is read twice.", _path.Member(name))
            : Find(name);
    }

    private void Store(string name, TreeValue value)
    {
        if (Find(name) is not null)
        {
            throw TreeErrors.Fail(Codes.Duplicate, $"Member \"{name}\" is stored twice.", _path.Member(name));
        }

        _add!(name, value);
    }

    private ConversionException Type(string name, string expected) =>
        TreeErrors.Fail(Codes.Type, $"{name} is {expected}.", _path.Member(name));

    private long RawInteger(string name, long current, int width, bool signed)
    {
        if (Loading)
        {
            TreeValue? found = Take(name);
            return found is null
                ? current
                : found is IntegerNode node && node.Width == width && node.Signed == signed ? node.Value : throw Type(name, "an integer");
        }
        Store(name, new IntegerNode(width, signed, current));
        return current;
    }

    private bool Continue(Exception error) => _errors is null || _errors.Continue(error);

    // rwCreateEObject creates the object and runs its load. A failure releases the object, and the
    // load's handler decides whether the parent continues with null.
    private IPersistable? Create(ObjectNode node, string className, DocumentNodePath at)
    {
        IPersistable? created = null;
        try
        {
            created = Factory.Create(className);
            MemberPersister child = new(node, loading: true, factory: Factory, path: at, errors: _errors, preview: Preview);
            created.Persist(child);
            child.Finish();
            return created;
        }
        catch (Exception error)
        {
            if (created is not null)
            {
                Factory.Release(created);
            }

            if (!Continue(error))
            {
                throw;
            }

            return null;
        }
    }

    private static List<string> Split(string text)
    {
        List<string> lines = [];
        int i = 0;
        while (i < text.Length && text[i] != '\0')
        {
            int start = i;
            while (i < text.Length && text[i] is not ('\0' or '\r' or '\n'))
            {
                i++;
            }

            lines.Add(text[start..i]);
            if (i < text.Length && text[i] == '\r')
            {
                i++;
            }

            if (i < text.Length && text[i] == '\n')
            {
                i++;
            }
        }
        return lines;
    }
}

/// <summary>One list item, holding a single tree value.</summary>
internal sealed class ItemSlot(TreeValue? loaded, bool loading, IPersistableFactory factory, DocumentNodePath path, LoadErrorPolicy? errors = null, bool preview = false) : IItemSlot
{
    private readonly TreeValue? _loaded = loaded;
    private readonly bool _loading = loading;
    private readonly IPersistableFactory _factory = factory;
    private readonly DocumentNodePath _path = path;
    private readonly LoadErrorPolicy? _errors = errors;
    private readonly bool _preview = preview;
    private TreeValue? _stored;
    private bool _used;

    public TreeValue Finish() =>
        !_used
            ? throw TreeErrors.Fail(Codes.MissingMember, "A list item stores one value.", _path)
            : _loading
                ? _loaded!
                : _stored!;

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
            return _loaded is not BooleanNode node ? throw TreeErrors.Fail(Codes.Type, "A list item is a boolean.", _path) : node.Value;
        }
        _stored = new BooleanNode(value);
        return value;
    }

    public string String(string value)
    {
        Use();
        if (_loading)
        {
            return _loaded is not StringNode node ? throw TreeErrors.Fail(Codes.Type, "A list item is a string.", _path) : node.Value;
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
            if (_loaded is not RecordNode node)
            {
                throw TreeErrors.Fail(Codes.Type, "A list item is a record.", _path);
            }

            MemberPersister child = new(node, true, _factory, _path, _errors, preview: _preview);
            body(child);
            child.Finish();
            return;
        }
        RecordNode written = new();
        body(new MemberPersister(written, false, _factory, _path, _errors, preview: _preview));
        _stored = written;
    }

    public IPersistable? Object(IPersistable? value)
    {
        Use();
        if (_loading)
        {
            if (_loaded is NullNode or null)
            {
                return null;
            }

            if (_loaded is not ObjectNode node || node.ClassName is not string className)
            {
                throw TreeErrors.Fail(Codes.Type, "A list item is an object.", _path);
            }

            IPersistable? created = null;
            try
            {
                created = _factory.Create(className);
                MemberPersister child = new(node, true, _factory, _path, _errors, preview: _preview);
                created.Persist(child);
                child.Finish();
                return created;
            }
            catch (Exception error)
            {
                if (created is not null)
                {
                    _factory.Release(created);
                }

                if (_errors is not null && !_errors.Continue(error))
                {
                    throw;
                }

                return null;
            }
        }
        if (value is null)
        {
            _stored = NullNode.Instance;
            return null;
        }
        ObjectNode written = new(_factory.ClassName(value), null);
        value.Persist(new MemberPersister(written, false, _factory, _path, _errors, preview: _preview));
        _stored = written;
        return value;
    }

    private long Integer(long current, int width, bool signed)
    {
        Use();
        if (_loading)
        {
            return _loaded is not IntegerNode node || node.Width != width || node.Signed != signed
                ? throw TreeErrors.Fail(Codes.Type, "A list item is an integer.", _path)
                : node.Value;
        }
        _stored = new IntegerNode(width, signed, current);
        return current;
    }

    private void Use()
    {
        if (_used)
        {
            throw TreeErrors.Fail(Codes.Duplicate, "A list item stores one value.", _path);
        }

        _used = true;
    }
}
