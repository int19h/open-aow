using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace Aow.Graphics;

public static partial class Blit
{
    internal static ushort UInt16(ReadOnlySpan<byte> s, int offset) => BinaryPrimitives.ReadUInt16LittleEndian(s.Slice(offset, 2));

    internal static uint Dword(ReadOnlySpan<byte> s, int offset) => BinaryPrimitives.ReadUInt32LittleEndian(s.Slice(offset, 4));

    internal static void UInt16(Span<byte> d, int offset, int value) => BinaryPrimitives.WriteUInt16LittleEndian(d.Slice(offset, 2), unchecked((ushort)value));

    internal static void Dword(Span<byte> d, int offset, uint value) => BinaryPrimitives.WriteUInt32LittleEndian(d.Slice(offset, 4), value);

    private static Span<ushort> Words(Span<byte> row) => MemoryMarshal.Cast<byte, ushort>(row);

    private static Span<uint> Dwords(Span<byte> row) => MemoryMarshal.Cast<byte, uint>(row);

    public static void BlitRectangle(SurfaceView s, SurfaceView d, int w, int h) => RegisterBlitRectangle(s, d, w, h);

    // Whether a row copy of `count` bytes writes what the original's forward loop writes: both
    // rows lie inside their blocks and the destination does not start inside the source row after
    // its first byte. A destination in another block, or at or before the source, receives every
    // source byte before the loop writes over it. `shared` and `shift` describe the two
    // blocks: whether they overlap and where the destination block starts in the source block.
    private static bool Plain(SurfaceView s, int si, SurfaceView d, int di, int count, bool shared, int shift)
    {
        if (si < 0 || di < 0 || si > s.Memory.Length - count || di > d.Memory.Length - count) return false;
        int at = di + shift;
        return !shared || at <= si || at >= si + count;
    }

    // The assembler aligns the source to 4 with single bytes, copies pairs of dwords, a dword and
    // the remaining bytes. When the head and the tail exceed the width the dword count wraps around
    // and the loop runs off the block, so the original's order is kept for that case, for a row
    // that leaves its block and for a destination that starts inside the source row ahead of it.
    public static void RegisterBlitRectangle(SurfaceView s, SurfaceView d, int w, int h)
    {
        if (w == 0 || h == 0) return;
        int head = (-s.Start) & 3, tail = (s.Start + w) & 3;
        bool whole = w - tail - head >= 0;
        uint words = unchecked((uint)(w - tail - head)) >> 2;
        bool shared = s.Memory.Overlaps(d.Memory, out int shift);
        for (int y = 0; y < h; y++)
        {
            int si = s.RowIndex(y), di = d.RowIndex(y);
            if (whole && Plain(s, si, d, di, w, shared, shift)) s.Memory.Slice(si, w).CopyTo(d.Memory.Slice(di, w));
            else RegisterRow(s.Memory, si, d.Memory, di, head, words, tail);
        }
    }

    private static void RegisterRow(Span<byte> source, int si, Span<byte> destination, int di, int head, uint words, int tail)
    {
        int offset = 0;
        for (int i = 0; i < head; i++) { byte value = source[si + offset]; destination[di + offset++] = value; }
        for (uint i = 0; i < (words >> 1); i++)
        {
            uint first = Dword(source, si + offset), second = Dword(source, si + offset + 4);
            Dword(destination, di + offset, first); Dword(destination, di + offset + 4, second); offset += 8;
        }
        if ((words & 1) != 0) { uint value = Dword(source, si + offset); Dword(destination, di + offset, value); offset += 4; }
        for (int i = 0; i < tail; i++) { byte value = source[si + offset]; destination[di + offset++] = value; }
    }

    // The MMX and floating point copies align the source to 8 when the row is 31 bytes or more,
    // move blocks of 32 bytes (all four quadwords read before the first is written, the floating
    // point version writing them last to first), then dwords and bytes. The rows run in a
    // do-while loop, so a zero or negative height with a short row runs until it leaves the
    // block, which the original's order reproduces.
    private static void CopyRows(SurfaceView s, SurfaceView d, int w, int h, bool floating)
    {
        if (w == 0 || (w >= 31 && h == 0)) return;
        int head = w >= 31 ? (-s.Start) & 7 : 0;
        int blocks = w >= 31 ? (w - head) >> 5 : 0;
        int words = (w - head - blocks * 32) >> 2, tail = (w - head) & 3;
        bool whole = w > 0 && h > 0;
        bool shared = s.Memory.Overlaps(d.Memory, out int shift);
        int y = 0;
        do
        {
            int si = s.RowIndex(y), di = d.RowIndex(y);
            if (whole && Plain(s, si, d, di, w, shared, shift)) s.Memory.Slice(si, w).CopyTo(d.Memory.Slice(di, w));
            else CopyRow(s.Memory, si, d.Memory, di, head, blocks, words, tail, floating);
        } while (++y != h);
    }

