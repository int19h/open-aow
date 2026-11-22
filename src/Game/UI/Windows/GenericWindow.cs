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
    public new event NotifyEvent<GenericWindow> CursorChanged
    {
        add => ((WindowControl)this).CursorChanged += EventSlot.Add<WindowControl, GenericWindow>(this, slot: nameof(CursorChanged), value: value);
        remove => ((WindowControl)this).CursorChanged -= EventSlot.Remove<WindowControl, GenericWindow>(this, slot: nameof(CursorChanged), value: value);
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

    public new event NotifyEvent<GenericWindow> Changed
    {
        add => ((UIComponent)this).Changed += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(Changed), value: value);
        remove => ((UIComponent)this).Changed -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(Changed), value: value);
    }

    public new event MouseEvent<GenericWindow> MousePressed
    {
        add => ((UIComponent)this).MousePressed += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(MousePressed), value: value);
        remove => ((UIComponent)this).MousePressed -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(MousePressed), value: value);
    }

    public new event MouseEvent<GenericWindow> MouseReleased
    {
        add => ((UIComponent)this).MouseReleased += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(MouseReleased), value: value);
        remove => ((UIComponent)this).MouseReleased -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(MouseReleased), value: value);
    }

    public new event MouseMoveEvent<GenericWindow> MouseMoved
    {
        add => ((UIComponent)this).MouseMoved += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(MouseMoved), value: value);
        remove => ((UIComponent)this).MouseMoved -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(MouseMoved), value: value);
    }

    public new event KeyPressEvent<GenericWindow> CharacterTyped
    {
        add => ((UIComponent)this).CharacterTyped += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(CharacterTyped), value: value);
        remove => ((UIComponent)this).CharacterTyped -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(CharacterTyped), value: value);
    }

    public new event KeyEvent<GenericWindow> KeyPressed
    {
        add => ((UIComponent)this).KeyPressed += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(KeyPressed), value: value);
        remove => ((UIComponent)this).KeyPressed -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(KeyPressed), value: value);
    }

    public new event AoWOnDrawEvent<GenericWindow> Drawing
    {
        add => ((UIComponent)this).Drawing += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(Drawing), value: value);
        remove => ((UIComponent)this).Drawing -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(Drawing), value: value);
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

    public new event AoWOnDrawBackEvent<GenericWindow> BackgroundDrawn
    {
        add => ((WindowControl)this).BackgroundDrawn += EventSlot.Add<WindowControl, GenericWindow>(this, slot: nameof(BackgroundDrawn), value: value);
        remove => ((WindowControl)this).BackgroundDrawn -= EventSlot.Remove<WindowControl, GenericWindow>(this, slot: nameof(BackgroundDrawn), value: value);
    }

    public new event NotifyEvent<GenericWindow> MouseEntered
    {
        add => ((WindowControl)this).MouseEntered += EventSlot.Add<WindowControl, GenericWindow>(this, slot: nameof(MouseEntered), value: value);
        remove => ((WindowControl)this).MouseEntered -= EventSlot.Remove<WindowControl, GenericWindow>(this, slot: nameof(MouseEntered), value: value);
    }

    public new event NotifyEvent<GenericWindow> MouseLeft
    {
        add => ((WindowControl)this).MouseLeft += EventSlot.Add<WindowControl, GenericWindow>(this, slot: nameof(MouseLeft), value: value);
        remove => ((WindowControl)this).MouseLeft -= EventSlot.Remove<WindowControl, GenericWindow>(this, slot: nameof(MouseLeft), value: value);
    }

    public new event NotifyEvent<GenericWindow> Hidden
    {
        add => ((BaseWindow)this).Hidden += EventSlot.Add<BaseWindow, GenericWindow>(this, slot: nameof(Hidden), value: value);
        remove => ((BaseWindow)this).Hidden -= EventSlot.Remove<BaseWindow, GenericWindow>(this, slot: nameof(Hidden), value: value);
    }

    public new event NotifyEvent<GenericWindow> Shown
    {
        add => ((BaseWindow)this).Shown += EventSlot.Add<BaseWindow, GenericWindow>(this, slot: nameof(Shown), value: value);
        remove => ((BaseWindow)this).Shown -= EventSlot.Remove<BaseWindow, GenericWindow>(this, slot: nameof(Shown), value: value);
    }
}
