// Translated from AoWWindow.pas.
using Aow.Game.Draw;

using Aow.Game.Ui.Controls;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Windows;

// { The AoWWindow unit of aowInt.dpl: TAOWWindow, the ordinary window of the window manager. It
//   draws its frame and texture, can be dragged by its title area (DragHeight) and resized by its
//   borders, and keeps itself inside the window manager's screen and borders. }
// no fields of its own. It uses the dragging fields of TAOWWindowControl: FDragging (0 none, 1
// dragging, 2 resizing), FDragX and FDragY (the mouse's offset from the edge being moved),
// FResizingX and FResizingY (0 none, 1 the left or top edge, 2 the right or bottom edge)
public partial class AOWWindow : AOWGenericWindow
{
    // AoWWindow.pas:54
    public AOWWindow(Component owner) : base(owner)
    {
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
            case WindowType.Normal:
            case WindowType.Hollow:
                if (FILFrame != null && (FILTexture != null || FWindowType == WindowType.Hollow))
                {
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

                    if (FMinWidth > 0 && FMinWidth > FWinWidth)
                    {
                        FWinWidth = FMinWidth;
                    }

                    if (FMinHeight > 0 && FMinHeight > FWinHeight)
                    {
                        FWinHeight = FMinHeight;
                    }

                    if (FMaxWidth > 0 && FMaxWidth < FWinWidth)
                    {
                        FWinWidth = FMaxWidth;
                    }

                    if (FMaxHeight > 0 && FMaxHeight < FWinHeight)
                    {
                        FWinHeight = FMaxHeight;
                    }

                    if (FWinManager.FWinWidth > 0 && FWinManager.FWinHeight > 0)
                    {
                        if (FUseManagerBorders)
                        {
                            if (FWinManager.FWinWidth - FWinManager.FBorderRight < FWinLeft + FWinWidth)
                            {
                                FWinLeft = FWinManager.FWinWidth - FWinManager.FBorderRight - FWinWidth;
                            }

                            if (FWinManager.FBorderLeft > FWinLeft)
                            {
                                FWinLeft = FWinManager.FBorderLeft;
                                if (FWinManager.FWinWidth - FWinManager.FBorderRight < FWinLeft + FWinWidth)
                                {
                                    FWinWidth = FWinManager.FWinWidth - FWinManager.FBorderRight - FWinLeft;
                                }
                            }

                            if (FWinManager.FWinHeight - FWinManager.FBorderBottom < FWinTop + FWinHeight)
                            {
                                FWinTop = FWinManager.FWinHeight - FWinManager.FBorderBottom - FWinHeight;
                            }

                            if (FWinManager.FBorderTop > FWinTop)
                            {
                                FWinTop = FWinManager.FBorderTop;
                                if (FWinManager.FWinHeight - FWinManager.FBorderBottom < FWinTop + FWinHeight)
                                {
                                    FWinHeight = FWinManager.FWinHeight - FWinManager.FBorderBottom - FWinTop;
                                }
                            }
                        }
                        else
                        {
                            if (FWinManager.FWinWidth < FWinLeft + FWinWidth)
                            {
                                FWinLeft = FWinManager.FWinWidth - FWinWidth;
                            }

                            if (FWinLeft < 0)
                            {
                                FWinLeft = 0;
                                if (FWinManager.FWinWidth < FWinLeft + FWinWidth)
                                {
                                    FWinWidth = FWinManager.FWinWidth - FWinLeft;
                                }
                            }

                            if (FWinManager.FWinHeight < FWinTop + FWinHeight)
                            {
                                FWinTop = FWinManager.FWinHeight - FWinHeight;
                            }

                            if (FWinTop < 0)
                            {
                                FWinTop = 0;
                                if (FWinManager.FWinHeight < FWinTop + FWinHeight)
                                {
                                    FWinHeight = FWinManager.FWinHeight - FWinTop;
                                }
                            }
                        }
                    }

                    if (FMasked)
                    {
                        if (FWindowType == WindowType.Hollow)
                        {
                            FFrameLeft = FWinWidth;
                            FFrameRight = 0;
                            FFrameTop = FWinHeight;
                            FFrameBottom = 0;
                        }
                        else
                        {
                            FFrameLeft = FILFrame[FILIndexFrame + 3].Width;
                            var ilFrame2 = FILFrame[FILIndexFrame + 4];
                            FFrameRight = FWinWidth - ilFrame2.Width;
                            FFrameTop = FILFrame[FILIndexFrame + 1].Height;
                            var ilFrame3 = FILFrame[FILIndexFrame + 6];
                            FFrameBottom = FWinHeight - ilFrame3.Height;
                        }
                    }
                }

                break;
            case WindowType.Borderless:
            case WindowType.Blank:
                if (FILTexture != null || FWindowType == WindowType.Blank)
                {
                    if (FMinWidth > 0 && FMinWidth > FWinWidth)
                    {
                        FWinWidth = FMinWidth;
                    }

                    if (FMinHeight > 0 && FMinHeight > FWinHeight)
                    {
                        FWinHeight = FMinHeight;
                    }

                    if (FMaxWidth > 0 && FMaxWidth < FWinWidth)
                    {
                        FWinWidth = FMaxWidth;
                    }

                    if (FMaxHeight > 0 && FMaxHeight < FWinHeight)
                    {
                        FWinHeight = FMaxHeight;
                    }

                    if (FWinManager.FWinWidth > 0 && FWinManager.FWinHeight > 0)
                    {
                        if (FUseManagerBorders)
                        {
                            if (FWinManager.FWinWidth - FWinManager.FBorderRight < FWinLeft + WinWidth)
                            {
                                FWinLeft = FWinManager.FWinWidth - FWinManager.FBorderRight - FWinWidth;
                            }

                            if (FWinManager.BorderLeft > FWinLeft)
                            {
                                FWinLeft = FWinManager.FBorderLeft;
                                if (FWinManager.FWinWidth - FWinManager.FBorderRight < FWinLeft + FWinWidth)
                                {
                                    FWinWidth = FWinManager.FWinWidth - FWinManager.FBorderRight - FWinLeft;
                                }
                            }

                            if (FWinManager.FWinHeight - FWinManager.FBorderBottom < FWinTop + FWinHeight)
                            {
                                FWinTop = FWinManager.FWinHeight - FWinManager.FBorderBottom - FWinHeight;
                            }

                            if (FWinManager.BorderTop > FWinTop)
                            {
                                FWinTop = FWinManager.FBorderTop;
                                if (FWinManager.FWinHeight - FWinManager.FBorderBottom < FWinTop + FWinHeight)
                                {
                                    FWinHeight = FWinManager.FWinHeight - FWinManager.FBorderBottom - FWinTop;
                                }
                            }
                        }
                        else
                        {
                            if (FWinManager.FWinWidth < FWinLeft + WinWidth)
                            {
                                FWinLeft = FWinManager.FWinWidth - FWinWidth;
                            }

                            if (FWinLeft < 0)
                            {
                                FWinLeft = 0;
                                if (FWinManager.FWinWidth < FWinLeft + FWinWidth)
                                {
                                    FWinWidth = FWinManager.FWinWidth - FWinLeft;
                                }
                            }

                            if (FWinManager.FWinHeight < FWinTop + FWinHeight)
                            {
                                FWinTop = FWinManager.FWinHeight - FWinHeight;
                            }

                            if (FWinTop < 0)
                            {
                                FWinTop = 0;
                                if (FWinManager.FWinHeight < FWinTop + FWinHeight)
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

        if (FDoubleBuffered && FBackSurface != null)
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
        base.ReAlign();
        if (FUseManagerBorders)
        {
            if (FAlignment.AlignWidth == AOWAlignWidth.Center)
            {
                FWinLeft = (FAOWOwner.FWinWidth - FWinManager.FBorderLeft - FWinManager.FBorderRight) / 2 + FWinManager.FBorderLeft - FWinWidth / 2;
            }

            if (FAlignment.AlignHeight == AOWAlignHeight.Center)
            {
                FWinTop = (FAOWOwner.FWinHeight - FWinManager.FBorderTop - FWinManager.FBorderBottom) / 2 + FWinManager.FBorderTop - FWinHeight / 2;
            }
        }
    }

    // AoWWindow.pas:222
    public override void Draw(SurfaceDesc SurfDesc)
    {
        try
        {
            if (FValid && FVisible)
            {
                switch (FWindowType)
                {
                    case WindowType.Normal:
                        var rect = new Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                        FILFrame.DrawILF(FILIndexFrame, rect, ref SurfDesc);
                        var ilFrameHeight = FILFrame[FILIndexFrame + 1];
                        var ilFrame = FILFrame[FILIndexFrame + 6];
                        var ilFrameWidth = FILFrame[FILIndexFrame + 3];
                        var ilFrame2 = FILFrame[FILIndexFrame + 4];
                        var ilFrame3 = FILFrame[FILIndexFrame + 1];
                        var ilFrame4 = FILFrame[FILIndexFrame + 3];
                        var rect2 = new Rect(FWinLeft + ilFrame4.Width, FWinTop + ilFrame3.Height, FWinWidth - ilFrameWidth.Width - ilFrame2.Width, FWinHeight - ilFrameHeight.Height - ilFrame.Height);
                        FILTexture.DrawILT(FILIndexTexture, rect2, ref SurfDesc);
                        break;
                    case WindowType.Hollow:
                        var rect3 = new Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                        FILFrame.DrawILF(FILIndexFrame, rect3, ref SurfDesc);
                        break;
                    case WindowType.Borderless:
                        var rect4 = new Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                        FILTexture.DrawILT(FILIndexTexture, rect4, ref SurfDesc);
                        break;
                }

                if (FOnDrawBack != null)
                {
                    FOnDrawBack(this, SurfDesc);
                }

                FSurfaceValid = false;
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AoWWindow.pas:253
    // { CheckMouseDown: a click brings the window to the front; the controls get it first; a left
    //   click in the title area starts dragging, one on a border (Resizable) starts resizing }
    public override void CheckMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        AoWComponent C = default;
        FHandled = false;
        FRedraw = false;
        FDragging = 0;
        if (FWinManager != null && !FWinManager.FHandled && FVisible == true)
        {
            if (X > FWinLeft - 1 && X < FWinLeft + FWinWidth && Y > FWinTop - 1 && Y < FWinTop + FWinHeight)
            {
                SetPriority(FPriority);
            }

            for (int I = FChildren.Count - 1; I >= 0; I--)
            {
                C = (AoWComponent)FChildren[I];
                C.FHandled = false;
                if ((!FHandled || C.FRedraw) && C.FVisible == true && C.FCaptureInput)
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

            if (!FHandled && X > FWinLeft - 1 && X < FWinLeft + FWinWidth && Y > FWinTop - 1 && Y < FWinTop + FWinHeight)
            {
                FDragX = 0;
                FDragY = 0;
                if (Button == MouseButton.Left && FDraggable == true && Y < FWinTop + FBorderHeight + FDragHeight - 1)
                {
                    FDragging = 1;
                    FDragX = X - FWinLeft;
                    FDragY = Y - FWinTop;
                }

                if (Button == MouseButton.Left && FResizable == true)
                {
                    FResizingX = 0;
                    FResizingY = 0;
                    if (X < FWinLeft + FBorderWidth - 1)
                    {
                        FDragging = 2;
                        FResizingX = 1;
                        FDragX = X - FWinLeft;
                        if (Y < FBorderHeight * 5 + FWinTop - 1)
                        {
                            FResizingY = 1;
                            FDragY = Y - FWinTop;
                        }

                        if (Y > FWinTop + FWinHeight - FBorderHeight * 5)
                        {
                            FResizingY = 2;
                            FDragY = FWinTop + FWinHeight - Y;
                        }
                    }

                    if (X > FWinLeft + FWinWidth - FBorderHeight)
                    {
                        FDragging = 2;
                        FResizingX = 2;
                        FDragX = FWinLeft + FWinWidth - X;
                        if (Y < FBorderHeight * 5 + FWinTop - 1)
                        {
                            FResizingY = 1;
                            FDragY = Y - FWinTop;
                        }

                        if (Y > FWinTop + FWinHeight - FBorderHeight * 5)
                        {
                            FResizingY = 2;
                            FDragY = FWinTop + FWinHeight - Y;
                        }
                    }

                    if (Y < FWinTop + FBorderHeight - 1)
                    {
                        FDragging = 2;
                        FResizingY = 1;
                        FDragY = Y - FWinTop;
                        if (X < FBorderWidth * 5 + FWinLeft - 1)
                        {
                            FResizingX = 1;
                            FDragX = X - FWinLeft;
                        }

                        if (X > FWinLeft + FWinWidth - FBorderWidth * 5)
                        {
                            FResizingX = 2;
                            FDragX = FWinLeft + FWinWidth - X;
                        }
                    }

                    if (Y > FWinTop + FWinHeight - FBorderHeight)
                    {
                        FDragging = 2;
                        FResizingY = 2;
                        FDragY = FWinTop + FWinHeight - Y;
                        if (X < FBorderWidth * 5 + FWinLeft - 1)
                        {
                            FResizingX = 1;
                            FDragX = X - FWinLeft;
                        }

                        if (X > FWinLeft + FWinWidth - FBorderWidth * 5)
                        {
                            FResizingX = 2;
                            FDragX = FWinLeft + FWinWidth - X;
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
                    if (!FWinManager.FHandled && FOnMouseDown != null)
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
    public override void CheckMouseUp(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        AoWComponent C = default;
        FHandled = false;
        if (FVisible && FValid)
        {
            FDragging = 0;
            for (int I = FChildren.Count - 1; I >= 0; I--)
            {
                C = (AoWComponent)FChildren[I];
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
            }

            if (FHandled)
            {
                if (!FWinManager.FHandled && FOnMouseUp != null)
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
    public override void CheckMouseMove(Set<ShiftState> Shift, int X, int Y)
    {
        bool Resized;
        int I = default;
        AoWComponent C = default;
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
                    if (FWinManager != null && FUseManagerBorders)
                    {
                        if (FWinManager.FBorderLeft > FWinLeft)
                        {
                            FWinLeft = FWinManager.FBorderLeft;
                        }

                        if (FWinManager.FWinWidth - FWinManager.FBorderRight < FWinLeft + FWinWidth)
                        {
                            FWinLeft = FWinManager.FWinWidth - FWinManager.FBorderRight - FWinWidth;
                        }

                        if (FWinManager.FBorderTop > FWinTop)
                        {
                            FWinTop = FWinManager.FBorderTop;
                        }

                        if (FWinManager.FWinHeight - FWinManager.FBorderBottom < FWinTop + FWinHeight)
                        {
                            FWinTop = FWinManager.FWinHeight - FWinManager.FBorderBottom - FWinHeight;
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
                            if (FWinLeft + FDragX > X)
                            {
                                if (FWinManager != null && FUseManagerBorders)
                                {
                                    if (FWinManager.FBorderLeft > X - FDragX)
                                    {
                                        X = FWinManager.FBorderLeft + FDragX;
                                    }
                                }
                            }

                            I = FWinLeft + FWinWidth;
                            FWinWidth = FWinWidth - X + FWinLeft + FDragX;
                            if (FWinWidth < FMinWidth)
                            {
                                FWinWidth = FMinWidth;
                            }

                            if (FMaxWidth < FWinWidth && FMaxWidth > 0)
                            {
                                FWinWidth = FMaxWidth;
                            }

                            FWinLeft = I - FWinWidth;
                            Resized = true;
                            break;
                        case 2:
                            FWinWidth = X + FDragX - FWinLeft;
                            if (FWinManager != null && FUseManagerBorders)
                            {
                                if (FWinManager.FWinWidth - FWinManager.FBorderRight < FWinLeft + FWinWidth)
                                {
                                    FWinWidth = FWinManager.FWinWidth - FWinManager.FBorderRight - FWinLeft;
                                }
                            }

                            if (FMinWidth > FWinWidth)
                            {
                                FWinWidth = FMinWidth;
                            }

                            if (FMaxWidth < FWinWidth && FMaxWidth > 0)
                            {
                                FWinWidth = FMaxWidth;
                            }

                            Resized = true;
                            break;
                    }

                    switch (FResizingY)
                    {
                        case 1:
                            if (FWinTop + FDragY > Y)
                            {
                                if (FWinManager != null && FUseManagerBorders)
                                {
                                    if (FWinManager.FBorderTop > Y - FDragY)
                                    {
                                        Y = FWinManager.FBorderTop + FDragY;
                                    }
                                }
                            }

                            I = FWinTop + FWinHeight;
                            FWinHeight = FWinHeight - Y + FWinTop + FDragY;
                            if (FWinHeight < FMinHeight)
                            {
                                FWinHeight = FMinHeight;
                            }

                            if (FMaxHeight < FWinHeight && FMaxHeight > 0)
                            {
                                FWinHeight = FMaxHeight;
                            }

                            FWinTop = I - FWinHeight;
                            Resized = true;
                            break;
                        case 2:
                            FWinHeight = Y + FDragY - FWinTop;
                            if (FWinManager != null && FUseManagerBorders)
                            {
                                if (FWinManager.FWinHeight - FWinManager.FBorderBottom < FWinTop + FWinHeight)
                                {
                                    FWinHeight = FWinManager.FWinHeight - FWinManager.FBorderBottom - FWinTop;
                                }
                            }

                            if (FMinHeight > FWinHeight)
                            {
                                FWinHeight = FMinHeight;
                            }

                            if (FMaxHeight < FWinHeight && FMaxHeight > 0)
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

            I = FChildren.Count - 1;
            for (; I >= 0; I -= 1)
            {
                C = (AoWComponent)FChildren[I];
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

            if (!FHandled && X > FWinLeft - 1 && X < FWinLeft + FWinWidth && Y > FWinTop - 1 && Y < FWinTop + FWinHeight)
            {
                FHandled = true;
                if (FResizable == true)
                {
                    if (X < FWinLeft + FBorderWidth - 1)
                    {
                        FCursor = -9;
                        if (Y < FBorderHeight * 5 + FWinTop - 1)
                        {
                            FCursor = -8;
                        }

                        if (Y > FWinTop + FWinHeight - FBorderHeight * 5)
                        {
                            FCursor = -6;
                        }
                    }
                    else
                    {
                        if (X > FWinLeft + FWinWidth - FBorderWidth)
                        {
                            FCursor = -9;
                            if (Y < FBorderHeight * 5 + FWinTop - 1)
                            {
                                FCursor = -6;
                            }

                            if (Y > FWinTop + FWinHeight - FBorderHeight * 5)
                            {
                                FCursor = -8;
                            }
                        }
                        else
                        {
                            if (Y < FWinTop + FBorderHeight - 1)
                            {
                                FCursor = -7;
                                if (X < FBorderWidth * 5 + FWinLeft - 1)
                                {
                                    FCursor = -8;
                                }

                                if (X > FWinLeft + FWinWidth - FBorderWidth * 5)
                                {
                                    FCursor = -6;
                                }
                            }
                            else
                            {
                                if (Y > FWinTop + FWinHeight - FBorderHeight)
                                {
                                    FCursor = -7;
                                    if (X < FBorderWidth * 5 + FWinLeft - 1)
                                    {
                                        FCursor = -6;
                                    }

                                    if (X > FWinLeft + FWinWidth - FBorderWidth * 5)
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
                    if (!FWinManager.FHandled && FOnMouseMove != null)
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

        if (FCursor != FOldCursor && FOnChangeCursor != null)
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
    public NotifyEvent<AOWWindow> OnDrag
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    [Published]
    public NotifyEvent<AOWWindow> OnResize
    {
        get => EventSlot.Get(this, FOnResize);
        set => EventSlot.Set(this, ref FOnResize, value);
    }

    [Published]
    public NotifyEvent<AOWWindow> OnInternalResize
    {
        get => EventSlot.Get(this, FOnInternalResize);
        set => EventSlot.Set(this, ref FOnInternalResize, value);
    }

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

    public new NotifyEvent<AOWWindow> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<AOWWindow> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<AOWWindow> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<AOWWindow> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<AOWWindow> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<AOWWindow> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<AOWWindow> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<AOWWindow> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<AOWWindow> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }

    public new AoWOnDrawBackEvent<AOWWindow> OnDrawBack
    {
        get => EventSlot.Get(this, FOnDrawBack);
        set => EventSlot.Set(this, ref FOnDrawBack, value);
    }

    public new NotifyEvent<AOWWindow> OnEnter
    {
        get => EventSlot.Get(this, FOnEnter);
        set => EventSlot.Set(this, ref FOnEnter, value);
    }

    public new NotifyEvent<AOWWindow> OnExit
    {
        get => EventSlot.Get(this, FOnExit);
        set => EventSlot.Set(this, ref FOnExit, value);
    }

    public new NotifyEvent<AOWWindow> OnChangeCursor
    {
        get => EventSlot.Get(this, FOnChangeCursor);
        set => EventSlot.Set(this, ref FOnChangeCursor, value);
    }

    public new AoWOnDrawSurface<AOWWindow> OnDrawSurface
    {
        get => EventSlot.Get(this, FOnDrawSurface);
        set => EventSlot.Set(this, ref FOnDrawSurface, value);
    }

    public new AoWOnDrawSurface<AOWWindow> OnPreDrawSurface
    {
        get => EventSlot.Get(this, FOnPreDrawSurface);
        set => EventSlot.Set(this, ref FOnPreDrawSurface, value);
    }

    public new NotifyEvent<AOWWindow> OnHide
    {
        get => EventSlot.Get(this, FOnHide);
        set => EventSlot.Set(this, ref FOnHide, value);
    }

    public new NotifyEvent<AOWWindow> OnShow
    {
        get => EventSlot.Get(this, FOnShow);
        set => EventSlot.Set(this, ref FOnShow, value);
    }
}
