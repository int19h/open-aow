// Translated from GFXE.pas.
using Aow.Engine.Buffers;

using Aow.Rtl;

namespace Aow.Game.Draw;

// a 16 bit DIB section (555 or 565). _handle, _width, _height, _bits, _pitch, _pixelFormatId,
// _info, _masks
public partial class Dib16Surface : Surface
{
    internal int _handle;
    internal int _width;
    internal int _height;
    internal byte[] _bits;
    internal int _pitch;
    internal int _pixelFormatId;
    internal BitmapInfoHeader _info;
    internal Int32Array3 _masks;

    // GFXE.pas:6991
    public override int GetWidth() => _width;

    // GFXE.pas:6996
    public override int GetHeight() => _height;

    // GFXE.pas:7001
    public override int GetPixelFormatId() => _pixelFormatId;

    public new int PixelFormatId { get => _pixelFormatId; set => SetPixelFormatId(value); }

}
