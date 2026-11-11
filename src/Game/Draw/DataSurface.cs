// Translated from GFXE.pas.
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

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
    internal Palette FPalette;

    // GFXE.pas:12387
    public DataSurface(int AWidth, int AHeight, int APixelFormatID, Palette APalette)
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
        LockedSurface SD = default;
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
    public void SetPalette(Palette Palette)
    {
        if (Palette != null)
        {
            if (FPalette == null)
            {
                FPalette = new Palette();
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
    public Palette Palette { get => FPalette; set => SetPalette(value); }

}
