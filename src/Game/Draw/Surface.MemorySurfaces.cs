
using Aow.Graphics;
using Aow.Rtl;

namespace Aow.Game.Draw;

public partial class Surface
{
    public virtual int BlitDisplayController(int displayController, Rect? sourceRectangle, Rect? destinationRectangle) => throw new NotSupportedException("Native device context");

    public virtual int Clear(int color)
    {
        LockedSurface lockedSurface = default;
        int result = Lock(ref lockedSurface);
        if (result < 0) return result;
        try
        {
            int pixel = PixelFormats.Color2PixelInfo(color, lockedSurface.PixelFormatId, lockedSurface.Palette);
            for (int y = 0; y < lockedSurface.Height; y++)
            {
                var row = lockedSurface.Data.AsSpan(lockedSurface.DataIndex + y * lockedSurface.Pitch);
                switch (lockedSurface.BitCount)
                {
                    case 8: row[..lockedSurface.Width].Fill((byte)pixel); break;
                    case 16: Graphics.Blit.FillWord(row, lockedSurface.Width, pixel); break;
                    case 24: if (lockedSurface.Width > 0) Graphics.Blit.FillTripleByte(row, lockedSurface.Width, pixel); break;
                    case 32: Graphics.Blit.FillInteger(row, lockedSurface.Width, pixel); break;
                    default: return PixelFormats.GFXE_NOTSUPPORTED;
                }
            }
            return PixelFormats.GFXE_OK;
        }
        finally { Unlock(); }
    }

}
