namespace Aow.Rtl;

public static partial class RtlUnit
{
    public static byte Chr(int value) => unchecked((byte)value);
    public static int Ord(byte value) => value;
    public static int Ord(int value) => value;
    public static int Ord(uint value) => unchecked((int)value);
    public static int Ord(bool value) => value ? 1 : 0;
    public static int Ord<E>(E value) where E : unmanaged, Enum => Convert.ToInt32(value);
    public static byte UpCase(byte value) => value is >= (byte)'a' and <= (byte)'z' ? (byte)(value - 32) : value;

    public static int Ord(char value) => Cp1252.Byte(value);
    public static char UpCase(char value) => value is >= 'a' and <= 'z' ? (char)(value - 32) : value;
}
