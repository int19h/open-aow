// Translated from GFXE.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Game.Engine.Buffers;
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

public enum DDrawSurfaceStatusElement : byte
{
    VideoMemory = 0,
    Locked = 1,
    Primary = 2,
    Created = 3,
    Complex = 4
}

public enum DDSCreationFlagsElement : byte
{
    VideoMemory = 0,
    SystemMemory = 1
}

public partial class GFXEPalette
{
    public int PixelFormatID;
    public PaletteEntry[] Entries = new PaletteEntry[256];

    // GFXE.pas:11050
    public void LoadFromStream(Stream Stream)
    {
        Stream.ReadBytes(Bytes.Of(ref PixelFormatID, 4));
        Stream.ReadBytes(MemoryMarshal.AsBytes((Span<PaletteEntry>)Entries).Slice(0, 1024));
    }

    // GFXE.pas:11056
    public void SaveToStream(Stream Stream)
    {
        Stream.Write(Bytes.Of(ref PixelFormatID, 4));
        Stream.Write(MemoryMarshal.AsBytes((ReadOnlySpan<PaletteEntry>)Entries).Slice(0, 1024));
    }

    // GFXE.pas:11068
    // The original returns a pointer to the entry, a reference here.
    public ref PaletteEntry GetEntry(int Index) => ref Entries[Index];

