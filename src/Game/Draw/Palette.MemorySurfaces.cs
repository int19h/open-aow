
namespace Aow.Game.Draw;

public partial class Palette
{
    public int GetNearestColor(int color)
    {
        int r = color & 255, g = (color >> 8) & 255, b = (color >> 16) & 255, best = 0, distance = int.MaxValue;
        for (int i = 0; i < 256; i++)
        {
            int next = Math.Abs(Entries[i].Red - r) + Math.Abs(Entries[i].Green - g) + Math.Abs(Entries[i].Blue - b);
            if (next < distance) { distance = next; best = i; }
        }
        return best;
    }
}

public partial class Palette
{
    public void Assign(Palette source)
    {
        Array.Copy(source.Entries, Entries, 256);
        PixelFormatId = source.PixelFormatId;
    }
}
