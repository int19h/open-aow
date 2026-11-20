using System.Runtime.InteropServices;
using static Aow.Rtl.DelphiFailure;
using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiMemory;
using static Aow.Rtl.DelphiOrdinals;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;
using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.PCharStrings;
using static Aow.Rtl.PointerMemory;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class SystemTests
{
    [Fact]
    public void MathFunctionsAndIntegerLimits()
    {
        Assert.Equal(int.MaxValue, Round(int.MaxValue + 0.49));
        Assert.Equal(int.MinValue, Round(int.MinValue - 0.5));
        Assert.Equal(int.MaxValue, Trunc(int.MaxValue + 0.9));
        Assert.Equal(int.MinValue, Trunc(int.MinValue - 0.9));
        foreach (double value in new[] { double.NaN, double.PositiveInfinity, double.NegativeInfinity, 2147483648.0, -2147483649.0 })
        {
            // SYSTEM.PAS:1776 and :1787 use FISTP dword with invalid operation unmasked.
            _ = Assert.Throws<ArithmeticException>(() => Round(value));
            _ = Assert.Throws<ArithmeticException>(() => Trunc(value));
        }
        Assert.Equal(0, Round(-0.5));
        Assert.Equal(0, Trunc(-0.9));
        Assert.Equal(-2.0, Int(-2.75));
        Assert.Equal(-0.75, Frac(-2.75));
        Assert.Equal(0.0, Int(0));
        Assert.Equal(0.0, Frac(0));
    }

    [Fact]
    public void MathDomainAndRangeFollowDelphiSource() => _ = Assert.Throws<ArithmeticException>(() => Frac(double.PositiveInfinity));

    [Fact]
    public void RandomIncludesUnsignedBoundsAndConsumesEveryCall()
    {
        RandSeed = 0;
        Assert.Equal(0, Random(0));
        Assert.Equal(1, RandSeed);
        RandSeed = 0;
        Assert.Equal(0, Random(1));
        Assert.Equal(1, RandSeed);
        RandSeed = 0;
        Assert.Equal(0, Random(-1));
        RandSeed = 1;
        Assert.Equal(0x08088405, Random(-1));
        Assert.Equal(0x08088406, RandSeed);
        RandSeed = 0;
        Assert.Equal(1.0 / 4294967296, Random());
        double before = DateTime.UtcNow.TimeOfDay.TotalMilliseconds;
        Randomize();
        double after = DateTime.UtcNow.TimeOfDay.TotalMilliseconds;
        Assert.InRange((RandSeed - Math.Floor(before) + 86400000) % 86400000, low: 0, high: Math.Ceiling((after - before + 86400000) % 86400000) + 1);
    }

    [Fact]
    public void StringsHandleExtremeIndicesAndIndependentAliases()
    {
        string text = "abcd";
        Assert.Equal("ab", Copy(text, index: int.MinValue, count: 2));
        Assert.Equal("", Copy(text, index: int.MaxValue, count: int.MaxValue));
        Assert.Equal(text, Delete(text, index: int.MinValue, count: int.MaxValue));
        Assert.Equal(text, Delete(text, index: int.MaxValue, count: int.MaxValue));
        Assert.Equal("", Delete(text, index: 1, count: int.MaxValue));
        Assert.Equal("XYabcd", Insert("XY", value: text, index: int.MinValue));
        Assert.Equal("abcdXY", Insert("XY", value: text, index: int.MaxValue));
        Assert.Equal(0, Pos("x", haystack: text));
        Assert.Equal(4, Length(text));
        Assert.Equal((byte)255, Chr(-1));
        Assert.Equal((byte)0, Chr(256));
        Assert.Equal(255, Ord((byte)255));
        Assert.Equal(-1, Ord(-1));
        Assert.Equal(-1, Ord(uint.MaxValue));
        Assert.Equal(1, Ord(Generated.Flag.Second));
        for (int i = 0; i <= 255; i++)
        {
            Assert.Equal(i is >= 97 and <= 122 ? i - 32 : i, UpCase((byte)i));
        }

        Ptr<byte> pointer = Ptr<byte>.Unbox("ab\0cd");
        Assert.Equal(2, StrLen(pointer));
        Assert.Equal("ab", StrPas(pointer));
        Assert.Equal("b", StrPas(pointer + 1));
        Assert.Equal("", StrPas(default));
        Assert.Equal("ab\0cd", SetString(pointer, length: 5));
        SetString(out string value, source: pointer + 1, length: 3);
        Assert.Equal("b\0c", value);
        pointer[1] = 99;
        Assert.Equal("b\0c", value);
        Assert.Equal("\0\0\0", SetString(default, length: 3));
        Assert.Equal("", SetString(default, length: -1));
        Ptr<byte> destination = Ptr<byte>.Alloc(10);
        Ptr<byte>.Fill(destination, count: 10, value: 255);
        Assert.Equal(destination, StrCopy(destination + 0, source: pointer));
        Assert.Equal(0, destination[2]);
        Assert.Equal(255, destination[3]);
        Assert.Equal(0, StrLComp(pointer, destination, maxLength: 10));
        Assert.Equal(0, StrLComp(default, default, maxLength: 0));
        Assert.Equal(0, StrLComp(Ptr<byte>.Unbox("abc"), Ptr<byte>.Unbox("abd"), maxLength: 2));
        Assert.Equal(-1, StrLComp(Ptr<byte>.Unbox("abc"), Ptr<byte>.Unbox("abd"), maxLength: 3));
        Assert.Equal(127, StrLComp(Ptr<byte>.Unbox("\u00FF"), Ptr<byte>.Unbox("\u20AC"), maxLength: 2));
        Assert.Equal(-99, StrLComp(Ptr<byte>.Unbox("ab"), Ptr<byte>.Unbox("abc"), maxLength: 10));
        Assert.Equal(pointer + 1, StrScan(pointer, character: (byte)'c'));
        Assert.Equal(pointer + 2, StrScan(pointer, character: 0));
        Assert.True(StrScan(pointer, character: (byte)'d').IsNull);
        Assert.Equal(pointer, StrScan(pointer, character: (byte)'a'));
    }

    [StoredLayout]
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    private struct PackedRecord
    {
        [FieldOffset(0)] public int Value;
        [FieldOffset(4)] public byte Byte;
    }

    [Fact]
    public void TypedMemoryAndByteViews()
    {
        GetMem(out Ptr<int> pointer, count: 3);
        GetMem(out Ptr<int> zero, count: 0);
        Assert.True(zero.IsNull);
        FillChar(pointer, count: 3, value: 7);
        Assert.Equal((int[])[7, 7, 7], pointer.Array);
        pointer.Value = 1;
        Move(pointer, destination: pointer + 1, count: 2);
        Assert.Equal((int[])[1, 1, 7], pointer.Array);
        ReallocMem(ref pointer, count: 5);
        Assert.Equal((int[])[1, 1, 7, 0, 0], pointer.Array);
        ReallocMem(ref pointer, count: 1);
        Assert.Equal((int[])[1], pointer.Array);
        Ptr<int> alias = pointer;
        FreeMem(out pointer);
        Assert.True(pointer.IsNull);
        Assert.Equal(1, alias.Value);
        New(out Ptr<PackedRecord> record);
        Assert.Equal(0, record.Value.Value);
        record.Value.Value = 0x04030201;
        record.Value.Byte = 5;
        Span<byte> bytes = Bytes.Of(ref record.Value);
        Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 0, 0, 0 }, bytes.ToArray());
        Assert.Equal(4, Bytes.Of(ref record.Value, count: 4).Length);
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => Bytes.Of(ref record.Value, count: 9).ToArray());
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => Bytes.Of(ref record.Value, count: -1).ToArray());
        FillChar(bytes, count: 2, value: 0x80);
        Assert.Equal(0x04038080, record.Value.Value);
        PackedRecord other = new();
        Move(bytes, destination: Bytes.Of(ref other), count: 8);
        Assert.True(CompareMem(bytes, Bytes.Of(ref other), count: 8));
        other.Byte = 6;
        Assert.False(CompareMem(bytes, Bytes.Of(ref other), count: 8));
        Move(bytes[..4], destination: bytes[1..], count: 4);
        Assert.Equal(new byte[] { 128, 128, 128, 3, 4, 0, 0, 0 }, bytes.ToArray());
        Move([], destination: [], count: -1);
        FillChar([], count: 0, value: 1);
        Assert.True(CompareMem([], [], count: 0));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => Move(new byte[1], destination: new byte[1], count: 2));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => FillChar(new byte[1], count: 2, value: 0));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => CompareMem(new byte[1], new byte[1], count: 2));
        Dispose(out record);
        Assert.True(record.IsNull);
        FreeMem(out pointer);
        Dispose(out record);
        ReallocMem(ref pointer, count: 0);
        Assert.True(pointer.IsNull);
    }

    [Fact]
    public void ClassCastAbortHaltAndAssertKeepTheirReports()
    {
        Assert.Null(As<object>(null));
        object objectValue = new();
        Assert.Same(objectValue, As<object>(objectValue));
        Assert.Equal("Invalid class typecast", Assert.Throws<InvalidCastException>(() => As<Generated.Sample>(objectValue)).Message);
        _ = Assert.Throws<AbortException>(Abort);
        Assert.Equal(0, Assert.Throws<HaltException>(() => Halt()).ExitCode);
        Assert.Equal(-1, Assert.Throws<HaltException>(() => Halt(-1)).ExitCode);
        Assert.Equal("broken (unit.pas, line 42)", Assert.Throws<AssertionFailedException>(() => DelphiAssert.Assert(false, message: "broken", file: "unit.pas", line: 42)).Message);
        // SYSUTILS.PAS:5380 substitutes SAssertionFailed for an empty message.
        Assert.Equal("Assertion failed (unit.pas, line 1)", Assert.Throws<AssertionFailedException>(() => DelphiAssert.Assert(false, message: "", file: "unit.pas", line: 1)).Message);
        DelphiAssert.Assert(true, message: "", file: "unit.pas", line: 1);
    }

    [Fact]
    public void ExceptionFormattingUsesTaggedArguments()
    {
        Assert.Equal("0007 x   FF", Format("%.4d %-3s %X", 7, "x", -1 & 255));
        Assert.Equal("bb:aa", Format("%1:s:%0:s", "aa", "bb"));
        Assert.Equal("   abc", Format("%*.*s", 6, 3, "abcdef"));
        Assert.Equal("-0007", Format("%.4d", -7));
        Assert.Equal("4294967295", Format("%u", -1));
        Assert.Equal("A", Format("%s", (byte)65));
        _ = Assert.Throws<FormatException>(() => Format("%s", 1));
        _ = Assert.Throws<FormatException>(() => Format("%d", true));
        _ = Assert.Throws<FormatException>(() => Format("%d"));
        Assert.Equal("", Format("%"));
        _ = Assert.Throws<FormatException>(() => Format("%f", 1.25));
        Assert.Equal("x", Format("%*s", true, "x"));
    }
}
