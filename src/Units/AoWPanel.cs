// Translated from AoWPanel.pas.
using Aow.Rtl;
using Aow.Units.AoWBaseWin;
using Aow.Units.AoWComp;
using Aow.Units.AoWTabPanel;
using Aow.Units.AoWWinControl;
using Aow.Units.GFXE;
using static Aow.Rtl.RtlUnit;

namespace Aow.Units.AoWPanel;

// { The AoWPanel unit of aowInt.dpl: TAOWPanel, a window inside a window (AOWWindow), drawn into
//   its owner's surface. Its controls are positioned relative to it: while it updates, draws or
//   passes input on, it moves itself by the window's position and restores its own position
//   afterwards. A panel can belong to a tab of a TAOWTabPanel (TabPanel and OwnerItem): it is then
//   visible only while its tab is selected. }
// Fields: FOrgLeft, FOrgTop (the panel's own position, saved while it is moved by the window's
// position), FTabPanel, FAOWWindow
public partial class TAOWPanel : TAOWBaseWindow
{
    public int FOrgLeft;
    public int FOrgTop;
    public TAOWTabPanel FTabPanel;
    public TAOWBaseWindow FAOWWindow;

    // AoWPanel.pas:63
    public TAOWPanel(TComponent owner) : base(owner)
    {
        FCaptureInput = true;
    }

    // AoWPanel.pas:69
    public override void Destroy()
    {
        if (FTabPanel != null)
        {
            var indexOf = FTabPanel.FPanels.IndexOf(this);
            FTabPanel.FPanels.Delete(indexOf);
        }

        base.Destroy();
    }

    // AoWPanel.pas:235
    // { SetSize: at least as large as the frame corners, kept inside the window, at least MinWidth by
    //   MinHeight; the panel uses the window's window manager }
    public override void SetSize()
    {
        if (!FVisible && FTabPanel != null && FTabPanel.FSelected == FOwnerItem)
        {
            FVisible = true;
        }

        switch (FWindowType)
        {
            case TWindowType.winNormal:
            case TWindowType.winHollow:
                var ilFrameWidth = FILFrame[FILIndexFrame];
                var ilFrame = FILFrame[FILIndexFrame + 2];
                if (ilFrameWidth.Width + ilFrame.Width > FWinWidth)
                {
                    var ilFrame2 = FILFrame[FILIndexFrame];
                    var ilFrame3 = FILFrame[FILIndexFrame + 2];
                    FWinWidth = ilFrame2.Width + ilFrame3.Width;
                }

                var ilFrameHeight = FILFrame[FILIndexFrame];
                var ilFrame4 = FILFrame[FILIndexFrame + 5];
                if (ilFrameHeight.Height + ilFrame4.Height > FWinHeight)
                {
                    var ilFrame2 = FILFrame[FILIndexFrame];
                    var ilFrame3 = FILFrame[FILIndexFrame + 5];
                    FWinHeight = ilFrame2.Height + ilFrame3.Height;
                }

                if (FAOWWindow.FWinWidth < FWinLeft + FWinWidth)
                {
                    FWinLeft = FAOWWindow.FWinWidth - FWinWidth;
                }

                if (FWinLeft < 0)
                {
                    FWinLeft = 0;
                    if (FAOWWindow.FWinWidth < FWinLeft + FWinWidth)
                    {
                        FWinWidth = FAOWWindow.FWinWidth - FWinLeft;
                    }
                }

                if (FAOWWindow.FWinHeight < FWinTop + FWinHeight)
                {
                    FWinTop = FAOWWindow.FWinHeight - FWinHeight;
                }

                if (FWinTop < 0)
                {
                    FWinTop = 0;
                    if (FAOWWindow.FWinHeight < FWinTop + FWinHeight)
                    {
                        FWinHeight = FAOWWindow.FWinHeight - FWinTop;
                    }
                }

                break;
            case TWindowType.winBorderless:
            case TWindowType.winBlank:
                if (FAOWWindow.FWinWidth < FWinLeft + FWinWidth)
                {
                    FWinLeft = FAOWWindow.FWinWidth - FWinWidth;
                }

                if (FWinLeft < 0)
                {
                    FWinLeft = 0;
                    if (FAOWWindow.FWinWidth < FWinLeft + FWinWidth)
                    {
                        FWinWidth = FAOWWindow.FWinWidth - FWinLeft;
                    }
                }

                if (FAOWWindow.FWinHeight < FWinTop + FWinHeight)
                {
                    FWinTop = FAOWWindow.FWinHeight - FWinHeight;
                }

                if (FWinTop < 0)
                {
                    FWinTop = 0;
                    if (FAOWWindow.FWinHeight < FWinTop + FWinHeight)
                    {
                        FWinHeight = FAOWWindow.FWinHeight - FWinTop;
                    }
                }

                break;
        }

        if (FMinWidth > 0 && FMinWidth > FWinWidth)
        {
            FWinWidth = FMinWidth;
        }

        if (FMinHeight > 0 && FMinHeight > FWinHeight)
        {
            FWinHeight = FMinHeight;
        }

        if (FAOWWindow.FWinManager != null)
        {
            FWinManager = FAOWWindow.FWinManager;
        }

        FSurfaceValid = false;
        if (FWindowType == TWindowType.winBlank)
        {
            FMasked = true;
        }

        base.SetSize();
    }

