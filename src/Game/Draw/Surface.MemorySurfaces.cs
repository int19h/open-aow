
using Aow.Graphics;
using Aow.Rtl;

namespace Aow.Game.Draw;

public partial class Surface
{
    public virtual int BltDC(int DC, Rect? SrcRect, Rect? DestRect) => throw new NotSupportedException("Native device context");

    public virtual int Cls(int Color)
    {
        LockedSurface sd = default;
        int result = Lock(ref sd);
        if (result < 0) return result;
        try
        {
            int pixel = PixelFormats.Color2PixelInfo(Color, sd.PixelFormatID, sd.Palette);
            for (int y = 0; y < sd.Height; y++)
            {
                var row = sd.Data.AsSpan(sd.DataIndex + y * sd.Pitch);
                switch (sd.BitCount)
                {
                    case 8: row[..sd.Width].Fill((byte)pixel); break;
                    case 16: Blit.FillWord(row, sd.Width, pixel); break;
                    case 24: if (sd.Width > 0) Blit.FillTrippleByte(row, sd.Width, pixel); break;
                    case 32: Blit.FillInt(row, sd.Width, pixel); break;
                    default: return PixelFormats.GFXE_NOTSUPPORTED;
                }
            }
            return PixelFormats.GFXE_OK;
        }
        finally { Unlock(); }
    }

}
