using System.Runtime.CompilerServices;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

internal sealed class StringMode : IDisposable
{
    private readonly bool previous = Rtl.StrictStrings;
    public StringMode(bool strict) => Rtl.StrictStrings = strict;
    public void Dispose() => Rtl.StrictStrings = previous;

    [ModuleInitializer]
    internal static void Initialize()
    {
        ProcessSafety.Initialize();
        Rtl.StrictStrings = true;
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
            () => Rtl.Length((string?)null), () => Rtl.IsEmpty(null),
            () => Rtl.Copy((string?)null, 1, 1), () => Rtl.Delete((string?)null, 1, 1),
            () => Rtl.Insert((string?)null, "", 1), () => Rtl.Insert("", (string?)null, 1),
            () => { string value = null!; Rtl.Delete(ref value, 1, 1); },
            () => { string value = null!; Rtl.Insert("", ref value, 1); },
            () => { string value = ""; Rtl.Insert(null, ref value, 1); },
            () => { string value = null!; Rtl.SetLength(ref value, 1); },
            () => Rtl.Pos((string?)null, ""), () => Rtl.Pos("", (string?)null),
            () => Rtl.AnsiPos((string?)null, ""), () => Rtl.AnsiPos("", (string?)null),
            () => Rtl.Resized(null, 1), () => Rtl.NullTerminatedText((string?)null),
            () => Rtl.UpperCase((string?)null), () => Rtl.LowerCase((string?)null),
            () => Rtl.AnsiUpperCase((string?)null), () => Rtl.AnsiLowerCase((string?)null),
            () => Rtl.CompareStr((string?)null, ""), () => Rtl.CompareStr("", (string?)null),
            () => Rtl.CompareText((string?)null, ""), () => Rtl.CompareText("", (string?)null),
            () => Rtl.AnsiCompareStr((string?)null, ""), () => Rtl.AnsiCompareStr("", (string?)null),
            () => Rtl.AnsiCompareText((string?)null, ""), () => Rtl.AnsiCompareText("", (string?)null),
            () => Rtl.Trim((string?)null), () => Rtl.TrimLeft((string?)null), () => Rtl.TrimRight((string?)null),
            () => Rtl.QuotedStr((string?)null), () => Rtl.StrToIntDef((string?)null, 1),
            () => Rtl.Format((string?)null), () => Rtl.Format("%s", (string?)null),
            () => Rtl.Assert(true, (string?)null, "file.pas", 1),
            () => Rtl.ExtractFileName((string?)null), () => Rtl.ExtractFileDrive((string?)null),
            () => Rtl.ExtractFilePath((string?)null), () => Rtl.ExtractFileDir((string?)null),
            () => Rtl.ExtractFileExt((string?)null), () => Rtl.ExpandFileName((string?)null),
            () => Rtl.ChangeFileExt((string?)null, ""), () => Rtl.ChangeFileExt("", (string?)null)
        ];
        foreach (Action read in reads)
        {
            if (strict) Assert.Throws<NullStringException>(read);
            else read();
        }
        if (strict)
        {
            Assert.Throws<NullStringException>(() => Rtl.WithChar(null, 1, 'x'));
            Assert.Throws<NullStringException>(() => Rtl.StrToInt((string?)null));
            Assert.Throws<NullStringException>(() => Rtl.Assert(false, (string?)null, "file.pas", 1));
        }
        else
        {
            Assert.Throws<ERangeError>(() => Rtl.WithChar(null, 1, 'x'));
            Assert.Throws<EConvertError>(() => Rtl.StrToInt((string?)null));
            Assert.Throws<EAssertionFailed>(() => Rtl.Assert(false, (string?)null, "file.pas", 1));
            Assert.Empty(Cp1252.Encode(null));
            Assert.Equal("", Cp1252.Normalize(null));
            Assert.True(Rtl.IsEmpty(null));
        }
        Assert.True(Rtl.IsEmpty(""));
        Assert.False(Rtl.IsEmpty("\0"));
        Assert.False(Rtl.IsEmpty("x"));
        Assert.Empty(Cp1252.Encode(""));
        Assert.Equal("", Cp1252.Normalize(""));
        Assert.Equal(0, Cp1252.Compare("", ""));
        Assert.Equal("", "");
    }
}
