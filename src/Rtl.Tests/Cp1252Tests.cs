using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class Cp1252Tests
{
    private static char ExpectedChar(byte value) => value switch
    {
        128 => '\u20ac', 130 => '\u201a', 131 => '\u0192', 132 => '\u201e',
        133 => '\u2026', 134 => '\u2020', 135 => '\u2021', 136 => '\u02c6',
        137 => '\u2030', 138 => '\u0160', 139 => '\u2039', 140 => '\u0152',
        142 => '\u017d', 145 => '\u2018', 146 => '\u2019', 147 => '\u201c',
        148 => '\u201d', 149 => '\u2022', 150 => '\u2013', 151 => '\u2014',
        152 => '\u02dc', 153 => '\u2122', 154 => '\u0161', 155 => '\u203a',
        156 => '\u0153', 158 => '\u017e', 159 => '\u0178',
        _ => (char)value
    };

    [Fact]
    public void EveryByteHasTheSpecifiedCharAndInverseAndCachedText()
    {
        var chars = new HashSet<char>();
        for (int b = 0; b < 256; b++)
        {
            char c = Cp1252.Char((byte)b);
            Assert.Equal(ExpectedChar((byte)b), c);
            Assert.Equal(AnsiString.FromBytes((byte)b).ToString(), c.ToString());
            Assert.True(chars.Add(c));
            Assert.Equal((byte)b, Cp1252.Byte(c));
            Assert.Equal(c.ToString(), Cp1252.Text((byte)b));
            Assert.Same(Cp1252.Text((byte)b), Cp1252.Text((byte)b));
        }
        Assert.Equal(256, chars.Count);
    }

    [Fact]
    public void EveryCharMapsToItsByteOrQuestionMark()
    {
        var inverse = Enumerable.Range(0, 256).ToDictionary(i => ExpectedChar((byte)i), i => (byte)i);
        for (int c = 0; c <= char.MaxValue; c++)
            Assert.Equal(inverse.GetValueOrDefault((char)c, (byte)63), Cp1252.Byte((char)c));
    }

    [Fact]
    public void CompareUsesUnsignedBytesOverEveryPair()
    {
        for (int a = 0; a < 256; a++)
            for (int b = 0; b < 256; b++)
                Assert.Equal(a - b, Cp1252.Compare(Cp1252.Text((byte)a), Cp1252.Text((byte)b)));
        Assert.Equal(-2, Cp1252.Compare("a", "abc"));
        Assert.Equal(1, Cp1252.Compare("a\0c", "a\0b"));
        Assert.Equal(-127, Cp1252.Compare("\u20ac", "\u00ff"));
        Assert.Equal(0, Cp1252.Compare(null, ""));
        Assert.Equal(-1, Cp1252.Compare(null, "x"));
        Assert.Equal(1, Cp1252.Compare("x", null));
        Assert.Equal(0, Cp1252.Compare("\u2603", "?"));
    }

    [Fact]
    public void DecodeEncodeRoundTripsRandomBuffersIncludingEmbeddedZeros()
    {
        var random = new Random(9501);
        foreach (int length in new[] { 0, 1, 2, 3, 31, 256, 4096 })
        {
            for (int run = 0; run < 32; run++)
            {
                var bytes = new byte[length];
                random.NextBytes(bytes);
                if (length > 0) bytes[length / 2] = 0;
                string text = Cp1252.Decode(bytes);
                Assert.Equal(length, text.Length);
                Assert.Equal(bytes, Cp1252.Encode(text));
                Assert.Equal(AnsiString.FromBytes(bytes).ToString(), text);
            }
        }
        byte[] all = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
        Assert.Equal(all, Cp1252.Encode(Cp1252.Decode(all)));
        Assert.Equal(new byte[] { 0, 128, 63, 63, 63 }, Cp1252.Encode("\0\u20ac\u0080\u2603\ud800"));
        Assert.Empty(Cp1252.Encode(null));
        Assert.NotSame(Cp1252.Encode("a"), Cp1252.Encode("a"));
    }

    [Fact]
    public void AnsiCaseTablesMatchEveryByte()
    {
        var all = AnsiString.FromBytes(Enumerable.Range(0, 256).Select(i => (byte)i).ToArray());
        Assert.Equal(Rtl.UpperCase(all).ToString(), Rtl.UpperCase(all.ToString()));
        Assert.Equal(Rtl.LowerCase(all).ToString(), Rtl.LowerCase(all.ToString()));
        Assert.Equal(Rtl.AnsiUpperCase(all).ToString(), Rtl.AnsiUpperCase(all.ToString()));
        Assert.Equal(Rtl.AnsiLowerCase(all).ToString(), Rtl.AnsiLowerCase(all.ToString()));
        for (int b = 0; b < 256; b++)
        {
            var ansi = AnsiString.FromBytes((byte)b);
            string text = Cp1252.Text((byte)b);
            Assert.Equal(Rtl.AnsiUpperCase(ansi).ToString(), Rtl.AnsiUpperCase(text));
            Assert.Equal(Rtl.AnsiLowerCase(ansi).ToString(), Rtl.AnsiLowerCase(text));
            Assert.Equal(Rtl.UpperCase(ansi).ToString(), Rtl.UpperCase(text));
            Assert.Equal(Rtl.LowerCase(ansi).ToString(), Rtl.LowerCase(text));
            Assert.Equal(Cp1252.Char(Rtl.UpCase((byte)b)), Rtl.UpCase(Cp1252.Char((byte)b)));
            Assert.Equal(b, Rtl.Ord(Cp1252.Char((byte)b)));
        }
        Assert.Equal("\u00b5", Rtl.AnsiUpperCase("\u00b5"));
    }

    [Fact]
    public void FormatPreservesEveryHighCharAsStringCharByteAndPChar()
    {
        for (int b = 128; b < 256; b++)
        {
            string text = Cp1252.Text((byte)b);
            var ansi = AnsiString.FromBytes((byte)b);
            Assert.Equal(text, Rtl.Format("%s", text));
            Assert.Equal(text, Rtl.Format("%s", text[0]));
            Assert.Equal(text, Rtl.Format("%s", (byte)b));
            Assert.Equal(text, Rtl.Format("%s", ansi.AsPtr()));
            Assert.Equal(text, Rtl.Format((AnsiString)"%s", text[0]).ToString());
            Assert.Equal(text, Rtl.Format((AnsiString)"%s", text).ToString());
            Assert.Equal("[" + text + "\0" + text + "]", Rtl.Format("[%s]", text + "\0" + text));
        }
        Assert.Equal("?", Rtl.Format("%s", '\u2603'));
        Assert.Equal("?", Rtl.Format("%s", "\u0080"));
    }
}
