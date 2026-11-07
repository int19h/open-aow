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
    public TNotifyEvent OnChangeCursor { get => FOnChangeCursor; set => FOnChangeCursor = value; }

    [Published]
    public int MaskColor { get => FMaskColor; set => FMaskColor = value; }

    [Published]
    public TAoWOnDrawSurface OnDrawSurface { get => FOnDrawSurface; set => FOnDrawSurface = value; }

    [Published]
    public TAoWOnDrawSurface OnPreDrawSurface { get => FOnPreDrawSurface; set => FOnPreDrawSurface = value; }

    [Published]
    public bool FreeSurfaceOnHide { get => FFreeSurfaceOnHide; set => FFreeSurfaceOnHide = value; }
}

public static partial class AoWWinGenericUnit
{
}
