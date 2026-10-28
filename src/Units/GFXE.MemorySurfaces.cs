using Aow.Rtl;

namespace Aow.Units.GFXE;

internal static class MemorySurfaces
{
    public static TSurfaceDesc Describe(int w, int h, int pitch, int format, object bits, TGFXEPalette palette = null)
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
            ClipRect = Rtl.Rtl.Rect(0, 0, w, h)
        };
        GFXE.PFIDtoMASK(format, ref sd.RMask, ref sd.GMask, ref sd.BMask, ref sd.AMask);
        return sd;
    }
    public static (int Pitch, object Bits) Allocate(int w, int h, int format, int alignment)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(w);
        ArgumentOutOfRangeException.ThrowIfNegative(h);
        int pitch = checked((checked(w * (((format & 255) + 7) / 8)) + alignment - 1) & -alignment);
        return (pitch, Ptr<byte>.To(new byte[checked(pitch * h)], 0).Box());
    }
}

public partial class TDIBSurface
{
    public int Setup(int AWidth, int AHeight, int APixelFormatID, TGFXEPalette APalette)
    {
        Clear(); FPixelFormatID = APixelFormatID;
        if (APalette != null || (APixelFormatID & 255) == 8)
        { FPalette = TGFXEPalette.Create(); if (APalette != null) FPalette.Assign(APalette); }
        return Resize(AWidth, AHeight);
    }
    public void Clear() { FWidth = FHeight = FPitch = 0; FBits = null; FMem = default; FPalette = null; }
    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && FBits != null) return GFXE.GFXE_OK; (FPitch, FBits) = MemorySurfaces.Allocate(AWidth, AHeight, FPixelFormatID, 4); FWidth = AWidth; FHeight = AHeight; return GFXE.GFXE_OK; }
    public override int Lock(ref TSurfaceDesc SD) { SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, FPixelFormatID, FBits, FPalette); return GFXE.GFXE_OK; }
    public override int Unlock() => GFXE.GFXE_OK;
    public override int Blt(TSurface Dest, Ptr<TRect> SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int Cls(int Color) => base.Cls(Color);
    public override int BltDC(int DC, Ptr<TRect> SrcRect, Ptr<TRect> DestRect) => throw new ENotSupported("Native device context");
    public override void Destroy() { FBits = null; FWidth = FHeight = FPitch = 0; base.Destroy(); }
    public override int UpdatePalette() => FPalette != null && FPixelFormatID == GFXE.pfPAL8 ? GFXE.GFXE_OK : GFXE.GFXE_NOTSUPPORTED;
    public int Handle => throw new ENotSupported("Native surface handle");
}

public partial class TDIB08Surface
{
    public void Create__init_TDIB08Surface(int AWidth, int AHeight, TGFXEPalette APalette) { SetPalette(APalette); Resize(AWidth, AHeight); }
    public void SetPalette(TGFXEPalette Palette) { FPalette ??= TGFXEPalette.Create(); if (Palette != null && !ReferenceEquals(Palette, FPalette)) FPalette.Assign(Palette); }
    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && FBits != null) return GFXE.GFXE_OK; (FPitch, FBits) = MemorySurfaces.Allocate(AWidth, AHeight, GFXE.pfPAL8, 4); FWidth = AWidth; FHeight = AHeight; return GFXE.GFXE_OK; }
    public override int Lock(ref TSurfaceDesc SD) { SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, GFXE.pfPAL8, FBits, FPalette); return GFXE.GFXE_OK; }
    public override int Unlock() => GFXE.GFXE_OK;
    public override int Blt(TSurface Dest, Ptr<TRect> SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int Cls(int Color) => base.Cls(Color);
    public override int BltDC(int DC, Ptr<TRect> SrcRect, Ptr<TRect> DestRect) => throw new ENotSupported("Native device context");
    public override void Destroy() { FBits = null; FWidth = FHeight = FPitch = 0; base.Destroy(); }
    public override int UpdatePalette() => GFXE.GFXE_OK;
}

public partial class TDIB16Surface { private bool memoryLocked; }

