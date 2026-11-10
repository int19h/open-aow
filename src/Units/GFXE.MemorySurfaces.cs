using Aow.Graphics;
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
        Aow.Game.Draw.MemorySurfaces.PFIDtoMASK(format, ref sd.RMask, ref sd.GMask, ref sd.BMask, ref sd.AMask);
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
        var order = default(Engine.Buffers.FixedArray_byte_4);
        var masks = default(Engine.Buffers.FixedArray_int_4);
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

public partial class DIBSurface
{
    public int Setup(int AWidth, int AHeight, int APixelFormatID, GFXEPalette APalette)
    {
        Clear(); FPixelFormatID = APixelFormatID;
        if (APalette != null || (APixelFormatID & 255) == 8)
        { FPalette = new GFXEPalette(); if (APalette != null) FPalette.Assign(APalette); }
        return Resize(AWidth, AHeight);
    }

    public void Clear() { FWidth = FHeight = FPitch = 0; FBits = null; FMem = null; FPalette = null; }

    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && FBits != null) return PixelFormats.GFXE_OK; (FPitch, FBits) = MemorySurfaces.Allocate(AWidth, AHeight, FPixelFormatID, 4); FWidth = AWidth; FHeight = AHeight; return PixelFormats.GFXE_OK; }

    public override int Lock(ref SurfaceDesc SD) { SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, FPixelFormatID, FBits, FPalette); return PixelFormats.GFXE_OK; }

    public override int Unlock() => PixelFormats.GFXE_OK;
    public override int Blt(Surface Dest, Rect? SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int Cls(int Color) => base.Cls(Color);
    public override int BltDC(int DC, Rect? SrcRect, Rect? DestRect) => throw new NotSupportedException("Native device context");

    public override void Dispose() { FBits = null; FWidth = FHeight = FPitch = 0; base.Dispose(); }

    public override int UpdatePalette() => FPalette != null && FPixelFormatID == PixelFormats.pfPAL8 ? PixelFormats.GFXE_OK : PixelFormats.GFXE_NOTSUPPORTED;

    public int Handle => throw new NotSupportedException("Native surface handle");
}

public partial class DIB08Surface
{
    public DIB08Surface(int AWidth, int AHeight, GFXEPalette APalette)
    { SetPalette(APalette); Resize(AWidth, AHeight); }

    public void SetPalette(GFXEPalette Palette) { FPalette ??= new GFXEPalette(); if (Palette != null && !ReferenceEquals(Palette, FPalette)) FPalette.Assign(Palette); }

    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && FBits != null) return PixelFormats.GFXE_OK; (FPitch, FBits) = MemorySurfaces.Allocate(AWidth, AHeight, PixelFormats.pfPAL8, 4); FWidth = AWidth; FHeight = AHeight; return PixelFormats.GFXE_OK; }

    public override int Lock(ref SurfaceDesc SD) { SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, PixelFormats.pfPAL8, FBits, FPalette); return PixelFormats.GFXE_OK; }

    public override int Unlock() => PixelFormats.GFXE_OK;
    public override int Blt(Surface Dest, Rect? SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int Cls(int Color) => base.Cls(Color);
    public override int BltDC(int DC, Rect? SrcRect, Rect? DestRect) => throw new NotSupportedException("Native device context");

    public override void Dispose() { FBits = null; FWidth = FHeight = FPitch = 0; base.Dispose(); }

    public override int UpdatePalette() => PixelFormats.GFXE_OK;
}

public partial class DIB16Surface { private bool memoryLocked; }

public partial class DIB16Surface
{
    public DIB16Surface(int AWidth, int AHeight, int APixelFormatID)
    { SetPixelFormatID(APixelFormatID); Resize(AWidth, AHeight); }

    internal void SetPixelFormatID(int Value) { if (Value != PixelFormats.pfRGB555 && Value != PixelFormats.pfRGB565 && Value != PixelFormats.pfARGB1555) throw new NotSupportedException("Pixel format"); if (memoryLocked && Value != FPixelFormatID) throw new InvalidOperationException("Pixel format after Lock"); FPixelFormatID = Value; }

    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && FBits != null) return PixelFormats.GFXE_OK; (FPitch, FBits) = MemorySurfaces.Allocate(AWidth, AHeight, FPixelFormatID, 4); FWidth = AWidth; FHeight = AHeight; return PixelFormats.GFXE_OK; }

    public override int Lock(ref SurfaceDesc SD) { memoryLocked = true; SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, FPixelFormatID, FBits, null); return PixelFormats.GFXE_OK; }

    public override int Unlock() => PixelFormats.GFXE_OK;
    public override int Blt(Surface Dest, Rect? SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int Cls(int Color) => base.Cls(Color);
    public override int BltDC(int DC, Rect? SrcRect, Rect? DestRect) => throw new NotSupportedException("Native device context");

    public override void Dispose() { FBits = null; FWidth = FHeight = FPitch = 0; base.Dispose(); }

    public int Handle => throw new NotSupportedException("Native surface handle");
}

public partial class DIB24Surface
{
    public DIB24Surface(int AWidth, int AHeight)
    { Resize(AWidth, AHeight); }

    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && FBits != null) return PixelFormats.GFXE_OK; (FPitch, FBits) = MemorySurfaces.Allocate(AWidth, AHeight, PixelFormats.pfRGB24, 4); FWidth = AWidth; FHeight = AHeight; return PixelFormats.GFXE_OK; }

    public override int Lock(ref SurfaceDesc SD) { SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, PixelFormats.pfRGB24, FBits, null); return PixelFormats.GFXE_OK; }

    public override int Unlock() => PixelFormats.GFXE_OK;
    public override int Blt(Surface Dest, Rect? SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int Cls(int Color) => base.Cls(Color);
    public override int BltDC(int DC, Rect? SrcRect, Rect? DestRect) => throw new NotSupportedException("Native device context");

    public override void Dispose() { FBits = null; FWidth = FHeight = FPitch = 0; base.Dispose(); }

    public int Handle => throw new NotSupportedException("Native surface handle");
}

