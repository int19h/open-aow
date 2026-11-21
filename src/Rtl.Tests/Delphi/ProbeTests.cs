using System.Globalization;
using Xunit.Abstractions;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;
using static Aow.Rtl.DelphiStrings;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class ProbeFactAttribute(string name) : FactAttribute
{
    public string Recording { get; } = name;
}

internal sealed class ProbeLines(string name)
{
    private readonly string[] _lines = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Probes", $"{name}.txt"));
    private int _position;

    internal string Current => _lines[_position];

    internal void Fact(string label, object value)
    {
        string actual = $"{label} {Convert.ToString(value, provider: CultureInfo.InvariantCulture)}";
        Assert.True(_position < _lines.Length, userMessage: $"{name}: extra line {actual}");
        Assert.True(_lines[_position] == actual,
            userMessage: $"{name}.txt:{_position + 1} [{label}] expected <{_lines[_position]}> actual <{actual}>");
        _position++;
    }

    internal void End()
    {
        Fact("end", value: _position);
        Assert.Equal(_lines.Length, _position);
    }

    // The recording names the Delphi class the original raised. The probe selects which class a
    // .NET type stands for. A type with no row fails the test.
    private static readonly Dictionary<string, Dictionary<Type, string>> s_delphiNames = new()
    {
        ["format"] = new()
        {
            [typeof(FormatException)] = "EConvertError",
        },
        ["strings"] = new()
        {
            [typeof(FormatException)] = "EConvertError",
            [typeof(ArgumentOutOfRangeException)] = "ERangeError",
        },
        ["streams"] = new()
        {
            [typeof(EndOfStreamException)] = "EReadError",
            [typeof(IOException)] = "EWriteError",
        },
        ["lists"] = new()
        {
            [typeof(ArgumentOutOfRangeException)] = "EListError",
            [typeof(InvalidOperationException)] = "EStringListError",
            [typeof(ArgumentException)] = "EStringListError",
        },
    };

    internal string DelphiName(Exception error)
        => s_delphiNames.TryGetValue(name, out Dictionary<Type, string>? table)
        && table.TryGetValue(error.GetType(), out string? delphi)
            ? delphi
            : throw new Xunit.Sdk.XunitException($"{name} has no Delphi name for {error.GetType().Name}");

    internal string Thrown(Action action)
    {
        try
        {
            action();
        }
        catch (Exception error)
        {
            return DelphiName(error);
        }

        throw new Xunit.Sdk.XunitException($"{name} returned where the recording names an exception");
    }
}

public sealed class ProbeTests(ITestOutputHelper output)
{
    [ProbeFact("random")]
    public void RandomFixture()
    {
        ProbeLines probe = new("random");
        foreach (int bound in new[] { 2, 6, 10, 100, 1000, 65536, int.MaxValue, 0, 1 })
        {
            RandSeed = 123456789;
            probe.Fact($"initial_seed_n_{bound}", value: RandSeed);
            for (int i = 1; i <= 1000; i++)
            {
                int value = Random(bound);
                probe.Fact($"n_{bound}_call_{i}_seed", value: RandSeed);
                probe.Fact($"n_{bound}_call_{i}_value", value);
            }
        }
        RandSeed = 123456789;
        probe.Fact("initial_seed_float", value: RandSeed);
        for (int i = 1; i <= 1000; i++)
        {
            double value = Random();
            probe.Fact($"float_call_{i}_seed", value: RandSeed);
            probe.Fact($"float_call_{i}_scale_1048576", value: Trunc(value * 1048576));
        }
        RandSeed = 0;
        int next = Random(10);
        probe.Fact("seed_zero_next_seed", value: RandSeed);
        probe.Fact("seed_zero_value", value: next);
        RandSeed = -1;
        next = Random(10);
        probe.Fact("seed_neg1_next_seed", value: RandSeed);
        probe.Fact("seed_neg1_value", value: next);
        probe.End();
    }

