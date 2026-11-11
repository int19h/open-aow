namespace Aow.Formats.Json;

/// <summary>
/// A parsed or generated JSON value. Parsed values keep the span of their first character so
/// that a later semantic error points at the text (own-formats.md 2.8). The kinds of JSON values
/// are closed by the JSON grammar.
/// </summary>
public abstract class JsonValue
{
    public TextSpan Span { get; set; }

    /// <summary>The migration that synthesized this value, or null for parsed and imported values.</summary>
    public string? Origin { get; set; }

    /// <summary>The name of the value kind in diagnostics.</summary>
    public abstract string KindName { get; }
}

public sealed class JsonMember(string name, JsonValue value)
{
    public string Name { get; } = name;
    public JsonValue Value { get; set; } = value;
    public TextSpan NameSpan { get; set; }
}

public sealed class JsonObject : JsonValue
{
    public List<JsonMember> Members { get; } = [];
    public override string KindName => "object";

    public JsonValue? this[string name] => Find(name)?.Value;

    public JsonMember? Find(string name)
    {
        foreach (var member in Members)
            if (member.Name == name) return member;
        return null;
    }

    public JsonObject Add(string name, JsonValue value)
    {
        Members.Add(new JsonMember(name, value));
        return this;
    }

    public bool Remove(string name) => Members.RemoveAll(m => m.Name == name) > 0;
}

/// <summary>How the canonical writer lays out an array. Only map field rows use single lines.</summary>
public enum ArrayLayout
{
    Expanded,
    RowsOnLines,
}

public sealed class JsonArray : JsonValue
{
    public List<JsonValue> Items { get; } = [];
    public ArrayLayout Layout { get; set; }
    public override string KindName => "array";

    public JsonArray Add(JsonValue value)
    {
        Items.Add(value);
        return this;
    }
}

public sealed class JsonString(string value) : JsonValue
{
    public string Value { get; } = value;
    public override string KindName => "string";
}

/// <summary>Documents carry only integers. Floating-point payloads use exact bits.</summary>
public sealed class JsonInteger(long value) : JsonValue
{
    public long Value { get; } = value;
    public override string KindName => "integer";
}

public sealed class JsonBoolean(bool value) : JsonValue
{
    public bool Value { get; } = value;
    public override string KindName => "boolean";
}

public sealed class JsonNull : JsonValue
{
    public override string KindName => "null";
}
