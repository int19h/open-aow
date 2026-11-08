// Translated from AoWWinGeneric.pas.
using Aow.Rtl;
using Aow.Units.AoWBaseWin;
using Aow.Units.AoWComp;
using Aow.Units.AoWWinControl;
using Aow.Units.AOWWinManager;

namespace Aow.Units.AoWWinGeneric;

// { The AoWWinGeneric unit of aowInt.dpl: TAOWGenericWindow, the base window class that publishes
//   the window manager's settings (the ancestor of TAOWWindow, TAOWToolTip and TAOWMenu). }
// no fields of its own
public partial class TAOWGenericWindow : TAOWBaseWindow
{
    public TAOWGenericWindow(TComponent owner) : base(owner)
    { }

    // AoWWinGeneric.pas:39
    public override void Destroy()
    {
        FBuffer.Free();
        base.Destroy();
    }

    // AoWWinGeneric.pas:45
    public override void SetOwner(TAoWComponent Value)
    {
        base.SetOwner(Value);
        FWinManager = (TAOWWinManager)FAOWOwner;
    }

    // AoWWinGeneric.pas:51
    public void SetCursor(int Value)
    {
        if (Value != FCursor)
        {
            FCursor = Value;
            FWindowCursor = Value;
            if (FOnChangeCursor != null)
            {
                FOnChangeCursor(this);
            }
        }
    }

    // AoWWinGeneric.pas:62
    public void SetBuffer(bool Value)
    {
        if (Value != FDoubleBuffered)
        {
            FDrawn = false;
            FDoubleBuffered = Value;
            if (!FDoubleBuffered)
            {
                if (FBackSurface != null)
                {
                    FBackSurface.Free();
                    FBackSurface = null;
                }
            }
        }
    }

    [Published]
    public TAOWWinManager WinManager { get => FWinManager; set => SetManager(value); }

    [Published]
    public bool UseVMem { get => FUseVMem; set => FUseVMem = value; }

    [Published]
    public int BorderWidth { get => FBorderWidth; set => FBorderWidth = value; }

    [Published]
    public int BorderHeight { get => FBorderHeight; set => FBorderHeight = value; }

    [Published]
    public bool OverlapTopCorners { get => FOverlapTopCorners; set => SetOverlap(value); }

    [Published]
    public int Cursor { get => FCursor; set => SetCursor(value); }

    [Published]
    public bool DoubleBuffered { get => FDoubleBuffered; set => SetBuffer(value); }

    [Published]
    public TNotifyEvent<TAOWGenericWindow> OnChangeCursor
    {
        get => EventSlot.Get(this, "TAOWGenericWindow.FOnChangeCursor", FOnChangeCursor, static slot => slot);
        set => EventSlot.Set(this, "TAOWGenericWindow.FOnChangeCursor", ref FOnChangeCursor, value, handler => sender => handler(this));
    }

    [Published]
    public int MaskColor { get => FMaskColor; set => FMaskColor = value; }

    [Published]
    public TAoWOnDrawSurface<TAOWGenericWindow> OnDrawSurface
    {
        get => EventSlot.Get(this, "TAOWGenericWindow.FOnDrawSurface", FOnDrawSurface, static slot => slot);
        set => EventSlot.Set(this, "TAOWGenericWindow.FOnDrawSurface", ref FOnDrawSurface, value, handler => (sender, surface, left, top) => handler(this, surface, left, top));
    }

    [Published]
    public TAoWOnDrawSurface<TAOWGenericWindow> OnPreDrawSurface
    {
        get => EventSlot.Get(this, "TAOWGenericWindow.FOnPreDrawSurface", FOnPreDrawSurface, static slot => slot);
        set => EventSlot.Set(this, "TAOWGenericWindow.FOnPreDrawSurface", ref FOnPreDrawSurface, value, handler => (sender, surface, left, top) => handler(this, surface, left, top));
    }

    [Published]
    public bool FreeSurfaceOnHide { get => FFreeSurfaceOnHide; set => FFreeSurfaceOnHide = value; }

    public new TNotifyEvent<TAOWGenericWindow> OnChange
    {
        get => EventSlot.Get(this, "TAOWGenericWindow.FOnChange", FOnChange, static slot => slot);
        set => EventSlot.Set(this, "TAOWGenericWindow.FOnChange", ref FOnChange, value, handler => sender => handler(this));
    }

