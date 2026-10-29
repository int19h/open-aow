using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class StringRoutineTests
{
    private static AnsiString Ansi(string? text) => AnsiString.FromBytes(Cp1252.Encode(text));

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
                Assert.Equal(Rtl.Copy(ansi, index, count).ToString(), Rtl.Copy(text, index, count));
                string changed = text;
                var changedAnsi = ansi;
                Rtl.Delete(ref changed, index, count);
                Rtl.Delete(ref changedAnsi, index, count);
                Assert.Equal(changedAnsi.ToString(), changed);
                Assert.Equal(Rtl.Delete(ansi, index, count).ToString(), Rtl.Delete(text, index, count));
            }
            foreach (int index in new[] { 1, 3, 5, 99, 0, -1, int.MinValue, int.MaxValue })
            {
                foreach (string source in new[] { "XY", "", text, "\u20ac\0" })
                {
                    string changed = text;
                    var changedAnsi = ansi;
                    Rtl.Insert(source, ref changed, index);
                    Rtl.Insert(Ansi(source), ref changedAnsi, index);
                    Assert.Equal(changedAnsi.ToString(), changed);
                    Assert.Equal(Rtl.Insert(Ansi(source), ansi, index).ToString(), Rtl.Insert(source, text, index));
                }
            }
        }
        string self = "abcd";
        Rtl.Insert(self, ref self, 3);
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
            Assert.Equal(Rtl.Pos(Ansi(needle), Ansi(text)), Rtl.Pos(needle, text));
            Assert.Equal(Rtl.AnsiPos(Ansi(needle), Ansi(text)), Rtl.AnsiPos(needle, text));
        }
    }

    [Fact]
    public void StringsResizeAndWithCharKeepIndependentValuesAndOneBasedIndices()
    {
        var strings = Rtl.Strings(4);
        Assert.Equal(new[] { "", "", "", "" }, strings);
        strings[1] = "one";
        Assert.Equal("", strings[0]);
        Assert.Empty(Rtl.Strings(0));
        foreach (string text in new[] { "abc", "a\0\u20ac\u00ffz", "" })
        {
            var ansi = Ansi(text);
            Assert.Equal(Rtl.Length(ansi), Rtl.Length(text));
            for (int i = 1; i <= text.Length; i++)
                foreach (byte b in new byte[] { 0, 88, 128, 255 })
                    Assert.Equal(ansi.WithChar(i, b).ToString(), Rtl.WithChar(text, i, Cp1252.Char(b)));
            foreach (int length in new[] { 0, 1, 3, 5, 8 })
            {
                Assert.Equal(ansi.Resized(length).ToString(), Rtl.Resized(text, length));
                string resized = text;
                var resizedAnsi = ansi;
                Rtl.SetLength(ref resized, length);
                Rtl.SetLength(ref resizedAnsi, length);
                Assert.Equal(resizedAnsi.ToString(), resized);
                Assert.Equal(text, ansi.ToString());
            }
            foreach (int index in new[] { int.MinValue, -1, 0, text.Length + 1, int.MaxValue })
                Assert.Throws<ERangeError>(() => Rtl.WithChar(text, index, 'X'));
            Assert.Throws<ERangeError>(() => Rtl.Resized(text, -1));
            string invalid = text;
            Assert.Throws<ERangeError>(() => Rtl.SetLength(ref invalid, -1));
            Assert.Equal(text, invalid);
        }
        string original = "abc";
        Assert.Equal("aXc", Rtl.WithChar(original, 2, 'X'));
        Assert.Equal("abc", original);
        Assert.Equal("abc\0\0", Rtl.Resized(original, 5));
    }

    [Fact]
    public void ComparisonsMatchAnsiRoutinesOverEveryBytePairAndProbeStrings()
    {
        for (int a = 0; a < 256; a++)
        {
            for (int b = 0; b < 256; b++)
            {
                var first = AnsiString.FromBytes((byte)a);
                var second = AnsiString.FromBytes((byte)b);
                string x = Cp1252.Text((byte)a), y = Cp1252.Text((byte)b);
                Assert.Equal(Rtl.CompareStr(first, second), Rtl.CompareStr(x, y));
                Assert.Equal(Rtl.CompareText(first, second), Rtl.CompareText(x, y));
                Assert.Equal(Rtl.AnsiCompareStr(first, second), Rtl.AnsiCompareStr(x, y));
                Assert.Equal(Rtl.AnsiCompareText(first, second), Rtl.AnsiCompareText(x, y));
            }
        }
        foreach (var (a, b) in new[] {
            ("abc", "ABC"), ("ab", "ABC"), ("\u00e0", "\u00c0"), ("", "a"),
            ("a\0b", "A\0c"), ("a", "abc"), ("\u00c0\u0160\u0152\u0178", "\u00e0\u0161\u0153\u00ff") })
        {
            Assert.Equal(Rtl.CompareStr(Ansi(a), Ansi(b)), Rtl.CompareStr(a, b));
            Assert.Equal(Rtl.CompareText(Ansi(a), Ansi(b)), Rtl.CompareText(a, b));
            Assert.Equal(Rtl.AnsiCompareStr(Ansi(a), Ansi(b)), Rtl.AnsiCompareStr(a, b));
            Assert.Equal(Rtl.AnsiCompareText(Ansi(a), Ansi(b)), Rtl.AnsiCompareText(a, b));
        }
    }

    [Fact]
    public void TrimQuoteAndStringOfCharMatchAnsiRoutines()
    {
        foreach (string text in new[] { "  abc  ", "\0\t\n\rabc\x1f ", "\0\t ", "", "\u00a0abc\u00a0", " a b ", "a'b\0c", "'", "\u20ac" })
        {
            Assert.Equal(Rtl.Trim(Ansi(text)).ToString(), Rtl.Trim(text));
            Assert.Equal(Rtl.TrimLeft(Ansi(text)).ToString(), Rtl.TrimLeft(text));
            Assert.Equal(Rtl.TrimRight(Ansi(text)).ToString(), Rtl.TrimRight(text));
            Assert.Equal(Rtl.QuotedStr(Ansi(text)).ToString(), Rtl.QuotedStr(text));
        }
        for (int b = 0; b < 256; b++)
            foreach (int count in new[] { -1, 0, 1, 3 })
                Assert.Equal(Rtl.StringOfChar((byte)b, count).ToString(), Rtl.StringOfChar(Cp1252.Char((byte)b), count));
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
            Assert.Equal(Rtl.StrToIntDef(Ansi(text), 91), Rtl.StrToIntDef(text, 91));
            var error = Record.Exception(() => Rtl.StrToInt(Ansi(text)));
            if (error is null) Assert.Equal(Rtl.StrToInt(Ansi(text)), Rtl.StrToInt(text));
            else Assert.Equal(Assert.IsType<EConvertError>(error).Message.ToString(),
                Assert.Throws<EConvertError>(() => Rtl.StrToInt(text)).Message.ToString());
        }
    }

    [Fact]
    public void PointerAndNullTerminatedBoundariesMatchAnsiRoutines()
    {
        foreach (string text in new[] { "", "abc", "a\0b", "\u20ac\u0081\0\u00ff" })
        {
            var ansi = Ansi(text);
            var pointer = ansi.AsPtr();
            Assert.Equal(Rtl.StrPas(pointer).ToString(), Rtl.FromPChar(pointer));
            Assert.Equal(Rtl.NullTerminatedText(ansi).ToString(), Rtl.NullTerminatedText(text));
            foreach (int length in new[] { -1, 0, text.Length })
            {
                string value = "old";
                Rtl.SetString(ref value, pointer, length);
                Assert.Equal(Rtl.SetString(pointer, length).ToString(), value);
            }
        }
        Assert.Equal("", Rtl.FromPChar(default));
        foreach (int length in new[] { -1, 0, 3 })
        {
            string value = "old";
            Rtl.SetString(ref value, default, length);
            Assert.Equal(Rtl.SetString(default, length).ToString(), value);
        }
    }

    [Fact]
    public void NullArgumentsBehaveAsEmptyText()
    {
        Assert.Equal(0, Rtl.Length((string?)null));
        Assert.Equal("", Rtl.Copy((string?)null, 1, 3));
        Assert.Equal("", Rtl.Delete((string?)null, 1, 3));
        Assert.Equal("x", Rtl.Insert("x", (string?)null, 1));
        Assert.Equal("x", Rtl.Insert((string?)null, "x", 1));
        Assert.Equal(0, Rtl.Pos((string?)null, "x"));
        Assert.Equal(0, Rtl.Pos("x", (string?)null));
        Assert.Equal(0, Rtl.AnsiPos((string?)null, null));
        Assert.Equal("\0\0", Rtl.Resized(null, 2));
        Assert.Throws<ERangeError>(() => Rtl.WithChar(null, 1, 'x'));
        string value = null!;
        Rtl.Delete(ref value, 1, 3);
        Assert.Equal("", value);
        value = null!;
        Rtl.Insert(null, ref value, 1);
        Assert.Equal("", value);
        value = null!;
        Rtl.SetLength(ref value, 2);
        Assert.Equal("\0\0", value);
        foreach (Func<string?, string> function in new Func<string?, string>[] {
            Rtl.UpperCase, Rtl.LowerCase, Rtl.AnsiUpperCase, Rtl.AnsiLowerCase,
            Rtl.Trim, Rtl.TrimLeft, Rtl.TrimRight, Rtl.NullTerminatedText }) Assert.Equal("", function(null));
        foreach (Func<string?, string?, int> function in new Func<string?, string?, int>[] {
            Rtl.CompareStr, Rtl.CompareText, Rtl.AnsiCompareStr, Rtl.AnsiCompareText })
        {
            Assert.Equal(0, function(null, ""));
            Assert.Equal(0, function("", null));
        }
        Assert.Equal("''", Rtl.QuotedStr((string?)null));
        Assert.Equal(91, Rtl.StrToIntDef((string?)null, 91));
        Assert.Throws<EConvertError>(() => Rtl.StrToInt((string?)null));
        Assert.Equal("", Rtl.Format((string?)null));
        Assert.Equal("", Rtl.Format("%s", (string?)null));
    }

    [Fact]
    public void AssertionMessagesMatchAnsiRoutine()
    {
        foreach (string text in new[] { "", "broken", "\u20ac" })
        {
            Rtl.Assert(true, text, "unit.pas", 42);
            var expected = Assert.Throws<EAssertionFailed>(() => Rtl.Assert(false, Ansi(text), "unit.pas", 42));
            var actual = Assert.Throws<EAssertionFailed>(() => Rtl.Assert(false, text, "unit.pas", 42));
            Assert.Equal(expected.Message.ToString(), actual.Message.ToString());
        }
        Assert.Equal("Assertion failed (unit.pas, line 1)",
            Assert.Throws<EAssertionFailed>(() => Rtl.Assert(false, (string?)null, "unit.pas", 1)).Message.ToString());
    }
}
