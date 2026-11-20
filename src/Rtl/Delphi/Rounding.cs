namespace Aow.Rtl;

public static class DelphiRounding
{
    public static Action<double, int, int>? TraceRound { get; set; }

    public static int Round(double value)
    {
        int result = ToInteger(Math.Round(value, mode: MidpointRounding.ToEven));
        if (TraceRound is { } record && double.IsFinite(value) && value is >= int.MinValue and <= int.MaxValue)
        {
            double roundedSingle = Math.Round((double)(float)value, mode: MidpointRounding.ToEven);
            if (roundedSingle is >= int.MinValue and <= int.MaxValue && result != (int)roundedSingle)
            {
                record(value, result, (int)roundedSingle);
            }
        }
        return result;
    }

    public static int Trunc(double value) => ToInteger(Math.Truncate(value));

    private static int ToInteger(double value) => double.IsFinite(value) && value is >= int.MinValue and <= int.MaxValue
        ? (int)value : throw new ArithmeticException("Invalid floating point operation");

    public static double Int(double value) => Math.Truncate(value);

    public static double Frac(double value) => double.IsInfinity(value)
        ? throw new ArithmeticException("Invalid floating point operation") : value - Math.Truncate(value);
}