    public new TMouseEvent<TAOWGenericWindow> OnMouseDown
    {
        get => EventSlot.Get(this, "TAOWGenericWindow.FOnMouseDown", FOnMouseDown, static slot => slot);
        set => EventSlot.Set(this, "TAOWGenericWindow.FOnMouseDown", ref FOnMouseDown, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseEvent<TAOWGenericWindow> OnMouseUp
    {
        get => EventSlot.Get(this, "TAOWGenericWindow.FOnMouseUp", FOnMouseUp, static slot => slot);
        set => EventSlot.Set(this, "TAOWGenericWindow.FOnMouseUp", ref FOnMouseUp, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseMoveEvent<TAOWGenericWindow> OnMouseMove
    {
        get => EventSlot.Get(this, "TAOWGenericWindow.FOnMouseMove", FOnMouseMove, static slot => slot);
        set => EventSlot.Set(this, "TAOWGenericWindow.FOnMouseMove", ref FOnMouseMove, value, handler => (sender, shift, x, y) => handler(this, shift, x, y));
    }

    public new TKeyPressEvent<TAOWGenericWindow> OnKeyPress
    {
        get => EventSlot.Get(this, "TAOWGenericWindow.FOnKeyPress", FOnKeyPress, static slot => slot);
        set => EventSlot.Set(this, "TAOWGenericWindow.FOnKeyPress", ref FOnKeyPress, value, handler => (sender, ref key) => handler(this, ref key));
    }

    public new TKeyEvent<TAOWGenericWindow> OnKeyDown
    {
        get => EventSlot.Get(this, "TAOWGenericWindow.FOnKeyDown", FOnKeyDown, static slot => slot);
        set => EventSlot.Set(this, "TAOWGenericWindow.FOnKeyDown", ref FOnKeyDown, value, handler => (sender, ref key, shift) => handler(this, ref key, shift));
    }

    public new TAoWOnDrawEvent<TAOWGenericWindow> OnPreDraw
    {
        get => EventSlot.Get(this, "TAOWGenericWindow.FOnPreDraw", FOnPreDraw, static slot => slot);
        set => EventSlot.Set(this, "TAOWGenericWindow.FOnPreDraw", ref FOnPreDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TAoWOnDrawEvent<TAOWGenericWindow> OnDraw
    {
        get => EventSlot.Get(this, "TAOWGenericWindow.FOnDraw", FOnDraw, static slot => slot);
        set => EventSlot.Set(this, "TAOWGenericWindow.FOnDraw", ref FOnDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TNotifyEvent<TAOWGenericWindow> OnUpdate
    {
        get => EventSlot.Get(this, "TAOWGenericWindow.FOnUpdate", FOnUpdate, static slot => slot);
        set => EventSlot.Set(this, "TAOWGenericWindow.FOnUpdate", ref FOnUpdate, value, handler => sender => handler(this));
    }

    public new TAoWOnDrawBackEvent<TAOWGenericWindow> OnDrawBack
    {
        get => EventSlot.Get(this, "TAOWGenericWindow.FOnDrawBack", FOnDrawBack, static slot => slot);
        set => EventSlot.Set(this, "TAOWGenericWindow.FOnDrawBack", ref FOnDrawBack, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TNotifyEvent<TAOWGenericWindow> OnEnter
    {
        get => EventSlot.Get(this, "TAOWGenericWindow.FOnEnter", FOnEnter, static slot => slot);
        set => EventSlot.Set(this, "TAOWGenericWindow.FOnEnter", ref FOnEnter, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TAOWGenericWindow> OnExit
    {
        get => EventSlot.Get(this, "TAOWGenericWindow.FOnExit", FOnExit, static slot => slot);
        set => EventSlot.Set(this, "TAOWGenericWindow.FOnExit", ref FOnExit, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TAOWGenericWindow> OnHide
    {
        get => EventSlot.Get(this, "TAOWGenericWindow.FOnHide", FOnHide, static slot => slot);
        set => EventSlot.Set(this, "TAOWGenericWindow.FOnHide", ref FOnHide, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TAOWGenericWindow> OnShow
    {
        get => EventSlot.Get(this, "TAOWGenericWindow.FOnShow", FOnShow, static slot => slot);
        set => EventSlot.Set(this, "TAOWGenericWindow.FOnShow", ref FOnShow, value, handler => sender => handler(this));
    }
}

public static partial class AoWWinGenericUnit
{
}
