using System.Globalization;
using Xunit.Abstractions;
using static Aow.Rtl.RtlUnit;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class ProbeFactAttribute : FactAttribute
{
    public ProbeFactAttribute(string name) => Recording = name;

    public string Recording { get; }
}

internal sealed class ProbeLines(string name)
{
    private readonly string[] lines = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Probes", name + ".txt"));
    private int position;

    internal string Current => lines[position];

    internal void Fact(string label, object value)
    {
        string actual = label + " " + Convert.ToString(value, CultureInfo.InvariantCulture);
        Assert.True(position < lines.Length, $"{name}: extra line {actual}");
        Assert.True(lines[position] == actual,
            $"{name}.txt:{position + 1} [{label}] expected <{lines[position]}> actual <{actual}>");
        position++;
    }

    internal void End()
    {
        Fact("end", position);
        Assert.Equal(lines.Length, position);
    }

    // The recording names the Delphi class the original raised. The probe selects which class a
    // .NET type stands for. A type with no row fails the test.
    private static readonly Dictionary<string, Dictionary<Type, string>> DelphiNames = new()
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

    internal string DelphiName(System.Exception error) =>
        DelphiNames.TryGetValue(name, out Dictionary<Type, string>? table)
        && table.TryGetValue(error.GetType(), out string? delphi)
            ? delphi
            : throw new Xunit.Sdk.XunitException(name + " has no Delphi name for " + error.GetType().Name);

    internal string Thrown(Action action)
    {
        try
        {
            action();
        }
        catch (System.Exception error)
        {
            return DelphiName(error);
        }

        throw new Xunit.Sdk.XunitException(name + " returned where the recording names an exception");
    }
}

public sealed class ProbeTests(ITestOutputHelper output)
{
    [ProbeFact("random")]
    public void RandomFixture()
    {
        var probe = new ProbeLines("random");
        foreach (int bound in new[] { 2, 6, 10, 100, 1000, 65536, int.MaxValue, 0, 1 })
        {
            RandSeed = 123456789;
            probe.Fact($"initial_seed_n_{bound}", RandSeed);
            for (int i = 1; i <= 1000; i++)
            {
                int value = Random(bound);
                probe.Fact($"n_{bound}_call_{i}_seed", RandSeed);
                probe.Fact($"n_{bound}_call_{i}_value", value);
            }
        }
        RandSeed = 123456789;
        probe.Fact("initial_seed_float", RandSeed);
        for (int i = 1; i <= 1000; i++)
        {
            double value = Random();
            probe.Fact($"float_call_{i}_seed", RandSeed);
            probe.Fact($"float_call_{i}_scale_1048576", Trunc(value * 1048576));
        }
        RandSeed = 0;
        int next = Random(10);
        probe.Fact("seed_zero_next_seed", RandSeed);
        probe.Fact("seed_zero_value", next);
        RandSeed = -1;
        next = Random(10);
        probe.Fact("seed_neg1_next_seed", RandSeed);
        probe.Fact("seed_neg1_value", next);
        probe.End();
    }

    [ProbeFact("floats")]
    public void FloatsFixture()
    {
        var probe = new ProbeLines("floats");
        for (int i = -8; i <= 8; i++)
        {
            double x = i / 2.0;
            probe.Fact($"round_half_index_{i}", Round(x));
            probe.Fact($"trunc_half_index_{i}", Trunc(x));
            probe.Fact($"int_half_index_{i}_scale_1048576", Trunc(Int(x) * 1048576));
            probe.Fact($"frac_half_index_{i}_scale_1048576", Trunc(Frac(x) * 1048576));
        }
        probe.Fact("round_below_pos_half", Round(2.5 - 1.0 / 1048576));
        probe.Fact("round_above_pos_half", Round(2.5 + 1.0 / 1048576));
        probe.Fact("round_below_neg_half", Round(-2.5 - 1.0 / 1048576));
        probe.Fact("round_above_neg_half", Round(-2.5 + 1.0 / 1048576));
        for (int i = -3; i <= 3; i++)
        {
            double x = i / 4.0;
            probe.Fact($"quarter_trunc_{i}", Trunc(x));
            probe.Fact($"quarter_int_{i}_scale_1048576", Trunc(Int(x) * 1048576));
            probe.Fact($"quarter_frac_{i}_scale_1048576", Trunc(Frac(x) * 1048576));
        }
        probe.Fact("integer_divide_7_2_scale_1048576", Trunc(7.0 / 2 * 1048576));
        probe.Fact("integer_divide_neg7_2_scale_1048576", Trunc(-7.0 / 2 * 1048576));
        probe.Fact("integer_divide_1_3_scale_1048576", Trunc(1.0 / 3 * 1048576));
        probe.Fact("trunc_large_pos", Trunc(123456.75));
        probe.Fact("trunc_large_neg", Trunc(-123456.75));
        probe.End();
    }

