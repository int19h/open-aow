using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class Cp1252Tests
{
    private static string HeldText(FormatArg argument) => argument is string text ? text : throw new InvalidOperationException("not the String case");
    private static char HeldChar(FormatArg argument) => argument is char c ? c : throw new InvalidOperationException("not the Char case");

    private static char ExpectedChar(byte value) => value switch
    {
        128 => '\u20ac',
        130 => '\u201a',
        131 => '\u0192',
        132 => '\u201e',
        133 => '\u2026',
        134 => '\u2020',
        135 => '\u2021',
        136 => '\u02c6',
        137 => '\u2030',
        138 => '\u0160',
        139 => '\u2039',
        140 => '\u0152',
        142 => '\u017d',
        145 => '\u2018',
        146 => '\u2019',
        147 => '\u201c',
        148 => '\u201d',
        149 => '\u2022',
        150 => '\u2013',
        151 => '\u2014',
        152 => '\u02dc',
        153 => '\u2122',
        154 => '\u0161',
        155 => '\u203a',
        156 => '\u0153',
        158 => '\u017e',
        159 => '\u0178',
        _ => (char)value
    };

    [Fact]
    public void EveryByteHasTheSpecifiedCharAndInverseAndCachedText()
    {
        HashSet<char> chars = [];
        for (int b = 0; b < 256; b++)
        {
            char c = Cp1252.Char((byte)b);
            Assert.Equal(ExpectedChar((byte)b), c);
            Assert.Equal(Cp1252.Text((byte)b), c.ToString());
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
        Dictionary<char, byte> inverse = Enumerable.Range(0, count: 256).ToDictionary(i => ExpectedChar((byte)i), i => (byte)i);
        for (int c = 0; c <= char.MaxValue; c++)
        {
            Assert.Equal(inverse.GetValueOrDefault((char)c, (byte)63), Cp1252.Byte((char)c));
        }
    }

    [Fact]
    public void CompareUsesUnsignedBytesOverEveryPair()
    {
        using StringMode mode = new(false);
        for (int a = 0; a < 256; a++)
        {
            for (int b = 0; b < 256; b++)
            {
                Assert.Equal(a - b, Cp1252.Compare(Cp1252.Text((byte)a), Cp1252.Text((byte)b)));
            }
        }

        Assert.Equal(-2, Cp1252.Compare("a", "abc"));
        Assert.Equal(1, Cp1252.Compare("a\0c", "a\0b"));
        Assert.Equal(-127, Cp1252.Compare("\u20ac", "\u00ff"));
        Assert.Equal(0, Cp1252.Compare(null, ""));
        Assert.Equal(-1, Cp1252.Compare(null, "x"));
        Assert.Equal(1, Cp1252.Compare("x", null));
        Assert.True(Cp1252.Compare("\u2603", "?") > 0);
    }

    [Fact]
    public void DecodeEncodeRoundTripsRandomBuffersIncludingEmbeddedZeros()
    {
        using StringMode mode = new(false);
        Random random = new(9501);
        foreach (int length in new[] { 0, 1, 2, 3, 31, 256, 4096 })
        {
            for (int run = 0; run < 32; run++)
            {
                byte[] bytes = new byte[length];
                random.NextBytes(bytes);
                if (length > 0)
                {
                    bytes[length / 2] = 0;
                }

                string text = Cp1252.Decode(bytes);
                Assert.Equal(length, text.Length);
                Assert.Equal(bytes, Cp1252.Encode(text));
                Assert.Equal(Cp1252.Decode(bytes), text);
            }
        }
        byte[] all = [.. Enumerable.Range(0, count: 256).Select(i => (byte)i)];
        Assert.Equal(all, Cp1252.Encode(Cp1252.Decode(all)));
        Assert.Equal(new byte[] { 0, 128, 63, 63, 63 }, Cp1252.Encode("\0\u20ac\u0080\u2603\ud800"));
        Assert.Empty(Cp1252.Encode(null));
        Assert.NotSame(Cp1252.Encode("a"), Cp1252.Encode("a"));
    }

    [Fact]
    public void AnsiCaseTablesMatchEveryByte()
    {
        string all = Cp1252.Decode(Enumerable.Range(0, count: 256).Select(i => (byte)i).ToArray());
        Assert.Equal(DelphiTextCompare.UpperCase(all), DelphiTextCompare.UpperCase(all));
        Assert.Equal(DelphiTextCompare.LowerCase(all), DelphiTextCompare.LowerCase(all));
        Assert.Equal(DelphiTextCompare.AnsiUpperCase(all), DelphiTextCompare.AnsiUpperCase(all));
        Assert.Equal(DelphiTextCompare.AnsiLowerCase(all), DelphiTextCompare.AnsiLowerCase(all));
        for (int b = 0; b < 256; b++)
        {
            string ansi = Cp1252.Text((byte)b);
            string text = Cp1252.Text((byte)b);
            Assert.Equal(DelphiTextCompare.AnsiUpperCase(ansi), DelphiTextCompare.AnsiUpperCase(text));
            Assert.Equal(DelphiTextCompare.AnsiLowerCase(ansi), DelphiTextCompare.AnsiLowerCase(text));
            Assert.Equal(DelphiTextCompare.UpperCase(ansi), DelphiTextCompare.UpperCase(text));
            Assert.Equal(DelphiTextCompare.LowerCase(ansi), DelphiTextCompare.LowerCase(text));
            Assert.Equal(Cp1252.Char(DelphiOrdinals.UpCase((byte)b)), DelphiOrdinals.UpCase(Cp1252.Char((byte)b)));
            Assert.Equal(b, Cp1252.Byte(Cp1252.Char((byte)b)));
        }
        Assert.Equal("\u00b5", DelphiTextCompare.AnsiUpperCase("\u00b5"));
    }

    [Fact]
    public void FormatPreservesEveryHighCharAsStringCharByteAndPChar()
    {
        for (int b = 128; b < 256; b++)
        {
            string text = Cp1252.Text((byte)b);
            Assert.Equal(text, DelphiFormat.Format("%s", text));
            Assert.Equal(text, DelphiFormat.Format("%s", text[0]));
            Assert.Equal(text, DelphiFormat.Format("%s", (byte)b));
            Assert.Equal(text, DelphiFormat.Format("%s", PCharStrings.FromPChar(Ptr<byte>.Unbox(text))));
            Assert.Equal($"[{text}\0{text}]", DelphiFormat.Format("[%s]", text + "\0" + text));
        }
        // A char outside the table stays itself in a string or char argument. It meets the table
        // where it becomes a byte, as in a PChar, which reaches Format through StrPas.
        Assert.Equal("\u2603", DelphiFormat.Format("%s", '\u2603'));
        Assert.Equal("\u0080", DelphiFormat.Format("%s", "\u0080"));
        Assert.Equal("?", DelphiFormat.Format("%s", PCharStrings.FromPChar(Ptr<byte>.Unbox("\u2603"))));
        Assert.Equal("?", DelphiFormat.Format("%s", PCharStrings.FromPChar(Ptr<byte>.Unbox("\u0080"))));
    }

    [Fact]
    public void OneEncoderMatchesTheFrozenMapForEveryCharAndEveryProducer()
    {
        Dictionary<char, byte> inverse = Enumerable.Range(0, count: 256).ToDictionary(i => ExpectedChar((byte)i), i => (byte)i);
        string text = new([.. Enumerable.Range(0, count: char.MaxValue + 1).Select(i => (char)i)]);
        byte[] expected = [.. text.Select(c => inverse.GetValueOrDefault(c, (byte)63))];
        string normalized = new([.. expected.Select(ExpectedChar)]);
        Assert.Equal(expected, Cp1252.Encode(text));
        Assert.Equal(expected, Cp1252.Encode(HeldText(text)));
        Assert.Equal(expected, Cp1252.Encode(normalized));
        Assert.Equal(normalized, Cp1252.Normalize(text));
        Assert.Equal(normalized, Cp1252.Decode(Cp1252.Encode(text)));
        // A host string keeps its chars through Format and meets the table at Encode and Byte.
        Assert.Equal(text, DelphiFormat.Format("%s", text));
        int changed = 0;
        for (int i = 0; i <= char.MaxValue; i++)
        {
            char c = (char)i;
            string single = c.ToString();
            Assert.Equal(inverse.ContainsKey(c), actual: Cp1252.IsMapped(c));
            Assert.Equal(expected[i], Cp1252.Byte(c));
            Assert.Equal(expected[i], Cp1252.Encode(single)[0]);
            Assert.Equal(expected[i], Cp1252.Byte(HeldChar(c)));
            Assert.Equal(expected[i], Cp1252.Byte(HeldText(single)[0]));
            Assert.Equal(single, DelphiFormat.Format("%s", c));
            Assert.Equal(normalized[i].ToString(), Cp1252.Normalize(single));

            // The old shim accepted Latin-1 C1 escapes before consulting its CP1252 table.
            byte oldShim = c <= 255 ? (byte)c : inverse.GetValueOrDefault(c, (byte)63);
            bool correctedC1 = c is >= '\u0080' and <= '\u009f' && !inverse.ContainsKey(c);
            Assert.Equal(correctedC1, actual: oldShim != expected[i]);
            if (correctedC1)
            {
                Assert.Equal((byte)63, expected[i]);
                changed++;
            }
        }
        Assert.Equal(27, changed);
        Assert.Equal(256, text.Count(Cp1252.IsMapped));
    }

    [Fact]
    public void EveryBytePairKeepsTheExistingRoutineOrder()
    {
        foreach ((Func<string?, string?, int>? textCompare, Func<string, string, int>? byteCompare) in new[] {
            ((Func<string?, string?, int>)DelphiTextCompare.CompareStr, (Func<string, string, int>)DelphiTextCompare.CompareStr),
            (DelphiTextCompare.CompareText, DelphiTextCompare.CompareText), (DelphiTextCompare.AnsiCompareStr, DelphiTextCompare.AnsiCompareStr),
            (DelphiTextCompare.AnsiCompareText, DelphiTextCompare.AnsiCompareText) })
        {
            for (int a = 0; a < 256; a++)
            {
                for (int b = 0; b < 256; b++)
                {
                    Assert.Equal(byteCompare(Cp1252.Text((byte)a), Cp1252.Text((byte)b)),
                        textCompare(ExpectedChar((byte)a).ToString(), ExpectedChar((byte)b).ToString()));
                }
            }
        }
    }

    [Fact]
    public void UnmappedCharsKeepTheirCaseAndOrderAfterEveryMappedChar()
    {
        HashSet<char> mapped = [.. Enumerable.Range(0, count: 256).Select(i => ExpectedChar((byte)i))];
        for (int i = 0; i <= char.MaxValue; i++)
        {
            char c = (char)i;
            if (mapped.Contains(c))
            {
                continue;
            }

            string text = c.ToString();
            Assert.Equal(text, DelphiTextCompare.UpperCase(text));
            Assert.Equal(text, DelphiTextCompare.LowerCase(text));
            Assert.Equal(text, DelphiTextCompare.AnsiUpperCase(text));
            Assert.Equal(text, DelphiTextCompare.AnsiLowerCase(text));
            foreach (Func<string?, string?, int> compare in new Func<string?, string?, int>[] {
                Cp1252.Compare, DelphiTextCompare.CompareStr, DelphiTextCompare.CompareText, DelphiTextCompare.AnsiCompareStr, DelphiTextCompare.AnsiCompareText })
            {
                Assert.Equal(256 + c - 255, compare(text, "\u00ff"));
                Assert.Equal(255 - 256 - c, compare("\u00ff", text));
                Assert.Equal(0, compare(text, text));
                if (i < char.MaxValue && !mapped.Contains((char)(i + 1)))
                {
                    Assert.Equal(-1, compare(text, ((char)(i + 1)).ToString()));
                }
            }
        }
        foreach (char c in mapped)
        {
            Assert.True(Cp1252.Compare(c.ToString(), "\u0080") < 0);
        }
    }

    [Fact]
    public void CompareAndComparerGiveAnInjectiveTotalOrderOutsideTheImage()
    {
        Random random = new(95102);
        string Next() => new([.. Enumerable.Range(0, count: random.Next(9)).Select(_ => (char)random.Next(65536))]);
        Dictionary<char, int> inverse = Enumerable.Range(0, count: 256).ToDictionary(i => ExpectedChar((byte)i), i => i);
        int Key(char c) => inverse.GetValueOrDefault(c, 256 + c);
        int Expected(string a, string b)
        {
            for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
            {
                int difference = Key(a[i]) - Key(b[i]);
                if (difference != 0)
                {
                    return difference;
                }
            }
            return a.Length - b.Length;
        }
        for (int run = 0; run < 4096; run++)
        {
            string a = Next();
            string b = run % 3 == 0 ? new string(a.ToCharArray()) : run % 3 == 1 ? a + Next() : Next();
            string c = Next();
            int order = Cp1252.Compare(a, b);
            Assert.Equal(Expected(a, b), order);
            Assert.Equal(a == b, actual: order == 0);
            Assert.Equal(order, DelphiTextCompare.CompareStr(a, b));
            Assert.Equal(order, Cp1252.Comparer.Compare(a, b));
            Assert.Equal(-order, Cp1252.Compare(b, a));
            if (order <= 0 && Cp1252.Compare(b, c) <= 0)
            {
                Assert.True(Cp1252.Compare(a, c) <= 0);
            }
        }
        string[] values = ["\u0080", "?", "\u20ac", "\u00ff", "\u2603", "\u0081", "\ud800", "\0"];
        Assert.Equal(actual: values.Order(Cp1252.Comparer).ToArray(), expected: ["\0", "?", "\u20ac", "\u0081", "\u00ff", "\u0080", "\u2603", "\ud800"]);
    }
}
