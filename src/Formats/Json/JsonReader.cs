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
    private readonly string _text;
    private readonly string? _file;
    private readonly bool _decompressed;
    private readonly FormatLimits _limits;
    private int _position;
    private int _line = 1;
    private int _lineStart;
    private long _nodes;

    private JsonReader(string text, string? file, bool decompressed, FormatLimits limits)
    {
        this._text = text;
        this._file = file;
        this._decompressed = decompressed;
        this._limits = limits;
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
                File = file,
                Offset = error.Index,
                Decompressed = decompressed,
            });
        }
        return Parse(text, file, decompressed, limits);
    }

    public static JsonValue Parse(string text, string? file = null, bool decompressed = false, FormatLimits? limits = null)
    {
        var reader = new JsonReader(text, file, decompressed, limits ?? FormatLimits.Default);
        if (reader.Peek() == '\uFEFF') reader._position++;
        reader.SkipWhitespace();
        var value = reader.ReadValue(0);
        reader.SkipWhitespace();
        if (reader._position < text.Length)
            throw reader.Error(Codes.Syntax, "Content follows the document value.");
        return value;
    }

    private TextSpan Here => new(_position, _line, ColumnAt(_position));

    private int _cacheOffset;
    private int _cacheColumn = 1;

    // Positions only grow, so the column advances from the last computed one on the same line.
    private int ColumnAt(int offset)
    {
        if (_cacheOffset < _lineStart || _cacheOffset > offset)
        {
            _cacheOffset = _lineStart;
            _cacheColumn = 1;
        }
        for (int i = _cacheOffset; i < offset; i++)
            if (!char.IsLowSurrogate(_text[i])) _cacheColumn++;
        _cacheOffset = offset;
        return _cacheColumn;
    }

    private char Peek() => _position < _text.Length ? _text[_position] : '\0';

    private ConversionException Error(string code, string message, TextSpan? span = null, string? value = null, string? constraint = null) =>
        new(new FormatDiagnostic(code, message)
        {
            File = _file,
            Span = span ?? Here,
            Decompressed = _decompressed,
            Value = value,
            Constraint = constraint,
        });

    private void SkipWhitespace()
    {
        while (_position < _text.Length)
        {
            char c = _text[_position];
            if (c == ' ' || c == '\t' || c == '\r') _position++;
            else if (c == '\n')
            {
                _position++;
                _line++;
                _lineStart = _position;
            }
            else break;
        }
    }

    private void CountNode(TextSpan span)
    {
        if (++_nodes > _limits.Nodes)
            throw Error(Codes.Limit, "The document has too many values.", span, null, $"at most {_limits.Nodes} values");
    }

    private JsonValue ReadValue(int depth)
    {
        if (_position >= _text.Length) throw Error(Codes.Syntax, "The document ends where a value is expected.");
        var span = Here;
        CountNode(span);
        char c = _text[_position];
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
                value = new JsonBoolean(true);
                break;
            case 'f':
                Expect("false");
                value = new JsonBoolean(false);
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
        if (string.CompareOrdinal(_text, _position, word, 0, word.Length) != 0)
            throw Error(Codes.Syntax, $"Unexpected text, '{word}' or another value expected.");
        _position += word.Length;
    }

    private void CheckDepth(int depth)
    {
        if (depth > _limits.Depth)
            throw Error(Codes.Limit, "The document nests too deeply.", null, null, $"depth at most {_limits.Depth}");
    }

    private JsonObject ReadObject(int depth)
    {
        CheckDepth(depth);
        var result = new JsonObject();
        _position++;
        SkipWhitespace();
        if (Peek() == '}')
        {
            _position++;
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
            _position++;
            SkipWhitespace();
            var value = ReadValue(depth);
            result.Members.Add(new JsonMember(name, value) { NameSpan = nameSpan });
            if (result.Members.Count > _limits.CollectionItems)
                throw Error(Codes.Limit, "An object has too many members.", nameSpan, null, $"at most {_limits.CollectionItems} members");
            SkipWhitespace();
            char c = Peek();
            _position++;
            if (c == '}') return result;
            if (c != ',')
            {
                _position--;
                throw Error(Codes.Syntax, "',' or '}' is expected.");
            }
        }
    }

    private JsonArray ReadArray(int depth)
    {
        CheckDepth(depth);
        var result = new JsonArray();
        _position++;
        SkipWhitespace();
        if (Peek() == ']')
        {
            _position++;
            return result;
        }
        while (true)
        {
            SkipWhitespace();
            if (Peek() == ']') throw Error(Codes.Syntax, "A trailing comma precedes ']'.");
            result.Items.Add(ReadValue(depth));
            if (result.Items.Count > _limits.CollectionItems)
                throw Error(Codes.Limit, "An array has too many items.", null, null, $"at most {_limits.CollectionItems} items");
            SkipWhitespace();
            char c = Peek();
            _position++;
            if (c == ']') return result;
            if (c != ',')
            {
                _position--;
                throw Error(Codes.Syntax, "',' or ']' is expected.");
            }
        }
    }

    private string ReadString()
    {
        var start = Here;
        _position++;
        var builder = new StringBuilder();
        while (true)
        {
            if (_position >= _text.Length) throw Error(Codes.Syntax, "The string is not closed.", start);
            char c = _text[_position];
            if (c == '"')
            {
                _position++;
                break;
            }
            if (c < ' ') throw Error(Codes.Syntax, "A control character in a string is not escaped.");
            if (c == '\\')
            {
                _position++;
                char e = Peek();
                _position++;
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
                        if (_position + 4 > _text.Length || !int.TryParse(_text.AsSpan(_position, 4), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out int code))
                        {
                            _position -= 2;
                            throw Error(Codes.Syntax, "A \\u escape needs four hexadecimal digits.");
                        }
                        _position += 4;
                        builder.Append((char)code);
                        break;
                    default:
                        _position -= 2;
                        throw Error(Codes.Syntax, "Unknown escape sequence.");
                }
            }
            else
            {
                builder.Append(c);
                _position++;
            }
            if (builder.Length > _limits.StringBytes)
                throw Error(Codes.Limit, "A string is too long.", start, null, $"at most {_limits.StringBytes} characters");
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
        int begin = _position;
        if (Peek() == '-') _position++;
        if (Peek() == '0') _position++;
        else if (Peek() is >= '1' and <= '9')
            while (Peek() is >= '0' and <= '9') _position++;
        else throw Error(Codes.Syntax, "A digit is expected.", start);
        if (Peek() is '.' or 'e' or 'E')
        {
            int end = _position + 1;
            while (end < _text.Length && (char.IsAsciiDigit(_text[end]) || _text[end] is '+' or '-' or 'e' or 'E' or '.')) end++;
            throw Error(Codes.Number, "Documents hold only integers. Floating-point values use { \"$bits\": ... }.",
                start, _text[begin..end]);
        }
        var digits = _text.AsSpan(begin, _position - begin);
        if (!long.TryParse(digits, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out long value))
            throw Error(Codes.Range, "The integer is outside the 64-bit range.", start, digits.ToString());
        return new JsonInteger(value);
    }
}
