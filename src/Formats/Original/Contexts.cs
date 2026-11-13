using System.Buffers.Binary;
using Aow.Formats.Json;
using Aow.Formats.Schema;

namespace Aow.Formats.Original;

/// <summary>Counts of layout residue kept while importing, for the check summary.</summary>
public sealed class LayoutCounts
{
    public int Parts { get; set; }
    public int Objects { get; set; }
    public Dictionary<string, int> Categories { get; } = new(StringComparer.Ordinal);
    public Dictionary<string, int> LegacyRules { get; } = new(StringComparer.Ordinal);

    public void Add(string category)
    {
        Parts++;
        Categories[category] = Categories.GetValueOrDefault(category) + 1;
    }

    public void Merge(LayoutCounts other)
    {
        Parts += other.Parts;
        Objects += other.Objects;
        foreach (var (k, v) in other.Categories) Categories[k] = Categories.GetValueOrDefault(k) + v;
        foreach (var (k, v) in other.LegacyRules) LegacyRules[k] = LegacyRules.GetValueOrDefault(k) + v;
    }
}

/// <summary>The state of one original import: the bytes, the engine table and the limits.</summary>
public sealed class DecodeContext
{
    private long _nodes;
    private int _depth;

    internal DecodeContext(byte[] data, FormatSchema schema, string engine, string profile, string? file, FormatLimits limits, LayoutCounts counts)
    {
        Data = data;
        Schema = schema;
        Engine = engine;
        Profile = profile;
        File = file;
        Limits = limits;
        Counts = counts;
    }

    public byte[] Data { get; }
    public FormatSchema Schema { get; }
    public string Engine { get; }
    public string Profile { get; }
    public string? File { get; }
    public FormatLimits Limits { get; }
    public LayoutCounts Counts { get; }

    /// <summary>Set for a nested image: diagnostics name the enclosing property.</summary>
    public string? Nested { get; init; }

    /// <summary>Offsets are in the inflated payload, not in the file.</summary>
    public bool Inflated { get; init; }

    public ConversionException Fail(string code, string message, long offset, DocumentNodePath? path = null, string? value = null, string? constraint = null) =>
        new(new FormatDiagnostic(code, Nested is null ? message : message + " In the nested image of " + Nested + ".")
        {
            File = File,
            Offset = offset,
            Profile = Profile,
            Path = path?.ToString(),
            Value = value,
            Constraint = constraint,
            Decompressed = Inflated,
        });

    public void Need(int start, int count, int end, DocumentNodePath path, string what)
    {
        if (count < 0 || start < 0 || (long)start + count > end)
            throw Fail(Codes.Truncated, $"The {what} runs past the end of its value.", start, path);
    }

    public int Int32(int at) => BinaryPrimitives.ReadInt32LittleEndian(Data.AsSpan(at, 4));

    public void Enter(int offset, DocumentNodePath path)
    {
        if (++_nodes > Limits.Nodes) throw Fail(Codes.Limit, "The image decodes to too many objects.", offset, path, null, $"at most {Limits.Nodes} decoded objects");
        if (++_depth > Limits.Depth) throw Fail(Codes.Limit, "The image nests objects too deeply.", offset, path, null, $"depth at most {Limits.Depth}");
    }

    public void Leave() => _depth--;

    public void CheckItems(int count, int offset, DocumentNodePath path)
    {
        if (count > Limits.CollectionItems)
            throw Fail(Codes.Limit, "A collection has too many items.", offset, path, count.ToString(System.Globalization.CultureInfo.InvariantCulture), $"at most {Limits.CollectionItems} items");
    }
}

/// <summary>The state of one strict export.</summary>
public sealed class EncodeContext
{
    private int _depth;

    internal EncodeContext(FormatSchema schema, string engine, string profile, string? file, FormatLimits limits, ExportOptions options)
    {
        Schema = schema;
        Engine = engine;
        Profile = profile;
        File = file;
        Limits = limits;
        Options = options;
    }

    public FormatSchema Schema { get; }
    public string Engine { get; }
    public string Profile { get; }
    public string? File { get; }
    public FormatLimits Limits { get; }
    public ExportOptions Options { get; }

    /// <summary>Encoding identity that a stale fingerprint or an edit discarded.</summary>
    public List<FormatDiagnostic> Invalidated { get; } = [];

