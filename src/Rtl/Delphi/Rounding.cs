namespace Aow.Rtl;

public static partial class RtlUnit
{
    public static double DegToRad(double degrees) => degrees * (Math.PI / 180);

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
}
