using System.Runtime.CompilerServices;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

internal sealed class StringMode : IDisposable
{
    private readonly bool _previous = RtlUnit.StrictStrings;

    public StringMode(bool strict) => RtlUnit.StrictStrings = strict;
    public void Dispose() => RtlUnit.StrictStrings = _previous;

    [ModuleInitializer]
    internal static void Initialize()
    {
        ProcessSafety.Initialize();
        RtlUnit.StrictStrings = true;
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
            () => { FormatArg value = (string?)null; Assert.True(value is string text && text.Length == 0); },
            () => RtlUnit.Length(null), () => RtlUnit.IsEmpty(null),
            () => RtlUnit.Copy(null, 1, 1), () => RtlUnit.Delete(null, 1, 1),
            () => RtlUnit.Insert(null, "", 1), () => RtlUnit.Insert("", null, 1),
            () => { string value = null!; RtlUnit.Delete(ref value, 1, 1); },
            () => { string value = null!; RtlUnit.Insert("", ref value, 1); },
            () => { string value = ""; RtlUnit.Insert(null, ref value, 1); },
            () => { string value = null!; RtlUnit.SetLength(ref value, 1); },
            () => RtlUnit.Pos(null, ""), () => RtlUnit.Pos("", null),
            () => RtlUnit.AnsiPos(null, ""), () => RtlUnit.AnsiPos("", null),
            () => RtlUnit.Resized(null, 1), () => RtlUnit.NullTerminatedText(null),
            () => RtlUnit.UpperCase(null), () => RtlUnit.LowerCase(null),
            () => RtlUnit.AnsiUpperCase(null), () => RtlUnit.AnsiLowerCase(null),
            () => RtlUnit.CompareStr(null, ""), () => RtlUnit.CompareStr("", null),
            () => RtlUnit.CompareText(null, ""), () => RtlUnit.CompareText("", null),
            () => RtlUnit.AnsiCompareStr(null, ""), () => RtlUnit.AnsiCompareStr("", null),
            () => RtlUnit.AnsiCompareText(null, ""), () => RtlUnit.AnsiCompareText("", null),
            () => RtlUnit.Trim(null), () => RtlUnit.TrimLeft(null), () => RtlUnit.TrimRight(null),
            () => RtlUnit.QuotedStr(null), () => RtlUnit.StrToIntDef(null, 1),
            () => RtlUnit.Format(null), () => RtlUnit.Format("%s", (string?)null),
            () => RtlUnit.Assert(true, null, "file.pas", 1),
            () => RtlUnit.ExtractFileName(null), () => RtlUnit.ExtractFileDrive(null),
            () => RtlUnit.ExtractFilePath(null), () => RtlUnit.ExtractFileDir(null),
            () => RtlUnit.ExtractFileExt(null), () => RtlUnit.ExpandFileName(null),
            () => RtlUnit.ChangeFileExt(null, ""), () => RtlUnit.ChangeFileExt("", null)
        ];
        foreach (Action read in reads)
        {
            if (strict)
            {
                Assert.Throws<NullStringException>(read);
            }
            else
            {
                read();
            }
        }
        if (strict)
        {
            Assert.Throws<NullStringException>(() => RtlUnit.WithChar(null, 1, 'x'));
            Assert.Throws<NullStringException>(() => RtlUnit.StrToInt(null));
            Assert.Throws<NullStringException>(() => RtlUnit.Assert(false, null, "file.pas", 1));
        }
        else
        {
            Assert.Throws<IndexOutOfRangeException>(() => RtlUnit.WithChar(null, 1, 'x'));
            Assert.Throws<FormatException>(() => RtlUnit.StrToInt(null));
            Assert.Throws<AssertionFailedException>(() => RtlUnit.Assert(false, null, "file.pas", 1));
            Assert.Empty(Cp1252.Encode(null));
            Assert.Equal("", Cp1252.Normalize(null));
            Assert.True(RtlUnit.IsEmpty(null));
        }
        Assert.True(RtlUnit.IsEmpty(""));
        Assert.False(RtlUnit.IsEmpty("\0"));
        Assert.False(RtlUnit.IsEmpty("x"));
        Assert.Empty(Cp1252.Encode(""));
        Assert.Equal("", Cp1252.Normalize(""));
        Assert.Equal(0, Cp1252.Compare("", ""));
        Assert.Equal("", "");
    }
}
