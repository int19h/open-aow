// Translated from AowUserWindow.pas.
using Aow.Game.Draw;
using Aow.Game.Ui.Controls;

using Aow.Rtl;

namespace Aow.Game.Ui.Windows;

// extended drawing event
public partial class AoWUserWindow : AOWWindow
{
    public AoWUserWindow(Component owner) : base(owner)
    { }

    public DrawSurfaceExEvent FOnDrawSurfaceEx;

    // AowUserWindow.pas:29
    public override void DrawSurface(Surface Surface)
    {
        bool Handled;
        Handled = false;
        if (FOnDrawSurfaceEx != null)
        {
            FOnDrawSurfaceEx(Surface, ref Handled);
        }

        if (!Handled)
        {
            base.DrawSurface(Surface);
        }
    }

    [Published]
    public DrawSurfaceExEvent OnDrawSurfaceEx { get => FOnDrawSurfaceEx; set => FOnDrawSurfaceEx = value; }

    public new NotifyEvent<AoWUserWindow> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<AoWUserWindow> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<AoWUserWindow> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<AoWUserWindow> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<AoWUserWindow> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<AoWUserWindow> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<AoWUserWindow> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<AoWUserWindow> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<AoWUserWindow> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }

    public new AoWOnDrawBackEvent<AoWUserWindow> OnDrawBack
    {
        get => EventSlot.Get(this, FOnDrawBack);
        set => EventSlot.Set(this, ref FOnDrawBack, value);
    }

    public new NotifyEvent<AoWUserWindow> OnEnter
    {
        get => EventSlot.Get(this, FOnEnter);
        set => EventSlot.Set(this, ref FOnEnter, value);
    }

    public new NotifyEvent<AoWUserWindow> OnExit
    {
        get => EventSlot.Get(this, FOnExit);
        set => EventSlot.Set(this, ref FOnExit, value);
    }

    public new NotifyEvent<AoWUserWindow> OnChangeCursor
    {
        get => EventSlot.Get(this, FOnChangeCursor);
        set => EventSlot.Set(this, ref FOnChangeCursor, value);
    }

    public new AoWOnDrawSurface<AoWUserWindow> OnDrawSurface
    {
        get => EventSlot.Get(this, FOnDrawSurface);
        set => EventSlot.Set(this, ref FOnDrawSurface, value);
    }

    public new AoWOnDrawSurface<AoWUserWindow> OnPreDrawSurface
    {
        get => EventSlot.Get(this, FOnPreDrawSurface);
        set => EventSlot.Set(this, ref FOnPreDrawSurface, value);
    }

    public new NotifyEvent<AoWUserWindow> OnDrag
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new NotifyEvent<AoWUserWindow> OnResize
    {
        get => EventSlot.Get(this, FOnResize);
        set => EventSlot.Set(this, ref FOnResize, value);
    }

    public new NotifyEvent<AoWUserWindow> OnInternalResize
    {
        get => EventSlot.Get(this, FOnInternalResize);
        set => EventSlot.Set(this, ref FOnInternalResize, value);
    }

    public new NotifyEvent<AoWUserWindow> OnHide
    {
        get => EventSlot.Get(this, FOnHide);
        set => EventSlot.Set(this, ref FOnHide, value);
    }

    public new NotifyEvent<AoWUserWindow> OnShow
    {
        get => EventSlot.Get(this, FOnShow);
        set => EventSlot.Set(this, ref FOnShow, value);
    }

    public delegate void DrawSurfaceExEvent(Surface Surface, ref bool Handled);
}