    public ConversionException Fail(string code, string message, JsonValue? at, DocumentNodePath path, string? value = null, string? constraint = null) =>
        new(new FormatDiagnostic(code, message)
        {
            File = File,
            Span = at?.Span ?? default,
            Path = path.ToString(),
            Value = value,
            Constraint = constraint,
        });

    public void Enter(JsonValue at, DocumentNodePath path)
    {
        if (++_depth > Limits.Depth) throw Fail(Codes.Limit, "The document nests objects too deeply.", at, path, null, $"depth at most {Limits.Depth}");
    }

    public void Leave() => _depth--;
}

public sealed record ExportOptions
{
    /// <summary>Refuse to rebuild any part whose retained exact layout is stale or missing.</summary>
    public bool ExactLayout { get; init; }

    public static ExportOptions Default { get; } = new();
}

/// <summary>One entry of a stored property table.</summary>
public readonly record struct TableEntry(int Id, int Offset, bool Long, int Index);

/// <summary>A stored property table (persistence.md 1.3).</summary>
public sealed class PropertyTable
{
    public required List<TableEntry> Entries { get; init; }
    public required int DataBase { get; init; }

    /// <summary>The count byte has bit 7 set although no long entry follows.</summary>
    public bool EmptyLongMarker { get; init; }

    public static PropertyTable Read(DecodeContext context, int start, int end, DocumentNodePath path)
    {
        var data = context.Data;
        context.Need(start, 1, end, path, "property table");
        int at = start;
        int shortCount = data[at++];
        int longCount = 0;
        bool marker = false;
        if ((shortCount & 0x80) != 0)
        {
            shortCount &= 0x7F;
            context.Need(at, 4, end, path, "property table");
            longCount = context.Int32(at);
            at += 4;
            marker = true;
            if (longCount < 0) throw context.Fail(Codes.Corrupt, "A property table has a negative long entry count.", start, path, longCount.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        context.CheckItems(shortCount + longCount, start, path);
        long tableEnd = at + 2L * shortCount + 8L * longCount;
        if (tableEnd > end) throw context.Fail(Codes.Truncated, "A property table runs past the end of its object.", start, path);
        var entries = new List<TableEntry>(shortCount + longCount);
        for (int i = 0; i < shortCount; i++, at += 2)
            entries.Add(new TableEntry(data[at], data[at + 1], false, i));
        for (int i = 0; i < longCount; i++, at += 8)
            entries.Add(new TableEntry(context.Int32(at), context.Int32(at + 4), true, shortCount + i));
        int length = end - at;
        foreach (var e in entries)
            if (e.Offset < 0 || e.Offset > length)
                throw context.Fail(Codes.Corrupt, $"Property {e.Id} has an offset outside its object.", start, path, e.Offset.ToString(System.Globalization.CultureInfo.InvariantCulture), $"0 to {length}");
        return new PropertyTable { Entries = entries, DataBase = at, EmptyLongMarker = marker && longCount == 0 };
    }

    public static bool IsShort(int id, int offset) => id is >= 0 and <= 127 && offset is >= 0 and <= 255;

    /// <summary>Writes a table in the original writer's form: short entries first, then long ones.</summary>
    public static void WriteCanonical(ByteBuffer output, IReadOnlyList<(int Id, int Offset)> entries, DocumentNodePath path, EncodeContext context)
    {
        int shortCount = 0, longCount = 0;
        foreach (var (id, offset) in entries)
            if (IsShort(id, offset)) shortCount++;
            else longCount++;
        if (shortCount > 127)
            throw context.Fail(Codes.Unrepresentable, "An object has more short property entries than the original table can count.", null, path, shortCount.ToString(System.Globalization.CultureInfo.InvariantCulture), "at most 127 short entries");
        if (longCount != 0)
        {
            output.Byte((byte)(shortCount | 0x80));
            output.Int32(longCount);
        }
        else output.Byte((byte)shortCount);
        foreach (var (id, offset) in entries)
            if (IsShort(id, offset))
            {
                output.Byte((byte)id);
                output.Byte((byte)offset);
            }
        foreach (var (id, offset) in entries)
            if (!IsShort(id, offset))
            {
                output.Int32(id);
                output.Int32(offset);
            }
    }
}
