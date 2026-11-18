// Translated from GFXE.pas.

namespace Aow.Game.Draw;

// PixelFormatId, Width, Height
public partial record class DisplayModeItem
{
    public PixelFormat PixelFormatId { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }
}
