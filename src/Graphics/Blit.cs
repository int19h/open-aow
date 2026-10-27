using Aow.Rtl;
using System.Buffers.Binary;

namespace Aow.Graphics;

public static partial class Blit
{
    internal static ushort Word(Ptr<byte> p, int offset = 0) => BinaryPrimitives.ReadUInt16LittleEndian((p + offset).AsSpan(2));
    internal static uint Dword(Ptr<byte> p, int offset = 0) => BinaryPrimitives.ReadUInt32LittleEndian((p + offset).AsSpan(4));
    internal static void Word(Ptr<byte> p, int offset, int value) => BinaryPrimitives.WriteUInt16LittleEndian((p + offset).AsSpan(2), unchecked((ushort)value));
    internal static void Dword(Ptr<byte> p, int offset, uint value) => BinaryPrimitives.WriteUInt32LittleEndian((p + offset).AsSpan(4), value);
    public static void BltRect(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h) => RegisterBltRect(s, d, sp, dp, w, h);
    public static void RegisterBltRect(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h)
    {
        if (w == 0 || h == 0) return;
        int head = (-s.ByteIndex) & 3, tail = (s.ByteIndex + w) & 3;
        uint words = unchecked((uint)(w - tail - head)) >> 2;
        for (int y = 0; y < h; y++)
        {
            var src = s + y * sp; var dst = d + y * dp; int offset = 0;
            for (int i = 0; i < head; i++) { byte value = src[offset]; dst[offset++] = value; }
            for (uint i = 0; i < (words >> 1); i++)
            {
                uint first = Dword(src, offset), second = Dword(src, offset + 4);
                Dword(dst, offset, first); Dword(dst, offset + 4, second); offset += 8;
            }
            if ((words & 1) != 0) { uint value = Dword(src, offset); Dword(dst, offset, value); offset += 4; }
            for (int i = 0; i < tail; i++) { byte value = src[offset]; dst[offset++] = value; }
        }
    }
    private static void CopyRows(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h, bool floating)
    {
        if (w == 0 || (w >= 31 && h == 0)) return;
        int head = w >= 31 ? (-s.ByteIndex) & 7 : 0;
        int blocks = w >= 31 ? (w - head) >> 5 : 0;
        int words = (w - head - blocks * 32) >> 2, tail = (w - head) & 3;
        int y = 0;
        do
        {
            var src = s + y * sp; var dst = d + y * dp; int offset = 0;
            for (int i = 0; i < head; i++) { byte value = src[offset]; dst[offset++] = value; }
            for (int i = 0; i < blocks; i++)
            {
                ulong a = src.Reinterpret<ulong>(offset).Value, b = src.Reinterpret<ulong>(offset + 8).Value;
                ulong c = src.Reinterpret<ulong>(offset + 16).Value, e = src.Reinterpret<ulong>(offset + 24).Value;
                if (floating)
                {
                    dst.Reinterpret<ulong>(offset + 24).Value = e; dst.Reinterpret<ulong>(offset + 16).Value = c;
                    dst.Reinterpret<ulong>(offset + 8).Value = b; dst.Reinterpret<ulong>(offset).Value = a;
                }
                else
                {
                    dst.Reinterpret<ulong>(offset).Value = a; dst.Reinterpret<ulong>(offset + 8).Value = b;
                    dst.Reinterpret<ulong>(offset + 16).Value = c; dst.Reinterpret<ulong>(offset + 24).Value = e;
                }
                offset += 32;
            }
            for (int i = 0; i < words; i++) { uint value = Dword(src, offset); Dword(dst, offset, value); offset += 4; }
            for (int i = 0; i < tail; i++) { byte value = src[offset]; dst[offset++] = value; }
        } while (++y != h);
    }
    public static void FPBltRect(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h) => CopyRows(s, d, sp, dp, w, h, true);
    public static void MMXBltRect(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h) => CopyRows(s, d, sp, dp, w, h, false);
    public static void RepMovsdBltRect(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h)
    {
        if (w == 0 || h == 0) return;
        int tail = w < 4 ? w : (s.ByteIndex + w) & 3;
        int head = w < 4 ? 0 : (w - tail) & 3;
        int words = w < 4 ? 0 : (w - tail) >> 2;
        for (int y = 0; y < h; y++)
        {
            var src = s + y * sp; var dst = d + y * dp; int offset = 0;
            for (int i = 0; i < head; i++) { byte value = src[offset]; dst[offset++] = value; }
            for (int i = 0; i < words; i++) { uint value = Dword(src, offset); Dword(dst, offset, value); offset += 4; }
            for (int i = 0; i < tail; i++) { byte value = src[offset]; dst[offset++] = value; }
        }
    }
    public static void BltColorKey16(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h, int key)
    {
        for (int y = 0; y < h; y++) for (int x = 0; x < w; x++)
        {
            int pixel = Word(s, y * sp + x * 2);
            if (pixel != key) Word(d, y * dp + x * 2, pixel);
        }
    }
    internal static int Half(int s, int d, bool rgb565, bool high = false)
    {
        int mask = rgb565 ? 0xF7DF : 0x7BDF;
        if (high) mask &= ~1;
        return ((s & mask) + (d & mask)) >> 1;
    }
    private static void Transparent(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h, bool rgb565)
    {
        int paired = ((s.ByteIndex | d.ByteIndex) & 3) == 0 ? w & ~3 : 0;
        for (int y = 0; y < h; y++) for (int x = 0; x < w; x++)
            Word(d, y * dp + x * 2, Half(Word(s, y * sp + x * 2), Word(d, y * dp + x * 2), rgb565, x < paired && (x & 1) != 0));
    }
    public static void BltTransparent555(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h) => Transparent(s, d, sp, dp, w, h, false);
    public static void BltTransparent565(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h) => Transparent(s, d, sp, dp, w, h, true);
    public static void FillInt(Ptr<byte> d, int count, int value) => FillInt(d.AsSpan(), count, value);
    public static void FillWord(Ptr<byte> d, int count, int value) => FillWord(d.AsSpan(), count, value);
    public static void FillTrippleByte(Ptr<byte> d, int count, int value) => FillTrippleByte(d.AsSpan(), count, value);
    public static void FillInt(Span<byte> d, int count, int value)
    {
        for (uint i = 0; i < unchecked((uint)count); i++)
        {
            if (i >= (uint)(d.Length / 4)) throw new EAccessViolation("FillInt crossed the buffer");
            BinaryPrimitives.WriteInt32LittleEndian(d.Slice((int)i * 4, 4), value);
        }
    }
    public static void FillWord(Span<byte> d, int count, int value)
    {
        uint pair = unchecked(((uint)value << 16) + (uint)value), pairs = (uint)count >> 1;
        for (uint i = 0; i < pairs; i++)
        {
            if (i >= (uint)(d.Length / 4)) throw new EAccessViolation("FillWord crossed the buffer");
            BinaryPrimitives.WriteUInt32LittleEndian(d.Slice((int)i * 4, 4), pair);
        }
        if ((count & 1) != 0)
        {
            int offset = checked((int)pairs * 4);
            if (offset > d.Length - 2) throw new EAccessViolation("FillWord crossed the buffer");
            BinaryPrimitives.WriteUInt16LittleEndian(d.Slice(offset, 2), (ushort)pair);
        }
    }
    public static void FillTrippleByte(Span<byte> d, int count, int value)
    {
        int offset = 0;
        do
        {
            if (offset > d.Length - 2) throw new EAccessViolation("FillTrippleByte crossed the buffer");
            BinaryPrimitives.WriteUInt16LittleEndian(d.Slice(offset, 2), (ushort)value);
            if (offset + 2 >= d.Length) throw new EAccessViolation("FillTrippleByte crossed the buffer");
            d[offset + 2] = (byte)(value >> 16); offset += 3;
        } while (--count != 0);
    }
    private static void Remap(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h, int size, Func<int, int, int> map, bool unsafeShort = false)
    {
        if (unsafeShort && w > 0 && w < 4 && h > 0 && ((s.ByteIndex | d.ByteIndex) & 3) == 0)
            throw new EAccessViolation("Original aligned remapper overruns short rows");
        for (int y = 0; y < h; y++) for (int x = 0; x < w; x++)
        {
            int pixel = map(Word(s, y * sp + x * 2), x);
            int offset = y * dp + x * size;
            if (size == 1) d[offset] = (byte)pixel;
            else if (size == 2) Word(d, offset, pixel);
            else Dword(d, offset, (uint)pixel);
        }
    }
    public static void RemapSurface16to16(Ptr<byte> s, Ptr<byte> d, Ptr<byte> table, int sp, int dp, int w, int h) => Remap(s, d, sp, dp, w, h, 2, (p, _) => Word(table, p * 2), true);
    public static void RemapSurface16to08(Ptr<byte> s, Ptr<byte> d, Ptr<byte> table, int sp, int dp, int w, int h) => Remap(s, d, sp, dp, w, h, 1, (p, _) => table[p], true);
    public static void RemapSurface16to32(Ptr<byte> s, Ptr<byte> d, Ptr<byte> table, int sp, int dp, int w, int h) => Remap(s, d, sp, dp, w, h, 4, (p, _) => (int)Dword(table, p * 4), true);
    internal static int To565(int p) => ((p & 0x7FE0) << 1) + (p & 31);
    internal static int To555(int p) => ((p & 0xFFC0) >> 1) + (p & 31);
    public static void RemapSurface555to565(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h) => Remap(s, d, sp, dp, w, h, 2, (p, _) => To565(p));
    public static void RemapSurface565to555(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h) => Remap(s, d, sp, dp, w, h, 2, (p, _) => To555(p));
    public static void RemapSurface555to32(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h)
    {
        int paired = ((s.ByteIndex | d.ByteIndex) & 3) == 0 ? w & ~3 : 0;
        Remap(s, d, sp, dp, w, h, 4, (p, x) => (x < paired ? (p & 0x7C00) << 9 : 0) + ((p & 0x3E0) << 6) + ((p & 31) << 3), true);
    }
    public static void RemapSurface565to32(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h)
    {
        bool paired = ((s.ByteIndex | d.ByteIndex) & 3) == 0;
        Remap(s, d, sp, dp, w, h, 4, (p, x) => ((p & 0xF800) << 8) + ((p & (paired && x < (w & ~3) && (x & 3) == 3 ? 0x7C0 : 0x7E0)) << 5) + ((p & 31) << 3), true);
    }
    internal static int BlendPixel(int s, int d, Ptr<byte> table, bool rgb565)
    {
        int g = rgb565 ? 6 : 5, r = rgb565 ? 11 : 10;
        return table[((s & 31) << 5) + (d & 31)] + (table[(((s >> g) & 31) << 5) + ((d >> g) & 31)] << g) + (table[(((s >> r) & 31) << 5) + ((d >> r) & 31)] << r);
    }
    private static void Blend(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h, Ptr<byte> table, bool rgb565, int key = -1)
    {
        for (int y = 0; y < h; y++) for (int x = 0; x < w; x++)
        {
            int p = Word(s, y * sp + x * 2);
            if (p != key) Word(d, y * dp + x * 2, BlendPixel(p, Word(d, y * dp + x * 2), table, rgb565));
        }
    }
    public static void BlendSurface555(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h, Ptr<byte> t)
    {
        int head = (d.ByteIndex & 3) == 0 ? 0 : 1;
        int draw = (w - head) / 2 == 0 ? head : w;
        Blend(s, d, sp - (w - draw) * 2, dp - (w - draw) * 2, draw, h, t, false);
    }
    public static void BlendSurface565(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h, Ptr<byte> t) => Blend(s, d, sp, dp, w, h, t, true);
    public static void BlendColorKey555(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h, int k, Ptr<byte> t) => Blend(s, d, sp, dp, w, h, t, false, k);
    public static void BlendColorKey565(Ptr<byte> s, Ptr<byte> d, int sp, int dp, int w, int h, int k, Ptr<byte> t) => Blend(s, d, sp, dp, w, h, t, true, k);
    private static void Stretch(Ptr<byte> src, ReadOnlySpan<int> rows, ReadOnlySpan<int> cols, Ptr<byte> dst, int pitch, int w, int h, Ptr<byte> table, bool rgb565, bool blend)
    {
        for (int y = 0; y < h; y++) for (int x = 0; x < w; x++)
        {
            int p = Word(src, rows[y] + cols[x] * 2);
            Word(dst, y * pitch + x * 2, !blend ? p : BlendPixel(p, Word(dst, y * pitch + x * 2), table, rgb565));
        }
    }
    public static void BltStretched16(Ptr<byte> src, ReadOnlySpan<int> rows, ReadOnlySpan<int> cols, Ptr<byte> dst, int pitch, int w, int h) => Stretch(src, rows, cols, dst, pitch, w, h, default, false, false);
    public static void BlendStretched555(Ptr<byte> src, ReadOnlySpan<int> rows, ReadOnlySpan<int> cols, Ptr<byte> dst, int pitch, int w, int h, Ptr<byte> table) => Stretch(src, rows, cols, dst, pitch, w, h, table, false, true);
    public static void BlendStretched565(Ptr<byte> src, ReadOnlySpan<int> rows, ReadOnlySpan<int> cols, Ptr<byte> dst, int pitch, int w, int h, Ptr<byte> table) => Stretch(src, rows, cols, dst, pitch, w, h, table, true, true);
    public static int RDTSC() => 0;
    public static bool CPUIDSupported() => false;
    public static bool MMXsupport() => false;
}