    // GFXE.pas:11073
    public int Remap(int NewPixelFormatID)
    {
        int Result = PixelFormats.GFXE_OK;
        if (NewPixelFormatID != PixelFormatID)
        {
            switch (PixelFormatID)
            {
                case PixelFormats.pfBGR24:
                    switch (NewPixelFormatID)
                    {
                        case PixelFormats.pfRGB565:
                            {
                                for (int I = 0; I <= 255; I++)
                                {
                                    ref PaletteEntry paletteEntry = ref Entries[I];
                                    paletteEntry.W = (ushort)(((paletteEntry.R & 248) << 8) + ((paletteEntry.G & 252) << 3) + (int)(((uint)paletteEntry.B) >> 3));
                                }
                            }

                            break;
                        case PixelFormats.pfRGB555:
                            {
                                for (int I = 0; I <= 255; I++)
                                {
                                    ref PaletteEntry paletteEntry = ref Entries[I];
                                    paletteEntry.W = (ushort)(((paletteEntry.R & 248) << 7) + ((paletteEntry.G & 248) << 2) + (int)(((uint)paletteEntry.B) >> 3));
                                }
                            }

                            break;
                        case PixelFormats.pfXRGB32:
                            Result = PixelFormats.GFXE_OK;
                            break;
                        default:
                            Result = PixelFormats.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case PixelFormats.pfRGB24:
                    switch (NewPixelFormatID)
                    {
                        case PixelFormats.pfRGB565:
                            {
                                for (int I = 0; I <= 255; I++)
                                {
                                    ref PaletteEntry paletteEntry = ref Entries[I];
                                    paletteEntry.W = (ushort)(((paletteEntry.B & 248) << 8) + ((paletteEntry.G & 252) << 3) + (int)(((uint)paletteEntry.R) >> 3));
                                }
                            }

                            break;
                        case PixelFormats.pfRGB555:
                            {
                                for (int I = 0; I <= 255; I++)
                                {
                                    ref PaletteEntry paletteEntry = ref Entries[I];
                                    paletteEntry.W = (ushort)(((paletteEntry.B & 248) << 7) + ((paletteEntry.G & 248) << 2) + (int)(((uint)paletteEntry.R) >> 3));
                                }
                            }

                            break;
                        case PixelFormats.pfXRGB32:
                            Result = PixelFormats.GFXE_OK;
                            break;
                        default:
                            Result = PixelFormats.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                default:
                    Result = PixelFormats.GFXE_NOTSUPPORTED;
                    break;
            }

            if (Succeeded(Result))
            {
                PixelFormatID = NewPixelFormatID;
            }
        }
        else
        {
            Result = PixelFormats.GFXE_OK;
        }

        return Result;
    }

    // GFXE.pas:11140
    public bool Compare(GFXEPalette Palette)
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
                    Result = CompareMem(MemoryMarshal.AsBytes((ReadOnlySpan<PaletteEntry>)Entries).Slice(0, 1024), MemoryMarshal.AsBytes((ReadOnlySpan<PaletteEntry>)Palette.Entries).Slice(0, 1024), 1024);
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

    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct PaletteEntry
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
}

// the abstract surface. GetWidth and GetHeight are abstract
public partial class Surface : IDisposable
{
    // GFXE.pas:11964
    public virtual int GetColor(int X, int Y)
    {
        int Result = default;
        SurfaceDesc SD = default;
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
                    case PixelFormats.pfRGB555:
                        W = SD.View().Word(X + X + Y * SD.Pitch);
                        Result = (int)(((uint)(W & 31744)) >> 7) + ((W & 992) << 6) + ((W & 31) << 19);
                        break;
                    case PixelFormats.pfRGB565:
                        W = SD.View().Word(X + X + Y * SD.Pitch);
                        Result = (int)(((uint)(W & 63488)) >> 8) + ((W & 2016) << 5) + ((W & 31) << 19);
                        break;
                    case PixelFormats.pfPAL8:
                        Assert(SD.Palette != null, "Palette not found, cannot retrieve color (GetColor)", "GFXE.pas", 11986);
                        Index = SD.View().Byte(X + Y * SD.Pitch);
                        ref readonly GFXEPalette.PaletteEntry E = ref SD.Palette.Entries[Index];
                        Result = E.R + (E.G << 8) + (E.B << 16);
                        break;
                    case PixelFormats.pfRGB24:
                        var P = SD.View(X + X + X + Y * SD.Pitch);
                        Result = P[2] + (P[1] << 8) + (P[0] << 16);
                        break;
                    case PixelFormats.pfRGB32:
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
        SurfaceDesc SD = default;
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
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GFXE.pas:399
    public virtual int GetHeight()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GFXE.pas:11954
    public virtual int GetPixelFormatID()
    {
        int Result;
        SurfaceDesc SD = default;
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
    public virtual int Blt(Surface Dest, Rect? SrcRect, int X, int Y)
    {
        int Result;
        SurfaceDesc DSD = default;
        SurfaceDesc SSD = default;
        Rect R = default;
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
                    BlitRects.BltRect(S, D, (SrcRect.Value.Right - SrcRect.Value.Left) * DSD.BitCount / 8, SrcRect.Value.Bottom - SrcRect.Value.Top);
                    Unlock();
                    Result = PixelFormats.GFXE_OK;
                }
                else
                {
                    Result = GFXE_CANNOTLOCK;
                }

                Dest.Unlock();
            }
            else
            {
                Result = GFXE_CANNOTLOCK;
            }
        }
        else
        {
            Result = PixelFormats.GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:12045
    public virtual int BltClipped(Surface Dest, Rect? SrcRect, int X, int Y)
    {
        int Result;
        int W;
        int H;
        Rect R = default;
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
            Result = PixelFormats.GFXE_OK;
        }

        return Result;
    }

    // GFXE.pas:12130
    public virtual int BltColorKey(Surface Dest, Rect? SrcRect, int X, int Y, int ColorKey)
    {
        int Result;
        SurfaceDesc DSD = default;
        SurfaceDesc SSD = default;
        Rect R = default;
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
                    BlitRects.BltColorKey16(S, D, SrcRect.Value.Right - SrcRect.Value.Left, SrcRect.Value.Bottom - SrcRect.Value.Top, ColorKey);
                    Result = PixelFormats.GFXE_OK;
                    Unlock();
                }
                else
                {
                    Result = GFXE_CANNOTLOCK;
                }

                Dest.Unlock();
            }
            else
            {
                Result = GFXE_CANNOTLOCK;
            }
        }
        else
        {
            Result = PixelFormats.GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:12168
    public virtual int BltColorKeyClipped(Surface Dest, Rect? SrcRect, int X, int Y, int ColorKey)
    {
        int Result;
        int W;
        int H;
        Rect R = default;
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
            Result = PixelFormats.GFXE_OK;
        }

        return Result;
    }

