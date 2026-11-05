using System.Globalization;
using System.Text;

namespace Aow.Formats.Json;

/// <summary>
/// The strict reader of own documents (own-formats.md 2.9). It accepts one leading byte order
/// mark and JSON whitespace around exactly one value. Comments, trailing commas, duplicate
/// members, non-integer numbers, unpaired surrogates and trailing content fail with a located
/// diagnostic. Every value keeps the span of its first character.
/// </summary>
public sealed class JsonReader
{
    private readonly string text;
    private readonly string? file;
    private readonly bool decompressed;
    private readonly FormatLimits limits;
    private int position;
    private int line = 1;
    private int lineStart;
    private long nodes;

    private JsonReader(string text, string? file, bool decompressed, FormatLimits limits)
    {
        this.text = text;
        this.file = file;
        this.decompressed = decompressed;
        this.limits = limits;
    }

    /// <summary>Decodes strict UTF-8 and parses it. The bytes are never retried as another format.</summary>
    public static JsonValue Parse(ReadOnlySpan<byte> utf8, string? file = null, bool decompressed = false, FormatLimits? limits = null)
    {
        var encoding = new UTF8Encoding(false, true);
        string text;
        try
        {
            text = encoding.GetString(utf8);
        }
        catch (DecoderFallbackException error)
        {
            throw new ConversionException(new FormatDiagnostic(Codes.Utf8, "The document is not valid UTF-8.")
            {
                File = file, Offset = error.Index, Decompressed = decompressed,
            });
        }
        return Parse(text, file, decompressed, limits);
    }

    public static JsonValue Parse(string text, string? file = null, bool decompressed = false, FormatLimits? limits = null)
    {
        var reader = new JsonReader(text, file, decompressed, limits ?? FormatLimits.Default);
        if (reader.Peek() == '\uFEFF') reader.position++;
        reader.SkipWhitespace();
        var value = reader.ReadValue(0);
        reader.SkipWhitespace();
        if (reader.position < text.Length)
            throw reader.Error(Codes.Syntax, "Content follows the document value.");
        return value;
    }

    private TextSpan Here => new(position, line, ColumnAt(position));

    private int cacheOffset;
    private int cacheColumn = 1;

    // Positions only grow, so the column advances from the last computed one on the same line.
    private int ColumnAt(int offset)
    {
        if (cacheOffset < lineStart || cacheOffset > offset)
        {
            cacheOffset = lineStart;
            cacheColumn = 1;
        }
        for (int i = cacheOffset; i < offset; i++)
            if (!char.IsLowSurrogate(text[i])) cacheColumn++;
        cacheOffset = offset;
        return cacheColumn;
    }

    private char Peek() => position < text.Length ? text[position] : '\0';

    private ConversionException Error(string code, string message, TextSpan? span = null, string? value = null, string? constraint = null) =>
        new(new FormatDiagnostic(code, message)
        {
            File = file, Span = span ?? Here, Decompressed = decompressed, Value = value, Constraint = constraint,
        });

    private void SkipWhitespace()
    {
        while (position < text.Length)
        {
            char c = text[position];
            if (c == ' ' || c == '\t' || c == '\r') position++;
            else if (c == '\n')
            {
                position++;
                line++;
                lineStart = position;
            }
            else break;
        }
    }

    private void CountNode(TextSpan span)
    {
        if (++nodes > limits.Nodes)
            throw Error(Codes.Limit, "The document has too many values.", span, null, $"at most {limits.Nodes} values");
    }

    private JsonValue ReadValue(int depth)
    {
        if (position >= text.Length) throw Error(Codes.Syntax, "The document ends where a value is expected.");
        var span = Here;
        CountNode(span);
        char c = text[position];
        JsonValue value;
        switch (c)
        {
            case '{':
                value = ReadObject(depth + 1);
                break;
            case '[':
                value = ReadArray(depth + 1);
                break;
            case '"':
                value = new JsonString(ReadString());
                break;
            case 't':
                Expect("true");
                value = new JsonBool(true);
                break;
            case 'f':
                Expect("false");
                value = new JsonBool(false);
                break;
            case 'n':
                Expect("null");
                value = new JsonNull();
                break;
            default:
                if (c == '-' || c is >= '0' and <= '9') value = ReadNumber();
                else throw Error(Codes.Syntax, $"Unexpected character '{Printable(c)}'.");
                break;
        }
        value.Span = span;
        return value;
    }

    private static string Printable(char c) => c < ' ' ? $"\\u{(int)c:x4}" : c.ToString();

    private void Expect(string word)
    {
        if (string.CompareOrdinal(text, position, word, 0, word.Length) != 0)
            throw Error(Codes.Syntax, $"Unexpected text, '{word}' or another value expected.");
        position += word.Length;
    }

    private void CheckDepth(int depth)
    {
        if (depth > limits.Depth)
            throw Error(Codes.Limit, "The document nests too deeply.", null, null, $"depth at most {limits.Depth}");
    }

