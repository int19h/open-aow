namespace Aow.Rtl;

public static class DelphiOrdinals
{
    public static byte UpCase(byte value) => value is >= (byte)'a' and <= (byte)'z' ? (byte)(value - 32) : value;
    public static char UpCase(char value) => value is >= 'a' and <= 'z' ? (char)(value - 32) : value;
}
