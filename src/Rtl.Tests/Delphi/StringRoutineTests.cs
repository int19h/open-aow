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
            var ansi = Ansi(text);
            foreach (var (index, count) in new[] {
                (1, 2), (3, 99), (0, 2), (-2, 2), (-1, 2), (5, 1), (5, 2),
                (1, 0), (1, -1), (2, 0), (2, -1), (int.MinValue, 2),
                (int.MaxValue, int.MaxValue), (1, int.MaxValue) })
            {
                Assert.Equal(RtlUnit.Copy(ansi, index, count), RtlUnit.Copy(text, index, count));
                string changed = text;
                var changedAnsi = ansi;
                RtlUnit.Delete(ref changed, index, count);
                RtlUnit.Delete(ref changedAnsi, index, count);
                Assert.Equal(changedAnsi, changed);
                Assert.Equal(RtlUnit.Delete(ansi, index, count), RtlUnit.Delete(text, index, count));
            }
            foreach (int index in new[] { 1, 3, 5, 99, 0, -1, int.MinValue, int.MaxValue })
            {
                foreach (string source in new[] { "XY", "", text, "\u20ac\0" })
                {
                    string changed = text;
                    var changedAnsi = ansi;
                    RtlUnit.Insert(source, ref changed, index);
                    RtlUnit.Insert(Ansi(source), ref changedAnsi, index);
                    Assert.Equal(changedAnsi, changed);
                    Assert.Equal(RtlUnit.Insert(Ansi(source), ansi, index), RtlUnit.Insert(source, text, index));
                }
            }
        }
        string self = "abcd";
        RtlUnit.Insert(self, ref self, 3);
        Assert.Equal("ababcdcd", self);
    }

    [Fact]
    public void PosAndAnsiPosMatchProbeInputs()
    {
        foreach (var (needle, text) in new[] {
            ("ab", "abcd"), ("cd", "abcd"), ("x", "abcd"), ("", "abcd"),
            ("a", ""), ("", ""), ("aa", "aaaa"), ("abcde", "abcd"),
            ("\0b", "a\0b"), ("\u20ac", "a\u20ac\u00ff"), ("\u00ff", "\u20ac") })
        {
            Assert.Equal(RtlUnit.Pos(Ansi(needle), Ansi(text)), RtlUnit.Pos(needle, text));
            Assert.Equal(RtlUnit.AnsiPos(Ansi(needle), Ansi(text)), RtlUnit.AnsiPos(needle, text));
        }
    }

    [Fact]
    public void StringsResizeAndWithCharKeepIndependentValuesAndOneBasedIndices()
    {
        var strings = RtlUnit.Strings(4);
        Assert.Equal(new[] { "", "", "", "" }, strings);
        strings[1] = "one";
        Assert.Equal("", strings[0]);
        Assert.Empty(RtlUnit.Strings(0));
        foreach (string text in new[] { "abc", "a\0\u20ac\u00ffz", "" })
        {
            var ansi = Ansi(text);
            Assert.Equal(RtlUnit.Length(ansi), RtlUnit.Length(text));
            for (int i = 1; i <= text.Length; i++)
                foreach (byte b in new byte[] { 0, 88, 128, 255 })
                    Assert.Equal(RtlUnit.WithChar(ansi, i, Cp1252.Char(b)), RtlUnit.WithChar(text, i, Cp1252.Char(b)));
            foreach (int length in new[] { 0, 1, 3, 5, 8 })
            {
                Assert.Equal(RtlUnit.Resized(ansi, length), RtlUnit.Resized(text, length));
                string resized = text;
                var resizedAnsi = ansi;
                RtlUnit.SetLength(ref resized, length);
                RtlUnit.SetLength(ref resizedAnsi, length);
                Assert.Equal(resizedAnsi, resized);
                Assert.Equal(text, ansi);
            }
            foreach (int index in new[] { int.MinValue, -1, 0, text.Length + 1, int.MaxValue })
                Assert.Throws<IndexOutOfRangeException>(() => RtlUnit.WithChar(text, index, 'X'));
            Assert.Throws<ArgumentOutOfRangeException>(() => RtlUnit.Resized(text, -1));
            string invalid = text;
            Assert.Throws<ArgumentOutOfRangeException>(() => RtlUnit.SetLength(ref invalid, -1));
            Assert.Equal(text, invalid);
        }
        string original = "abc";
        Assert.Equal("aXc", RtlUnit.WithChar(original, 2, 'X'));
        Assert.Equal("abc", original);
        Assert.Equal("abc\0\0", RtlUnit.Resized(original, 5));
    }

    [Fact]
    public void ComparisonsMatchAnsiRoutinesOverEveryBytePairAndProbeStrings()
    {
        for (int a = 0; a < 256; a++)
        {
            for (int b = 0; b < 256; b++)
            {
                var first = Cp1252.Text((byte)a);
                var second = Cp1252.Text((byte)b);
                string x = Cp1252.Text((byte)a), y = Cp1252.Text((byte)b);
                Assert.Equal(RtlUnit.CompareStr(first, second), RtlUnit.CompareStr(x, y));
                Assert.Equal(RtlUnit.CompareText(first, second), RtlUnit.CompareText(x, y));
                Assert.Equal(RtlUnit.AnsiCompareStr(first, second), RtlUnit.AnsiCompareStr(x, y));
                Assert.Equal(RtlUnit.AnsiCompareText(first, second), RtlUnit.AnsiCompareText(x, y));
            }
        }
        foreach (var (a, b) in new[] {
            ("abc", "ABC"), ("ab", "ABC"), ("\u00e0", "\u00c0"), ("", "a"),
            ("a\0b", "A\0c"), ("a", "abc"), ("\u00c0\u0160\u0152\u0178", "\u00e0\u0161\u0153\u00ff") })
        {
            Assert.Equal(RtlUnit.CompareStr(Ansi(a), Ansi(b)), RtlUnit.CompareStr(a, b));
            Assert.Equal(RtlUnit.CompareText(Ansi(a), Ansi(b)), RtlUnit.CompareText(a, b));
            Assert.Equal(RtlUnit.AnsiCompareStr(Ansi(a), Ansi(b)), RtlUnit.AnsiCompareStr(a, b));
            Assert.Equal(RtlUnit.AnsiCompareText(Ansi(a), Ansi(b)), RtlUnit.AnsiCompareText(a, b));
        }
    }

    [Fact]
    public void TrimQuoteAndStringOfCharMatchAnsiRoutines()
    {
        foreach (string text in new[] { "  abc  ", "\0\t\n\rabc\x1f ", "\0\t ", "", "\u00a0abc\u00a0", " a b ", "a'b\0c", "'", "\u20ac" })
        {
            Assert.Equal(RtlUnit.Trim(Ansi(text)), RtlUnit.Trim(text));
            Assert.Equal(RtlUnit.TrimLeft(Ansi(text)), RtlUnit.TrimLeft(text));
            Assert.Equal(RtlUnit.TrimRight(Ansi(text)), RtlUnit.TrimRight(text));
            Assert.Equal(RtlUnit.QuotedStr(Ansi(text)), RtlUnit.QuotedStr(text));
        }
        for (int b = 0; b < 256; b++)
            foreach (int count in new[] { -1, 0, 1, 3 })
                Assert.Equal(RtlUnit.StringOfChar((byte)b, count), RtlUnit.StringOfChar(Cp1252.Char((byte)b), count));
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
            Assert.Equal(RtlUnit.StrToIntDef(Ansi(text), 91), RtlUnit.StrToIntDef(text, 91));
            var error = Record.Exception(() => RtlUnit.StrToInt(Ansi(text)));
            if (error is null) Assert.Equal(RtlUnit.StrToInt(Ansi(text)), RtlUnit.StrToInt(text));
            else Assert.Equal(Assert.IsType<FormatException>(error).Message,
                Assert.Throws<FormatException>(() => RtlUnit.StrToInt(text)).Message);
        }
    }

    [Fact]
    public void PointerAndNullTerminatedBoundariesMatchAnsiRoutines()
    {
        foreach (string text in new[] { "", "abc", "a\0b", "\u20ac\u0081\0\u00ff" })
        {
            var ansi = Ansi(text);
            var pointer = Ptr<byte>.Unbox(ansi);
            Assert.Equal(RtlUnit.StrPas(pointer), RtlUnit.FromPChar(pointer));
            Assert.Equal(RtlUnit.NullTerminatedText(ansi), RtlUnit.NullTerminatedText(text));
            foreach (int length in new[] { -1, 0, text.Length })
            {
                string value = "old";
                RtlUnit.SetString(ref value, pointer, length);
                Assert.Equal(RtlUnit.SetString(pointer, length), value);
            }
        }
        Assert.Equal("", RtlUnit.FromPChar(default));
        foreach (int length in new[] { -1, 0, 3 })
        {
            string value = "old";
            RtlUnit.SetString(ref value, default, length);
            Assert.Equal(RtlUnit.SetString(default, length), value);
        }
    }

    [Fact]
    public void NullArgumentsBehaveAsEmptyText()
    {
        using var mode = new StringMode(false);
        Assert.Equal(0, RtlUnit.Length((string?)null));
        Assert.Equal("", RtlUnit.Copy((string?)null, 1, 3));
        Assert.Equal("", RtlUnit.Delete((string?)null, 1, 3));
        Assert.Equal("x", RtlUnit.Insert("x", (string?)null, 1));
        Assert.Equal("x", RtlUnit.Insert((string?)null, "x", 1));
        Assert.Equal(0, RtlUnit.Pos((string?)null, "x"));
        Assert.Equal(0, RtlUnit.Pos("x", (string?)null));
        Assert.Equal(0, RtlUnit.AnsiPos((string?)null, null));
        Assert.Equal("\0\0", RtlUnit.Resized(null, 2));
        Assert.Throws<IndexOutOfRangeException>(() => RtlUnit.WithChar(null, 1, 'x'));
        string value = null!;
        RtlUnit.Delete(ref value, 1, 3);
        Assert.Equal("", value);
        value = null!;
        RtlUnit.Insert(null, ref value, 1);
        Assert.Equal("", value);
        value = null!;
        RtlUnit.SetLength(ref value, 2);
        Assert.Equal("\0\0", value);
        foreach (Func<string?, string> function in new Func<string?, string>[] {
            RtlUnit.UpperCase, RtlUnit.LowerCase, RtlUnit.AnsiUpperCase, RtlUnit.AnsiLowerCase,
            RtlUnit.Trim, RtlUnit.TrimLeft, RtlUnit.TrimRight, RtlUnit.NullTerminatedText }) Assert.Equal("", function(null));
        foreach (Func<string?, string?, int> function in new Func<string?, string?, int>[] {
            RtlUnit.CompareStr, RtlUnit.CompareText, RtlUnit.AnsiCompareStr, RtlUnit.AnsiCompareText })
        {
            Assert.Equal(0, function(null, ""));
            Assert.Equal(0, function("", null));
        }
        Assert.Equal("''", RtlUnit.QuotedStr((string?)null));
        Assert.Equal(91, RtlUnit.StrToIntDef((string?)null, 91));
        Assert.Throws<FormatException>(() => RtlUnit.StrToInt((string?)null));
        Assert.Equal("", RtlUnit.Format((string?)null));
        Assert.Equal("", RtlUnit.Format("%s", (string?)null));
    }

    [Fact]
    public void AssertionMessagesMatchAnsiRoutine()
    {
        using var mode = new StringMode(false);
        foreach (string text in new[] { "", "broken", "\u20ac" })
        {
            RtlUnit.Assert(true, text, "unit.pas", 42);
            var expected = Assert.Throws<AssertionFailedException>(() => RtlUnit.Assert(false, Ansi(text), "unit.pas", 42));
            var actual = Assert.Throws<AssertionFailedException>(() => RtlUnit.Assert(false, text, "unit.pas", 42));
            Assert.Equal(expected.Message, actual.Message);
        }
        Assert.Equal("Assertion failed (unit.pas, line 1)",
            Assert.Throws<AssertionFailedException>(() => RtlUnit.Assert(false, (string?)null, "unit.pas", 1)).Message);
    }
}