    [ProbeFact("strings")]
    public void StringsFixture()
    {
        var probe = new ProbeLines("strings");
        void Bytes(string label, string value) => probe.Fact(label, Convert.ToHexString(Ptr<byte>.Unbox(value).Array!.AsSpan(0, value.Length)));
        string s = "abcd";
        foreach (var (label, index, count) in new[] {
            ("start",1,2), ("tail",3,99), ("zero_index",0,2), ("negative_index",-2,2),
            ("past_end",5,1), ("zero_count",1,0), ("negative_count",1,-1) })
            Bytes("copy_" + label, Copy(s, index, count));
        Bytes("copy_empty", Copy("", 1, 4));
        foreach (var (label, index, count) in new[] {
            ("start",1,2), ("tail",3,99), ("zero_index",0,2), ("negative_index",-1,2),
            ("past_end",5,2), ("zero_count",2,0), ("negative_count",2,-1) })
        {
            var t = s;
            Delete(ref t, index, count);
            Bytes("delete_" + label, t);
        }
        foreach (var (label, index) in new[] {
            ("start",1), ("middle",3), ("end",5), ("past_end",99), ("zero_index",0), ("negative_index",-1) })
        {
            var t = s;
            Insert("XY", ref t, index);
            Bytes("insert_" + label, t);
        }
        Bytes("insert_empty_source", Insert("", s, 2));
        Bytes("insert_empty_target", Insert("XY", "", 1));
        var self = s;
        Insert(self, ref self, 3);
        Bytes("insert_self", self);
        probe.Fact("pos_start", Pos("ab", s));
        probe.Fact("pos_end", Pos("cd", s));
        probe.Fact("pos_absent", Pos("x", s));
        probe.Fact("pos_empty_needle", Pos("", s));
        probe.Fact("pos_empty_haystack", Pos("a", ""));
        probe.Fact("pos_both_empty", Pos("", ""));
        probe.Fact("pos_first_repeat", Pos("aa", "aaaa"));
        probe.Fact("pos_long_needle", Pos("abcde", s));
        probe.Fact("pos_embedded_zero", Pos("\0b", "a\0b"));
        probe.Fact("prefix_lt", Ord(Cp1252.Compare("ab", "abc") < 0));
        probe.Fact("prefix_gt", Ord(Cp1252.Compare("abc", "ab") > 0));
        probe.Fact("empty_lt", Ord(Cp1252.Compare("", "a") < 0));
        probe.Fact("prefix_le", Ord(Cp1252.Compare("ab", "abc") <= 0));
        probe.Fact("prefix_ge", Ord(Cp1252.Compare("abc", "ab") >= 0));
        probe.Fact("equal", Ord(s == "abcd"));
        probe.Fact("unequal", Ord(s != "abc"));
        probe.Fact("high_byte_lt", Ord(Cp1252.Compare("\u20AC", "\u00FF") < 0));
        probe.Fact("high_vs_ascii_gt", Ord(Cp1252.Compare("\u20AC", "z") > 0));
        probe.Fact("embedded_zero_compare", Ord(Cp1252.Compare("a\0b", "a\0c") < 0));
        s = "abc";
        var alias = s;
        s = WithChar(s, 2, 'X');
        Bytes("alias_mutated", s);
        Bytes("alias_original", alias);
        alias = WithChar(alias, 1, 'Z');
        Bytes("alias_second_write", alias);
        Bytes("alias_first_preserved", s);
        s = "a\0\u20AC\u00FFz";
        Bytes("binary_string", s);
        probe.Fact("binary_length", Length(s));
        alias = s;
        SetLength(ref alias, 3);
        Bytes("setlength_shrink", alias);
        Bytes("setlength_alias", s);
        SetLength(ref alias, 0);
        Bytes("setlength_empty", alias);

        SysUtilsProbeTests.StringsRemaining(probe, output);
    }

