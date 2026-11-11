// Translated from DisplayC.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// the full screen window of the DirectDraw controllers. passes the mouse and key events on to
// its display. FDisplay
public partial class DirectDrawFullScreenWindowControl : WinControl
{
    internal CustomDisplay FDisplay;

    // DisplayC.pas:1129
    protected internal override void MouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        FDisplay.MouseDown(Button, Shift, X, Y);
    }

    // DisplayC.pas:1134
    protected internal override void MouseMove(Set<ShiftState> Shift, int X, int Y)
    {
        /* TODO: Implement Controls.MouseMove in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1139
    protected internal override void MouseUp(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        /* TODO: Implement Controls.MouseUp in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1159
    protected internal override void DblClick()
    {
        /* TODO: Implement Controls.DblClick in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1164
    protected internal override void Click()
    {
        /* TODO: Implement Controls.Click in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1144
    protected internal override void KeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        FDisplay.KeyDown(ref Key, Shift);
    }

    // DisplayC.pas:1149
    protected internal override void KeyUp(ref ushort Key, Set<ShiftState> Shift)
    {
        /* TODO: Implement Controls.KeyUp in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1154
    protected internal override void KeyPress(ref byte Key)
    {
        /* TODO: Implement Controls.KeyPress in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1169
    protected internal override void CreateParams(ref CreateParams Params)
    {
        /* TODO: Implement Controls.CreateParams in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1180
    protected internal override void WndProc(ref Message Message)
    {
        /* TODO: Implement Forms.Active in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1114

    public DirectDrawFullScreenWindowControl(Component owner) : base(owner)
    {
        /* TODO: Implement Controls.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1121
    public override void Dispose()
    {
        /* TODO: Implement Controls.Handle in the platform or editor service. */
        throw new NotImplementedException();
    }
}
