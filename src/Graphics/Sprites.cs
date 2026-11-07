using Aow.Rtl;

namespace Aow.Graphics;

public static partial class Blit
{
    private static int Dark(SurfaceView dst, int at, int m1, int m2)
    {
        int p = dst.Word(at);
        return ((p >> 1) & m1) + ((p & m2) >> 2);
    }

    private static bool Visible(int x, int y, in TRect clip) => x >= clip.Left && x < clip.Right && y >= clip.Top && y < clip.Bottom;
    public static void BltRLE16(int x, int y, SurfaceView dst, ReadOnlySpan<byte> data, int w, int h, int key) => Rle16(x, y, dst, data, w, h, key, false, false);
    public static void BlendRLE555(int x, int y, SurfaceView dst, ReadOnlySpan<byte> data, int w, int h, int key) => Rle16(x, y, dst, data, w, h, key, true, false);
    public static void BlendRLE565(int x, int y, SurfaceView dst, ReadOnlySpan<byte> data, int w, int h, int key) => Rle16(x, y, dst, data, w, h, key, true, true);

    private static void Rle16(int x, int y, SurfaceView dst, ReadOnlySpan<byte> data, int w, int h, int key, bool blend, bool rgb565)
    {
        var clip = dst.Clip;
        int pitch = dst.Pitch;
        if (w <= 0 || h <= 0 || x >= clip.Right || x + w <= clip.Left || y >= clip.Bottom || y + h <= clip.Top) return;
        bool fast = x >= clip.Left && x + w <= clip.Right;
        int row = 0;
        for (int ry = 0; ry < h; ry++)
        {
            int length = (int)Dword(data, row), end = row + length, p = row + 4, dx = x;
            if (y + ry >= clip.Top && y + ry < clip.Bottom)
            {
                int dest = (y + ry) * pitch + x * 2;
                if (blend && fast) new RleBlend { Data = data, P = p, Count = length - 4, Dst = dst, D = dest, Key = key, Rgb565 = rgb565 }.Run();
                else while (p < end)
                {
                    int pixel = Word(data, p); p += 2;
                    if (pixel == key) { dx += Word(data, p) / 2; p += 2; }
                    else
                    {
                        if (Visible(dx, y + ry, clip))
                        {
                            int at = (y + ry) * pitch + dx * 2;
                            dst.Word(at, blend ? Half(pixel, dst.Word(at), rgb565) : pixel);
                        }
                        dx++;
                    }
                }
            }
            row = (end + 3) & ~3;
        }
    }

    // ImageLib's fast path tests eight encoded bytes, then handles two pairs.
    // The low blue bit is cleared on the high member of each opaque pair.
    private ref struct RleBlend
    {
        public ReadOnlySpan<byte> Data;
        public SurfaceView Dst;
        public int P;
        public int Count;
        public int D;
        public int Key;
        public bool Rgb565;

        private void Scalar()
        {
            int pixel = Word(Data, P); P += 2; Count -= 2;
            if (pixel == Key) { D += Word(Data, P); P += 2; Count -= 2; }
            else { Dst.Word(D, Half(pixel, Dst.Word(D), Rgb565)); D += 2; }
        }

        private void Pair()
        {
            Dst.Word(D, Half(Word(Data, P), Dst.Word(D), Rgb565));
            Dst.Word(D + 2, Half(Word(Data, P + 2), Dst.Word(D + 2), Rgb565, true));
            P += 4; Count -= 4; D += 4;
        }

        public void Run()
        {
            while (Count > 0)
            {
                if (Count < 8) { Scalar(); continue; }
                if (Word(Data, P) == Key)
                {
                    Scalar();
                    if (Word(Data, P) == Key) { Scalar(); continue; }
                }
                else if (Word(Data, P + 2) == Key)
                {
                    Scalar(); Scalar(); Scalar(); continue;
                }
                else Pair();
                if (Word(Data, P) == Key) Scalar();
                else if (Word(Data, P + 2) != Key) Pair();
                else
                {
                    Scalar(); Scalar();
                    while (Count > 0 && Word(Data, P) == Key) Scalar();
                    if (Count > 0) Scalar();
                }
            }
        }
    }

    private static void RemapRle(Span<byte> data, int h, int key, bool to565)
    {
        int row = 0;
        for (int y = 0; y < h; y++)
        {
            int end = row + (int)Dword(data, row);
            for (int p = row + 4; p < end; p += 2)
            {
                int pixel = Word(data, p);
                if (pixel == key) p += 2;
                else
                {
                    int converted = to565 ? ((pixel << 1) & 0xFFC0) | (pixel & 31) : ((pixel >> 1) & 0xFFE0) | (pixel & 31);
                    if (converted == key) converted += to565 ? 1 : 0x400;
                    Word(data, p, converted);
                }
            }
            row = (end + 3) & ~3;
        }
    }

    public static void RemapRLE555to565(Span<byte> data, int h, int key) => RemapRle(data, h, key, true);
    public static void RemapRLE565to555(Span<byte> data, int h, int key) => RemapRle(data, h, key, false);
    public static void ShowRLE08(int x, int y, SurfaceView dst, ReadOnlySpan<byte> data, int w, int h, byte key) => Rle08(x, y, dst, data, w, h, key, default, false);
    public static void ShowRLE08to16(int x, int y, SurfaceView dst, ReadOnlySpan<byte> data, int w, int h, byte key, ReadOnlySpan<byte> palette) => Rle08(x, y, dst, data, w, h, key, palette, true);

