using Aow.Engine.Buffers;

using Aow.Rtl;

namespace Aow.Game.Draw;

internal static class MemorySurfaces
{
    public static SurfaceDesc Describe(int w, int h, int pitch, int format, byte[] bits, GFXEPalette palette = null)
    {
        var sd = new SurfaceDesc
        {
            Width = w,
            Height = h,
            BitCount = format & 255,
            Pitch = pitch,
            PixelFormatID = format,
            Data = bits,
            Palette = palette,
            ClipRect = new Rect(0, 0, w, h)
        };
        PFIDtoMASK(format, ref sd.RMask, ref sd.GMask, ref sd.BMask, ref sd.AMask);
        return sd;
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
        if (size == 0) return null;
        var result = new byte[size];
        if (memory != null) memory.AsSpan(0, Math.Min(memory.Length, size)).CopyTo(result);
        return result;
    }

    private static int PFIDtoMASK(int PixelFormatID, ref int RMask, ref int GMask, ref int BMask, ref int AMask)
    {
        var order = default(FixedArray_byte_4);
        var masks = default(FixedArray_int_4);
        int bitCount = PixelFormatID & 255;
        int pos = 0;
        order[(int)((uint)PixelFormatID >> 8) & 3] = 0;
        order[(int)((uint)PixelFormatID >> 10) & 3] = 1;
        order[(int)((uint)PixelFormatID >> 12) & 3] = 2;
        order[(int)((uint)PixelFormatID >> 14) & 3] = 3;
        for (int i = 0; i <= 3; i++)
        {
            int bits = (int)((uint)PixelFormatID >> (order[i] * 4 + 16)) & 15;
            int j = bits;
            int m = 0;
            while (j != 0)
            {
                m = m * 2 + 1;
                j--;
            }

            m <<= pos;
            pos += bits;
            masks[order[i]] = m;
        }

        AMask = masks[0];
        BMask = masks[1];
        GMask = masks[2];
        RMask = masks[3];
        return bitCount;
    }
}
