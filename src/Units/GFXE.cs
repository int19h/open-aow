// Translated from GFXE.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;
using Exception = Aow.Rtl.Exception;

namespace Aow.Units.GFXE;

public enum TDDrawSurfaceStatusElement : byte
{
    ssVideoMemory = 0,
    ssLocked = 1,
    ssPrimary = 2,
    ssCreated = 3,
    ssComplex = 4
}

public enum TDDSCreationFlagsElement : byte
{
    cfVideoMemory = 0,
    cfSystemMemory = 1
}

[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct TPaletteEntry
{
    [FieldOffset(0)]
    public byte R;
    [FieldOffset(1)]
    public byte G;
    [FieldOffset(2)]
    public byte B;
    [FieldOffset(3)]
    public byte Reserved;
    [FieldOffset(0)]
    public ushort W;
    [FieldOffset(0)]
    public int Value;
}

public partial class TGFXEPalette : TObject
{
    public int PixelFormatID;
    public TPaletteEntry[] Entries = new TPaletteEntry[256];

    // GFXE.pas:11050
    public void LoadFromStream(TStream Stream)
    {
        Stream.Read(Bytes.Of(ref PixelFormatID, 4));
        Stream.Read(MemoryMarshal.AsBytes((Span<TPaletteEntry>)Entries).Slice(0, 1024));
    }

    // GFXE.pas:11056
    public void SaveToStream(TStream Stream)
    {
        Stream.Write(Bytes.Of(ref PixelFormatID, 4));
        Stream.Write(MemoryMarshal.AsBytes((ReadOnlySpan<TPaletteEntry>)Entries).Slice(0, 1024));
    }

    // GFXE.pas:11068
    // The original returns a pointer to the entry, a reference here.
    public ref TPaletteEntry GetEntry(int Index) => ref Entries[Index];

    // GFXE.pas:11073
    public int Remap(int NewPixelFormatID)
    {
        int Result = GFXEUnit.GFXE_OK;
        if (NewPixelFormatID != PixelFormatID)
        {
            switch (PixelFormatID)
            {
                case GFXEUnit.pfBGR24:
                    switch (NewPixelFormatID)
                    {
                        case GFXEUnit.pfRGB565:
                            {
                                for (int I = 0; I <= 255; I++)
                                {
                                    ref var paletteEntry = ref Entries[I];
                                    paletteEntry.W = (ushort)(((paletteEntry.R & 248) << 8) + ((paletteEntry.G & 252) << 3) + (int)(((uint)paletteEntry.B) >> 3));
                                }
                            }

                            break;
                        case GFXEUnit.pfRGB555:
                            {
                                for (int I = 0; I <= 255; I++)
                                {
                                    ref var paletteEntry = ref Entries[I];
                                    paletteEntry.W = (ushort)(((paletteEntry.R & 248) << 7) + ((paletteEntry.G & 248) << 2) + (int)(((uint)paletteEntry.B) >> 3));
                                }
                            }

                            break;
                        case GFXEUnit.pfXRGB32:
                            Result = GFXEUnit.GFXE_OK;
                            break;
                        default:
                            Result = GFXEUnit.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case GFXEUnit.pfRGB24:
                    switch (NewPixelFormatID)
                    {
                        case GFXEUnit.pfRGB565:
                            {
                                for (int I = 0; I <= 255; I++)
                                {
                                    ref var paletteEntry = ref Entries[I];
                                    paletteEntry.W = (ushort)(((paletteEntry.B & 248) << 8) + ((paletteEntry.G & 252) << 3) + (int)(((uint)paletteEntry.R) >> 3));
                                }
                            }

                            break;
                        case GFXEUnit.pfRGB555:
                            {
                                for (int I = 0; I <= 255; I++)
                                {
                                    ref var paletteEntry = ref Entries[I];
                                    paletteEntry.W = (ushort)(((paletteEntry.B & 248) << 7) + ((paletteEntry.G & 248) << 2) + (int)(((uint)paletteEntry.R) >> 3));
                                }
                            }

                            break;
                        case GFXEUnit.pfXRGB32:
                            Result = GFXEUnit.GFXE_OK;
                            break;
                        default:
                            Result = GFXEUnit.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                default:
                    Result = GFXEUnit.GFXE_NOTSUPPORTED;
                    break;
            }

            if (Succeeded(Result))
            {
                PixelFormatID = NewPixelFormatID;
            }
        }
        else
        {
            Result = GFXEUnit.GFXE_OK;
        }

        return Result;
    }

    // GFXE.pas:11140
    public bool Compare(TGFXEPalette Palette)
    {
        bool Result;
        int I;
        if (Palette.PixelFormatID == PixelFormatID)
        {
            switch (PixelFormatID & 31)
            {
                case 8:
                    Result = true;
                    I = 255;
                    while (I >= 0 && Result)
                    {
                        Result = Palette.Entries[I].W == Entries[I].W;
                        I -= 1;
                    }

                    break;
                case 16:
                    Result = true;
                    I = 255;
                    while (I >= 0 && Result)
                    {
                        Result = Palette.Entries[I].W == Entries[I].W;
                        I -= 1;
                    }

                    break;
                case 24:
                    Result = true;
                    I = 255;
                    while (I >= 0 && Result)
                    {
                        Result = Palette.Entries[I].W == Entries[I].W && Palette.Entries[I].B == Entries[I].B;
                        I -= 1;
                    }

                    break;
                case 32:
                    Result = CompareMem(MemoryMarshal.AsBytes((ReadOnlySpan<TPaletteEntry>)Entries).Slice(0, 1024), MemoryMarshal.AsBytes((ReadOnlySpan<TPaletteEntry>)Palette.Entries).Slice(0, 1024), 1024);
                    break;
                default:
                    Result = false;
                    break;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }
}

// the abstract surface. GetWidth and GetHeight are abstract
public partial class TSurface : TObject
{
    // GFXE.pas:11964
    public virtual int GetColor(int X, int Y)
    {
        int Result = default;
        TSurfaceDesc SD = default;
        int V;
        ushort W;
        byte Index;
        if (Succeeded(Lock(ref SD)))
        {
            try
            {
                Assert(X >= 0 && X < SD.Width && Y >= 0 && Y < SD.Height, "Coordinates out of bounds!", "GFXE.pas", 11974);
                // { 8 bit pixels need the palette; 1555 and the TColor ordered 24 bit layout are not handled }
                switch (SD.PixelFormatID)
                {
                    case GFXEUnit.pfRGB555:
                        W = SD.View().Word(X + X + Y * SD.Pitch);
                        Result = (int)(((uint)(W & 31744)) >> 7) + ((W & 992) << 6) + ((W & 31) << 19);
                        break;
                    case GFXEUnit.pfRGB565:
                        W = SD.View().Word(X + X + Y * SD.Pitch);
                        Result = (int)(((uint)(W & 63488)) >> 8) + ((W & 2016) << 5) + ((W & 31) << 19);
                        break;
                    case GFXEUnit.pfPAL8:
                        Assert(SD.Palette != null, "Palette not found, cannot retrieve color (GetColor)", "GFXE.pas", 11986);
                        Index = SD.View().Byte(X + Y * SD.Pitch);
                        ref readonly var E = ref SD.Palette.Entries[Index];
                        Result = E.R + (E.G << 8) + (E.B << 16);
                        break;
                    case GFXEUnit.pfRGB24:
                        var P = SD.View(X + X + X + Y * SD.Pitch);
                        Result = P[2] + (P[1] << 8) + (P[0] << 16);
                        break;
                    case GFXEUnit.pfRGB32:
                        V = (int)SD.View().Dword((X << 2) + Y * SD.Pitch);
                        Result = (int)(((uint)(V & 16711680)) >> 16) + (V & 65280) + ((V & 255) << 16);
                        break;
                    default:
                        Assert(false, "Pixel format not supported (GetColor)", "GFXE.pas", 11999);
                        Result = 0;
                        break;
                }
            }
            finally
            {
                Unlock();
            }
        }
        else
        {
            Assert(false, "Cannot Lock Surface", "GFXE.pas", 12004);
            Result = -1;
        }

        return Result;
    }

    // GFXE.pas:11924
    // { The original unit was longer: the line numbers compiled into the Assert calls (11932 for the next Assert below) prove that 737 more lines preceded this point. Their content is unknown; the blank lines below keep the Assert line numbers identical. }
    public virtual int GetPixelInfo(int X, int Y)
    {
        int Result = default;
        TSurfaceDesc SD = default;
        int P;
        if (Succeeded(Lock(ref SD)))
        {
            try
            {
                Assert(X >= 0 && X < SD.Width && Y >= 0 && Y < SD.Height, "Coordinates out of bounds!", "GFXE.pas", 11932);
                switch (SD.BitCount)
                {
                    case 8:
                        Result = SD.View().Byte(X + Y * SD.Pitch);
                        break;
                    case 16:
                        Result = SD.View().Word(X + X + Y * SD.Pitch);
                        break;
                    case 24:
                        if (X + 1 < SD.Width)
                        {
                            Result = (int)SD.View().Dword(X + X + X + Y * SD.Pitch) & 16777215;
                        }
                        else
                        {
                            P = X + X + X + Y * SD.Pitch;
                            Result = SD.View().Word(P) + (SD.View().Byte(P + 2) << 16);
                        }

                        break;
                    case 32:
                        Result = (int)SD.View().Dword((X << 2) + Y * SD.Pitch);
                        break;
                    default:
                        Assert(false, "Pixel format not supported (GetColor)", "GFXE.pas", 11946);
                        Result = 0;
                        break;
                }
            }
            finally
            {
                Unlock();
            }
        }
        else
        {
            Assert(false, "Cannot Lock Surface", "GFXE.pas", 11951);
            Result = -1;
        }

        return Result;
    }

    // GFXE.pas:398
    public virtual int GetWidth()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // GFXE.pas:399
    public virtual int GetHeight()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // GFXE.pas:11954
    public virtual int GetPixelFormatID()
    {
        int Result;
        TSurfaceDesc SD = default;
        if (Succeeded(Lock(ref SD)))
        {
            Result = SD.PixelFormatID;
            Unlock();
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // GFXE.pas:12007
    public virtual int Blt(TSurface Dest, TRect? SrcRect, int X, int Y)
    {
        int Result;
        TSurfaceDesc DSD = default;
        TSurfaceDesc SSD = default;
        TRect R = default;
        SurfaceView S;
        SurfaceView D;
        if (Dest.PixelFormatID == PixelFormatID)
        {
            if (Succeeded(Dest.Lock(ref DSD)))
            {
                if (Succeeded(Lock(ref SSD)))
                {
                    if (SrcRect is null)
                    {
                        R.Left = 0;
                        R.Right = SSD.Width;
                        R.Top = 0;
                        R.Bottom = SSD.Height;
                        SrcRect = R;
                    }

                    S = SSD.View(SrcRect.Value.Left * SSD.BitCount / 8 + SrcRect.Value.Top * SSD.Pitch);
                    D = DSD.View(X * DSD.BitCount / 8 + Y * DSD.Pitch);
                    GFXEUnit.BltRect(S, D, (SrcRect.Value.Right - SrcRect.Value.Left) * DSD.BitCount / 8, SrcRect.Value.Bottom - SrcRect.Value.Top);
                    Unlock();
                    Result = GFXEUnit.GFXE_OK;
                }
                else
                {
                    Result = GFXEUnit.GFXE_CANNOTLOCK;
                }

                Dest.Unlock();
            }
            else
            {
                Result = GFXEUnit.GFXE_CANNOTLOCK;
            }
        }
        else
        {
            Result = GFXEUnit.GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:12045
    public virtual int BltClipped(TSurface Dest, TRect? SrcRect, int X, int Y)
    {
        int Result;
        int W;
        int H;
        TRect R = default;
        if (SrcRect is null)
        {
            R.Left = 0;
            var width = GetWidth();
            R.Right = width;
            R.Top = 0;
            var height = GetHeight();
            R.Bottom = height;
        }
        else
        {
            R = SrcRect.Value;
            var width = GetWidth();
            if (R.Right > width)
            {
                var width2 = GetWidth();
                R.Right = width2;
            }

            if (R.Left < 0)
            {
                X = X - R.Left;
                R.Left = 0;
            }
        }

        if (X < 0)
        {
            R.Left = R.Left - X;
            X = 0;
        }

        W = R.Right - R.Left;
        if (Dest.GetWidth() < X + W)
        {
            W = W - (X + W - Dest.GetWidth());
        }

        var height2 = GetHeight();
        if (R.Bottom > height2)
        {
            var height = GetHeight();
            R.Bottom = height;
        }

        if (R.Top < 0)
        {
            Y = Y - R.Top;
            R.Top = 0;
        }

        if (Y < 0)
        {
            R.Top = R.Top - Y;
            Y = 0;
        }

        H = R.Bottom - R.Top;
        if (Dest.GetHeight() < H + Y)
        {
            H = H - (H + Y - Dest.GetHeight());
        }

        if (W > 0 && H > 0)
        {
            R.Right = W + R.Left;
            R.Bottom = R.Top + H;
            Result = Blt(Dest, R, X, Y);
        }
        else
        {
            Result = GFXEUnit.GFXE_OK;
        }

        return Result;
    }

    // GFXE.pas:12130
    public virtual int BltColorKey(TSurface Dest, TRect? SrcRect, int X, int Y, int ColorKey)
    {
        int Result;
        TSurfaceDesc DSD = default;
        TSurfaceDesc SSD = default;
        TRect R = default;
        SurfaceView S;
        SurfaceView D;
        if (Dest.PixelFormatID == PixelFormatID && (PixelFormatID & 255) == 16)
        {
            if (Succeeded(Dest.Lock(ref DSD)))
            {
                if (Succeeded(Lock(ref SSD)))
                {
                    if (SrcRect is null)
                    {
                        R.Left = 0;
                        R.Right = SSD.Width;
                        R.Top = 0;
                        R.Bottom = SSD.Height;
                        SrcRect = R;
                    }

                    S = SSD.View(SrcRect.Value.Left * SSD.BitCount / 8 + SrcRect.Value.Top * SSD.Pitch);
                    D = DSD.View(X * DSD.BitCount / 8 + Y * DSD.Pitch);
                    GFXEUnit.BltColorKey16(S, D, SrcRect.Value.Right - SrcRect.Value.Left, SrcRect.Value.Bottom - SrcRect.Value.Top, ColorKey);
                    Result = GFXEUnit.GFXE_OK;
                    Unlock();
                }
                else
                {
                    Result = GFXEUnit.GFXE_CANNOTLOCK;
                }

                Dest.Unlock();
            }
            else
            {
                Result = GFXEUnit.GFXE_CANNOTLOCK;
            }
        }
        else
        {
            Result = GFXEUnit.GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:12168
    public virtual int BltColorKeyClipped(TSurface Dest, TRect? SrcRect, int X, int Y, int ColorKey)
    {
        int Result;
        int W;
        int H;
        TRect R = default;
        if (SrcRect is null)
        {
            R.Left = 0;
            var width = GetWidth();
            R.Right = width;
            R.Top = 0;
            var height = GetHeight();
            R.Bottom = height;
        }
        else
        {
            R = SrcRect.Value;
            var width = GetWidth();
            if (R.Right > width)
            {
                var width2 = GetWidth();
                R.Right = width2;
            }

            if (R.Left < 0)
            {
                X = X - R.Left;
                R.Left = 0;
            }
        }

        if (X < 0)
        {
            R.Left = R.Left - X;
            X = 0;
        }

        W = R.Right - R.Left;
        if (Dest.GetWidth() < X + W)
        {
            W = W - (X + W - Dest.GetWidth());
        }

        var height2 = GetHeight();
        if (R.Bottom > height2)
        {
            var height = GetHeight();
            R.Bottom = height;
        }

        if (R.Top < 0)
        {
            Y = Y - R.Top;
            R.Top = 0;
        }

        if (Y < 0)
        {
            R.Top = R.Top - Y;
            Y = 0;
        }

        H = R.Bottom - R.Top;
        if (Dest.GetHeight() < H + Y)
        {
            H = H - (H + Y - Dest.GetHeight());
        }

        if (W > 0 && H > 0)
        {
            R.Right = W + R.Left;
            R.Bottom = R.Top + H;
            Result = BltColorKey(Dest, R, X, Y, ColorKey);
        }
        else
        {
            Result = GFXEUnit.GFXE_OK;
        }

        return Result;
    }

    // GFXE.pas:12102
    public virtual int BltStretch(TSurface Dest, TRect? SrcRect, TRect? DestRect)
    {
        int Result;
        TRect R = default;
        TRect DR = default;
        if (SrcRect is null)
        {
            R.Left = 0;
            var width = GetWidth();
            R.Right = width;
            R.Top = 0;
            var height = GetHeight();
            R.Bottom = height;
            SrcRect = R;
        }

        if (DestRect is null)
        {
            DR.Left = 0;
            var width = Dest.GetWidth();
            DR.Right = width;
            DR.Top = 0;
            var height = Dest.GetHeight();
            DR.Bottom = height;
            DestRect = DR;
        }

        if (SrcRect.Value.Right - SrcRect.Value.Left == DestRect.Value.Right - DestRect.Value.Left && SrcRect.Value.Bottom - SrcRect.Value.Top == DestRect.Value.Bottom - DestRect.Value.Top)
        {
            Result = Blt(Dest, SrcRect, DestRect.Value.Left, DestRect.Value.Top);
        }
        else
        {
            Result = GFXEUnit.GFXE_NOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:12225
    public virtual int Lock(ref TSurfaceDesc SD) => GFXEUnit.GFXE_NOTSUPPORTED;

    // GFXE.pas:12248
    public virtual int Unlock() => GFXEUnit.GFXE_NOTSUPPORTED;

    // GFXE.pas:12230
    public virtual int LockEx(ref TSurfaceDesc SD, in TRect R)
    {
        int Result;
        bool condition = R.Left >= 0 && R.Top >= 0;
        if (condition)
        {
            var height = GetHeight();
            condition = R.Bottom <= height;
        }

        bool test = condition;
        if (test)
        {
            var width = GetWidth();
            test = R.Right <= width;
        }

        if (test)
        {
            Result = Lock(ref SD);
            if (Succeeded(Result))
            {
                SD.DataIndex = SD.DataIndex + (int)(((uint)(R.Left * SD.BitCount)) >> 3) + R.Top * SD.Pitch;
                SD.Width = R.Right - R.Left;
                SD.Height = R.Bottom - R.Top;
                SD.ClipRect.Right = SD.Width;
                SD.ClipRect.Bottom = SD.Height;
            }
        }
        else
        {
            Result = GFXEUnit.GFXE_ERROR;
        }

        return Result;
    }

    // GFXE.pas:12253
    public virtual int UnlockEx(in TSurfaceDesc SD) => Unlock();

    // GFXE.pas:12220
    public virtual int UpdatePalette() => GFXEUnit.GFXE_NOTSUPPORTED;

    // GFXE.pas:12258
    public virtual int Resize(int AWidth, int AHeight) => GFXEUnit.GFXE_NOTSUPPORTED;

    public int Width { get => GetWidth(); }
    public int Height { get => GetHeight(); }
    public int PixelFormatID { get => GetPixelFormatID(); }
}

// a surface in memory. FWidth, FHeight, FMem, FData (FMem aligned to 8), FBitCount (Byte),
// FPitch, FPixelFormatID, FX, FY (origin of the captured rectangle), FPalette. The port's FMem
// is the pixel array and its first byte is the first pixel, so FData has no field of its own
// (graphics.md 3.4).
public partial class TDataSurface : TSurface
{

    internal int FWidth;
    internal int FHeight;
    internal byte[] FMem;
    internal byte FBitCount;
    internal int FPitch;
    internal int FPixelFormatID;
    internal int FX;
    internal int FY;
    internal TGFXEPalette FPalette;

    // GFXE.pas:12387
    public TDataSurface(int AWidth, int AHeight, int APixelFormatID, TGFXEPalette APalette)
    {
        FBitCount = (byte)(APixelFormatID & 255);
        FPixelFormatID = APixelFormatID;
        SetPalette(APalette);
        Resize(AWidth, AHeight);
    }

    // GFXE.pas:12396
    public TDataSurface(TSurface Surface, TRect? R)
    {
        CaptureSurface(Surface, R);
    }

    // GFXE.pas:12402
    public override void Destroy()
    {
        FPalette.Free();
        FPalette = null;
        FMem = null;
        base.Destroy();
    }

    // GFXE.pas:12410
    public override int GetPixelFormatID() => FPixelFormatID;

    // GFXE.pas:12415
    public override int GetWidth() => FWidth;

    // GFXE.pas:12420
    public override int GetHeight() => FHeight;

    // GFXE.pas:12441
    public override int Unlock() => GFXEUnit.GFXE_OK;

    // GFXE.pas:12446
    public int CaptureSurface(TSurface Surface, TRect? R)
    {
        TSurfaceDesc SD = default;
        TRect RR = default;
        int W;
        int H;
        int Result = Surface.Lock(ref SD);
        if (Succeeded(Result))
        {
            try
            {
                FBitCount = (byte)SD.BitCount;
                FPixelFormatID = SD.PixelFormatID;
                if (R is null)
                {
                    RR.Left = 0;
                    RR.Right = SD.Width;
                    RR.Top = 0;
                    RR.Bottom = SD.Height;
                    R = RR;
                }

                W = R.Value.Right - R.Value.Left;
                H = R.Value.Bottom - R.Value.Top;
                if (W > 0 && H > 0)
                {
                    Resize(W, H);
                    SetPalette(SD.Palette);
                    FX = R.Value.Left;
                    FY = R.Value.Top;
                    GFXEUnit.BltRect(SD.View(FY * SD.Pitch + (int)(((uint)(FX * FBitCount)) >> 3)), new SurfaceView(FMem, 0, FPitch), (int)(((uint)(FWidth * FBitCount)) >> 3), FHeight);
                }
            }
            catch
            {
                FX = 30;
            }

            Surface.Unlock();
            Result = GFXEUnit.GFXE_OK;
        }

        return Result;
    }

    // GFXE.pas:12485
    public void SetPalette(TGFXEPalette Palette)
    {
        if (Palette != null)
        {
            if (FPalette == null)
            {
                FPalette = new TGFXEPalette();
            }

            FPalette.Assign(Palette);
        }
        else
        {
            FPalette.Free();
            FPalette = null;
        }
    }

    // GFXE.pas:12499
    public override int Resize(int AWidth, int AHeight)
    {
        int Result;
        int Size;
        if (AWidth != FWidth || AHeight != FHeight)
        {
            FWidth = AWidth;
            FHeight = AHeight;
            FPitch = ((int)(((uint)(FBitCount + 7)) >> 3) * FWidth + 7) & ~7;
            // The original allocates 7 bytes more and aligns FData to 8 for MMX. The alignment has
            // no effect on pixels, so the port's data is the array from its first byte. The 7
            // bytes stay after the last row: the 24-bit pixel readers read 4 bytes at the last
            // pixel, which the original's slack absorbs.
            Size = FPitch * FHeight + 7;
            FMem = MemorySurfaces.Reallocate(FMem, Size);
            Result = GFXEUnit.GFXE_OK;
        }
        else
        {
            Result = GFXEUnit.GFXE_OK;
        }

        return Result;
    }

    public byte[] Data { get => FMem; }
    public int Pitch { get => FPitch; }
    public TGFXEPalette Palette { get => FPalette; set => SetPalette(value); }

}

// a DIB section of any format (8, 16 or 24 bit). FHandle, FWidth, FHeight, FMem, FBits, FPitch,
// FPixelFormatID, FInfo (BITMAPINFOHEADER and 256 colors), FPalette
public partial class TDIBSurface : TSurface
{
    internal int FHandle;
    internal int FWidth;
    internal int FHeight;
    internal byte[] FMem;
    internal byte[] FBits;
    internal int FPitch;
    internal int FPixelFormatID;
    internal TBitmapInfoHeader FInfo;
    internal FixedArray_Aow_Rtl_TRGBQuad_256 FColors;
    internal TGFXEPalette FPalette;

    // GFXE.pas:6407
    public override int GetWidth() => FWidth;

    // GFXE.pas:6412
    public override int GetHeight() => FHeight;

    // GFXE.pas:6767
    public void SaveToBMP24(string FileName)
    {
        TSurfaceDesc SD = default;
        Lock(ref SD);
        GFXEUnit.SaveSurfaceDescToBMP24(SD, SD.ClipRect, FileName);
        Unlock();
    }

    public byte[] Bits { get => FBits; }
    public int Pitch { get => FPitch; }
    public TGFXEPalette Palette { get => FPalette; }
}

// an 8 bit bitmap in memory (bottom-up: the pitch is negative). FWidth, FHeight, FMem, FBits,
// FPitch, FInfo (header and 256 colors), FPalette
public partial class TDIB08Surface : TSurface
{
    internal int FWidth;
    internal int FHeight;
    internal byte[] FMem;
    internal byte[] FBits;
    internal int FPitch;
    internal TBitmapInfoHeader FInfo;
    internal FixedArray_Aow_Rtl_TRGBQuad_256 FColors;
    internal TGFXEPalette FPalette;

    // GFXE.pas:6805
    public override int GetPixelFormatID() => GFXEUnit.pfPAL8;

    // GFXE.pas:6810
    public override int GetWidth() => FWidth;

    // GFXE.pas:6815
    public override int GetHeight() => FHeight;

    public TGFXEPalette Palette { get => FPalette; set => SetPalette(value); }

}

// a 16 bit DIB section (555 or 565). FHandle, FWidth, FHeight, FBits, FPitch, FPixelFormatID,
// FInfo, FMasks
public partial class TDIB16Surface : TSurface
{
    internal int FHandle;
    internal int FWidth;
    internal int FHeight;
    internal byte[] FBits;
    internal int FPitch;
    internal int FPixelFormatID;
    internal TBitmapInfoHeader FInfo;
    internal FixedArray_int_3 FMasks;

    // GFXE.pas:6991
    public override int GetWidth() => FWidth;

    // GFXE.pas:6996
    public override int GetHeight() => FHeight;

    // GFXE.pas:7001
    public override int GetPixelFormatID() => FPixelFormatID;

    public new int PixelFormatID { get => FPixelFormatID; set => SetPixelFormatID(value); }

}

// a 24 bit DIB section. FHandle, FWidth, FHeight, FBits, FPitch, FInfo
public partial class TDIB24Surface : TSurface
{
    internal int FHandle;
    internal int FWidth;
    internal int FHeight;
    internal byte[] FBits;
    internal int FPitch;
    internal TBitmapInfoHeader FInfo;

    // GFXE.pas:7238
    public override int GetWidth() => FWidth;

    // GFXE.pas:7243
    public override int GetHeight() => FHeight;

}

// a 32 bit bitmap in memory (bottom-up). FWidth, FHeight, FMem, FBits, FPitch, FInfo
public partial class TDIB32Surface : TSurface
{
    internal int FWidth;
    internal int FHeight;
    internal byte[] FMem;
    internal byte[] FBits;
    internal int FPitch;
    internal TBitmapInfoHeader FInfo;

    // GFXE.pas:7446
    public override int GetWidth() => FWidth;

    // GFXE.pas:7451
    public override int GetHeight() => FHeight;

}

// a window drawn through GDI. FHandle. Two more virtuals: GetHandle, SetHandle
public partial class TGDISurface : TSurface
{

    internal int FHandle;

    // GFXE.pas:12293
    public TGDISurface(int AHandle)
    {
        FHandle = AHandle;
    }

    // GFXE.pas:12299
    public virtual int GetHandle() => FHandle;

    // GFXE.pas:12304
    public virtual void SetHandle(int Value)
    {
        FHandle = Value;
    }

    // GFXE.pas:12309
    public override int GetWidth()
    {
        /* TODO: Implement Windows.GetClientRect in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:12319
    public override int GetHeight()
    {
        /* TODO: Implement Windows.GetClientRect in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:12329
    public override int Blt(TSurface Dest, TRect? SrcRect, int X, int Y) => GFXEUnit.GFXE_NOTSUPPORTED;

    // GFXE.pas:12334
    public override int BltDC(int DC, TRect? SrcRect, TRect? DestRect)
    {
        /* TODO: Implement Windows.GetDC in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:12360
    public override int Resize(int AWidth, int AHeight)
    {
        /* TODO: Implement Windows.SetWindowPos in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:12368
    public override int Cls(int Color)
    {
        /* TODO: Implement Windows.GetDC in the platform or editor service. */
        throw new NotImplementedException();
    }

    public int Handle { get => GetHandle(); set => SetHandle(value); }

}

// a DirectDraw surface. FOnSurfaceLost, FPixelFormatID, FWidth, FHeight, FSurface,
// FCreationFlags, FStatus, FLockCount, FLockExCount, FSurfaceDesc (the description of the lock)
public partial class TDDrawSurface : TSurface
{
    protected internal TNotifyEvent FOnSurfaceLost;
    protected internal int FPixelFormatID;
    protected internal int FWidth;
    protected internal int FHeight;
    protected internal TSet<TDDSCreationFlagsElement> FCreationFlags;
    protected internal TSet<TDDrawSurfaceStatusElement> FStatus;
    protected internal int FLockCount;
    protected internal int FLockExCount;
    protected internal TSurfaceDesc FSurfaceDesc;

    // GFXE.pas:9279
    public override int GetPixelFormatID() => FPixelFormatID;

    // GFXE.pas:9284
    public override int GetWidth() => FWidth;

    // GFXE.pas:9289
    public override int GetHeight() => FHeight;

    public TSet<TDDrawSurfaceStatusElement> Status { get => FStatus; }
    public TSet<TDDSCreationFlagsElement> CreationFlags { get => FCreationFlags; }
    public TNotifyEvent OnSurfaceLost { get => FOnSurfaceLost; set => FOnSurfaceLost = value; }
}

// Rect
public partial class TRectangle : TObject
{
    public TRect Rect;

    // GFXE.pas:2432
    public TRectangle(int ALeft, int ATop, int ARight, int ABottom)
    {
        Rect.Left = ALeft;
        Rect.Top = ATop;
        Rect.Right = ARight;
        Rect.Bottom = ABottom;
    }

    // GFXE.pas:2440
    public TRectangle(in TRect R)
    {
        Rect = R;
    }
}

// a TList of TRectangle, owning them
public partial class TRectangleList : TList
{
    // GFXE.pas:2458
    internal TRectangle GetRectangle(int Index) => (TRectangle)List[Index];

    // GFXE.pas:2447
    public override void Destroy()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            ((TRectangle)this[I]).Destroy();
            Delete(I);
        }
    }

    public sealed class RectanglesAccessor(TRectangleList target)
    {
        public TRectangle this[int Index] { get => target.GetRectangle(Index); }
    }

    public RectanglesAccessor Rectangles => new RectanglesAccessor(this);
}

public delegate void TOnDrawRectEvent(TObject Sender, in TRect R);

// a panel that paints through OnDraw. FEraseBkgndEnabled, FOnDraw
public partial class TDrawPanel : TCustomPanel
{
    internal bool FEraseBkgndEnabled;
    internal TOnDrawRectEvent FOnDraw;

    // GFXE.pas:8730
    protected internal override void WndProc(ref TMessage Message)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:8741
    protected internal override void Paint()
    {
        /* TODO: Implement Controls.ClientRect in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:8721

    public TDrawPanel(TComponent owner) : base(owner)
    {
        /* TODO: Implement ExtCtrls.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    [Published]
    public TOnDrawRectEvent OnDraw { get => FOnDraw; set => FOnDraw = value; }

    [Published]
    public bool EraseBkgndEnabled { get => FEraseBkgndEnabled; set => FEraseBkgndEnabled = value; }
}

// an image file format, all class functions
public partial class TImageFile : TObject, IImageFileClass
{
    // GFXE.pas:693
    public static string Name => throw EAbstractError.Create("Abstract Error");

    // GFXE.pas:694
    public static string Extension => throw EAbstractError.Create("Abstract Error");

    // GFXE.pas:695
    public static string Description => throw EAbstractError.Create("Abstract Error");

    // GFXE.pas:696
    public static int GetPixelFormatID(string FileName)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // GFXE.pas:697
    public static TSurface CreateSurface(string FileName)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // GFXE.pas:698
    public static int LoadToSurfaceDesc(string FileName, ref TSurfaceDesc SD, int X, int Y)
    {
        throw EAbstractError.Create("Abstract Error");
    }

}

// a TOpenDialog whose filter lists the registered image file classes
public partial class TOpenImageFileDialog : TOpenDialog
{
    // GFXE.pas:972
    public override void Loaded()
    {
        /* TODO: Implement Dialogs.Filter in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:966

    public TOpenImageFileDialog(TComponent owner) : base(owner)
    {
        /* TODO: Implement Dialogs.Create in the platform or editor service. */
        throw new NotImplementedException();
    }
}

// PixelFormatID, Width, Height
public partial class TDisplayModeItem : TObject
{
    public int PixelFormatID;
    public int Width;
    public int Height;
}

// FList
public partial class TDisplayModeList : TObject
{
    internal TList FList;

    // GFXE.pas:12629
    internal TDisplayModeItem GetItems(int Index) => (TDisplayModeItem)FList.List[Index];

    // GFXE.pas:12634
    internal int GetCount() => FList.Count;

    // GFXE.pas:12611
    public TDisplayModeList()
    {
        FList = new TList();
    }

    // GFXE.pas:12617
    public override void Destroy()
    {
        Clear();
        FList.Free();
        base.Destroy();
    }

    // GFXE.pas:12624
    public void Clear()
    {
        while (FList.Count != 0)
        {
            Delete(FList.Count - 1);
        }
    }

    // GFXE.pas:12639
    public void Add(int AWidth, int AHeight, int APixelFormatID)
    {
        TDisplayModeItem Item;
        Item = new TDisplayModeItem();
        Item.Width = AWidth;
        Item.Height = AHeight;
        Item.PixelFormatID = APixelFormatID;
        FList.Add(Item);
    }

    // GFXE.pas:12650
    public void Delete(int Index)
    {
        ((TDisplayModeItem)FList[Index]).Free();
        FList.Delete(Index);
    }

    public sealed class ItemsAccessor(TDisplayModeList target)
    {
        public TDisplayModeItem this[int Index] { get => target.GetItems(Index); }
    }

    public ItemsAccessor Items => new ItemsAccessor(this);
    public int Count { get => GetCount(); }

}

// FBits, FTable (the bytes of the table, which the original addresses with a pointer into the
// derived class's own field)
public partial class TBlendTable : TObject
{
    internal int FBits;
    internal byte[] FTable;

    // GFXE.pas:741
    public virtual TBlendTable Copy()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // GFXE.pas:742
    public virtual void Assign(TBlendTable Source)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // GFXE.pas:9601
    public void Initialize(int ABits, byte[] ATable)
    {
        FTable = ATable;
        FBits = ABits;
    }

    public int Bits { get => FBits; }
    public byte[] Table { get => FTable; }
}

// Table (32 x 32 bytes, row I at byte I * 32)
public partial class TBlendTable5Bit : TBlendTable
{
    // The rows of the table, so Table[I][J] reads and writes the byte of `Table[I, J]`.
    public readonly struct Rows(byte[] bytes)
    {
        public Span<byte> this[int I] => bytes.AsSpan(I * 32, 32);
    }

    public new Rows Table => new(FTable);

    // The 32 by 32 bytes of the table in one span, as the blenders read it.
    public ReadOnlySpan<byte> Bytes => FTable;

    // GFXE.pas:9609
    public TBlendTable5Bit()
    {
        Initialize(5, new byte[32 * 32]);
    }

    // GFXE.pas:9621
    public override TBlendTable Copy()
    {
        TBlendTable Result = new TBlendTable5Bit();
        Result.Assign(this);
        return Result;
    }

}

// FList
public partial class TBlendTableList : TObject
{
    internal TList FList;

    // GFXE.pas:9642
    internal int GetCount() => FList.Count;

    // GFXE.pas:9656
    internal TBlendTable GetTables(int Index) => (TBlendTable)FList.List[Index];

    // GFXE.pas:9629
    public TBlendTableList()
    {
        FList = new TList();
    }

    // GFXE.pas:9635
    public override void Destroy()
    {
        Clear();
        FList.Free();
        base.Destroy();
    }

    // GFXE.pas:9647
    public void Clear()
    {
        while (FList.Count != 0)
        {
            ((TBlendTable)FList.List[FList.Count - 1]).Free();
            FList.Delete(FList.Count - 1);
        }
    }

    // GFXE.pas:9661
    public void Add(TBlendTable Table)
    {
        FList.Add(Table);
    }

    // GFXE.pas:9666
    public void Delete(int Index)
    {
        ((TBlendTable)FList.List[Index]).Free();
        FList.Delete(Index);
    }

    public int Count { get => GetCount(); }

    public sealed class TablesAccessor(TBlendTableList target)
    {
        public TBlendTable this[int Index] { get => target.GetTables(Index); }
    }

    public TablesAccessor Tables => new TablesAccessor(this);

}

public delegate void TGenerateBlendTableProc(TBlendTable Table, float Value);

// FTables, Lookup (101 tables, indexed by percentage)
public partial class TLookupBlendTableCollection100 : TObject
{
    internal TBlendTableList FTables;
    // { ImageLib's blitters read Lookup directly; adding lines here moves the Asserts below }
    public FixedArray_Aow_Units_GFXE_TBlendTable_101 Lookup;

    // GFXE.pas:9687
    public TBlendTable GetLookupTables(int Index) => Lookup[Index];

    // GFXE.pas:9674
    public TLookupBlendTableCollection100()
    {
        FTables = new TBlendTableList();
    }

    // GFXE.pas:9680
    public override void Destroy()
    {
        Clear();
        FTables.Free();
        base.Destroy();
    }

    // GFXE.pas:9692
    public void Clear()
    {
        FTables.Clear();
        Lookup = default;
    }

    // GFXE.pas:9698
    public bool GenerateLookupTables(int Bits, TGenerateBlendTableProc Proc, int Count)
    {
        bool Result;
        TBlendTable T;
        float V;
        int J;
        Clear();
        if (Bits == 5 && Count > 0)
        {
            for (int I = 0; I <= Count - 1; I++)
            {
                switch (Bits)
                {
                    case 5:
                        T = new TBlendTable5Bit();
                        break;
                    default:
                        T = null;
                        break;
                }

                if (Count != 1)
                {
                    V = (float)((double)I / (Count - 1));
                }
                else
                {
                    V = (float)0.5;
                }

                Proc(T, V);
                FTables.Add(T);
            }

            for (int I = 0; I <= 100; I++)
            {
                J = I * Count / 100;
                if (J == Count)
                {
                    J = Count - 1;
                }

                var blendTable = FTables.Tables[J];
                Lookup[I] = blendTable;
            }

            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    public TBlendTableList Tables { get => FTables; }

    public sealed class LookupTablesAccessor(TLookupBlendTableCollection100 target)
    {
        public TBlendTable this[int Index] { get => target.GetLookupTables(Index); }
    }

    public LookupTablesAccessor LookupTables => new LookupTablesAccessor(this);

}

public delegate void TProgressEvent(TObject Sender, int Percent, int Param);

// { The implementation list of image file classes (TList of TImageFileClass). }
public partial class TImageFileClassList : TList
{
    // GFXE.pas:941
    internal ImageFileClass GetClasses(int Index)
    {
        object P;
        P = this[Index];
        return (ImageFileClass)P;
    }

    // GFXE.pas:949
    public ImageFileClass FindClass(string Ext)
    {
        ImageFileClass Result;
        int I;
        I = 0;
        if (!IsEmpty(Ext) && Ext[0] == 46)
        {
            Ext = Copy(Ext, 2, Ext.Length - 1);
        }

        while (I != Count && CompareText(((ImageFileClass)List[I]).Extension, Ext) != 0)
        {
            I += 1;
        }

        if (I != Count)
        {
            Result = (ImageFileClass)List[I];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    public sealed class ClassesAccessor(TImageFileClassList target)
    {
        public ImageFileClass this[int Index] { get => target.GetClasses(Index); }
    }

    public ClassesAccessor Classes => new ClassesAccessor(this);
}

// the Windows bitmap file format
public partial class TBMPImageFile : TImageFile, IImageFileClass
{
    // GFXE.pas:9043
    public static new string Extension => "BMP";

    // GFXE.pas:9048
    public static new string Description => "Bitmap files (*.bmp)";

    // GFXE.pas:9053
    public static new int GetPixelFormatID(string FileName) => GFXEUnit.GetBMPPixelFormat(FileName);

    // GFXE.pas:9058
    public static new TSurface CreateSurface(string FileName) => GFXEUnit.CreateSurfaceFromBMP(FileName);

    // GFXE.pas:9063
    public static new int LoadToSurfaceDesc(string FileName, ref TSurfaceDesc SD, int X, int Y) => GFXEUnit.LoadBMP(FileName, ref SD, X, Y);

}

public static partial class GFXEUnit
{
    // The GFXE unit of Gfxepack.dpl (the graphics engine of Age of Wonders: surfaces, blitters,
    // blend tables, DirectDraw access). The original was D:\GEngine\GFXE\GFXMAIN\GFXE.pas with
    // inc\DIBSURF.PAS and inc\DDSurf.PAS included (the Assert line numbers tell). The original
    // declares the DirectDraw interfaces and GUIDs in this unit itself. The port has no
    // DirectDraw (graphics.md 1, decision 3) and drops them with the blit timing tests of
    // GetOptimalGFXESettings and the settings routines (graphics.md 2). The assembler
    // blitters are transcribed from the disassembly: `asm` blocks compile verbatim. the MMX
    // instructions are DB-encoded (Delphi 3 has no MMX mnemonics)
    // { HResult codes returned by the surfaces (GetErrorMsg) }
    public const byte GFXE_OK = 0;
    public const int GFXE_ERROR = -2147483647;
    public const int GFXE_NOTSUPPORTED = -2147483646;
    public const int GFXE_PIXELFORMATNOTSUPPORTED = -2147483645;
    public const int GFXE_BITMAPFORMATNOTSUPPORTED = -2147483644;
    public const int GFXE_CANNOTLOCK = -2147483643;
    // { Pixel format IDs (MASKtoPFID): bits 0-7 the bit count, bits 8-15 the channel order, the
    //     high nibbles the bits per channel (A, B, G, R) }
    public const byte pfPAL8 = 8;
    public const int pfRGB555 = 1431343888;
    public const int pfARGB1555 = 1431409424;
    public const int pfRGB565 = 1448121104;
    public const int pfRGB24 = -2004839656;
    public const int pfBGR24 = -2004870376;
    public const int pfRGB32 = -2004315360;
    public const int pfXRGB32 = -2004839648;
    // { The result of the DirectDraw surface methods that TDDrawSurface keeps }
    public const byte DD_OK = 0;

    public static TLookupBlendTableCollection100 AlphaBlendTable;
    public static TLookupBlendTableCollection100 IntensityBlendTable;
    public static TLookupBlendTableCollection100 BrightenBlendTable;
    public static TLookupBlendTableCollection100 ShadowBlendTable;
    public static TLookupBlendTableCollection100 LinearAlphaBlendTable;

    // GFXE.pas:978
    public static string GetImageFilesFilter()
    {
        string Result = "";
        for (int I = 0, last = ImageFileClassList.Count - 1; I <= last; I++)
        {
            if (!IsEmpty(Result))
            {
                Result = Result + "|";
            }

            Result = Result + ImageFileClassList.Classes[I].Description + "|*." + ImageFileClassList.Classes[I].Extension;
        }

        return Result;
    }

    // GFXE.pas:991
    public static bool ValidImageFile(string FileName)
    {
        var extractFileExt = ExtractFileExt(FileName);
        return ImageFileClassList.FindClass(extractFileExt) != null;
    }

    // GFXE.pas:996
    public static int LoadImageFile(string FileName, ref TSurfaceDesc SD, int X, int Y)
    {
        int Result;
        ImageFileClass C;
        var extractFileExt = ExtractFileExt(FileName);
        C = ImageFileClassList.FindClass(extractFileExt);
        if (C != null)
        {
            Result = C.LoadToSurfaceDesc(FileName, ref SD, X, Y);
        }
        else
        {
            Result = GFXE_NOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:1007
    public static TSurface CreateSurfaceFromImageFile(string FileName)
    {
        TSurface Result;
        ImageFileClass C;
        var extractFileExt = ExtractFileExt(FileName);
        C = ImageFileClassList.FindClass(extractFileExt);
        if (C != null)
        {
            Result = C.CreateSurface(FileName);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // GFXE.pas:1018
    public static int GetImageFilePixelFormat(string FileName)
    {
        int Result;
        ImageFileClass C;
        var extractFileExt = ExtractFileExt(FileName);
        C = ImageFileClassList.FindClass(extractFileExt);
        if (C != null)
        {
            Result = C.GetPixelFormatID(FileName);
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // GFXE.pas:1029
    public static void RegisterImageFileClasses(ReadOnlySpan<ImageFileClass> AClasses)
    {
        for (int I = 0, last = AClasses.Length - 1; I <= last; I++)
        {
            var extension = AClasses[I].Extension;
            if (ImageFileClassList.FindClass(extension) == null)
            {
                ImageFileClassList.Add(AClasses[I]);
            }
            else
            {
                throw Exception.Create("ImageFile class with " + AClasses[I].Extension + " extension is already registered");
            }
        }
    }

    // GFXE.pas:1748
    public static int BltSDRectClipped(in TSurfaceDesc Src, TRect SrcRect, in TSurfaceDesc Dest, int X, int Y)
    {
        int Result;
        int W;
        int H;
        SurfaceView S;
        SurfaceView D;
        if (SrcRect.Right > Src.ClipRect.Right)
        {
            SrcRect.Right = Src.ClipRect.Right;
        }

        if (SrcRect.Left < Src.ClipRect.Left)
        {
            X = X + (Src.ClipRect.Left - SrcRect.Left);
            SrcRect.Left = Src.ClipRect.Left;
        }

        if (X < Dest.ClipRect.Left)
        {
            SrcRect.Left = SrcRect.Left + (Dest.ClipRect.Left - X);
            X = Dest.ClipRect.Left;
        }

        W = SrcRect.Right - SrcRect.Left;
        if (X + W > Dest.ClipRect.Right)
        {
            W = W - (X + W - Dest.ClipRect.Right);
        }

        if (SrcRect.Bottom > Src.ClipRect.Bottom)
        {
            SrcRect.Bottom = Src.ClipRect.Bottom;
        }

        if (SrcRect.Top < Src.ClipRect.Top)
        {
            Y = Y + (Src.ClipRect.Top - SrcRect.Top);
            SrcRect.Top = Src.ClipRect.Top;
        }

        if (Dest.ClipRect.Top > Y)
        {
            SrcRect.Top = SrcRect.Top + (Dest.ClipRect.Top - Y);
            Y = Dest.ClipRect.Top;
        }

        H = SrcRect.Bottom - SrcRect.Top;
        if (Y + H > Dest.ClipRect.Bottom)
        {
            H = H - (Y + H - Dest.ClipRect.Bottom);
        }

        if (Src.PixelFormatID == Dest.PixelFormatID)
        {
            if (W > 0 && H > 0)
            {
                W = (int)(((uint)(Src.BitCount * W)) >> 3);
                S = Src.View(Src.Pitch * SrcRect.Top + (int)(((uint)(Src.BitCount * SrcRect.Left)) >> 3));
                D = Dest.View(Dest.Pitch * Y + (int)(((uint)(Dest.BitCount * X)) >> 3));
                BltRect(S, D, W, H);
            }

            Result = GFXE_OK;
        }
        else
        {
            Result = GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:2141
    public static int BltSprite16(in TSurfaceDesc Src, TRect SrcRect, in TSurfaceDesc Dest, int X, int Y, int ColorKey)
    {
        int Result;
        int Key;
        SurfaceView S;
        SurfaceView D;
        if (Src.BitCount == 16 && Dest.PixelFormatID == Src.PixelFormatID)
        {
            Key = Color2PixelInfo(ColorKey, Src.PixelFormatID, null);
            S = Src.View(Src.Pitch * SrcRect.Top + SrcRect.Left * 2);
            D = Dest.View(Dest.Pitch * Y + X * 2);
            BltColorKey16(S, D, SrcRect.Right - SrcRect.Left, SrcRect.Bottom - SrcRect.Top, Key);
            Result = GFXE_OK;
        }
        else
        {
            Result = GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:2158
    public static int BltSpriteClipped16(in TSurfaceDesc Src, TRect SrcRect, in TSurfaceDesc Dest, int X, int Y, int ColorKey)
    {
        int Result;
        int W;
        int H;
        SurfaceView S;
        SurfaceView D;
        int Key;
        if (Src.BitCount == 16 && Dest.BitCount == 16)
        {
            if (SrcRect.Right > Src.ClipRect.Right)
            {
                SrcRect.Right = Src.ClipRect.Right;
            }

            if (SrcRect.Left < Src.ClipRect.Left)
            {
                X = X + (Src.ClipRect.Left - SrcRect.Left);
                SrcRect.Left = Src.ClipRect.Left;
            }

            if (X < Dest.ClipRect.Left)
            {
                SrcRect.Left = SrcRect.Left + (Dest.ClipRect.Left - X);
                X = Dest.ClipRect.Left;
            }

            W = SrcRect.Right - SrcRect.Left;
            if (W + X > Dest.ClipRect.Right)
            {
                W = W - (W + X - Dest.ClipRect.Right);
            }

            if (SrcRect.Bottom > Src.ClipRect.Bottom)
            {
                SrcRect.Bottom = Src.ClipRect.Bottom;
            }

            if (SrcRect.Top < Src.ClipRect.Top)
            {
                Y = Y + (Src.ClipRect.Top - SrcRect.Top);
                SrcRect.Top = Src.ClipRect.Top;
            }

            if (Y < Dest.ClipRect.Top)
            {
                SrcRect.Top = SrcRect.Top + (Dest.ClipRect.Top - Y);
                Y = Dest.ClipRect.Top;
            }

            H = SrcRect.Bottom - SrcRect.Top;
            if (Y + H > Dest.ClipRect.Bottom)
            {
                H = H - (Y + H - Dest.ClipRect.Bottom);
            }

            if (W > 0 && H > 0)
            {
                Key = Color2PixelInfo(ColorKey, Src.PixelFormatID, null);
                S = Src.View(Src.Pitch * SrcRect.Top + SrcRect.Left * 2);
                D = Dest.View(Dest.Pitch * Y);
                X += X;
                D = D.At(X);
                BltColorKey16(S, D, W, H, Key);
            }

            Result = GFXE_OK;
        }
        else
        {
            Result = GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:2326
    public static int GetPixelInfo(in TSurfaceDesc SD, int X, int Y)
    {
        int Result;
        switch (SD.BitCount)
        {
            case 8:
                Result = SD.View().Byte(X + SD.Pitch * Y);
                break;
            case 16:
                Result = SD.View().Word(X + X + SD.Pitch * Y);
                break;
            case 24:
                // Four bytes at the last pixel of a row too, without the width test of
                // TSurface.GetPixelInfo (GFXE.pas:2331, graphics.md 3.6).
                Result = (int)SD.View().Dword(X * 3 + SD.Pitch * Y) & 16777215;
                break;
            case 32:
                Result = (int)SD.View().Dword(X * 4 + SD.Pitch * Y);
                break;
            default:
                throw Exception.Create("PixelFormat is not supported");
        }

        return Result;
    }

    // GFXE.pas:2340
    // { The pixel format ID: bits 0-7 the bit count, bits 8-15 the position (0-3) of each channel
    //   (A, B, G, R) counted from the low end, bits 16-31 the bits per channel (A, B, G, R). }
    public static int MASKtoPFID(int BitCount, int RMask, int GMask, int BMask, int AMask)
    {
        FixedArray_byte_4 Order = default;
        bool Swapped;
        FixedArray_int_4 Masks = default;
        int Bits;
        int T;
        int Result = BitCount;
        Masks[0] = AMask;
        Order[3] = 3;
        Masks[1] = BMask;
        Order[0] = 0;
        Masks[2] = GMask;
        Order[1] = 1;
        Masks[3] = RMask;
        Order[2] = 2;
        do
        {
            Swapped = false;
            for (int I = 0; I <= 2; I++)
            {
                if (((Masks[Order[I]] < 0 || Masks[Order[I + 1]] < Masks[Order[I]]) && Masks[Order[I + 1]] > 0) || (Masks[Order[I]] == 0 && Masks[Order[I + 1]] != 0))
                {
                    T = Order[I];
                    Order[I] = Order[I + 1];
                    Order[I + 1] = (byte)T;
                    Swapped = true;
                }
            }
        }
        while (!!Swapped);
        for (int I = 0; I <= 3; I++)
        {
            Result = Result + (I << (Order[I] * 2 + 8));
        }

        for (int I = 0; I <= 3; I++)
        {
            Bits = 0;
            while (Masks[I] != 0)
            {
                if ((Masks[I] & 1) != 0)
                {
                    Bits += 1;
                }

                Masks[I] = (int)(((uint)Masks[I]) >> 1);
            }

            Result = Result + (Bits << (I * 4 + 16));
        }

        return Result;
    }

    // GFXE.pas:2382
    public static int PFIDtoMASK(int PixelFormatID, ref int RMask, ref int GMask, ref int BMask, ref int AMask)
    {
        int BitCount;
        FixedArray_byte_4 Order;
        int Pos;
        FixedArray_int_4 Masks;
        int J;
        int Bits;
        int M;
        Order = default;
        Masks = default;
        BitCount = PixelFormatID & 255;
        Pos = 0;
        Order[((int)(((uint)PixelFormatID) >> 8)) & 3] = 0;
        Order[((int)(((uint)PixelFormatID) >> 10)) & 3] = 1;
        Order[((int)(((uint)PixelFormatID) >> 12)) & 3] = 2;
        Order[((int)(((uint)PixelFormatID) >> 14)) & 3] = 3;
        for (int I = 0; I <= 3; I++)
        {
            Bits = ((int)(((uint)PixelFormatID) >> (Order[I] * 4 + 16))) & 15;
            J = Bits;
            M = 0;
            while (J != 0)
            {
                M = M * 2 + 1;
                J -= 1;
            }

            M = M << Pos;
            Pos = Pos + Bits;
            Masks[Order[I]] = M;
        }

        AMask = Masks[0];
        BMask = Masks[1];
        GMask = Masks[2];
        RMask = Masks[3];
        return BitCount;
    }

    // GFXE.pas:2419
    public static void CreateSDfromSD(in TSurfaceDesc Src, in TRect R, ref TSurfaceDesc Dest)
    {
        Dest = Src;
        IntersectRect(ref Dest.ClipRect, Src.ClipRect, R);
        Dest.DataIndex = Src.DataIndex + R.Top * Src.Pitch + (int)(((uint)Src.BitCount) >> 3) * R.Left;
        Dest.ClipRect.Left -= R.Left;
        Dest.ClipRect.Right -= R.Left;
        Dest.ClipRect.Top -= R.Top;
        Dest.ClipRect.Bottom -= R.Top;
    }

    // GFXE.pas:2465
    // { The rectangles of a surface: the maximal rectangles of TransparentColor bordered by other
    //   colors, found from their top left corners. }
    public static TRectangleList GetSurfaceRectangles(TSurfaceDesc SD, int TransparentColor, TProgressEvent OnProgress)
    {
        TRectangleList Result;
        TRectangleList List;
        TRectangle R;
        ushort X;
        ushort Y;
        int GetPixel(int X, int Y)
        {
            int Result;
            int P;
            switch (SD.BitCount)
            {
                case 8:
                    Result = SD.View().Byte(X + SD.Pitch * Y);
                    break;
                case 16:
                    Result = SD.View().Word(X + X + SD.Pitch * Y);
                    break;
                case 24:
                    if (X + 1 < SD.Width)
                    {
                        Result = (int)SD.View().Dword(X + X + X + SD.Pitch * Y) & 16777215;
                    }
                    else
                    {
                        P = X + X + X + SD.Pitch * Y;
                        Result = SD.View().Word(P) + (SD.View().Byte(P + 2) << 16);
                    }

                    break;
                case 32:
                    Result = (int)SD.View().Dword((X << 2) + SD.Pitch * Y);
                    break;
                default:
                    throw Exception.Create("PixelFormat is not supported");
            }

            return Result;
        }

        TRectangle FindRectangle(int X, int Y)
        {
            int W;
            int H;
            int I;
            bool Ok;
            TRectangle R;
            R = null;
            W = 1;
            H = 1;
            while (GetPixel(X + W, Y) == TransparentColor && GetPixel(X + W, Y + 1) != TransparentColor && X + W < SD.Width - 1)
            {
                W += 1;
            }

            if (GetPixel(X + W, Y) == TransparentColor && GetPixel(X + W, Y + 1) == TransparentColor && X + W < SD.Width - 1 && W > 1)
            {
                while (GetPixel(X, Y + H) == TransparentColor && GetPixel(X + W, Y + H) == TransparentColor && GetPixel(X + 1, Y + H) != TransparentColor && GetPixel(X + W - 1, Y + H) != TransparentColor && Y + H < SD.Height)
                {
                    H += 1;
                }

                if (GetPixel(X, Y + H) == TransparentColor && GetPixel(X + 1, Y + H) == TransparentColor && GetPixel(X + W, Y + H) == TransparentColor && GetPixel(X + W - 1, Y + H) == TransparentColor && Y + H < SD.Height && H > 1)
                {
                    Ok = true;
                    I = 1;
                    while (Ok && I != W)
                    {
                        Ok = GetPixel(X + I, Y + H) == TransparentColor;
                        I += 1;
                    }

                    if (Ok)
                    {
                        R = new TRectangle(X + 1, Y + 1, X + W, Y + H);
                    }
                }
            }

            return R;
        }

        List = new TRectangleList();
        for (int yOrdinal = 0, last = SD.Height - 1; yOrdinal <= last; yOrdinal++)
        {
            Y = (ushort)yOrdinal;
            for (int xOrdinal = 0, lastXOrdinal = SD.Width - 1; xOrdinal <= lastXOrdinal; xOrdinal++)
            {
                X = (ushort)xOrdinal;
                if (GetPixel(X, Y) == TransparentColor)
                {
                    R = FindRectangle(X, Y);
                    if (R != null)
                    {
                        List.Add(R);
                    }
                }
            }

            if (OnProgress != null && (Y % 20 == 0 || Y == SD.Height - 1))
            {
                OnProgress(null, Round((double)(Y * 100) / (SD.Height - 1)), 0);
            }
        }

        if (List.Count == 0)
        {
            List.Free();
            Result = null;
        }
        else
        {
            Result = List;
        }

        return Result;
    }

    // GFXE.pas:2547
    // { Shrinks R to the part of the surface that is not TransparentColor (MinimizeTransparentRect)
    //   or that is TransparentColor (MinimizeInvTransparentRect). }
    public static void MinimizeTransparentRect(in TSurfaceDesc SD, in TRect R, int TransparentColor, ref TRect Result)
    {
        TSurfaceDesc SD__value = SD;
        bool Found;
        int X;
        int Y;
        TRect Rect;
        int GetPixel(int X, int Y)
        {
            int Result;
            switch (SD__value.BitCount)
            {
                case 8:
                    Result = SD__value.View().Byte(X + SD__value.Pitch * Y);
                    break;
                case 16:
                    Result = SD__value.View().Word(X + X + SD__value.Pitch * Y);
                    break;
                case 24:
                    // Four bytes at the last pixel of a row too, as GFXEUnit.GetPixelInfo.
                    Result = (int)SD__value.View().Dword(X * 3 + SD__value.Pitch * Y) & 16777215;
                    break;
                case 32:
                    Result = (int)SD__value.View().Dword(X * 4 + SD__value.Pitch * Y);
                    break;
                default:
                    throw Exception.Create("PixelFormat is not supported");
            }

            return Result;
        }

        Rect = R;
        Found = false;
        while (!Found && Rect.Top < Rect.Bottom)
        {
            X = Rect.Left;
            while (!Found && X != Rect.Right)
            {
                Found = GetPixel(X, Rect.Top) != TransparentColor;
                X += 1;
            }

            if (!Found)
            {
                Rect.Top += 1;
            }
        }

        Found = false;
        while (!Found && Rect.Bottom > Rect.Top)
        {
            X = Rect.Left;
            while (!Found && X != Rect.Right)
            {
                Found = GetPixel(X, Rect.Bottom - 1) != TransparentColor;
                X += 1;
            }

            if (!Found)
            {
                Rect.Bottom -= 1;
            }
        }

        Found = false;
        while (!Found && Rect.Left < Rect.Right)
        {
            Y = Rect.Top;
            while (!Found && Y != Rect.Bottom)
            {
                Found = GetPixel(Rect.Left, Y) != TransparentColor;
                Y += 1;
            }

            if (!Found)
            {
                Rect.Left += 1;
            }
        }

        Found = false;
        while (!Found && Rect.Right > Rect.Left)
        {
            Y = Rect.Top;
            while (!Found && Y != Rect.Bottom)
            {
                Found = GetPixel(Rect.Right - 1, Y) != TransparentColor;
                Y += 1;
            }

            if (!Found)
            {
                Rect.Right -= 1;
            }
        }

        Result = Rect;
    }

    // GFXE.pas:2614
    public static void MinimizeInvTransparentRect(in TSurfaceDesc SD, in TRect R, int TransparentColor, ref TRect Result)
    {
        TSurfaceDesc SD__value = SD;
        bool Found;
        int X;
        int Y;
        TRect Rect;
        int GetPixel(int X, int Y)
        {
            int Result;
            switch (SD__value.BitCount)
            {
                case 8:
                    Result = SD__value.View().Byte(X + SD__value.Pitch * Y);
                    break;
                case 16:
                    Result = SD__value.View().Word(X + X + SD__value.Pitch * Y);
                    break;
                case 24:
                    // Four bytes at the last pixel of a row too, as GFXEUnit.GetPixelInfo.
                    Result = (int)SD__value.View().Dword(X * 3 + SD__value.Pitch * Y) & 16777215;
                    break;
                case 32:
                    Result = (int)SD__value.View().Dword(X * 4 + SD__value.Pitch * Y);
                    break;
                default:
                    throw Exception.Create("PixelFormat is not supported");
            }

            return Result;
        }

        Rect = R;
        Found = false;
        while (!Found && Rect.Top < Rect.Bottom)
        {
            X = Rect.Left;
            while (!Found && X != Rect.Right)
            {
                Found = GetPixel(X, Rect.Top) == TransparentColor;
                X += 1;
            }

            if (!Found)
            {
                Rect.Top += 1;
            }
        }

        Found = false;
        while (!Found && Rect.Bottom > Rect.Top)
        {
            X = Rect.Left;
            while (!Found && X != Rect.Right)
            {
                Found = GetPixel(X, Rect.Bottom - 1) == TransparentColor;
                X += 1;
            }

            if (!Found)
            {
                Rect.Bottom -= 1;
            }
        }

        Found = false;
        while (!Found && Rect.Left < Rect.Right)
        {
            Y = Rect.Top;
            while (!Found && Y != Rect.Bottom)
            {
                Found = GetPixel(Rect.Left, Y) == TransparentColor;
                Y += 1;
            }

            if (!Found)
            {
                Rect.Left += 1;
            }
        }

        Found = false;
        while (!Found && Rect.Right > Rect.Left)
        {
            Y = Rect.Top;
            while (!Found && Y != Rect.Bottom)
            {
                Found = GetPixel(Rect.Right - 1, Y) == TransparentColor;
                Y += 1;
            }

            if (!Found)
            {
                Rect.Right -= 1;
            }
        }

        Result = Rect;
    }

    // GFXE.pas:2958
    // { Skip is not passed on: DrawLine16 always draws every pixel (original). }
    public static void DrawLine(int X1, int Y1, int X2, int Y2, int Skip, int Color, in TSurfaceDesc SD)
    {
        int Pixel;
        switch (SD.PixelFormatID)
        {
            case pfRGB555:
            case pfARGB1555:
            case pfRGB565:
                Pixel = Color2PixelInfo(Color, SD.PixelFormatID, null);
                DrawLine16(X1, Y1, X2, Y2, 0, (ushort)Pixel, SD);
                break;
        }
    }

    // GFXE.pas:8325
    public static void RemapSurface24to565(SurfaceView Src, SurfaceView Dst, int Width, int Height)
    {
        for (int Y = 0; Y <= Height - 1; Y++)
        {
            var S = Src.Row(Y, Width * 3);
            var D = Words(Dst.Row(Y, Width * 2));
            for (int X = 0; X <= Width - 1; X++)
            {
                int I = X * 3;
                D[X] = (ushort)(((S[I + 2] & 248) << 8) + ((S[I + 1] & 252) << 3) + (S[I] >> 3));
            }
        }
    }

    // GFXE.pas:8345
    public static void RemapSurface24to555(SurfaceView Src, SurfaceView Dst, int Width, int Height)
    {
        for (int Y = 0; Y <= Height - 1; Y++)
        {
            var S = Src.Row(Y, Width * 3);
            var D = Words(Dst.Row(Y, Width * 2));
            for (int X = 0; X <= Width - 1; X++)
            {
                int I = X * 3;
                D[X] = (ushort)(((S[I + 2] & 248) << 7) + ((S[I + 1] & 248) << 2) + (S[I] >> 3));
            }
        }
    }

    // GFXE.pas:8365
    public static void RemapSurface555to24(SurfaceView Src, SurfaceView Dst, int Width, int Height)
    {
        for (int Y = 0; Y <= Height - 1; Y++)
        {
            var S = Words(Src.Row(Y, Width * 2));
            var D = Dst.Row(Y, Width * 3);
            for (int X = 0; X <= Width - 1; X++)
            {
                int I = X * 3;
                D[I + 2] = (byte)((S[X] & 31744) >> 7);
                D[I + 1] = (byte)((S[X] & 992) >> 2);
                D[I] = (byte)((S[X] & 31) << 3);
            }
        }
    }

    // GFXE.pas:8386
    public static void RemapSurface565to24(SurfaceView Src, SurfaceView Dst, int Width, int Height)
    {
        for (int Y = 0; Y <= Height - 1; Y++)
        {
            var S = Words(Src.Row(Y, Width * 2));
            var D = Dst.Row(Y, Width * 3);
            for (int X = 0; X <= Width - 1; X++)
            {
                int I = X * 3;
                D[I + 2] = (byte)((S[X] & 63488) >> 8);
                D[I + 1] = (byte)((S[X] & 2016) >> 3);
                D[I] = (byte)((S[X] & 31) << 3);
            }
        }
    }

    // GFXE.pas:8408
    // { Both 32 bit converters produce 555 pixels from bytes 1-3 of the source (original). }
    public static void RemapSurface32to565(SurfaceView Src, SurfaceView Dst, int Width, int Height) => RemapSurface32to555(Src, Dst, Width, Height);

    // GFXE.pas:8428
    public static void RemapSurface32to555(SurfaceView Src, SurfaceView Dst, int Width, int Height)
    {
        for (int Y = 0; Y <= Height - 1; Y++)
        {
            var S = Src.Row(Y, Width * 4);
            var D = Words(Dst.Row(Y, Width * 2));
            for (int X = 0; X <= Width - 1; X++)
            {
                int I = X * 4;
                D[X] = (ushort)(((S[I + 1] & 248) << 7) + ((S[I + 2] & 248) << 2) + (S[I + 3] >> 3));
            }
        }
    }

    // GFXE.pas:8490
    public static void RemapSurface08to24(SurfaceView Src, SurfaceView Dst, int Width, int Height, TGFXEPalette Palette)
    {
        for (int Y = 0; Y <= Height - 1; Y++)
        {
            var S = Src.Row(Y, Width);
            var D = Dst.Row(Y, Width * 3);
            for (int X = 0; X <= Width - 1; X++)
            {
                ref var E = ref Palette.Entries[S[X]];
                int I = X * 3;
                D[I + 2] = E.R;
                D[I + 1] = E.G;
                D[I] = E.B;
            }
        }
    }

    // The 16-bit pixels of a row, little-endian as the original's PWord.
    private static Span<ushort> Words(Span<byte> row) => MemoryMarshal.Cast<byte, ushort>(row);

    // GFXE.pas:8513
    public static int RemapSurfacePart(in TSurfaceDesc Src, int SrcX, int SrcY, int Width, int Height, in TSurfaceDesc Dest, int DestX, int DestY)
    {
        SurfaceView S;
        SurfaceView D;
        int Result = GFXE_OK;
        if (SrcX + Width > Src.Width)
        {
            Width = Src.Width - SrcX;
        }

        if (SrcY + Height > Src.Height)
        {
            Height = Src.Height - SrcY;
        }

        if (SrcX < 0)
        {
            DestX = DestX - SrcX;
            Width = Width + SrcX;
            SrcX = 0;
        }

        if (SrcY < 0)
        {
            DestY = DestY - SrcY;
            Height = Height + SrcY;
            SrcY = 0;
        }

        if (DestX + Width > Dest.Width)
        {
            Width = Dest.Width - DestX;
        }

        if (DestY + Height > Dest.Height)
        {
            Height = Dest.Height - DestY;
        }

        if (DestX < 0)
        {
            SrcX = SrcX - DestX;
            Width = Width + DestX;
            DestX = 0;
        }

        if (DestY < 0)
        {
            SrcY = SrcY - DestY;
            Height = Height + DestY;
            DestY = 0;
        }

        if (Width > 0 && Height > 0)
        {
            S = Src.View(Src.Pitch * SrcY + (int)(((uint)(Src.BitCount + 7)) >> 3) * SrcX);
            D = Dest.View(Dest.Pitch * DestY + (int)(((uint)(Dest.BitCount + 7)) >> 3) * DestX);
            switch (Src.PixelFormatID)
            {
                case pfPAL8:
                    switch (Dest.PixelFormatID)
                    {
                        case pfRGB565:
                            RemapSurface08to565(S, D, Width, Height, Src.Palette);
                            break;
                        case pfRGB555:
                            RemapSurface08to555(S, D, Width, Height, Src.Palette);
                            break;
                        case pfRGB24:
                            RemapSurface08to24(S, D, Width, Height, Src.Palette);
                            break;
                        case pfPAL8:
                            BltRect(S, D, Width, Height);
                            break;
                        default:
                            Result = GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case pfRGB555:
                case pfARGB1555:
                    switch (Dest.PixelFormatID)
                    {
                        case pfRGB24:
                            RemapSurface555to24(S, D, Width, Height);
                            break;
                        case pfRGB32:
                            RemapSurface555to32(S, D, Width, Height);
                            break;
                        case pfRGB565:
                            RemapSurface555to565(S, D, Width, Height);
                            break;
                        case pfRGB555:
                            BltRect(S, D, Width * 2, Height);
                            break;
                        default:
                            Result = GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case pfRGB565:
                    switch (Dest.PixelFormatID)
                    {
                        case pfRGB24:
                            RemapSurface565to24(S, D, Width, Height);
                            break;
                        case pfRGB32:
                            RemapSurface565to32(S, D, Width, Height);
                            break;
                        case pfRGB555:
                            RemapSurface565to555(S, D, Width, Height);
                            break;
                        case pfRGB565:
                            BltRect(S, D, Width * 2, Height);
                            break;
                        default:
                            Result = GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case pfRGB24:
                    switch (Dest.PixelFormatID)
                    {
                        case pfRGB565:
                            RemapSurface24to565(S, D, Width, Height);
                            break;
                        case pfRGB555:
                            RemapSurface24to555(S, D, Width, Height);
                            break;
                        case pfRGB24:
                            BltRect(S, D, Width * 3, Height);
                            break;
                        default:
                            Result = GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case pfRGB32:
                    switch (Dest.PixelFormatID)
                    {
                        case pfRGB565:
                            RemapSurface32to565(S, D, Width, Height);
                            break;
                        case pfRGB555:
                            RemapSurface32to555(S, D, Width, Height);
                            break;
                        case pfRGB32:
                            BltRect(S, D, Width * 4, Height);
                            break;
                        default:
                            Result = GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                default:
                    Result = GFXE_NOTSUPPORTED;
                    break;
            }
        }

        return Result;
    }

    // GFXE.pas:8600
    public static int RemapSurface(TSurface Src, TSurface Dest)
    {
        TSurfaceDesc SSD = default;
        TSurfaceDesc DSD = default;
        Src.Lock(ref SSD);
        Dest.Lock(ref DSD);
        int Result = RemapSurfacePart(SSD, 0, 0, SSD.Width, SSD.Height, DSD, 0, 0);
        Src.Unlock();
        Dest.Unlock();
        return Result;
    }

    // GFXE.pas:8637
    public static int PixelInfo2Color(int PixelInfo, int PixelFormatID, TGFXEPalette Palette) => RemapPixelInfo(PixelInfo, PixelFormatID, Palette, pfBGR24, null);

    // GFXE.pas:8642
    public static int RemapPixelInfo(int PixelInfo, int SrcPixelFormatID, TGFXEPalette SrcPalette, int DestPixelFormatID, TGFXEPalette DestPalette)
    {
        int Result;
        byte R;
        byte G;
        byte B;
        byte A;
        switch (SrcPixelFormatID)
        {
            case pfPAL8:
                if (SrcPalette != null)
                {
                    R = SrcPalette.Entries[PixelInfo].R;
                    G = SrcPalette.Entries[PixelInfo].G;
                    B = SrcPalette.Entries[PixelInfo].B;
                    A = 0;
                }
                else
                {
                    throw Exception.Create("Source Palette needed (GFXE)");
                }

                break;
            case pfRGB565:
                R = (byte)(((int)(((uint)PixelInfo) >> 8)) & 248);
                G = (byte)(((int)(((uint)PixelInfo) >> 3)) & 252);
                B = (byte)((PixelInfo << 3) & 248);
                A = 0;
                break;
            case pfRGB555:
                R = (byte)(((int)(((uint)PixelInfo) >> 7)) & 248);
                G = (byte)(((int)(((uint)PixelInfo) >> 2)) & 248);
                B = (byte)((PixelInfo << 3) & 248);
                A = 0;
                break;
            case pfARGB1555:
                R = (byte)(((int)(((uint)PixelInfo) >> 7)) & 248);
                G = (byte)(((int)(((uint)PixelInfo) >> 2)) & 248);
                B = (byte)((PixelInfo << 3) & 248);
                A = (byte)(((int)(((uint)PixelInfo) >> 8)) & 1);
                break;
            case pfRGB24:
                R = (byte)(((int)(((uint)PixelInfo) >> 16)) & 255);
                G = (byte)(((int)(((uint)PixelInfo) >> 8)) & 255);
                B = (byte)(PixelInfo & 255);
                A = 0;
                break;
            case pfBGR24:
                R = (byte)(PixelInfo & 255);
                G = (byte)(((int)(((uint)PixelInfo) >> 8)) & 255);
                B = (byte)(((int)(((uint)PixelInfo) >> 16)) & 255);
                A = 0;
                break;
            case pfRGB32:
                A = (byte)(((int)(((uint)PixelInfo) >> 24)) & 255);
                R = (byte)(((int)(((uint)PixelInfo) >> 16)) & 255);
                G = (byte)(((int)(((uint)PixelInfo) >> 8)) & 255);
                B = (byte)(PixelInfo & 255);
                break;
            default:
                throw Exception.Create("Source PixelFormat not supported");
        }

        switch (DestPixelFormatID)
        {
            case pfPAL8:
                if (DestPalette != null)
                {
                    Result = DestPalette.GetNearestColor(R + (G << 8) + (B << 16));
                }
                else
                {
                    throw Exception.Create("Destination Palette needed");
                }

                break;
            case pfRGB565:
                Result = ((R & 248) << 8) + ((G & 252) << 3) + (int)(((uint)(B & 248)) >> 3);
                break;
            case pfRGB555:
                Result = ((R & 248) << 7) + ((G & 248) << 2) + (int)(((uint)(B & 248)) >> 3);
                break;
            case pfARGB1555:
                Result = ((A & 128) << 8) + ((R & 248) << 7) + ((G & 248) << 2) + (int)(((uint)(B & 248)) >> 3);
                break;
            case pfRGB24:
                Result = (R << 16) + (G << 8) + B;
                break;
            case pfBGR24:
                Result = R + (G << 8) + (B << 16);
                break;
            case pfRGB32:
                Result = (A << 24) + (R << 16) + (G << 8) + B;
                break;
            default:
                throw Exception.Create("Destination PixelFormat not supported");
        }

        return Result;
    }

    // GFXE.pas:8923
    public static int SaveSurfaceDescToBMP24(in TSurfaceDesc SD, TRect R, string FileName)
    {
        /* TODO: Implement the untyped file stream buffer. */
        throw new NotImplementedException();
    }

    // GFXE.pas:9517
    // { The 5 bit blend tables: Table[I, J] is the blend of source intensity I over destination
    //   intensity J (0..31). }
    public static void GenerateAlphaBlendTable(TBlendTable Table, float Alpha)
    {
        if (Table is TBlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                for (int J = 0; J <= 31; J++)
                {
                    ((TBlendTable5Bit)Table).Table[I][J] = (byte)Round((double)((double)I * Alpha) + (double)((double)1 - Alpha) * J);
                }
            }
        }
        else
        {
            throw Exception.Create("BlendTable format not supported");
        }
    }

    // GFXE.pas:9549
    public static void GenerateIntensityBlendTable(TBlendTable Table, float Intensity)
    {
        int V;
        float F;
        if (Table is TBlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                for (int J = 0; J <= 31; J++)
                {
                    if (I == 0)
                    {
                        F = 0;
                    }
                    else
                    {
                        F = (float)((double)((double)((double)I * Intensity) * 1.75) / (I + J));
                    }

                    V = Round((double)I * F) + J;
                    if (V > 31)
                    {
                        V = 31;
                    }

                    ((TBlendTable5Bit)Table).Table[I][J] = (byte)V;
                }
            }
        }
    }

    // GFXE.pas:9568
    public static void GenerateShadowBlendTable(TBlendTable Table, float Shadow)
    {
        int V;
        float F;
        if (Table is TBlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                for (int J = 0; J <= 31; J++)
                {
                    F = (float)((double)((double)(31 - I) * ((double)1 / 31)) * Shadow);
                    V = Round((double)((double)1 - F) * J);
                    if (V < 0)
                    {
                        V = 0;
                    }

                    if (V > J)
                    {
                        V = J;
                    }

                    ((TBlendTable5Bit)Table).Table[I][J] = (byte)V;
                }
            }
        }
    }

    // GFXE.pas:9585
    public static void GenerateBrightenBlendTable(TBlendTable Table, float Brighten)
    {
        int V;
        if (Table is TBlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                for (int J = 0; J <= 31; J++)
                {
                    V = Round((double)((double)I * Brighten) + (double)((double)1 - Brighten) * J);
                    if (V < J)
                    {
                        V = J;
                    }

                    ((TBlendTable5Bit)Table).Table[I][J] = (byte)V;
                }
            }
        }
    }

    // GFXE.pas:12517
    public static string GetErrorMsg(int ErrorCode)
    {
        string Result;
        switch (ErrorCode)
        {
            case GFXE_OK:
                Result = "No ERROR occurred!";
                break;
            case GFXE_ERROR:
                Result = "Undefine ERROR occurred";
                break;
            case GFXE_NOTSUPPORTED:
                Result = "Function is not supported";
                break;
            case GFXE_PIXELFORMATNOTSUPPORTED:
                Result = "PixelFormat is not supported";
                break;
            case GFXE_BITMAPFORMATNOTSUPPORTED:
                Result = "Bitmap format is not supported";
                break;
            default:
                Result = "Undocumented Error (Error Code:" + IntToStr(ErrorCode) + ")";
                break;
        }

        return Result;
    }

    // GFXE.pas:12697
    public static void ForceOptimal16BitPixelFormatID(int PixelFormatID)
    {
        if (PixelFormatID == pfRGB565 || PixelFormatID == pfRGB555)
        {
            ForcedPixelFormatID = PixelFormatID;
        }
        else
        {
            ForcedPixelFormatID = 0;
        }
    }

    // GFXE.pas:12705
    public static int GetOptimalPixelFormatID(int BitCount)
    {
        /* TODO: Implement Forms.Width in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:12874
    public static void Register()
    {
        RegisterComponents("GFX Engine", [typeof(TDrawPanel), typeof(TOpenImageFileDialog)]);
    }

    public static int ForcedPixelFormatID;
    public static TImageFileClassList ImageFileClassList;

    // GFXE.pas:8752
    // { The BMP file format: TBMPImageFile and its helpers. }
    public static int LoadBMP(string FileName, ref TSurfaceDesc SD, int X, int Y)
    {
        /* TODO: Implement the untyped file stream buffer. */
        throw new NotImplementedException();
    }

    // GFXE.pas:8968
    public static TSurface CreateSurfaceFromBMP(string FileName)
    {
        /* TODO: Implement the untyped file stream buffer. */
        throw new NotImplementedException();
    }

    // GFXE.pas:9009
    public static int GetBMPPixelFormat(string FileName)
    {
        /* TODO: Implement the untyped file stream buffer. */
        throw new NotImplementedException();
    }

    // GFXE.pas:9529
    public static void GenerateLinearAlphaBlendTable(TBlendTable Table, float Alpha)
    {
        float F;
        if (Table is TBlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                if ((double)Alpha > 0)
                {
                    F = (float)((double)I / ((double)31 * Alpha));
                }
                else
                {
                    F = 1;
                }

                if ((double)F > 1)
                {
                    F = 1;
                }

                for (int J = 0; J <= 31; J++)
                {
                    ((TBlendTable5Bit)Table).Table[I][J] = (byte)Round((double)((double)I * F) + (double)((double)1 - F) * J);
                }
            }
        }
        else
        {
            throw Exception.Create("BlendTable format not supported");
        }
    }

    // GFXE.pas:11039
    public static void InitBlendTables(int Bits, int Count)
    {
        AlphaBlendTable.GenerateLookupTables(Bits, GenerateAlphaBlendTable, Count);
        IntensityBlendTable.GenerateLookupTables(Bits, GenerateIntensityBlendTable, Count);
        BrightenBlendTable.GenerateLookupTables(Bits, GenerateBrightenBlendTable, Count);
        ShadowBlendTable.GenerateLookupTables(Bits, GenerateShadowBlendTable, Count);
        LinearAlphaBlendTable.GenerateLookupTables(Bits, GenerateLinearAlphaBlendTable, Count);
    }

    public static void Initialize()
    {
        AlphaBlendTable = new TLookupBlendTableCollection100();
        IntensityBlendTable = new TLookupBlendTableCollection100();
        BrightenBlendTable = new TLookupBlendTableCollection100();
        ShadowBlendTable = new TLookupBlendTableCollection100();
        LinearAlphaBlendTable = new TLookupBlendTableCollection100();
        InitBlendTables(5, 31);
        ForcedPixelFormatID = 0;
        ImageFileClassList = new TImageFileClassList();
        RegisterImageFileClasses([ImageFileClass.Of<TBMPImageFile>()]);
    }

    public static void Finalize()
    {
        AlphaBlendTable.Free();
        IntensityBlendTable.Free();
        BrightenBlendTable.Free();
        ShadowBlendTable.Free();
        LinearAlphaBlendTable.Free();
        ImageFileClassList.Free();
    }
}