    private static void Rle08(int x, int y, SurfaceView dst, ReadOnlySpan<byte> data, int w, int h, byte key, ReadOnlySpan<byte> palette, bool paletteMode)
    {
        var clip = dst.Clip;
        int pitch = dst.Pitch;
        int p = 0;
        for (int ry = 0; ry < h; ry++)
        {
            p += 4;
            for (int dx = 0; dx < w;)
            {
                byte pixel = data[p++];
                if (pixel == key) dx += data[p++];
                else
                {
                    // ShowRLE08 ignores the clip rectangle. The palette variant clips.
                    if (!paletteMode) dst[(y + ry) * pitch + x + dx] = pixel;
                    else if (Visible(x + dx, y + ry, clip)) dst.Word((y + ry) * pitch + (x + dx) * 2, Word(palette, pixel * 4));
                    dx++;
                }
            }
        }
    }

    public static void ShowBitMask16(int x, int y, SurfaceView dst, ReadOnlySpan<byte> data, int w, int h) => Mask(x, y, dst, data, w, h, false, false);
    public static void ShowShadow555(int x, int y, SurfaceView dst, ReadOnlySpan<byte> data, int w, int h) => Mask(x, y, dst, data, w, h, true, false);
    public static void ShowShadow565(int x, int y, SurfaceView dst, ReadOnlySpan<byte> data, int w, int h) => Mask(x, y, dst, data, w, h, true, true);

    private static void Mask(int x, int y, SurfaceView dst, ReadOnlySpan<byte> data, int w, int h, bool shadow, bool rgb565)
    {
        var clip = dst.Clip;
        int pitch = dst.Pitch;
        int clipLeft = clip.Left, clipRight = clip.Right, clipTop = clip.Top, clipBottom = clip.Bottom;
        if (x + w < clipLeft || x >= clipRight || clipRight <= clipLeft || y + h < clipTop || y >= clipBottom || clipBottom <= clipTop)
            return;
        int words = (w + 31) >> 5, sourcePitch = words * 4;
        int first = Math.Max(0, clipTop - y), last = Math.Min(h, clipBottom - y);
        bool clipped = x < clipLeft || x + w > clipRight;
        int left = 0, limit = w * 2, skip = 0;
        if (clipped)
        {
            int cut = Math.Max(0, clipLeft - x); left = cut & 31;
            skip = cut >> 5;
            if (skip != 0) { words -= skip; limit -= skip * 64; x += skip * 32; }
            int right = x + w - skip * 32 - clipRight;
            if (right >= 0) { limit -= right * 2; words -= (right >> 5) + 1; }
        }
        if (clipped && words == 0 && left == 0) throw new EAccessViolation("Original clipped mask loop underflows its word count");
        int m1 = rgb565 ? 0x7BEF : 0x3DEF, m2 = rgb565 ? 0xE79C : 0x739C;
        uint pair1 = rgb565 ? 0x7BEF7BEFu : 0x3DEF3DEFu, pair2 = rgb565 ? 0xE79CE79Cu : 0x739C739Cu;
        for (int ry = first; ry < last; ry++)
        {
            int p = ry * sourcePitch + skip * 4, d = (y + ry) * pitch + x * 2, offset = left * 2, next = 64, remaining = words;
            uint bits = Dword(data, p);
            if (left != 0) { bits <<= left; p += 4; }
            else if (!shadow) p += 4;
            bool partial = clipped && left != 0 && remaining == 0;
            if (!partial)
            {
                do
                {
                    while (true)
                    {
                        bool set = (bits & 0x80000000u) != 0; bits <<= 1;
                        if (!set) { if (bits == 0) break; offset += 2; continue; }
                        if (!shadow) { dst.Word(d + offset, 0xFFFF); offset += 2; if (bits == 0) break; continue; }
                        if (bits == 0) { dst.Word(d + offset, Dark(dst, d + offset, m1, m2)); break; }
                        bool second = (bits & 0x80000000u) != 0; bits <<= 1;
                        if (second)
                        {
                            uint pixel = dst.Dword(d + offset);
                            dst.Dword(d + offset, ((pixel >> 1) & pair1) + ((pixel & pair2) >> 2)); offset += 4;
                        }
                        else
                        {
                            int pixel = Dark(dst, d + offset, m1, m2); offset += 4;
                            dst.Word(d + offset, pixel);
                        }
                        if (bits == 0) break;
                    }
                    if (shadow) p += 4;
                    remaining--;
                    if (!clipped && remaining == 0) break;
                    offset = next; next += 64;
                    if (remaining == 0) break;
                    bits = Dword(data, p);
                    if (!shadow) p += 4;
                } while (true);
                if (!clipped || limit <= offset) continue;
                bits = Dword(data, p);
            }
            while (true)
            {
                bool set = (bits & 0x80000000u) != 0; bits <<= 1;
                if (set)
                {
                    int pixel = shadow ? Dark(dst, d + offset, m1, m2) : 0xFFFF;
                    if (shadow) offset += 2;
                    dst.Word(d + offset, pixel);
                    if (!shadow) offset += 2;
                }
                else offset += 2;
                if (bits == 0 || limit <= offset) break;
            }
        }
    }
}
