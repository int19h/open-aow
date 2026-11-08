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
    public override void Destroy()
    {
        TSurface S;
        FPageTurn.Free();
        S = FPageSurface;
        if (S != null)
        {
            S.Free();
        }

        base.Destroy();
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
                    S.Free();
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
        get => EventSlot.Get(this, "TAoWBookWindow.FOnChange", FOnChange, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnChange", ref FOnChange, value, handler => sender => handler(this));
    }

    public new TMouseEvent<TAoWBookWindow> OnMouseDown
    {
        get => EventSlot.Get(this, "TAoWBookWindow.FOnMouseDown", FOnMouseDown, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnMouseDown", ref FOnMouseDown, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseEvent<TAoWBookWindow> OnMouseUp
    {
        get => EventSlot.Get(this, "TAoWBookWindow.FOnMouseUp", FOnMouseUp, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnMouseUp", ref FOnMouseUp, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseMoveEvent<TAoWBookWindow> OnMouseMove
    {
        get => EventSlot.Get(this, "TAoWBookWindow.FOnMouseMove", FOnMouseMove, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnMouseMove", ref FOnMouseMove, value, handler => (sender, shift, x, y) => handler(this, shift, x, y));
    }

    public new TKeyPressEvent<TAoWBookWindow> OnKeyPress
    {
        get => EventSlot.Get(this, "TAoWBookWindow.FOnKeyPress", FOnKeyPress, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnKeyPress", ref FOnKeyPress, value, handler => (sender, ref key) => handler(this, ref key));
    }

    public new TKeyEvent<TAoWBookWindow> OnKeyDown
    {
        get => EventSlot.Get(this, "TAoWBookWindow.FOnKeyDown", FOnKeyDown, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnKeyDown", ref FOnKeyDown, value, handler => (sender, ref key, shift) => handler(this, ref key, shift));
    }

    public new TAoWOnDrawEvent<TAoWBookWindow> OnPreDraw
    {
        get => EventSlot.Get(this, "TAoWBookWindow.FOnPreDraw", FOnPreDraw, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnPreDraw", ref FOnPreDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TAoWOnDrawEvent<TAoWBookWindow> OnDraw
    {
        get => EventSlot.Get(this, "TAoWBookWindow.FOnDraw", FOnDraw, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnDraw", ref FOnDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TNotifyEvent<TAoWBookWindow> OnUpdate
    {
        get => EventSlot.Get(this, "TAoWBookWindow.FOnUpdate", FOnUpdate, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnUpdate", ref FOnUpdate, value, handler => sender => handler(this));
    }

    public new TAoWOnDrawBackEvent<TAoWBookWindow> OnDrawBack
    {
        get => EventSlot.Get(this, "TAoWBookWindow.FOnDrawBack", FOnDrawBack, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnDrawBack", ref FOnDrawBack, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TNotifyEvent<TAoWBookWindow> OnEnter
    {
        get => EventSlot.Get(this, "TAoWBookWindow.FOnEnter", FOnEnter, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnEnter", ref FOnEnter, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TAoWBookWindow> OnExit
    {
        get => EventSlot.Get(this, "TAoWBookWindow.FOnExit", FOnExit, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnExit", ref FOnExit, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TAoWBookWindow> OnChangeCursor
    {
        get => EventSlot.Get(this, "TAoWBookWindow.FOnChangeCursor", FOnChangeCursor, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnChangeCursor", ref FOnChangeCursor, value, handler => sender => handler(this));
    }

    public new TAoWOnDrawSurface<TAoWBookWindow> OnDrawSurface
    {
        get => EventSlot.Get(this, "TAoWBookWindow.FOnDrawSurface", FOnDrawSurface, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnDrawSurface", ref FOnDrawSurface, value, handler => (sender, surface, left, top) => handler(this, surface, left, top));
    }

    public new TAoWOnDrawSurface<TAoWBookWindow> OnPreDrawSurface
    {
        get => EventSlot.Get(this, "TAoWBookWindow.FOnPreDrawSurface", FOnPreDrawSurface, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnPreDrawSurface", ref FOnPreDrawSurface, value, handler => (sender, surface, left, top) => handler(this, surface, left, top));
    }

    public new TNotifyEvent<TAoWBookWindow> OnDrag
    {
        get => EventSlot.Get(this, "TAoWBookWindow.FOnChange", FOnChange, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnChange", ref FOnChange, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TAoWBookWindow> OnResize
    {
        get => EventSlot.Get(this, "TAoWBookWindow.FOnResize", FOnResize, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnResize", ref FOnResize, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TAoWBookWindow> OnInternalResize
    {
        get => EventSlot.Get(this, "TAoWBookWindow.FOnInternalResize", FOnInternalResize, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnInternalResize", ref FOnInternalResize, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TAoWBookWindow> OnHide
    {
        get => EventSlot.Get(this, "TAoWBookWindow.FOnHide", FOnHide, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnHide", ref FOnHide, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TAoWBookWindow> OnShow
    {
        get => EventSlot.Get(this, "TAoWBookWindow.FOnShow", FOnShow, static slot => slot);
        set => EventSlot.Set(this, "TAoWBookWindow.FOnShow", ref FOnShow, value, handler => sender => handler(this));
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
