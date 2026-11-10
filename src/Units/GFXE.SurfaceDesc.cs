using Aow.Graphics;
using Aow.Rtl;

namespace Aow.Game.Draw;

// GFXE.pas: the description of a locked surface (graphics.md 3.1). The original record is 60
// bytes and its assembler readers use the field offsets. No reader in the port sees it as bytes,
// so it is a plain struct. The original's `Data: Pointer` is two fields: Data, the block of memory
// that holds the surface's rows, and DataIndex, the byte index of the first byte of row 0 in it.
// `Pointer(Integer(SD.Data) + n)` is DataIndex + n, and `SD.Data = nil` is a null Data.
public struct TSurfaceDesc
{
    public int Width;
    public int Height;
    public int BitCount;
    public int Pitch;
    public TRect ClipRect;
    public byte[] Data;
    public int DataIndex;
    public TGFXEPalette Palette;
    public int PixelFormatID;
    public int RMask;
    public int GMask;
    public int BMask;
    public int AMask;

    // The view of the surface's rows for the blitters, moved by a byte offset from the first
    // pixel of row 0 (`Pointer(Integer(SD.Data) + Offset)` with `SD.Pitch` in the original), with
    // the descriptor's clip rectangle.
    public readonly SurfaceView View(int offset = 0) => new(Data, DataIndex + offset, Pitch, ClipRect);
}
