// Translated from AoWBookWindow.pas.
using Aow.Game.Draw;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Grids;
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

// completion event, first frame, previous surface validity, page turn, animating, saved page
// surface, page-turn mode
public partial class BookWindow : Window
{
    public NotifyEvent<BookWindow> FOnAnimationComplete;
    public bool FFirstAnimationFrame;
    public bool FPreviousSurfaceValid;
    public PageTurnControl FPageTurn;
    public bool FAnimating;
    public Surface FPageSurface;
    public PageTurnMode FPageTurnMode;

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
    public BookWindow(Component owner) : base(owner)
    {
        FPageTurn = new PageTurnControl();
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
    public void StartAnim(PageTurnMode Mode)
    {
        LockedSurface Source = default;
        LockedSurface Dest = default;
        Rect R;
        if (FBackSurface != null && FPageSurface != null)
        {
            R = new Rect(0, 0, FWinWidth, FWinHeight);
            FBackSurface.Lock(ref Source);
            FPageSurface.Lock(ref Dest);
            LockedSurface.BltSDRectClipped(Source, R, Dest, 0, 0);
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
    public NotifyEvent<BookWindow> OnAnimDone { get => FOnAnimationComplete; set => FOnAnimationComplete = value; }

    public new NotifyEvent<BookWindow> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<BookWindow> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<BookWindow> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<BookWindow> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<BookWindow> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<BookWindow> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<BookWindow> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<BookWindow> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<BookWindow> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }

    public new AoWOnDrawBackEvent<BookWindow> OnDrawBack
    {
        get => EventSlot.Get(this, FOnDrawBack);
        set => EventSlot.Set(this, ref FOnDrawBack, value);
    }

    public new NotifyEvent<BookWindow> OnEnter
    {
        get => EventSlot.Get(this, FOnEnter);
        set => EventSlot.Set(this, ref FOnEnter, value);
    }

    public new NotifyEvent<BookWindow> OnExit
    {
        get => EventSlot.Get(this, FOnExit);
        set => EventSlot.Set(this, ref FOnExit, value);
    }

    public new NotifyEvent<BookWindow> OnChangeCursor
    {
        get => EventSlot.Get(this, FOnChangeCursor);
        set => EventSlot.Set(this, ref FOnChangeCursor, value);
    }

    public new AoWOnDrawSurface<BookWindow> OnDrawSurface
    {
        get => EventSlot.Get(this, FOnDrawSurface);
        set => EventSlot.Set(this, ref FOnDrawSurface, value);
    }

    public new AoWOnDrawSurface<BookWindow> OnPreDrawSurface
    {
        get => EventSlot.Get(this, FOnPreDrawSurface);
        set => EventSlot.Set(this, ref FOnPreDrawSurface, value);
    }

    public new NotifyEvent<BookWindow> OnDrag
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new NotifyEvent<BookWindow> OnResize
    {
        get => EventSlot.Get(this, FOnResize);
        set => EventSlot.Set(this, ref FOnResize, value);
    }

    public new NotifyEvent<BookWindow> OnInternalResize
    {
        get => EventSlot.Get(this, FOnInternalResize);
        set => EventSlot.Set(this, ref FOnInternalResize, value);
    }

    public new NotifyEvent<BookWindow> OnHide
    {
        get => EventSlot.Get(this, FOnHide);
        set => EventSlot.Set(this, ref FOnHide, value);
    }

    public new NotifyEvent<BookWindow> OnShow
    {
        get => EventSlot.Get(this, FOnShow);
        set => EventSlot.Set(this, ref FOnShow, value);
    }
}
