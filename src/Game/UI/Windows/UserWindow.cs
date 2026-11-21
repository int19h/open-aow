#nullable disable
// Translated from AowUserWindow.pas.
using Aow.Game.Draw;
using Aow.Game.UI.Controls;
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

// extended drawing event
public sealed class UserWindow(Component owner) : Window(owner)
{
    [Published]
    public event DrawSurfaceExEvent CustomDrawSurface;

    // AowUserWindow.pas:29
    public override void DrawSurface(Surface surface)
    {
        bool handled = false;
        CustomDrawSurface?.Invoke(surface, out handled);

        if (!handled)
        {
            base.DrawSurface(surface);
        }
    }

    public new event NotifyEvent<UserWindow> Change
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(Change), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(Change), value: value);
    }

    public new event MouseEvent<UserWindow> MouseDown
    {
        add => ((UIComponent)this).MouseDown += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(MouseDown), value: value);
        remove => ((UIComponent)this).MouseDown -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(MouseDown), value: value);
    }

    public new event MouseEvent<UserWindow> MouseUp
    {
        add => ((UIComponent)this).MouseUp += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(MouseUp), value: value);
        remove => ((UIComponent)this).MouseUp -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(MouseUp), value: value);
    }

    public new event MouseMoveEvent<UserWindow> MouseMove
    {
        add => ((UIComponent)this).MouseMove += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(MouseMove), value: value);
        remove => ((UIComponent)this).MouseMove -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(MouseMove), value: value);
    }

    public new event KeyPressEvent<UserWindow> KeyPress
    {
        add => ((UIComponent)this).KeyPress += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(KeyPress), value: value);
        remove => ((UIComponent)this).KeyPress -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(KeyPress), value: value);
    }

    public new event KeyEvent<UserWindow> KeyDown
    {
        add => ((UIComponent)this).KeyDown += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(KeyDown), value: value);
        remove => ((UIComponent)this).KeyDown -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(KeyDown), value: value);
    }

    public new event AoWOnDrawEvent<UserWindow> PreDraw
    {
        add => ((UIComponent)this).PreDraw += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(PreDraw), value: value);
        remove => ((UIComponent)this).PreDraw -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(PreDraw), value: value);
    }

    public new event AoWOnDrawEvent<UserWindow> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<UserWindow> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(Updated), value: value);
    }

    public new event AoWOnDrawBackEvent<UserWindow> DrawBack
    {
        add => ((WindowControl)this).DrawBack += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(DrawBack), value: value);
        remove => ((WindowControl)this).DrawBack -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(DrawBack), value: value);
    }

    public new event NotifyEvent<UserWindow> Enter
    {
        add => ((WindowControl)this).Enter += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(Enter), value: value);
        remove => ((WindowControl)this).Enter -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(Enter), value: value);
    }

    public new event NotifyEvent<UserWindow> Exit
    {
        add => ((WindowControl)this).Exit += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(Exit), value: value);
        remove => ((WindowControl)this).Exit -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(Exit), value: value);
    }

    public new event NotifyEvent<UserWindow> ChangeCursor
    {
        add => ((WindowControl)this).ChangeCursor += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(ChangeCursor), value: value);
        remove => ((WindowControl)this).ChangeCursor -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(ChangeCursor), value: value);
    }

    public new event AoWOnDrawSurface<UserWindow> SurfaceDrawn
    {
        add => ((WindowControl)this).SurfaceDrawn += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(SurfaceDrawn), value: value);
        remove => ((WindowControl)this).SurfaceDrawn -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(SurfaceDrawn), value: value);
    }

    public new event AoWOnDrawSurface<UserWindow> PreDrawSurface
    {
        add => ((WindowControl)this).PreDrawSurface += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(PreDrawSurface), value: value);
        remove => ((WindowControl)this).PreDrawSurface -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(PreDrawSurface), value: value);
    }

    public new event NotifyEvent<UserWindow> Drag
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(Drag), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(Drag), value: value);
    }

    public new event NotifyEvent<UserWindow> Resize
    {
        add => ((WindowControl)this).Resize += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(Resize), value: value);
        remove => ((WindowControl)this).Resize -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(Resize), value: value);
    }

    public new event NotifyEvent<UserWindow> InternalResize
    {
        add => ((WindowControl)this).InternalResize += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(InternalResize), value: value);
        remove => ((WindowControl)this).InternalResize -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(InternalResize), value: value);
    }

    public new event NotifyEvent<UserWindow> Hide
    {
        add => ((BaseWindow)this).Hide += EventSlot.Add<BaseWindow, UserWindow>(this, slot: nameof(Hide), value: value);
        remove => ((BaseWindow)this).Hide -= EventSlot.Remove<BaseWindow, UserWindow>(this, slot: nameof(Hide), value: value);
    }

    public new event NotifyEvent<UserWindow> Show
    {
        add => ((BaseWindow)this).Show += EventSlot.Add<BaseWindow, UserWindow>(this, slot: nameof(Show), value: value);
        remove => ((BaseWindow)this).Show -= EventSlot.Remove<BaseWindow, UserWindow>(this, slot: nameof(Show), value: value);
    }

    public delegate void DrawSurfaceExEvent(Surface Surface, out bool Handled);
}
