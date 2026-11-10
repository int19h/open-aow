using System.Runtime.InteropServices;
using Aow.Graphics;

namespace Aow.Game.Draw;

public static partial class GFXEUnit
{
    public static void RemapSurface08to555(SurfaceView source, SurfaceView destination, int width, int height, TGFXEPalette palette) =>
        RemapIndexedSurface(source, destination, width, height, palette, false);

    public static void RemapSurface08to565(SurfaceView source, SurfaceView destination, int width, int height, TGFXEPalette palette) =>
        RemapIndexedSurface(source, destination, width, height, palette, true);

    private static void RemapIndexedSurface(SurfaceView source, SurfaceView destination, int width, int height, TGFXEPalette palette, bool rgb565)
    {
        for (int y = 0; y < height; y++)
        {
            var src = source.Row(y, width);
            var dst = MemoryMarshal.Cast<byte, ushort>(destination.Row(y, width * 2));
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