public partial class DIB32Surface
{
    public DIB32Surface(int AWidth, int AHeight)
    { Resize(AWidth, AHeight); }

    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && FBits != null) return PixelFormats.GFXE_OK; (FPitch, FBits) = MemorySurfaces.Allocate(AWidth, AHeight, PixelFormats.pfRGB32, 4); FWidth = AWidth; FHeight = AHeight; return PixelFormats.GFXE_OK; }

    public override int Lock(ref SurfaceDesc SD) { SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, PixelFormats.pfRGB32, FBits, null); return PixelFormats.GFXE_OK; }

    public override int Unlock() => PixelFormats.GFXE_OK;
    public override int Blt(Surface Dest, Rect? SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int Cls(int Color) => base.Cls(Color);
    public override int BltDC(int DC, Rect? SrcRect, Rect? DestRect) => throw new NotSupportedException("Native device context");

    public override void Dispose() { FBits = null; FWidth = FHeight = FPitch = 0; base.Dispose(); }
}

public partial class DDrawSurface
{
    private int memoryPitch;
    private byte[] memoryBits;

    public int CreateSurface(int AWidth, int AHeight, int APixelFormatID, Set<DDSCreationFlagsElement> AFlags) { if (FPixelFormatID != APixelFormatID) memoryBits = null; FPixelFormatID = APixelFormatID; FCreationFlags = AFlags; FStatus = Set<DDrawSurfaceStatusElement>.Of(ssSystemMemory); return Resize(AWidth, AHeight); }

    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && memoryBits != null) return PixelFormats.DD_OK; (memoryPitch, memoryBits) = MemorySurfaces.Allocate(AWidth, AHeight, FPixelFormatID, 8); FWidth = AWidth; FHeight = AHeight; return PixelFormats.DD_OK; }

    public override int Lock(ref SurfaceDesc SD) { SD = MemorySurfaces.Describe(FWidth, FHeight, memoryPitch, FPixelFormatID, memoryBits); return PixelFormats.DD_OK; }

    public override int Blt(Surface Dest, Rect? SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int BltColorKey(Surface Dest, Rect? SrcRect, int X, int Y, int ColorKey) => base.BltColorKey(Dest, SrcRect, X, Y, ColorKey);
    public override int Cls(int Color) => base.Cls(Color);
    public int AsyncCls(int Color) => Cls(Color);
    public bool Lost() => false;
    public int Restore() => PixelFormats.DD_OK;
    public override int Unlock() => PixelFormats.DD_OK;
    public override int LockEx(ref SurfaceDesc SD, in Rect R) => base.LockEx(ref SD, R);
    public override int UnlockEx(in SurfaceDesc SD) => Unlock();

    public override void Dispose() { memoryBits = null; FWidth = FHeight = 0; base.Dispose(); }
}

public partial class DataSurface
{
    public override int Lock(ref SurfaceDesc SD)
    { SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, FPixelFormatID, FMem, FPalette); return PixelFormats.GFXE_OK; }
}

public partial class Surface
{
    public virtual int BltDC(int DC, Rect? SrcRect, Rect? DestRect) => throw new NotSupportedException("Native device context");

    public virtual int Cls(int Color)
    {
        SurfaceDesc sd = default;
        int result = Lock(ref sd);
        if (result < 0) return result;
        try
        {
            int pixel = Aow.Game.Draw.PixelFormats.Color2PixelInfo(Color, sd.PixelFormatID, sd.Palette);
            for (int y = 0; y < sd.Height; y++)
            {
                var row = sd.Data.AsSpan(sd.DataIndex + y * sd.Pitch);
                switch (sd.BitCount)
                {
                    case 8: row[..sd.Width].Fill((byte)pixel); break;
                    case 16: Blit.FillWord(row, sd.Width, pixel); break;
                    case 24: if (sd.Width > 0) Blit.FillTrippleByte(row, sd.Width, pixel); break;
                    case 32: Blit.FillInt(row, sd.Width, pixel); break;
                    default: return PixelFormats.GFXE_NOTSUPPORTED;
                }
            }
            return PixelFormats.GFXE_OK;
        }
        finally { Unlock(); }
    }

}

public partial class GFXEPalette
{
    public int GetNearestColor(int Color)
    {
        int r = Color & 255, g = (Color >> 8) & 255, b = (Color >> 16) & 255, best = 0, distance = int.MaxValue;
        for (int i = 0; i < 256; i++)
        {
            int next = Math.Abs(Entries[i].R - r) + Math.Abs(Entries[i].G - g) + Math.Abs(Entries[i].B - b);
            if (next < distance) { distance = next; best = i; }
        }
        return best;
    }
}

public partial class GFXEPalette
{
    public void Assign(GFXEPalette Source)
    {
        Array.Copy(Source.Entries, Entries, 256);
        PixelFormatID = Source.PixelFormatID;
    }
}

public partial class BlendTable5Bit
{
    public override void Assign(BlendTable Source)
    {
        if (Source is BlendTable5Bit table) Array.Copy(table.FTable, FTable, 32 * 32);
    }
}
