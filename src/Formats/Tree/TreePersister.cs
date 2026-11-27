using System.Runtime.CompilerServices;
using Aow.Rtl;

namespace Aow.Formats.Tree;

/// <summary>
/// The load's <see cref="LoadErrorHandler"/>, with the same stop flag as the original stream.
/// A null handler continues. Once a handler stops the load, later exceptions stop too.
/// An empty message is reported as "Error Reading EngineObject". A load with a dropped list adds
/// each object it goes on without to that list. The list changes nothing about whether the load
/// goes on: the record never throws, so a failure to name or add an object cannot replace the
/// failed read the load goes on without.
/// </summary>
internal sealed class LoadErrorPolicy(LoadErrorHandler? handler, ICollection<DroppedObject>? dropped = null)
{
    private bool _stopped;

    /// <summary>Decides on the failed read of a created object or a child of the stored class.</summary>
    internal bool Continue(Exception error, DocumentNodePath at, string className) => Continue(error, at: at, className: className, objectValue: null, factory: null);

    /// <summary>
    /// Decides on the failed read of a fixed object, whose class the document does not store. The
    /// factory names the class only for a load that records what it dropped.
    /// </summary>
    internal bool Continue(Exception error, DocumentNodePath at, IPersistable objectValue, IPersistableFactory factory)
        => Continue(error, at: at, className: null, objectValue: objectValue, factory: factory);

    private bool Continue(Exception error, DocumentNodePath at, string? className, IPersistable? objectValue, IPersistableFactory? factory)
    {
        if (_stopped)
        {
            return false;
        }

        string message = error.Message.Length == 0 ? "Error Reading EObject" : error.Message;
        bool continueLoad = true;
        handler?.Invoke(message, out continueLoad);
        if (!continueLoad)
        {
            _stopped = true;
            return false;
        }

        if (dropped is not null)
        {
            Record(dropped, error: error, at: at, message: message, className: className ?? StoredName(objectValue, factory));
        }

        return true;
    }

    // The stored name of a fixed object, or its plain type name when the factory cannot name it.
    private static string StoredName(IPersistable? objectValue, IPersistableFactory? factory)
    {
        try
        {
            return (objectValue, factory) is ({ } value, { } names) ? names.ClassName(value) : objectValue?.GetType().Name ?? "";
        }
        catch (Exception)
        {
            return objectValue?.GetType().Name ?? "";
        }
    }

