using System.Runtime.InteropServices;
using static Aow.Rtl.RtlUnit;
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
            Assert.Throws<ArithmeticException>(() => Round(value));
            Assert.Throws<ArithmeticException>(() => Trunc(value));
        }
        Assert.Equal(0, Round(-0.5));
        Assert.Equal(0, Trunc(-0.9));
        Assert.Equal(-2.0, Int(-2.75));
        Assert.Equal(-0.75, Frac(-2.75));
        Assert.Equal(0.0, Int(0));
        Assert.Equal(0.0, Frac(0));
        Assert.Equal(3, Abs(-3));
        Assert.Equal(3, Abs(3));
        Assert.Equal(int.MinValue, Abs(int.MinValue));
        Assert.Equal(long.MinValue, Abs(long.MinValue));
        Assert.Equal(3L, Abs(-3L));
        Assert.Equal(3L, Abs(3L));
        Assert.Equal(3.25, Abs(-3.25));
        Assert.Equal(9, Sqr(-3));
        Assert.Equal(0, Sqr(65536));
        Assert.Equal(6.25, Sqr(-2.5));
        Assert.Equal(3, Sqrt(9));
        Assert.Throws<ArithmeticException>(() => Sqrt(-1));
        Assert.Equal(0, Sin(0));
        Assert.Equal(1, Cos(0));
        Assert.Equal(Math.PI / 4, ArcTan(1));
        Assert.Equal(1, Ln(Math.E));
        Assert.Equal(Math.E, Exp(1));
        Assert.Equal(1024, Power(2, 10));
        Assert.Equal(1, Power(0, 0));
    }

    [Fact]
    public void MathDomainAndRangeFollowDelphiSource()
    {
        // SYSTEM.PAS:312 enables invalid operation, zero divide and overflow exceptions.
        // SYSTEM.PAS:1709 and :1750 return zero at the x87 trigonometric range limit.
        // MATH.PAS:639 selects IntPower only for integer exponents up to MaxInt.
        // A DCC32 check in src/.build-env/math_edges.pas confirms these boundary results.
        Assert.Throws<ArithmeticException>(() => Sqrt(-1));
        Assert.Throws<DivideByZeroException>(() => Ln(0));
        Assert.Throws<ArithmeticException>(() => Ln(-1));
        Assert.Equal(0, Sin(9223372036854775808.0));
        Assert.Equal(0, Cos(9223372036854775808.0));
        Assert.Equal(0, Sin(-1e30));
        Assert.Equal(0, Cos(-1e30));
        Assert.Throws<OverflowException>(() => Exp(1000));
        Assert.Throws<OverflowException>(() => Exp(12000));
        Assert.Throws<ArithmeticException>(() => Power(-2, 0.5));
        Assert.Throws<DivideByZeroException>(() => Power(0, -1));
        Assert.Throws<ArithmeticException>(() => Power(-1, 2147483648.0));
        Assert.Throws<ArithmeticException>(() => Frac(double.PositiveInfinity));
        Assert.Throws<ArithmeticException>(() => Sin(double.PositiveInfinity));
        Assert.Throws<ArithmeticException>(() => Cos(double.NegativeInfinity));
        Assert.Throws<ArithmeticException>(() => Exp(double.PositiveInfinity));
        Assert.Throws<OverflowException>(() => Sqr(double.MaxValue));
        Assert.Throws<OverflowException>(() => Power(double.MaxValue, 2));
        Assert.Equal(0, Exp(-1000));
        Assert.Equal(-8, Power(-2, 3));
        Assert.Equal(2, Power(4, 0.5), 12);
        Assert.Equal(0, Power(0, 0.5));
        Assert.Equal(double.PositiveInfinity, Sqrt(double.PositiveInfinity));
        Assert.True(double.IsNaN(Sqrt(double.NaN)));
        Assert.True(double.IsNaN(Sin(double.NaN)));
        Assert.True(double.IsNaN(Cos(double.NaN)));
        Assert.True(double.IsNaN(Exp(double.NaN)));
    }

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
        double elapsed = (after - before + 86400000) % 86400000;
        double offset = (RandSeed - Math.Floor(before) + 86400000) % 86400000;
        Assert.InRange(offset, 0, Math.Ceiling(elapsed) + 1);
    }

    [Fact]
    public void StringsHandleExtremeIndicesAndIndependentAliases()
    {
        string text = "abcd";
        Assert.Equal("ab", Copy(text, int.MinValue, 2));
        Assert.Equal("", Copy(text, int.MaxValue, int.MaxValue));
        Assert.Equal(text, Delete(text, int.MinValue, int.MaxValue));
        Assert.Equal(text, Delete(text, int.MaxValue, int.MaxValue));
        Assert.Equal("", Delete(text, 1, int.MaxValue));
        Assert.Equal("XYabcd", Insert("XY", text, int.MinValue));
        Assert.Equal("abcdXY", Insert("XY", text, int.MaxValue));
        Assert.Equal(0, Pos("x", text));
        Assert.Equal(4, Length(text));
        Assert.Equal((byte)255, Chr(-1));
        Assert.Equal((byte)0, Chr(256));
        Assert.Equal(255, Ord((byte)255));
        Assert.Equal(-1, Ord(-1));
        Assert.Equal(-1, Ord(uint.MaxValue));
        Assert.Equal(1, Ord(Generated.Flag.Second));
        for (int i = 0; i <= 255; i++)
            Assert.Equal(i is >= 97 and <= 122 ? i - 32 : i, UpCase((byte)i));
        var pointer = Ptr<byte>.Unbox("ab\0cd");
        Assert.Equal(2, StrLen(pointer));
        Assert.Equal("ab", StrPas(pointer));
        Assert.Equal("b", StrPas(pointer + 1));
        Assert.Equal("", StrPas(default));
        Assert.Equal("ab\0cd", SetString(pointer, 5));
        string value = "";
        SetString(ref value, pointer + 1, 3);
        Assert.Equal("b\0c", value);
        pointer[1] = 99;
        Assert.Equal("b\0c", value);
        Assert.Equal("\0\0\0", SetString(default, 3));
        Assert.Equal("", SetString(default, -1));
        var destination = Ptr<byte>.Alloc(10);
        Ptr<byte>.Fill(destination, 10, 255);
        Assert.Equal(destination, StrCopy(destination + 0, pointer));
        Assert.Equal(0, destination[2]);
        Assert.Equal(255, destination[3]);
        Assert.Equal(0, StrLComp(pointer, destination, 10));
        Assert.Equal(0, StrLComp(default, default, 0));
        Assert.Equal(0, StrLComp(Ptr<byte>.Unbox("abc"), Ptr<byte>.Unbox("abd"), 2));
        Assert.Equal(-1, StrLComp(Ptr<byte>.Unbox("abc"), Ptr<byte>.Unbox("abd"), 3));
        Assert.Equal(127, StrLComp(Ptr<byte>.Unbox("\u00FF"), Ptr<byte>.Unbox("\u20AC"), 2));
        Assert.Equal(-99, StrLComp(Ptr<byte>.Unbox("ab"), Ptr<byte>.Unbox("abc"), 10));
        Assert.Equal(pointer + 1, StrScan(pointer, (byte)'c'));
        Assert.Equal(pointer + 2, StrScan(pointer, 0));
        Assert.True(StrScan(pointer, (byte)'d').IsNull);
        Assert.Equal(pointer, StrScan(pointer, (byte)'a'));
    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    private struct PackedRecord
    {
        [FieldOffset(0)] public int Value;
        [FieldOffset(4)] public byte Byte;
    }

    [Fact]
    public void TypedMemoryAndByteViews()
    {
        GetMem<int>(out var pointer, 3);
        GetMem<int>(out var zero, 0);
        Assert.True(zero.IsNull);
        FillChar(pointer, 3, 7);
        Assert.Equal(new[] { 7, 7, 7 }, pointer.Array);
        pointer.Value = 1;
        Move(pointer, pointer + 1, 2);
        Assert.Equal(new[] { 1, 1, 7 }, pointer.Array);
        ReallocMem(ref pointer, 5);
        Assert.Equal(new[] { 1, 1, 7, 0, 0 }, pointer.Array);
        ReallocMem(ref pointer, 1);
        Assert.Equal(new[] { 1 }, pointer.Array);
        var alias = pointer;
        FreeMem(ref pointer);
        Assert.True(pointer.IsNull);
        Assert.Equal(1, alias.Value);
        New<PackedRecord>(out var record);
        Assert.Equal(0, record.Value.Value);
        record.Value.Value = 0x04030201;
        record.Value.Byte = 5;
        var bytes = Bytes.Of(ref record.Value);
        Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 0, 0, 0 }, bytes.ToArray());
        Assert.Equal(4, Bytes.Of(ref record.Value, 4).Length);
        Assert.Throws<ArgumentOutOfRangeException>(() => Bytes.Of(ref record.Value, 9).ToArray());
        Assert.Throws<ArgumentOutOfRangeException>(() => Bytes.Of(ref record.Value, -1).ToArray());
        FillChar(bytes, 2, 0x80);
        Assert.Equal(0x04038080, record.Value.Value);
        var other = new PackedRecord();
        Move(bytes, Bytes.Of(ref other), 8);
        Assert.True(CompareMem(bytes, Bytes.Of(ref other), 8));
        other.Byte = 6;
        Assert.False(CompareMem(bytes, Bytes.Of(ref other), 8));
        Move(bytes[..4], bytes[1..], 4);
        Assert.Equal(new byte[] { 128, 128, 128, 3, 4, 0, 0, 0 }, bytes.ToArray());
        Move([], [], -1);
        FillChar([], 0, 1);
        Assert.True(CompareMem([], [], 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Move(new byte[1], new byte[1], 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => FillChar(new byte[1], 2, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => CompareMem(new byte[1], new byte[1], 2));
        Dispose(ref record);
        Assert.True(record.IsNull);
        FreeMem(ref pointer);
        Dispose(ref record);
        ReallocMem(ref pointer, 0);
        Assert.True(pointer.IsNull);
    }

    [Fact]
    public void ClassCastAbortHaltAndAssertKeepTheirReports()
    {
        Assert.Null(As<object>(null));
        var objectValue = new object();
        Assert.Same(objectValue, As<object>(objectValue));
        Assert.Equal("Invalid class typecast", Assert.Throws<InvalidCastException>(() => As<Generated.Sample>(objectValue)).Message);
        Assert.Throws<AbortException>(Abort);
        Assert.Equal(0, Assert.Throws<HaltException>(() => Halt()).ExitCode);
        Assert.Equal(-1, Assert.Throws<HaltException>(() => Halt(-1)).ExitCode);
        Assert.Equal("broken (unit.pas, line 42)", Assert.Throws<AssertionFailedException>(() => RtlUnit.Assert(false, "broken", "unit.pas", 42)).Message);
        // SYSUTILS.PAS:5380 substitutes SAssertionFailed for an empty message.
        Assert.Equal("Assertion failed (unit.pas, line 1)", Assert.Throws<AssertionFailedException>(() => RtlUnit.Assert(false, "", "unit.pas", 1)).Message);
        RtlUnit.Assert(true, "", "unit.pas", 1);
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
        Assert.Throws<FormatException>(() => Format("%s", 1));
        Assert.Throws<FormatException>(() => Format("%d", true));
        Assert.Throws<FormatException>(() => Format("%d"));
        Assert.Equal("", Format("%"));
        Assert.Throws<FormatException>(() => Format("%f", 1.25));
        Assert.Equal("x", Format("%*s", true, "x"));
    }

    [Fact]
    public void CommandLineUsesHostSuppliedSnapshot()
    {
        string[] arguments = ["game.exe", "one", "two words"];
        SetCommandLine(arguments);
        arguments[1] = "changed";
        Assert.Equal(2, ParamCount());
        Assert.Equal("game.exe", ParamStr(0));
        Assert.Equal("one", ParamStr(1));
        Assert.Equal("two words", ParamStr(2));
        Assert.Equal("", ParamStr(-1));
        Assert.Equal("", ParamStr(3));
        SetCommandLine("game.exe");
        Assert.Equal(0, ParamCount());
        Assert.Throws<ArgumentException>(() => SetCommandLine());
    }
}
