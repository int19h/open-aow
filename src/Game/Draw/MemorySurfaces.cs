#nullable disable
using Aow.Rtl;

namespace Aow.Game.Draw;

internal static class MemorySurfaces
{
    public static LockedSurface Describe(int w, int h, int pitch, int format, byte[] bits, Palette palette = null)
    {
        LockedSurface lockedSurface = new()
        {
            Width = w,
            Height = h,
            BitCount = format & 255,
            Pitch = pitch,
            PixelFormatId = format,
            Data = bits,
            Palette = palette,
            ClipRectangle = new Rect(0, 0, w, h)
        };
        int rMask = lockedSurface.RMask;
        int gMask = lockedSurface.GMask;
        int bMask = lockedSurface.BMask;
        int aMask = lockedSurface.AMask;
        PixelFormatIdToMask(format, out rMask, out gMask, out bMask, out aMask);
        lockedSurface.RMask = rMask;
        lockedSurface.GMask = gMask;
        lockedSurface.BMask = bMask;
        lockedSurface.AMask = aMask;
        return lockedSurface;
    }

    public static (int Pitch, byte[] Bits) Allocate(int w, int h, int format, int alignment)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(w);
        ArgumentOutOfRangeException.ThrowIfNegative(h);
        int pitch = checked((checked(w * (((format & 255) + 7) / 8)) + alignment - 1) & -alignment);
        return (pitch, new byte[checked(pitch * h)]);
    }

    // ReallocMem of a pixel buffer: size 0 frees it, otherwise the new array keeps the bytes the
    // two sizes share.
    public static byte[] Reallocate(byte[] memory, int size)
    {
        if (size == 0)
        {
            return null;
        }

        byte[] result = new byte[size];
        memory?.AsSpan(0, Math.Min(memory.Length, size)).CopyTo(result);

        return result;
    }

    private static int PixelFormatIdToMask(int pixelFormatId, out int rMask, out int gMask, out int bMask, out int mask)
    {
        Span<byte> order = stackalloc byte[4];
        Span<int> masks = stackalloc int[4];
        int bitCount = pixelFormatId & 255;
        int position = 0;
        order[(int)((uint)pixelFormatId >> 8) & 3] = 0;
        order[(int)((uint)pixelFormatId >> 10) & 3] = 1;
        order[(int)((uint)pixelFormatId >> 12) & 3] = 2;
        order[(int)((uint)pixelFormatId >> 14) & 3] = 3;
        for (int i = 0; i <= 3; i++)
        {
            int bits = (int)((uint)pixelFormatId >> ((order[i] * 4) + 16)) & 15;
            int j = bits;
            int m = 0;
            while (j != 0)
            {
                m = (m * 2) + 1;
                j--;
            }

            m <<= position;
            position += bits;
            masks[order[i]] = m;
        }

        mask = masks[0];
        bMask = masks[1];
        gMask = masks[2];
        rMask = masks[3];
        return bitCount;
    }
}
