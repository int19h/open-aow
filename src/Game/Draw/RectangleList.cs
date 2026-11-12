// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// a TList of TRectangle, owning them
public partial class RectangleList : DelphiList
{
    // GFXE.pas:2458
    internal Rectangle GetRectangle(int index) => (Rectangle)List[index];

    // GFXE.pas:2447
    public override void Dispose()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            ((this[I] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
            Delete(I);
        }
    }

    public sealed class RectanglesAccessor(RectangleList target)
    {
        public Rectangle this[int index] { get => target.GetRectangle(index); }
    }

    public RectanglesAccessor Rectangles => new RectanglesAccessor(this);
}
