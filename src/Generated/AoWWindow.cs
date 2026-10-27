// Generated from AoWWindow.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.AoWWinGeneric;
using static Aow.Units.AoWWinGeneric.AoWWinGeneric;

namespace Aow.Units.AoWWindow
{
    // { The AoWWindow unit of aowInt.dpl: TAOWWindow, the ordinary window of the window manager. It
//   draws its frame and texture, can be dragged by its title area (DragHeight) and resized by its
//   borders, and keeps itself inside the window manager's screen and borders. }
    // { AoWWindow.TAOWWindow (instance size 492): no fields of its own. It uses the dragging fields of
//     TAOWWindowControl: FDragging (0 none, 1 dragging, 2 resizing), FDragX and FDragY (the
//     mouse's offset from the edge being moved), FResizingX and FResizingY (0 none, 1 the left or
//     top edge, 2 the right or bottom edge). }
    public partial class TAOWWindow : Aow.Units.AoWWinGeneric.TAOWGenericWindow
    {
        // AoWWindow.pas:54
        // { TAOWWindow }
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            base.Create__init(AOwner);
            FUseManagerBorders = true;
        }

        // AoWWindow.pas:63
        // { SetSize: at least as large as the frame corners, between the minimum and maximum size, and
//   inside the window manager's screen (and borders); the frame rectangle is set for masked
//   windows }
        public override void SetSize()
        {
            switch (FWindowType)
            {
                case Aow.Units.AoWWinControl.TWindowType.winNormal:
                case Aow.Units.AoWWinControl.TWindowType.winHollow:
                    if ((FILFrame != null) && ((FILTexture != null) || (FWindowType == Aow.Units.AoWWinControl.TWindowType.winHollow)))
                    {
                        var call__1 = FILFrame[FILIndexFrame];
                        var call__2 = FILFrame[FILIndexFrame + 2];
                        if ((call__1.Width + call__2.Width) > FWinWidth)
                        {
                            var call__3 = FILFrame[FILIndexFrame];
                            var call__4 = FILFrame[FILIndexFrame + 2];
                            FWinWidth = call__3.Width + call__4.Width;
                        }

                        var call__5 = FILFrame[FILIndexFrame];
                        var call__6 = FILFrame[FILIndexFrame + 5];
                        if ((call__5.Height + call__6.Height) > FWinHeight)
                        {
                            var call__7 = FILFrame[FILIndexFrame];
                            var call__8 = FILFrame[FILIndexFrame + 5];
                            FWinHeight = call__7.Height + call__8.Height;
                        }

                        if ((FMinWidth > 0) && (FMinWidth > FWinWidth))
                        {
                            FWinWidth = FMinWidth;
                        }

                        if ((FMinHeight > 0) && (FMinHeight > FWinHeight))
                        {
                            FWinHeight = FMinHeight;
                        }

                        if ((FMaxWidth > 0) && (FMaxWidth < FWinWidth))
                        {
                            FWinWidth = FMaxWidth;
                        }

                        if ((FMaxHeight > 0) && (FMaxHeight < FWinHeight))
                        {
                            FWinHeight = FMaxHeight;
                        }

                        if ((FWinManager.FWinWidth > 0) && (FWinManager.FWinHeight > 0))
                        {
                            if (FUseManagerBorders)
                            {
                                if ((FWinManager.FWinWidth - FWinManager.FBorderRight) < (FWinLeft + FWinWidth))
                                {
                                    FWinLeft = (FWinManager.FWinWidth - FWinManager.FBorderRight) - FWinWidth;
                                }

                                if (FWinManager.FBorderLeft > FWinLeft)
                                {
                                    FWinLeft = FWinManager.FBorderLeft;
                                    if ((FWinManager.FWinWidth - FWinManager.FBorderRight) < (FWinLeft + FWinWidth))
                                    {
                                        FWinWidth = (FWinManager.FWinWidth - FWinManager.FBorderRight) - FWinLeft;
                                    }
                                }

                                if ((FWinManager.FWinHeight - FWinManager.FBorderBottom) < (FWinTop + FWinHeight))
                                {
                                    FWinTop = (FWinManager.FWinHeight - FWinManager.FBorderBottom) - FWinHeight;
                                }

                                if (FWinManager.FBorderTop > FWinTop)
                                {
                                    FWinTop = FWinManager.FBorderTop;
                                    if ((FWinManager.FWinHeight - FWinManager.FBorderBottom) < (FWinTop + FWinHeight))
                                    {
                                        FWinHeight = (FWinManager.FWinHeight - FWinManager.FBorderBottom) - FWinTop;
                                    }
                                }
                            }
                            else
                            {
                                if (FWinManager.FWinWidth < (FWinLeft + FWinWidth))
                                {
                                    FWinLeft = FWinManager.FWinWidth - FWinWidth;
                                }

                                if (FWinLeft < 0)
                                {
                                    FWinLeft = 0;
                                    if (FWinManager.FWinWidth < (FWinLeft + FWinWidth))
                                    {
                                        FWinWidth = FWinManager.FWinWidth - FWinLeft;
                                    }
                                }

                                if (FWinManager.FWinHeight < (FWinTop + FWinHeight))
                                {
                                    FWinTop = FWinManager.FWinHeight - FWinHeight;
                                }

                                if (FWinTop < 0)
                                {
                                    FWinTop = 0;
                                    if (FWinManager.FWinHeight < (FWinTop + FWinHeight))
                                    {
                                        FWinHeight = FWinManager.FWinHeight - FWinTop;
                                    }
                                }
                            }
                        }

                        if (FMasked)
                        {
                            if (FWindowType == Aow.Units.AoWWinControl.TWindowType.winHollow)
                            {
                                FFrameLeft = FWinWidth;
                                FFrameRight = 0;
                                FFrameTop = FWinHeight;
                                FFrameBottom = 0;
                            }
                            else
                            {
                                var call__9 = FILFrame[FILIndexFrame + 3];
                                FFrameLeft = call__9.Width;
                                var call__10 = FILFrame[FILIndexFrame + 4];
                                FFrameRight = FWinWidth - call__10.Width;
                                var call__11 = FILFrame[FILIndexFrame + 1];
                                FFrameTop = call__11.Height;
                                var call__12 = FILFrame[FILIndexFrame + 6];
                                FFrameBottom = FWinHeight - call__12.Height;
                            }
                        }
                    }

                    break;
                case Aow.Units.AoWWinControl.TWindowType.winBorderless:
                case Aow.Units.AoWWinControl.TWindowType.winBlank:
                    if ((FILTexture != null) || (FWindowType == Aow.Units.AoWWinControl.TWindowType.winBlank))
                    {
                        if ((FMinWidth > 0) && (FMinWidth > FWinWidth))
                        {
                            FWinWidth = FMinWidth;
                        }

                        if ((FMinHeight > 0) && (FMinHeight > FWinHeight))
                        {
                            FWinHeight = FMinHeight;
                        }

                        if ((FMaxWidth > 0) && (FMaxWidth < FWinWidth))
                        {
                            FWinWidth = FMaxWidth;
                        }

                        if ((FMaxHeight > 0) && (FMaxHeight < FWinHeight))
                        {
                            FWinHeight = FMaxHeight;
                        }

                        if ((FWinManager.FWinWidth > 0) && (FWinManager.FWinHeight > 0))
                        {
                            if (FUseManagerBorders)
                            {
                                if ((FWinManager.FWinWidth - FWinManager.FBorderRight) < (FWinLeft + WinWidth))
                                {
                                    FWinLeft = (FWinManager.FWinWidth - FWinManager.FBorderRight) - FWinWidth;
                                }

                                if (FWinManager.BorderLeft > FWinLeft)
                                {
                                    FWinLeft = FWinManager.FBorderLeft;
                                    if ((FWinManager.FWinWidth - FWinManager.FBorderRight) < (FWinLeft + FWinWidth))
                                    {
                                        FWinWidth = (FWinManager.FWinWidth - FWinManager.FBorderRight) - FWinLeft;
                                    }
                                }

                                if ((FWinManager.FWinHeight - FWinManager.FBorderBottom) < (FWinTop + FWinHeight))
                                {
                                    FWinTop = (FWinManager.FWinHeight - FWinManager.FBorderBottom) - FWinHeight;
                                }

                                if (FWinManager.BorderTop > FWinTop)
                                {
                                    FWinTop = FWinManager.FBorderTop;
                                    if ((FWinManager.FWinHeight - FWinManager.FBorderBottom) < (FWinTop + FWinHeight))
                                    {
                                        FWinHeight = (FWinManager.FWinHeight - FWinManager.FBorderBottom) - FWinTop;
                                    }
                                }
                            }
                            else
                            {
                                if (FWinManager.FWinWidth < (FWinLeft + WinWidth))
                                {
                                    FWinLeft = FWinManager.FWinWidth - FWinWidth;
                                }

                                if (FWinLeft < 0)
                                {
                                    FWinLeft = 0;
                                    if (FWinManager.FWinWidth < (FWinLeft + FWinWidth))
                                    {
                                        FWinWidth = FWinManager.FWinWidth - FWinLeft;
                                    }
                                }

                                if (FWinManager.FWinHeight < (FWinTop + FWinHeight))
                                {
                                    FWinTop = FWinManager.FWinHeight - FWinHeight;
                                }

                                if (FWinTop < 0)
                                {
                                    FWinTop = 0;
                                    if (FWinManager.FWinHeight < (FWinTop + FWinHeight))
                                    {
                                        FWinHeight = FWinManager.FWinHeight - FWinTop;
                                    }
                                }
                            }
                        }

                        if (FMasked)
                        {
                            FFrameLeft = FWinWidth;
                            FFrameRight = 0;
                            FFrameTop = FWinHeight;
                            FFrameBottom = 0;
                        }
                    }

                    break;
            }

            if (FDoubleBuffered && (FBackSurface != null))
            {
                FBackSurface.Resize(FWinWidth, FWinHeight);
                if (FMasked)
                {
                    FBackSurface.Cls(FMaskColor);
                }
            }

            if (FOnInternalResize != null)
            {
                FOnInternalResize(this);
            }

            FSurfaceValid = false;
            base.SetSize();
        }