    // GFXE.pas:12102
    public virtual int BltStretch(Surface Dest, Rect? SrcRect, Rect? DestRect)
    {
        int Result;
        Rect R = default;
        Rect DR = default;
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
            Result = PixelFormats.GFXE_NOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:12225
    public virtual int Lock(ref SurfaceDesc SD) => PixelFormats.GFXE_NOTSUPPORTED;

    // GFXE.pas:12248
    public virtual int Unlock() => PixelFormats.GFXE_NOTSUPPORTED;

    // GFXE.pas:12230
    public virtual int LockEx(ref SurfaceDesc SD, in Rect R)
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
            Result = PixelFormats.GFXE_ERROR;
        }

        return Result;
    }

    // GFXE.pas:12253
    public virtual int UnlockEx(in SurfaceDesc SD) => Unlock();

    // GFXE.pas:12220
    public virtual int UpdatePalette() => PixelFormats.GFXE_NOTSUPPORTED;

    // GFXE.pas:12258
    public virtual int Resize(int AWidth, int AHeight) => PixelFormats.GFXE_NOTSUPPORTED;

    public int Width { get => GetWidth(); }
    public int Height { get => GetHeight(); }
    public int PixelFormatID { get => GetPixelFormatID(); }

    public virtual void Dispose()
    {
    }

    private const int GFXE_CANNOTLOCK = -2147483643;
}

// a surface in memory. FWidth, FHeight, FMem, FData (FMem aligned to 8), FBitCount (Byte),
// FPitch, FPixelFormatID, FX, FY (origin of the captured rectangle), FPalette. The port's FMem
// is the pixel array and its first byte is the first pixel, so FData has no field of its own
// (graphics.md 3.4).
public partial class DataSurface : Surface
{

    internal int FWidth;
    internal int FHeight;
    internal byte[] FMem;
    internal byte FBitCount;
    internal int FPitch;
    internal int FPixelFormatID;
    internal int FX;
    internal int FY;
    internal GFXEPalette FPalette;

    // GFXE.pas:12387
    public DataSurface(int AWidth, int AHeight, int APixelFormatID, GFXEPalette APalette)
    {
        FBitCount = (byte)(APixelFormatID & 255);
        FPixelFormatID = APixelFormatID;
        SetPalette(APalette);
        Resize(AWidth, AHeight);
    }

    // GFXE.pas:12396
    public DataSurface(Surface Surface, Rect? R)
    {
        CaptureSurface(Surface, R);
    }

    // GFXE.pas:12402
    public override void Dispose()
    {
        FPalette = null;
        FMem = null;
        base.Dispose();
    }

    // GFXE.pas:12410
    public override int GetPixelFormatID() => FPixelFormatID;

    // GFXE.pas:12415
    public override int GetWidth() => FWidth;

    // GFXE.pas:12420
    public override int GetHeight() => FHeight;

    // GFXE.pas:12441
    public override int Unlock() => PixelFormats.GFXE_OK;

    // GFXE.pas:12446
    public int CaptureSurface(Surface Surface, Rect? R)
    {
        SurfaceDesc SD = default;
        Rect RR = default;
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
                    BlitRects.BltRect(SD.View(FY * SD.Pitch + (int)(((uint)(FX * FBitCount)) >> 3)), new SurfaceView(FMem, 0, FPitch), (int)(((uint)(FWidth * FBitCount)) >> 3), FHeight);
                }
            }
            catch
            {
                FX = 30;
            }

            Surface.Unlock();
            Result = PixelFormats.GFXE_OK;
        }

        return Result;
    }

    // GFXE.pas:12485
    public void SetPalette(GFXEPalette Palette)
    {
        if (Palette != null)
        {
            if (FPalette == null)
            {
                FPalette = new GFXEPalette();
            }

            FPalette.Assign(Palette);
        }
        else
        {
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
            Result = PixelFormats.GFXE_OK;
        }
        else
        {
            Result = PixelFormats.GFXE_OK;
        }

        return Result;
    }

    public byte[] Data { get => FMem; }
    public int Pitch { get => FPitch; }
    public GFXEPalette Palette { get => FPalette; set => SetPalette(value); }

}

