
using System.Globalization;

namespace Aow.Formats.Tree;

/// <summary>One value in the persistence tree. The tree has no schema.</summary>
public abstract class TreeValue
{
    /// <summary>A version 1 document has no list and no record. Nested values are checked too.</summary>
    internal virtual void RejectVersion(int version, DocumentNodePath path)
    {
    }

    internal virtual IEnumerable<(string Path, int Key)> ReferencesIn(string domain, string path)
    {
        return [];
    }
}

/// <summary>A signed or unsigned integer of a fixed width. The document stores the number only.</summary>
public sealed class IntegerNode(int width, bool signed, long value) : TreeValue
{
    public int Width { get; } = width;
    public bool Signed { get; } = signed;
    public long Value { get; } = value;
}

/// <summary>
/// One enum value. The type name is the C# enum and the integer is the stored value.
/// A declared value is written as that member's name. Any other value is written as its integer.
/// </summary>
public sealed class EnumNode(string typeName, int width, bool signed, long value) : TreeValue
{
    public string TypeName { get; } = typeName;
    public int Width { get; } = width;
    public bool Signed { get; } = signed;
    public long Value { get; } = value;
}

/// <summary>
/// One set. The mask is the little-endian image. Member names are stored in ascending ordinal
/// order, and an unnamed bit is stored as its integer.
/// </summary>
public sealed class SetNode(string typeName, int width, ulong mask) : TreeValue
{
    public string TypeName { get; } = typeName;
    public int Width { get; } = width;
    public ulong Mask { get; } = mask;
}

/// <summary>A cross reference. Version 1 writes the key as a plain integer and keeps the domain here.</summary>
public sealed class ReferenceNode(string domain, int key) : TreeValue
{
    public string Domain { get; } = domain;
    public int Key { get; } = key;

    internal override IEnumerable<(string Path, int Key)> ReferencesIn(string domain, string path)
    {
        if (domain == Domain) yield return (path, Key);
    }
}

/// <summary>A boolean. A named false is omitted. A list item stores false.</summary>
public sealed class BooleanNode(bool value) : TreeValue
{
    public bool Value { get; } = value;
}

/// <summary>Text that maps through code page 1252. An empty string is not stored.</summary>
public sealed class StringNode(string value) : TreeValue
{
    public string Value { get; } = value;
}

/// <summary>
/// rwStrings. <see cref="Lines"/> is the line list. <see cref="RawText"/> is set when the block
/// is not an array of lines, and the document then uses $text.
/// </summary>
public sealed class LinesNode : TreeValue
{
    public IReadOnlyList<string>? Lines { get; }
    public string? RawText { get; }

    private LinesNode(IReadOnlyList<string>? lines, string? rawText)
    {
        Lines = lines;
        RawText = rawText;
    }

    public static LinesNode FromLines(IReadOnlyList<string> lines) => new(lines, null);

    internal static LinesNode FromText(string text)
    {
        var lines = Original.StringsCodec.Lines(text);
        return lines is null ? new LinesNode(null, text) : new LinesNode(lines, null);
    }
}

/// <summary>rwStringsEx. The object values are signed 32-bit integers, one per line.</summary>
public sealed class LinesWithValuesNode : TreeValue
{
    public IReadOnlyList<string>? Lines { get; }
    public string? RawText { get; }
    public IReadOnlyList<int> Objects { get; }

    private LinesWithValuesNode(IReadOnlyList<string>? lines, string? rawText, IReadOnlyList<int> objects)
    {
        Lines = lines;
        RawText = rawText;
        Objects = objects;
    }

    public static LinesWithValuesNode FromLines(IReadOnlyList<string> lines, IReadOnlyList<int> objects) => new(lines, null, objects);

    internal static LinesWithValuesNode FromText(string text, IReadOnlyList<int> objects)
    {
        var lines = Original.StringsCodec.Lines(text);
        return lines is null ? new LinesWithValuesNode(null, text, objects) : new LinesWithValuesNode(lines, null, objects);
    }
}

/// <summary>Exact bytes of an rwData value. Length zero is not stored.</summary>
public sealed class BytesNode(byte[] data) : TreeValue
{
    public byte[] Data { get; } = data;
}

/// <summary>A stored null, the class id 0 of rwCreateEObject.</summary>
public sealed class NullNode : TreeValue
{
    public static NullNode Instance { get; } = new();
}

/// <summary>
/// One object. <see cref="ClassName"/> is set for a created object and a child. <see cref="Slot"/>
/// is the child's $slot. Members stay in call order. The document backend reorders them.
/// </summary>
public sealed class ObjectNode : TreeValue
{
    private readonly List<(string Name, TreeValue Value)> _members = [];

    public ObjectNode(string? className, int? slot)
    {
        ClassName = className;
        Slot = slot;
    }

    public string? ClassName { get; }
    public int? Slot { get; }
    public IReadOnlyList<(string Name, TreeValue Value)> Members => _members;

