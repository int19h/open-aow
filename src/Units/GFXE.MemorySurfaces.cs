using Aow.Graphics;
using Aow.Rtl;

namespace Aow.Game.Draw;

internal static class MemorySurfaces
{
    public static TSurfaceDesc Describe(int w, int h, int pitch, int format, byte[] bits, TGFXEPalette palette = null)
    {
        var sd = new TSurfaceDesc
        {
            Width = w,
            Height = h,
            BitCount = format & 255,
            Pitch = pitch,
            PixelFormatID = format,
            Data = bits,
            Palette = palette,
            ClipRect = RtlUnit.Rect(0, 0, w, h)
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
        var order = default(Aow.Game.Engine.Buffers.FixedArray_byte_4);
        var masks = default(Aow.Game.Engine.Buffers.FixedArray_int_4);
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

public partial class TDIBSurface
{
    public int Setup(int AWidth, int AHeight, int APixelFormatID, TGFXEPalette APalette)
    {
        Clear(); FPixelFormatID = APixelFormatID;
        if (APalette != null || (APixelFormatID & 255) == 8)
        { FPalette = new TGFXEPalette(); if (APalette != null) FPalette.Assign(APalette); }
        return Resize(AWidth, AHeight);
    }

    public void Clear() { FWidth = FHeight = FPitch = 0; FBits = null; FMem = null; FPalette = null; }

    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && FBits != null) return Aow.Game.Draw.PixelFormats.GFXE_OK; (FPitch, FBits) = MemorySurfaces.Allocate(AWidth, AHeight, FPixelFormatID, 4); FWidth = AWidth; FHeight = AHeight; return Aow.Game.Draw.PixelFormats.GFXE_OK; }

    public override int Lock(ref TSurfaceDesc SD) { SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, FPixelFormatID, FBits, FPalette); return Aow.Game.Draw.PixelFormats.GFXE_OK; }

    public override int Unlock() => Aow.Game.Draw.PixelFormats.GFXE_OK;
    public override int Blt(TSurface Dest, TRect? SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int Cls(int Color) => base.Cls(Color);
    public override int BltDC(int DC, TRect? SrcRect, TRect? DestRect) => throw new NotSupportedException("Native device context");

    public override void Dispose() { FBits = null; FWidth = FHeight = FPitch = 0; base.Dispose(); }

    public override int UpdatePalette() => FPalette != null && FPixelFormatID == Aow.Game.Draw.PixelFormats.pfPAL8 ? Aow.Game.Draw.PixelFormats.GFXE_OK : Aow.Game.Draw.PixelFormats.GFXE_NOTSUPPORTED;

    public int Handle => throw new NotSupportedException("Native surface handle");
}

public partial class TDIB08Surface
{
    public TDIB08Surface(int AWidth, int AHeight, TGFXEPalette APalette)
    { SetPalette(APalette); Resize(AWidth, AHeight); }

    public void SetPalette(TGFXEPalette Palette) { FPalette ??= new TGFXEPalette(); if (Palette != null && !ReferenceEquals(Palette, FPalette)) FPalette.Assign(Palette); }

    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && FBits != null) return Aow.Game.Draw.PixelFormats.GFXE_OK; (FPitch, FBits) = MemorySurfaces.Allocate(AWidth, AHeight, Aow.Game.Draw.PixelFormats.pfPAL8, 4); FWidth = AWidth; FHeight = AHeight; return Aow.Game.Draw.PixelFormats.GFXE_OK; }

    public override int Lock(ref TSurfaceDesc SD) { SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, Aow.Game.Draw.PixelFormats.pfPAL8, FBits, FPalette); return Aow.Game.Draw.PixelFormats.GFXE_OK; }

    public override int Unlock() => Aow.Game.Draw.PixelFormats.GFXE_OK;
    public override int Blt(TSurface Dest, TRect? SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int Cls(int Color) => base.Cls(Color);
    public override int BltDC(int DC, TRect? SrcRect, TRect? DestRect) => throw new NotSupportedException("Native device context");

    public override void Dispose() { FBits = null; FWidth = FHeight = FPitch = 0; base.Dispose(); }

    public override int UpdatePalette() => Aow.Game.Draw.PixelFormats.GFXE_OK;
}