    private JsonObject ReadObject(int depth)
    {
        CheckDepth(depth);
        var result = new JsonObject();
        position++;
        SkipWhitespace();
        if (Peek() == '}')
        {
            position++;
            return result;
        }
        var names = new Dictionary<string, TextSpan>(StringComparer.Ordinal);
        while (true)
        {
            SkipWhitespace();
            if (Peek() != '"') throw Error(Codes.Syntax, Peek() == '}' ? "A trailing comma precedes '}'." : "A member name is expected.");
            var nameSpan = Here;
            string name = ReadString();
            if (names.TryGetValue(name, out var first))
                throw Error(Codes.Duplicate, $"Member \"{name}\" is declared twice, first at {first.Line}:{first.Column}.", nameSpan, name);
            names.Add(name, nameSpan);
            SkipWhitespace();
            if (Peek() != ':') throw Error(Codes.Syntax, "':' is expected after a member name.");
            position++;
            SkipWhitespace();
            var value = ReadValue(depth);
            result.Members.Add(new JsonMember(name, value) { NameSpan = nameSpan });
            if (result.Members.Count > limits.CollectionItems)
                throw Error(Codes.Limit, "An object has too many members.", nameSpan, null, $"at most {limits.CollectionItems} members");
            SkipWhitespace();
            char c = Peek();
            position++;
            if (c == '}') return result;
            if (c != ',')
            {
                position--;
                throw Error(Codes.Syntax, "',' or '}' is expected.");
            }
        }
    }

    private JsonArray ReadArray(int depth)
    {
        CheckDepth(depth);
        var result = new JsonArray();
        position++;
        SkipWhitespace();
        if (Peek() == ']')
        {
            position++;
            return result;
        }
        while (true)
        {
            SkipWhitespace();
            if (Peek() == ']') throw Error(Codes.Syntax, "A trailing comma precedes ']'.");
            result.Items.Add(ReadValue(depth));
            if (result.Items.Count > limits.CollectionItems)
                throw Error(Codes.Limit, "An array has too many items.", null, null, $"at most {limits.CollectionItems} items");
            SkipWhitespace();
            char c = Peek();
            position++;
            if (c == ']') return result;
            if (c != ',')
            {
                position--;
                throw Error(Codes.Syntax, "',' or ']' is expected.");
            }
        }
    }

    private string ReadString()
    {
        var start = Here;
        position++;
        var builder = new StringBuilder();
        while (true)
        {
            if (position >= text.Length) throw Error(Codes.Syntax, "The string is not closed.", start);
            char c = text[position];
            if (c == '"')
            {
                position++;
                break;
            }
            if (c < ' ') throw Error(Codes.Syntax, "A control character in a string is not escaped.");
            if (c == '\\')
            {
                position++;
                char e = Peek();
                position++;
                switch (e)
                {
                    case '"': builder.Append('"'); break;
                    case '\\': builder.Append('\\'); break;
                    case '/': builder.Append('/'); break;
                    case 'b': builder.Append('\b'); break;
                    case 'f': builder.Append('\f'); break;
                    case 'n': builder.Append('\n'); break;
                    case 'r': builder.Append('\r'); break;
                    case 't': builder.Append('\t'); break;
                    case 'u':
                        if (position + 4 > text.Length || !int.TryParse(text.AsSpan(position, 4), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out int code))
                        {
                            position -= 2;
                            throw Error(Codes.Syntax, "A \\u escape needs four hexadecimal digits.");
                        }
                        position += 4;
                        builder.Append((char)code);
                        break;
                    default:
                        position -= 2;
                        throw Error(Codes.Syntax, "Unknown escape sequence.");
                }
            }
            else
            {
                builder.Append(c);
                position++;
            }
            if (builder.Length > limits.StringBytes)
                throw Error(Codes.Limit, "A string is too long.", start, null, $"at most {limits.StringBytes} characters");
        }
        string result = builder.ToString();
        for (int i = 0; i < result.Length; i++)
        {
            if (char.IsHighSurrogate(result[i]) && i + 1 < result.Length && char.IsLowSurrogate(result[i + 1])) i++;
            else if (char.IsSurrogate(result[i])) throw Error(Codes.Surrogate, "A string holds an unpaired surrogate.", start);
        }
        return result;
    }

    private JsonValue ReadNumber()
    {
        var start = Here;
        int begin = position;
        if (Peek() == '-') position++;
        if (Peek() == '0') position++;
        else if (Peek() is >= '1' and <= '9')
            while (Peek() is >= '0' and <= '9') position++;
        else throw Error(Codes.Syntax, "A digit is expected.", start);
        if (Peek() is '.' or 'e' or 'E')
        {
            int end = position + 1;
            while (end < text.Length && (char.IsAsciiDigit(text[end]) || text[end] is '+' or '-' or 'e' or 'E' or '.')) end++;
            throw Error(Codes.Number, "Documents hold only integers. Floating-point values use { \"$bits\": ... }.",
                start, text[begin..end]);
        }
        var digits = text.AsSpan(begin, position - begin);
        if (!long.TryParse(digits, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out long value))
            throw Error(Codes.Range, "The integer is outside the 64-bit range.", start, digits.ToString());
        return new JsonInteger(value);
    }
}