// a DIB section of any format (8, 16 or 24 bit). FHandle, FWidth, FHeight, FMem, FBits, FPitch,
// FPixelFormatID, FInfo (BITMAPINFOHEADER and 256 colors), FPalette
public partial class DIBSurface : Surface
{
    internal int FHandle;
    internal int FWidth;
    internal int FHeight;
    internal byte[] FMem;
    internal byte[] FBits;
    internal int FPitch;
    internal int FPixelFormatID;
    internal BitmapInfoHeader FInfo;
    internal FixedArray_Aow_Rtl_TRGBQuad_256 FColors;
    internal GFXEPalette FPalette;

    // GFXE.pas:6407
    public override int GetWidth() => FWidth;

    // GFXE.pas:6412
    public override int GetHeight() => FHeight;

    // GFXE.pas:6767
    public void SaveToBMP24(string FileName)
    {
        SurfaceDesc SD = default;
        Lock(ref SD);
        SaveSurfaceDescToBMP24(SD, SD.ClipRect, FileName);
        Unlock();
    }

    public byte[] Bits { get => FBits; }
    public int Pitch { get => FPitch; }
    public GFXEPalette Palette { get => FPalette; }

    private static int SaveSurfaceDescToBMP24(in SurfaceDesc SD, Rect R, string FileName)
    {
        throw new NotImplementedException();
    }
}

// an 8 bit bitmap in memory (bottom-up: the pitch is negative). FWidth, FHeight, FMem, FBits,
// FPitch, FInfo (header and 256 colors), FPalette
public partial class DIB08Surface : Surface
{
    internal int FWidth;
    internal int FHeight;
    internal byte[] FMem;
    internal byte[] FBits;
    internal int FPitch;
    internal BitmapInfoHeader FInfo;
    internal FixedArray_Aow_Rtl_TRGBQuad_256 FColors;
    internal GFXEPalette FPalette;

    // GFXE.pas:6805
    public override int GetPixelFormatID() => PixelFormats.pfPAL8;

    // GFXE.pas:6810
    public override int GetWidth() => FWidth;

    // GFXE.pas:6815
    public override int GetHeight() => FHeight;

    public GFXEPalette Palette { get => FPalette; set => SetPalette(value); }

}

// a 16 bit DIB section (555 or 565). FHandle, FWidth, FHeight, FBits, FPitch, FPixelFormatID,
// FInfo, FMasks
public partial class DIB16Surface : Surface
{
    internal int FHandle;
    internal int FWidth;
    internal int FHeight;
    internal byte[] FBits;
    internal int FPitch;
    internal int FPixelFormatID;
    internal BitmapInfoHeader FInfo;
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
public partial class DIB24Surface : Surface
{
    internal int FHandle;
    internal int FWidth;
    internal int FHeight;
    internal byte[] FBits;
    internal int FPitch;
    internal BitmapInfoHeader FInfo;

    // GFXE.pas:7238
    public override int GetWidth() => FWidth;

    // GFXE.pas:7243
    public override int GetHeight() => FHeight;

}

// a 32 bit bitmap in memory (bottom-up). FWidth, FHeight, FMem, FBits, FPitch, FInfo
public partial class DIB32Surface : Surface
{
    internal int FWidth;
    internal int FHeight;
    internal byte[] FMem;
    internal byte[] FBits;
    internal int FPitch;
    internal BitmapInfoHeader FInfo;

    // GFXE.pas:7446
    public override int GetWidth() => FWidth;

    // GFXE.pas:7451
    public override int GetHeight() => FHeight;

}

// a window drawn through GDI. FHandle. Two more virtuals: GetHandle, SetHandle
public partial class GDISurface : Surface
{

    internal int FHandle;

    // GFXE.pas:12293
    public GDISurface(int AHandle)
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
    public override int Blt(Surface Dest, Rect? SrcRect, int X, int Y) => PixelFormats.GFXE_NOTSUPPORTED;

