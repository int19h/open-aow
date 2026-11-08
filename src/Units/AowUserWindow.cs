// Translated from AowUserWindow.pas.
using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AoWWinControl;
using Aow.Units.AoWWindow;
using Aow.Units.GFXE;
using static Aow.Rtl.RtlUnit;

namespace Aow.Units.AowUserWindow;

// extended drawing event
public partial class TAoWUserWindow : TAOWWindow
{
    public TAoWUserWindow(TComponent owner) : base(owner)
    { }

    public TDrawSurfaceExEvent FOnDrawSurfaceEx;

    // AowUserWindow.pas:29
    public override void DrawSurface(TSurface Surface)
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
    public TDrawSurfaceExEvent OnDrawSurfaceEx { get => FOnDrawSurfaceEx; set => FOnDrawSurfaceEx = value; }

    public new TNotifyEvent<TAoWUserWindow> OnChange
    {
        get => EventSlot.Get(this, FOnChange, static slot => (TNotifyEvent<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new TMouseEvent<TAoWUserWindow> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown, static slot => (TMouseEvent<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new TMouseEvent<TAoWUserWindow> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp, static slot => (TMouseEvent<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new TMouseMoveEvent<TAoWUserWindow> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove, static slot => (TMouseMoveEvent<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new TKeyPressEvent<TAoWUserWindow> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress, static slot => (TKeyPressEvent<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new TKeyEvent<TAoWUserWindow> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown, static slot => (TKeyEvent<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new TAoWOnDrawEvent<TAoWUserWindow> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw, static slot => (TAoWOnDrawEvent<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new TAoWOnDrawEvent<TAoWUserWindow> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw, static slot => (TAoWOnDrawEvent<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new TNotifyEvent<TAoWUserWindow> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate, static slot => (TNotifyEvent<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }

    public new TAoWOnDrawBackEvent<TAoWUserWindow> OnDrawBack
    {
        get => EventSlot.Get(this, FOnDrawBack, static slot => (TAoWOnDrawBackEvent<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnDrawBack, value);
    }

    public new TNotifyEvent<TAoWUserWindow> OnEnter
    {
        get => EventSlot.Get(this, FOnEnter, static slot => (TNotifyEvent<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnEnter, value);
    }

    public new TNotifyEvent<TAoWUserWindow> OnExit
    {
        get => EventSlot.Get(this, FOnExit, static slot => (TNotifyEvent<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnExit, value);
    }

    public new TNotifyEvent<TAoWUserWindow> OnChangeCursor
    {
        get => EventSlot.Get(this, FOnChangeCursor, static slot => (TNotifyEvent<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnChangeCursor, value);
    }

    public new TAoWOnDrawSurface<TAoWUserWindow> OnDrawSurface
    {
        get => EventSlot.Get(this, FOnDrawSurface, static slot => (TAoWOnDrawSurface<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnDrawSurface, value);
    }

    public new TAoWOnDrawSurface<TAoWUserWindow> OnPreDrawSurface
    {
        get => EventSlot.Get(this, FOnPreDrawSurface, static slot => (TAoWOnDrawSurface<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnPreDrawSurface, value);
    }

    public new TNotifyEvent<TAoWUserWindow> OnDrag
    {
        get => EventSlot.Get(this, FOnChange, static slot => (TNotifyEvent<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new TNotifyEvent<TAoWUserWindow> OnResize
    {
        get => EventSlot.Get(this, FOnResize, static slot => (TNotifyEvent<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnResize, value);
    }

    public new TNotifyEvent<TAoWUserWindow> OnInternalResize
    {
        get => EventSlot.Get(this, FOnInternalResize, static slot => (TNotifyEvent<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnInternalResize, value);
    }

    public new TNotifyEvent<TAoWUserWindow> OnHide
    {
        get => EventSlot.Get(this, FOnHide, static slot => (TNotifyEvent<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnHide, value);
    }

    public new TNotifyEvent<TAoWUserWindow> OnShow
    {
        get => EventSlot.Get(this, FOnShow, static slot => (TNotifyEvent<TAoWUserWindow>)slot);
        set => EventSlot.Set(this, ref FOnShow, value);
    }
}

public delegate void TDrawSurfaceExEvent(TSurface Surface, ref bool Handled);

public static partial class AowUserWindowUnit
{
    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
