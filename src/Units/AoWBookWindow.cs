// Translated from AoWBookWindow.pas.
using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AoWWinControl;
using Aow.Units.AoWWindow;
using Aow.Units.GFXE;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.GFXE.GFXEUnit;

namespace Aow.Units.AoWBookWindow;

// completion event, first frame, previous surface validity, page turn, animating, saved page
// surface, page-turn mode
public partial class TAoWBookWindow : TAOWWindow
{
    public TNotifyEvent<TAoWBookWindow> FOnAnimationComplete;
    public bool FFirstAnimationFrame;
    public bool FPreviousSurfaceValid;
    public PageTurnCtrl.TPageTurnControl FPageTurn;
    public bool FAnimating;
    public TSurface FPageSurface;
    public PageTurnCtrl.TPageTurnMode FPageTurnMode;

    // AoWBookWindow.pas:50
    public override void Dispose()
    {
        TSurface S;
        S = FPageSurface;
        if (S != null)
        {
            S?.Dispose();
        }

        base.Dispose();
    }

    // AoWBookWindow.pas:44
    public TAoWBookWindow(TComponent owner) : base(owner)
    {
        FPageTurn = new PageTurnCtrl.TPageTurnControl();
    }

    // AoWBookWindow.pas:78
    public override void Update()
    {
        if (FVisible && FAnimating)
        {
            FSurfaceValid = false;
            if (!FPageTurn.UpdateScene())
            {
                FAnimating = false;
                if (FOnAnimationComplete != null)
                {
                    FOnAnimationComplete(this);
                }
            }
        }

        base.Update();
    }

    // AoWBookWindow.pas:59
    public override void SetVisible(bool Value)
    {
        TSurface S;
        if (Value != FVisible)
        {
            FAnimating = false;
            if (FFreeSurfaceOnHide)
            {
                S = FPageSurface;
                if (S != null)
                {
                    S?.Dispose();
                    FPageSurface = null;
                }
            }
        }

        base.SetVisible(Value);
    }

    // AoWBookWindow.pas:113
    public override void DrawSurface(TSurface Surface)
    {
        FPreviousSurfaceValid = FSurfaceValid;
        base.DrawSurface(Surface);
        if (FPreviousSurfaceValid && FAnimating)
        {
            Blt(Surface);
        }
    }

    // AoWBookWindow.pas:186
    public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        FHandled = FAnimating;
        if (!FAnimating)
        {
            base.CheckMouseDown(Button, Shift, X, Y);
        }
        else
        {
            if (X > FWinLeft - 1 && X < FWinLeft + FWinWidth && Y > FWinTop - 1 && Y < FWinTop + FWinHeight)
            {
                FSurfaceValid = false;
                FAnimating = false;
                if (FOnAnimationComplete != null)
                {
                    FOnAnimationComplete(this);
                }
            }
        }
    }

    // AoWBookWindow.pas:199
    public override void CheckMouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        FHandled = FAnimating;
        if (!FAnimating)
        {
            base.CheckMouseUp(Button, Shift, X, Y);
        }
    }

    // AoWBookWindow.pas:205
    public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        FHandled = FAnimating;
        if (!FAnimating)
        {
            base.CheckMouseMove(Shift, X, Y);
        }
    }

    // AoWBookWindow.pas:92
    public void StartAnim(PageTurnCtrl.TPageTurnMode Mode)
    {
        TSurfaceDesc Source = default;
        TSurfaceDesc Dest = default;
        TRect R;
        if (FBackSurface != null && FPageSurface != null)
        {
            R = Rect(0, 0, FWinWidth, FWinHeight);
            FBackSurface.Lock(ref Source);
            FPageSurface.Lock(ref Dest);
            BltSDRectClipped(Source, R, Dest, 0, 0);
            FPageSurface.Unlock();
            FBackSurface.Unlock();
            FPageTurn.SetPageDimensions(271, 368);
            FPageTurn.FSpeed = 15;
            FPageTurn.Start(Mode);
            FPageTurnMode = Mode;
            FDrawn = false;
            FAnimating = true;
            FFirstAnimationFrame = true;
        }
    }

    [Published]
    public TNotifyEvent<TAoWBookWindow> OnAnimDone { get => FOnAnimationComplete; set => FOnAnimationComplete = value; }

    public new TNotifyEvent<TAoWBookWindow> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new TMouseEvent<TAoWBookWindow> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new TMouseEvent<TAoWBookWindow> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new TMouseMoveEvent<TAoWBookWindow> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new TKeyPressEvent<TAoWBookWindow> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new TKeyEvent<TAoWBookWindow> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new TAoWOnDrawEvent<TAoWBookWindow> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new TAoWOnDrawEvent<TAoWBookWindow> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new TNotifyEvent<TAoWBookWindow> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }

    public new TAoWOnDrawBackEvent<TAoWBookWindow> OnDrawBack
    {
        get => EventSlot.Get(this, FOnDrawBack);
        set => EventSlot.Set(this, ref FOnDrawBack, value);
    }

    public new TNotifyEvent<TAoWBookWindow> OnEnter
    {
        get => EventSlot.Get(this, FOnEnter);
        set => EventSlot.Set(this, ref FOnEnter, value);
    }

    public new TNotifyEvent<TAoWBookWindow> OnExit
    {
        get => EventSlot.Get(this, FOnExit);
        set => EventSlot.Set(this, ref FOnExit, value);
    }

    public new TNotifyEvent<TAoWBookWindow> OnChangeCursor
    {
        get => EventSlot.Get(this, FOnChangeCursor);
        set => EventSlot.Set(this, ref FOnChangeCursor, value);
    }

    public new TAoWOnDrawSurface<TAoWBookWindow> OnDrawSurface
    {
        get => EventSlot.Get(this, FOnDrawSurface);
        set => EventSlot.Set(this, ref FOnDrawSurface, value);
    }

    public new TAoWOnDrawSurface<TAoWBookWindow> OnPreDrawSurface
    {
        get => EventSlot.Get(this, FOnPreDrawSurface);
        set => EventSlot.Set(this, ref FOnPreDrawSurface, value);
    }

    public new TNotifyEvent<TAoWBookWindow> OnDrag
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new TNotifyEvent<TAoWBookWindow> OnResize
    {
        get => EventSlot.Get(this, FOnResize);
        set => EventSlot.Set(this, ref FOnResize, value);
    }

    public new TNotifyEvent<TAoWBookWindow> OnInternalResize
    {
        get => EventSlot.Get(this, FOnInternalResize);
        set => EventSlot.Set(this, ref FOnInternalResize, value);
    }

    public new TNotifyEvent<TAoWBookWindow> OnHide
    {
        get => EventSlot.Get(this, FOnHide);
        set => EventSlot.Set(this, ref FOnHide, value);
    }

    public new TNotifyEvent<TAoWBookWindow> OnShow
    {
        get => EventSlot.Get(this, FOnShow);
        set => EventSlot.Set(this, ref FOnShow, value);
    }
}

public static partial class AoWBookWindowUnit
{
    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