    // GFXE.pas:12334
    public override int BltDC(int DC, Rect? SrcRect, Rect? DestRect)
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
public partial class DDrawSurface : Surface
{
    protected internal NotifyEvent<object> FOnSurfaceLost;
    protected internal int FPixelFormatID;
    protected internal int FWidth;
    protected internal int FHeight;
    protected internal Set<DDSCreationFlagsElement> FCreationFlags;
    protected internal Set<DDrawSurfaceStatusElement> FStatus;
    protected internal int FLockCount;
    protected internal int FLockExCount;
    protected internal SurfaceDesc FSurfaceDesc;

    // GFXE.pas:9279
    public override int GetPixelFormatID() => FPixelFormatID;

    // GFXE.pas:9284
    public override int GetWidth() => FWidth;

    // GFXE.pas:9289
    public override int GetHeight() => FHeight;

    public Set<DDrawSurfaceStatusElement> Status { get => FStatus; }
    public Set<DDSCreationFlagsElement> CreationFlags { get => FCreationFlags; }
    public NotifyEvent<object> OnSurfaceLost { get => FOnSurfaceLost; set => FOnSurfaceLost = value; }

    public const DDrawSurfaceStatusElement ssSystemMemory = DDrawSurfaceStatusElement.Created;
}

// Rect
public partial class Rectangle
{
    public Rect Rect;

    // GFXE.pas:2432
    public Rectangle(int ALeft, int ATop, int ARight, int ABottom)
    {
        Rect.Left = ALeft;
        Rect.Top = ATop;
        Rect.Right = ARight;
        Rect.Bottom = ABottom;
    }

    // GFXE.pas:2440
    public Rectangle(in Rect R)
    {
        Rect = R;
    }
}

// a TList of TRectangle, owning them
public partial class RectangleList : DelphiList
{
    // GFXE.pas:2458
    internal Rectangle GetRectangle(int Index) => (Rectangle)List[Index];

    // GFXE.pas:2447
    public override void Dispose()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            ((this[I] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
            Delete(I);
        }
    }

    public sealed class RectanglesAccessor(RectangleList target)
    {
        public Rectangle this[int Index] { get => target.GetRectangle(Index); }
    }

    public RectanglesAccessor Rectangles => new RectanglesAccessor(this);
}

// a panel that paints through OnDraw. FEraseBkgndEnabled, FOnDraw
public partial class DrawPanel : CustomPanel
{
    internal bool FEraseBkgndEnabled;
    internal OnDrawRectEvent FOnDraw;

    // GFXE.pas:8730
    protected internal override void WndProc(ref Message Message)
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

    public DrawPanel(Component owner) : base(owner)
    {
        /* TODO: Implement ExtCtrls.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    [Published]
    public OnDrawRectEvent OnDraw { get => FOnDraw; set => FOnDraw = value; }

    [Published]
    public bool EraseBkgndEnabled { get => FEraseBkgndEnabled; set => FEraseBkgndEnabled = value; }

    public delegate void OnDrawRectEvent(object Sender, in Rect R);
}

// an image file format, all class functions
public partial class ImageFile : IImageFileClass
{
    // GFXE.pas:693
    public static string Name => throw AbstractErrorException.Create("Abstract Error");

    // GFXE.pas:694
    public static string Extension => throw AbstractErrorException.Create("Abstract Error");

    // GFXE.pas:695
    public static string Description => throw AbstractErrorException.Create("Abstract Error");

    // GFXE.pas:696
    public static int GetPixelFormatID(string FileName)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GFXE.pas:697
    public static Surface CreateSurface(string FileName)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GFXE.pas:698
    public static int LoadToSurfaceDesc(string FileName, ref SurfaceDesc SD, int X, int Y)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

}

// a TOpenDialog whose filter lists the registered image file classes
public partial class OpenImageFileDialog : OpenDialog
{
    // GFXE.pas:972
    public override void Loaded()
    {
        /* TODO: Implement Dialogs.Filter in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:966

    public OpenImageFileDialog(Component owner) : base(owner)
    {
        /* TODO: Implement Dialogs.Create in the platform or editor service. */
        throw new NotImplementedException();
    }
}

// PixelFormatID, Width, Height
public partial class DisplayModeItem
{
    public int PixelFormatID;
    public int Width;
    public int Height;
}

// FList
public partial class DisplayModeList : IDisposable
{
    internal DelphiList FList;