    internal void Add(string name, TreeValue value) => _members.Add((name, value));

    public TreeValue? Find(string name)
    {
        foreach (var (member, value) in _members)
            if (member == name) return value;
        return null;
    }

    /// <summary>Every reference of <paramref name="domain"/> under this object, with its path.</summary>
    public IEnumerable<(string Path, int Key)> References(string domain)
    {
        foreach (var (name, value) in _members)
            foreach (var hit in value.ReferencesIn(domain, name))
                yield return hit;
    }

    internal override void RejectVersion(int version, DocumentNodePath path)
    {
        foreach (var (name, value) in _members)
            value.RejectVersion(version, path.Member(name));
    }

    internal override IEnumerable<(string Path, int Key)> ReferencesIn(string domain, string path)
    {
        foreach (var (name, value) in _members)
            foreach (var hit in value.ReferencesIn(domain, path + "." + name))
                yield return hit;
    }
}

/// <summary>Named fields with no class and no slot. Version 1 refuses a record.</summary>
public sealed class RecordNode : TreeValue
{
    private readonly List<(string Name, TreeValue Value)> _members = [];

    public IReadOnlyList<(string Name, TreeValue Value)> Members => _members;

    internal void Add(string name, TreeValue value) => _members.Add((name, value));

    public TreeValue? Find(string name)
    {
        foreach (var (member, value) in _members)
            if (member == name) return value;
        return null;
    }

    internal override void RejectVersion(int version, DocumentNodePath path)
    {
        if (version < 3)
            throw TreeErrors.Fail(Codes.Type, "Version " + version.ToString(System.Globalization.CultureInfo.InvariantCulture) + " does not store a record.", path);
        foreach (var (name, value) in _members)
            value.RejectVersion(version, path.Member(name));
    }

    internal override IEnumerable<(string Path, int Key)> ReferencesIn(string domain, string path)
    {
        foreach (var (name, value) in _members)
            foreach (var hit in value.ReferencesIn(domain, path + "." + name))
                yield return hit;
    }
}

/// <summary>
/// Set bits of a counted bit array, in ascending index order. Bits past the count in the last byte
/// are <see cref="Residue"/>. Version 1 and version 2 refuse the shape.
/// </summary>
public sealed class BitArrayNode(int count, int[] indices, byte residue) : TreeValue
{
    public int Count { get; } = count;
    public IReadOnlyList<int> Indices { get; } = indices;
    public byte Residue { get; } = residue;

    internal override void RejectVersion(int version, DocumentNodePath path)
    {
        if (version < 3)
            throw TreeErrors.Fail(Codes.Type, "Version " + version.ToString(System.Globalization.CultureInfo.InvariantCulture) + " does not store a bit array.", path);
    }
}

/// <summary>
/// A bit list (Delphi <c>TBitList</c>): the stored default byte, the count, the set bits in
/// ascending order and the residue byte of the bits past the count. Since PE-2c-4 the backends
/// store it. No serializer moves it yet.
/// </summary>
public sealed class BitListNode(byte defaultValue, int count, int[] indices, byte residue) : TreeValue
{
    public byte Default { get; } = defaultValue;
    public int Count { get; } = count;
    public IReadOnlyList<int> Indices { get; } = indices;
    public byte Residue { get; } = residue;
}

/// <summary>
/// A list with a stored default (Delphi <c>TIntegerList</c> and <c>TByteList</c>). Width 4 holds
/// signed 32-bit values and width 1 unsigned bytes. A read past the end of the list returns the
/// default, so the default is state and is stored with the items.
/// </summary>
public sealed class DefaultListNode(int width, long defaultValue, IReadOnlyList<long> items) : TreeValue
{
    public int Width { get; } = width;
    public long Default { get; } = defaultValue;
    public IReadOnlyList<long> Items { get; } = items;
}

/// <summary>One slot of a keyed dynamic member: an object node or the stored null.</summary>
public readonly record struct SlotEntry(int Slot, TreeValue Value);

/// <summary>
/// The created objects of a keyed dynamic member, in stored order. The slot is the stored id less
/// the member's slot base, so it carries the object's identity. A slot holds an object node with a
/// class name, or <see cref="NullNode"/> for a stored null.
/// </summary>
public sealed class SlotListNode : TreeValue
{
    private readonly List<SlotEntry> _entries = [];

    public IReadOnlyList<SlotEntry> Entries => _entries;

    internal void Add(int slot, TreeValue value) => _entries.Add(new SlotEntry(slot, value));

    /// <summary>The value stored at <paramref name="slot"/>, or null when the slot is absent.</summary>
    public TreeValue? Find(int slot)
    {
        foreach (var entry in _entries)
            if (entry.Slot == slot) return entry.Value;
        return null;
    }

