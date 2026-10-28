using Aow.Rtl;

namespace Aow.Units.GFXE;

public static partial class GFXE
{
    public static void RemapSurface08to555(object source, object destination, int sourcePitch,
        int destinationPitch, int width, int height, TGFXEPalette palette) =>
        RemapIndexedSurface(source, destination, sourcePitch, destinationPitch, width, height, palette, false);

    public static void RemapSurface08to565(object source, object destination, int sourcePitch,
        int destinationPitch, int width, int height, TGFXEPalette palette) =>
        RemapIndexedSurface(source, destination, sourcePitch, destinationPitch, width, height, palette, true);

    private static void RemapIndexedSurface(object source, object destination, int sourcePitch,
        int destinationPitch, int width, int height, TGFXEPalette palette, bool rgb565)
    {
        var input = Rtl.Rtl.BytePointer(source);
        var output = Rtl.Rtl.BytePointer(destination);
        for (int y = 0; y < height; y++)
        {
            var src = input + y * sourcePitch;
            var dst = (output + y * destinationPitch).Reinterpret<ushort>();
            for (int x = 0; x < width; x++)
            {
                ref var entry = ref palette.Entries[src[x]];
                dst[x] = (ushort)(rgb565
                    ? ((entry.R & 248) << 8) + ((entry.G & 252) << 3) + (entry.B >> 3)
                    : ((entry.R & 248) << 7) + ((entry.G & 248) << 2) + (entry.B >> 3));
            }
        }
    }
}
