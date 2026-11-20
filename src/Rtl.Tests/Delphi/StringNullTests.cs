using System.Runtime.CompilerServices;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

internal sealed class StringMode : IDisposable
{
    private readonly bool _previous = DelphiStrings.StrictStrings;

    public StringMode(bool strict) => DelphiStrings.StrictStrings = strict;
    public void Dispose() => DelphiStrings.StrictStrings = _previous;

    [ModuleInitializer]
    internal static void Initialize()
    {
        ProcessSafety.Initialize();
        DelphiStrings.StrictStrings = true;
    }
}

public sealed class StringNullTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void EveryTextRoutineReadsTheNullSwitch(bool strict)
    {
        using StringMode mode = new(strict);
        Action[] reads = [
            () => Cp1252.Encode(null), () => Cp1252.Normalize(null),
            () => Cp1252.Compare(null, ""), () => Cp1252.Compare("", null),
            () => Cp1252.Comparer.Compare(null, ""), () => Cp1252.Comparer.Compare("", null),
            () => { Assert.Equal(actual: Cp1252.Normalize(null), expected: ""); },
            () =>
            {
                FormatArg value = (string?)null;
                Assert.True(value is string text && text.Length == 0);
            },
            () => DelphiStrings.Length(null), () => DelphiStrings.IsEmpty(null),
            () => DelphiStrings.Copy(null, index: 1, count: 1), () => DelphiStrings.Delete(null, index: 1, count: 1),
            () => DelphiStrings.Insert(null, value: "", index: 1), () => DelphiStrings.Insert("", value: null, index: 1),
            () =>
            {
                string value = null!;
                DelphiStrings.Delete(ref value, index: 1, count: 1);
            },
            () =>
            {
                string value = null!;
                DelphiStrings.Insert("", ref value, index: 1);
            },
            () =>
            {
                string value = "";
                DelphiStrings.Insert(null, ref value, index: 1);
            },
            () =>
            {
                string value = null!;
                DelphiStrings.SetLength(ref value, length: 1);
            },
            () => DelphiStrings.Pos(null, haystack: ""), () => DelphiStrings.Pos("", haystack: null),
            () => DelphiStrings.Pos(null, haystack: (string?)""), () => DelphiStrings.Pos((string?)"", haystack: null),
            () => DelphiStrings.Resized(null, length: 1), () => PCharStrings.NullTerminatedText(null),
            () => DelphiTextCompare.UpperCase(null), () => DelphiTextCompare.LowerCase(null),
            () => DelphiTextCompare.AnsiUpperCase(null), () => DelphiTextCompare.AnsiLowerCase(null),
            () => DelphiTextCompare.CompareStr(null, ""), () => DelphiTextCompare.CompareStr("", null),
            () => DelphiTextCompare.CompareText(null, ""), () => DelphiTextCompare.CompareText("", null),
            () => DelphiTextCompare.AnsiCompareStr(null, ""), () => DelphiTextCompare.AnsiCompareStr("", null),
            () => DelphiTextCompare.AnsiCompareText(null, ""), () => DelphiTextCompare.AnsiCompareText("", null),
            () => DelphiStrings.TrimLeft(DelphiStrings.TrimRight(null)), () => DelphiStrings.TrimLeft(null), () => DelphiStrings.TrimRight(null),
            () => DelphiStrings.QuotedStr(null), () => DelphiConversions.StrToIntDef(null, fallback: 1),
            () => DelphiFormat.Format(null), () => DelphiFormat.Format("%s", (string?)null),
            () => DelphiAssert.Assert(true, message: null, file: "file.pas", line: 1),
            () => DelphiPaths.ExtractFileName(null), () => DelphiPaths.ExtractFileDrive(null),
            () => DelphiPaths.ExtractFilePath(null), () => DelphiPaths.ExtractFileDir(null),
            () => DelphiPaths.ExtractFileExt(null), () => DelphiPaths.ExpandFileName(null),
            () => DelphiPaths.ChangeFileExt(null, extension: ""), () => DelphiPaths.ChangeFileExt("", extension: null)
        ];
        foreach (Action read in reads)
        {
            if (strict)
            {
                _ = Assert.Throws<NullStringException>(read);
            }
            else
            {
                read();
            }
        }
        if (strict)
        {
            _ = Assert.Throws<NullStringException>(() => DelphiStrings.WithChar(null, index: 1, character: 'x'));
            _ = Assert.Throws<NullStringException>(() => DelphiConversions.StrToInt(null));
            _ = Assert.Throws<NullStringException>(() => DelphiAssert.Assert(false, message: null, file: "file.pas", line: 1));
        }
        else
        {
            _ = Assert.Throws<IndexOutOfRangeException>(() => DelphiStrings.WithChar(null, index: 1, character: 'x'));
            _ = Assert.Throws<FormatException>(() => DelphiConversions.StrToInt(null));
            _ = Assert.Throws<AssertionFailedException>(() => DelphiAssert.Assert(false, message: null, file: "file.pas", line: 1));
            Assert.Empty(Cp1252.Encode(null));
            Assert.Equal("", Cp1252.Normalize(null));
            Assert.True(DelphiStrings.IsEmpty(null));
        }
        Assert.True(DelphiStrings.IsEmpty(""));
        Assert.False(DelphiStrings.IsEmpty("\0"));
        Assert.False(DelphiStrings.IsEmpty("x"));
        Assert.Empty(Cp1252.Encode(""));
        Assert.Equal("", Cp1252.Normalize(""));
        Assert.Equal(0, Cp1252.Compare("", ""));
    }
}
