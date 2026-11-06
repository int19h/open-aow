using Aow.Rtl;

namespace Aow.Graphics;

public static partial class Blit
{
    public static void DrawLine16(int x1, int y1, int x2, int y2, int skip, ushort color, in TSurfaceDesc sd)
    {
        if (x2 < x1) { (x1, x2) = (x2, x1); (y1, y2) = (y2, y1); }
        int dx = x2 - x1, dy = Math.Abs(y2 - y1), sign = y2 >= y1 ? 1 : -1;
        bool steep = dy >= dx;
        int major = steep ? dy : dx, minor = steep ? dx : dy, error = 2 * minor - major, count = 0, x = x1, y = y1;
        var data = RtlUnit.BytePointer(sd.Data);
        for (int step = 0; step <= major; step++)
        {
            if (count == 0)
            {
                if (Visible(x, y, sd)) Word(data, y * sd.Pitch + x * 2, color);
                count = skip;
            }
            else count--;
            if (error >= 0) { if (steep) x++; else y += sign; error += 2 * (minor - major); }
            else error += 2 * minor;
            if (steep) y += sign; else x++;
        }
    }
}
