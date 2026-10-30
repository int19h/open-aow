// Translated from AoWPanel.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.AoWTabPanel;
using static Aow.Units.AoWTabPanel.AoWTabPanel;
using Aow.Units.AOWToolTip;
using static Aow.Units.AOWToolTip.AOWToolTip;
using Aow.Units.AoWControl;
using static Aow.Units.AoWControl.AoWControl;
using Aow.Units.AOWWinManager;
using static Aow.Units.AOWWinManager.AOWWinManager;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AOWInterfaceImgLib;
using static Aow.Units.AOWInterfaceImgLib.AOWInterfaceImgLib;
using Aow.Units.AoWWinControl;
using static Aow.Units.AoWWinControl.AoWWinControl;
using Aow.Units.AoWBaseWin;
using static Aow.Units.AoWBaseWin.AoWBaseWin;

namespace Aow.Units.AoWPanel
{
    // { The AoWPanel unit of aowInt.dpl: TAOWPanel, a window inside a window (AOWWindow), drawn into
//   its owner's surface. Its controls are positioned relative to it: while it updates, draws or
//   passes input on, it moves itself by the window's position and restores its own position
//   afterwards. A panel can belong to a tab of a TAOWTabPanel (TabPanel and OwnerItem): it is then
//   visible only while its tab is selected. }
    // { AoWPanel.TAOWPanel (instance size 508). Fields: +$1EC FOrgLeft, +$1F0 FOrgTop (the panel's
//     own position, saved while it is moved by the window's position), +$1F4 FTabPanel, +$1F8
//     FAOWWindow.
//     VMT (new): SetTabPanel }
    public partial class TAOWPanel : Aow.Units.AoWBaseWin.TAOWBaseWindow
    {
        public int FOrgLeft;
        public int FOrgTop;
        public Aow.Units.AoWTabPanel.TAOWTabPanel FTabPanel;
        public Aow.Units.AoWBaseWin.TAOWBaseWindow FAOWWindow;
        // AoWPanel.pas:63
        // { TAOWPanel }
        public TAOWPanel(TComponent owner) : base(owner)
        {
            FCaptureInput = true;
        }

        // AoWPanel.pas:69
        public override void Destroy()
        {
            if (FTabPanel != null)
            {
                var call__1 = FTabPanel.FPanels.IndexOf(this);
                FTabPanel.FPanels.Delete(call__1);
            }

            base.Destroy();
        }

