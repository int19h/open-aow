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
    public static byte[] ToUtf8(JsonValue value) => new UTF8Encoding(false, true).GetBytes(ToText(value));

    public static string ToText(JsonValue value)
    {
        var builder = new StringBuilder();
        Write(builder, value, 0);
        builder.Append('\n');
        return builder.ToString();
    }

    private static void Indent(StringBuilder builder, int depth) => builder.Append(' ', depth * 2);

    private static void Write(StringBuilder builder, JsonValue value, int depth)
    {
        switch (value)
        {
            case JsonObject obj:
                if (obj.Members.Count == 0)
                {
                    builder.Append("{}");
                    return;
                }
                builder.Append("{\n");
                for (int i = 0; i < obj.Members.Count; i++)
                {
                    Indent(builder, depth + 1);
                    WriteString(builder, obj.Members[i].Name);
                    builder.Append(": ");
                    var member = obj.Members[i];
                    // Map field rows take one line each, also in a parsed document without layout marks.
                    if (member.Name == "Fields" && member.Value is JsonArray { Items.Count: > 0 } rows && rows.Items.All(r => r is JsonArray))
                        WriteRows(builder, rows, depth + 1);
                    else Write(builder, member.Value, depth + 1);
                    builder.Append(i + 1 < obj.Members.Count ? ",\n" : "\n");
                }
                Indent(builder, depth);
                builder.Append('}');
                return;
            case JsonArray array:
                if (array.Items.Count == 0)
                {
                    builder.Append("[]");
                    return;
                }
                builder.Append("[\n");
                for (int i = 0; i < array.Items.Count; i++)
                {
                    Indent(builder, depth + 1);
                    if (array.Layout == ArrayLayout.RowsOnLines) WriteCompact(builder, array.Items[i]);
                    else Write(builder, array.Items[i], depth + 1);
                    builder.Append(i + 1 < array.Items.Count ? ",\n" : "\n");
                }
                Indent(builder, depth);
                builder.Append(']');
                return;
            default:
                WriteScalar(builder, value);
                return;
        }
    }

    private static void WriteRows(StringBuilder builder, JsonArray rows, int depth)
    {
        builder.Append("[\n");
        for (int i = 0; i < rows.Items.Count; i++)
        {
            Indent(builder, depth + 1);
            WriteCompact(builder, rows.Items[i]);
            builder.Append(i + 1 < rows.Items.Count ? ",\n" : "\n");
        }
        Indent(builder, depth);
        builder.Append(']');
    }

    /// <summary>One line, one space after colons and commas.</summary>
    public static void WriteCompact(StringBuilder builder, JsonValue value)
    {
        switch (value)
        {
            case JsonObject obj:
                builder.Append('{');
                for (int i = 0; i < obj.Members.Count; i++)
                {
                    if (i > 0) builder.Append(", ");
                    WriteString(builder, obj.Members[i].Name);
                    builder.Append(": ");
                    WriteCompact(builder, obj.Members[i].Value);
                }
                builder.Append('}');
                return;
            case JsonArray array:
                builder.Append('[');
                for (int i = 0; i < array.Items.Count; i++)
                {
                    if (i > 0) builder.Append(", ");
                    WriteCompact(builder, array.Items[i]);
                }
                builder.Append(']');
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
                builder.Append(n.Value.ToString(CultureInfo.InvariantCulture));
                break;
            case JsonBoolean b:
                builder.Append(b.Value ? "true" : "false");
                break;
            case JsonNull:
                builder.Append("null");
                break;
            default:
                throw new InvalidOperationException("Unknown JSON value " + value.GetType().Name);
        }
    }

    public static void WriteString(StringBuilder builder, string value)
    {
        builder.Append('"');
        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];
            switch (c)
            {
                case '"': builder.Append("\\\""); break;
                case '\\': builder.Append("\\\\"); break;
                case '\b': builder.Append("\\b"); break;
                case '\t': builder.Append("\\t"); break;
                case '\n': builder.Append("\\n"); break;
                case '\f': builder.Append("\\f"); break;
                case '\r': builder.Append("\\r"); break;
                default:
                    if (c < ' ' || c is >= '\u007f' and <= '\u009f')
                        builder.Append("\\u").Append(((int)c).ToString("x4", CultureInfo.InvariantCulture));
                    else if (char.IsHighSurrogate(c) && i + 1 < value.Length && char.IsLowSurrogate(value[i + 1]))
                    {
                        builder.Append(c).Append(value[i + 1]);
                        i++;
                    }
                    else if (char.IsSurrogate(c))
                        throw new ConversionException(new FormatDiagnostic(Codes.Surrogate, "A string holds an unpaired surrogate."));
                    else builder.Append(c);
                    break;
            }
        }
        builder.Append('"');
    }
}