    // A list that refuses the entry leaves it out. The load goes on as it would without the list.
    private static void Record(ICollection<DroppedObject> dropped, Exception error, DocumentNodePath at, string message, string className)
    {
        try
        {
            dropped.Add(new DroppedObject(className, Path: at.ToString(), Message: message, Error: error));
        }
        catch (Exception)
        {
        }
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
        ObjectNode node = new(null, slot: null);
        MemberPersister persister = new(node, loading: false, factory, path: DocumentNodePath.Root, preview: preview);
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
            return new ObjectNode(null, slot: null);
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
    /// <remarks>
    /// Without a dropped list the root is strict: a failure of the root throws whatever the
    /// handler says, as before. A load that passes a dropped list takes the root rule of
    /// <see cref="ReadObject"/>, so a root failure goes to the handler and, when the load goes on,
    /// is in the list. No load that passes no list changes.
    /// </remarks>
    public static void Read(ObjectNode node, IPersistable objectValue, IPersistableFactory factory, LoadErrorHandler? onError = null, bool preview = false, ICollection<DroppedObject>? dropped = null)
    {
        if (dropped is not null)
        {
            ReadObject(node, objectValue: objectValue, factory: factory, onError: onError, preview: preview, dropped: dropped);
            return;
        }

        MemberPersister persister = new(node, loading: true, factory, path: DocumentNodePath.Root, errors: new LoadErrorPolicy(onError), preview: preview);
        objectValue.Persist(persister);
        persister.Finish();
    }

    /// <summary>
    /// Reads a tree into an object the caller holds the way <c>TEngine.ReadEObject</c> reads a
    /// stream (<c>rwEObject</c> with id 0): a failure of the root goes to the load's handler like a
    /// failure of a nested object. A handler that continues, or no handler, leaves the object as the
    /// load left it, and a handler that stops makes the load throw the original exception. A load
    /// that goes on without an object adds it to <paramref name="dropped"/> when the caller passes one.
    /// </summary>
    public static void ReadObject(ObjectNode node, IPersistable objectValue, IPersistableFactory factory, LoadErrorHandler? onError = null, bool preview = false, ICollection<DroppedObject>? dropped = null)
    {
        LoadErrorPolicy errors = new(onError, dropped: dropped);
        MemberPersister persister = new(node, loading: true, factory: factory, path: DocumentNodePath.Root, errors: errors, preview: preview);
        try
        {
            objectValue.Persist(persister);
            persister.Finish();
        }
        catch (Exception error)
        {
            if (!errors.Continue(error, at: DocumentNodePath.Root, objectValue: objectValue, factory: factory))
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Creates the root of a tree and reads it the way <c>TEngine.RetrieveEObject</c> reads a
    /// stream (<c>rwCreateEObject</c> with id 0): the factory creates the class, and a failure
    /// releases the object and goes to the load's handler. A handler that continues, or no
    /// handler, returns null. A handler that stops makes the load throw the original exception. A
    /// load that goes on without an object adds it to <paramref name="dropped"/> when the caller passes one.
    /// </summary>
    public static IPersistable? ReadCreated(ObjectNode node, string className, IPersistableFactory factory, LoadErrorHandler? onError = null, bool preview = false, ICollection<DroppedObject>? dropped = null)
    {
        LoadErrorPolicy errors = new(onError, dropped: dropped);
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

            if (!errors.Continue(error, at: DocumentNodePath.Root, className: className))
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
        : this(node.Members, add: loading ? null : node.Add, loading, factory, path, errors, preview)
    {
    }

    public MemberPersister(RecordNode node, bool loading, IPersistableFactory factory, DocumentNodePath path, LoadErrorPolicy? errors = null, bool preview = false)
        : this(node.Members, add: loading ? null : node.Add, loading, factory, path, errors, preview)
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
                throw TreeErrors.Fail(Codes.Required, message: $"Member \"{name}\" was not read.", path: _path.Member(name));
            }
        }
    }

    public int Int32(string name, int value, int fallback = 0)
        => (int)Integer(name, current: value, fallback, width: 4, signed: true);

    public byte Byte(string name, byte value, byte fallback = 0)
        => (byte)Integer(name, current: value, fallback, width: 1, signed: false);

    public int SByte(string name, int value, int fallback = 0)
        => (sbyte)Integer(name, current: value, fallback, width: 1, signed: true);

    public int UInt16(string name, int value, int fallback = 0)
        => (ushort)Integer(name, current: value, fallback, width: 2, signed: false);

    public int Int16(string name, int value, int fallback = 0)
        => (short)Integer(name, current: value, fallback, width: 2, signed: true);

    public uint UInt32(string name, uint value, uint fallback = 0)
        => (uint)Integer(name, current: value, fallback, width: 4, signed: false);

    public TEnum Enum<TEnum>(string name, TEnum value, TEnum fallback = default) where TEnum : struct, Enum
    {
        (int width, bool signed) = EnumTypeInfo.Layout(typeof(TEnum));
        string typeName = NameTable.Stored(typeof(TEnum));
        if (Loading)
        {
            TreeValue? found = Take(name);
            return found is null
                ? fallback
                : found is not EnumNode node || !NameTable.Names(typeof(TEnum), name: node.TypeName) || node.Width != width || node.Signed != signed
                ? throw Type(name, expected: $"a {typeName}")
                : (TEnum)System.Enum.ToObject(typeof(TEnum), node.Value);
        }
        Store(name, new EnumNode(typeName, width, signed, value: Convert.ToInt64(value)));
        return value;
    }

    public F Set<F>(string name, F value) where F : unmanaged, Enum
    {
        if (!FlagsExtensions.IsFlags(typeof(F)))
        {
            throw new InvalidOperationException($"{typeof(F).Name} is not a flags enum.");
        }

        int width = FlagsExtensions.StorageSize(typeof(F));
        Type element = FlagsExtensions.StoredElement(typeof(F));
        string typeName = NameTable.Stored(element);
        if (width is < 1 or > 8)
        {
            throw new InvalidOperationException("A set image is 1 to 8 bytes.");
        }

        if (Loading)
        {
            TreeValue? found = Take(name);
            return found is null
                ? value
                : found is not SetNode node || !NameTable.Names(element, name: node.TypeName) || node.Width != width
                ? throw Type(name, expected: $"a set of {typeName}")
                : FlagsExtensions.FromBits<F>(node.Mask);
        }

        ulong mask = FlagsExtensions.Bits(value) & (width == 8 ? ulong.MaxValue : (1UL << (8 * width)) - 1);
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
                _ => throw Type(name, expected: "a boolean"),
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
                _ => throw Type(name, expected: "a string"),
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
                throw Type(name, expected: "lines");
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
                throw Type(name, expected: "lines with objects");
            }

            foreach (string line in node.Lines ?? Split(node.RawText ?? ""))
            {
                lines.Add(line);
            }

            if (lines.Count != node.Objects.Count)
            {
                throw TreeErrors.Fail(Codes.Corrupt, message: $"The text has {lines.Count} lines and {node.Objects.Count} object values.", path: _path.Member(name));
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
            throw TreeErrors.Fail(Codes.Corrupt, message: $"The text has {lines.Count} lines and {objects.Count} object values.", path: _path.Member(name));
        }

        Store(name, LinesWithValuesNode.FromLines(lines is List<string> list ? list : [.. lines], objects: objects is List<int> ids ? ids : [.. objects]));
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
                throw Type(name, expected: "bytes");
            }