        // AoWPanel.pas:235
        // { SetSize: at least as large as the frame corners, kept inside the window, at least MinWidth by
//   MinHeight; the panel uses the window's window manager }
        public override void SetSize()
        {
            if ((!FVisible && (FTabPanel != null)) && (FTabPanel.FSelected == FOwnerItem))
            {
                FVisible = true;
            }

            switch (FWindowType)
            {
                case Aow.Units.AoWWinControl.TWindowType.winNormal:
                case Aow.Units.AoWWinControl.TWindowType.winHollow:
                    var call__2 = FILFrame[FILIndexFrame];
                    var call__3 = FILFrame[FILIndexFrame + 2];
                    if ((call__2.Width + call__3.Width) > FWinWidth)
                    {
                        var call__4 = FILFrame[FILIndexFrame];
                        var call__5 = FILFrame[FILIndexFrame + 2];
                        FWinWidth = call__4.Width + call__5.Width;
                    }

                    var call__6 = FILFrame[FILIndexFrame];
                    var call__7 = FILFrame[FILIndexFrame + 5];
                    if ((call__6.Height + call__7.Height) > FWinHeight)
                    {
                        var call__8 = FILFrame[FILIndexFrame];
                        var call__9 = FILFrame[FILIndexFrame + 5];
                        FWinHeight = call__8.Height + call__9.Height;
                    }

                    if (FAOWWindow.FWinWidth < (FWinLeft + FWinWidth))
                    {
                        FWinLeft = FAOWWindow.FWinWidth - FWinWidth;
                    }

                    if (FWinLeft < 0)
                    {
                        FWinLeft = 0;
                        if (FAOWWindow.FWinWidth < (FWinLeft + FWinWidth))
                        {
                            FWinWidth = FAOWWindow.FWinWidth - FWinLeft;
                        }
                    }

                    if (FAOWWindow.FWinHeight < (FWinTop + FWinHeight))
                    {
                        FWinTop = FAOWWindow.FWinHeight - FWinHeight;
                    }

                    if (FWinTop < 0)
                    {
                        FWinTop = 0;
                        if (FAOWWindow.FWinHeight < (FWinTop + FWinHeight))
                        {
                            FWinHeight = FAOWWindow.FWinHeight - FWinTop;
                        }
                    }

                    break;
                case Aow.Units.AoWWinControl.TWindowType.winBorderless:
                case Aow.Units.AoWWinControl.TWindowType.winBlank:
                    if (FAOWWindow.FWinWidth < (FWinLeft + FWinWidth))
                    {
                        FWinLeft = FAOWWindow.FWinWidth - FWinWidth;
                    }

                    if (FWinLeft < 0)
                    {
                        FWinLeft = 0;
                        if (FAOWWindow.FWinWidth < (FWinLeft + FWinWidth))
                        {
                            FWinWidth = FAOWWindow.FWinWidth - FWinLeft;
                        }
                    }

                    if (FAOWWindow.FWinHeight < (FWinTop + FWinHeight))
                    {
                        FWinTop = FAOWWindow.FWinHeight - FWinHeight;
                    }

                    if (FWinTop < 0)
                    {
                        FWinTop = 0;
                        if (FAOWWindow.FWinHeight < (FWinTop + FWinHeight))
                        {
                            FWinHeight = FAOWWindow.FWinHeight - FWinTop;
                        }
                    }

                    break;
            }

            if ((FMinWidth > 0) && (FMinWidth > FWinWidth))
            {
                FWinWidth = FMinWidth;
            }

            if ((FMinHeight > 0) && (FMinHeight > FWinHeight))
            {
                FWinHeight = FMinHeight;
            }

            if (FAOWWindow.FWinManager != null)
            {
                FWinManager = FAOWWindow.FWinManager;
            }

            FSurfaceValid = false;
            if (FWindowType == Aow.Units.AoWWinControl.TWindowType.winBlank)
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
            int I = default;
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
                    if (FAOWWindow is Aow.Units.AoWPanel.TAOWPanel)
                    {
                        FWinLeft += FAOWWindow.FWinLeft;
                        FWinTop += FAOWWindow.FWinTop;
                    }

                    {
                        I = 0;
                        int end__10 = FChildren.Count - 1;
                        for (; I <= end__10; I += 1)
                        {
                            var call__11 = FChildren[I];
                            ((Aow.Units.AoWComp.TAoWComponent)call__11).Setup();
                        }
                    }

                    FWinLeft = FOrgLeft;
                    FWinTop = FOrgTop;
                    FBltCount = 0;
                    FDrawCount = 0;
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Exception during Setup", ((AnsiString)"Error - ") + Name, (int)0);
            }
        }

