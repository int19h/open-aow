using Aow.Graphics;

namespace Aow.Game.Draw;

public sealed partial class Palette
{
    public int GetNearestColor(Color color)
    {
        int r = color.ToPacked() & 0xFF, g = (color.ToPacked() >> 8) & 0xFF, b = (color.ToPacked() >> 16) & 0xFF, best = 0, distance = int.MaxValue;
        for (int i = 0; i < 256; i++)
        {
            (byte red, byte green, byte blue) = (this[i].Red, this[i].Green, this[i].Blue);
            int next = Math.Abs(red - r) + Math.Abs(green - g) + Math.Abs(blue - b);
            if (next < distance)
            {
                distance = next;
                best = i;
            }
        }
        return best;
    }
}

public sealed partial class Palette
{
    public void Assign(Palette source)
    {
        source._pixels.CopyTo(_pixels);
        PixelFormatId = source.PixelFormatId;
    }
}
