
namespace Aow.Game.Draw;

public partial class Palette
{
    public int GetNearestColor(int color)
    {
        int r = color & 255, g = (color >> 8) & 255, b = (color >> 16) & 255, best = 0, distance = int.MaxValue;
        for (int i = 0; i < 256; i++)
        {
            var (red, green, blue) = (this[i].Red, this[i].Green, this[i].Blue);
            int next = Math.Abs(red - r) + Math.Abs(green - g) + Math.Abs(blue - b);
            if (next < distance) { distance = next; best = i; }
        }
        return best;
    }
}

public partial class Palette
{
    public void Assign(Palette source)
    {
        source._pixels.CopyTo(_pixels);
        PixelFormatId = source.PixelFormatId;
    }
}
