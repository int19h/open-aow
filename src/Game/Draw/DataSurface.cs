// Translated from GFXE.pas.
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// a surface in memory. _width, _height, _memory, FData (_memory aligned to 8), _bitCount (Byte),
// FPitch, FPixelFormatID, FX, FY (origin of the captured rectangle), FPalette. The port's FMem
// is the pixel array and its first byte is the first pixel, so FData has no field of its own
// (graphics.md 3.4).
public partial class DataSurface : Surface
{

    private int _width;
    private int _height;
    private byte[] _memory;
    private byte _bitCount;
    private int _pitch;
    private int _pixelFormatId;

    internal int X { get; private set; }
    internal int Y { get; private set; }

    private Palette _palette;

    // GFXE.pas:12387
    public DataSurface(int width, int height, int pixelFormatId, Palette palette)
    {
        _bitCount = (byte)(pixelFormatId & 255);
        _pixelFormatId = pixelFormatId;
        SetPalette(palette);
        Resize(width, height);
    }

    // GFXE.pas:12396
    public DataSurface(Surface surface, Rect? R)
    {
        CaptureSurface(surface, R);
    }

    // GFXE.pas:12402
    public override void Dispose()
    {
        _palette = null;
        _memory = null;
        base.Dispose();
    }

    // GFXE.pas:12410
    public override int GetPixelFormatId() => _pixelFormatId;

    // GFXE.pas:12415
    public override int GetWidth() => _width;

    // GFXE.pas:12420
    public override int GetHeight() => _height;

    // GFXE.pas:12441
    public override int Unlock() => PixelFormats.GFXE_OK;

    // GFXE.pas:12446
    public int CaptureSurface(Surface surface, Rect? R)
    {
        LockedSurface lockedSurface = default;
        Rect rr = default;
        int W;
        int H;
        int result = surface.Lock(ref lockedSurface);
        if (Succeeded(result))
        {
            try
            {
                _bitCount = (byte)lockedSurface.BitCount;
                _pixelFormatId = lockedSurface.PixelFormatId;
                if (R is null)
                {
                    rr.Left = 0;
                    rr.Right = lockedSurface.Width;
                    rr.Top = 0;
                    rr.Bottom = lockedSurface.Height;
                    R = rr;
                }

                W = R.Value.Right - R.Value.Left;
                H = R.Value.Bottom - R.Value.Top;
                if (W > 0 && H > 0)
                {
                    Resize(W, H);
                    SetPalette(lockedSurface.Palette);
                    X = R.Value.Left;
                    Y = R.Value.Top;
                    BlitRects.BlitRectangle(lockedSurface.View(Y * lockedSurface.Pitch + (int)(((uint)(X * _bitCount)) >> 3)), new SurfaceView(_memory, 0, _pitch), (int)(((uint)(_width * _bitCount)) >> 3), _height);
                }
            }
            catch
            {
                X = 30;
            }

            surface.Unlock();
            result = PixelFormats.GFXE_OK;
        }

        return result;
    }

    // GFXE.pas:12485
    public void SetPalette(Palette palette)
    {
        if (palette != null)
        {
            if (_palette == null)
            {
                _palette = new Palette();
            }

            _palette.Assign(palette);
        }
        else
        {
            _palette = null;
        }
    }

    // GFXE.pas:12499
    public override int Resize(int width, int height)
    {
        int result;
        int size;
        if (width != _width || height != _height)
        {
            _width = width;
            _height = height;
            _pitch = ((int)(((uint)(_bitCount + 7)) >> 3) * _width + 7) & ~7;
            // The original allocates 7 bytes more and aligns FData to 8 for MMX. The alignment has
            // no effect on pixels, so the port's data is the array from its first byte. The 7
            // bytes stay after the last row: the 24-bit pixel readers read 4 bytes at the last
            // pixel, which the original's slack absorbs.
            size = _pitch * _height + 7;
            _memory = MemorySurfaces.Reallocate(_memory, size);
            result = PixelFormats.GFXE_OK;
        }
        else
        {
            result = PixelFormats.GFXE_OK;
        }

        return result;
    }

    public byte[] Data { get => _memory; }
    public int Pitch { get => _pitch; }
    public Palette Palette { get => _palette; set => SetPalette(value); }

}
