// Translated from GFXE.pas.
namespace Aow.Game.Draw;

// a 16 bit DIB section (555 or 565). _handle, _width, _height, _bits, _pitch, _pixelFormatId,
// Info, Masks
public partial class Dib16Surface : Surface
{
    private int _width;
    private int _height;
    private byte[] _bits;
    private int _pitch;
    private PixelFormat _pixelFormatId;

    // GFXE.pas:6991
    public override int Width => _width;

    // GFXE.pas:6996
    public override int Height => _height;

    // GFXE.pas:7001
    public override PixelFormat GetPixelFormatId() => _pixelFormatId;

    public new PixelFormat PixelFormatId { get => _pixelFormatId; set => SetPixelFormatId(value); }
}