    // GFXE.pas:12629
    internal DisplayModeItem GetItems(int Index) => (DisplayModeItem)FList.List[Index];

    // GFXE.pas:12634
    internal int GetCount() => FList.Count;

    // GFXE.pas:12611
    public DisplayModeList()
    {
        FList = new DelphiList();
    }

    // GFXE.pas:12617
    public virtual void Dispose()
    {
        Clear();
        FList?.Dispose();
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
        DisplayModeItem Item;
        Item = new DisplayModeItem();
        Item.Width = AWidth;
        Item.Height = AHeight;
        Item.PixelFormatID = APixelFormatID;
        FList.Add(Item);
    }

    // GFXE.pas:12650
    public void Delete(int Index)
    {
        FList.Delete(Index);
    }

    public sealed class ItemsAccessor(DisplayModeList target)
    {
        public DisplayModeItem this[int Index] { get => target.GetItems(Index); }
    }

    public ItemsAccessor Items => new ItemsAccessor(this);
    public int Count { get => GetCount(); }

}

// FBits, FTable (the bytes of the table, which the original addresses with a pointer into the
// derived class's own field)
public partial class BlendTable
{
    internal int FBits;
    internal byte[] FTable;

    // GFXE.pas:741
    public virtual BlendTable Copy()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GFXE.pas:742
    public virtual void Assign(BlendTable Source)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GFXE.pas:9601
    public void Initialize(int ABits, byte[] ATable)
    {
        FTable = ATable;
        FBits = ABits;
    }

    public int Bits { get => FBits; }
    public byte[] Table { get => FTable; }

    internal static void GenerateAlphaBlendTable(BlendTable Table, float Alpha)
    {
        if (Table is BlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                for (int J = 0; J <= 31; J++)
                {
                    ((BlendTable5Bit)Table).Table[I][J] = (byte)Round((double)((double)I * Alpha) + (double)((double)1 - Alpha) * J);
                }
            }
        }
        else
        {
            throw new NotSupportedException("BlendTable format not supported");
        }
    }

    internal static void GenerateIntensityBlendTable(BlendTable Table, float Intensity)
    {
        int V;
        float F;
        if (Table is BlendTable5Bit)
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

                    ((BlendTable5Bit)Table).Table[I][J] = (byte)V;
                }
            }
        }
    }

    internal static void GenerateShadowBlendTable(BlendTable Table, float Shadow)
    {
        int V;
        float F;
        if (Table is BlendTable5Bit)
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

                    ((BlendTable5Bit)Table).Table[I][J] = (byte)V;
                }
            }
        }
    }

    internal static void GenerateBrightenBlendTable(BlendTable Table, float Brighten)
    {
        int V;
        if (Table is BlendTable5Bit)
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

                    ((BlendTable5Bit)Table).Table[I][J] = (byte)V;
                }
            }
        }
    }

    internal static void GenerateLinearAlphaBlendTable(BlendTable Table, float Alpha)
    {
        float F;
        if (Table is BlendTable5Bit)
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
                    ((BlendTable5Bit)Table).Table[I][J] = (byte)Round((double)((double)I * F) + (double)((double)1 - F) * J);
                }
            }
        }
        else
        {
            throw new NotSupportedException("BlendTable format not supported");
        }
    }

}

// Table (32 x 32 bytes, row I at byte I * 32)
public partial class BlendTable5Bit : BlendTable
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
    public BlendTable5Bit()
    {
        Initialize(5, new byte[32 * 32]);
    }

    // GFXE.pas:9621
    public override BlendTable Copy()
    {
        BlendTable Result = new BlendTable5Bit();
        Result.Assign(this);
        return Result;
    }

}