    // AoWPanel.pas:297
    // { Setup: like TAoWComponent.Setup, with the controls set up while the panel is moved by the
    //   position of a panel it sits in }
    public override void Setup()
    {
        try
        {
            if (!FValid)
            {
                Validate();
            }

            if (FValid)
            {
                if (!FSized)
                {
                    SetSize();
                }

                if (!FAligned)
                {
                    ReAlign();
                }

                if (!FSized)
                {
                    SetSize();
                }

                FOrgLeft = FWinLeft;
                FOrgTop = FWinTop;
                if (FAOWWindow is TAOWPanel)
                {
                    FWinLeft += FAOWWindow.FWinLeft;
                    FWinTop += FAOWWindow.FWinTop;
                }

                for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
                {
                    ((TAoWComponent)FChildren[I]).Setup();
                }

                FWinLeft = FOrgLeft;
                FWinTop = FOrgTop;
                FBltCount = 0;
                FDrawCount = 0;
            }
        }
        catch
        {
            MessageBox(0, "Exception during Setup", "Error - " + Name, 0);
        }
    }

    // AoWPanel.pas:333
    // { Update: the controls are updated with the panel moved by its window's position; when the
    //   panel moved or changed size, its old rectangle is redrawn in the owner }
    public override void Update()
    {
        TRect R = default;
        if (FVisible)
        {
            FOrgLeft = FWinLeft;
            FOrgTop = FWinTop;
            if (FAOWWindow is TAOWPanel)
            {
                FWinLeft += FAOWWindow.FWinLeft;
                FWinTop += FAOWWindow.FWinTop;
            }

            if (FOnUpdate != null)
            {
                FOnUpdate(this);
            }

            for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
            {
                ((TAoWComponent)FChildren[I]).Update();
            }

            FWinLeft = FOrgLeft;
            FWinTop = FOrgTop;
            if (FOldRect.Right > 0 && FOldRect.Bottom > 0 && (FOldRect.Left != FWinLeft || FOldRect.Top != FWinTop || FOldRect.Right != FWinWidth || FOldRect.Bottom != FWinHeight))
            {
                if (FAOWOwner.FAOWOwner.FAOWOwner == null)
                {
                    R = Rect(FOldRect.Left, FOldRect.Top, FOldRect.Left + FOldRect.Right - 1, FOldRect.Top + FOldRect.Bottom - 1);
                }
                else
                {
                    R = Rect(FAOWOwner.FWinLeft + FOldRect.Left, FAOWOwner.FWinTop + FOldRect.Top, FAOWOwner.FWinLeft + FOldRect.Left + FOldRect.Right - 1, FAOWOwner.FWinTop + FOldRect.Top + FOldRect.Bottom - 1);
                }

                if (FMasked)
                {
                    FAOWOwner.AddDrawRect(ref R, this);
                }
                else
                {
                    FAOWOwner.AddBltRect(ref R, this);
                }
            }

            FOldRect.Left = FWinLeft;
            FOldRect.Top = FWinTop;
            FOldRect.Right = FWinWidth;
            FOldRect.Bottom = FWinHeight;
        }
        else
        {
            if (FHidden)
            {
                if (FAOWOwner.FAOWOwner.FAOWOwner == null)
                {
                    R = Rect(FWinLeft, FWinTop, FWinLeft + FWinWidth - 1, FWinTop + FWinHeight - 1);
                }
                else
                {
                    R = Rect(FAOWOwner.FWinLeft + FWinLeft, FAOWOwner.FWinTop + FWinTop, FAOWOwner.FWinLeft + FWinLeft + FWinWidth - 1, FAOWOwner.FWinTop + FWinTop + FWinHeight - 1);
                }

                FAOWOwner.AddDrawRect(ref R, this);
                FHidden = false;
            }
        }

        FSurfaceValid = FDrawn;
        FDrawn = false;
        FRedraw = true;
    }

