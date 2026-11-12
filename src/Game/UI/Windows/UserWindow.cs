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

    public DrawSurfaceExEvent _onCustomDrawSurface;

    // AowUserWindow.pas:29
    public override void DrawSurface(Surface surface)
    {
        bool handled;
        handled = false;
        if (_onCustomDrawSurface != null)
        {
            _onCustomDrawSurface(surface, ref handled);
        }

        if (!handled)
        {
            base.DrawSurface(surface);
        }
    }

    [Published]
    public DrawSurfaceExEvent OnCustomDrawSurface { get => _onCustomDrawSurface; set => _onCustomDrawSurface = value; }

    public new NotifyEvent<UserWindow> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<UserWindow> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<UserWindow> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<UserWindow> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<UserWindow> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<UserWindow> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<UserWindow> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<UserWindow> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<UserWindow> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }

    public new AoWOnDrawBackEvent<UserWindow> OnDrawBack
    {
        get => EventSlot.Get(this, _onDrawBack);
        set => EventSlot.Set(this, ref _onDrawBack, value);
    }

    public new NotifyEvent<UserWindow> OnEnter
    {
        get => EventSlot.Get(this, _onEnter);
        set => EventSlot.Set(this, ref _onEnter, value);
    }

    public new NotifyEvent<UserWindow> OnExit
    {
        get => EventSlot.Get(this, _onExit);
        set => EventSlot.Set(this, ref _onExit, value);
    }

    public new NotifyEvent<UserWindow> OnChangeCursor
    {
        get => EventSlot.Get(this, _onChangeCursor);
        set => EventSlot.Set(this, ref _onChangeCursor, value);
    }

    public new AoWOnDrawSurface<UserWindow> OnDrawSurface
    {
        get => EventSlot.Get(this, _onDrawSurface);
        set => EventSlot.Set(this, ref _onDrawSurface, value);
    }

    public new AoWOnDrawSurface<UserWindow> OnPreDrawSurface
    {
        get => EventSlot.Get(this, _onPreDrawSurface);
        set => EventSlot.Set(this, ref _onPreDrawSurface, value);
    }

    public new NotifyEvent<UserWindow> OnDrag
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new NotifyEvent<UserWindow> OnResize
    {
        get => EventSlot.Get(this, _onResize);
        set => EventSlot.Set(this, ref _onResize, value);
    }

    public new NotifyEvent<UserWindow> OnInternalResize
    {
        get => EventSlot.Get(this, _onInternalResize);
        set => EventSlot.Set(this, ref _onInternalResize, value);
    }

    public new NotifyEvent<UserWindow> OnHide
    {
        get => EventSlot.Get(this, _onHide);
        set => EventSlot.Set(this, ref _onHide, value);
    }

    public new NotifyEvent<UserWindow> OnShow
    {
        get => EventSlot.Get(this, _onShow);
        set => EventSlot.Set(this, ref _onShow, value);
    }

    public delegate void DrawSurfaceExEvent(Surface Surface, ref bool Handled);
}
