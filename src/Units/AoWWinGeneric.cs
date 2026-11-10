// Translated from AoWWinGeneric.pas.
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;

namespace Aow.Game.Ui.Windows;

// { The AoWWinGeneric unit of aowInt.dpl: TAOWGenericWindow, the base window class that publishes
//   the window manager's settings (the ancestor of TAOWWindow, TAOWToolTip and TAOWMenu). }
// no fields of its own
public partial class AOWGenericWindow : AOWBaseWindow
{
    public AOWGenericWindow(Component owner) : base(owner)
    { }

    // AoWWinGeneric.pas:39
    public override void Dispose()
    {
        (FBuffer as IDisposable)?.Dispose();
        base.Dispose();
    }

    // AoWWinGeneric.pas:45
    public override void SetOwner(AoWComponent Value)
    {
        base.SetOwner(Value);
        FWinManager = (AOWWinManager)FAOWOwner;
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
                    FBackSurface?.Dispose();
                    FBackSurface = null;
                }
            }
        }
    }

    [Published]
    public AOWWinManager WinManager { get => FWinManager; set => SetManager(value); }

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
    public NotifyEvent<AOWGenericWindow> OnChangeCursor
    {
        get => EventSlot.Get(this, FOnChangeCursor);
        set => EventSlot.Set(this, ref FOnChangeCursor, value);
    }

    [Published]
    public int MaskColor { get => FMaskColor; set => FMaskColor = value; }

    [Published]
    public AoWOnDrawSurface<AOWGenericWindow> OnDrawSurface
    {
        get => EventSlot.Get(this, FOnDrawSurface);
        set => EventSlot.Set(this, ref FOnDrawSurface, value);
    }

    [Published]
    public AoWOnDrawSurface<AOWGenericWindow> OnPreDrawSurface
    {
        get => EventSlot.Get(this, FOnPreDrawSurface);
        set => EventSlot.Set(this, ref FOnPreDrawSurface, value);
    }

    [Published]
    public bool FreeSurfaceOnHide { get => FFreeSurfaceOnHide; set => FFreeSurfaceOnHide = value; }

    public new NotifyEvent<AOWGenericWindow> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<AOWGenericWindow> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<AOWGenericWindow> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<AOWGenericWindow> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<AOWGenericWindow> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<AOWGenericWindow> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<AOWGenericWindow> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<AOWGenericWindow> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<AOWGenericWindow> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }

    public new AoWOnDrawBackEvent<AOWGenericWindow> OnDrawBack
    {
        get => EventSlot.Get(this, FOnDrawBack);
        set => EventSlot.Set(this, ref FOnDrawBack, value);
    }

    public new NotifyEvent<AOWGenericWindow> OnEnter
    {
        get => EventSlot.Get(this, FOnEnter);
        set => EventSlot.Set(this, ref FOnEnter, value);
    }

    public new NotifyEvent<AOWGenericWindow> OnExit
    {
        get => EventSlot.Get(this, FOnExit);
        set => EventSlot.Set(this, ref FOnExit, value);
    }

    public new NotifyEvent<AOWGenericWindow> OnHide
    {
        get => EventSlot.Get(this, FOnHide);
        set => EventSlot.Set(this, ref FOnHide, value);
    }

    public new NotifyEvent<AOWGenericWindow> OnShow
    {
        get => EventSlot.Get(this, FOnShow);
        set => EventSlot.Set(this, ref FOnShow, value);
    }
}
