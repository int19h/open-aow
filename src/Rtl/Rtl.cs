namespace Aow.Rtl;

public static partial class RtlUnit
{
    private static int randomSeed;

    public static Action<RandomTraceEvent>? RandomTrace { get; set; }

    public static int RandSeed
    {
        get => randomSeed;
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        set
        {
            int before = randomSeed;
            randomSeed = value;
            RandomTrace?.Invoke(new("seed", null, value, before, value));
        }
    }

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    public static int Random(int range)
    {
        int before = randomSeed;
        randomSeed = unchecked(randomSeed * 0x08088405 + 1);
        int result = (int)(((ulong)(uint)randomSeed * (uint)range) >> 32);
        RandomTrace?.Invoke(new("random", range, result, before, randomSeed));
        return result;
    }

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    public static double Random()
    {
        int before = randomSeed;
        randomSeed = unchecked(randomSeed * 0x08088405 + 1);
        double result = (uint)randomSeed * (1.0 / 4294967296.0);
        RandomTrace?.Invoke(new("random", null, result, before, randomSeed));
        return result;
    }

    public static double DegToRad(double degrees) => degrees * (Math.PI / 180);

    // SYSTEM.PAS:1647 uses GetSystemTime, which returns UTC.
    public static void Randomize()
    {
        if (RandomizeSeed is int seed) RandSeed = seed;
        else if (!SuppressRandomize) RandSeed = (int)Clock.UtcNow.TimeOfDay.TotalMilliseconds;
    }

    public static int? RandomizeSeed { get; set; }
    public static Action<double, int, int>? TraceRound { get; set; }

    public static int Round(double value)
    {
        int result = ToInteger(Math.Round(value, MidpointRounding.ToEven));
        if (TraceRound is { } record && double.IsFinite(value) && value >= int.MinValue && value <= int.MaxValue)
        {
            double roundedSingle = Math.Round((double)(float)value, MidpointRounding.ToEven);
            if (roundedSingle >= int.MinValue && roundedSingle <= int.MaxValue && result != (int)roundedSingle)
                record(value, result, (int)roundedSingle);
        }
        return result;
    }

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

    public static byte Chr(int value) => unchecked((byte)value);
    public static int Ord(byte value) => value;
    public static int Ord(int value) => value;
    public static int Ord(uint value) => unchecked((int)value);
    public static int Ord(bool value) => value ? 1 : 0;
    public static int Ord<E>(E value) where E : unmanaged, Enum => Convert.ToInt32(value);
    public static byte UpCase(byte value) => value is >= (byte)'a' and <= (byte)'z' ? (byte)(value - 32) : value;

    public static string SetString(Ptr<byte> source, int length)
    {
        if (length <= 0) return "";
        if (source.IsNull) return Resized("", length);
        return Cp1252.Decode(source.Array!.AsSpan(source.Index, length).ToArray());
    }

    public static int StrLen(Ptr<byte> value)
    {
        int count = 0;
        while (value[count] != 0) count++;
        return count;
    }

    public static string StrPas(Ptr<byte> value) => FromPChar(value);

    public static string StrPasBuffer(ReadOnlySpan<byte> value)
    {
        int end = value.IndexOf((byte)0);
        return Cp1252.Decode((end < 0 ? value : value[..end]).ToArray());
    }

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

    // ReallocMem of a block of records held as an array: a count of 0 frees it (null), any other
    // count gives a new array that keeps the elements the two sizes share, the rest zero.
    public static T[]? ReallocArray<T>(T[]? array, int count)
    {
        if (count == 0) return null;
        var result = new T[count];
        if (array is not null) array.AsSpan(0, Math.Min(array.Length, count)).CopyTo(result);
        return result;
    }

    // Move of count records from one array index to another, in one array or two. Overlapping
    // ranges copy as Move copies them. A count of 0 or less moves nothing.
    public static void MoveElements<T>(T[]? source, int sourceIndex, T[]? destination, int destinationIndex, int count)
    {
        if (count <= 0) return;
        source.AsSpan(sourceIndex, count).CopyTo(destination.AsSpan(destinationIndex, count));
    }

    // FillChar(A[Index], Count * SizeOf(T), 0) of records held in an array. A count of 0 or less
    // clears nothing.
    public static void ClearElements<T>(T[]? array, int index, int count)
    {
        if (count <= 0) return;
        array.AsSpan(index, count).Clear();
    }

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

    private static string[] arguments = Environment.GetCommandLineArgs().Select(s => s).ToArray();

    // Until IRtlHost is introduced, the host supplies argument zero and the arguments here.
    public static void SetCommandLine(params string[] values)
    {
        if (values.Length == 0) throw new ArgumentException("Argument zero is the executable name.", nameof(values));
        arguments = (string[])values.Clone();
    }

    public static int ParamCount() => arguments.Length - 1;
    public static string ParamStr(int index) => index >= 0 && index < arguments.Length ? arguments[index] : "";
}