    internal override void RejectVersion(int version, DocumentNodePath path)
    {
        for (int i = 0; i < _entries.Count; i++)
            _entries[i].Value.RejectVersion(version, path.Item(i));
    }

    internal override IEnumerable<(string Path, int Key)> ReferencesIn(string domain, string path)
    {
        for (int i = 0; i < _entries.Count; i++)
            foreach (var hit in _entries[i].Value.ReferencesIn(domain, path + "[" + i.ToString(System.Globalization.CultureInfo.InvariantCulture) + "]"))
                yield return hit;
    }
}

/// <summary>
/// The fields of a map level (Delphi <c>TMapLevel</c> property 6): <see cref="Height"/> rows of
/// <see cref="Width"/> fixed objects of the level's field class, stored row after row. A field
/// carries no class name and no slot.
/// </summary>
public sealed class GridNode(int width, int height, IReadOnlyList<ObjectNode> fields) : TreeValue
{
    public int Width { get; } = width;
    public int Height { get; } = height;

    /// <summary>The fields in row order: the field at x, y is at <c>y * Width + x</c>.</summary>
    public IReadOnlyList<ObjectNode> Fields { get; } = fields;

    internal override void RejectVersion(int version, DocumentNodePath path)
    {
        for (int i = 0; i < Fields.Count; i++)
        {
            int y = Math.DivRem(i, Width, out int x);
            Fields[i].RejectVersion(version, path.Item(y).Item(x));
        }
    }

    internal override IEnumerable<(string Path, int Key)> ReferencesIn(string domain, string path)
    {
        for (int i = 0; i < Fields.Count; i++)
        {
            int y = Math.DivRem(i, Width, out int x);
            foreach (var hit in Fields[i].ReferencesIn(domain, path + "[" + y.ToString(CultureInfo.InvariantCulture) + "][" + x.ToString(CultureInfo.InvariantCulture) + "]"))
                yield return hit;
        }
    }
}

/// <summary>
/// The created objects of a dynamic member without slots (Delphi <c>TMapField</c> sprites, ids 50
/// and on), in stored order. An item is an object node with a class name and no slot, or
/// <see cref="NullNode"/> for a stored null.
/// </summary>
public sealed class SequenceNode(IReadOnlyList<TreeValue> items) : TreeValue
{
    public IReadOnlyList<TreeValue> Items { get; } = items;

    internal override void RejectVersion(int version, DocumentNodePath path)
    {
        for (int i = 0; i < Items.Count; i++)
            Items[i].RejectVersion(version, path.Item(i));
    }

    internal override IEnumerable<(string Path, int Key)> ReferencesIn(string domain, string path)
    {
        for (int i = 0; i < Items.Count; i++)
            foreach (var hit in Items[i].ReferencesIn(domain, path + "[" + i.ToString(CultureInfo.InvariantCulture) + "]"))
                yield return hit;
    }
}

/// <summary>
/// A whole original file nested in an object (Delphi <c>TAoWCampaign</c> ids 5 and 6, the campaign
/// map): the file's bytes and the name of the profile that reads them. The document holds the
/// file as a nested document of that profile.
/// </summary>
public sealed class NestedDocumentNode(string profile, byte[] image) : TreeValue
{
    public string Profile { get; } = profile;
    public byte[] Image { get; } = image;
}

/// <summary>A variable-length list. Version 1 refuses it. It is not the children of a node.</summary>
public sealed class ListNode : TreeValue
{
    public List<TreeValue> Items { get; } = [];

    internal override void RejectVersion(int version, DocumentNodePath path)
    {
        if (version == 1)
            throw TreeErrors.Fail(Codes.Type, "Version 1 does not store a list.", path);
        for (int i = 0; i < Items.Count; i++)
            Items[i].RejectVersion(version, path.Item(i));
    }

    internal override IEnumerable<(string Path, int Key)> ReferencesIn(string domain, string path)
    {
        for (int i = 0; i < Items.Count; i++)
            foreach (var hit in Items[i].ReferencesIn(domain, path + "[" + i.ToString(System.Globalization.CultureInfo.InvariantCulture) + "]"))
                yield return hit;
    }
}

/// <summary>rwChildren. Each child carries its class name and slot. Version 1 stores this shape.</summary>
public sealed class ChildListNode(IReadOnlyList<ObjectNode> children) : TreeValue
{
    public IReadOnlyList<ObjectNode> Children { get; } = children;

    internal override void RejectVersion(int version, DocumentNodePath path)
    {
        for (int i = 0; i < Children.Count; i++)
            Children[i].RejectVersion(version, path.Item(i));
    }

    internal override IEnumerable<(string Path, int Key)> ReferencesIn(string domain, string path)
    {
        for (int i = 0; i < Children.Count; i++)
            foreach (var hit in Children[i].ReferencesIn(domain, path + "[" + i.ToString(System.Globalization.CultureInfo.InvariantCulture) + "]"))
                yield return hit;
    }
}
