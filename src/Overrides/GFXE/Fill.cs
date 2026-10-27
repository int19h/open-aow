namespace Aow.Units.GFXE;

public static partial class GFXE
{
    public static void FillInt(Span<byte> destination, int count, int value) => Aow.Graphics.Blit.FillInt(destination, count, value);
    public static void FillWord(Span<byte> destination, int count, int value) => Aow.Graphics.Blit.FillWord(destination, count, value);
    public static void FillTrippleByte(Span<byte> destination, int count, int value) => Aow.Graphics.Blit.FillTrippleByte(destination, count, value);
    public static int RDTSC() => 0;
    public static bool CPUIDSupported() => false;
    public static bool MMXsupport() => false;
}
