using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class StringRoutineTests
{
    private static string Ansi(string? text) => Cp1252.Decode(Cp1252.Encode(text));

    [Fact]
    public void CopyDeleteAndInsertMatchProbeInputsAndExtremeIndices()
    {
        foreach (string text in new[] { "abcd", "", "a\0\u20ac\u00ffz" })
        {
            string ansi = Ansi(text);
            foreach ((int index, int count) in new[] {
                (1, 2), (3, 99), (0, 2), (-2, 2), (-1, 2), (5, 1), (5, 2),
                (1, 0), (1, -1), (2, 0), (2, -1), (int.MinValue, 2),
                (int.MaxValue, int.MaxValue), (1, int.MaxValue) })
            {
                Assert.Equal(DelphiStrings.Copy(ansi, index, count), DelphiStrings.Copy(text, index, count));
                string changed = text;
                string changedAnsi = ansi;
                DelphiStrings.Delete(ref changed, index, count);
                DelphiStrings.Delete(ref changedAnsi, index, count);
                Assert.Equal(changedAnsi, changed);
                Assert.Equal(DelphiStrings.Delete(ansi, index, count), DelphiStrings.Delete(text, index, count));
            }
            foreach (int index in new[] { 1, 3, 5, 99, 0, -1, int.MinValue, int.MaxValue })
            {
                foreach (string source in new[] { "XY", "", text, "\u20ac\0" })
                {
                    string changed = text;
                    string changedAnsi = ansi;
                    DelphiStrings.Insert(source, value: ref changed, index);
                    DelphiStrings.Insert(Ansi(source), value: ref changedAnsi, index);
                    Assert.Equal(changedAnsi, changed);
                    Assert.Equal(DelphiStrings.Insert(Ansi(source), value: ansi, index), DelphiStrings.Insert(source, value: text, index));
                }
            }
        }
        string self = "abcd";
        DelphiStrings.Insert(self, value: ref self, index: 3);
        Assert.Equal("ababcdcd", self);
    }

    [Fact]
    public void PositionAndAnsiPositionMatchProbeInputs()
    {
        foreach ((string? needle, string? text) in new[] {
            ("ab", "abcd"), ("cd", "abcd"), ("x", "abcd"), ("", "abcd"),
            ("a", ""), ("", ""), ("aa", "aaaa"), ("abcde", "abcd"),
            ("\0b", "a\0b"), ("\u20ac", "a\u20ac\u00ff"), ("\u00ff", "\u20ac") })
        {
            Assert.Equal(DelphiStrings.Pos(Ansi(needle), haystack: Ansi(text)), DelphiStrings.Pos(needle, haystack: text));
            Assert.Equal(DelphiStrings.AnsiPos(Ansi(needle), value: Ansi(text)), DelphiStrings.AnsiPos(needle, value: text));
        }
    }

    [Fact]
    public void StringsResizeAndWithCharKeepIndependentValuesAndOneBasedIndices()
    {
        foreach (string text in new[] { "abc", "a\0\u20ac\u00ffz", "" })
        {
            string ansi = Ansi(text);
            Assert.Equal(DelphiStrings.Length(ansi), DelphiStrings.Length(text));
            for (int i = 1; i <= text.Length; i++)
            {
                foreach (byte b in new byte[] { 0, 88, 128, 255 })
                {
                    Assert.Equal(DelphiStrings.WithChar(ansi, index: i, character: Cp1252.Char(b)), DelphiStrings.WithChar(text, index: i, character: Cp1252.Char(b)));
                }
            }

            foreach (int length in new[] { 0, 1, 3, 5, 8 })
            {
                Assert.Equal(DelphiStrings.Resized(ansi, length), DelphiStrings.Resized(text, length));
                string resized = text;
                string resizedAnsi = ansi;
                DelphiStrings.SetLength(ref resized, length);
                DelphiStrings.SetLength(ref resizedAnsi, length);
                Assert.Equal(resizedAnsi, resized);
                Assert.Equal(text, ansi);
            }
            foreach (int index in new[] { int.MinValue, -1, 0, text.Length + 1, int.MaxValue })
            {
                _ = Assert.Throws<IndexOutOfRangeException>(() => DelphiStrings.WithChar(text, index, character: 'X'));
            }

            _ = Assert.Throws<ArgumentOutOfRangeException>(() => DelphiStrings.Resized(text, length: -1));
            string invalid = text;
            _ = Assert.Throws<ArgumentOutOfRangeException>(() => DelphiStrings.SetLength(ref invalid, length: -1));
            Assert.Equal(text, invalid);
        }
        string original = "abc";
        Assert.Equal("aXc", DelphiStrings.WithChar(original, index: 2, character: 'X'));
        Assert.Equal("abc", original);
        Assert.Equal("abc\0\0", DelphiStrings.Resized(original, length: 5));
    }

    [Fact]
    public void ComparisonsMatchAnsiRoutinesOverEveryBytePairAndProbeStrings()
    {
        for (int a = 0; a < 256; a++)
        {
            for (int b = 0; b < 256; b++)
            {
                string first = Cp1252.Text((byte)a);
                string second = Cp1252.Text((byte)b);
                string x = Cp1252.Text((byte)a), y = Cp1252.Text((byte)b);
                Assert.Equal(DelphiTextCompare.CompareStr(first, second), DelphiTextCompare.CompareStr(x, y));
                Assert.Equal(DelphiTextCompare.CompareText(first, second), DelphiTextCompare.CompareText(x, y));
                Assert.Equal(DelphiTextCompare.AnsiCompareStr(first, second), DelphiTextCompare.AnsiCompareStr(x, y));
                Assert.Equal(DelphiTextCompare.AnsiCompareText(first, second), DelphiTextCompare.AnsiCompareText(x, y));
            }
        }
        foreach ((string? a, string? b) in new[] {
            ("abc", "ABC"), ("ab", "ABC"), ("\u00e0", "\u00c0"), ("", "a"),
            ("a\0b", "A\0c"), ("a", "abc"), ("\u00c0\u0160\u0152\u0178", "\u00e0\u0161\u0153\u00ff") })
        {
            Assert.Equal(DelphiTextCompare.CompareStr(Ansi(a), Ansi(b)), DelphiTextCompare.CompareStr(a, b));
            Assert.Equal(DelphiTextCompare.CompareText(Ansi(a), Ansi(b)), DelphiTextCompare.CompareText(a, b));
            Assert.Equal(DelphiTextCompare.AnsiCompareStr(Ansi(a), Ansi(b)), DelphiTextCompare.AnsiCompareStr(a, b));
            Assert.Equal(DelphiTextCompare.AnsiCompareText(Ansi(a), Ansi(b)), DelphiTextCompare.AnsiCompareText(a, b));
        }
    }

    [Fact]
    public void TrimQuoteAndStringOfCharMatchAnsiRoutines()
    {
        foreach (string text in new[] { "  abc  ", "\0\t\n\rabc\x1f ", "\0\t ", "", "\u00a0abc\u00a0", " a b ", "a'b\0c", "'", "\u20ac" })
        {
            Assert.Equal(DelphiStrings.Trim(Ansi(text)), DelphiStrings.Trim(text));
            Assert.Equal(DelphiStrings.TrimLeft(Ansi(text)), DelphiStrings.TrimLeft(text));
            Assert.Equal(DelphiStrings.TrimRight(Ansi(text)), DelphiStrings.TrimRight(text));
            Assert.Equal(DelphiStrings.QuotedStr(Ansi(text)), DelphiStrings.QuotedStr(text));
        }
        for (int b = 0; b < 256; b++)
        {
            foreach (int count in new[] { -1, 0, 1, 3 })
            {
                Assert.Equal(DelphiStrings.StringOfChar((byte)b, count), DelphiStrings.StringOfChar(Cp1252.Char((byte)b), count));
            }
        }
    }

    [Fact]
    public void IntegerParsingMatchesEveryProbeInput()
    {
        foreach (string text in new[] {
            "0", "-123", "+123", "2147483647", "-2147483648", "$FF", "0xFF", "-$10",
            "$FFFFFFFF", " 12", "12 ", "\t12", "", "  ", "12x", "2147483648",
            "-2147483649", "+", "$", "--1", "1.0", "12\0" + "3", "+$1", "-$1",
            "$100000000", "$G", "1\n", "\0" + "1", "  +42", "-0", "$80000000",
            "$abcdef", "00000000", "0002147483647", "12\0junk", "$fF\0junk", "-2147483647" })
        {
            Assert.Equal(DelphiConversions.StrToIntDef(Ansi(text), fallback: 91), DelphiConversions.StrToIntDef(text, fallback: 91));
            Exception? error = Record.Exception(() => DelphiConversions.StrToInt(Ansi(text)));
            if (error is null)
            {
                Assert.Equal(DelphiConversions.StrToInt(Ansi(text)), DelphiConversions.StrToInt(text));
            }
            else
            {
                Assert.Equal(Assert.IsType<FormatException>(error).Message,
                Assert.Throws<FormatException>(() => DelphiConversions.StrToInt(text)).Message);
            }
        }
    }

    [Fact]
    public void PointerAndNullTerminatedBoundariesMatchAnsiRoutines()
    {
        foreach (string text in new[] { "", "abc", "a\0b", "\u20ac\u0081\0\u00ff" })
        {
            string ansi = Ansi(text);
            Ptr<byte> pointer = Ptr<byte>.Unbox(ansi);
            Assert.Equal(PCharStrings.StrPas(pointer), PCharStrings.FromPChar(pointer));
            Assert.Equal(PCharStrings.NullTerminatedText(ansi), PCharStrings.NullTerminatedText(text));
            foreach (int length in new[] { -1, 0, text.Length })
            {
                PCharStrings.SetString(out string value, source: pointer, length);
                Assert.Equal(PCharStrings.SetString(pointer, length), value);
            }
        }
        Assert.Equal("", PCharStrings.FromPChar(default));
        foreach (int length in new[] { -1, 0, 3 })
        {
            PCharStrings.SetString(out string value, source: default, length);
            Assert.Equal(PCharStrings.SetString(default, length), value);
        }
    }

    [Fact]
    public void NullArgumentsBehaveAsEmptyText()
    {
        using StringMode mode = new(false);
        Assert.Equal(0, DelphiStrings.Length(null));
        Assert.Equal("", DelphiStrings.Copy(null, index: 1, count: 3));
        Assert.Equal("", DelphiStrings.Delete(null, index: 1, count: 3));
        Assert.Equal("x", DelphiStrings.Insert("x", value: null, index: 1));
        Assert.Equal("x", DelphiStrings.Insert(null, value: "x", index: 1));
        Assert.Equal(0, DelphiStrings.Pos(null, haystack: "x"));
        Assert.Equal(0, DelphiStrings.Pos("x", haystack: null));
        Assert.Equal(0, DelphiStrings.AnsiPos(null, value: null));
        Assert.Equal("\0\0", DelphiStrings.Resized(null, length: 2));
        _ = Assert.Throws<IndexOutOfRangeException>(() => DelphiStrings.WithChar(null, index: 1, character: 'x'));
        string value = null!;
        DelphiStrings.Delete(ref value, index: 1, count: 3);
        Assert.Equal("", value);
        value = null!;
        DelphiStrings.Insert(null, ref value, index: 1);
        Assert.Equal("", value);
        value = null!;
        DelphiStrings.SetLength(ref value, length: 2);
        Assert.Equal("\0\0", value);
        foreach (Func<string?, string> function in new Func<string?, string>[] {
            DelphiTextCompare.UpperCase, DelphiTextCompare.LowerCase, DelphiTextCompare.AnsiUpperCase, DelphiTextCompare.AnsiLowerCase,
            DelphiStrings.Trim, DelphiStrings.TrimLeft, DelphiStrings.TrimRight, PCharStrings.NullTerminatedText })
        {
            Assert.Equal("", function(null));
        }

        foreach (Func<string?, string?, int> function in new Func<string?, string?, int>[] {
            DelphiTextCompare.CompareStr, DelphiTextCompare.CompareText, DelphiTextCompare.AnsiCompareStr, DelphiTextCompare.AnsiCompareText })
        {
            Assert.Equal(0, function(null, ""));
            Assert.Equal(0, function("", null));
        }
        Assert.Equal("''", DelphiStrings.QuotedStr(null));
        Assert.Equal(91, DelphiConversions.StrToIntDef(null, fallback: 91));
        _ = Assert.Throws<FormatException>(() => DelphiConversions.StrToInt(null));
        Assert.Equal("", DelphiFormat.Format(null));
        Assert.Equal("", DelphiFormat.Format("%s", (string?)null));
    }

    [Fact]
    public void AssertionMessagesMatchAnsiRoutine()
    {
        using StringMode mode = new(false);
        foreach (string text in new[] { "", "broken", "\u20ac" })
        {
            DelphiAssert.Assert(true, message: text, file: "unit.pas", line: 42);
            AssertionFailedException expected = Assert.Throws<AssertionFailedException>(() => DelphiAssert.Assert(false, message: Ansi(text), file: "unit.pas", line: 42));
            AssertionFailedException actual = Assert.Throws<AssertionFailedException>(() => DelphiAssert.Assert(false, message: text, file: "unit.pas", line: 42));
            Assert.Equal(expected.Message, actual.Message);
        }
        Assert.Equal("Assertion failed (unit.pas, line 1)",
            Assert.Throws<AssertionFailedException>(() => DelphiAssert.Assert(false, message: null, file: "unit.pas", line: 1)).Message);
    }
}