public partial class TDIB16Surface { private bool memoryLocked; }

public partial class TDIB16Surface
{
    public TDIB16Surface(int AWidth, int AHeight, int APixelFormatID)
    { SetPixelFormatID(APixelFormatID); Resize(AWidth, AHeight); }

    internal void SetPixelFormatID(int Value) { if (Value != Aow.Game.Draw.PixelFormats.pfRGB555 && Value != Aow.Game.Draw.PixelFormats.pfRGB565 && Value != Aow.Game.Draw.PixelFormats.pfARGB1555) throw new NotSupportedException("Pixel format"); if (memoryLocked && Value != FPixelFormatID) throw new InvalidOperationException("Pixel format after Lock"); FPixelFormatID = Value; }

    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && FBits != null) return Aow.Game.Draw.PixelFormats.GFXE_OK; (FPitch, FBits) = MemorySurfaces.Allocate(AWidth, AHeight, FPixelFormatID, 4); FWidth = AWidth; FHeight = AHeight; return Aow.Game.Draw.PixelFormats.GFXE_OK; }

    public override int Lock(ref TSurfaceDesc SD) { memoryLocked = true; SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, FPixelFormatID, FBits, null); return Aow.Game.Draw.PixelFormats.GFXE_OK; }

    public override int Unlock() => Aow.Game.Draw.PixelFormats.GFXE_OK;
    public override int Blt(TSurface Dest, TRect? SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int Cls(int Color) => base.Cls(Color);
    public override int BltDC(int DC, TRect? SrcRect, TRect? DestRect) => throw new NotSupportedException("Native device context");

    public override void Dispose() { FBits = null; FWidth = FHeight = FPitch = 0; base.Dispose(); }

    public int Handle => throw new NotSupportedException("Native surface handle");
}

public partial class TDIB24Surface
{
    public TDIB24Surface(int AWidth, int AHeight)
    { Resize(AWidth, AHeight); }

    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && FBits != null) return Aow.Game.Draw.PixelFormats.GFXE_OK; (FPitch, FBits) = MemorySurfaces.Allocate(AWidth, AHeight, Aow.Game.Draw.PixelFormats.pfRGB24, 4); FWidth = AWidth; FHeight = AHeight; return Aow.Game.Draw.PixelFormats.GFXE_OK; }

    public override int Lock(ref TSurfaceDesc SD) { SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, Aow.Game.Draw.PixelFormats.pfRGB24, FBits, null); return Aow.Game.Draw.PixelFormats.GFXE_OK; }

    public override int Unlock() => Aow.Game.Draw.PixelFormats.GFXE_OK;
    public override int Blt(TSurface Dest, TRect? SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int Cls(int Color) => base.Cls(Color);
    public override int BltDC(int DC, TRect? SrcRect, TRect? DestRect) => throw new NotSupportedException("Native device context");

    public override void Dispose() { FBits = null; FWidth = FHeight = FPitch = 0; base.Dispose(); }

    public int Handle => throw new NotSupportedException("Native surface handle");
}