    [ProbeFact("sets")]
    public void SetsFixture()
    {
        var probe = new ProbeLines("sets");
        string Bits(TSet<byte> set, int width) => string.Concat(Enumerable.Range(0, width).Select(i => set.Contains((byte)i) ? '1' : '0'));
        foreach (int width in new[] { 8, 16, 32, 256 })
        {
            var empty = TSet<byte>.WithBounds(0, width - 1);
            var a = empty.With(0).With((byte)(width - 1));
            var b = empty.With(1).With((byte)(width - 1));
            probe.Fact($"size_{width}", a.StorageSize);
            probe.Fact($"union_{width}", Bits(a + b, width));
            probe.Fact($"difference_{width}", Bits(a - b, width));
            var c = a & b;
            probe.Fact($"intersection_{width}", Bits(c, width));
            probe.Fact($"subset_true_{width}", Ord(c.IsSubsetOf(a)));
            probe.Fact($"subset_false_{width}", Ord(a.IsSubsetOf(b)));
            probe.Fact($"superset_{width}", Ord(c.IsSubsetOf(a)));
            probe.Fact($"equal_{width}", Ord(a == b));
            probe.Fact($"not_equal_{width}", Ord(a != b));
            probe.Fact($"member_low_{width}", Ord(a.Contains(0)));
            probe.Fact($"member_high_{width}", Ord(a.Contains((byte)(width - 1))));
            probe.Fact($"member_absent_{width}", Ord(a.Contains(2)));
            a = a.With(1).With(1).Without((byte)(width - 1)).Without(2);
            probe.Fact($"include_exclude_{width}", Bits(a, width));
            probe.Fact($"empty_{width}", Bits(empty, width));
            probe.Fact($"full_{width}", Bits(empty.WithRange(0, (byte)(width - 1)), width));
        }
        probe.Fact("byte_mask", (byte)TSet<byte>.Of(0, 7).Image);
        probe.Fact("word_mask", (ushort)TSet<byte>.Of(0, 8, 15).Image);
        var shifted = TSet<byte>.WithBounds(8, 15);
        probe.Fact("shifted_size", shifted.StorageSize);
        probe.Fact("shifted_mask", (byte)shifted.With(8).With(15).Image);
        probe.Fact("shifted_low_mask", (byte)shifted.With(8).Image);
        probe.Fact("shifted_high_mask", (byte)shifted.With(15).Image);
        var cross = TSet<byte>.WithBounds(7, 8);
        probe.Fact("cross_size", cross.StorageSize);
        probe.Fact("cross_mask", (ushort)cross.With(7).With(8).Image);
        probe.Fact("cross_low_mask", (ushort)cross.With(7).Image);
        probe.Fact("cross_high_mask", (ushort)cross.With(8).Image);
        var full = TSet<byte>.WithBounds(0, 255).WithRange(0, 255);
        for (int i = 0; i <= 255; i++) probe.Fact($"full_member_{i}", Ord(full.Contains((byte)i)));
        probe.End();
    }

