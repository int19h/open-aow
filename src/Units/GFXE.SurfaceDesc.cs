using Aow.Graphics;
using Aow.Rtl;

namespace Aow.Units.GFXE;

// GFXE.pas: the description of a locked surface (graphics.md 3.1). The original record is 60
// bytes and its assembler readers use the field offsets. No reader in the port sees it as bytes,
// so it is a plain struct: Data addresses the first byte of row 0 in the surface's memory.
public struct TSurfaceDesc
{
    public int Width;
    public int Height;
    public int BitCount;
    public int Pitch;
    public TRect ClipRect;
    public Ptr<byte> Data;
    public TGFXEPalette Palette;
    public int PixelFormatID;
    public int RMask;
    public int GMask;
    public int BMask;
    public int AMask;

    // The view of the surface's rows for the blitters, moved by a byte offset from the first
    // pixel of row 0 (`Pointer(Integer(SD.Data) + Offset)` with `SD.Pitch` in the original), with
    // the descriptor's clip rectangle.
    public readonly SurfaceView View(int offset = 0) => SurfaceView.Of(Data, Pitch, ClipRect).At(offset);
}