// FList
public partial class BlendTableList : IDisposable
{
    internal DelphiList FList;

    // GFXE.pas:9642
    internal int GetCount() => FList.Count;

    // GFXE.pas:9656
    internal BlendTable GetTables(int Index) => (BlendTable)FList.List[Index];

    // GFXE.pas:9629
    public BlendTableList()
    {
        FList = new DelphiList();
    }

    // GFXE.pas:9635
    public virtual void Dispose()
    {
        Clear();
        FList?.Dispose();
    }

    // GFXE.pas:9647
    public void Clear()
    {
        while (FList.Count != 0)
        {
            FList.Delete(FList.Count - 1);
        }
    }

    // GFXE.pas:9661
    public void Add(BlendTable Table)
    {
        FList.Add(Table);
    }

    // GFXE.pas:9666
    public void Delete(int Index)
    {
        FList.Delete(Index);
    }

    public int Count { get => GetCount(); }

    public sealed class TablesAccessor(BlendTableList target)
    {
        public BlendTable this[int Index] { get => target.GetTables(Index); }
    }

    public TablesAccessor Tables => new TablesAccessor(this);

}

// FTables, Lookup (101 tables, indexed by percentage)
public partial class LookupBlendTableCollection100 : IDisposable
{
    internal BlendTableList FTables;
    // { ImageLib's blitters read Lookup directly; adding lines here moves the Asserts below }
    public FixedArray_Aow_Units_GFXE_TBlendTable_101 Lookup;

    // GFXE.pas:9687
    public BlendTable GetLookupTables(int Index) => Lookup[Index];

    // GFXE.pas:9674
    public LookupBlendTableCollection100()
    {
        FTables = new BlendTableList();
    }

    // GFXE.pas:9680
    public virtual void Dispose()
    {
        Clear();
        FTables?.Dispose();
    }

    // GFXE.pas:9692
    public void Clear()
    {
        FTables.Clear();
        Lookup = default;
    }

    // GFXE.pas:9698
    public bool GenerateLookupTables(int Bits, GenerateBlendTableProc Proc, int Count)
    {
        bool Result;
        BlendTable T;
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
                        T = new BlendTable5Bit();
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

    public BlendTableList Tables { get => FTables; }

    public sealed class LookupTablesAccessor(LookupBlendTableCollection100 target)
    {
        public BlendTable this[int Index] { get => target.GetLookupTables(Index); }
    }

    public LookupTablesAccessor LookupTables => new LookupTablesAccessor(this);

    public delegate void GenerateBlendTableProc(BlendTable Table, float Value);

}

public delegate void ProgressEvent(object Sender, int Percent, int Param);

// { The implementation list of image file classes (TList of TImageFileClass). }
public partial class ImageFileClassList : DelphiList
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

    public sealed class ClassesAccessor(ImageFileClassList target)
    {
        public ImageFileClass this[int Index] { get => target.GetClasses(Index); }
    }

    public ClassesAccessor Classes => new ClassesAccessor(this);
}

// the Windows bitmap file format
public partial class BMPImageFile : ImageFile, IImageFileClass
{
    // GFXE.pas:9043
    public static new string Extension => "BMP";

    // GFXE.pas:9048
    public static new string Description => "Bitmap files (*.bmp)";

    // GFXE.pas:9053
    public static new int GetPixelFormatID(string FileName) => GetBMPPixelFormat(FileName);

    // GFXE.pas:9058
    public static new Surface CreateSurface(string FileName) => CreateSurfaceFromBMP(FileName);

    // GFXE.pas:9063
    public static new int LoadToSurfaceDesc(string FileName, ref SurfaceDesc SD, int X, int Y) => LoadBMP(FileName, ref SD, X, Y);

    private static int LoadBMP(string FileName, ref SurfaceDesc SD, int X, int Y)
    {
        throw new NotImplementedException();
    }

    private static Surface CreateSurfaceFromBMP(string FileName)
    {
        throw new NotImplementedException();
    }

    private static int GetBMPPixelFormat(string FileName)
    {
        throw new NotImplementedException();
    }

}
