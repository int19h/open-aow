// Translated from AoWBookWindow.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.AoWWindow;

namespace Aow.Units.AoWBookWindow;

// completion event, first frame, previous surface validity, page turn, animating, saved page
// surface, page-turn mode
public partial class TAoWBookWindow : TAOWWindow
{
    public TNotifyEvent FOnAnimationComplete;
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
    public TNotifyEvent OnAnimDone { get => FOnAnimationComplete; set => FOnAnimationComplete = value; }
}

public static partial class AoWBookWindow
{
    // AoWBookWindow.pas:211
    public static void Register()
    {
        RegisterComponents("AOW Tools", [typeof(TAoWBookWindow)]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
