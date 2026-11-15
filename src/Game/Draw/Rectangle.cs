// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// Bounds
public partial class Rectangle
{
    public Rect Bounds { get; }

    // GFXE.pas:2432
    public Rectangle(int left, int top, int right, int bottom)
    {
        Rect bounds = Bounds;
        bounds.Left = left;
        Bounds = bounds;
        Rect bounds2 = Bounds;
        bounds2.Top = top;
        Bounds = bounds2;
        Rect bounds3 = Bounds;
        bounds3.Right = right;
        Bounds = bounds3;
        Rect bounds4 = Bounds;
        bounds4.Bottom = bottom;
        Bounds = bounds4;
    }

    // GFXE.pas:2440
    public Rectangle(in Rect R) => Bounds = R;
}
