using System.Globalization;
using static Aow.Rtl.Rtl;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class SysUtilsTests
{
    [Fact]
    public void IntegerParsingAndConversionsHaveDelphiBounds()
    {
        // SYSTEM.PAS:2343 _ValLong rejects signs on hexadecimal values and stops at NUL.
        foreach (AnsiString invalid in new AnsiString[] { "+$1", "-$1", "\t1", "1 ", "$100000000", "-2147483648", "$G", "$", "  ", "+", "0x1", "1\n", "\0" + "1" })
        {
            Assert.Equal(91, StrToIntDef(invalid, 91));
            Assert.Equal((AnsiString)$"'{invalid}' is not a valid integer value", Assert.Throws<EConvertError>(() => StrToInt(invalid)).Message);
        }
        foreach (var (text, value) in new[] { ("  +42", 42), ("-0", 0), ("$80000000", int.MinValue), ("$abcdef", 0xabcdef), ("00000000", 0), ("0002147483647", int.MaxValue), ("12\0junk", 12), ("$fF\0junk", 255), ("-2147483647", -int.MaxValue) })
        {
            Assert.Equal(value, StrToInt(text)); Assert.Equal(value, StrToIntDef(text, 91));
        }
        // SYSUTILS.PAS:3719 ignores integer precision outside 0..15.
        Assert.Equal((AnsiString)"000000000000001", IntToHex(1, 15));
        foreach (int width in new[] { int.MinValue, -1, 0, 16, 32, int.MaxValue }) Assert.Equal((AnsiString)"1", IntToHex(1, width));
        Assert.Equal((AnsiString)"80000000", IntToHex(int.MinValue, 1));
        Assert.Equal((AnsiString)"000", StringOfChar((byte)'0', 3));
        Assert.Equal(default, StringOfChar(255, -1));
        Assert.Equal(AnsiString.FromBytes(255, 255), StringOfChar(255, 2));
        Assert.Equal((AnsiString)"'a''b\0c'", QuotedStr("a'b\0c"));
        Assert.Equal((AnsiString)"''", QuotedStr(default));
        Assert.Equal(3, AnsiPos("\0b", "aa\0bb"));
        Assert.Equal(0, AnsiPos(default, "aa"));
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
            var value = AnsiString.FromBytes((byte)c);
            Assert.Equal(AnsiString.FromBytes((byte)lower), AnsiLowerCase(value));
            Assert.Equal(AnsiString.FromBytes((byte)upper), AnsiUpperCase(value));
        }
        Assert.Equal(default, AnsiUpperCase(default)); Assert.Equal(default, AnsiLowerCase(default));
        Assert.Equal(128, CompareStr("\xff", "\x7f"));
        Assert.Equal(-2, CompareStr("a", "abc"));
        Assert.Equal(-1, CompareStr("a\0b", "a\0c"));
        Assert.Equal(0, CompareText("aB", "Ab"));
        Assert.Equal(128, CompareText("\xff", "\x7f"));
        Assert.Equal(0, AnsiCompareStr("\xad", "\0"));
        Assert.True(AnsiCompareStr("A", "a") < 0);
        Assert.Equal(0, AnsiCompareText("\xc0\x8a\x8c\x9f", "\xe0\x9a\x9c\xff"));
        Assert.True(AnsiCompareText("ab", "ABC") < 0);
        Assert.Equal((AnsiString)"a \0", TrimLeft("\t a \0"));
        Assert.Equal((AnsiString)"\0 a", TrimRight("\0 a\t\0"));
        Assert.Equal(default, TrimLeft(" \t")); Assert.Equal(default, TrimRight(" \t"));
    }

    [Fact]
    public void FormatCoversAllTaggedKindsAndSourceParserEdges()
    {
        // SYSUTILS.PAS:3498 FormatBuf, checked with DCC32 shim-edges.pas.
        foreach (string text in new[] { "%", "%3", "%-", "%.", "%0:", "%*", "%.*" }) Assert.Equal(default, Format(text));
        Assert.Equal((AnsiString)"abc", Format("abc%"));
        Assert.Equal((AnsiString)"7", Format("%.16d", 7));
        Assert.Equal((AnsiString)"000000000000007", Format("%.15d", 7));
        Assert.Equal((AnsiString)"A", Format("%.0s", (byte)'A'));
        Assert.Equal((AnsiString)"4294967295", Format("%u", -1));
        Assert.Equal((AnsiString)"-2147483648", Format("%D", int.MinValue));
        Assert.Equal((AnsiString)"80000000", Format("%X", int.MinValue));
        Assert.Equal((AnsiString)"FFFFFFFF", Format("%p", TVarRec.Pointer(uint.MaxValue)));
        Assert.Equal((AnsiString)"  00000001", Format("%10.2P", TVarRec.Pointer(1)));
        Assert.Equal((AnsiString)"abc", Format("%s", ((AnsiString)"abc\0def").AsPtr()));
        Assert.Equal((AnsiString)"a\0b", Format("%s", (AnsiString)"a\0b"));
        Assert.Equal((AnsiString)"bc", Format("%*:*s", 1, 2, (AnsiString)"bc"));
        Assert.Equal((AnsiString)"1", Format("%*d", false, 1));
        Assert.Equal((AnsiString)"1", Format("%*d", (AnsiString)"x", 1));
        Assert.Equal((AnsiString)"abcdef", Format("%0:1:s", 7, (AnsiString)"abcdef"));
        Assert.Equal((AnsiString)"abc", Format("%:s", (AnsiString)"abc"));
        foreach (string kind in new[] { "%e", "%f", "%g", "%n", "%m" }) Assert.Throws<EConvertError>(() => Format(kind, 1.25));
        foreach (TVarRec arg in new TVarRec[] { true, 1.25, new TObject(), (TObject.Meta?)null, TVarRec.Pointer(0) }) Assert.Throws<EConvertError>(() => Format("%s", arg));
        Assert.Throws<EConvertError>(() => Format("%p", 1));
        Assert.Throws<EConvertError>(() => Format("%d", (byte)'a'));
        Assert.Throws<EConvertError>(() => Format("%0:1:d", 7, (AnsiString)"abcdef"));
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
                DecodeTime(v, out int h, out int m, out int s, out int ms);
                Assert.Equal((12, 34, 56, 789), (h, m, s, ms));
            }
            DecodeDate(-1.5, out int y, out int month, out int day);
            Assert.Equal((1899, 12, 29), (y, month, day));
            DecodeDate(-0.5, out y, out month, out day);
            Assert.Equal((1899, 12, 30), (y, month, day));
            DecodeDate(EncodeDate(1, 1, 1) - 1, out y, out month, out day);
            Assert.Equal((0, 0, 0), (y, month, day));
            Assert.Equal((AnsiString)"2/29/2024 12:34:56 PM", DateTimeToStr(Now()));
            Assert.Equal(0, DateTimeToFileDate(EncodeDate(1979, 12, 31)));
            Assert.Equal(0, DateTimeToFileDate(EncodeDate(2100, 1, 1)));
            int dos = DateTimeToFileDate(date + time);
            Assert.Equal(date + EncodeTime(12, 34, 56, 0), FileDateToDateTime(dos));
            ushort yearWord = 0, monthWord = 0, dayWord = 0, hourWord = 0, minuteWord = 0, secondWord = 0, msWord = 0;
            DecodeDate(date, ref yearWord, ref monthWord, ref dayWord);
            DecodeTime(time, ref hourWord, ref minuteWord, ref secondWord, ref msWord);
            Assert.Equal((2024, 2, 29), ((int)yearWord, (int)monthWord, (int)dayWord));
            Assert.Equal((12, 34, 56, 789), ((int)hourWord, (int)minuteWord, (int)secondWord, (int)msWord));
            Assert.Throws<EConvertError>(() => EncodeDate(1900, 2, 29));
            Assert.Throws<EConvertError>(() => EncodeDate(0, 1, 1));
            foreach (var (h, m, s, ms) in new[] { (-1, 0, 0, 0), (24, 0, 0, 0), (0, 60, 0, 0), (0, 0, 60, 0), (0, 0, 0, 1000) }) Assert.Throws<EConvertError>(() => EncodeTime(h, m, s, ms));
        }
        finally { Clock = saved; }
        var system = new SystemClock();
        Assert.InRange((DateTime.Now - system.Now).Duration(), TimeSpan.Zero, TimeSpan.FromSeconds(1));
        Assert.InRange((DateTime.UtcNow - system.UtcNow).Duration(), TimeSpan.Zero, TimeSpan.FromSeconds(1));
        uint systemTick = system.TickCount;
        Assert.InRange(unchecked((uint)Environment.TickCount64 - systemTick), 0u, 1000u);
    }
}