    // AoWPanel.pas:97
    // { InvalidateRect: the controls from Index on that ARect touches are redrawn (windows are asked to
    //   invalidate the rectangle themselves), then the windows above the panel in its owner }
    public override void InvalidateRect(int Index, ref TRect ARect, TAoWComponent Sender)
    {
        int OldLeft;
        int OldTop;
        TAoWComponent C;
        TRect R = default;
        if (FValid && FVisible && FWindowType != TWindowType.winBlank)
        {
            AddDrawRect(ref ARect, Sender);
        }

        OldLeft = FWinLeft;
        OldTop = FWinTop;
        FWinLeft += FAOWOwner.FWinLeft;
        FWinTop += FAOWOwner.FWinTop;
        for (int I = Index, last = FChildren.Count - 1; I <= last; I++)
        {
            C = (TAoWComponent)FChildren[I];
            if (C.FVisible)
            {
                if (((FWinLeft + C.FWinLeft >= ARect.Left && FWinLeft + C.FWinLeft <= ARect.Right) || (FWinLeft + C.FWinLeft <= ARect.Left && ARect.Left <= FWinLeft + C.FWinLeft + C.FWinWidth)) && ((FWinTop + C.FWinTop >= ARect.Top && FWinTop + C.FWinTop <= ARect.Bottom) || (FWinTop + C.FWinTop <= ARect.Top && ARect.Top <= FWinTop + C.FWinTop + C.FWinHeight)) && C != Sender)
                {
                    if (C is TAOWBaseWindow)
                    {
                        C.InvalidateRect(0, ref ARect, this);
                    }
                    else
                    {
                        if (C.FDrawn)
                        {
                            C.FDrawn = false;
                            if (C.FMasked)
                            {
                                R = Rect(FWinLeft + C.FWinLeft, FWinTop + C.FWinTop, FWinLeft + C.FWinLeft + C.FWinWidth, FWinTop + C.FWinTop + C.FWinHeight);
                                AddDrawRect(ref R, this);
                            }
                        }
                    }
                }
            }
        }

        if (FAOWOwner != Sender)
        {
            var indexOf = FAOWOwner.FChildren.IndexOf(this);
            FAOWOwner.InvalidateRect(indexOf + 1, ref ARect, this);
        }

        FWinLeft = OldLeft;
        FWinTop = OldTop;
    }

    // AoWPanel.pas:82
    public override void AddBltRect(ref TRect ARect, TAoWComponent Sender)
    {
        FAOWOwner.AddBltRect(ref ARect, this);
    }

