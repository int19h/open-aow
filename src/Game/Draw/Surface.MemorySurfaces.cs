using Aow.Graphics;
using Aow.Rtl;

namespace Aow.Game.Draw;

public partial class Surface
{
    public virtual int BlitDisplayController(int displayController, Rect? sourceRectangle, Rect? destinationRectangle) => throw new NotSupportedException("Native device context");

    public virtual GraphicsResult Clear(Color color)
    {
        LockedSurface lockedSurface = default;
        GraphicsResult result = Lock(ref lockedSurface);
        if (result != GraphicsResult.Ok)
        {
            return result;
        }

        try
        {
            int pixel = PixelFormats.Color2PixelInfo(color, lockedSurface.PixelFormatId, lockedSurface.Palette);
            for (int y = 0; y < lockedSurface.Height; y++)
            {
                Span<byte> row = lockedSurface.Data.AsSpan(lockedSurface.DataIndex + (y * lockedSurface.Pitch));
                switch (lockedSurface.BitCount)
                {
                    case 8: row[..lockedSurface.Width].Fill((byte)pixel); break;
                    case 16: Graphics.Blit.FillWord(row, count: lockedSurface.Width, value: pixel); break;
                    case 24:
                        if (lockedSurface.Width > 0)
                        {
                            Graphics.Blit.FillTripleByte(row, count: lockedSurface.Width, value: pixel);
                        }
                        break;
                    case 32: Graphics.Blit.FillInteger(row, count: lockedSurface.Width, value: pixel); break;
                    default: return GraphicsResult.NotSupported;
                }
            }
            return GraphicsResult.Ok;
        }
        finally
        {
            _ = Unlock();
        }
    }
}
