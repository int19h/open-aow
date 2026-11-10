// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// a panel that paints through OnDraw. FEraseBkgndEnabled, FOnDraw
public partial class DrawPanel : CustomPanel
{
    internal bool FEraseBkgndEnabled;
    internal OnDrawRectEvent FOnDraw;

    // GFXE.pas:8730
    protected internal override void WndProc(ref Message Message)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:8741
    protected internal override void Paint()
    {
        /* TODO: Implement Controls.ClientRect in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:8721

    public DrawPanel(Component owner) : base(owner)
    {
        /* TODO: Implement ExtCtrls.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    [Published]
    public OnDrawRectEvent OnDraw { get => FOnDraw; set => FOnDraw = value; }

    [Published]
    public bool EraseBkgndEnabled { get => FEraseBkgndEnabled; set => FEraseBkgndEnabled = value; }

    public delegate void OnDrawRectEvent(object Sender, in Rect R);
}