    [ProbeFact("floats")]
    public void FloatsFixture()
    {
        ProbeLines probe = new("floats");
        for (int i = -8; i <= 8; i++)
        {
            double x = i / 2.0;
            probe.Fact($"round_half_index_{i}", value: Round(x));
            probe.Fact($"trunc_half_index_{i}", value: Trunc(x));
            probe.Fact($"int_half_index_{i}_scale_1048576", value: Trunc(Math.Truncate(x) * 1048576));
            probe.Fact($"frac_half_index_{i}_scale_1048576", value: Trunc((double.IsInfinity(x)
        ? throw new ArithmeticException("Invalid floating point operation") : x - Math.Truncate(x)) * 1048576));
        }
        probe.Fact("round_below_pos_half", value: Round(2.5 - (1.0 / 1048576)));
        probe.Fact("round_above_pos_half", value: Round(2.5 + (1.0 / 1048576)));
        probe.Fact("round_below_neg_half", value: Round(-2.5 - (1.0 / 1048576)));
        probe.Fact("round_above_neg_half", value: Round(-2.5 + (1.0 / 1048576)));
        for (int i = -3; i <= 3; i++)
        {
            double x = i / 4.0;
            probe.Fact($"quarter_trunc_{i}", value: Trunc(x));
            probe.Fact($"quarter_int_{i}_scale_1048576", value: Trunc(Math.Truncate(x) * 1048576));
            probe.Fact($"quarter_frac_{i}_scale_1048576", value: Trunc((double.IsInfinity(x)
        ? throw new ArithmeticException("Invalid floating point operation") : x - Math.Truncate(x)) * 1048576));
        }
        probe.Fact("integer_divide_7_2_scale_1048576", value: Trunc(7.0 / 2 * 1048576));
        probe.Fact("integer_divide_neg7_2_scale_1048576", value: Trunc(-7.0 / 2 * 1048576));
        probe.Fact("integer_divide_1_3_scale_1048576", value: Trunc(1.0 / 3 * 1048576));
        probe.Fact("trunc_large_pos", value: Trunc(123456.75));
        probe.Fact("trunc_large_neg", value: Trunc(-123456.75));
        probe.End();
    }

    [ProbeFact("strings")]
    public void StringsFixture()
    {
        ProbeLines probe = new("strings");
        void Bytes(string label, string value) => probe.Fact(label, value: Convert.ToHexString(Ptr<byte>.Unbox(value).Array.AsSpan(0, value.Length)));
        string s = "abcd";
        foreach ((string? label, int index, int count) in new[] {
            ("start",1,2), ("tail",3,99), ("zero_index",0,2), ("negative_index",-2,2),
            ("past_end",5,1), ("zero_count",1,0), ("negative_count",1,-1) })
        {
            Bytes($"copy_{label}", value: Copy(s, index, count));
        }

        Bytes("copy_empty", value: Copy("", index: 1, count: 4));
        foreach ((string? label, int index, int count) in new[] {
            ("start",1,2), ("tail",3,99), ("zero_index",0,2), ("negative_index",-1,2),
            ("past_end",5,2), ("zero_count",2,0), ("negative_count",2,-1) })
        {
            string t = s;
            Delete(ref t, index, count);
            Bytes($"delete_{label}", value: t);
        }
        foreach ((string? label, int index) in new[] {
            ("start",1), ("middle",3), ("end",5), ("past_end",99), ("zero_index",0), ("negative_index",-1) })
        {
            string t = s;
            Insert("XY", value: ref t, index);
            Bytes($"insert_{label}", value: t);
        }
        Bytes("insert_empty_source", value: Insert("", value: s, index: 2));
        Bytes("insert_empty_target", value: Insert("XY", value: "", index: 1));
        string self = s;
        Insert(self, value: ref self, index: 3);
        Bytes("insert_self", value: self);
        probe.Fact("pos_start", value: Pos("ab", haystack: s));
        probe.Fact("pos_end", value: Pos("cd", haystack: s));
        probe.Fact("pos_absent", value: Pos("x", haystack: s));
        probe.Fact("pos_empty_needle", value: Pos("", haystack: s));
        probe.Fact("pos_empty_haystack", value: Pos("a", haystack: ""));
        probe.Fact("pos_both_empty", value: Pos("", haystack: ""));
        probe.Fact("pos_first_repeat", value: Pos("aa", haystack: "aaaa"));
        probe.Fact("pos_long_needle", value: Pos("abcde", haystack: s));
        probe.Fact("pos_embedded_zero", value: Pos("\0b", haystack: "a\0b"));
        probe.Fact("prefix_lt", value: Cp1252.Compare("ab", "abc") < 0 ? 1 : 0);
        probe.Fact("prefix_gt", value: Cp1252.Compare("abc", "ab") > 0 ? 1 : 0);
        probe.Fact("empty_lt", value: Cp1252.Compare("", "a") < 0 ? 1 : 0);
        probe.Fact("prefix_le", value: Cp1252.Compare("ab", "abc") <= 0 ? 1 : 0);
        probe.Fact("prefix_ge", value: Cp1252.Compare("abc", "ab") >= 0 ? 1 : 0);
        probe.Fact("equal", value: s == "abcd" ? 1 : 0);
        probe.Fact("unequal", value: s != "abc" ? 1 : 0);
        probe.Fact("high_byte_lt", value: Cp1252.Compare("\u20AC", "\u00FF") < 0 ? 1 : 0);
        probe.Fact("high_vs_ascii_gt", value: Cp1252.Compare("\u20AC", "z") > 0 ? 1 : 0);
        probe.Fact("embedded_zero_compare", value: Cp1252.Compare("a\0b", "a\0c") < 0 ? 1 : 0);
        s = "abc";
        string alias = s;
        s = WithChar(s, index: 2, character: 'X');
        Bytes("alias_mutated", value: s);
        Bytes("alias_original", value: alias);
        alias = WithChar(alias, index: 1, character: 'Z');
        Bytes("alias_second_write", value: alias);
        Bytes("alias_first_preserved", value: s);
        s = "a\0\u20AC\u00FFz";
        Bytes("binary_string", value: s);
        probe.Fact("binary_length", value: RequireString((string?)s).Length);
        alias = s;
        SetLength(ref alias, length: 3);
        Bytes("setlength_shrink", value: alias);
        Bytes("setlength_alias", value: s);
        SetLength(ref alias, length: 0);
        Bytes("setlength_empty", value: alias);

        SysUtilsProbeTests.StringsRemaining(probe, output);
    }