    // AoWPanel.pas:87
    public override void AddDrawRect(ref TRect ARect, TAoWComponent Sender)
    {
        if (FMasked)
        {
            FAOWOwner.AddDrawRect(ref ARect, this);
        }

        if (FWindowType != TWindowType.winBlank)
        {
            base.AddDrawRect(ref ARect, Sender);
        }
    }

    // AoWPanel.pas:76
    public override void SetOwner(TAoWComponent Value)
    {
        base.SetOwner(Value);
        FAOWWindow = (TAOWBaseWindow)FAOWOwner;
    }

    // AoWPanel.pas:191
    // { SetVisible: showing sets the back rectangle and marks the panel hidden (original bug: FHidden
    //   set instead of cleared) }
    public override void SetVisible(bool Value)
    {
        if (Value != FVisible)
        {
            FVisible = Value;
            if (!FVisible)
            {
                FHidden = true;
                if (FOnHide != null)
                {
                    FOnHide(this);
                }
            }
            else
            {
                FDrawn = false;
                if (!FHidden)
                {
                    FBackRect.Right = FWinWidth;
                    FBackRect.Bottom = FWinHeight;
                    FBackRect.Top = FWinTop;
                    FBackRect.Left = FWinLeft;
                    FHidden = true;
                }

                if (FOnShow != null)
                {
                    FOnShow(this);
                }
            }
        }

        if (FVisible)
        {
            if (!FValid || !FAligned || !FSized)
            {
                Setup();
            }

            ClearBltList();
            ClearDrawList();
            FDrawn = false;
            SetPriority(FPriority);
        }
    }

    // AoWPanel.pas:391
    // { Draw: the background (redrawn only in the dirty rectangles once the surface is valid), then
    //   the controls that need it; the rectangles of redrawn controls are passed on to the controls
    //   above them and blitted }
    public override void Draw(TSurfaceDesc SurfDesc)
    {
        TRect BltRect = default;
        TRect OldClip = default;
        TAoWComponent C = default;
        TaowRect D = default;
        void DrawBack()
        {
            if (FWindowType != TWindowType.winBlank)
            {
                switch (FWindowType)
                {
                    case TWindowType.winNormal:
                        var rect = Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                        FILFrame.DrawILF(FILIndexFrame, rect, ref SurfDesc);
                        var ilFrameHeight = FILFrame[FILIndexFrame + 1];
                        var ilFrame = FILFrame[FILIndexFrame + 6];
                        var ilFrameWidth = FILFrame[FILIndexFrame + 3];
                        var ilFrame2 = FILFrame[FILIndexFrame + 4];
                        var ilFrame3 = FILFrame[FILIndexFrame + 1];
                        var ilFrame4 = FILFrame[FILIndexFrame + 3];
                        var rect2 = Rect(FWinLeft + ilFrame4.Width, FWinTop + ilFrame3.Height, FWinWidth - ilFrameWidth.Width - ilFrame2.Width, FWinHeight - ilFrameHeight.Height - ilFrame.Height);
                        FILTexture.DrawILT(FILIndexTexture, rect2, ref SurfDesc);
                        break;
                    case TWindowType.winHollow:
                        var rect3 = Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                        FILFrame.DrawILF(FILIndexFrame, rect3, ref SurfDesc);
                        break;
                    case TWindowType.winBorderless:
                        var rect4 = Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                        FILTexture.DrawILT(FILIndexTexture, rect4, ref SurfDesc);
                        break;
                }

                BltRect = Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, SurfDesc.ClipRect.Right, SurfDesc.ClipRect.Bottom);
                FAOWOwner.AddBltRect(ref BltRect, this);
            }
        }