        // AoWWindow.pas:208
        // { ReAlign: a centered window is centered between the window manager's borders }
        public override void ReAlign()
        {
            // { Tried Self/manager casts, receiver scope and pointer-cell view; border reuse still differs. }
            base.ReAlign();
            if (FUseManagerBorders)
            {
                if (FAlignment.AlignWidth == Aow.Units.AoWComp.TAOWAlignWidth.awCenter)
                {
                    FWinLeft = ((((FAOWOwner.FWinWidth - FWinManager.FBorderLeft) - FWinManager.FBorderRight) / 2) + FWinManager.FBorderLeft) - (FWinWidth / 2);
                }

                if (FAlignment.AlignHeight == Aow.Units.AoWComp.TAOWAlignHeight.ahCenter)
                {
                    FWinTop = ((((FAOWOwner.FWinHeight - FWinManager.FBorderTop) - FWinManager.FBorderBottom) / 2) + FWinManager.FBorderTop) - (FWinHeight / 2);
                }
            }
        }

        // AoWWindow.pas:222
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            try
            {
                if (FValid && FVisible)
                {
                    switch (FWindowType)
                    {
                        case Aow.Units.AoWWinControl.TWindowType.winNormal:
                            var call__13 = Aow.Rtl.Rtl.Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                            FILFrame.DrawILF(FILIndexFrame, call__13, ref SurfDesc);
                            var call__14 = FILFrame[FILIndexFrame + 1];
                            var call__15 = FILFrame[FILIndexFrame + 6];
                            var call__16 = FILFrame[FILIndexFrame + 3];
                            var call__17 = FILFrame[FILIndexFrame + 4];
                            var call__18 = FILFrame[FILIndexFrame + 1];
                            var call__19 = FILFrame[FILIndexFrame + 3];
                            var call__20 = Aow.Rtl.Rtl.Rect(FWinLeft + call__19.Width, FWinTop + call__18.Height, (FWinWidth - call__16.Width) - call__17.Width, (FWinHeight - call__14.Height) - call__15.Height);
                            FILTexture.DrawILT(FILIndexTexture, call__20, ref SurfDesc);
                            break;
                        case Aow.Units.AoWWinControl.TWindowType.winHollow:
                            var call__21 = Aow.Rtl.Rtl.Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                            FILFrame.DrawILF(FILIndexFrame, call__21, ref SurfDesc);
                            break;
                        case Aow.Units.AoWWinControl.TWindowType.winBorderless:
                            var call__22 = Aow.Rtl.Rtl.Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                            FILTexture.DrawILT(FILIndexTexture, call__22, ref SurfDesc);
                            break;
                    }

                    if (FOnDrawBack != null)
                    {
                        FOnDrawBack(this, SurfDesc);
                    }

                    FSurfaceValid = false;
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Exception during Draw", ((AnsiString)"Error - ") + Name, (int)0);
            }
        }

