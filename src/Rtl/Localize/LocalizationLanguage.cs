using System.Globalization;
using System.Runtime.InteropServices;

namespace Aow.Rtl.Localize;

public static class LocalizationLanguage
{
    public static int FromTag(string tag) => tag.Split('-', '_')[0].ToLowerInvariant() switch
    {
        "de" => 2,
        "fr" => 3,
        "it" => 4,
        "es" => 5,
        _ => 1
    };

    public static int Automatic()
    {
        if (OperatingSystem.IsWindows())
            return (GetUserDefaultUILanguage() & 0x3ff) switch
            {
                7 => 2,
                12 => 3,
                16 => 4,
                10 => 5,
                _ => 1
            };
        string? tag = Environment.GetEnvironmentVariable("LC_ALL");
        if (string.IsNullOrEmpty(tag)) tag = Environment.GetEnvironmentVariable("LC_MESSAGES");
        if (string.IsNullOrEmpty(tag)) tag = Environment.GetEnvironmentVariable("LANG");
        return FromTag(tag ?? CultureInfo.CurrentUICulture.Name);
    }

    [DllImport("kernel32.dll")]
    private static extern ushort GetUserDefaultUILanguage();
}
