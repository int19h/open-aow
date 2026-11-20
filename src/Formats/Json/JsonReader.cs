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
        _text = text;
        _file = file;
        _decompressed = decompressed;
        _limits = limits;
    }

    /// <summary>Decodes strict UTF-8 and parses it. The bytes are never retried as another format.</summary>
    public static JsonValue Parse(ReadOnlySpan<byte> utf8, string? file = null, bool decompressed = false, FormatLimits? limits = null)
    {
        UTF8Encoding encoding = new(false, throwOnInvalidBytes: true);
        string text;
        try
        {
            text = encoding.GetString(utf8);
        }
        catch (DecoderFallbackException error)
        {
            throw new ConversionException(new FormatDiagnostic(Codes.Utf8, Message: "The document is not valid UTF-8.")
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
        JsonReader reader = new(text, file, decompressed, limits: limits ?? FormatLimits.Default);
        if (reader.Peek() == '\uFEFF')
        {
            reader._position++;
        }

        reader.SkipWhitespace();
        JsonValue value = reader.ReadValue(0);
        reader.SkipWhitespace();
        return reader._position < text.Length ? throw reader.Error(Codes.Syntax, message: "Content follows the document value.") : value;
    }

    private TextSpan Here => new(_position, _line, Column: ColumnAt(_position));

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
        {
            if (!char.IsLowSurrogate(_text[i]))
            {
                _cacheColumn++;
            }
        }

        _cacheOffset = offset;
        return _cacheColumn;
    }

    private char Peek() => _position < _text.Length ? _text[_position] : '\0';

    private ConversionException Error(string code, string message, TextSpan? span = null, string? value = null, string? constraint = null)
        => new(new FormatDiagnostic(code, message)
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
            if (c is ' ' or '\t' or '\r')
            {
                _position++;
            }
            else if (c == '\n')
            {
                _position++;
                _line++;
                _lineStart = _position;
            }
            else
            {
                break;
            }
        }
    }

    private void CountNode(TextSpan span)
    {
        if (++_nodes > _limits.Nodes)
        {
            throw Error(Codes.Limit, message: "The document has too many values.", span, value: null, constraint: $"at most {_limits.Nodes} values");
        }
    }

    private JsonValue ReadValue(int depth)
    {
        if (_position >= _text.Length)
        {
            throw Error(Codes.Syntax, message: "The document ends where a value is expected.");
        }

        TextSpan span = Here;
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
                value = c is '-' or (>= '0' and <= '9') ? ReadNumber() : throw Error(Codes.Syntax, message: $"Unexpected character '{Printable(c)}'.");

                break;
        }
        value.Span = span;
        return value;
    }

    private static string Printable(char c) => c < ' ' ? $"\\u{(int)c:x4}" : c.ToString();

    private void Expect(string word)
    {
        if (string.CompareOrdinal(_text, _position, word, indexB: 0, word.Length) != 0)
        {
            throw Error(Codes.Syntax, message: $"Unexpected text, '{word}' or another value expected.");
        }

        _position += word.Length;
    }

    private void CheckDepth(int depth)
    {
        if (depth > _limits.Depth)
        {
            throw Error(Codes.Limit, message: "The document nests too deeply.", span: null, value: null, constraint: $"depth at most {_limits.Depth}");
        }
    }

    private JsonObject ReadObject(int depth)
    {
        CheckDepth(depth);
        JsonObject result = new();
        _position++;
        SkipWhitespace();
        if (Peek() == '}')
        {
            _position++;
            return result;
        }
        Dictionary<string, TextSpan> names = [with(StringComparer.Ordinal)];
        while (true)
        {
            SkipWhitespace();
            if (Peek() != '"')
            {
                throw Error(Codes.Syntax, message: Peek() == '}' ? "A trailing comma precedes '}'." : "A member name is expected.");
            }

            TextSpan nameSpan = Here;
            string name = ReadString();
            if (names.TryGetValue(name, out TextSpan first))
            {
                throw Error(Codes.Duplicate, message: $"Member \"{name}\" is declared twice, first at {first.Line}:{first.Column}.", span: nameSpan, value: name);
            }

            names.Add(name, nameSpan);
            SkipWhitespace();
            if (Peek() != ':')
            {
                throw Error(Codes.Syntax, message: "':' is expected after a member name.");
            }

            _position++;
            SkipWhitespace();
            JsonValue value = ReadValue(depth);
            result.Members.Add(new JsonMember(name, value) { NameSpan = nameSpan });
            if (result.Members.Count > _limits.CollectionItems)
            {
                throw Error(Codes.Limit, message: "An object has too many members.", span: nameSpan, value: null, constraint: $"at most {_limits.CollectionItems} members");
            }

            SkipWhitespace();
            char c = Peek();
            _position++;
            if (c == '}')
            {
                return result;
            }

            if (c != ',')
            {
                _position--;
                throw Error(Codes.Syntax, message: "',' or '}' is expected.");
            }
        }
    }

    private JsonArray ReadArray(int depth)
    {
        CheckDepth(depth);
        JsonArray result = new();
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
            if (Peek() == ']')
            {
                throw Error(Codes.Syntax, message: "A trailing comma precedes ']'.");
            }

            result.Items.Add(ReadValue(depth));
            if (result.Items.Count > _limits.CollectionItems)
            {
                throw Error(Codes.Limit, message: "An array has too many items.", span: null, value: null, constraint: $"at most {_limits.CollectionItems} items");
            }

            SkipWhitespace();
            char c = Peek();
            _position++;
            if (c == ']')
            {
                return result;
            }

            if (c != ',')
            {
                _position--;
                throw Error(Codes.Syntax, message: "',' or ']' is expected.");
            }
        }
    }

    private string ReadString()
    {
        TextSpan start = Here;
        _position++;
        StringBuilder builder = new();
        while (true)
        {
            if (_position >= _text.Length)
            {
                throw Error(Codes.Syntax, message: "The string is not closed.", span: start);
            }

            char c = _text[_position];
            if (c == '"')
            {
                _position++;
                break;
            }
            if (c < ' ')
            {
                throw Error(Codes.Syntax, message: "A control character in a string is not escaped.");
            }

            if (c == '\\')
            {
                _position++;
                char e = Peek();
                _position++;
                switch (e)
                {
                    case '"': _ = builder.Append('"'); break;
                    case '\\': _ = builder.Append('\\'); break;
                    case '/': _ = builder.Append('/'); break;
                    case 'b': _ = builder.Append('\b'); break;
                    case 'f': _ = builder.Append('\f'); break;
                    case 'n': _ = builder.Append('\n'); break;
                    case 'r': _ = builder.Append('\r'); break;
                    case 't': _ = builder.Append('\t'); break;
                    case 'u':
                        if (_position + 4 > _text.Length || !int.TryParse(_text.AsSpan(_position, length: 4), style: NumberStyles.AllowHexSpecifier, provider: CultureInfo.InvariantCulture, result: out int code))
                        {
                            _position -= 2;
                            throw Error(Codes.Syntax, message: "A \\u escape needs four hexadecimal digits.");
                        }
                        _position += 4;
                        _ = builder.Append((char)code);
                        break;
                    default:
                        _position -= 2;
                        throw Error(Codes.Syntax, message: "Unknown escape sequence.");
                }
            }
            else
            {
                _ = builder.Append(c);
                _position++;
            }
            if (builder.Length > _limits.StringBytes)
            {
                throw Error(Codes.Limit, message: "A string is too long.", span: start, value: null, constraint: $"at most {_limits.StringBytes} characters");
            }
        }
        string result = builder.ToString();
        for (int i = 0; i < result.Length; i++)
        {
            if (char.IsHighSurrogate(result[i]) && i + 1 < result.Length && char.IsLowSurrogate(result[i + 1]))
            {
                i++;
            }
            else if (char.IsSurrogate(result[i]))
            {
                throw Error(Codes.Surrogate, message: "A string holds an unpaired surrogate.", span: start);
            }
        }
        return result;
    }

    private JsonValue ReadNumber()
    {
        TextSpan start = Here;
        int begin = _position;
        if (Peek() == '-')
        {
            _position++;
        }

        if (Peek() == '0')
        {
            _position++;
        }
        else if (Peek() is >= '1' and <= '9')
        {
            while (Peek() is >= '0' and <= '9')
            {
                _position++;
            }
        }
        else
        {
            throw Error(Codes.Syntax, message: "A digit is expected.", span: start);
        }

        if (Peek() is '.' or 'e' or 'E')
        {
            // A fraction needs a digit after the point and an exponent a digit after its sign.
            if (Peek() == '.')
            {
                _position++;
                if (Peek() is not (>= '0' and <= '9'))
                {
                    throw Error(Codes.Number, message: "A digit is expected after the decimal point.", span: start, value: _text[begin.._position]);
                }

                while (Peek() is >= '0' and <= '9')
                {
                    _position++;
                }
            }
            if (Peek() is 'e' or 'E')
            {
                _position++;
                if (Peek() is '+' or '-')
                {
                    _position++;
                }

                if (Peek() is not (>= '0' and <= '9'))
                {
                    throw Error(Codes.Number, message: "A digit is expected in the exponent.", span: start, value: _text[begin.._position]);
                }

                while (Peek() is >= '0' and <= '9')
                {
                    _position++;
                }
            }
            return new JsonNumber(_text[begin.._position]);
        }
        ReadOnlySpan<char> digits = _text.AsSpan(begin, length: _position - begin);
        return !long.TryParse(digits, style: NumberStyles.AllowLeadingSign, provider: CultureInfo.InvariantCulture, result: out long value)
            ? throw Error(Codes.Range, message: "The integer is outside the 64-bit range.", span: start, value: digits.ToString())
            : (JsonValue)new JsonInteger(value);
    }
}
