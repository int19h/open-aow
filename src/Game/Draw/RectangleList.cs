#nullable disable
// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// a TList of TRectangle, owning them
public class RectangleList : DelphiList<Rectangle>
{
    // GFXE.pas:2458
    internal Rectangle GetRectangle(int index) => List[index];

    // GFXE.pas:2447
    public override void Dispose()
    {
        for (int i = Count - 1; i >= 0; i--)
        {
            ((this[i] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
            Delete(i);
        }
    }

    public sealed class RectanglesAccessor(RectangleList target)
    {
        public Rectangle this[int index] => target.GetRectangle(index);
    }

    public RectanglesAccessor Rectangles => new(this);
}
