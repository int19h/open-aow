// Translated from DisplayC.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// the full screen window of the DirectDraw controllers. passes the mouse and key events on to
// its display. _display
public partial class DirectDrawFullScreenWindowControl : WinControl
{
    internal CustomDisplay _display;

    // DisplayC.pas:1129
    protected internal override void MouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        _display.MouseDown(button, shift, X, Y);
    }

    // DisplayC.pas:1134
    protected internal override void MouseMove(Set<ShiftState> shift, int X, int Y)
    {
        /* TODO: Implement Controls.MouseMove in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1139
    protected internal override void MouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        /* TODO: Implement Controls.MouseUp in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1159
    protected internal override void DoubleClick()
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
    protected internal override void KeyDown(ref ushort key, Set<ShiftState> shift)
    {
        _display.KeyDown(ref key, shift);
    }

    // DisplayC.pas:1149
    protected internal override void KeyUp(ref ushort key, Set<ShiftState> shift)
    {
        /* TODO: Implement Controls.KeyUp in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1154
    protected internal override void KeyPress(ref byte key)
    {
        /* TODO: Implement Controls.KeyPress in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1169
    protected internal override void CreateParams(ref CreateParams paramsValue)
    {
        /* TODO: Implement Controls.CreateParams in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1180
    protected internal override void WindowProc(ref Message message)
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
