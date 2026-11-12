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
        using var mode = new StringMode(strict);
        Action[] reads = [
            () => Cp1252.Encode(null), () => Cp1252.Normalize(null),
            () => Cp1252.Compare(null, ""), () => Cp1252.Compare("", null),
            () => Cp1252.Comparer.Compare(null, ""), () => Cp1252.Comparer.Compare("", null),
            () => { string value = Cp1252.Normalize((string?)null); Assert.Equal("", value); },
            () => { FormatArg value = (string?)null; Assert.True(value is string text && text.Length == 0); },
            () => RtlUnit.Length((string?)null), () => RtlUnit.IsEmpty(null),
            () => RtlUnit.Copy((string?)null, 1, 1), () => RtlUnit.Delete((string?)null, 1, 1),
            () => RtlUnit.Insert((string?)null, "", 1), () => RtlUnit.Insert("", (string?)null, 1),
            () => { string value = null!; RtlUnit.Delete(ref value, 1, 1); },
            () => { string value = null!; RtlUnit.Insert("", ref value, 1); },
            () => { string value = ""; RtlUnit.Insert(null, ref value, 1); },
            () => { string value = null!; RtlUnit.SetLength(ref value, 1); },
            () => RtlUnit.Pos((string?)null, ""), () => RtlUnit.Pos("", (string?)null),
            () => RtlUnit.AnsiPos((string?)null, ""), () => RtlUnit.AnsiPos("", (string?)null),
            () => RtlUnit.Resized(null, 1), () => RtlUnit.NullTerminatedText((string?)null),
            () => RtlUnit.UpperCase((string?)null), () => RtlUnit.LowerCase((string?)null),
            () => RtlUnit.AnsiUpperCase((string?)null), () => RtlUnit.AnsiLowerCase((string?)null),
            () => RtlUnit.CompareStr((string?)null, ""), () => RtlUnit.CompareStr("", (string?)null),
            () => RtlUnit.CompareText((string?)null, ""), () => RtlUnit.CompareText("", (string?)null),
            () => RtlUnit.AnsiCompareStr((string?)null, ""), () => RtlUnit.AnsiCompareStr("", (string?)null),
            () => RtlUnit.AnsiCompareText((string?)null, ""), () => RtlUnit.AnsiCompareText("", (string?)null),
            () => RtlUnit.Trim((string?)null), () => RtlUnit.TrimLeft((string?)null), () => RtlUnit.TrimRight((string?)null),
            () => RtlUnit.QuotedStr((string?)null), () => RtlUnit.StrToIntDef((string?)null, 1),
            () => RtlUnit.Format((string?)null), () => RtlUnit.Format("%s", (string?)null),
            () => RtlUnit.Assert(true, (string?)null, "file.pas", 1),
            () => RtlUnit.ExtractFileName((string?)null), () => RtlUnit.ExtractFileDrive((string?)null),
            () => RtlUnit.ExtractFilePath((string?)null), () => RtlUnit.ExtractFileDir((string?)null),
            () => RtlUnit.ExtractFileExt((string?)null), () => RtlUnit.ExpandFileName((string?)null),
            () => RtlUnit.ChangeFileExt((string?)null, ""), () => RtlUnit.ChangeFileExt("", (string?)null)
        ];
        foreach (Action read in reads)
        {
            if (strict) Assert.Throws<NullStringException>(read);
            else read();
        }
        if (strict)
        {
            Assert.Throws<NullStringException>(() => RtlUnit.WithChar(null, 1, 'x'));
            Assert.Throws<NullStringException>(() => RtlUnit.StrToInt((string?)null));
            Assert.Throws<NullStringException>(() => RtlUnit.Assert(false, (string?)null, "file.pas", 1));
        }
        else
        {
            Assert.Throws<IndexOutOfRangeException>(() => RtlUnit.WithChar(null, 1, 'x'));
            Assert.Throws<FormatException>(() => RtlUnit.StrToInt((string?)null));
            Assert.Throws<AssertionFailedException>(() => RtlUnit.Assert(false, (string?)null, "file.pas", 1));
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
