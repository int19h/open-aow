#nullable disable warnings

using Aow.Game.UI.Controls;
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

// { The AoWWinGeneric unit of aowInt.dpl: TAOWGenericWindow, the base window class that publishes
//   the window manager's settings (the ancestor of TAOWWindow, TAOWToolTip and TAOWMenu). }
// no fields of its own
public class GenericWindow(Component? owner) : BaseWindow(owner)
{

    // AoWWinGeneric.pas:39
    public override void Dispose()
    {
        (Buffer as IDisposable)?.Dispose();
        base.Dispose();
    }

    // AoWWinGeneric.pas:45
    public override void SetOwner(UIComponent? value)
    {
        base.SetOwner(value);
        // The owner is a UIComponent. This window sits in a window manager, and the cast stays.
        base.WindowManager = (WindowManager?)OwnerComponent;
    }

    // AoWWinGeneric.pas:51
    public void SetCursor(int value)
    {
        if (value != base.Cursor)
        {
            base.Cursor = value;
            WindowCursor = value;
            RaiseChangeCursor();
        }
    }

    // AoWWinGeneric.pas:62
    public void SetBuffer(bool value)
    {
        if (value != base.DoubleBuffered)
        {
            IsDrawn = false;
            base.DoubleBuffered = value;
            if (!base.DoubleBuffered)
            {
                if (BackSurface is not null)
                {
                    BackSurface = null;
                }
            }
        }
    }

    [Published]
    public override WindowManager? WindowManager { get => base.WindowManager; set => SetManager(value); }

    [Published]
    public override int Cursor { get => base.Cursor; set => SetCursor(value); }

    [Published]
    public override bool DoubleBuffered { get => base.DoubleBuffered; set => SetBuffer(value); }
}