public partial class TDIB16Surface
{
    public void Create__init_TDIB16Surface(int AWidth, int AHeight, int APixelFormatID) { SetPixelFormatID(APixelFormatID); Resize(AWidth, AHeight); }
    internal void SetPixelFormatID(int Value) { if (Value != GFXE.pfRGB555 && Value != GFXE.pfRGB565 && Value != GFXE.pfARGB1555) throw new ENotSupported("Pixel format"); if (memoryLocked && Value != FPixelFormatID) throw new EInvalidOperation("Pixel format after Lock"); FPixelFormatID = Value; }
    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && FBits != null) return GFXE.GFXE_OK; (FPitch, FBits) = MemorySurfaces.Allocate(AWidth, AHeight, FPixelFormatID, 4); FWidth = AWidth; FHeight = AHeight; return GFXE.GFXE_OK; }
    public override int Lock(ref TSurfaceDesc SD) { memoryLocked = true; SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, FPixelFormatID, FBits, null); return GFXE.GFXE_OK; }
    public override int Unlock() => GFXE.GFXE_OK;
    public override int Blt(TSurface Dest, Ptr<TRect> SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int Cls(int Color) => base.Cls(Color);
    public override int BltDC(int DC, Ptr<TRect> SrcRect, Ptr<TRect> DestRect) => throw new ENotSupported("Native device context");
    public override void Destroy() { FBits = null; FWidth = FHeight = FPitch = 0; base.Destroy(); }
    public int Handle => throw new ENotSupported("Native surface handle");
}

public partial class TDIB24Surface
{
    public void Create__init_TDIB24Surface(int AWidth, int AHeight) { Resize(AWidth, AHeight); }
    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && FBits != null) return GFXE.GFXE_OK; (FPitch, FBits) = MemorySurfaces.Allocate(AWidth, AHeight, GFXE.pfRGB24, 4); FWidth = AWidth; FHeight = AHeight; return GFXE.GFXE_OK; }
    public override int Lock(ref TSurfaceDesc SD) { SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, GFXE.pfRGB24, FBits, null); return GFXE.GFXE_OK; }
    public override int Unlock() => GFXE.GFXE_OK;
    public override int Blt(TSurface Dest, Ptr<TRect> SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int Cls(int Color) => base.Cls(Color);
    public override int BltDC(int DC, Ptr<TRect> SrcRect, Ptr<TRect> DestRect) => throw new ENotSupported("Native device context");
    public override void Destroy() { FBits = null; FWidth = FHeight = FPitch = 0; base.Destroy(); }
    public int Handle => throw new ENotSupported("Native surface handle");
}

public partial class TDIB32Surface
{
    public void Create__init_TDIB32Surface(int AWidth, int AHeight) { Resize(AWidth, AHeight); }
    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && FBits != null) return GFXE.GFXE_OK; (FPitch, FBits) = MemorySurfaces.Allocate(AWidth, AHeight, GFXE.pfRGB32, 4); FWidth = AWidth; FHeight = AHeight; return GFXE.GFXE_OK; }
    public override int Lock(ref TSurfaceDesc SD) { SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, GFXE.pfRGB32, FBits, null); return GFXE.GFXE_OK; }
    public override int Unlock() => GFXE.GFXE_OK;
    public override int Blt(TSurface Dest, Ptr<TRect> SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int Cls(int Color) => base.Cls(Color);
    public override int BltDC(int DC, Ptr<TRect> SrcRect, Ptr<TRect> DestRect) => throw new ENotSupported("Native device context");
    public override void Destroy() { FBits = null; FWidth = FHeight = FPitch = 0; base.Destroy(); }
}

public partial class TDDrawSurface
{
    private int memoryPitch;
    private object memoryBits;
    public int CreateSurface(int AWidth, int AHeight, int APixelFormatID, TSet<TDDSCreationFlagsElement> AFlags) { if (FPixelFormatID != APixelFormatID) memoryBits = null; FPixelFormatID = APixelFormatID; FCreationFlags = AFlags; FStatus = TSet<TDDrawSurfaceStatusElement>.Of(GFXE.ssSystemMemory); return Resize(AWidth, AHeight); }
    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && memoryBits != null) return GFXE.DD_OK; (memoryPitch, memoryBits) = MemorySurfaces.Allocate(AWidth, AHeight, FPixelFormatID, 8); FWidth = AWidth; FHeight = AHeight; return GFXE.DD_OK; }
    public override int Lock(ref TSurfaceDesc SD) { SD = MemorySurfaces.Describe(FWidth, FHeight, memoryPitch, FPixelFormatID, memoryBits); return GFXE.DD_OK; }
    public override int Blt(TSurface Dest, Ptr<TRect> SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int BltColorKey(TSurface Dest, Ptr<TRect> SrcRect, int X, int Y, int ColorKey) => base.BltColorKey(Dest, SrcRect, X, Y, ColorKey);
    public override int Cls(int Color) => base.Cls(Color);
    public int AsyncCls(int Color) => Cls(Color);
    public bool Lost() => false;
    public int Restore() => GFXE.DD_OK;
    public override int Unlock() => GFXE.DD_OK;
    public override int LockEx(ref TSurfaceDesc SD, in TRect R) => base.LockEx(ref SD, R);
    public override int UnlockEx(in TSurfaceDesc SD) => Unlock();
    public override void Destroy() { memoryBits = null; FWidth = FHeight = 0; base.Destroy(); }
}

