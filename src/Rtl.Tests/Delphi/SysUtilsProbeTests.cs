using System.Text;
using Xunit.Abstractions;
using static Aow.Rtl.RtlUnit;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class SysUtilsProbeTests
{
    internal static void StringsRemaining(ProbeLines probe, ITestOutputHelper output)
    {
        void Bytes(string label, string s) => probe.Fact(label, Convert.ToHexString(Ptr<byte>.Unbox(s).Array!.AsSpan(0, s.Length)));
        var all = Cp1252.Decode(Enumerable.Range(0, 256).Select(i => (byte)i).ToArray());
        Bytes("uppercase_all_bytes", UpperCase(all)); Bytes("lowercase_all_bytes", LowerCase(all));
        foreach (var (label, a, b) in new (string, string, string)[] { ("case", "abc", "ABC"), ("prefix", "ab", "ABC"), ("high", "\xe0", "\xc0"), ("empty", "", "a"), ("zero", "a\0b", "A\0c") })
            probe.Fact("comparetext_" + label, Math.Sign(CompareText(a, b)));
        foreach (var (label, text) in new (string, string)[] { ("space", "  abc  "), ("controls", "\0\t\n\rabc\x1f "), ("all", "\0\t "), ("empty", ""), ("high", "\xa0" + "abc\xa0"), ("inner", " a b ") }) Bytes("trim_" + label, Trim(text));
        foreach (var (label, text) in new (string, string)[] {
            ("zero","0"), ("negative","-123"), ("plus","+123"), ("max","2147483647"), ("min","-2147483648"), ("dollar_hex","$FF"), ("c_hex","0xFF"), ("negative_hex","-$10"), ("hex_all_bits","$FFFFFFFF"), ("leading_space"," 12"), ("trailing_space","12 "), ("leading_tab","\t12"), ("empty",""), ("spaces","  "), ("junk","12x"), ("overflow","2147483648"), ("underflow","-2147483649"), ("bare_sign","+"), ("bare_hex","$"), ("double_sign","--1"), ("decimal_point","1.0"), ("embedded_zero","12\0" + "3") })
        {
            object result;
            try { result = StrToInt(text); } catch (FormatException e) { result = e.GetType().Name; }
            probe.Fact("strtoint_" + label, result);
        }
        foreach (var (label, value) in new[] { ("zero", 0), ("min", int.MinValue), ("max", int.MaxValue), ("negative", -42) }) Bytes("inttostr_" + label, IntToStr(value));
        foreach (var (label, value, digits) in new[] { ("byte", 255, 2), ("pad", 255, 6), ("narrow", 256, 1), ("negative", -1, 2), ("zero_width", 0, 0) }) Bytes("inttohex_" + label, IntToHex(value, digits));
        foreach (var (label, text) in new (string, string)[] { ("empty", ""), ("single", "a"), ("crlf", "a\r\nb\r\n"), ("lf", "a\nb\n"), ("cr", "a\rb\r"), ("blanks", "\r\n\r\n"), ("mixed", "a\r\n\nb"), ("zero", "a\0b") })
        {
            var list = new TStringList { Text = text };
            probe.Fact("text_" + label + "_count", list.Count);
            for (int i = 0; i < list.Count; i++) Bytes($"text_{label}_item_{i}", list[i]);
            var roundtrip = list.Text; Bytes("text_" + label + "_text", roundtrip);
            list.Text = roundtrip; Bytes("text_" + label + "_text_again", list.Text);
        }
        var differences = new StringBuilder();
        var kinds = new Dictionary<string, int>();
        for (int a = 0; a < 256; a++) for (int b = 0; b < 256; b++)
        {
            int expected = int.Parse(probe.Current.Split(' ')[2], System.Globalization.CultureInfo.InvariantCulture);
            int actual = Math.Sign(AnsiCompareText(Cp1252.Text((byte)a), Cp1252.Text((byte)b)));
            Assert.Equal(expected == 0, actual == 0);
            if (actual != expected)
            {
                string kind = a >= 128 || b >= 128 ? "extended byte" : a < 32 || b < 32 || a == 127 || b == 127 ? "control" : "punctuation";
                kinds[kind] = kinds.GetValueOrDefault(kind) + 1;
                differences.AppendLine($"{a:X2} {b:X2} oracle={expected} shim={actual} {kind}");
            }
            probe.Fact($"{a:X2} {b:X2}", expected);
        }
        string report = differences.ToString().Replace("\r\n", "\n", StringComparison.Ordinal);
        output.WriteLine($"AnsiCompareText: {kinds.Values.Sum()} sign exceptions. {string.Join(", ", kinds.Select(k => $"{k.Key}={k.Value}"))}.");
        // A checked-in list fixes the precise allowed order exceptions, including both directions.
        string reportPath = Path.Combine(AppContext.BaseDirectory, "AnsiCompareText-order-exceptions.txt");
        Assert.Equal(File.ReadAllText(reportPath).Replace("\r\n", "\n", StringComparison.Ordinal), report);
        probe.End();
    }

    [ProbeFact("format")]
    public void FormatFixture()
    {
        var p = new ProbeLines("format");
        void F(string label, string format, params ReadOnlySpan<FormatArg> args) => p.Fact(label, Format(format, args));
        void B(string label, string format, params ReadOnlySpan<FormatArg> args) => p.Fact(label, "[" + Format(format, args) + "]");
        void Error(string label, string format, params ReadOnlySpan<FormatArg> args)
        {
            object result; try { result = Format(format, args); } catch (FormatException e) { result = e.GetType().Name; }
            p.Fact(label, result);
        }
        F("string", "%s", "abc"); F("decimal", "%d", -123); F("hex", "%x", 255); F("hex_negative", "%x", -1);
        B("string_width", "%6s", "abc"); B("decimal_width", "%6d", -12); B("hex_width", "%6x", 255);
        F("string_precision", "%.2s", "abcd"); F("decimal_precision", "%.5d", -12); F("hex_precision", "%.6x", 255);
        B("left_string", "%-6s", "abc"); B("left_decimal", "%-6.4d", 12); B("star_width", "%*d", 6, 12);
        F("star_precision", "%.*s", 2, "abcd"); B("star_both", "%*.*x", 8, 4, 255);
        F("explicit_index", "%1:s/%0:d/%1:s", 7, "abc"); F("index_then_implicit", "%1:d/%d/%0:d", 10, 20, 30);
        B("index_star", "%1:*.*s", 99, 6, 2, "abcd"); F("percent", "%% %d %%", 5);
        B("empty_string", "%3s", ""); B("zero_precision_string", "%.0s", "abc"); F("zero_precision_decimal", "%.0d", 0); F("extra_argument", "%d", 1, 2);
        Error("bad_type", "%d", "abc"); Error("missing_argument", "%s %d", "abc"); Error("bad_specifier", "%q", 1); Error("bad_index", "%2:d", 1); Error("bad_star_type", "%*d", "bad", 1); p.End();
    }
}