    [ProbeFact("integers")]
    public void IntegersFixture()
    {
        var probe = new ProbeLines("integers");
        int i = int.MaxValue, j = 1;
        probe.Fact("add_max_1", unchecked(i + j));
        i = int.MinValue;
        probe.Fact("sub_min_1", unchecked(i - j));
        i = 65536;
        probe.Fact("mul_65536_65536", unchecked(i * i));
        i = int.MaxValue;
        probe.Fact("mul_max_2", unchecked(i * 2));
        i = int.MinValue;
        probe.Fact("neg_min", unchecked(-i));
        i = int.MaxValue;
        probe.Fact("inc_max", unchecked(++i));
        probe.Fact("dec_min", unchecked(--i));
        byte b = 255;
        probe.Fact("byte_inc_max", unchecked(++b));
        b = 255;
        probe.Fact("byte_plus_max", unchecked((byte)(b + 1)));
        b = 0;
        probe.Fact("byte_dec_min", unchecked(--b));
        ushort w = 65535;
        probe.Fact("word_inc_max", unchecked(++w));
        w = 65535;
        probe.Fact("word_plus_max", unchecked((ushort)(w + 1)));
        w = 0;
        probe.Fact("word_dec_min", unchecked(--w));
        sbyte s = 127;
        probe.Fact("shortint_inc_max", unchecked(++s));
        s = 127;
        probe.Fact("shortint_plus_max", unchecked((sbyte)(s + 1)));
        s = -128;
        probe.Fact("shortint_dec_min", unchecked(--s));
        short t = 32767;
        probe.Fact("smallint_inc_max", unchecked(++t));
        t = 32767;
        probe.Fact("smallint_plus_max", unchecked((short)(t + 1)));
        t = -32768;
        probe.Fact("smallint_dec_min", unchecked(--t));
        b = 255; w = 65535;
        probe.Fact("byte_promote_add", b + b);
        probe.Fact("word_promote_add", w + w);
        probe.Fact("byte_promote_sub", 0 - b);
        probe.Fact("word_promote_mul", unchecked(w * w));
        for (int k = 0; k < 4; k++)
        {
            i = (k & 1) != 0 ? -7 : 7;
            j = (k & 2) != 0 ? -2 : 2;
            probe.Fact($"div_{i}_{j}", i / j);
            probe.Fact($"mod_{i}_{j}", i % j);
            i -= i % j;
            probe.Fact($"exact_div_sign_{k}", i / j);
            probe.Fact($"exact_mod_sign_{k}", i % j);
        }
        i = 0; j = -2;
        probe.Fact("div_zero_neg2", i / j);
        probe.Fact("mod_zero_neg2", i % j);
        i = -1;
        for (int n = 0; n <= 33; n++)
        {
            probe.Fact($"shr_neg1_{n}", (int)((uint)i >> n));
            probe.Fact($"shl_neg1_{n}", i << n);
        }
        s = -1; t = -1;
        probe.Fact("shortint_shr_neg1_1", (int)((uint)s >> 1));
        probe.Fact("smallint_shr_neg1_1", (int)((uint)t >> 1));
        probe.Fact("byte_shl_8", b << 8);
        probe.Fact("word_shl_16", w << 16);
        i = int.MinValue;
        probe.Fact("shr_min_1", (int)((uint)i >> 1));
        i = -7;
        probe.Fact("shr_neg7_1", (int)((uint)i >> 1));
        probe.Fact("ord_true", Ord(true));
        probe.Fact("ord_false", Ord(false));
        foreach (int n in new[] { 0, 1, 2, 255, 256, -1 })
        {
            byte raw = unchecked((byte)n);
            probe.Fact($"boolean_raw_{n}", Ord(raw));
            probe.Fact($"boolean_branch_{n}", Ord(raw != 0));
            probe.Fact($"boolean_eq_true_{n}", Ord(raw == 1));
            // DCC32 integers.pas:89 emits XOR DL,1 then AND EDX,$7F for Ord(not Bool).
            probe.Fact($"boolean_not_{n}", (raw ^ 1) & 0x7f);
        }
        probe.End();
    }
}
