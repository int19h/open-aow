using System.Globalization;
using System.Text;

namespace Aow.Formats.Json;

/// <summary>
/// The canonical writer (own-formats.md 2.1): UTF-8 without a byte order mark, LF line ends,
/// two-space indentation, one member or item per line, empty containers as {} and [], and one
/// final LF. The only exception is an array marked <see cref="ArrayLayout.RowsOnLines"/>: each of
/// its items, a map field row, takes one line with compact nested values. Strings escape quotes,
/// backslashes and controls with the short escapes for backspace, tab, LF, form feed and CR,
/// and lowercase \u escapes for other controls and U+007F to U+009F. Unpaired surrogates fail.
/// A member named Fields that holds an array of arrays is written as rows too, so a parsed
/// document writes back canonically without its layout marks.
/// </summary>
public static class JsonWriter
{
    public static byte[] ToUtf8(JsonValue value) => new UTF8Encoding(false, throwOnInvalidBytes: true).GetBytes(ToText(value));

    public static string ToText(JsonValue value)
    {
        StringBuilder builder = new();
        Write(builder, value, depth: 0);
        _ = builder.Append('\n');
        return builder.ToString();
    }

    private static void Indent(StringBuilder builder, int depth) => builder.Append(' ', repeatCount: depth * 2);

    private static void Write(StringBuilder builder, JsonValue value, int depth)
    {
        switch (value)
        {
            case JsonObject objectValue:
                if (objectValue.Members.Count == 0)
                {
                    _ = builder.Append("{}");
                    return;
                }
                _ = builder.Append("{\n");
                for (int i = 0; i < objectValue.Members.Count; i++)
                {
                    Indent(builder, depth: depth + 1);
                    WriteString(builder, value: objectValue.Members[i].Name);
                    _ = builder.Append(": ");
                    JsonMember member = objectValue.Members[i];
                    // Map field rows take one line each, also in a parsed document without layout marks.
                    if (member.Name == "Fields" && member.Value is JsonArray { Items.Count: > 0 } rows && rows.Items.All(r => r is JsonArray))
                    {
                        WriteRows(builder, rows, depth: depth + 1);
                    }
                    else
                    {
                        Write(builder, member.Value, depth: depth + 1);
                    }

                    _ = builder.Append(i + 1 < objectValue.Members.Count ? ",\n" : "\n");
                }
                Indent(builder, depth);
                _ = builder.Append('}');
                return;
            case JsonArray array:
                if (array.Items.Count == 0)
                {
                    _ = builder.Append("[]");
                    return;
                }
                _ = builder.Append("[\n");
                for (int i = 0; i < array.Items.Count; i++)
                {
                    Indent(builder, depth: depth + 1);
                    if (array.Layout == ArrayLayout.RowsOnLines)
                    {
                        WriteCompact(builder, value: array.Items[i]);
                    }
                    else
                    {
                        Write(builder, value: array.Items[i], depth: depth + 1);
                    }

                    _ = builder.Append(i + 1 < array.Items.Count ? ",\n" : "\n");
                }
                Indent(builder, depth);
                _ = builder.Append(']');
                return;
            default:
                WriteScalar(builder, value);
                return;
        }
    }

    private static void WriteRows(StringBuilder builder, JsonArray rows, int depth)
    {
        _ = builder.Append("[\n");
        for (int i = 0; i < rows.Items.Count; i++)
        {
            Indent(builder, depth: depth + 1);
            WriteCompact(builder, value: rows.Items[i]);
            _ = builder.Append(i + 1 < rows.Items.Count ? ",\n" : "\n");
        }
        Indent(builder, depth);
        _ = builder.Append(']');
    }

    /// <summary>
    /// One line with no space at all and no final LF: the minimal JSON of a recording line or a
    /// snapshot (issue 404). Scalars and strings are written as the canonical writer writes them.
    /// </summary>
    public static string ToMinimalText(JsonValue value)
    {
        StringBuilder builder = new();
        WriteMinimal(builder, value);
        return builder.ToString();
    }