        try
        {
            if (FValid && FVisible)
            {
                FOrgLeft = FWinLeft;
                FOrgTop = FWinTop;
                FWinLeft += FAOWWindow.FWinLeft;
                FWinTop += FAOWWindow.FWinTop;
                if (FWinLeft < SurfDesc.ClipRect.Right && FWinLeft > SurfDesc.ClipRect.Left)
                {
                    SurfDesc.ClipRect.Left = FWinLeft;
                }

                if (FWinTop < SurfDesc.ClipRect.Bottom && FWinTop > SurfDesc.ClipRect.Top)
                {
                    SurfDesc.ClipRect.Top = FWinTop;
                }

                if (FWinLeft + FWinWidth > SurfDesc.ClipRect.Left && FWinLeft + FWinWidth < SurfDesc.ClipRect.Right)
                {
                    SurfDesc.ClipRect.Right = FWinLeft + FWinWidth;
                }

                if (FWinTop + FWinHeight > SurfDesc.ClipRect.Top && FWinTop + FWinHeight < SurfDesc.ClipRect.Bottom)
                {
                    SurfDesc.ClipRect.Bottom = FWinTop + FWinHeight;
                }

                if (FSurfaceValid)
                {
                    for (int J = 0, last = FDrawCount - 1; J <= last; J++)
                    {
                        D = (TaowRect)FDrawList[J];
                        OldClip = SurfDesc.ClipRect;
                        var rect = Rect(D.R.Left, D.R.Top, D.R.Right + 1, D.R.Bottom + 1);
                        SurfDesc.ClipRect = rect;
                        DrawBack();
                        SurfDesc.ClipRect = OldClip;
                        FWinLeft = FOrgLeft;
                        FWinTop = FOrgTop;
                        InvalidateRect(0, ref D.R, D.Owner);
                        FWinLeft += FAOWWindow.FWinLeft;
                        FWinTop += FAOWWindow.FWinTop;
                    }
                }
                else
                {
                    DrawBack();
                    if (FOnDrawBack != null)
                    {
                        FOnDrawBack(this, SurfDesc);
                    }
                }

                FDrawCount = 0;
                FDrawn = FSurfaceValid;
                for (int I = 0, lastI = FChildren.Count - 1; I <= lastI; I++)
                {
                    C = (TAoWComponent)FChildren[I];
                    if (C.FValid && C.FVisible && (!FDrawn || !C.FDrawn))
                    {
                        C.FDrawn = false;
                        if (!FDrawn && C is TAOWBaseWindow)
                        {
                            C.FSurfaceValid = false;
                        }

                        C.Draw(SurfDesc);
                        C.FFirstDraw = C.FDrawn;
                        C.FDrawn = true;
                        for (int J = 0, last = FDrawCount - 1; J <= last; J++)
                        {
                            D = (TaowRect)FDrawList[J];
                            InvalidateRect(I + 1, ref D.R, null);
                        }

                        FDrawCount = 0;
                        if (!FDrawn || !C.FFirstDraw)
                        {
                            BltRect = Rect(FWinLeft + C.FWinLeft, FWinTop + C.FWinTop, FWinLeft + C.FWinLeft + C.FWinWidth, FWinTop + C.FWinTop + C.FWinHeight);
                            FAOWOwner.AddBltRect(ref BltRect, this);
                            FWinLeft = FOrgLeft;
                            FWinTop = FOrgTop;
                            BltRect = Rect(FAOWWindow.FWinLeft + FWinLeft + C.FWinLeft, FAOWWindow.FWinTop + FWinTop + C.FWinTop, FAOWWindow.FWinLeft + FWinLeft + C.FWinLeft + C.FWinWidth - 1, FAOWWindow.FWinTop + FWinTop + C.FWinTop + C.FWinHeight - 1);
                            InvalidateRect(I + 1, ref BltRect, C);
                            FWinLeft += FAOWWindow.FWinLeft;
                            FWinTop += FAOWWindow.FWinTop;
                        }
                    }
                }

                if (FOnDraw != null)
                {
                    FOnDraw(this, SurfDesc);
                }

                FDrawn = FSurfaceValid;
                SurfDesc.ClipRect = OldClip;
                FWinLeft = FOrgLeft;
                FWinTop = FOrgTop;
                FSurfaceValid = true;
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AoWPanel.pas:513
    // { CheckMouseDown: the controls first (from the top), with the panel moved by its window's
    //   position }
    public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        TAoWComponent C = default;
        FHandled = false;
        if (FVisible == true)
        {
            FOrgLeft = FWinLeft;
            FOrgTop = FWinTop;
            if (FAOWWindow != null)
            {
                FWinLeft += FAOWWindow.FWinLeft;
                FWinTop += FAOWWindow.FWinTop;
                FHandled = FAOWWindow.FHandled;
            }

            for (int I = FChildren.Count - 1; I >= 0; I--)
            {
                C = (TAoWComponent)FChildren[I];
                C.FHandled = false;
                if ((!FHandled || C.FRedraw) && C.FVisible == true && C.FCaptureInput)
                {
                    C.CheckMouseDown(Button, Shift, X, Y);
                }

                if (C.FHandled == true)
                {
                    FHandled = true;
                }

                if (C.FHandled == true)
                {
                    C.FRedraw = true;
                }
            }

            if (X > FWinLeft - 1 && X < FWinLeft + FWinWidth && Y > FWinTop - 1 && Y < FWinTop + FWinHeight)
            {
                FHandled = true;
                FRedraw = true;
                FWinLeft = FOrgLeft;
                FWinTop = FOrgTop;
                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }
            }
            else
            {
                FWinLeft = FOrgLeft;
                FWinTop = FOrgTop;
            }
        }
    }

