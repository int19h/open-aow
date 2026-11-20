#nullable disable
// Translated from GFXE.pas.
using Aow.Graphics;
using Aow.Rtl;

namespace Aow.Game.Draw;

// a surface in memory. _width, _height, _memory, FData (_memory aligned to 8), _bitCount (Byte),
// FPitch, FPixelFormatID, FX, FY (origin of the captured rectangle), FPalette. The port's FMem
// is the pixel array and its first byte is the first pixel, so FData has no field of its own
// (graphics.md 3.4).
public partial class DataSurface : Surface
{

    private int _width;
    private int _height;
    private byte _bitCount;
    private PixelFormat _pixelFormatId;

    internal int X { get; private set; }
    internal int Y { get; private set; }

    private Palette _palette;

    // GFXE.pas:12387
    public DataSurface(int width, int height, PixelFormat pixelFormatId, Palette palette)
    {
        _bitCount = (byte)pixelFormatId.BitCount();
        _pixelFormatId = pixelFormatId;
        SetPalette(palette);
        _ = Resize(width, height);
    }

    // GFXE.pas:12396
    public DataSurface(Surface surface, Rect? R) => CaptureSurface(surface, R);

    // GFXE.pas:12402
    public override void Dispose()
    {
        _palette = null;
        Data = null;
        base.Dispose();
    }

    // GFXE.pas:12410
    public override PixelFormat GetPixelFormatId() => _pixelFormatId;

    // GFXE.pas:12415
    public override int Width => _width;

    // GFXE.pas:12420
    public override int Height => _height;

    // GFXE.pas:12441
    public override GraphicsResult Unlock() => GraphicsResult.Ok;

    // GFXE.pas:12446
    public GraphicsResult CaptureSurface(Surface surface, Rect? R)
    {
        LockedSurface lockedSurface = default;
        Rect rr = default;
        GraphicsResult result = surface.Lock(ref lockedSurface);
        if (result.IsSuccess())
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

                int w = R.Value.Right - R.Value.Left;
                int h = R.Value.Bottom - R.Value.Top;
                if (w > 0 && h > 0)
                {
                    _ = Resize(w, h);
                    SetPalette(lockedSurface.Palette);
                    X = R.Value.Left;
                    Y = R.Value.Top;
                    BlitRects.BlitRectangle(lockedSurface.View((Y * lockedSurface.Pitch) + ((X * _bitCount) >>> 3)), new SurfaceView(Data, 0, Pitch), (_width * _bitCount) >>> 3, _height);
                }
            }
            catch
            {
                X = 30;
            }

            _ = surface.Unlock();
            return GraphicsResult.Ok;
        }

        return result;
    }

    // GFXE.pas:12485
    public void SetPalette(Palette palette)
    {
        if (palette is not null)
        {
            _palette ??= new Palette();

            _palette.Assign(palette);
        }
        else
        {
            _palette = null;
        }
    }

    // GFXE.pas:12499
    public override GraphicsResult Resize(int width, int height)
    {
        if (width != _width || height != _height)
        {
            _width = width;
            _height = height;
            Pitch = ((((_bitCount + 7) >>> 3) * _width) + 7) & ~7;
            // The original allocates 7 bytes more and aligns FData to 8 for MMX. The alignment has
            // no effect on pixels, so the port's data is the array from its first byte. The 7
            // bytes stay after the last row: the 24-bit pixel readers read 4 bytes at the last
            // pixel, which the original's slack absorbs.
            int size = (Pitch * _height) + 7;
            Data = MemorySurfaces.Reallocate(Data, size);
            return GraphicsResult.Ok;
        }
        else
        {
            return GraphicsResult.Ok;
        }
    }

    public byte[] Data { get; private set; }
    public int Pitch { get; private set; }
    public Palette Palette { get => _palette; set => SetPalette(value); }
}
