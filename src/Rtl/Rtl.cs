namespace Aow.Rtl;

public static partial class Rtl
{
    public static int RandSeed;
    public static int Random(int range)
    {
        RandSeed = unchecked(RandSeed * 0x08088405 + 1);
        return (int)(((ulong)(uint)RandSeed * (uint)range) >> 32);
    }
    public static double Random()
    {
        RandSeed = unchecked(RandSeed * 0x08088405 + 1);
        return (uint)RandSeed * (1.0 / 4294967296.0);
    }
    // SYSTEM.PAS:1647 uses GetSystemTime, which returns UTC.
    public static void Randomize() => RandSeed = (int)DateTime.UtcNow.TimeOfDay.TotalMilliseconds;
    public static int Round(double value) => ToInteger(Math.Round(value, MidpointRounding.ToEven));
    public static int Trunc(double value) => ToInteger(Math.Truncate(value));
    private static int ToInteger(double value) => double.IsFinite(value) && value >= int.MinValue && value <= int.MaxValue
        ? (int)value : throw new EInvalidOp("Invalid floating point operation");
    public static double Int(double value) => Math.Truncate(value);
    public static double Frac(double value) => double.IsInfinity(value)
        ? throw new EInvalidOp("Invalid floating point operation") : value - Math.Truncate(value);
    public static int Abs(int value) => value < 0 ? unchecked(-value) : value;
    public static long Abs(long value) => value < 0 ? unchecked(-value) : value;
    public static double Abs(double value) => Math.Abs(value);
    public static int Sqr(int value) => unchecked(value * value);
    public static double Sqr(double value) => FloatingResult(value * value, double.IsFinite(value));
    public static double Sqrt(double value) => value < 0
        ? throw new EInvalidOp("Invalid floating point operation") : Math.Sqrt(value);
    // SYSTEM.PAS:1709 and :1750 return zero when the x87 C2 range flag is set.
    public static double Sin(double value) => Trig(value, Math.Sin);
    public static double Cos(double value) => Trig(value, Math.Cos);
    private static double Trig(double value, Func<double, double> function)
    {
        if (double.IsInfinity(value)) throw new EInvalidOp("Invalid floating point operation");
        return Math.Abs(value) >= 9223372036854775808.0 ? 0 : function(value);
    }
    public static double ArcTan(double value) => Math.Atan(value);
    public static double Ln(double value) => value == 0 ? throw new EZeroDivide("Floating point division by zero")
        : value < 0 ? throw new EInvalidOp("Invalid floating point operation") : Math.Log(value);
    public static double Exp(double value)
    {
        if (double.IsInfinity(value)) throw new EInvalidOp("Invalid floating point operation");
        return FloatingResult(Math.Exp(value), double.IsFinite(value));
    }
    public static double Power(double value, double exponent)
    {
        // MATH.PAS:639 takes the integer path only within the signed 32-bit range.
        if (exponent == 0) return 1;
        if (value == 0 && exponent > 0) return 0;
        if (Frac(exponent) == 0 && Math.Abs(exponent) <= int.MaxValue)
        {
            if (value == 0) throw new EZeroDivide("Floating point division by zero");
            return FloatingResult(Math.Pow(value, exponent), double.IsFinite(value));
        }
        return Exp(exponent * Ln(value));
    }
    private static double FloatingResult(double result, bool finiteInput) => double.IsInfinity(result) && finiteInput
        ? throw new EOverflow("Floating point overflow") : result;