    private static void CopyRow(Span<byte> src, int si, Span<byte> dst, int di, int head, int blocks, int words, int tail, bool floating)
    {
        int offset = 0;
        for (int i = 0; i < head; i++) { byte value = src[si + offset]; dst[di + offset++] = value; }
        for (int i = 0; i < blocks; i++)
        {
            int at = si + offset, to = di + offset;
            ulong a = UInt64(src, at), b = UInt64(src, at + 8), c = UInt64(src, at + 16), e = UInt64(src, at + 24);
            if (floating)
            {
                UInt64(dst, to + 24, e); UInt64(dst, to + 16, c);
                UInt64(dst, to + 8, b); UInt64(dst, to, a);
            }
            else
            {
                UInt64(dst, to, a); UInt64(dst, to + 8, b);
                UInt64(dst, to + 16, c); UInt64(dst, to + 24, e);
            }
            offset += 32;
        }
        for (int i = 0; i < words; i++) { uint value = Dword(src, si + offset); Dword(dst, di + offset, value); offset += 4; }
        for (int i = 0; i < tail; i++) { byte value = src[si + offset]; dst[di + offset++] = value; }
    }

    private static ulong UInt64(Span<byte> s, int offset) => BinaryPrimitives.ReadUInt64LittleEndian(s.Slice(offset, 8));

    private static void UInt64(Span<byte> d, int offset, ulong value) => BinaryPrimitives.WriteUInt64LittleEndian(d.Slice(offset, 8), value);

    public static void FPBltRect(SurfaceView s, SurfaceView d, int w, int h) => CopyRows(s, d, w, h, true);

    public static void MMXBltRect(SurfaceView s, SurfaceView d, int w, int h) => CopyRows(s, d, w, h, false);

    // `rep movsd` with the bytes before and after: a row shorter than 4 bytes is copied byte by
    // byte, a longer one as the bytes that align its end, the dwords and the tail bytes. A
    // negative width copies nothing.
    public static void RepMovsdBlitRectangle(SurfaceView s, SurfaceView d, int w, int h)
    {
        if (w <= 0 || h <= 0) return;
        int tail = w < 4 ? w : (s.Start + w) & 3;
        int head = w < 4 ? 0 : (w - tail) & 3;
        int words = w < 4 ? 0 : (w - tail) >> 2;
        bool shared = s.Memory.Overlaps(d.Memory, out int shift);
        for (int y = 0; y < h; y++)
        {
            int si = s.RowIndex(y), di = d.RowIndex(y);
            if (Plain(s, si, d, di, w, shared, shift)) s.Memory.Slice(si, w).CopyTo(d.Memory.Slice(di, w));
            else
            {
                int offset = 0;
                for (int i = 0; i < head; i++) { byte value = s.Memory[si + offset]; d.Memory[di + offset++] = value; }
                for (int i = 0; i < words; i++) { uint value = Dword(s.Memory, si + offset); Dword(d.Memory, di + offset, value); offset += 4; }
                for (int i = 0; i < tail; i++) { byte value = s.Memory[si + offset]; d.Memory[di + offset++] = value; }
            }
        }
    }

    public static void BlitColorKey16(SurfaceView s, SurfaceView d, int w, int h, int key)
    {
        for (int y = 0; y < h; y++)
        {
            var source = Words(s.Row(y, w * 2));
            var destination = Words(d.Row(y, w * 2));
            for (int x = 0; x < w; x++)
            {
                int pixel = source[x];
                if (pixel != key) destination[x] = (ushort)pixel;
            }
        }
    }

    internal static int Half(int s, int d, bool rgb565, bool high = false)
    {
        int mask = rgb565 ? 0xF7DF : 0x7BDF;
        if (high) mask &= ~1;
        return ((s & mask) + (d & mask)) >> 1;
    }

    private static void Transparent(SurfaceView s, SurfaceView d, int w, int h, bool rgb565)
    {
        int paired = ((s.Start | d.Start) & 3) == 0 ? w & ~3 : 0;
        for (int y = 0; y < h; y++)
        {
            var source = Words(s.Row(y, w * 2));
            var destination = Words(d.Row(y, w * 2));
            for (int x = 0; x < w; x++) destination[x] = (ushort)Half(source[x], destination[x], rgb565, x < paired && (x & 1) != 0);
        }
    }

    public static void BlitTransparent555(SurfaceView s, SurfaceView d, int w, int h) => Transparent(s, d, w, h, false);

    public static void BlitTransparent565(SurfaceView s, SurfaceView d, int w, int h) => Transparent(s, d, w, h, true);

