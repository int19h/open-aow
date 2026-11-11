
namespace Aow.Game.Draw;

public partial class Palette
{
    public int GetNearestColor(int Color)
    {
        int r = Color & 255, g = (Color >> 8) & 255, b = (Color >> 16) & 255, best = 0, distance = int.MaxValue;
        for (int i = 0; i < 256; i++)
        {
            int next = Math.Abs(Entries[i].R - r) + Math.Abs(Entries[i].G - g) + Math.Abs(Entries[i].B - b);
            if (next < distance) { distance = next; best = i; }
        }
        return best;
    }
}

public partial class Palette
{
    public void Assign(Palette Source)
    {
        Array.Copy(Source.Entries, Entries, 256);
        PixelFormatID = Source.PixelFormatID;
    }
}