public partial class TDIB32Surface
{
    public TDIB32Surface(int AWidth, int AHeight)
    { Resize(AWidth, AHeight); }

    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && FBits != null) return Aow.Game.Draw.PixelFormats.GFXE_OK; (FPitch, FBits) = MemorySurfaces.Allocate(AWidth, AHeight, Aow.Game.Draw.PixelFormats.pfRGB32, 4); FWidth = AWidth; FHeight = AHeight; return Aow.Game.Draw.PixelFormats.GFXE_OK; }

    public override int Lock(ref TSurfaceDesc SD) { SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, Aow.Game.Draw.PixelFormats.pfRGB32, FBits, null); return Aow.Game.Draw.PixelFormats.GFXE_OK; }

    public override int Unlock() => Aow.Game.Draw.PixelFormats.GFXE_OK;
    public override int Blt(TSurface Dest, TRect? SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int Cls(int Color) => base.Cls(Color);
    public override int BltDC(int DC, TRect? SrcRect, TRect? DestRect) => throw new NotSupportedException("Native device context");

    public override void Dispose() { FBits = null; FWidth = FHeight = FPitch = 0; base.Dispose(); }
}

public partial class TDDrawSurface
{
    private int memoryPitch;
    private byte[] memoryBits;

    public int CreateSurface(int AWidth, int AHeight, int APixelFormatID, TSet<TDDSCreationFlagsElement> AFlags) { if (FPixelFormatID != APixelFormatID) memoryBits = null; FPixelFormatID = APixelFormatID; FCreationFlags = AFlags; FStatus = TSet<TDDrawSurfaceStatusElement>.Of(Aow.Game.Draw.TDDrawSurface.ssSystemMemory); return Resize(AWidth, AHeight); }

    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && memoryBits != null) return Aow.Game.Draw.PixelFormats.DD_OK; (memoryPitch, memoryBits) = MemorySurfaces.Allocate(AWidth, AHeight, FPixelFormatID, 8); FWidth = AWidth; FHeight = AHeight; return Aow.Game.Draw.PixelFormats.DD_OK; }

    public override int Lock(ref TSurfaceDesc SD) { SD = MemorySurfaces.Describe(FWidth, FHeight, memoryPitch, FPixelFormatID, memoryBits); return Aow.Game.Draw.PixelFormats.DD_OK; }

    public override int Blt(TSurface Dest, TRect? SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int BltColorKey(TSurface Dest, TRect? SrcRect, int X, int Y, int ColorKey) => base.BltColorKey(Dest, SrcRect, X, Y, ColorKey);
    public override int Cls(int Color) => base.Cls(Color);
    public int AsyncCls(int Color) => Cls(Color);
    public bool Lost() => false;
    public int Restore() => Aow.Game.Draw.PixelFormats.DD_OK;
    public override int Unlock() => Aow.Game.Draw.PixelFormats.DD_OK;
    public override int LockEx(ref TSurfaceDesc SD, in TRect R) => base.LockEx(ref SD, R);
    public override int UnlockEx(in TSurfaceDesc SD) => Unlock();

    public override void Dispose() { memoryBits = null; FWidth = FHeight = 0; base.Dispose(); }
}

public partial class TDataSurface
{
    public override int Lock(ref TSurfaceDesc SD)
    { SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, FPixelFormatID, FMem, FPalette); return Aow.Game.Draw.PixelFormats.GFXE_OK; }
}

public partial class TSurface
{
    public virtual int BltDC(int DC, TRect? SrcRect, TRect? DestRect) => throw new NotSupportedException("Native device context");

    public virtual int Cls(int Color)
    {
        TSurfaceDesc sd = default;
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
                    default: return Aow.Game.Draw.PixelFormats.GFXE_NOTSUPPORTED;
                }
            }
            return Aow.Game.Draw.PixelFormats.GFXE_OK;
        }
        finally { Unlock(); }
    }

}

public partial class TGFXEPalette
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

public partial class TGFXEPalette
{
    public void Assign(TGFXEPalette Source)
    {
        Array.Copy(Source.Entries, Entries, 256);
        PixelFormatID = Source.PixelFormatID;
    }
}

public partial class TBlendTable5Bit
{
    public override void Assign(TBlendTable Source)
    {
        if (Source is TBlendTable5Bit table) Array.Copy(table.FTable, FTable, 32 * 32);
    }
}
