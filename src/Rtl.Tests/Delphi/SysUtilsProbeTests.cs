using System.Globalization;
using System.Text;
using Xunit.Abstractions;
using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiTextCompare;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class SysUtilsProbeTests
{
    internal static void StringsRemaining(ProbeLines probe, ITestOutputHelper output)
    {
        void Bytes(string label, string s) => probe.Fact(label, value: Convert.ToHexString(Ptr<byte>.Unbox(s).Array.AsSpan(0, s.Length)));
        string all = Cp1252.Decode(Enumerable.Range(0, count: 256).Select(i => (byte)i).ToArray());
        Bytes("uppercase_all_bytes", s: UpperCase(all));
        Bytes("lowercase_all_bytes", s: LowerCase(all));
        foreach ((string? label, string? a, string? b) in new (string, string, string)[] { ("case", "abc", "ABC"), ("prefix", "ab", "ABC"), ("high", "\xe0", "\xc0"), ("empty", "", "a"), ("zero", "a\0b", "A\0c") })
        {
            probe.Fact($"comparetext_{label}", value: Math.Sign(CompareText(a, b)));
        }

        foreach ((string? label, string? text) in new (string, string)[] { ("space", "  abc  "), ("controls", "\0\t\n\rabc\x1f "), ("all", "\0\t "), ("empty", ""), ("high", "\xa0" + "abc\xa0"), ("inner", " a b ") })
        {
            Bytes($"trim_{label}", s: DelphiStrings.TrimLeft(DelphiStrings.TrimRight((string?)text)));
        }

        foreach ((string? label, string? text) in new (string, string)[] {
            ("zero","0"), ("negative","-123"), ("plus","+123"), ("max","2147483647"), ("min","-2147483648"), ("dollar_hex","$FF"), ("c_hex","0xFF"), ("negative_hex","-$10"), ("hex_all_bits","$FFFFFFFF"), ("leading_space"," 12"), ("trailing_space","12 "), ("leading_tab","\t12"), ("empty",""), ("spaces","  "), ("junk","12x"), ("overflow","2147483648"), ("underflow","-2147483649"), ("bare_sign","+"), ("bare_hex","$"), ("double_sign","--1"), ("decimal_point","1.0"), ("embedded_zero","12\0" + "3") })
        {
            object result;
            try
            {
                result = StrToInt(text);
            }
            catch (Exception e)
            {
                result = probe.DelphiName(e);
            }
            probe.Fact($"strtoint_{label}", value: result);
        }
        foreach ((string? label, int value) in new[] { ("zero", 0), ("min", int.MinValue), ("max", int.MaxValue), ("negative", -42) })
        {
            Bytes($"inttostr_{label}", s: value.ToString(CultureInfo.InvariantCulture));
        }

        foreach ((string? label, int value, int digits) in new[] { ("byte", 255, 2), ("pad", 255, 6), ("narrow", 256, 1), ("negative", -1, 2), ("zero_width", 0, 0) })
        {
            Bytes($"inttohex_{label}", s: IntToHex(value, digits));
        }

        foreach ((string? label, string? text) in new (string, string)[] { ("empty", ""), ("single", "a"), ("crlf", "a\r\nb\r\n"), ("lf", "a\nb\n"), ("cr", "a\rb\r"), ("blanks", "\r\n\r\n"), ("mixed", "a\r\n\nb"), ("zero", "a\0b") })
        {
            DelphiStringList list = new() { Text = text };
            probe.Fact($"text_{label}_count", value: list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                Bytes($"text_{label}_item_{i}", s: list[i]);
            }

            string roundtrip = list.Text;
            Bytes($"text_{label}_text", s: roundtrip);
            list.Text = roundtrip;
            Bytes($"text_{label}_text_again", s: list.Text);
        }
        StringBuilder differences = new();
        Dictionary<string, int> kinds = [];
        for (int a = 0; a < 256; a++)
        {
            for (int b = 0; b < 256; b++)
            {
                int expected = int.Parse(probe.Current.Split(' ')[2], provider: CultureInfo.InvariantCulture);
                int actual = Math.Sign(AnsiCompareText(Cp1252.Text((byte)a), Cp1252.Text((byte)b)));
                Assert.Equal(expected == 0, actual: actual == 0);
                if (actual != expected)
                {
                    string kind = a >= 128 || b >= 128 ? "extended byte" : a < 32 || b < 32 || a == 127 || b == 127 ? "control" : "punctuation";
                    kinds[kind] = kinds.GetValueOrDefault(kind) + 1;
                    _ = differences.AppendLine($"{a:X2} {b:X2} oracle={expected} shim={actual} {kind}");
                }
                probe.Fact($"{a:X2} {b:X2}", value: expected);
            }
        }

        string report = differences.ToString().Replace("\r\n", "\n", comparisonType: StringComparison.Ordinal);
        output.WriteLine($"AnsiCompareText: {kinds.Values.Sum()} sign exceptions. {string.Join(", ", values: kinds.Select(k => $"{k.Key}={k.Value}"))}.");
        Assert.Equal(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "AnsiCompareText-order-exceptions.txt")).Replace("\r\n", "\n", comparisonType: StringComparison.Ordinal), report);
        probe.End();
    }

    [ProbeFact("format")]
    public void FormatFixture()
    {
        ProbeLines p = new("format");
        void RecordFormat(string label, string format, params ReadOnlySpan<FormatArg> arguments) => p.Fact(label, value: Format(format, args: arguments));
        void RecordBracketedFormat(string label, string format, params ReadOnlySpan<FormatArg> arguments) => p.Fact(label, value: $"[{Format(format, args: arguments)}]");
        void Error(string label, string format, params ReadOnlySpan<FormatArg> arguments)
        {
            object result;
            try
            {
                result = Format(format, args: arguments);
            }
            catch (Exception e)
            {
                result = p.DelphiName(e);
            }
            p.Fact(label, value: result);
        }
        RecordFormat("string", format: "%s", "abc");
        RecordFormat("decimal", format: "%d", -123);
        RecordFormat("hex", format: "%x", 255);
        RecordFormat("hex_negative", format: "%x", -1);
        RecordBracketedFormat("string_width", format: "%6s", "abc");
        RecordBracketedFormat("decimal_width", format: "%6d", -12);
        RecordBracketedFormat("hex_width", format: "%6x", 255);
        RecordFormat("string_precision", format: "%.2s", "abcd");
        RecordFormat("decimal_precision", format: "%.5d", -12);
        RecordFormat("hex_precision", format: "%.6x", 255);
        RecordBracketedFormat("left_string", format: "%-6s", "abc");
        RecordBracketedFormat("left_decimal", format: "%-6.4d", 12);
        RecordBracketedFormat("star_width", format: "%*d", 6, 12);
        RecordFormat("star_precision", format: "%.*s", 2, "abcd");
        RecordBracketedFormat("star_both", format: "%*.*x", 8, 4, 255);
        RecordFormat("explicit_index", format: "%1:s/%0:d/%1:s", 7, "abc");
        RecordFormat("index_then_implicit", format: "%1:d/%d/%0:d", 10, 20, 30);
        RecordBracketedFormat("index_star", format: "%1:*.*s", 99, 6, 2, "abcd");
        RecordFormat("percent", format: "%% %d %%", 5);
        RecordBracketedFormat("empty_string", format: "%3s", "");
        RecordBracketedFormat("zero_precision_string", format: "%.0s", "abc");
        RecordFormat("zero_precision_decimal", format: "%.0d", 0);
        RecordFormat("extra_argument", format: "%d", 1, 2);
        Error("bad_type", format: "%d", "abc");
        Error("missing_argument", format: "%s %d", "abc");
        Error("bad_specifier", format: "%q", 1);
        Error("bad_index", format: "%2:d", 1);
        Error("bad_star_type", format: "%*d", "bad", 1);
        p.End();
    }
}