    public static void FillInteger(Span<byte> d, int count, int value)
    {
        for (uint i = 0; i < unchecked((uint)count); i++)
        {
            if (i >= (uint)(d.Length / 4)) throw new AccessViolationException("FillInt crossed the buffer");
            BinaryPrimitives.WriteInt32LittleEndian(d.Slice((int)i * 4, 4), value);
        }
    }

    public static void FillWord(Span<byte> d, int count, int value)
    {
        uint pair = unchecked(((uint)value << 16) + (uint)value), pairs = (uint)count >> 1;
        for (uint i = 0; i < pairs; i++)
        {
            if (i >= (uint)(d.Length / 4)) throw new AccessViolationException("FillWord crossed the buffer");
            BinaryPrimitives.WriteUInt32LittleEndian(d.Slice((int)i * 4, 4), pair);
        }
        if ((count & 1) != 0)
        {
            int offset = checked((int)pairs * 4);
            if (offset > d.Length - 2) throw new AccessViolationException("FillWord crossed the buffer");
            BinaryPrimitives.WriteUInt16LittleEndian(d.Slice(offset, 2), (ushort)pair);
        }
    }

    public static void FillTripleByte(Span<byte> d, int count, int value)
    {
        int offset = 0;
        do
        {
            if (offset > d.Length - 2) throw new AccessViolationException("FillTrippleByte crossed the buffer");
            BinaryPrimitives.WriteUInt16LittleEndian(d.Slice(offset, 2), (ushort)value);
            if (offset + 2 >= d.Length) throw new AccessViolationException("FillTrippleByte crossed the buffer");
            d[offset + 2] = (byte)(value >> 16); offset += 3;
        } while (--count != 0);
    }

    // The aligned remappers read and write four pixels at a time and run past a row shorter than
    // four pixels. The port raises there.
    private static void CheckShortRows(SurfaceView s, SurfaceView d, int w, int h)
    {
        if (w > 0 && w < 4 && h > 0 && ((s.Start | d.Start) & 3) == 0)
            throw new AccessViolationException("Original aligned remapper overruns short rows");
    }

    public static void RemapSurface16To16(SurfaceView s, SurfaceView d, ReadOnlySpan<byte> table, int w, int h)
    {
        CheckShortRows(s, d, w, h);
        var map = MemoryMarshal.Cast<byte, ushort>(table);
        for (int y = 0; y < h; y++)
        {
            var source = Words(s.Row(y, w * 2));
            var destination = Words(d.Row(y, w * 2));
            for (int x = 0; x < w; x++) destination[x] = map[source[x]];
        }
    }

    public static void RemapSurface16To08(SurfaceView s, SurfaceView d, ReadOnlySpan<byte> table, int w, int h)
    {
        CheckShortRows(s, d, w, h);
        for (int y = 0; y < h; y++)
        {
            var source = Words(s.Row(y, w * 2));
            var destination = d.Row(y, w);
            for (int x = 0; x < w; x++) destination[x] = table[source[x]];
        }
    }

    public static void RemapSurface16To32(SurfaceView s, SurfaceView d, ReadOnlySpan<byte> table, int w, int h)
    {
        CheckShortRows(s, d, w, h);
        var map = MemoryMarshal.Cast<byte, uint>(table);
        for (int y = 0; y < h; y++)
        {
            var source = Words(s.Row(y, w * 2));
            var destination = Dwords(d.Row(y, w * 4));
            for (int x = 0; x < w; x++) destination[x] = map[source[x]];
        }
    }

    internal static int To565(int p) => ((p & 0x7FE0) << 1) + (p & 31);

    internal static int To555(int p) => ((p & 0xFFC0) >> 1) + (p & 31);

    // In place (TImage16.RemapToPixelFormat passes one buffer as both) each pixel is read before
    // it is written, as in the original.
    public static void RemapSurface555To565(SurfaceView s, SurfaceView d, int w, int h)
    {
        for (int y = 0; y < h; y++)
        {
            var source = Words(s.Row(y, w * 2));
            var destination = Words(d.Row(y, w * 2));
            for (int x = 0; x < w; x++) destination[x] = (ushort)To565(source[x]);
        }
    }

    public static void RemapSurface565To555(SurfaceView s, SurfaceView d, int w, int h)
    {
        for (int y = 0; y < h; y++)
        {
            var source = Words(s.Row(y, w * 2));
            var destination = Words(d.Row(y, w * 2));
            for (int x = 0; x < w; x++) destination[x] = (ushort)To555(source[x]);
        }
    }

    public static void RemapSurface555To32(SurfaceView s, SurfaceView d, int w, int h)
    {
        CheckShortRows(s, d, w, h);
        int paired = ((s.Start | d.Start) & 3) == 0 ? w & ~3 : 0;
        for (int y = 0; y < h; y++)
        {
            var source = Words(s.Row(y, w * 2));
            var destination = Dwords(d.Row(y, w * 4));
            for (int x = 0; x < w; x++)
            {
                int p = source[x];
                destination[x] = (uint)((x < paired ? (p & 0x7C00) << 9 : 0) + ((p & 0x3E0) << 6) + ((p & 31) << 3));
            }
        }
    }

