using Aow.Formats.Original;

namespace Aow.Formats.Tree;

/// <summary>One value in the persistence tree. The tree has no schema.</summary>
public abstract class TreeValue
{
    /// <summary>A version 1 document has no list and no record. Nested values are checked too.</summary>
    internal virtual void RejectVersion(int version, DocPath path)
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
public sealed class BoolNode(bool value) : TreeValue
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
public sealed class LinesExNode : TreeValue
{
    public IReadOnlyList<string>? Lines { get; }
    public string? RawText { get; }
    public IReadOnlyList<int> Objects { get; }

    private LinesExNode(IReadOnlyList<string>? lines, string? rawText, IReadOnlyList<int> objects)
    {
        Lines = lines;
        RawText = rawText;
        Objects = objects;
    }

    public static LinesExNode FromLines(IReadOnlyList<string> lines, IReadOnlyList<int> objects) => new(lines, null, objects);

    internal static LinesExNode FromText(string text, IReadOnlyList<int> objects)
    {
        var lines = Original.StringsCodec.Lines(text);
        return lines is null ? new LinesExNode(null, text, objects) : new LinesExNode(lines, null, objects);
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
    private readonly List<(string Name, TreeValue Value)> members = [];

    public ObjectNode(string? className, int? slot)
    {
        ClassName = className;
        Slot = slot;
    }

    public string? ClassName { get; }
    public int? Slot { get; }
    public IReadOnlyList<(string Name, TreeValue Value)> Members => members;

    internal void Add(string name, TreeValue value) => members.Add((name, value));

    public TreeValue? Find(string name)
    {
        foreach (var (member, value) in members)
            if (member == name) return value;
        return null;
    }

    /// <summary>Every reference of <paramref name="domain"/> under this object, with its path.</summary>
    public IEnumerable<(string Path, int Key)> References(string domain)
    {
        foreach (var (name, value) in members)
            foreach (var hit in value.ReferencesIn(domain, name))
                yield return hit;
    }

    internal override void RejectVersion(int version, DocPath path)
    {
        foreach (var (name, value) in members)
            value.RejectVersion(version, path.Member(name));
    }

    internal override IEnumerable<(string Path, int Key)> ReferencesIn(string domain, string path)
    {
        foreach (var (name, value) in members)
            foreach (var hit in value.ReferencesIn(domain, path + "." + name))
                yield return hit;
    }
}

/// <summary>Named fields with no class and no slot. Version 1 refuses a record.</summary>
public sealed class RecordNode : TreeValue
{
    private readonly List<(string Name, TreeValue Value)> members = [];

    public IReadOnlyList<(string Name, TreeValue Value)> Members => members;

    internal void Add(string name, TreeValue value) => members.Add((name, value));

    public TreeValue? Find(string name)
    {
        foreach (var (member, value) in members)
            if (member == name) return value;
        return null;
    }

    internal override void RejectVersion(int version, DocPath path)
    {
        if (version < 3)
            throw TreeErrors.Fail(Codes.Type, "Version " + version.ToString(System.Globalization.CultureInfo.InvariantCulture) + " does not store a record.", path);
        foreach (var (name, value) in members)
            value.RejectVersion(version, path.Member(name));
    }

    internal override IEnumerable<(string Path, int Key)> ReferencesIn(string domain, string path)
    {
        foreach (var (name, value) in members)
            foreach (var hit in value.ReferencesIn(domain, path + "." + name))
                yield return hit;
    }
}

/// <summary>A variable-length list. Version 1 refuses it. It is not the children of a node.</summary>
public sealed class ListNode : TreeValue
{
    public List<TreeValue> Items { get; } = [];

    internal override void RejectVersion(int version, DocPath path)
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

    internal override void RejectVersion(int version, DocPath path)
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
