// Translated from AowUserWindow.pas.
using Aow.Game.Draw;
using Aow.Game.UI.Controls;
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

// extended drawing event
public partial class UserWindow : Window
{
    public UserWindow(Component owner) : base(owner)
    { }

    [Published]
    public event DrawSurfaceExEvent OnCustomDrawSurface;

    // AowUserWindow.pas:29
    public override void DrawSurface(Surface surface)
    {
        bool handled;
        handled = false;
        if (OnCustomDrawSurface != null)
        {
            OnCustomDrawSurface(surface, out handled);
        }

        if (!handled)
        {
            base.DrawSurface(surface);
        }
    }

    public new event NotifyEvent<UserWindow> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<UserWindow> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<UserWindow> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<UserWindow> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<UserWindow> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<UserWindow> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<UserWindow> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<UserWindow> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<UserWindow> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(OnUpdate), value: value);
    }

    public new event AoWOnDrawBackEvent<UserWindow> OnDrawBack
    {
        add => ((WindowControl)this).OnDrawBack += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(OnDrawBack), value: value);
        remove => ((WindowControl)this).OnDrawBack -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(OnDrawBack), value: value);
    }

    public new event NotifyEvent<UserWindow> OnEnter
    {
        add => ((WindowControl)this).OnEnter += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(OnEnter), value: value);
        remove => ((WindowControl)this).OnEnter -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(OnEnter), value: value);
    }

    public new event NotifyEvent<UserWindow> OnExit
    {
        add => ((WindowControl)this).OnExit += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(OnExit), value: value);
        remove => ((WindowControl)this).OnExit -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(OnExit), value: value);
    }

    public new event NotifyEvent<UserWindow> OnChangeCursor
    {
        add => ((WindowControl)this).OnChangeCursor += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(OnChangeCursor), value: value);
        remove => ((WindowControl)this).OnChangeCursor -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(OnChangeCursor), value: value);
    }

    public new event AoWOnDrawSurface<UserWindow> OnDrawSurface
    {
        add => ((WindowControl)this).OnDrawSurface += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(OnDrawSurface), value: value);
        remove => ((WindowControl)this).OnDrawSurface -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(OnDrawSurface), value: value);
    }

    public new event AoWOnDrawSurface<UserWindow> OnPreDrawSurface
    {
        add => ((WindowControl)this).OnPreDrawSurface += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(OnPreDrawSurface), value: value);
        remove => ((WindowControl)this).OnPreDrawSurface -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(OnPreDrawSurface), value: value);
    }

    public new event NotifyEvent<UserWindow> OnDrag
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(OnDrag), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(OnDrag), value: value);
    }

    public new event NotifyEvent<UserWindow> OnResize
    {
        add => ((WindowControl)this).OnResize += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(OnResize), value: value);
        remove => ((WindowControl)this).OnResize -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(OnResize), value: value);
    }

    public new event NotifyEvent<UserWindow> OnInternalResize
    {
        add => ((WindowControl)this).OnInternalResize += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(OnInternalResize), value: value);
        remove => ((WindowControl)this).OnInternalResize -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(OnInternalResize), value: value);
    }

    public new event NotifyEvent<UserWindow> OnHide
    {
        add => ((BaseWindow)this).OnHide += EventSlot.Add<BaseWindow, UserWindow>(this, slot: nameof(OnHide), value: value);
        remove => ((BaseWindow)this).OnHide -= EventSlot.Remove<BaseWindow, UserWindow>(this, slot: nameof(OnHide), value: value);
    }

    public new event NotifyEvent<UserWindow> OnShow
    {
        add => ((BaseWindow)this).OnShow += EventSlot.Add<BaseWindow, UserWindow>(this, slot: nameof(OnShow), value: value);
        remove => ((BaseWindow)this).OnShow -= EventSlot.Remove<BaseWindow, UserWindow>(this, slot: nameof(OnShow), value: value);
    }

    public delegate void DrawSurfaceExEvent(Surface Surface, out bool Handled);
}