    public static AnsiString Copy(AnsiString value, int index, int count)
    {
        index = Math.Max(1, index);
        if (index > value.Length || count <= 0) return default;
        return AnsiString.FromBytes(value.Span.Slice(index - 1, Math.Min(count, value.Length - index + 1)).ToArray());
    }
    public static AnsiString Delete(AnsiString value, int index, int count)
    {
        if (index < 1 || index > value.Length || count <= 0) return value;
        count = Math.Min(count, value.Length - index + 1);
        return Copy(value, 1, index - 1) + Copy(value, index + count, int.MaxValue);
    }
    public static void Delete(ref AnsiString value, int index, int count) => value = Delete(value, index, count);
    public static AnsiString Insert(AnsiString source, AnsiString value, int index)
    {
        int offset = index <= 1 ? 0 : Math.Min(index - 1, value.Length);
        return Copy(value, 1, offset) + source + Copy(value, offset + 1, int.MaxValue);
    }
    public static void Insert(AnsiString source, ref AnsiString value, int index) => value = Insert(source, value, index);
    public static int Pos(AnsiString needle, AnsiString haystack) => needle.Length == 0 ? 0 : haystack.Span.IndexOf(needle.Span) + 1;
    public static void SetLength(ref AnsiString value, int length) => value = value.Resized(length);
    public static int Length(AnsiString value) => value.Length;
    public static byte Chr(int value) => unchecked((byte)value);
    public static int Ord(byte value) => value;
    public static int Ord(int value) => value;
    public static int Ord(uint value) => unchecked((int)value);
    public static int Ord(bool value) => value ? 1 : 0;
    public static int Ord<E>(E value) where E : unmanaged, Enum => Convert.ToInt32(value);
    public static byte UpCase(byte value) => value is >= (byte)'a' and <= (byte)'z' ? (byte)(value - 32) : value;
    public static AnsiString SetString(Ptr<byte> source, int length)
    {
        if (length <= 0) return default;
        if (source.IsNull) return default(AnsiString).Resized(length);
        return AnsiString.FromBytes(source.Array!.AsSpan(source.Index, length).ToArray());
    }
    public static void SetString(ref AnsiString value, Ptr<byte> source, int length) => value = SetString(source, length);
    public static int StrLen(Ptr<byte> value)
    {
        int count = 0;
        while (value[count] != 0) count++;
        return count;
    }
    public static AnsiString StrPas(Ptr<byte> value) => AnsiString.FromPChar(value);
    public static Ptr<byte> StrCopy(Ptr<byte> destination, Ptr<byte> source)
    {
        Ptr<byte>.Copy(source, destination, StrLen(source) + 1);
        return destination;
    }
    public static int StrLComp(Ptr<byte> a, Ptr<byte> b, int maxLength)
    {
        for (int i = 0; i < maxLength; i++)
        {
            int difference = a[i] - b[i];
            if (difference != 0 || a[i] == 0) return difference;
        }
        return 0;
    }
    public static Ptr<byte> StrScan(Ptr<byte> value, byte character)
    {
        for (int i = 0; ; i++)
        {
            if (value[i] == character) return value + i;
            if (value[i] == 0) return default;
        }
    }

    public static void Move<T>(Ptr<T> source, Ptr<T> destination, int count) => Ptr<T>.Copy(source, destination, count);
    public static void Move(ReadOnlySpan<byte> source, Span<byte> destination, int count)
    {
        if (count <= 0) return;
        if (count > source.Length || count > destination.Length) throw new ERangeError("Move exceeds the buffer");
        source[..count].CopyTo(destination);
    }
    public static void FillChar<T>(Ptr<T> destination, int count, T value) => Ptr<T>.Fill(destination, count, value);
    public static void FillChar(Span<byte> destination, int count, byte value)
    {
        if (count > destination.Length) throw new ERangeError("FillChar exceeds the buffer");
        if (count > 0) destination[..count].Fill(value);
    }
    public static bool CompareMem(ReadOnlySpan<byte> a, ReadOnlySpan<byte> b, int count)
    {
        if (count > a.Length || count > b.Length) throw new ERangeError("CompareMem exceeds the buffer");
        return count <= 0 || a[..count].SequenceEqual(b[..count]);
    }
    public static void GetMem<T>(out Ptr<T> pointer, int count) => pointer = Ptr<T>.Alloc(count);
    public static void ReallocMem<T>(ref Ptr<T> pointer, int count) => pointer = pointer.Realloc(count);
    public static void FreeMem<T>(ref Ptr<T> pointer) => pointer = default;
    public static void New<T>(out Ptr<T> pointer) => pointer = Ptr<T>.Alloc(1);
    public static void Dispose<T>(ref Ptr<T> pointer) => pointer = default;
    public static void Assert(bool condition, AnsiString message, string file, int line)
    {
        if (!condition) throw new EAssertionFailed($"{(message.Length == 0 ? (AnsiString)"Assertion failed" : message)} ({file}, line {line})");
    }
    public static void Halt(int exitCode = 0) => throw new EHalt(exitCode);
    public static void Abort() => throw EAbort.Create();
    public static T? As<T>(object? value) where T : class => value is null ? null : value as T
        ?? throw new EInvalidCast("Invalid class typecast");
    public static Exception Wrap(System.Exception value) => value switch
    {
        Exception existing => existing,
        NullReferenceException => new EAccessViolation(value.Message),
        DivideByZeroException => new EDivByZero(value.Message),
        IndexOutOfRangeException => new ERangeError(value.Message),
        InvalidCastException => new EInvalidCast(value.Message),
        _ => new Exception(value.Message)
    };

    private static AnsiString[] arguments = Environment.GetCommandLineArgs().Select(s => (AnsiString)s).ToArray();
    // Until IRtlHost is introduced, the host supplies argument zero and the arguments here.
    public static void SetCommandLine(params AnsiString[] values)
    {
        if (values.Length == 0) throw new ArgumentException("Argument zero is the executable name.", nameof(values));
        arguments = (AnsiString[])values.Clone();
    }
    public static int ParamCount() => arguments.Length - 1;
    public static AnsiString ParamStr(int index) => index >= 0 && index < arguments.Length ? arguments[index] : default;
}