    // AoWPanel.pas:558
    public override void CheckMouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        TAoWComponent C = default;
        FHandled = false;
        if (FVisible == true)
        {
            FOrgLeft = FWinLeft;
            FOrgTop = FWinTop;
            if (FAOWWindow != null)
            {
                FWinLeft += FAOWWindow.FWinLeft;
                FWinTop += FAOWWindow.FWinTop;
                FHandled = FAOWWindow.FHandled;
            }

            for (int I = FChildren.Count - 1; I >= 0; I--)
            {
                C = (TAoWComponent)FChildren[I];
                C.FHandled = false;
                if ((!FHandled || C.FRedraw == true) && C.FVisible == true && C.FCaptureInput)
                {
                    C.CheckMouseUp(Button, Shift, X, Y);
                }

                if (C.FHandled == true)
                {
                    FHandled = true;
                }
            }

            if (X > FWinLeft - 1 && X < FWinLeft + FWinWidth && Y > FWinTop - 1 && Y < FWinTop + FWinHeight)
            {
                FHandled = true;
                FWinLeft = FOrgLeft;
                FWinTop = FOrgTop;
                if (FOnMouseUp != null)
                {
                    FOnMouseUp(this, Button, Shift, X, Y);
                }
            }
            else
            {
                FWinLeft = FOrgLeft;
                FWinTop = FOrgTop;
            }
        }
    }

    // AoWPanel.pas:600
    public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        TAoWComponent C = default;
        FHandled = false;
        if (FVisible == true)
        {
            FOrgLeft = FWinLeft;
            FOrgTop = FWinTop;
            if (FAOWWindow != null)
            {
                FWinLeft += FAOWWindow.FWinLeft;
                FWinTop += FAOWWindow.FWinTop;
                FHandled = FAOWWindow.FHandled;
            }

            for (int I = FChildren.Count - 1; I >= 0; I--)
            {
                C = (TAoWComponent)FChildren[I];
                C.FHandled = false;
                if ((!FHandled || C.FRedraw == true || C.FToolTipCount > 0) && C.FVisible == true && C.FCaptureInput)
                {
                    C.CheckMouseMove(Shift, X, Y);
                }

                if (C.FHandled == true)
                {
                    FHandled = true;
                }
            }

            if (X > FWinLeft - 1 && X < FWinLeft + FWinWidth && Y > FWinTop - 1 && Y < FWinTop + FWinHeight)
            {
                FHandled = true;
                FWinLeft = FOrgLeft;
                FWinTop = FOrgTop;
                if (FOnMouseMove != null)
                {
                    FOnMouseMove(this, Shift, X, Y);
                }
            }
            else
            {
                FWinLeft = FOrgLeft;
                FWinTop = FOrgTop;
            }
        }
    }

    // AoWPanel.pas:138
    public override void DetachMenuBar()
    {
        FTabPanel = null;
    }

    // AoWPanel.pas:145
    // { SetTabPanel moves the panel from the panel list of its old tab panel to the list of the new
    //   one, in priority order }
    public virtual void SetTabPanel(TAOWTabPanel Value)
    {
        int I;
        int NewIndex;
        TAOWPanel P;
        if (Value != FTabPanel)
        {
            if (FTabPanel != null)
            {
                I = 0;
                while (I < FTabPanel.FPanels.Count)
                {
                    P = (TAOWPanel)FTabPanel.FPanels[I];
                    if (P == this)
                    {
                        FTabPanel.FPanels.Delete(I);
                        I = FTabPanel.FPanels.Count;
                    }

                    I += 1;
                }
            }

            FTabPanel = Value;
            if (FTabPanel != null)
            {
                I = 0;
                NewIndex = -1;
                while (I < FTabPanel.FPanels.Count)
                {
                    P = (TAOWPanel)FTabPanel.FPanels[I];
                    if (P.FPriority > FPriority)
                    {
                        NewIndex = I;
                        I = FTabPanel.FPanels.Count;
                    }

                    I += 1;
                }

                if (NewIndex == -1)
                {
                    FTabPanel.FPanels.Add(this);
                }
                else
                {
                    FTabPanel.FPanels.Insert(NewIndex, this);
                }
            }
        }
    }

    // AoWPanel.pas:228
    public void SetAOWWindow(TAOWBaseWindow Value)
    {
        SetOwner(Value);
    }

    [Published]
    public TAOWBaseWindow AOWWindow { get => FAOWWindow; set => SetAOWWindow(value); }

    [Published]
    public TAOWTabPanel TabPanel { get => FTabPanel; set => SetTabPanel(value); }

    [Published]
    public int OwnerItem { get => FOwnerItem; set => FOwnerItem = value; }

    [Published]
    public int MinWidth { get => FMinWidth; set => FMinWidth = value; }

    [Published]
    public int MinHeight { get => FMinHeight; set => FMinHeight = value; }

    public new TNotifyEvent<TAOWPanel> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new TMouseEvent<TAOWPanel> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new TMouseEvent<TAOWPanel> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new TMouseMoveEvent<TAOWPanel> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new TKeyPressEvent<TAOWPanel> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new TKeyEvent<TAOWPanel> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new TAoWOnDrawEvent<TAOWPanel> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new TAoWOnDrawEvent<TAOWPanel> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new TNotifyEvent<TAOWPanel> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }

    public new TAoWOnDrawBackEvent<TAOWPanel> OnDrawBack
    {
        get => EventSlot.Get(this, FOnDrawBack);
        set => EventSlot.Set(this, ref FOnDrawBack, value);
    }

    public new TNotifyEvent<TAOWPanel> OnEnter
    {
        get => EventSlot.Get(this, FOnEnter);
        set => EventSlot.Set(this, ref FOnEnter, value);
    }

    public new TNotifyEvent<TAOWPanel> OnExit
    {
        get => EventSlot.Get(this, FOnExit);
        set => EventSlot.Set(this, ref FOnExit, value);
    }

    public new TNotifyEvent<TAOWPanel> OnHide
    {
        get => EventSlot.Get(this, FOnHide);
        set => EventSlot.Set(this, ref FOnHide, value);
    }

    public new TNotifyEvent<TAOWPanel> OnShow
    {
        get => EventSlot.Get(this, FOnShow);
        set => EventSlot.Set(this, ref FOnShow, value);
    }
}

public static partial class AoWPanelUnit
{
}
