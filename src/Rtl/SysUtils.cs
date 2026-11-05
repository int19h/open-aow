using System.Globalization;

namespace Aow.Rtl;

public static partial class RtlUnit
{
    public static string IntToStr(int value) => value.ToString(CultureInfo.InvariantCulture);
    // SYSUTILS.PAS IntToHex uses FormatBuf, which ignores precision outside 0..15.
    public static string IntToHex(int value, int digits) => ((uint)value).ToString("X", CultureInfo.InvariantCulture).PadLeft(digits is > 0 and < 16 ? digits : 0, '0');
    internal static byte Fold1252(byte c) => c switch
    {
        >= 65 and <= 90 or >= 192 and <= 214 or >= 216 and <= 222 => (byte)(c + 32),
        138 => 154,
        140 => 156,
        142 => 158,
        159 => 255,
        _ => c
    };
    private static byte CollationByte(byte c) => c switch { 173 => 0, 136 => 94, _ => c };
}