    [ProbeFact("integers")]
    public void IntegersFixture()
    {
        ProbeLines probe = new("integers");
        int i = int.MaxValue, j = 1;
        probe.Fact("add_max_1", value: unchecked(i + j));
        i = int.MinValue;
        probe.Fact("sub_min_1", value: unchecked(i - j));
        i = 65536;
        probe.Fact("mul_65536_65536", value: unchecked(i * i));
        i = int.MaxValue;
        probe.Fact("mul_max_2", value: unchecked(i * 2));
        i = int.MinValue;
        probe.Fact("neg_min", value: unchecked(-i));
        i = int.MaxValue;
        probe.Fact("inc_max", value: unchecked(++i));
        probe.Fact("dec_min", value: unchecked(--i));
        byte b = 255;
        probe.Fact("byte_inc_max", value: unchecked(++b));
        b = 255;
        probe.Fact("byte_plus_max", value: unchecked((byte)(b + 1)));
        b = 0;
        probe.Fact("byte_dec_min", value: unchecked(--b));
        ushort w = 65535;
        probe.Fact("word_inc_max", value: unchecked(++w));
        w = 65535;
        probe.Fact("word_plus_max", value: unchecked((ushort)(w + 1)));
        w = 0;
        probe.Fact("word_dec_min", value: unchecked(--w));
        sbyte s = 127;
        probe.Fact("shortint_inc_max", value: unchecked(++s));
        s = 127;
        probe.Fact("shortint_plus_max", value: unchecked((sbyte)(s + 1)));
        s = -128;
        probe.Fact("shortint_dec_min", value: unchecked(--s));
        short t = 32767;
        probe.Fact("smallint_inc_max", value: unchecked(++t));
        t = 32767;
        probe.Fact("smallint_plus_max", value: unchecked((short)(t + 1)));
        t = -32768;
        probe.Fact("smallint_dec_min", value: unchecked(--t));
        b = 255;
        w = 65535;
        probe.Fact("byte_promote_add", value: b + b);
        probe.Fact("word_promote_add", value: w + w);
        probe.Fact("byte_promote_sub", value: 0 - b);
        probe.Fact("word_promote_mul", value: unchecked(w * w));
        for (int k = 0; k < 4; k++)
        {
            i = (k & 1) != 0 ? -7 : 7;
            j = (k & 2) != 0 ? -2 : 2;
            probe.Fact($"div_{i}_{j}", value: i / j);
            probe.Fact($"mod_{i}_{j}", value: i % j);
            i -= i % j;
            probe.Fact($"exact_div_sign_{k}", value: i / j);
            probe.Fact($"exact_mod_sign_{k}", value: i % j);
        }
        i = 0;
        j = -2;
        probe.Fact("div_zero_neg2", value: i / j);
        probe.Fact("mod_zero_neg2", value: i % j);
        i = -1;
        for (int n = 0; n <= 33; n++)
        {
            probe.Fact($"shr_neg1_{n}", value: i >>> n);
            probe.Fact($"shl_neg1_{n}", value: i << n);
        }
        s = -1;
        t = -1;
        probe.Fact("shortint_shr_neg1_1", value: s >>> 1);
        probe.Fact("smallint_shr_neg1_1", value: t >>> 1);
        probe.Fact("byte_shl_8", value: b << 8);
        probe.Fact("word_shl_16", value: w << 16);
        i = int.MinValue;
        probe.Fact("shr_min_1", value: i >>> 1);
        i = -7;
        probe.Fact("shr_neg7_1", value: i >>> 1);
        probe.Fact("ord_true", value: true ? 1 : 0);
        probe.Fact("ord_false", value: false ? 1 : 0);
        foreach (int n in new[] { 0, 1, 2, 255, 256, -1 })
        {
            byte raw = unchecked((byte)n);
            probe.Fact($"boolean_raw_{n}", value: (int)raw);
            probe.Fact($"boolean_branch_{n}", value: raw != 0 ? 1 : 0);
            probe.Fact($"boolean_eq_true_{n}", value: raw == 1 ? 1 : 0);
            // DCC32 integers.pas:89 emits XOR DL,1 then AND EDX,$7F for Ord(not Bool).
            probe.Fact($"boolean_not_{n}", value: (raw ^ 1) & 0x7f);
        }
        probe.End();
    }
}