public partial class TDataSurface
{
    public override int Lock(ref TSurfaceDesc SD)
    { SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, FPixelFormatID, FData, FPalette); return GFXE.GFXE_OK; }
}
public partial class TSurface
{
    public virtual int BltDC(int DC, Ptr<TRect> SrcRect, Ptr<TRect> DestRect) => throw new ENotSupported("Native device context");
    public virtual int Cls(int Color)
    {
        TSurfaceDesc sd = default;
        int result = Lock(ref sd);
        if (result < 0) return result;
        try
        {
            int pixel = GFXE.Color2PixelInfo(Color, sd.PixelFormatID, sd.Palette);
            var data = Rtl.Rtl.BytePointer(sd.Data);
            for (int y = 0; y < sd.Height; y++)
            {
                var row = data + y * sd.Pitch;
                switch (sd.BitCount)
                {
                    case 8: row.AsSpan(sd.Width).Fill((byte)pixel); break;
                    case 16: Aow.Graphics.Blit.FillWord(row, sd.Width, pixel); break;
                    case 24: if (sd.Width > 0) Aow.Graphics.Blit.FillTrippleByte(row, sd.Width, pixel); break;
                    case 32: Aow.Graphics.Blit.FillInt(row, sd.Width, pixel); break;
                    default: return GFXE.GFXE_NOTSUPPORTED;
                }
            }
            return GFXE.GFXE_OK;
        }
        finally { Unlock(); }
    }

}
public static partial class GFXE
{
    public static int Color2PixelInfo(int Color, int PixelFormatID, TGFXEPalette Palette)
    {
        int r = Color & 255, g = (Color >> 8) & 255, b = (Color >> 16) & 255;
        return PixelFormatID switch
        {
            pfPAL8 => Palette is null ? throw new EInvalidOperation("Palette required") : Palette.GetNearestColor(Color),
            pfRGB555 or pfARGB1555 => ((r & 248) << 7) + ((g & 248) << 2) + (b >> 3),
            pfRGB565 => ((r & 248) << 8) + ((g & 252) << 3) + (b >> 3),
            pfRGB24 or pfRGB32 or pfXRGB32 => (r << 16) + (g << 8) + b,
            pfBGR24 => Color & 0xFFFFFF,
            _ => throw new ENotSupported("Pixel format")
        };
    }
    public static void DrawLine16(int X1, int Y1, int X2, int Y2, int Skip, ushort Color, in TSurfaceDesc SD)
        => Aow.Graphics.Blit.DrawLine16(X1, Y1, X2, Y2, Skip, Color, SD);
    public static int ColorFillSD(TRect R, int Color, in TSurfaceDesc SD)
    {
        if (SD.PixelFormatID != pfRGB555 && SD.PixelFormatID != pfARGB1555 && SD.PixelFormatID != pfRGB565) return GFXE_NOTSUPPORTED;
        int left = Math.Max(R.Left, SD.ClipRect.Left), right = Math.Min(R.Right, SD.ClipRect.Right);
        int top = Math.Max(R.Top, SD.ClipRect.Top), bottom = Math.Min(R.Bottom, SD.ClipRect.Bottom);
        if (left >= right || top >= bottom) return GFXE_OK;
        int pixel = Color2PixelInfo(Color, SD.PixelFormatID, null);
        var data = Rtl.Rtl.BytePointer(SD.Data);
        for (int y = top; y < bottom; y++) Aow.Graphics.Blit.FillWord(data + y * SD.Pitch + left * 2, right - left, pixel);
        return GFXE_OK;
    }
}

public static partial class GFXE
{
    // The recovered enum has no system-memory bit. Created memory surfaces have no video flag.
    public const TDDrawSurfaceStatusElement ssSystemMemory = TDDrawSurfaceStatusElement.ssCreated;
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
        Array.Copy(Source.Entries,Entries,256);
        PixelFormatID=Source.PixelFormatID;
    }
}
public partial class TBlendTable5Bit
{
    public override void Assign(TBlendTable Source)
    {
        if(Source is TBlendTable5Bit table) Array.Copy(table.Table,Table,32);
    }
}