        // AoWPanel.pas:333
        // { Update: the controls are updated with the panel moved by its window's position; when the
//   panel moved or changed size, its old rectangle is redrawn in the owner }
        public override void Update()
        {
            int I = default;
            Aow.Rtl.TRect R = default;
            // { Tried Self/field casts, receiver scope and receiver/local staging; Self/temporary registers still differ. }
            if (FVisible)
            {
                FOrgLeft = FWinLeft;
                FOrgTop = FWinTop;
                if (FAOWWindow is Aow.Units.AoWPanel.TAOWPanel)
                {
                    FWinLeft += FAOWWindow.FWinLeft;
                    FWinTop += FAOWWindow.FWinTop;
                }

                if (FOnUpdate != null)
                {
                    FOnUpdate(this);
                }

                {
                    I = 0;
                    int end__12 = FChildren.Count - 1;
                    for (; I <= end__12; I += 1)
                    {
                        var call__13 = FChildren[I];
                        ((Aow.Units.AoWComp.TAoWComponent)call__13).Update();
                    }
                }

                FWinLeft = FOrgLeft;
                FWinTop = FOrgTop;
                if (((FOldRect.Right > 0) && (FOldRect.Bottom > 0)) && ((((FOldRect.Left != FWinLeft) || (FOldRect.Top != FWinTop)) || (FOldRect.Right != FWinWidth)) || (FOldRect.Bottom != FWinHeight)))
                {
                    if (FAOWOwner.FAOWOwner.FAOWOwner == null)
                    {
                        var call__14 = Aow.Rtl.Rtl.Rect(FOldRect.Left, FOldRect.Top, (FOldRect.Left + FOldRect.Right) - 1, (FOldRect.Top + FOldRect.Bottom) - 1);
                        R = call__14;
                    }
                    else
                    {
                        var call__15 = Aow.Rtl.Rtl.Rect(FAOWOwner.FWinLeft + FOldRect.Left, FAOWOwner.FWinTop + FOldRect.Top, ((FAOWOwner.FWinLeft + FOldRect.Left) + FOldRect.Right) - 1, ((FAOWOwner.FWinTop + FOldRect.Top) + FOldRect.Bottom) - 1);
                        R = call__15;
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
                        var call__16 = Aow.Rtl.Rtl.Rect(FWinLeft, FWinTop, (FWinLeft + FWinWidth) - 1, (FWinTop + FWinHeight) - 1);
                        R = call__16;
                    }
                    else
                    {
                        var call__17 = Aow.Rtl.Rtl.Rect(FAOWOwner.FWinLeft + FWinLeft, FAOWOwner.FWinTop + FWinTop, ((FAOWOwner.FWinLeft + FWinLeft) + FWinWidth) - 1, ((FAOWOwner.FWinTop + FWinTop) + FWinHeight) - 1);
                        R = call__17;
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
        public override void InvalidateRect(int Index, ref Aow.Rtl.TRect ARect, Aow.Units.AoWComp.TAoWComponent Sender)
        {
            int OldLeft = default;
            int OldTop = default;
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            Aow.Rtl.TRect R = default;
            // { Tried Self identity, receiver scope and local/field staging; register/stack allocation still differs. }
            if ((FValid && FVisible) && (FWindowType != Aow.Units.AoWWinControl.TWindowType.winBlank))
            {
                AddDrawRect(ref ARect, Sender);
            }

            OldLeft = FWinLeft;
            OldTop = FWinTop;
            FWinLeft += FAOWOwner.FWinLeft;
            FWinTop += FAOWOwner.FWinTop;
            {
                I = Index;
                int end__18 = FChildren.Count - 1;
                for (; I <= end__18; I += 1)
                {
                    var call__19 = FChildren[I];
                    C = (Aow.Units.AoWComp.TAoWComponent)call__19;
                    if (C.FVisible)
                    {
                        if ((((((FWinLeft + C.FWinLeft) >= ARect.Left) && ((FWinLeft + C.FWinLeft) <= ARect.Right)) || (((FWinLeft + C.FWinLeft) <= ARect.Left) && (ARect.Left <= ((FWinLeft + C.FWinLeft) + C.FWinWidth)))) && ((((FWinTop + C.FWinTop) >= ARect.Top) && ((FWinTop + C.FWinTop) <= ARect.Bottom)) || (((FWinTop + C.FWinTop) <= ARect.Top) && (ARect.Top <= ((FWinTop + C.FWinTop) + C.FWinHeight))))) && (C != Sender))
                        {
                            if (C is Aow.Units.AoWBaseWin.TAOWBaseWindow)
                            {
                                ((Aow.Units.AoWBaseWin.TAOWBaseWindow)C).InvalidateRect(0, ref ARect, this);
                            }
                            else
                            {
                                if (C.FDrawn)
                                {
                                    C.FDrawn = false;
                                    if (C.FMasked)
                                    {
                                        var call__20 = Aow.Rtl.Rtl.Rect(FWinLeft + C.FWinLeft, FWinTop + C.FWinTop, (FWinLeft + C.FWinLeft) + C.FWinWidth, (FWinTop + C.FWinTop) + C.FWinHeight);
                                        R = call__20;
                                        AddDrawRect(ref R, this);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (FAOWOwner != Sender)
            {
                var call__21 = FAOWOwner.FChildren.IndexOf(this);
                FAOWOwner.InvalidateRect(call__21 + 1, ref ARect, this);
            }

            FWinLeft = OldLeft;
            FWinTop = OldTop;
        }

        // AoWPanel.pas:82
        public override void AddBltRect(ref Aow.Rtl.TRect ARect, Aow.Units.AoWComp.TAoWComponent Sender)
        {
            // { Tried owner with/cast and receiver scope; extra receiver/rectangle saves remain. }
            FAOWOwner.AddBltRect(ref ARect, this);
        }

        // AoWPanel.pas:87
        public override void AddDrawRect(ref Aow.Rtl.TRect ARect, Aow.Units.AoWComp.TAoWComponent Sender)
        {
            if (FMasked)
            {
                FAOWOwner.AddDrawRect(ref ARect, this);
            }

            if (FWindowType != Aow.Units.AoWWinControl.TWindowType.winBlank)
            {
                base.AddDrawRect(ref ARect, Sender);
            }
        }

        // AoWPanel.pas:76
        public override void SetOwner(Aow.Units.AoWComp.TAoWComponent Value)
        {
            base.SetOwner(Value);
            FAOWWindow = (Aow.Units.AoWBaseWin.TAOWBaseWindow)FAOWOwner;
        }

        // AoWPanel.pas:191
        // { SetVisible: showing sets the back rectangle and marks the panel hidden (original bug: FHidden
//   set instead of cleared) }
        public override void SetVisible(bool Value)
        {
            // { Tried Self identity and typed/untyped receiver scopes; Self register still differs. }
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
                if ((!FValid || !FAligned) || !FSized)
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
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Aow.Rtl.TRect BltRect = default;
            int I = default;
            int J = default;
            Aow.Rtl.TRect OldClip = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            Aow.Units.AoWComp.TaowRect D = default;
            void DrawBack()
            {
                // { Tried Self identity, receiver scope and direct record copy; parent-frame/REP registers still differ. }
                if (FWindowType != Aow.Units.AoWWinControl.TWindowType.winBlank)
                {
                    switch (FWindowType)
                    {
                        case Aow.Units.AoWWinControl.TWindowType.winNormal:
                            var call__22 = Aow.Rtl.Rtl.Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                            FILFrame.DrawILF(FILIndexFrame, call__22, ref SurfDesc);
                            var call__23 = FILFrame[FILIndexFrame + 1];
                            var call__24 = FILFrame[FILIndexFrame + 6];
                            var call__25 = FILFrame[FILIndexFrame + 3];
                            var call__26 = FILFrame[FILIndexFrame + 4];
                            var call__27 = FILFrame[FILIndexFrame + 1];
                            var call__28 = FILFrame[FILIndexFrame + 3];
                            var call__29 = Aow.Rtl.Rtl.Rect(FWinLeft + call__28.Width, FWinTop + call__27.Height, (FWinWidth - call__25.Width) - call__26.Width, (FWinHeight - call__23.Height) - call__24.Height);
                            FILTexture.DrawILT(FILIndexTexture, call__29, ref SurfDesc);
                            break;
                        case Aow.Units.AoWWinControl.TWindowType.winHollow:
                            var call__30 = Aow.Rtl.Rtl.Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                            FILFrame.DrawILF(FILIndexFrame, call__30, ref SurfDesc);
                            break;
                        case Aow.Units.AoWWinControl.TWindowType.winBorderless:
                            var call__31 = Aow.Rtl.Rtl.Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                            FILTexture.DrawILT(FILIndexTexture, call__31, ref SurfDesc);
                            break;
                    }

                    var call__32 = Aow.Rtl.Rtl.Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, SurfDesc.ClipRect.Right, SurfDesc.ClipRect.Bottom);
                    BltRect = call__32;
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
                    if ((FWinLeft < SurfDesc.ClipRect.Right) && (FWinLeft > SurfDesc.ClipRect.Left))
                    {
                        SurfDesc.ClipRect.Left = FWinLeft;
                    }

                    if ((FWinTop < SurfDesc.ClipRect.Bottom) && (FWinTop > SurfDesc.ClipRect.Top))
                    {
                        SurfDesc.ClipRect.Top = FWinTop;
                    }

                    if (((FWinLeft + FWinWidth) > SurfDesc.ClipRect.Left) && ((FWinLeft + FWinWidth) < SurfDesc.ClipRect.Right))
                    {
                        SurfDesc.ClipRect.Right = FWinLeft + FWinWidth;
                    }

                    if (((FWinTop + FWinHeight) > SurfDesc.ClipRect.Top) && ((FWinTop + FWinHeight) < SurfDesc.ClipRect.Bottom))
                    {
                        SurfDesc.ClipRect.Bottom = FWinTop + FWinHeight;
                    }

                    if (FSurfaceValid)
                    {
                        {
                            J = 0;
                            int end__33 = FDrawCount - 1;
                            for (; J <= end__33; J += 1)
                            {
                                var call__34 = FDrawList[J];
                                D = (Aow.Units.AoWComp.TaowRect)call__34;
                                OldClip = SurfDesc.ClipRect;
                                var call__35 = Aow.Rtl.Rtl.Rect(D.R.Left, D.R.Top, D.R.Right + 1, D.R.Bottom + 1);
                                SurfDesc.ClipRect = call__35;
                                DrawBack();
                                SurfDesc.ClipRect = OldClip;
                                FWinLeft = FOrgLeft;
                                FWinTop = FOrgTop;
                                InvalidateRect(0, ref D.R, D.Owner);
                                FWinLeft += FAOWWindow.FWinLeft;
                                FWinTop += FAOWWindow.FWinTop;
                            }
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
                    {
                        I = 0;
                        int end__36 = FChildren.Count - 1;
                        for (; I <= end__36; I += 1)
                        {
                            var call__37 = FChildren[I];
                            C = (Aow.Units.AoWComp.TAoWComponent)call__37;
                            if ((C.FValid && C.FVisible) && (!FDrawn || !C.FDrawn))
                            {
                                C.FDrawn = false;
                                if (!FDrawn && (C is Aow.Units.AoWBaseWin.TAOWBaseWindow))
                                {
                                    ((Aow.Units.AoWBaseWin.TAOWBaseWindow)C).FSurfaceValid = false;
                                }

                                C.Draw(SurfDesc);
                                C.FFirstDraw = C.FDrawn;
                                C.FDrawn = true;
                                {
                                    J = 0;
                                    int end__38 = FDrawCount - 1;
                                    for (; J <= end__38; J += 1)
                                    {
                                        var call__39 = FDrawList[J];
                                        D = (Aow.Units.AoWComp.TaowRect)call__39;
                                        InvalidateRect(I + 1, ref D.R, null);
                                    }
                                }

                                FDrawCount = 0;
                                if (!FDrawn || !C.FFirstDraw)
                                {
                                    var call__40 = Aow.Rtl.Rtl.Rect(FWinLeft + C.FWinLeft, FWinTop + C.FWinTop, (FWinLeft + C.FWinLeft) + C.FWinWidth, (FWinTop + C.FWinTop) + C.FWinHeight);
                                    BltRect = call__40;
                                    FAOWOwner.AddBltRect(ref BltRect, this);
                                    FWinLeft = FOrgLeft;
                                    FWinTop = FOrgTop;
                                    var call__41 = Aow.Rtl.Rtl.Rect((FAOWWindow.FWinLeft + FWinLeft) + C.FWinLeft, (FAOWWindow.FWinTop + FWinTop) + C.FWinTop, (((FAOWWindow.FWinLeft + FWinLeft) + C.FWinLeft) + C.FWinWidth) - 1, (((FAOWWindow.FWinTop + FWinTop) + C.FWinTop) + C.FWinHeight) - 1);
                                    BltRect = call__41;
                                    InvalidateRect(I + 1, ref BltRect, C);
                                    FWinLeft += FAOWWindow.FWinLeft;
                                    FWinTop += FAOWWindow.FWinTop;
                                }
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
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Exception during Draw", ((AnsiString)"Error - ") + Name, (int)0);
            }
        }

        // AoWPanel.pas:513
        // { CheckMouseDown: the controls first (from the top), with the panel moved by its window's
//   position }
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
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

                {
                    I = FChildren.Count - 1;
                    byte end__42 = 0;
                    for (; I >= end__42; I -= 1)
                    {
                        var call__43 = FChildren[I];
                        C = (Aow.Units.AoWComp.TAoWComponent)call__43;
                        C.FHandled = false;
                        if ((((FHandled == false) || C.FRedraw) && (C.FVisible == true)) && C.FCaptureInput)
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
                }

                if ((((X > (FWinLeft - 1)) && (X < (FWinLeft + FWinWidth))) && (Y > (FWinTop - 1))) && (Y < (FWinTop + FWinHeight)))
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
        public override void CheckMouseUp(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
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

                {
                    I = FChildren.Count - 1;
                    byte end__44 = 0;
                    for (; I >= end__44; I -= 1)
                    {
                        var call__45 = FChildren[I];
                        C = (Aow.Units.AoWComp.TAoWComponent)call__45;
                        C.FHandled = false;
                        if ((((FHandled == false) || (C.FRedraw == true)) && (C.FVisible == true)) && C.FCaptureInput)
                        {
                            C.CheckMouseUp(Button, Shift, X, Y);
                        }

                        if (C.FHandled == true)
                        {
                            FHandled = true;
                        }
                    }
                }

                if ((((X > (FWinLeft - 1)) && (X < (FWinLeft + FWinWidth))) && (Y > (FWinTop - 1))) && (Y < (FWinTop + FWinHeight)))
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
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
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

                {
                    I = FChildren.Count - 1;
                    byte end__46 = 0;
                    for (; I >= end__46; I -= 1)
                    {
                        var call__47 = FChildren[I];
                        C = (Aow.Units.AoWComp.TAoWComponent)call__47;
                        C.FHandled = false;
                        if (((((FHandled == false) || (C.FRedraw == true)) || (C.FToolTipCount > 0)) && (C.FVisible == true)) && C.FCaptureInput)
                        {
                            C.CheckMouseMove(Shift, X, Y);
                        }

                        if (C.FHandled == true)
                        {
                            FHandled = true;
                        }
                    }
                }

                if ((((X > (FWinLeft - 1)) && (X < (FWinLeft + FWinWidth))) && (Y > (FWinTop - 1))) && (Y < (FWinTop + FWinHeight)))
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
        public virtual void SetTabPanel(Aow.Units.AoWTabPanel.TAOWTabPanel Value)
        {
            int I = default;
            int NewIndex = default;
            Aow.Units.AoWPanel.TAOWPanel P = default;
            if (Value != FTabPanel)
            {
                if (FTabPanel != null)
                {
                    I = 0;
                    while (I < FTabPanel.FPanels.Count)
                    {
                        var call__48 = FTabPanel.FPanels[I];
                        P = (Aow.Units.AoWPanel.TAOWPanel)call__48;
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
                        var call__49 = FTabPanel.FPanels[I];
                        P = (Aow.Units.AoWPanel.TAOWPanel)call__49;
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
        public void SetAOWWindow(Aow.Units.AoWBaseWin.TAOWBaseWindow Value)
        {
            SetOwner(Value);
        }

        [Published]
        public Aow.Units.AoWBaseWin.TAOWBaseWindow AOWWindow { get => FAOWWindow; set => SetAOWWindow(value); }

        [Published]
        public Aow.Units.AoWTabPanel.TAOWTabPanel TabPanel { get => FTabPanel; set => SetTabPanel(value); }

        [Published]
        public int OwnerItem { get => FOwnerItem; set => FOwnerItem = value; }

        [Published]
        public int MinWidth { get => FMinWidth; set => FMinWidth = value; }

        [Published]
        public int MinHeight { get => FMinHeight; set => FMinHeight = value; }
    }

    public static partial class AoWPanel
    {
        // AoWPanel.pas:643
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Interface", [typeof(Aow.Units.AoWPanel.TAOWPanel)]);
        }
    }
}