    public static void RemapSurface565To32(SurfaceView s, SurfaceView d, int w, int h)
    {
        CheckShortRows(s, d, w, h);
        bool paired = ((s.Start | d.Start) & 3) == 0;
        for (int y = 0; y < h; y++)
        {
            var source = Words(s.Row(y, w * 2));
            var destination = Dwords(d.Row(y, w * 4));
            for (int x = 0; x < w; x++)
            {
                int p = source[x];
                destination[x] = (uint)(((p & 0xF800) << 8) + ((p & (paired && x < (w & ~3) && (x & 3) == 3 ? 0x7C0 : 0x7E0)) << 5) + ((p & 31) << 3));
            }
        }
    }

    internal static int BlendPixel(int s, int d, ReadOnlySpan<byte> table, bool rgb565)
    {
        int g = rgb565 ? 6 : 5, r = rgb565 ? 11 : 10;
        return table[((s & 31) << 5) + (d & 31)] + (table[(((s >> g) & 31) << 5) + ((d >> g) & 31)] << g) + (table[(((s >> r) & 31) << 5) + ((d >> r) & 31)] << r);
    }

    private static void Blend(SurfaceView s, SurfaceView d, int w, int h, ReadOnlySpan<byte> table, bool rgb565, int key = -1)
    {
        for (int y = 0; y < h; y++)
        {
            var source = Words(s.Row(y, w * 2));
            var destination = Words(d.Row(y, w * 2));
            for (int x = 0; x < w; x++)
            {
                int p = source[x];
                if (p != key) destination[x] = (ushort)BlendPixel(p, destination[x], table, rgb565);
            }
        }
    }

    // The 555 blender handles an unaligned first pixel alone and then pairs. A row of fewer than
    // three pixels (two when aligned) draws only that first pixel and keeps the row pitch short
    // by the pixels it did not draw.
    public static void BlendSurface555(SurfaceView s, SurfaceView d, int w, int h, ReadOnlySpan<byte> t)
    {
        int head = (d.Start & 3) == 0 ? 0 : 1;
        int draw = (w - head) / 2 == 0 ? head : w;
        Blend(s.WithPitch(s.Pitch - (w - draw) * 2), d.WithPitch(d.Pitch - (w - draw) * 2), draw, h, t, false);
    }

    public static void BlendSurface565(SurfaceView s, SurfaceView d, int w, int h, ReadOnlySpan<byte> t) => Blend(s, d, w, h, t, true);

    public static void BlendColorKey555(SurfaceView s, SurfaceView d, int w, int h, int k, ReadOnlySpan<byte> t) => Blend(s, d, w, h, t, false, k);

    public static void BlendColorKey565(SurfaceView s, SurfaceView d, int w, int h, int k, ReadOnlySpan<byte> t) => Blend(s, d, w, h, t, true, k);

    // `rows[y]` is the byte offset of source row y from the source's first pixel (an address in
    // the original) and `cols[x]` the source column of destination pixel x.
    private static void Stretch(SurfaceView source, ReadOnlySpan<int> rows, ReadOnlySpan<int> cols, SurfaceView destination, int w, int h, ReadOnlySpan<byte> table, bool rgb565, bool blend)
    {
        for (int y = 0; y < h; y++)
        {
            var row = Words(destination.Row(y, w * 2));
            for (int x = 0; x < w; x++)
            {
                int p = source.UInt16(rows[y] + cols[x] * 2);
                row[x] = (ushort)(!blend ? p : BlendPixel(p, row[x], table, rgb565));
            }
        }
    }

    public static void BlitStretched16(SurfaceView source, ReadOnlySpan<int> rows, ReadOnlySpan<int> cols, SurfaceView destination, int w, int h) => Stretch(source, rows, cols, destination, w, h, default, false, false);

    public static void BlendStretched555(SurfaceView source, ReadOnlySpan<int> rows, ReadOnlySpan<int> cols, SurfaceView destination, int w, int h, ReadOnlySpan<byte> table) => Stretch(source, rows, cols, destination, w, h, table, false, true);

    public static void BlendStretched565(SurfaceView source, ReadOnlySpan<int> rows, ReadOnlySpan<int> cols, SurfaceView destination, int w, int h, ReadOnlySpan<byte> table) => Stretch(source, rows, cols, destination, w, h, table, true, true);

    public static int ReadTimeStampCounter() => 0;

    public static bool CpuIdSupported() => false;

    public static bool MmxSupport() => false;
}
