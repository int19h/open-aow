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
            RaiseChangeCursor(this);
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
                    BackSurface?.Dispose();
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

    [Published]
    public new event NotifyEvent<GenericWindow> ChangeCursor
    {
        add => ((WindowControl)this).ChangeCursor += EventSlot.Add<WindowControl, GenericWindow>(this, slot: nameof(ChangeCursor), value: value);
        remove => ((WindowControl)this).ChangeCursor -= EventSlot.Remove<WindowControl, GenericWindow>(this, slot: nameof(ChangeCursor), value: value);
    }

    [Published]
    public new event AoWOnDrawSurface<GenericWindow> Blitted
    {
        add => ((WindowControl)this).Blitted += EventSlot.Add<WindowControl, GenericWindow>(this, slot: nameof(Blitted), value: value);
        remove => ((WindowControl)this).Blitted -= EventSlot.Remove<WindowControl, GenericWindow>(this, slot: nameof(Blitted), value: value);
    }

    [Published]
    public new event AoWOnDrawSurface<GenericWindow> Blitting
    {
        add => ((WindowControl)this).Blitting += EventSlot.Add<WindowControl, GenericWindow>(this, slot: nameof(Blitting), value: value);
        remove => ((WindowControl)this).Blitting -= EventSlot.Remove<WindowControl, GenericWindow>(this, slot: nameof(Blitting), value: value);
    }

    public new event NotifyEvent<GenericWindow> Change
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(Change), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(Change), value: value);
    }

    public new event MouseEvent<GenericWindow> MouseDown
    {
        add => ((UIComponent)this).MouseDown += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(MouseDown), value: value);
        remove => ((UIComponent)this).MouseDown -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(MouseDown), value: value);
    }

    public new event MouseEvent<GenericWindow> MouseUp
    {
        add => ((UIComponent)this).MouseUp += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(MouseUp), value: value);
        remove => ((UIComponent)this).MouseUp -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(MouseUp), value: value);
    }

    public new event MouseMoveEvent<GenericWindow> MouseMove
    {
        add => ((UIComponent)this).MouseMove += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(MouseMove), value: value);
        remove => ((UIComponent)this).MouseMove -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(MouseMove), value: value);
    }

    public new event KeyPressEvent<GenericWindow> KeyPress
    {
        add => ((UIComponent)this).KeyPress += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(KeyPress), value: value);
        remove => ((UIComponent)this).KeyPress -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(KeyPress), value: value);
    }

    public new event KeyEvent<GenericWindow> KeyDown
    {
        add => ((UIComponent)this).KeyDown += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(KeyDown), value: value);
        remove => ((UIComponent)this).KeyDown -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(KeyDown), value: value);
    }

    public new event AoWOnDrawEvent<GenericWindow> PreDraw
    {
        add => ((UIComponent)this).PreDraw += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(PreDraw), value: value);
        remove => ((UIComponent)this).PreDraw -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(PreDraw), value: value);
    }

    public new event AoWOnDrawEvent<GenericWindow> Drawn
    {
        add => ((UIComponent)this).Drawn += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(Drawn), value: value);
        remove => ((UIComponent)this).Drawn -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(Drawn), value: value);
    }

    public new event NotifyEvent<GenericWindow> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(Updated), value: value);
    }

    public new event AoWOnDrawBackEvent<GenericWindow> DrawBack
    {
        add => ((WindowControl)this).DrawBack += EventSlot.Add<WindowControl, GenericWindow>(this, slot: nameof(DrawBack), value: value);
        remove => ((WindowControl)this).DrawBack -= EventSlot.Remove<WindowControl, GenericWindow>(this, slot: nameof(DrawBack), value: value);
    }

    public new event NotifyEvent<GenericWindow> Enter
    {
        add => ((WindowControl)this).Enter += EventSlot.Add<WindowControl, GenericWindow>(this, slot: nameof(Enter), value: value);
        remove => ((WindowControl)this).Enter -= EventSlot.Remove<WindowControl, GenericWindow>(this, slot: nameof(Enter), value: value);
    }

    public new event NotifyEvent<GenericWindow> Exit
    {
        add => ((WindowControl)this).Exit += EventSlot.Add<WindowControl, GenericWindow>(this, slot: nameof(Exit), value: value);
        remove => ((WindowControl)this).Exit -= EventSlot.Remove<WindowControl, GenericWindow>(this, slot: nameof(Exit), value: value);
    }

    public new event NotifyEvent<GenericWindow> Hide
    {
        add => ((BaseWindow)this).Hide += EventSlot.Add<BaseWindow, GenericWindow>(this, slot: nameof(Hide), value: value);
        remove => ((BaseWindow)this).Hide -= EventSlot.Remove<BaseWindow, GenericWindow>(this, slot: nameof(Hide), value: value);
    }

    public new event NotifyEvent<GenericWindow> Show
    {
        add => ((BaseWindow)this).Show += EventSlot.Add<BaseWindow, GenericWindow>(this, slot: nameof(Show), value: value);
        remove => ((BaseWindow)this).Show -= EventSlot.Remove<BaseWindow, GenericWindow>(this, slot: nameof(Show), value: value);
    }
}