            if (node.Data.Length != data.Length)
            {
                throw TreeErrors.Fail(Codes.Range, message: $"{name} holds exactly {data.Length} bytes.", path: _path.Member(name), value: node.Data.Length.ToString(System.Globalization.CultureInfo.InvariantCulture), constraint: $"{data.Length} bytes");
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

    public int RawInt32(string name, int value) => (int)RawInteger(name, current: value, width: 4, signed: true);

    public byte RawByte(string name, byte value) => (byte)RawInteger(name, current: value, width: 1, signed: false);

    public int RawSByte(string name, int value) => (sbyte)RawInteger(name, current: value, width: 1, signed: true);

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
                    : throw Type(name, expected: "a boolean");
            return Unsafe.As<byte, bool>(ref loaded);
        }
        byte stored = Unsafe.As<bool, byte>(ref value);
        if (stored is 0 or 1)
        {
            Store(name, new BooleanNode(stored != 0));
        }
        else
        {
            Store(name, new IntegerNode(1, signed: false, value: stored));
        }

        return value;
    }

    public void BitArray(string name, int count, Span<byte> bits)
    {
        int length = count < 0 ? -1 : (count + 7) >> 3;
        if (length < 0)
        {
            throw TreeErrors.Fail(Codes.Range, message: $"{name} has a negative count.", path: _path.Member(name), value: count.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        if (bits.Length != length)
        {
            throw TreeErrors.Fail(Codes.Range, message: string.Create(System.Globalization.CultureInfo.InvariantCulture, handler: $"{name} holds exactly {length} bytes."), path: _path.Member(name), value: bits.Length.ToString(System.Globalization.CultureInfo.InvariantCulture), constraint: string.Create(System.Globalization.CultureInfo.InvariantCulture, handler: $"{length} bytes"));
        }

        if (Loading)
        {
            TreeValue? found = Take(name);
            if (length == 0)
            {
                if (found is not null)
                {
                    throw Type(name, expected: "omitted when the count is zero");
                }

                return;
            }
            if (found is null)
            {
                return;
            }

            if (found is not BitArrayNode node || node.Count != count)
            {
                throw Type(name, expected: "a bit array");
            }

            bits.Clear();
            int storedBits = length * 8;
            foreach (int index in node.Indices)
            {
                if ((uint)index >= (uint)storedBits)
                {
                    throw TreeErrors.Fail(Codes.Range, message: $"{name} is outside its storage range.", path: _path.Member(name), value: index.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }

                bits[index >> 3] |= (byte)(1 << (index & 7));
            }

            return;
        }
        if (length == 0)
        {
            return;
        }

        int totalBits = length * 8;
        List<int> indices = [];
        for (int i = 0; i < totalBits; i++)
        {
            if ((bits[i >> 3] & (1 << (i & 7))) != 0)
            {
                indices.Add(i);
            }
        }

        Store(name, new BitArrayNode(count, indices: [.. indices]));
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
                throw Type(name, expected: "a bit list");
            }

            byte[] bits = new byte[(node.Count + 7) >> 3];
            foreach (int index in node.Indices)
            {
                bits[index >> 3] |= (byte)(1 << (index & 7));
            }

            if ((node.Count & 7) != 0 && bits.Length != 0)
            {
                bits[^1] |= Original.BitListCodec.ExpectedTail(node.Default, node.Count);
            }

            return new BitListImage(node.Default, node.Count, bits, ReadPast: node.ReadPast);
        }
        if (value.Count < 0 || value.Bits.Length != (value.Count + 7L) >> 3)
        {
            throw TreeErrors.Fail(Codes.Range, message: string.Create(System.Globalization.CultureInfo.InvariantCulture, handler: $"{name} of {value.Count} bits holds {(value.Count + 7L) >> 3} bytes."), path: _path.Member(name));
        }

        List<int> indices = [];
        for (int i = 0; i < value.Count; i++)
        {
            if ((value.Bits[i >> 3] & (1 << (i & 7))) != 0)
            {
                indices.Add(i);
            }
        }

        Store(name, new BitListNode(defaultValue: value.Default, count: value.Count, indices: [.. indices]));
        return value;
    }

    public float Single(string name, float value)
    {
        if (Loading)
        {
            TreeValue? found = Take(name);
            return found is null ? value : found is not SingleNode node ? throw Type(name, expected: "a single") : node.Value;
        }
        Store(name, new SingleNode(BitConverter.SingleToInt32Bits(value)));
        return value;
    }

    public byte[]? Blob(string name, byte[] data)
    {
        if (Loading)
        {
            TreeValue? found = Take(name);
            return found is null ? null : found is not BytesNode node ? throw Type(name, expected: "bytes") : node.Data;
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
                throw Type(name, expected: "pairs of signed bytes");
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
                throw Type(name, expected: "an image sequence list");
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
                throw TreeErrors.Fail(Codes.Type, message: $"A sequence of {name} has at least one layer.", path: at.Item(i));
            }

            for (int k = 0; k < layers.Count; k++)
            {
                if ((layers[k].Frames is not null) != SequenceLayer.HasFrameTable(layers[k].Mode))
                {
                    throw TreeErrors.Fail(Codes.Type, message: "A layer stores a frame table exactly when its mode is FrameTable or RandomFrameTable.", path: at.Item(i).Item(k));
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
            ObjectNode nested = found is null ? new ObjectNode(null, slot: null) : found is ObjectNode node ? node : throw Type(name, expected: "an object");
            MemberPersister child = new(nested, loading: true, Factory, path: _path.Member(name), _errors, preview: Preview);
            // rwEObject wraps the main visit and leaves the caller's instance in place.
            try
            {
                objectValue.Persist(child);
                child.Finish();
            }
            catch (Exception error)
            {
                if (!Continue(error, at: _path.Member(name), objectValue: objectValue))
                {
                    throw;
                }
            }
            return;
        }
        ObjectNode written = new(null, slot: null);
        objectValue.Persist(new MemberPersister(written, loading: false, Factory, path: _path.Member(name), _errors, preview: Preview));
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
                ? throw Type(name, expected: "a created object")
                : Create(node, className, at: _path.Member(name));
        }
        if (objectValue is null)
        {
            Store(name, NullNode.Instance);
            return null;
        }
        ObjectNode written = new(Factory.ClassName(objectValue), slot: null);
        objectValue.Persist(new MemberPersister(written, loading: false, Factory, path: _path.Member(name), _errors, preview: Preview));
        Store(name, written);
        return objectValue;
    }

    public T? Created<T>(string name, T? objectValue) where T : class, IPersistable
    {
        IPersistable? box = Created(name, (IPersistable?)objectValue);
        return box is not null and not T
            ? throw TreeErrors.Fail(Codes.ClassFamily, message: $"Created class {Factory.ClassName(box)} is not a {typeof(T).Name}.", path: _path.Member(name), value: Factory.ClassName(box), constraint: typeof(T).Name)
            : (T?)box;
    }

    public IPersistable? CreatedSlot(string name, int slot, IPersistable? objectValue)
    {
        DocumentNodePath at = _path.Member(name).Item(slot);
        if (slot < 0)
        {
            throw TreeErrors.Fail(Codes.Range, message: $"A slot of {name} is negative.", path: at, value: slot.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        _slotMembers ??= [with(StringComparer.Ordinal)];
        if (Loading)
        {
            if (!_slotMembers.TryGetValue(name, out SlotListNode? stored))
            {
                TreeValue? found = Take(name);
                if (found is not null and not SlotListNode)
                {
                    throw Type(name, expected: "created objects at slots");
                }

                stored = (SlotListNode?)found;
                _slotMembers[name] = stored;
            }
            TreeValue? value = stored?.Find(slot);
            return value is null or NullNode
                ? null
                : value is not ObjectNode node || node.ClassName is not string className
                ? throw TreeErrors.Fail(Codes.Type, message: $"A slot of {name} is a created object.", path: at)
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
            throw TreeErrors.Fail(Codes.Slot, message: string.Create(System.Globalization.CultureInfo.InvariantCulture, handler: $"Slot {slot} of {name} is stored twice."), path: at);
        }

        if (objectValue is null)
        {
            list.Add(slot, value: NullNode.Instance);
            return null;
        }
        ObjectNode written = new(Factory.ClassName(objectValue), slot);
        objectValue.Persist(new MemberPersister(written, loading: false, factory: Factory, path: at, errors: _errors, preview: Preview));
        list.Add(slot, value: written);
        return objectValue;
    }

    public T? CreatedSlot<T>(string name, int slot, T? objectValue) where T : class, IPersistable
    {
        IPersistable? box = CreatedSlot(name, slot, objectValue: (IPersistable?)objectValue);
        return box is not null and not T
            ? throw TreeErrors.Fail(Codes.ClassFamily, message: $"Created class {Factory.ClassName(box)} is not a {typeof(T).Name}.", path: _path.Member(name).Item(slot), value: Factory.ClassName(box), constraint: typeof(T).Name)
            : (T?)box;
    }

    public void ObjectSlot(string name, int slot, IPersistable objectValue)
    {
        DocumentNodePath at = _path.Member(name).Item(slot);
        if (slot < 0)
        {
            throw TreeErrors.Fail(Codes.Range, message: $"A slot of {name} is negative.", path: at, value: slot.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        _slotMembers ??= [with(StringComparer.Ordinal)];
        if (Loading)
        {
            if (!_slotMembers.TryGetValue(name, out SlotListNode? stored))
            {
                TreeValue? found = Take(name);
                if (found is not null and not SlotListNode)
                {
                    throw Type(name, expected: "fixed objects at slots");
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
                throw TreeErrors.Fail(Codes.Type, message: $"A slot of {name} is a fixed object.", path: at);
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
                if (!Continue(error, at: at, objectValue: objectValue))
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
            throw TreeErrors.Fail(Codes.Slot, message: string.Create(System.Globalization.CultureInfo.InvariantCulture, handler: $"Slot {slot} of {name} is stored twice."), path: at);
        }

        ObjectNode written = new(null, slot);
        objectValue.Persist(new MemberPersister(written, loading: false, factory: Factory, path: at, errors: _errors, preview: Preview));
        list.Add(slot, value: written);
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
                throw Type(name, expected: "a list of 32-bit integers with a default");
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

        Store(name, new DefaultListNode(4, defaultValue, items: values));
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
                throw Type(name, expected: "a list of bytes with a default");
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

        Store(name, new DefaultListNode(1, defaultValue, items: values));
        return defaultValue;
    }

    public void Children(string name, IChildCollection children)
    {
        if (Loading)
        {
            TreeValue? found = Take(name);
            IReadOnlyList<ObjectNode> list = found is null ? [] : found is ChildListNode node ? node.Children : throw Type(name, expected: "children");
            foreach (ObjectNode child in list)
            {
                if (child.ClassName is not string className || child.Slot is not int slot)
                {
                    throw TreeErrors.Fail(Codes.MissingMember, message: "A child needs a class and a slot.", path: _path.Member(name));
                }

                IPersistable? created = null;
                try
                {
                    created = Factory.Create(className);
                    children.Attach(created);
                    MemberPersister reader = new(child, loading: true, Factory, path: _path.Member(name).Item(slot), _errors, preview: Preview);
                    created.Persist(reader);
                    reader.Finish();
                    children.Place(slot, child: created);
                }
                catch (Exception error)
                {
                    if (created is not null)
                    {
                        Factory.Release(created);
                    }

                    if (!Continue(error, at: _path.Member(name).Item(slot), className: className))
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

            IPersistable child = children.Get(i) ?? throw TreeErrors.Fail(Codes.Corrupt, message: "A stored child is missing.", path: _path.Member(name).Item(i));
            ObjectNode node = new(Factory.ClassName(child), slot: children.Slot(i));
            child.Persist(new MemberPersister(node, loading: false, Factory, path: _path.Member(name).Item(children.Slot(i)), _errors, preview: Preview));
            written.Add(node);
        }
        Store(name, new ChildListNode(written));
    }

    public void Grid(string name, int width, int height, IReadOnlyList<IPersistable> fields, Action<int>? visited = null)
    {
        DocumentNodePath at = _path.Member(name);
        if (width < 0 || height < 0 || (long)width * height != fields.Count)
        {
            throw TreeErrors.Fail(Codes.Dimensions, message: $"{name} holds {fields.Count} fields for a {width} by {height} grid.", path: at,
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
                throw Type(name, expected: "a grid of fields");
            }

            if (node.Width != width || node.Height != height)
            {
                throw TreeErrors.Fail(Codes.Dimensions, message: $"{name} is a {node.Width} by {node.Height} grid and the object is {width} by {height}.", path: at,
                    value: $"{node.Width} by {node.Height}", constraint: $"{width} by {height}");
            }

            for (int i = 0; i < fields.Count; i++)
            {
                // The original object read wraps the main visit and leaves the field in place.
                MemberPersister reader = new(node.Fields[i], loading: true, factory: Factory, path: FieldPath(at, index: i, width), errors: _errors, preview: Preview);
                try
                {
                    fields[i].Persist(reader);
                    reader.Finish();
                }
                catch (Exception error)
                {
                    if (!Continue(error, at: FieldPath(at, index: i, width), objectValue: fields[i]))
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
            ObjectNode field = new(null, slot: null);
            fields[i].Persist(new MemberPersister(field, loading: false, factory: Factory, path: FieldPath(at, index: i, width), errors: _errors, preview: Preview));
            written[i] = field;
            visited?.Invoke(i);
        }
        Store(name, new GridNode(width, height, fields: written));
    }

    // The field at an index sits at [y][x] of the document's rows.
    private static DocumentNodePath FieldPath(DocumentNodePath grid, int index, int width)
    {
        int y = Math.DivRem(index, width, result: out int x);
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
                throw Type(name, expected: "a sequence of created objects");
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
                    throw TreeErrors.Fail(Codes.Type, message: $"An item of {name} is a created object.", path: at.Item(k));
                }

                IPersistable? created = null;
                try
                {
                    created = Factory.Create(className);
                    items.Attach(created);
                    MemberPersister reader = new(item, loading: true, factory: Factory, path: at.Item(k), errors: _errors, preview: Preview);
                    created.Persist(reader);
                    reader.Finish();
                    items.Place(k, child: created);
                }
                catch (Exception error)
                {
                    if (created is not null)
                    {
                        Factory.Release(created);
                    }

                    if (!Continue(error, at: at.Item(k), className: className))
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

            IPersistable item = items.Get(i) ?? throw TreeErrors.Fail(Codes.Corrupt, message: "A stored item is missing.", path: at.Item(i));
            written ??= [];
            ObjectNode node = new(Factory.ClassName(item), slot: null);
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
                ? throw Type(name, expected: $"a nested {profile} document")
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
                    ? throw TreeErrors.Fail(Codes.Reference, message: $"Reference \"{name}\" is in domain {reference.Domain}.", path: _path.Member(name), value: reference.Domain, constraint: domain)
                    : reference.Key
                : found is IntegerNode integer && integer.Width == 4 && integer.Signed ? (int)integer.Value : throw Type(name, expected: "a reference");
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
            RecordNode record = found is null ? new RecordNode() : found is RecordNode node ? node : throw Type(name, expected: "a record");
            MemberPersister child = new(record, loading: true, Factory, path: _path.Member(name), _errors, preview: Preview);
            body(child);
            child.Finish();
            return;
        }
        RecordNode written = new();
        body(new MemberPersister(written, loading: false, Factory, path: _path.Member(name), _errors, preview: Preview));
        Store(name, written);
    }

    public void List<T>(string name, IList<T> items, Func<IItemSlot, T, T> transfer)
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
                throw Type(name, expected: "a list");
            }

            for (int i = 0; i < list.Items.Count; i++)
            {
                ItemSlot slot = new(list.Items[i], loading: true, Factory, path: _path.Member(name).Item(i), _errors, preview: Preview);
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
            ItemSlot slot = new(null, loading: false, Factory, path: _path.Member(name).Item(i), _errors, preview: Preview);
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
                : found is ReferenceNode reference && width == 4 && signed ? reference.Key : throw Type(name, expected: "an integer");
        }
        if (current != fallback)
        {
            Store(name, new IntegerNode(width, signed, value: current));
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
            ? throw TreeErrors.Fail(Codes.Duplicate, message: $"Member \"{name}\" is read twice.", path: _path.Member(name))
            : Find(name);
    }

    private void Store(string name, TreeValue value)
    {
        if (Find(name) is not null)
        {
            throw TreeErrors.Fail(Codes.Duplicate, message: $"Member \"{name}\" is stored twice.", path: _path.Member(name));
        }

        _add!(name, value);
    }

    private ConversionException Type(string name, string expected)
        => TreeErrors.Fail(Codes.Type, message: $"{name} is {expected}.", path: _path.Member(name));

    private long RawInteger(string name, long current, int width, bool signed)
    {
        if (Loading)
        {
            TreeValue? found = Take(name);
            return found is null
                ? current
                : found is IntegerNode node && node.Width == width && node.Signed == signed ? node.Value : throw Type(name, expected: "an integer");
        }
        Store(name, new IntegerNode(width, signed, value: current));
        return current;
    }

    private bool Continue(Exception error, DocumentNodePath at, string className) => _errors is null || _errors.Continue(error, at: at, className: className);

    private bool Continue(Exception error, DocumentNodePath at, IPersistable objectValue) => _errors is null || _errors.Continue(error, at: at, objectValue: objectValue, factory: Factory);

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

            if (!Continue(error, at: at, className: className))
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

    public TreeValue Finish()
        => !_used
            ? throw TreeErrors.Fail(Codes.MissingMember, message: "A list item stores one value.", _path)
            : _loading
                ? _loaded!
                : _stored!;

    public int Int32(int value) => (int)Integer(value, width: 4, signed: true);

    public byte Byte(byte value) => (byte)Integer(value, width: 1, signed: false);

    public sbyte SByte(sbyte value) => (sbyte)Integer(value, width: 1, signed: true);

    public ushort UInt16(ushort value) => (ushort)Integer(value, width: 2, signed: false);

    public short Int16(short value) => (short)Integer(value, width: 2, signed: true);

    public uint UInt32(uint value) => (uint)Integer(value, width: 4, signed: false);

    public bool Boolean(bool value)
    {
        Use();
        if (_loading)
        {
            return _loaded is not BooleanNode node ? throw TreeErrors.Fail(Codes.Type, message: "A list item is a boolean.", _path) : node.Value;
        }
        _stored = new BooleanNode(value);
        return value;
    }

    public string String(string value)
    {
        Use();
        if (_loading)
        {
            return _loaded is not StringNode node ? throw TreeErrors.Fail(Codes.Type, message: "A list item is a string.", _path) : node.Value;
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
                throw TreeErrors.Fail(Codes.Type, message: "A list item is a record.", _path);
            }

            MemberPersister child = new(node, loading: true, _factory, _path, _errors, preview: _preview);
            body(child);
            child.Finish();
            return;
        }
        RecordNode written = new();
        body(new MemberPersister(written, loading: false, _factory, _path, _errors, preview: _preview));
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
                throw TreeErrors.Fail(Codes.Type, message: "A list item is an object.", _path);
            }

            IPersistable? created = null;
            try
            {
                created = _factory.Create(className);
                MemberPersister child = new(node, loading: true, _factory, _path, _errors, preview: _preview);
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

                if (_errors is not null && !_errors.Continue(error, at: _path, className: className))
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
        ObjectNode written = new(_factory.ClassName(value), slot: null);
        value.Persist(new MemberPersister(written, loading: false, _factory, _path, _errors, preview: _preview));
        _stored = written;
        return value;
    }

    private long Integer(long current, int width, bool signed)
    {
        Use();
        if (_loading)
        {
            return _loaded is not IntegerNode node || node.Width != width || node.Signed != signed
                ? throw TreeErrors.Fail(Codes.Type, message: "A list item is an integer.", _path)
                : node.Value;
        }
        _stored = new IntegerNode(width, signed, value: current);
        return current;
    }

    private void Use()
    {
        if (_used)
        {
            throw TreeErrors.Fail(Codes.Duplicate, message: "A list item stores one value.", _path);
        }

        _used = true;
    }
}