        // AoWWindow.pas:253
        // { CheckMouseDown: a click brings the window to the front; the controls get it first; a left
//   click in the title area starts dragging, one on a border (Resizable) starts resizing }
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            FHandled = false;
            FRedraw = false;
            FDragging = 0;
            if (((FWinManager != null) && !FWinManager.FHandled) && (FVisible == true))
            {
                if ((((X > (FWinLeft - 1)) && (X < (FWinLeft + FWinWidth))) && (Y > (FWinTop - 1))) && (Y < (FWinTop + FWinHeight)))
                {
                    SetPriority(FPriority);
                }

                {
                    I = FChildren.Count - 1;
                    byte end__23 = 0;
                    for (; I >= end__23; I -= 1)
                    {
                        var call__24 = FChildren[I];
                        C = (Aow.Units.AoWComp.TAoWComponent)call__24;
                        C.FHandled = false;
                        if ((((FHandled == false) || C.FRedraw) && (C.FVisible == true)) && C.FCaptureInput)
                        {
                            C.CheckMouseDown(Button, Shift, X, Y);
                        }

                        if (C.FHandled == true)
                        {
                            C.FRedraw = true;
                            FHandled = true;
                            FRedraw = true;
                        }
                    }
                }

                if (((((FHandled == false) && (X > (FWinLeft - 1))) && (X < (FWinLeft + FWinWidth))) && (Y > (FWinTop - 1))) && (Y < (FWinTop + FWinHeight)))
                {
                    FDragX = 0;
                    FDragY = 0;
                    if (((Button == Aow.Rtl.TMouseButton.mbLeft) && (FDraggable == true)) && (Y < (((FWinTop + FBorderHeight) + FDragHeight) - 1)))
                    {
                        FDragging = 1;
                        FDragX = X - FWinLeft;
                        FDragY = Y - FWinTop;
                    }

                    if ((Button == Aow.Rtl.TMouseButton.mbLeft) && (FResizable == true))
                    {
                        FResizingX = 0;
                        FResizingY = 0;
                        if (X < ((FWinLeft + FBorderWidth) - 1))
                        {
                            FDragging = 2;
                            FResizingX = 1;
                            FDragX = X - FWinLeft;
                            if (Y < (((FBorderHeight * 5) + FWinTop) - 1))
                            {
                                FResizingY = 1;
                                FDragY = Y - FWinTop;
                            }

                            if (Y > ((FWinTop + FWinHeight) - (FBorderHeight * 5)))
                            {
                                FResizingY = 2;
                                FDragY = (FWinTop + FWinHeight) - Y;
                            }
                        }

                        if (X > ((FWinLeft + FWinWidth) - FBorderHeight))
                        {
                            FDragging = 2;
                            FResizingX = 2;
                            FDragX = (FWinLeft + FWinWidth) - X;
                            if (Y < (((FBorderHeight * 5) + FWinTop) - 1))
                            {
                                FResizingY = 1;
                                FDragY = Y - FWinTop;
                            }

                            if (Y > ((FWinTop + FWinHeight) - (FBorderHeight * 5)))
                            {
                                FResizingY = 2;
                                FDragY = (FWinTop + FWinHeight) - Y;
                            }
                        }

                        if (Y < ((FWinTop + FBorderHeight) - 1))
                        {
                            FDragging = 2;
                            FResizingY = 1;
                            FDragY = Y - FWinTop;
                            if (X < (((FBorderWidth * 5) + FWinLeft) - 1))
                            {
                                FResizingX = 1;
                                FDragX = X - FWinLeft;
                            }

                            if (X > ((FWinLeft + FWinWidth) - (FBorderWidth * 5)))
                            {
                                FResizingX = 2;
                                FDragX = (FWinLeft + FWinWidth) - X;
                            }
                        }

                        if (Y > ((FWinTop + FWinHeight) - FBorderHeight))
                        {
                            FDragging = 2;
                            FResizingY = 2;
                            FDragY = (FWinTop + FWinHeight) - Y;
                            if (X < (((FBorderWidth * 5) + FWinLeft) - 1))
                            {
                                FResizingX = 1;
                                FDragX = X - FWinLeft;
                            }

                            if (X > ((FWinLeft + FWinWidth) - (FBorderWidth * 5)))
                            {
                                FResizingX = 2;
                                FDragX = (FWinLeft + FWinWidth) - X;
                            }
                        }
                    }

                    FRedraw = true;
                    FHandled = true;
                }

                if (FHandled)
                {
                    if (FWinManager != null)
                    {
                        if ((FWinManager.FHandled == false) && (FOnMouseDown != null))
                        {
                            FOnMouseDown(this, Button, Shift, X, Y);
                        }
                    }
                    else
                    {
                        if (FOnMouseDown != null)
                        {
                            FOnMouseDown(this, Button, Shift, X, Y);
                        }
                    }
                }

                if (FModal)
                {
                    FHandled = true;
                }
            }
        }

        // AoWWindow.pas:376
        public override void CheckMouseUp(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            FHandled = false;
            if (FVisible && FValid)
            {
                FDragging = 0;
                {
                    I = FChildren.Count - 1;
                    byte end__25 = 0;
                    for (; I >= end__25; I -= 1)
                    {
                        var call__26 = FChildren[I];
                        C = (Aow.Units.AoWComp.TAoWComponent)call__26;
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
                }

                if (FHandled)
                {
                    if ((FWinManager.FHandled == false) && (FOnMouseUp != null))
                    {
                        FOnMouseUp(this, Button, Shift, X, Y);
                    }
                }
                else
                {
                    if (FOnMouseUp != null)
                    {
                        FOnMouseUp(this, Button, Shift, X, Y);
                    }
                }

                if (FModal)
                {
                    FHandled = true;
                }
            }
        }

        // AoWWindow.pas:411
        // { CheckMouseMove drags or resizes the window, passes the move to the controls and sets the
//   resize cursors over the borders }
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            bool Resized = default;
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            FHandled = false;
            FOldCursor = FCursor;
            FCursor = FWindowCursor;
            FInResizeArea = true;
            Resized = false;
            if (FVisible == true)
            {
                switch (FDragging)
                {
                    case 1:
                        FWinLeft = X - FDragX;
                        FWinTop = Y - FDragY;
                        if ((FWinManager != null) && FUseManagerBorders)
                        {
                            if (FWinManager.FBorderLeft > FWinLeft)
                            {
                                FWinLeft = FWinManager.FBorderLeft;
                            }

                            if ((FWinManager.FWinWidth - FWinManager.FBorderRight) < (FWinLeft + FWinWidth))
                            {
                                FWinLeft = (FWinManager.FWinWidth - FWinManager.FBorderRight) - FWinWidth;
                            }

                            if (FWinManager.FBorderTop > FWinTop)
                            {
                                FWinTop = FWinManager.FBorderTop;
                            }

                            if ((FWinManager.FWinHeight - FWinManager.FBorderBottom) < (FWinTop + FWinHeight))
                            {
                                FWinTop = (FWinManager.FWinHeight - FWinManager.FBorderBottom) - FWinHeight;
                            }
                        }

                        if (FOnChange != null)
                        {
                            FOnChange(this);
                        }

                        if (!FDoubleBuffered)
                        {
                            FDrawn = false;
                        }

                        FSurfaceValid = false;
                        break;
                    case 2:
                        switch (FResizingX)
                        {
                            case 1:
                                if ((FWinLeft + FDragX) > X)
                                {
                                    if ((FWinManager != null) && FUseManagerBorders)
                                    {
                                        if ((FWinManager.FBorderLeft > (X - FDragX)))
                                        {
                                            X = FWinManager.FBorderLeft + FDragX;
                                        }
                                    }
                                }

                                I = FWinLeft + FWinWidth;
                                FWinWidth = ((FWinWidth - X) + FWinLeft) + FDragX;
                                if (FWinWidth < FMinWidth)
                                {
                                    FWinWidth = FMinWidth;
                                }

                                if ((FMaxWidth < FWinWidth) && (FMaxWidth > 0))
                                {
                                    FWinWidth = FMaxWidth;
                                }

                                FWinLeft = I - FWinWidth;
                                Resized = true;
                                break;
                            case 2:
                                FWinWidth = (X + FDragX) - FWinLeft;
                                if ((FWinManager != null) && FUseManagerBorders)
                                {
                                    if ((FWinManager.FWinWidth - FWinManager.FBorderRight) < (FWinLeft + FWinWidth))
                                    {
                                        FWinWidth = (FWinManager.FWinWidth - FWinManager.FBorderRight) - FWinLeft;
                                    }
                                }

                                if (FMinWidth > FWinWidth)
                                {
                                    FWinWidth = FMinWidth;
                                }

                                if ((FMaxWidth < FWinWidth) && (FMaxWidth > 0))
                                {
                                    FWinWidth = FMaxWidth;
                                }

                                Resized = true;
                                break;
                        }

                        switch (FResizingY)
                        {
                            case 1:
                                if ((FWinTop + FDragY) > Y)
                                {
                                    if ((FWinManager != null) && FUseManagerBorders)
                                    {
                                        if ((FWinManager.FBorderTop > (Y - FDragY)))
                                        {
                                            Y = FWinManager.FBorderTop + FDragY;
                                        }
                                    }
                                }

                                I = FWinTop + FWinHeight;
                                FWinHeight = ((FWinHeight - Y) + FWinTop) + FDragY;
                                if (FWinHeight < FMinHeight)
                                {
                                    FWinHeight = FMinHeight;
                                }

                                if ((FMaxHeight < FWinHeight) && (FMaxHeight > 0))
                                {
                                    FWinHeight = FMaxHeight;
                                }

                                FWinTop = I - FWinHeight;
                                Resized = true;
                                break;
                            case 2:
                                FWinHeight = (Y + FDragY) - FWinTop;
                                if ((FWinManager != null) && FUseManagerBorders)
                                {
                                    if ((FWinManager.FWinHeight - FWinManager.FBorderBottom) < (FWinTop + FWinHeight))
                                    {
                                        FWinHeight = (FWinManager.FWinHeight - FWinManager.FBorderBottom) - FWinTop;
                                    }
                                }

                                if (FMinHeight > FWinHeight)
                                {
                                    FWinHeight = FMinHeight;
                                }

                                if ((FMaxHeight < FWinHeight) && (FMaxHeight > 0))
                                {
                                    FWinHeight = FMaxHeight;
                                }

                                Resized = true;
                                break;
                        }

                        if (Resized)
                        {
                            if (FOnResize != null)
                            {
                                FOnResize(this);
                            }

                            FAligned = false;
                        }

                        break;
                }

                {
                    I = FChildren.Count - 1;
                    byte end__27 = 0;
                    for (; I >= end__27; I -= 1)
                    {
                        var call__28 = FChildren[I];
                        C = (Aow.Units.AoWComp.TAoWComponent)call__28;
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

                if (((((FHandled == false) && (X > (FWinLeft - 1))) && (X < (FWinLeft + FWinWidth))) && (Y > (FWinTop - 1))) && (Y < (FWinTop + FWinHeight)))
                {
                    FHandled = true;
                    if (FResizable == true)
                    {
                        if (X < ((FWinLeft + FBorderWidth) - 1))
                        {
                            FCursor = -9;
                            if (Y < (((FBorderHeight * 5) + FWinTop) - 1))
                            {
                                FCursor = -8;
                            }

                            if (Y > ((FWinTop + FWinHeight) - (FBorderHeight * 5)))
                            {
                                FCursor = -6;
                            }
                        }
                        else
                        {
                            if (X > ((FWinLeft + FWinWidth) - FBorderWidth))
                            {
                                FCursor = -9;
                                if (Y < (((FBorderHeight * 5) + FWinTop) - 1))
                                {
                                    FCursor = -6;
                                }

                                if (Y > ((FWinTop + FWinHeight) - (FBorderHeight * 5)))
                                {
                                    FCursor = -8;
                                }
                            }
                            else
                            {
                                if (Y < ((FWinTop + FBorderHeight) - 1))
                                {
                                    FCursor = -7;
                                    if (X < (((FBorderWidth * 5) + FWinLeft) - 1))
                                    {
                                        FCursor = -8;
                                    }

                                    if (X > ((FWinLeft + FWinWidth) - (FBorderWidth * 5)))
                                    {
                                        FCursor = -6;
                                    }
                                }
                                else
                                {
                                    if (Y > ((FWinTop + FWinHeight) - FBorderHeight))
                                    {
                                        FCursor = -7;
                                        if (X < (((FBorderWidth * 5) + FWinLeft) - 1))
                                        {
                                            FCursor = -6;
                                        }

                                        if (X > ((FWinLeft + FWinWidth) - (FBorderWidth * 5)))
                                        {
                                            FCursor = -8;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (FHandled)
                {
                    if (FWinManager != null)
                    {
                        if ((FWinManager.FHandled == false) && (FOnMouseMove != null))
                        {
                            FOnMouseMove(this, Shift, X, Y);
                        }
                    }
                    else
                    {
                        if (FOnMouseMove != null)
                        {
                            FOnMouseMove(this, Shift, X, Y);
                        }
                    }
                }

                if (FModal)
                {
                    FHandled = true;
                }
            }

            if ((FCursor != FOldCursor) && (FOnChangeCursor != null))
            {
                FOnChangeCursor(this);
            }
        }

        [Published]
        public int DragHeight { get => FDragHeight; set => FDragHeight = value; }

        [Published]
        public bool Draggable { get => FDraggable; set => FDraggable = value; }

        [Published]
        public bool Resizable { get => FResizable; set => FResizable = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnDrag { get => FOnChange; set => FOnChange = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnResize { get => FOnResize; set => FOnResize = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnInternalResize { get => FOnInternalResize; set => FOnInternalResize = value; }

        [Published]
        public int MinWidth { get => FMinWidth; set => FMinWidth = value; }

        [Published]
        public int MinHeight { get => FMinHeight; set => FMinHeight = value; }

        [Published]
        public int MaxWidth { get => FMaxWidth; set => FMaxWidth = value; }

        [Published]
        public int MaxHeight { get => FMaxHeight; set => FMaxHeight = value; }

        [Published]
        public bool UseManagerBorders { get => FUseManagerBorders; set => FUseManagerBorders = value; }

        [Published]
        public bool Modal { get => FModal; set => FModal = value; }

        [Published]
        public bool Dockable { get => FDockable; set => FDockable = value; }

        // AoWWindow.pas:20
        public static new Aow.Units.AoWWindow.TAOWWindow Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.AoWWindow.TAOWWindow o = new Aow.Units.AoWWindow.TAOWWindow();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public static partial class AoWWindow
    {
        // AoWWindow.pas:575
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Interface", [typeof(Aow.Units.AoWWindow.TAOWWindow)]);
        }
    }
}
