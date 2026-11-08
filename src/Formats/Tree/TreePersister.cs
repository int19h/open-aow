using System.Runtime.CompilerServices;
using Aow.Rtl;

namespace Aow.Formats.Tree;

/// <summary>
/// The load's <see cref="LoadErrorHandler"/>, with the same stop flag as the original stream.
/// A null handler continues. Once a handler stops the load, later exceptions stop too.
/// An empty message is reported as "Error Reading EObject".
/// </summary>
sealed class LoadErrorPolicy
{
    private readonly LoadErrorHandler? handler;
    private bool stopped;

    internal LoadErrorPolicy(LoadErrorHandler? handler) => this.handler = handler;

    internal bool Continue(System.Exception error)
    {
        if (stopped) return false;
        bool continueLoad = true;
        if (handler is not null)
        {
            string message = error.Message.Length == 0 ? "Error Reading EObject" : error.Message;
            handler(message, ref continueLoad);
        }
        if (!continueLoad) stopped = true;
        return !stopped;
    }
}

/// <summary>Writes one object into a tree. The root carries no class name. Created objects and children do.</summary>
public static class TreeWriter
{
    public static ObjectNode Write(IPersistable obj, IPersistableFactory factory)
    {
        var node = new ObjectNode(null, null);
        var persister = new MemberPersister(node, false, factory, DocPath.Root);
        obj.Persist(persister);
        return node;
    }
}

/// <summary>
/// Reads a tree into an object. After <see cref="IPersistable.Persist"/> returns, a member the
/// serializer did not consume is an error. The path names that member.
/// </summary>
public static class TreeReader
{
    public static void Read(ObjectNode node, IPersistable obj, IPersistableFactory factory, LoadErrorHandler? onError = null)
    {
        var persister = new MemberPersister(node, true, factory, DocPath.Root, new LoadErrorPolicy(onError));
        obj.Persist(persister);
        persister.Finish();
    }
}

/// <summary>The persister over one object or one record.</summary>
sealed class MemberPersister : IPersister
{
    private readonly IReadOnlyList<(string Name, TreeValue Value)> existing;
    private readonly Action<string, TreeValue>? add;
    private readonly HashSet<string> consumed = new(StringComparer.Ordinal);
    private readonly DocPath path;
    private readonly LoadErrorPolicy? errors;

    public MemberPersister(ObjectNode node, bool loading, IPersistableFactory factory, DocPath path, LoadErrorPolicy? errors = null)
        : this(node.Members, loading ? null : node.Add, loading, factory, path, errors)
    {
    }

    public MemberPersister(RecordNode node, bool loading, IPersistableFactory factory, DocPath path, LoadErrorPolicy? errors = null)
        : this(node.Members, loading ? null : node.Add, loading, factory, path, errors)
    {
    }

    private MemberPersister(IReadOnlyList<(string Name, TreeValue Value)> existing, Action<string, TreeValue>? add, bool loading, IPersistableFactory factory, DocPath path, LoadErrorPolicy? errors)
    {
        this.existing = existing;
        this.add = add;
        Loading = loading;
        Factory = factory;
        this.path = path;
        this.errors = errors;
    }

    public bool Loading { get; }
    public IPersistableFactory Factory { get; }

    public bool Has(string name) => Loading && Find(name) is not null;

    public void Finish()
    {
        if (!Loading) return;
        foreach (var (name, _) in existing)
            if (!consumed.Contains(name))
                throw TreeErrors.Fail(Codes.Required, $"Member \"{name}\" was not read.", path.Member(name));
    }

    public void Int(string name, ref int value, int fallback = 0) =>
        value = (int)Integer(name, value, fallback, 4, true);

    public void Byte(string name, ref byte value, byte fallback = 0) =>
        value = (byte)Integer(name, value, fallback, 1, false);

    public void SByte(string name, ref sbyte value, sbyte fallback = 0) =>
        value = (sbyte)Integer(name, value, fallback, 1, true);

    public void UShort(string name, ref ushort value, ushort fallback = 0) =>
        value = (ushort)Integer(name, value, fallback, 2, false);

    public void Short(string name, ref short value, short fallback = 0) =>
        value = (short)Integer(name, value, fallback, 2, true);

    public void UInt(string name, ref uint value, uint fallback = 0) =>
        value = (uint)Integer(name, value, fallback, 4, false);

