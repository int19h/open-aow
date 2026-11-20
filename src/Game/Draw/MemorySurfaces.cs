#nullable disable
using Aow.Rtl;

namespace Aow.Game.Draw;

internal static class MemorySurfaces
{
    public static LockedSurface Describe(int w, int h, int pitch, PixelFormat format, byte[] bits, Palette palette = null)
    {
        LockedSurface lockedSurface = new()
        {
            Width = w,
            Height = h,
            BitCount = format.BitCount(),
            Pitch = pitch,
            PixelFormatId = format,
            Data = bits,
            Palette = palette,
            ClipRectangle = new Rect(0, 0, w, h)
        };
        _ = lockedSurface.RMask;
        _ = lockedSurface.GMask;
        _ = lockedSurface.BMask;
        _ = lockedSurface.AMask;
        _ = PixelFormatIdToMask(format, out int rMask, out int gMask, out int bMask, mask: out int aMask);
        lockedSurface.RMask = rMask;
        lockedSurface.GMask = gMask;
        lockedSurface.BMask = bMask;
        lockedSurface.AMask = aMask;
        return lockedSurface;
    }

    public static (int Pitch, byte[] Bits) Allocate(int w, int h, PixelFormat format, int alignment)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(w);
        ArgumentOutOfRangeException.ThrowIfNegative(h);
        int pitch = checked((checked(w * ((format.BitCount() + 7) / 8)) + alignment - 1) & -alignment);
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
        memory?.AsSpan(0, length: Math.Min(memory.Length, size)).CopyTo(result);

        return result;
    }

    private static int PixelFormatIdToMask(PixelFormat pixelFormatId, out int rMask, out int gMask, out int bMask, out int mask)
    {
        Span<byte> order = stackalloc byte[4];
        Span<int> masks = stackalloc int[4];
        int raw = (int)pixelFormatId;
        int bitCount = pixelFormatId.BitCount();
        int position = 0;
        order[(raw >>> 8) & 0b0011] = 0;
        order[(raw >>> 10) & 0b0011] = 1;
        order[(raw >>> 12) & 0b0011] = 2;
        order[(raw >>> 14) & 0b0011] = 3;
        for (int i = 0; i <= 3; i++)
        {
            int bits = (raw >>> ((order[i] * 4) + 16)) & 0xF;
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
