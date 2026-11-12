// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// Bounds
public partial class Rectangle
{
    public readonly Rect Bounds;

    // GFXE.pas:2432
    public Rectangle(int left, int top, int right, int bottom)
    {
        Bounds.Left = left;
        Bounds.Top = top;
        Bounds.Right = right;
        Bounds.Bottom = bottom;
    }

    // GFXE.pas:2440
    public Rectangle(in Rect R)
    {
        Bounds = R;
    }
}