    public void Enum<TEnum>(string name, ref TEnum value, TEnum fallback = default) where TEnum : struct, Enum
    {
        var (width, signed) = EnumTypeInfo.Layout(typeof(TEnum));
        string typeName = typeof(TEnum).Name;
        if (Loading)
        {
            var found = Take(name);
            if (found is null)
            {
                value = fallback;
                return;
            }
            if (found is not EnumNode node || node.TypeName != typeName || node.Width != width || node.Signed != signed)
                throw Type(name, "a " + typeName);
            value = (TEnum)System.Enum.ToObject(typeof(TEnum), node.Value);
            return;
        }
        Store(name, new EnumNode(typeName, width, signed, Convert.ToInt64(value)));
    }

    public void Set<E>(string name, ref TSet<E> value) where E : unmanaged, Enum
    {
        int width = default(TSet<E>).StorageSize;
        string typeName = typeof(E).Name;
        if (width is < 1 or > 8)
            throw new InvalidOperationException("A set image is 1 to 8 bytes.");
        if (Loading)
        {
            var found = Take(name);
            if (found is null) return;
            if (found is not SetNode node || node.TypeName != typeName || node.Width != width)
                throw Type(name, "a set of " + typeName);
            value = TSet<E>.FromMask(node.Mask);
            return;
        }
        Span<byte> image = stackalloc byte[width];
        value.CopyImageTo(image);
        ulong mask = 0;
        for (int i = 0; i < width; i++) mask |= (ulong)image[i] << (8 * i);
        Store(name, new SetNode(typeName, width, mask));
    }

    public void Bool(string name, ref bool value)
    {
        if (Loading)
        {
            var found = Take(name);
            if (found is null) value = false;
            else if (found is BoolNode node) value = node.Value;
            else throw Type(name, "a boolean");
            return;
        }
        if (value) Store(name, new BoolNode(true));
    }

