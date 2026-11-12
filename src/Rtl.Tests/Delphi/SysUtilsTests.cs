
using static Aow.Rtl.RtlUnit;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class SysUtilsTests
{
    [Fact]
    public void IntegerParsingAndConversionsHaveDelphiBounds()
    {
        // SYSTEM.PAS:2343 _ValLong rejects signs on hexadecimal values and stops at NUL.
        foreach (string invalid in new string[] { "+$1", "-$1", "\t1", "1 ", "$100000000", "-2147483648", "$G", "$", "  ", "+", "0x1", "1\n", "\0" + "1" })
        {
            Assert.Equal(91, StrToIntDef(invalid, 91));
            Assert.Equal($"'{invalid}' is not a valid integer value", Assert.Throws<FormatException>(() => StrToInt(invalid)).Message);
        }
        foreach (var (text, value) in new[] { ("  +42", 42), ("-0", 0), ("$80000000", int.MinValue), ("$abcdef", 0xabcdef), ("00000000", 0), ("0002147483647", int.MaxValue), ("12\0junk", 12), ("$fF\0junk", 255), ("-2147483647", -int.MaxValue) })
        {
            Assert.Equal(value, StrToInt(text)); Assert.Equal(value, StrToIntDef(text, 91));
        }
        // SYSUTILS.PAS:3719 ignores integer precision outside 0..15.
        Assert.Equal("000000000000001", IntToHex(1, 15));
        foreach (int width in new[] { int.MinValue, -1, 0, 16, 32, int.MaxValue }) Assert.Equal("1", IntToHex(1, width));
        Assert.Equal("80000000", IntToHex(int.MinValue, 1));
        Assert.Equal("000", StringOfChar((byte)'0', 3));
        Assert.Equal("", StringOfChar(255, -1));
        Assert.Equal("\u00FF\u00FF", StringOfChar(255, 2));
        Assert.Equal("'a''b\0c'", QuotedStr("a'b\0c"));
        Assert.Equal("''", QuotedStr(""));
        Assert.Equal(3, AnsiPos("\0b", "aa\0bb"));
        Assert.Equal(0, AnsiPos("", "aa"));
        Assert.Equal(0, AnsiPos("z", "aa"));
    }

    [Fact]
    public void CaseTablesCoverEveryByteAndComparisonsKeepByteDifferences()
    {
        // SYSUTILS.PAS:2171 and :2180 use CP1252 CharUpperBuff and CharLowerBuff.
        var pairs = Enumerable.Range(65, 26).Select(c => (Upper: c, Lower: c + 32))
            .Concat(Enumerable.Range(192, 23).Select(c => (Upper: c, Lower: c + 32)))
            .Concat(Enumerable.Range(216, 7).Select(c => (Upper: c, Lower: c + 32)))
            .Concat(new[] { (Upper: 138, Lower: 154), (Upper: 140, Lower: 156), (Upper: 142, Lower: 158), (Upper: 159, Lower: 255) }).ToArray();
        for (int c = 0; c < 256; c++)
        {
            int lower = pairs.Where(p => p.Upper == c).Select(p => p.Lower).DefaultIfEmpty(c).Single();
            int upper = pairs.Where(p => p.Lower == c).Select(p => p.Upper).DefaultIfEmpty(c).Single();
            var value = Cp1252.Text((byte)c);
            Assert.Equal(Cp1252.Text((byte)lower), AnsiLowerCase(value));
            Assert.Equal(Cp1252.Text((byte)upper), AnsiUpperCase(value));
        }
        Assert.Equal("", AnsiUpperCase("")); Assert.Equal("", AnsiLowerCase(""));
        Assert.Equal(128, CompareStr("\xff", "\x7f"));
        Assert.Equal(-2, CompareStr("a", "abc"));
        Assert.Equal(-1, CompareStr("a\0b", "a\0c"));
        Assert.Equal(0, CompareText("aB", "Ab"));
        Assert.Equal(128, CompareText("\xff", "\x7f"));
        Assert.Equal(0, AnsiCompareStr("\xad", "\0"));
        Assert.True(AnsiCompareStr("A", "a") < 0);
        Assert.Equal(0, AnsiCompareText("\u00C0\u0160\u0152\u0178", "\u00E0\u0161\u0153\u00FF"));
        Assert.True(AnsiCompareText("ab", "ABC") < 0);
        Assert.Equal("a \0", TrimLeft("\t a \0"));
        Assert.Equal("\0 a", TrimRight("\0 a\t\0"));
        Assert.Equal("", TrimLeft(" \t")); Assert.Equal("", TrimRight(" \t"));
    }

    [Fact]
    public void FormatCoversAllTaggedKindsAndSourceParserEdges()
    {
        // SYSUTILS.PAS:3498 FormatBuf, checked with DCC32 shim-edges.pas.
        foreach (string text in new[] { "%", "%3", "%-", "%.", "%0:", "%*", "%.*" }) Assert.Equal("", Format(text));
        Assert.Equal("abc", Format("abc%"));
        Assert.Equal("7", Format("%.16d", 7));
        Assert.Equal("000000000000007", Format("%.15d", 7));
        Assert.Equal("A", Format("%.0s", (byte)'A'));
        Assert.Equal("4294967295", Format("%u", -1));
        Assert.Equal("-2147483648", Format("%D", int.MinValue));
        Assert.Equal("80000000", Format("%X", int.MinValue));
        Assert.Equal("FFFFFFFF", Format("%p", new PointerArg(uint.MaxValue)));
        Assert.Equal("  00000001", Format("%10.2P", new PointerArg(1)));
        Assert.Equal("abc", Format("%s", StrPas(Ptr<byte>.Unbox("abc\0def"))));
        Assert.Equal("a\0b", Format("%s", "a\0b"));
        Assert.Equal("bc", Format("%*:*s", 1, 2, "bc"));
        Assert.Equal("1", Format("%*d", false, 1));
        Assert.Equal("1", Format("%*d", "x", 1));
        Assert.Equal("abcdef", Format("%0:1:s", 7, "abcdef"));
        Assert.Equal("abc", Format("%:s", "abc"));
        foreach (string kind in new[] { "%e", "%f", "%g", "%n", "%m" }) Assert.Throws<FormatException>(() => Format(kind, 1.25));
        foreach (FormatArg argument in new FormatArg[] { true, 1.25, new object(), (object?)null, new PointerArg(0) }) Assert.Throws<FormatException>(() => Format("%s", argument));
        Assert.Throws<FormatException>(() => Format("%p", 1));
        Assert.Throws<FormatException>(() => Format("%d", (byte)'a'));
        Assert.Throws<FormatException>(() => Format("%0:1:d", 7, "abcdef"));
    }

    private sealed class FixedClock : IClock
    {
        public DateTime Now => new(2024, 2, 29, 12, 34, 56, 789, DateTimeKind.Local);
        public DateTime UtcNow => new(2024, 2, 29, 20, 34, 56, 789, DateTimeKind.Utc);
        public uint TickCount => uint.MaxValue;
        public double ElapsedMilliseconds => 0;

        public void WaitMilliseconds(int milliseconds) => ArgumentOutOfRangeException.ThrowIfNegative(milliseconds);
    }

    [Fact]
    public void TimeUsesAnInjectedClockAndDelphiNegativeFractions()
    {
        IClock saved = Clock;
        try
        {
            Clock = new FixedClock();
            double date = EncodeDate(2024, 2, 29), time = EncodeTime(12, 34, 56, 789);
            Assert.Equal(date + time, Now()); Assert.Equal(date, Date()); Assert.Equal(time, Time());
            Assert.Equal(uint.MaxValue, GetTickCount()); Assert.Equal(uint.MaxValue, timeGetTime());
            Randomize(); Assert.Equal(74096789, RandSeed);
            Assert.Equal(0, EncodeDate(1899, 12, 30));
            Assert.Equal(-1, EncodeDate(1899, 12, 29));
            foreach (double v in new[] { date + time, -1 - time, -time })
            {
                DecodeTime(v, out int h, out int m, out int s, out int milliseconds);
                Assert.Equal((12, 34, 56, 789), (h, m, s, milliseconds));
            }
            DecodeDate(-1.5, out int y, out int month, out int day);
            Assert.Equal((1899, 12, 29), (y, month, day));
            DecodeDate(-0.5, out y, out month, out day);
            Assert.Equal((1899, 12, 30), (y, month, day));
            DecodeDate(EncodeDate(1, 1, 1) - 1, out y, out month, out day);
            Assert.Equal((0, 0, 0), (y, month, day));
            Assert.Equal("2/29/2024 12:34:56 PM", DateTimeToStr(Now()));
            Assert.Equal(0, DateTimeToFileDate(EncodeDate(1979, 12, 31)));
            Assert.Equal(0, DateTimeToFileDate(EncodeDate(2100, 1, 1)));
            int dos = DateTimeToFileDate(date + time);
            Assert.Equal(date + EncodeTime(12, 34, 56, 0), FileDateToDateTime(dos));
            ushort yearWord = 0, monthWord = 0, dayWord = 0, hourWord = 0, minuteWord = 0, secondWord = 0, millisecondsWord = 0;
            DecodeDate(date, out yearWord, out monthWord, out dayWord);
            DecodeTime(time, out hourWord, out minuteWord, out secondWord, out millisecondsWord);
            Assert.Equal((2024, 2, 29), ((int)yearWord, (int)monthWord, (int)dayWord));
            Assert.Equal((12, 34, 56, 789), ((int)hourWord, (int)minuteWord, (int)secondWord, (int)millisecondsWord));
            Assert.Throws<FormatException>(() => EncodeDate(1900, 2, 29));
            Assert.Throws<FormatException>(() => EncodeDate(0, 1, 1));
            foreach (var (h, m, s, milliseconds) in new[] { (-1, 0, 0, 0), (24, 0, 0, 0), (0, 60, 0, 0), (0, 0, 60, 0), (0, 0, 0, 1000) }) Assert.Throws<FormatException>(() => EncodeTime(h, m, s, milliseconds));
        }
        finally { Clock = saved; }
        var system = new SystemClock();
        Assert.InRange((DateTime.Now - system.Now).Duration(), TimeSpan.Zero, TimeSpan.FromSeconds(1));
        Assert.InRange((DateTime.UtcNow - system.UtcNow).Duration(), TimeSpan.Zero, TimeSpan.FromSeconds(1));
        uint systemTick = system.TickCount;
        Assert.InRange(unchecked((uint)Environment.TickCount64 - systemTick), 0u, 1000u);
    }
}
