// Translated from GFXE.pas.
using Aow.Engine.Buffers;

using Aow.Rtl;

namespace Aow.Game.Draw;

// a 16 bit DIB section (555 or 565). _handle, _width, _height, _bits, _pitch, _pixelFormatId,
// Info, Masks
public partial class Dib16Surface : Surface
{
    internal int _handle;
    private int _width;
    private int _height;
    private byte[] _bits;
    private int _pitch;
    private int _pixelFormatId;

    internal BitmapInfoHeader Info { get; }
    internal Int32Array3 Masks { get; }

    // GFXE.pas:6991
    public override int GetWidth() => _width;

    // GFXE.pas:6996
    public override int GetHeight() => _height;

    // GFXE.pas:7001
    public override int GetPixelFormatId() => _pixelFormatId;

    public new int PixelFormatId { get => _pixelFormatId; set => SetPixelFormatId(value); }

}