    public void String(string name, ref string value)
    {
        if (Loading)
        {
            var found = Take(name);
            if (found is null) value = "";
            else if (found is StringNode node) value = node.Value;
            else throw Type(name, "a string");
            return;
        }
        if (!string.IsNullOrEmpty(value)) Store(name, new StringNode(value));
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
            if (found is not LinesExNode node) throw Type(name, "lines with objects");
            foreach (var line in node.Lines ?? Split(node.RawText ?? "")) lines.Add(line);
            if (lines.Count != node.Objects.Count)
                throw TreeErrors.Fail(Codes.Corrupt, $"The text has {lines.Count} lines and {node.Objects.Count} object values.", path.Member(name));
            foreach (var item in node.Objects) objects.Add(item);
            return;
        }
        if (lines.Count == 0) return;
        if (lines.Count != objects.Count)
            throw TreeErrors.Fail(Codes.Corrupt, $"The text has {lines.Count} lines and {objects.Count} object values.", path.Member(name));
        Store(name, LinesExNode.FromLines(lines is List<string> list ? list : [.. lines], objects is List<int> ids ? ids : [.. objects]));
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
                throw TreeErrors.Fail(Codes.Range, $"{name} holds exactly {data.Length} bytes.", path.Member(name), value: node.Data.Length.ToString(System.Globalization.CultureInfo.InvariantCulture), constraint: $"{data.Length} bytes");
            node.Data.CopyTo(data);
            return;
        }
        if (data.Length == 0) return;
        Store(name, new BytesNode(data.ToArray()));
    }

    public void RawInt(string name, ref int value) => value = (int)RawInteger(name, value, 4, true);

    public void RawByte(string name, ref byte value) => value = (byte)RawInteger(name, value, 1, false);

    public void RawSByte(string name, ref sbyte value) => value = (sbyte)RawInteger(name, value, 1, true);

    public void RawBool(string name, ref bool value)
    {
        if (Loading)
        {
            var found = Take(name);
            if (found is null) return;
            if (found is BoolNode node) Unsafe.As<bool, byte>(ref value) = node.Value ? (byte)1 : (byte)0;
            else if (found is IntegerNode { Width: 1, Signed: false } number && number.Value is >= 0 and <= 255)
                Unsafe.As<bool, byte>(ref value) = (byte)number.Value;
            else throw Type(name, "a boolean");
            return;
        }
        byte stored = Unsafe.As<bool, byte>(ref value);
        if (stored is 0 or 1) Store(name, new BoolNode(stored != 0));
        else Store(name, new IntegerNode(1, false, stored));
    }

    public void BitArray(string name, int count, Span<byte> bits)
    {
        int length = count < 0 ? -1 : (count + 7) >> 3;
        if (length < 0)
            throw TreeErrors.Fail(Codes.Range, name + " has a negative count.", path.Member(name), value: count.ToString(System.Globalization.CultureInfo.InvariantCulture));
        if (bits.Length != length)
            throw TreeErrors.Fail(Codes.Range, name + " holds exactly " + length.ToString(System.Globalization.CultureInfo.InvariantCulture) + " bytes.", path.Member(name), value: bits.Length.ToString(System.Globalization.CultureInfo.InvariantCulture), constraint: length.ToString(System.Globalization.CultureInfo.InvariantCulture) + " bytes");
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
                    throw TreeErrors.Fail(Codes.Range, name + " is outside its storage range.", path.Member(name), value: index.ToString(System.Globalization.CultureInfo.InvariantCulture));
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

    public void Object(string name, IPersistable obj)
    {
        if (Loading)
        {
            var found = Take(name);
            ObjectNode nested;
            if (found is null) nested = new ObjectNode(null, null);
            else if (found is ObjectNode node) nested = node;
            else throw Type(name, "an object");
            var child = new MemberPersister(nested, true, Factory, path.Member(name), errors);
            // rwEObject wraps MainReadWrite and leaves the caller's instance in place.
            try
            {
                obj.Persist(child);
                child.Finish();
            }
            catch (System.Exception error)
            {
                if (!Continue(error)) throw;
            }
            return;
        }
        var written = new ObjectNode(null, null);
        obj.Persist(new MemberPersister(written, false, Factory, path.Member(name), errors));
        Store(name, written);
    }

    public void Created(string name, ref IPersistable? obj)
    {
        if (Loading)
        {
            var found = Take(name);
            if (found is null || found is NullNode)
            {
                obj = null;
                return;
            }
            if (found is not ObjectNode node || node.ClassName is not string className)
                throw Type(name, "a created object");
            IPersistable? created = null;
            try
            {
                created = Factory.Create(className);
                var child = new MemberPersister(node, true, Factory, path.Member(name), errors);
                created.Persist(child);
                child.Finish();
                obj = created;
            }
            catch (System.Exception error)
            {
                if (created is not null) Factory.Release(created);
                obj = null;
                if (!Continue(error)) throw;
            }
            return;
        }
        if (obj is null)
        {
            Store(name, NullNode.Instance);
            return;
        }
        var written = new ObjectNode(Factory.ClassName(obj), null);
        obj.Persist(new MemberPersister(written, false, Factory, path.Member(name), errors));
        Store(name, written);
    }

    public void Created<T>(string name, ref T? obj) where T : class, IPersistable
    {
        IPersistable? box = obj;
        Created(name, ref box);
        if (box is not null && box is not T)
            throw TreeErrors.Fail(Codes.ClassFamily, $"Created class {Factory.ClassName(box)} is not a {typeof(T).Name}.", path.Member(name), value: Factory.ClassName(box), constraint: typeof(T).Name);
        obj = (T?)box;
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
                    throw TreeErrors.Fail(Codes.MissingMember, "A child needs a class and a slot.", path.Member(name));
                IPersistable? created = null;
                try
                {
                    created = Factory.Create(className);
                    children.Attach(created);
                    var reader = new MemberPersister(child, true, Factory, path.Member(name).Item(slot), errors);
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
            var child = children.Get(i) ?? throw TreeErrors.Fail(Codes.Corrupt, "A stored child is missing.", path.Member(name).Item(i));
            var node = new ObjectNode(Factory.ClassName(child), children.Slot(i));
            child.Persist(new MemberPersister(node, false, Factory, path.Member(name).Item(children.Slot(i)), errors));
            written.Add(node);
        }
        Store(name, new ChildListNode(written));
    }

    public void Reference(string name, string domain, ref int key, int fallback = 0)
    {
        if (Loading)
        {
            var found = Take(name);
            if (found is null)
            {
                key = fallback;
                return;
            }
            if (found is ReferenceNode reference)
            {
                if (reference.Domain != domain)
                    throw TreeErrors.Fail(Codes.Reference, $"Reference \"{name}\" is in domain {reference.Domain}.", path.Member(name), value: reference.Domain, constraint: domain);
                key = reference.Key;
                return;
            }
            if (found is IntegerNode integer && integer.Width == 4 && integer.Signed)
            {
                key = (int)integer.Value;
                return;
            }
            throw Type(name, "a reference");
        }
        if (key != fallback) Store(name, new ReferenceNode(domain, key));
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
            var child = new MemberPersister(record, true, Factory, path.Member(name), errors);
            body(child);
            child.Finish();
            return;
        }
        var written = new RecordNode();
        body(new MemberPersister(written, false, Factory, path.Member(name), errors));
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
                T value = default!;
                var slot = new ItemSlot(list.Items[i], true, Factory, path.Member(name).Item(i), errors);
                transfer(slot, ref value);
                slot.Finish();
                items.Add(value);
            }
            return;
        }
        if (items.Count == 0) return;
        var written = new ListNode();
        for (int i = 0; i < items.Count; i++)
        {
            T value = items[i];
            var slot = new ItemSlot(null, false, Factory, path.Member(name).Item(i), errors);
            transfer(slot, ref value);
            items[i] = value;
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
        foreach (var (member, value) in existing)
            if (member == name) return value;
        return null;
    }

    private TreeValue? Take(string name)
    {
        if (!consumed.Add(name))
            throw TreeErrors.Fail(Codes.Duplicate, $"Member \"{name}\" is read twice.", path.Member(name));
        return Find(name);
    }

    private void Store(string name, TreeValue value)
    {
        if (Find(name) is not null)
            throw TreeErrors.Fail(Codes.Duplicate, $"Member \"{name}\" is stored twice.", path.Member(name));
        add!(name, value);
    }

    private ConversionException Type(string name, string expected) =>
        TreeErrors.Fail(Codes.Type, $"{name} is {expected}.", path.Member(name));

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

    private bool Continue(System.Exception error) => errors is null || errors.Continue(RtlUnit.Wrap(error));

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
    private readonly TreeValue? loaded;
    private readonly bool loading;
    private readonly IPersistableFactory factory;
    private readonly DocPath path;
    private readonly LoadErrorPolicy? errors;
    private TreeValue? stored;
    private bool used;

    public ItemSlot(TreeValue? loaded, bool loading, IPersistableFactory factory, DocPath path, LoadErrorPolicy? errors = null)
    {
        this.loaded = loaded;
        this.loading = loading;
        this.factory = factory;
        this.path = path;
        this.errors = errors;
    }

    public TreeValue Finish()
    {
        if (!used) throw TreeErrors.Fail(Codes.MissingMember, "A list item stores one value.", path);
        return loading ? loaded! : stored!;
    }

    public void Int(ref int value) => value = (int)Integer(value, 4, true);

    public void Byte(ref byte value) => value = (byte)Integer(value, 1, false);

    public void SByte(ref sbyte value) => value = (sbyte)Integer(value, 1, true);

    public void UShort(ref ushort value) => value = (ushort)Integer(value, 2, false);

    public void Short(ref short value) => value = (short)Integer(value, 2, true);

    public void UInt(ref uint value) => value = (uint)Integer(value, 4, false);

    public void Bool(ref bool value)
    {
        Use();
        if (loading)
        {
            if (loaded is not BoolNode node) throw TreeErrors.Fail(Codes.Type, "A list item is a boolean.", path);
            value = node.Value;
            return;
        }
        stored = new BoolNode(value);
    }

    public void String(ref string value)
    {
        Use();
        if (loading)
        {
            if (loaded is not StringNode node) throw TreeErrors.Fail(Codes.Type, "A list item is a string.", path);
            value = node.Value;
            return;
        }
        stored = new StringNode(value ?? "");
    }

    public void Record(Action<IPersister> body)
    {
        Use();
        if (loading)
        {
            if (loaded is not RecordNode node) throw TreeErrors.Fail(Codes.Type, "A list item is a record.", path);
            var child = new MemberPersister(node, true, factory, path, errors);
            body(child);
            child.Finish();
            return;
        }
        var written = new RecordNode();
        body(new MemberPersister(written, false, factory, path, errors));
        stored = written;
    }

    public void Object(ref IPersistable? value)
    {
        Use();
        if (loading)
        {
            if (loaded is NullNode || loaded is null)
            {
                value = null;
                return;
            }
            if (loaded is not ObjectNode node || node.ClassName is not string className)
                throw TreeErrors.Fail(Codes.Type, "A list item is an object.", path);
            IPersistable? created = null;
            try
            {
                created = factory.Create(className);
                var child = new MemberPersister(node, true, factory, path, errors);
                created.Persist(child);
                child.Finish();
                value = created;
            }
            catch (System.Exception error)
            {
                if (created is not null) factory.Release(created);
                value = null;
                if (errors is not null && !errors.Continue(RtlUnit.Wrap(error))) throw;
            }
            return;
        }
        if (value is null)
        {
            stored = NullNode.Instance;
            return;
        }
        var written = new ObjectNode(factory.ClassName(value), null);
        value.Persist(new MemberPersister(written, false, factory, path, errors));
        stored = written;
    }

    private long Integer(long current, int width, bool signed)
    {
        Use();
        if (loading)
        {
            if (loaded is not IntegerNode node || node.Width != width || node.Signed != signed)
                throw TreeErrors.Fail(Codes.Type, "A list item is an integer.", path);
            return node.Value;
        }
        stored = new IntegerNode(width, signed, current);
        return current;
    }

    private void Use()
    {
        if (used) throw TreeErrors.Fail(Codes.Duplicate, "A list item stores one value.", path);
        used = true;
    }
}
