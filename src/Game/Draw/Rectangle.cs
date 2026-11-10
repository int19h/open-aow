// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// Rect
public partial class Rectangle
{
    public Rect Rect;

    // GFXE.pas:2432
    public Rectangle(int ALeft, int ATop, int ARight, int ABottom)
    {
        Rect.Left = ALeft;
        Rect.Top = ATop;
        Rect.Right = ARight;
        Rect.Bottom = ABottom;
    }

    // GFXE.pas:2440
    public Rectangle(in Rect R)
    {
        Rect = R;
    }
}
