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
            () => DelphiStrings.Copy(null, 1, 1), () => DelphiStrings.Delete(null, 1, 1),
            () => DelphiStrings.Insert(null, "", 1), () => DelphiStrings.Insert("", null, 1),
            () =>
            {
                string value = null!;
                DelphiStrings.Delete(ref value, 1, 1);
            },
            () =>
            {
                string value = null!;
                DelphiStrings.Insert("", ref value, 1);
            },
            () =>
            {
                string value = "";
                DelphiStrings.Insert(null, ref value, 1);
            },
            () =>
            {
                string value = null!;
                DelphiStrings.SetLength(ref value, 1);
            },
            () => DelphiStrings.Pos(null, ""), () => DelphiStrings.Pos("", null),
            () => DelphiStrings.AnsiPos(null, ""), () => DelphiStrings.AnsiPos("", null),
            () => DelphiStrings.Resized(null, 1), () => PCharStrings.NullTerminatedText(null),
            () => DelphiTextCompare.UpperCase(null), () => DelphiTextCompare.LowerCase(null),
            () => DelphiTextCompare.AnsiUpperCase(null), () => DelphiTextCompare.AnsiLowerCase(null),
            () => DelphiTextCompare.CompareStr(null, ""), () => DelphiTextCompare.CompareStr("", null),
            () => DelphiTextCompare.CompareText(null, ""), () => DelphiTextCompare.CompareText("", null),
            () => DelphiTextCompare.AnsiCompareStr(null, ""), () => DelphiTextCompare.AnsiCompareStr("", null),
            () => DelphiTextCompare.AnsiCompareText(null, ""), () => DelphiTextCompare.AnsiCompareText("", null),
            () => DelphiStrings.Trim(null), () => DelphiStrings.TrimLeft(null), () => DelphiStrings.TrimRight(null),
            () => DelphiStrings.QuotedStr(null), () => DelphiConversions.StrToIntDef(null, 1),
            () => DelphiFormat.Format(null), () => DelphiFormat.Format("%s", (string?)null),
            () => DelphiAssert.Assert(true, null, "file.pas", 1),
            () => DelphiPaths.ExtractFileName(null), () => DelphiPaths.ExtractFileDrive(null),
            () => DelphiPaths.ExtractFilePath(null), () => DelphiPaths.ExtractFileDir(null),
            () => DelphiPaths.ExtractFileExt(null), () => DelphiPaths.ExpandFileName(null),
            () => DelphiPaths.ChangeFileExt(null, ""), () => DelphiPaths.ChangeFileExt("", null)
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
            _ = Assert.Throws<NullStringException>(() => DelphiStrings.WithChar(null, 1, 'x'));
            _ = Assert.Throws<NullStringException>(() => DelphiConversions.StrToInt(null));
            _ = Assert.Throws<NullStringException>(() => DelphiAssert.Assert(false, null, "file.pas", 1));
        }
        else
        {
            _ = Assert.Throws<IndexOutOfRangeException>(() => DelphiStrings.WithChar(null, 1, 'x'));
            _ = Assert.Throws<FormatException>(() => DelphiConversions.StrToInt(null));
            _ = Assert.Throws<AssertionFailedException>(() => DelphiAssert.Assert(false, null, "file.pas", 1));
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
        Assert.Equal("", "");
    }
}
