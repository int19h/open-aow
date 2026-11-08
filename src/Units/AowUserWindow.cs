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
        get => EventSlot.Get(this, "TAoWUserWindow.FOnChange", FOnChange, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnChange", ref FOnChange, value, handler => sender => handler(this));
    }

    public new TMouseEvent<TAoWUserWindow> OnMouseDown
    {
        get => EventSlot.Get(this, "TAoWUserWindow.FOnMouseDown", FOnMouseDown, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnMouseDown", ref FOnMouseDown, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseEvent<TAoWUserWindow> OnMouseUp
    {
        get => EventSlot.Get(this, "TAoWUserWindow.FOnMouseUp", FOnMouseUp, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnMouseUp", ref FOnMouseUp, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseMoveEvent<TAoWUserWindow> OnMouseMove
    {
        get => EventSlot.Get(this, "TAoWUserWindow.FOnMouseMove", FOnMouseMove, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnMouseMove", ref FOnMouseMove, value, handler => (sender, shift, x, y) => handler(this, shift, x, y));
    }

    public new TKeyPressEvent<TAoWUserWindow> OnKeyPress
    {
        get => EventSlot.Get(this, "TAoWUserWindow.FOnKeyPress", FOnKeyPress, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnKeyPress", ref FOnKeyPress, value, handler => (sender, ref key) => handler(this, ref key));
    }

    public new TKeyEvent<TAoWUserWindow> OnKeyDown
    {
        get => EventSlot.Get(this, "TAoWUserWindow.FOnKeyDown", FOnKeyDown, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnKeyDown", ref FOnKeyDown, value, handler => (sender, ref key, shift) => handler(this, ref key, shift));
    }

    public new TAoWOnDrawEvent<TAoWUserWindow> OnPreDraw
    {
        get => EventSlot.Get(this, "TAoWUserWindow.FOnPreDraw", FOnPreDraw, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnPreDraw", ref FOnPreDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TAoWOnDrawEvent<TAoWUserWindow> OnDraw
    {
        get => EventSlot.Get(this, "TAoWUserWindow.FOnDraw", FOnDraw, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnDraw", ref FOnDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TNotifyEvent<TAoWUserWindow> OnUpdate
    {
        get => EventSlot.Get(this, "TAoWUserWindow.FOnUpdate", FOnUpdate, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnUpdate", ref FOnUpdate, value, handler => sender => handler(this));
    }

    public new TAoWOnDrawBackEvent<TAoWUserWindow> OnDrawBack
    {
        get => EventSlot.Get(this, "TAoWUserWindow.FOnDrawBack", FOnDrawBack, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnDrawBack", ref FOnDrawBack, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TNotifyEvent<TAoWUserWindow> OnEnter
    {
        get => EventSlot.Get(this, "TAoWUserWindow.FOnEnter", FOnEnter, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnEnter", ref FOnEnter, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TAoWUserWindow> OnExit
    {
        get => EventSlot.Get(this, "TAoWUserWindow.FOnExit", FOnExit, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnExit", ref FOnExit, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TAoWUserWindow> OnChangeCursor
    {
        get => EventSlot.Get(this, "TAoWUserWindow.FOnChangeCursor", FOnChangeCursor, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnChangeCursor", ref FOnChangeCursor, value, handler => sender => handler(this));
    }

    public new TAoWOnDrawSurface<TAoWUserWindow> OnDrawSurface
    {
        get => EventSlot.Get(this, "TAoWUserWindow.FOnDrawSurface", FOnDrawSurface, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnDrawSurface", ref FOnDrawSurface, value, handler => (sender, surface, left, top) => handler(this, surface, left, top));
    }

    public new TAoWOnDrawSurface<TAoWUserWindow> OnPreDrawSurface
    {
        get => EventSlot.Get(this, "TAoWUserWindow.FOnPreDrawSurface", FOnPreDrawSurface, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnPreDrawSurface", ref FOnPreDrawSurface, value, handler => (sender, surface, left, top) => handler(this, surface, left, top));
    }

    public new TNotifyEvent<TAoWUserWindow> OnDrag
    {
        get => EventSlot.Get(this, "TAoWUserWindow.FOnChange", FOnChange, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnChange", ref FOnChange, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TAoWUserWindow> OnResize
    {
        get => EventSlot.Get(this, "TAoWUserWindow.FOnResize", FOnResize, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnResize", ref FOnResize, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TAoWUserWindow> OnInternalResize
    {
        get => EventSlot.Get(this, "TAoWUserWindow.FOnInternalResize", FOnInternalResize, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnInternalResize", ref FOnInternalResize, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TAoWUserWindow> OnHide
    {
        get => EventSlot.Get(this, "TAoWUserWindow.FOnHide", FOnHide, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnHide", ref FOnHide, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TAoWUserWindow> OnShow
    {
        get => EventSlot.Get(this, "TAoWUserWindow.FOnShow", FOnShow, static slot => slot);
        set => EventSlot.Set(this, "TAoWUserWindow.FOnShow", ref FOnShow, value, handler => sender => handler(this));
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
