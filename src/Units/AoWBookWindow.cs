// Translated from AoWBookWindow.pas.
using Aow.Game.Draw;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Windows;

// completion event, first frame, previous surface validity, page turn, animating, saved page
// surface, page-turn mode
public partial class AoWBookWindow : AOWWindow
{
    public NotifyEvent<AoWBookWindow> FOnAnimationComplete;
    public bool FFirstAnimationFrame;
    public bool FPreviousSurfaceValid;
    public Grids.PageTurnControl FPageTurn;
    public bool FAnimating;
    public Surface FPageSurface;
    public Grids.PageTurnMode FPageTurnMode;

    // AoWBookWindow.pas:50
    public override void Dispose()
    {
        Surface S;
        S = FPageSurface;
        if (S != null)
        {
            S?.Dispose();
        }

        base.Dispose();
    }

    // AoWBookWindow.pas:44
    public AoWBookWindow(Component owner) : base(owner)
    {
        FPageTurn = new Grids.PageTurnControl();
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
        Surface S;
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
    public override void DrawSurface(Surface Surface)
    {
        FPreviousSurfaceValid = FSurfaceValid;
        base.DrawSurface(Surface);
        if (FPreviousSurfaceValid && FAnimating)
        {
            Blt(Surface);
        }
    }

    // AoWBookWindow.pas:186
    public override void CheckMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
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
    public override void CheckMouseUp(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        FHandled = FAnimating;
        if (!FAnimating)
        {
            base.CheckMouseUp(Button, Shift, X, Y);
        }
    }

    // AoWBookWindow.pas:205
    public override void CheckMouseMove(Set<ShiftState> Shift, int X, int Y)
    {
        FHandled = FAnimating;
        if (!FAnimating)
        {
            base.CheckMouseMove(Shift, X, Y);
        }
    }

    // AoWBookWindow.pas:92
    public void StartAnim(Grids.PageTurnMode Mode)
    {
        SurfaceDesc Source = default;
        SurfaceDesc Dest = default;
        Rect R;
        if (FBackSurface != null && FPageSurface != null)
        {
            R = new Rect(0, 0, FWinWidth, FWinHeight);
            FBackSurface.Lock(ref Source);
            FPageSurface.Lock(ref Dest);
            Aow.Game.Draw.SurfaceDesc.BltSDRectClipped(Source, R, Dest, 0, 0);
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
    public NotifyEvent<AoWBookWindow> OnAnimDone { get => FOnAnimationComplete; set => FOnAnimationComplete = value; }

    public new NotifyEvent<AoWBookWindow> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<AoWBookWindow> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<AoWBookWindow> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<AoWBookWindow> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<AoWBookWindow> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<AoWBookWindow> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<AoWBookWindow> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<AoWBookWindow> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<AoWBookWindow> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }

    public new AoWOnDrawBackEvent<AoWBookWindow> OnDrawBack
    {
        get => EventSlot.Get(this, FOnDrawBack);
        set => EventSlot.Set(this, ref FOnDrawBack, value);
    }

    public new NotifyEvent<AoWBookWindow> OnEnter
    {
        get => EventSlot.Get(this, FOnEnter);
        set => EventSlot.Set(this, ref FOnEnter, value);
    }

    public new NotifyEvent<AoWBookWindow> OnExit
    {
        get => EventSlot.Get(this, FOnExit);
        set => EventSlot.Set(this, ref FOnExit, value);
    }

    public new NotifyEvent<AoWBookWindow> OnChangeCursor
    {
        get => EventSlot.Get(this, FOnChangeCursor);
        set => EventSlot.Set(this, ref FOnChangeCursor, value);
    }

    public new AoWOnDrawSurface<AoWBookWindow> OnDrawSurface
    {
        get => EventSlot.Get(this, FOnDrawSurface);
        set => EventSlot.Set(this, ref FOnDrawSurface, value);
    }

    public new AoWOnDrawSurface<AoWBookWindow> OnPreDrawSurface
    {
        get => EventSlot.Get(this, FOnPreDrawSurface);
        set => EventSlot.Set(this, ref FOnPreDrawSurface, value);
    }

    public new NotifyEvent<AoWBookWindow> OnDrag
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new NotifyEvent<AoWBookWindow> OnResize
    {
        get => EventSlot.Get(this, FOnResize);
        set => EventSlot.Set(this, ref FOnResize, value);
    }

    public new NotifyEvent<AoWBookWindow> OnInternalResize
    {
        get => EventSlot.Get(this, FOnInternalResize);
        set => EventSlot.Set(this, ref FOnInternalResize, value);
    }

    public new NotifyEvent<AoWBookWindow> OnHide
    {
        get => EventSlot.Get(this, FOnHide);
        set => EventSlot.Set(this, ref FOnHide, value);
    }

    public new NotifyEvent<AoWBookWindow> OnShow
    {
        get => EventSlot.Get(this, FOnShow);
        set => EventSlot.Set(this, ref FOnShow, value);
    }
}
