// Translated from DisplayC.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// a TGDISurface on the window of a control, taking the handle from the control. FControl
public partial class DisplayGDISurface : GDISurface
{
    internal WinControl FControl;

    // DisplayC.pas:1878
    public DisplayGDISurface(WinControl AControl) : base(0)
    {
        /* TODO: Implement Controls.Handle in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1887
    public override int GetHandle()
    {
        /* TODO: Implement Controls.Handle in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:367

}