    /// <summary>Appends the minimal JSON of <paramref name="value"/>, see <see cref="ToMinimalText"/>.</summary>
    public static void WriteMinimal(StringBuilder builder, JsonValue value)
    {
        switch (value)
        {
            case JsonObject objectValue:
                _ = builder.Append('{');
                for (int i = 0; i < objectValue.Members.Count; i++)
                {
                    if (i > 0)
                    {
                        _ = builder.Append(',');
                    }

                    WriteString(builder, value: objectValue.Members[i].Name);
                    _ = builder.Append(':');
                    WriteMinimal(builder, value: objectValue.Members[i].Value);
                }
                _ = builder.Append('}');
                return;
            case JsonArray array:
                _ = builder.Append('[');
                for (int i = 0; i < array.Items.Count; i++)
                {
                    if (i > 0)
                    {
                        _ = builder.Append(',');
                    }

                    WriteMinimal(builder, value: array.Items[i]);
                }
                _ = builder.Append(']');
                return;
            default:
                WriteScalar(builder, value);
                return;
        }
    }

    /// <summary>One line, one space after colons and commas.</summary>
    public static void WriteCompact(StringBuilder builder, JsonValue value)
    {
        switch (value)
        {
            case JsonObject objectValue:
                _ = builder.Append('{');
                for (int i = 0; i < objectValue.Members.Count; i++)
                {
                    if (i > 0)
                    {
                        _ = builder.Append(", ");
                    }

                    WriteString(builder, value: objectValue.Members[i].Name);
                    _ = builder.Append(": ");
                    WriteCompact(builder, objectValue.Members[i].Value);
                }
                _ = builder.Append('}');
                return;
            case JsonArray array:
                _ = builder.Append('[');
                for (int i = 0; i < array.Items.Count; i++)
                {
                    if (i > 0)
                    {
                        _ = builder.Append(", ");
                    }

                    WriteCompact(builder, value: array.Items[i]);
                }
                _ = builder.Append(']');
                return;
            default:
                WriteScalar(builder, value);
                return;
        }
    }

    private static void WriteScalar(StringBuilder builder, JsonValue value)
    {
        switch (value)
        {
            case JsonString s:
                WriteString(builder, s.Value);
                break;
            case JsonInteger n:
                _ = builder.Append(n.Value.ToString(CultureInfo.InvariantCulture));
                break;
            case JsonNumber number:
                _ = builder.Append(number.Text);
                break;
            case JsonBoolean b:
                _ = builder.Append(b.Value ? "true" : "false");
                break;
            case JsonNull:
                _ = builder.Append("null");
                break;
            default:
                throw new InvalidOperationException($"Unknown JSON value {value.GetType().Name}");
        }
    }

    public static void WriteString(StringBuilder builder, string value)
    {
        _ = builder.Append('"');
        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];
            switch (c)
            {
                case '"': _ = builder.Append("\\\""); break;
                case '\\': _ = builder.Append("\\\\"); break;
                case '\b': _ = builder.Append("\\b"); break;
                case '\t': _ = builder.Append("\\t"); break;
                case '\n': _ = builder.Append("\\n"); break;
                case '\f': _ = builder.Append("\\f"); break;
                case '\r': _ = builder.Append("\\r"); break;
                default:
                    if (c is < ' ' or (>= '\u007f' and <= '\u009f'))
                    {
                        _ = builder.Append("\\u").Append(((int)c).ToString("x4", provider: CultureInfo.InvariantCulture));
                    }
                    else if (char.IsHighSurrogate(c) && i + 1 < value.Length && char.IsLowSurrogate(value[i + 1]))
                    {
                        _ = builder.Append(c).Append(value[i + 1]);
                        i++;
                    }
                    else
                    {
                        _ = char.IsSurrogate(c)
                            ? throw new ConversionException(new FormatDiagnostic(Codes.Surrogate, Message: "A string holds an unpaired surrogate."))
                            : builder.Append(c);
                    }

                    break;
            }
        }
        _ = builder.Append('"');
    }
}
