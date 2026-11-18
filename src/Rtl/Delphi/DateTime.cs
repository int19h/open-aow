using System.Globalization;

namespace Aow.Rtl;

public static class DelphiDateTime
{
    private static readonly DateTime Epoch = new(1899, 12, 30);

    // SYSUTILS.PAS FileDateToDateTime and DateTimeToFileDate use DOS two-second resolution.
    public static double FileDateToDateTime(int value) => EncodeDate((int)((uint)value >> 25) + 1980, (value >> 21) & 15, (value >> 16) & 31) + EncodeTime((value >> 11) & 31, (value >> 5) & 63, (value & 31) * 2, 0);

    // The DOS date and time of a file time, as SYSUTILS.PAS FileAge and TSearchRec.Time give it.
    internal static int FileDate(DateTime time)
    {
        return time.Year is < 1980 or > 2099
            ? 0
            : unchecked(((time.Year - 1980) << 25) | (time.Month << 21) | (time.Day << 16) | (time.Hour << 11) | (time.Minute << 5) | (time.Second >> 1));
    }

    // SYSUTILS.PAS EncodeDate and EncodeTime.
    public static double EncodeDate(int year, int month, int day)
    {
        try { return (new DateTime(year, month, day) - Epoch).TotalDays; }
        catch (ArgumentOutOfRangeException) { throw new FormatException("Invalid argument to date encode"); }
    }

    public static double EncodeTime(int hour, int minute, int second, int millisecond)
    {
        return hour is < 0 or >= 24 || minute is < 0 or >= 60 || second is < 0 or >= 60 || millisecond is < 0 or >= 1000
            ? throw new FormatException("Invalid argument to time encode")
            : ((hour * 3600000) + (minute * 60000) + (second * 1000) + millisecond) / 86400000.0;
    }

    // SYSUTILS.PAS DateTimeToTimeStamp rounds to milliseconds and uses the absolute fraction.
    private static void DecodeTime(double value, out int hour, out int minute, out int second, out int millisecond)
    {
        long ticks = (long)Math.Round(value * 86400000, MidpointRounding.ToEven);
        int ms = (int)Math.Abs(ticks % 86400000);
        hour = ms / 3600000; minute = ms / 60000 % 60; second = ms / 1000 % 60; millisecond = ms % 1000;
    }

    private static void DecodeDate(double value, out int year, out int month, out int day)
    {
        long rounded = (long)Math.Round(value * 86400000, MidpointRounding.ToEven);
        long days = rounded / 86400000;
        if (days + (Epoch - DateTime.MinValue).Days < 0) { year = month = day = 0; return; }
        DateTime date = Epoch.AddDays(days); year = date.Year; month = date.Month; day = date.Day;
    }

    public static string DateTimeToStr(double value)
    {
        DecodeDate(value, out int y, out int m, out int d); DecodeTime(value, out int h, out int n, out int s, out int ms);
        return new DateTime(y, m, d, h, n, s, ms).ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
    }
}
