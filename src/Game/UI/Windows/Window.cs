#nullable disable warnings
// Translated from AoWWindow.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Windows;

// { The AoWWindow unit of aowInt.dpl: TAOWWindow, the ordinary window of the window manager. It
//   draws its frame and texture, can be dragged by its title area (DragHeight) and resized by its
//   borders, and keeps itself inside the window manager's screen and borders. }
// no fields of its own. It uses the dragging fields of TAOWWindowControl: FDragging (0 none, 1
// dragging, 2 resizing), FDragX and FDragY (the mouse's offset from the edge being moved),
// FResizingX and FResizingY (0 none, 1 the left or top edge, 2 the right or bottom edge)
public partial class Window : GenericWindow
{
    // AoWWindow.pas:54
    public Window(Component? owner) : base(owner) => UseManagerBorders = true;

    // AoWWindow.pas:63
    // { SetSize: at least as large as the frame corners, between the minimum and maximum size, and
    //   inside the window manager's screen (and borders); the frame rectangle is set for masked
    //   windows }
    public override void SetSize()
    {
        switch (WindowStyle)
        {
            case Style.Normal:
            case Style.Hollow:
                if (ImageLibraryFrame is not null && (ImageLibraryTexture is not null || WindowStyle == Style.Hollow))
                {
                    LibraryImage? imageLibraryFrameWidth = ImageLibraryFrame[ImageLibraryIndexFrame];
                    LibraryImage? imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
                    if (imageLibraryFrameWidth.Width + imageLibraryFrame.Width > WinWidth)
                    {
                        LibraryImage? imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame];
                        LibraryImage? imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
                        SetWinWidthWithoutInvalidate(imageLibraryFrame2.Width + imageLibraryFrame3.Width);
                    }

                    LibraryImage? imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame];
                    LibraryImage? imageLibraryFrame4 = ImageLibraryFrame[ImageLibraryIndexFrame + 5];
                    if (imageLibraryFrameHeight.Height + imageLibraryFrame4.Height > WinHeight)
                    {
                        LibraryImage? imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame];
                        LibraryImage? imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 5];
                        SetWinHeightWithoutInvalidate(imageLibraryFrame2.Height + imageLibraryFrame3.Height);
                    }

                    if (MinWidth > 0 && MinWidth > WinWidth)
                    {
                        SetWinWidthWithoutInvalidate(MinWidth);
                    }

                    if (MinHeight > 0 && MinHeight > WinHeight)
                    {
                        SetWinHeightWithoutInvalidate(MinHeight);
                    }

                    if (MaxWidth > 0 && MaxWidth < WinWidth)
                    {
                        SetWinWidthWithoutInvalidate(MaxWidth);
                    }

                    if (MaxHeight > 0 && MaxHeight < WinHeight)
                    {
                        SetWinHeightWithoutInvalidate(MaxHeight);
                    }

                    if (WindowManager.WinWidth > 0 && WindowManager.WinHeight > 0)
                    {
                        if (UseManagerBorders)
                        {
                            if (WindowManager.WinWidth - WindowManager.BorderRight < WinLeft + WinWidth)
                            {
                                SetWinLeftWithoutInvalidate(WindowManager.WinWidth - WindowManager.BorderRight - WinWidth);
                            }

                            if (WindowManager.BorderLeft > WinLeft)
                            {
                                SetWinLeftWithoutInvalidate(WindowManager.BorderLeft);
                                if (WindowManager.WinWidth - WindowManager.BorderRight < WinLeft + WinWidth)
                                {
                                    SetWinWidthWithoutInvalidate(WindowManager.WinWidth - WindowManager.BorderRight - WinLeft);
                                }
                            }

                            if (WindowManager.WinHeight - WindowManager.BorderBottom < WinTop + WinHeight)
                            {
                                SetWinTopWithoutInvalidate(WindowManager.WinHeight - WindowManager.BorderBottom - WinHeight);
                            }

                            if (WindowManager.BorderTop > WinTop)
                            {
                                SetWinTopWithoutInvalidate(WindowManager.BorderTop);
                                if (WindowManager.WinHeight - WindowManager.BorderBottom < WinTop + WinHeight)
                                {
                                    SetWinHeightWithoutInvalidate(WindowManager.WinHeight - WindowManager.BorderBottom - WinTop);
                                }
                            }
                        }
                        else
                        {
                            if (WindowManager.WinWidth < WinLeft + WinWidth)
                            {
                                SetWinLeftWithoutInvalidate(WindowManager.WinWidth - WinWidth);
                            }

                            if (WinLeft < 0)
                            {
                                SetWinLeftWithoutInvalidate(0);
                                if (WindowManager.WinWidth < WinLeft + WinWidth)
                                {
                                    SetWinWidthWithoutInvalidate(WindowManager.WinWidth - WinLeft);
                                }
                            }

                            if (WindowManager.WinHeight < WinTop + WinHeight)
                            {
                                SetWinTopWithoutInvalidate(WindowManager.WinHeight - WinHeight);
                            }

                            if (WinTop < 0)
                            {
                                SetWinTopWithoutInvalidate(0);
                                if (WindowManager.WinHeight < WinTop + WinHeight)
                                {
                                    SetWinHeightWithoutInvalidate(WindowManager.WinHeight - WinTop);
                                }
                            }
                        }
                    }

                    if (Masked)
                    {
                        if (WindowStyle == Style.Hollow)
                        {
                            FrameLeft = WinWidth;
                            FrameRight = 0;
                            FrameTop = WinHeight;
                            FrameBottom = 0;
                        }
                        else
                        {
                            FrameLeft = ImageLibraryFrame[ImageLibraryIndexFrame + 3].Width;
                            LibraryImage? imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
                            FrameRight = WinWidth - imageLibraryFrame2.Width;
                            FrameTop = ImageLibraryFrame[ImageLibraryIndexFrame + 1].Height;
                            LibraryImage? imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
                            FrameBottom = WinHeight - imageLibraryFrame3.Height;
                        }
                    }
                }

                break;
            case Style.Borderless:
            case Style.Blank:
                if (ImageLibraryTexture is not null || WindowStyle == Style.Blank)
                {
                    if (MinWidth > 0 && MinWidth > WinWidth)
                    {
                        SetWinWidthWithoutInvalidate(MinWidth);
                    }

                    if (MinHeight > 0 && MinHeight > WinHeight)
                    {
                        SetWinHeightWithoutInvalidate(MinHeight);
                    }

                    if (MaxWidth > 0 && MaxWidth < WinWidth)
                    {
                        SetWinWidthWithoutInvalidate(MaxWidth);
                    }

                    if (MaxHeight > 0 && MaxHeight < WinHeight)
                    {
                        SetWinHeightWithoutInvalidate(MaxHeight);
                    }

                    if (WindowManager.WinWidth > 0 && WindowManager.WinHeight > 0)
                    {
                        if (UseManagerBorders)
                        {
                            if (WindowManager.WinWidth - WindowManager.BorderRight < WinLeft + WinWidth)
                            {
                                SetWinLeftWithoutInvalidate(WindowManager.WinWidth - WindowManager.BorderRight - WinWidth);
                            }

                            if (WindowManager.BorderLeft > WinLeft)
                            {
                                SetWinLeftWithoutInvalidate(WindowManager.BorderLeft);
                                if (WindowManager.WinWidth - WindowManager.BorderRight < WinLeft + WinWidth)
                                {
                                    SetWinWidthWithoutInvalidate(WindowManager.WinWidth - WindowManager.BorderRight - WinLeft);
                                }
                            }

                            if (WindowManager.WinHeight - WindowManager.BorderBottom < WinTop + WinHeight)
                            {
                                SetWinTopWithoutInvalidate(WindowManager.WinHeight - WindowManager.BorderBottom - WinHeight);
                            }

                            if (WindowManager.BorderTop > WinTop)
                            {
                                SetWinTopWithoutInvalidate(WindowManager.BorderTop);
                                if (WindowManager.WinHeight - WindowManager.BorderBottom < WinTop + WinHeight)
                                {
                                    SetWinHeightWithoutInvalidate(WindowManager.WinHeight - WindowManager.BorderBottom - WinTop);
                                }
                            }
                        }
                        else
                        {
                            if (WindowManager.WinWidth < WinLeft + WinWidth)
                            {
                                SetWinLeftWithoutInvalidate(WindowManager.WinWidth - WinWidth);
                            }

                            if (WinLeft < 0)
                            {
                                SetWinLeftWithoutInvalidate(0);
                                if (WindowManager.WinWidth < WinLeft + WinWidth)
                                {
                                    SetWinWidthWithoutInvalidate(WindowManager.WinWidth - WinLeft);
                                }
                            }

                            if (WindowManager.WinHeight < WinTop + WinHeight)
                            {
                                SetWinTopWithoutInvalidate(WindowManager.WinHeight - WinHeight);
                            }

                            if (WinTop < 0)
                            {
                                SetWinTopWithoutInvalidate(0);
                                if (WindowManager.WinHeight < WinTop + WinHeight)
                                {
                                    SetWinHeightWithoutInvalidate(WindowManager.WinHeight - WinTop);
                                }
                            }
                        }
                    }

                    if (Masked)
                    {
                        FrameLeft = WinWidth;
                        FrameRight = 0;
                        FrameTop = WinHeight;
                        FrameBottom = 0;
                    }
                }

                break;
            default:
                break;
        }

        if (DoubleBuffered && BackSurface is not null)
        {
            _ = BackSurface.Resize(WinWidth, WinHeight);
            if (Masked)
            {
                _ = BackSurface.Clear(MaskColor);
            }
        }

        RaiseInternalResize(this);

        SurfaceValid = false;
        base.SetSize();
    }

    // AoWWindow.pas:208
    // { ReAlign: a centered window is centered between the window manager's borders }
    public override void ReAlign()
    {
        base.ReAlign();
        if (UseManagerBorders)
        {
            if (Alignment.AlignWidth == AlignWidth.Center)
            {
                SetWinLeftWithoutInvalidate(((OwnerComponent.WinWidth - WindowManager.BorderLeft - WindowManager.BorderRight) / 2) + WindowManager.BorderLeft - (WinWidth / 2));
            }

            if (Alignment.AlignHeight == AlignHeight.Center)
            {
                SetWinTopWithoutInvalidate(((OwnerComponent.WinHeight - WindowManager.BorderTop - WindowManager.BorderBottom) / 2) + WindowManager.BorderTop - (WinHeight / 2));
            }
        }
    }

    // AoWWindow.pas:222
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (Valid && Visible)
            {
                switch (WindowStyle)
                {
                    case Style.Normal:
                        SizedRectangle rectangle = new(WinLeft, WinTop, WinWidth, WinHeight);
                        ImageLibraryFrame.DrawFrame(ImageLibraryIndexFrame, area: rectangle, ref lockedSurface);
                        LibraryImage? imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                        LibraryImage? imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
                        LibraryImage? imageLibraryFrameWidth = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                        LibraryImage? imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
                        LibraryImage? imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                        LibraryImage? imageLibraryFrame4 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                        SizedRectangle rectangle2 = new(WinLeft + imageLibraryFrame4.Width, WinTop + imageLibraryFrame3.Height, WinWidth - imageLibraryFrameWidth.Width - imageLibraryFrame2.Width, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                        ImageLibraryTexture.DrawTiled(ImageLibraryIndexTexture, area: rectangle2, ref lockedSurface);
                        break;
                    case Style.Hollow:
                        SizedRectangle rectangle3 = new(WinLeft, WinTop, WinWidth, WinHeight);
                        ImageLibraryFrame.DrawFrame(ImageLibraryIndexFrame, area: rectangle3, ref lockedSurface);
                        break;
                    case Style.Borderless:
                        SizedRectangle rectangle4 = new(WinLeft, WinTop, WinWidth, WinHeight);
                        ImageLibraryTexture.DrawTiled(ImageLibraryIndexTexture, area: rectangle4, ref lockedSurface);
                        break;
                    case Style.Blank:
                    default:
                        break;
                }

                RaiseDrawBack(this, surfDesc: lockedSurface);

                SurfaceValid = false;
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during Draw", caption: $"Error - {Name}", flags: 0);
        }
    }

    // AoWWindow.pas:253
    // { CheckMouseDown: a click brings the window to the front; the controls get it first; a left
    //   click in the title area starts dragging, one on a border (Resizable) starts resizing }
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = false;
        Redraw = false;
        Dragging = 0;
        if (WindowManager is not null && !WindowManager.Handled && Visible)
        {
            if (X > WinLeft - 1 && X < WinLeft + WinWidth && Y > WinTop - 1 && Y < WinTop + WinHeight)
            {
                SetPriority(Priority);
            }

            for (int i = Children.Count - 1; i >= 0; i--)
            {
                UIComponent? c = Children[i];
                c.Handled = false;
                if ((!Handled || c.Redraw) && c.Visible && c.CaptureInput)
                {
                    c.CheckMouseDown(button, shift, X, Y);
                }

                if (c.Handled)
                {
                    c.Redraw = true;
                    Handled = true;
                    Redraw = true;
                }
            }

            if (!Handled && X > WinLeft - 1 && X < WinLeft + WinWidth && Y > WinTop - 1 && Y < WinTop + WinHeight)
            {
                DragX = 0;
                DragY = 0;
                if (button == MouseButton.Left && Draggable && Y < WinTop + BorderHeight + DragHeight - 1)
                {
                    Dragging = 1;
                    DragX = X - WinLeft;
                    DragY = Y - WinTop;
                }

                if (button == MouseButton.Left && Resizable)
                {
                    ResizingX = 0;
                    ResizingY = 0;
                    if (X < WinLeft + BorderWidth - 1)
                    {
                        Dragging = 2;
                        ResizingX = 1;
                        DragX = X - WinLeft;
                        if (Y < (BorderHeight * 5) + WinTop - 1)
                        {
                            ResizingY = 1;
                            DragY = Y - WinTop;
                        }

                        if (Y > WinTop + WinHeight - (BorderHeight * 5))
                        {
                            ResizingY = 2;
                            DragY = WinTop + WinHeight - Y;
                        }
                    }

                    if (X > WinLeft + WinWidth - BorderHeight)
                    {
                        Dragging = 2;
                        ResizingX = 2;
                        DragX = WinLeft + WinWidth - X;
                        if (Y < (BorderHeight * 5) + WinTop - 1)
                        {
                            ResizingY = 1;
                            DragY = Y - WinTop;
                        }

                        if (Y > WinTop + WinHeight - (BorderHeight * 5))
                        {
                            ResizingY = 2;
                            DragY = WinTop + WinHeight - Y;
                        }
                    }

                    if (Y < WinTop + BorderHeight - 1)
                    {
                        Dragging = 2;
                        ResizingY = 1;
                        DragY = Y - WinTop;
                        if (X < (BorderWidth * 5) + WinLeft - 1)
                        {
                            ResizingX = 1;
                            DragX = X - WinLeft;
                        }

                        if (X > WinLeft + WinWidth - (BorderWidth * 5))
                        {
                            ResizingX = 2;
                            DragX = WinLeft + WinWidth - X;
                        }
                    }

                    if (Y > WinTop + WinHeight - BorderHeight)
                    {
                        Dragging = 2;
                        ResizingY = 2;
                        DragY = WinTop + WinHeight - Y;
                        if (X < (BorderWidth * 5) + WinLeft - 1)
                        {
                            ResizingX = 1;
                            DragX = X - WinLeft;
                        }

                        if (X > WinLeft + WinWidth - (BorderWidth * 5))
                        {
                            ResizingX = 2;
                            DragX = WinLeft + WinWidth - X;
                        }
                    }
                }

                Redraw = true;
                Handled = true;
            }

            if (Handled)
            {
                if (WindowManager is not null)
                {
                    if (!WindowManager.Handled)
                    {
                        RaiseMouseDown(this, button, shift, X, Y);
                    }
                }
                else
                {
                    RaiseMouseDown(this, button, shift, X, Y);
                }
            }

            if (Modal)
            {
                Handled = true;
            }
        }
    }

    // AoWWindow.pas:376
    public override void CheckMouseUp(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = false;
        if (Visible && Valid)
        {
            Dragging = 0;
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                UIComponent? c = Children[i];
                c.Handled = false;
                if ((!Handled || c.Redraw) && c.Visible && c.CaptureInput)
                {
                    c.CheckMouseUp(button, shift, X, Y);
                }

                if (c.Handled)
                {
                    Handled = true;
                }
            }

            if (X > WinLeft - 1 && X < WinLeft + WinWidth && Y > WinTop - 1 && Y < WinTop + WinHeight)
            {
                Handled = true;
            }

            if (Handled)
            {
                if (!WindowManager.Handled)
                {
                    RaiseMouseUp(this, button, shift, X, Y);
                }
            }
            else
            {
                RaiseMouseUp(this, button, shift, X, Y);
            }

            if (Modal)
            {
                Handled = true;
            }
        }
    }

    // AoWWindow.pas:411
    // { CheckMouseMove drags or resizes the window, passes the move to the controls and sets the
    //   resize cursors over the borders }
    public override void CheckMouseMove(ShiftStates shift, int X, int Y)
    {
        Handled = false;
        OldCursor = Cursor;
        Cursor = WindowCursor;
        InResizeArea = true;
        if (Visible)
        {
            int i;
            switch (Dragging)
            {
                case 1:
                    SetWinLeftWithoutInvalidate(X - DragX);
                    SetWinTopWithoutInvalidate(Y - DragY);
                    if (WindowManager is not null && UseManagerBorders)
                    {
                        if (WindowManager.BorderLeft > WinLeft)
                        {
                            SetWinLeftWithoutInvalidate(WindowManager.BorderLeft);
                        }

                        if (WindowManager.WinWidth - WindowManager.BorderRight < WinLeft + WinWidth)
                        {
                            SetWinLeftWithoutInvalidate(WindowManager.WinWidth - WindowManager.BorderRight - WinWidth);
                        }

                        if (WindowManager.BorderTop > WinTop)
                        {
                            SetWinTopWithoutInvalidate(WindowManager.BorderTop);
                        }

                        if (WindowManager.WinHeight - WindowManager.BorderBottom < WinTop + WinHeight)
                        {
                            SetWinTopWithoutInvalidate(WindowManager.WinHeight - WindowManager.BorderBottom - WinHeight);
                        }
                    }

                    RaiseChange(this);

                    if (!DoubleBuffered)
                    {
                        Drawn = false;
                    }

                    SurfaceValid = false;
                    break;
                case 2:
                    bool resized = false;
                    switch (ResizingX)
                    {
                        case 1:
                            if (WinLeft + DragX > X)
                            {
                                if (WindowManager is not null && UseManagerBorders)
                                {
                                    if (WindowManager.BorderLeft > X - DragX)
                                    {
                                        X = WindowManager.BorderLeft + DragX;
                                    }
                                }
                            }

                            i = WinLeft + WinWidth;
                            SetWinWidthWithoutInvalidate(WinWidth - X + WinLeft + DragX);
                            if (WinWidth < MinWidth)
                            {
                                SetWinWidthWithoutInvalidate(MinWidth);
                            }

                            if (MaxWidth < WinWidth && MaxWidth > 0)
                            {
                                SetWinWidthWithoutInvalidate(MaxWidth);
                            }

                            SetWinLeftWithoutInvalidate(i - WinWidth);
                            resized = true;
                            break;
                        case 2:
                            SetWinWidthWithoutInvalidate(X + DragX - WinLeft);
                            if (WindowManager is not null && UseManagerBorders)
                            {
                                if (WindowManager.WinWidth - WindowManager.BorderRight < WinLeft + WinWidth)
                                {
                                    SetWinWidthWithoutInvalidate(WindowManager.WinWidth - WindowManager.BorderRight - WinLeft);
                                }
                            }

                            if (MinWidth > WinWidth)
                            {
                                SetWinWidthWithoutInvalidate(MinWidth);
                            }

                            if (MaxWidth < WinWidth && MaxWidth > 0)
                            {
                                SetWinWidthWithoutInvalidate(MaxWidth);
                            }

                            resized = true;
                            break;
                        default:
                            break;
                    }

                    switch (ResizingY)
                    {
                        case 1:
                            if (WinTop + DragY > Y)
                            {
                                if (WindowManager is not null && UseManagerBorders)
                                {
                                    if (WindowManager.BorderTop > Y - DragY)
                                    {
                                        Y = WindowManager.BorderTop + DragY;
                                    }
                                }
                            }

                            i = WinTop + WinHeight;
                            SetWinHeightWithoutInvalidate(WinHeight - Y + WinTop + DragY);
                            if (WinHeight < MinHeight)
                            {
                                SetWinHeightWithoutInvalidate(MinHeight);
                            }

                            if (MaxHeight < WinHeight && MaxHeight > 0)
                            {
                                SetWinHeightWithoutInvalidate(MaxHeight);
                            }

                            SetWinTopWithoutInvalidate(i - WinHeight);
                            resized = true;
                            break;
                        case 2:
                            SetWinHeightWithoutInvalidate(Y + DragY - WinTop);
                            if (WindowManager is not null && UseManagerBorders)
                            {
                                if (WindowManager.WinHeight - WindowManager.BorderBottom < WinTop + WinHeight)
                                {
                                    SetWinHeightWithoutInvalidate(WindowManager.WinHeight - WindowManager.BorderBottom - WinTop);
                                }
                            }

                            if (MinHeight > WinHeight)
                            {
                                SetWinHeightWithoutInvalidate(MinHeight);
                            }

                            if (MaxHeight < WinHeight && MaxHeight > 0)
                            {
                                SetWinHeightWithoutInvalidate(MaxHeight);
                            }

                            resized = true;
                            break;
                        default:
                            break;
                    }

                    if (resized)
                    {
                        RaiseResize(this);

                        Aligned = false;
                    }

                    break;
                default:
                    break;
            }

            i = Children.Count - 1;
            for (; i >= 0; i -= 1)
            {
                UIComponent? c = Children[i];
                c.Handled = false;
                if ((!Handled || c.Redraw || c.ToolTipCount > 0) && c.Visible && c.CaptureInput)
                {
                    c.CheckMouseMove(shift, X, Y);
                }

                if (c.Handled)
                {
                    Handled = true;
                }
            }

            if (!Handled && X > WinLeft - 1 && X < WinLeft + WinWidth && Y > WinTop - 1 && Y < WinTop + WinHeight)
            {
                Handled = true;
                if (Resizable)
                {
                    if (X < WinLeft + BorderWidth - 1)
                    {
                        Cursor = -9;
                        if (Y < (BorderHeight * 5) + WinTop - 1)
                        {
                            Cursor = -8;
                        }

                        if (Y > WinTop + WinHeight - (BorderHeight * 5))
                        {
                            Cursor = -6;
                        }
                    }
                    else
                    {
                        if (X > WinLeft + WinWidth - BorderWidth)
                        {
                            Cursor = -9;
                            if (Y < (BorderHeight * 5) + WinTop - 1)
                            {
                                Cursor = -6;
                            }

                            if (Y > WinTop + WinHeight - (BorderHeight * 5))
                            {
                                Cursor = -8;
                            }
                        }
                        else
                        {
                            if (Y < WinTop + BorderHeight - 1)
                            {
                                Cursor = -7;
                                if (X < (BorderWidth * 5) + WinLeft - 1)
                                {
                                    Cursor = -8;
                                }

                                if (X > WinLeft + WinWidth - (BorderWidth * 5))
                                {
                                    Cursor = -6;
                                }
                            }
                            else
                            {
                                if (Y > WinTop + WinHeight - BorderHeight)
                                {
                                    Cursor = -7;
                                    if (X < (BorderWidth * 5) + WinLeft - 1)
                                    {
                                        Cursor = -6;
                                    }

                                    if (X > WinLeft + WinWidth - (BorderWidth * 5))
                                    {
                                        Cursor = -8;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (Handled)
            {
                if (WindowManager is not null)
                {
                    if (!WindowManager.Handled)
                    {
                        RaiseMouseMove(this, shift, X, Y);
                    }
                }
                else
                {
                    RaiseMouseMove(this, shift, X, Y);
                }
            }

            if (Modal)
            {
                Handled = true;
            }
        }

        if (Cursor != OldCursor)
        {
            RaiseChangeCursor(this);
        }
    }

    [Published]
    public event NotifyEvent<Window> Drag
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, Window>(this, slot: nameof(Drag), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(Drag), value: value);
    }

    [Published]
    public new event NotifyEvent<Window> Resize
    {
        add => ((WindowControl)this).Resize += EventSlot.Add<WindowControl, Window>(this, slot: nameof(Resize), value: value);
        remove => ((WindowControl)this).Resize -= EventSlot.Remove<WindowControl, Window>(this, slot: nameof(Resize), value: value);
    }

    [Published]
    public new event NotifyEvent<Window> InternalResize
    {
        add => ((WindowControl)this).InternalResize += EventSlot.Add<WindowControl, Window>(this, slot: nameof(InternalResize), value: value);
        remove => ((WindowControl)this).InternalResize -= EventSlot.Remove<WindowControl, Window>(this, slot: nameof(InternalResize), value: value);
    }

    public new event NotifyEvent<Window> Change
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, Window>(this, slot: nameof(Change), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(Change), value: value);
    }

    public new event MouseEvent<Window> MouseDown
    {
        add => ((UIComponent)this).MouseDown += EventSlot.Add<UIComponent, Window>(this, slot: nameof(MouseDown), value: value);
        remove => ((UIComponent)this).MouseDown -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(MouseDown), value: value);
    }

    public new event MouseEvent<Window> MouseUp
    {
        add => ((UIComponent)this).MouseUp += EventSlot.Add<UIComponent, Window>(this, slot: nameof(MouseUp), value: value);
        remove => ((UIComponent)this).MouseUp -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(MouseUp), value: value);
    }

    public new event MouseMoveEvent<Window> MouseMove
    {
        add => ((UIComponent)this).MouseMove += EventSlot.Add<UIComponent, Window>(this, slot: nameof(MouseMove), value: value);
        remove => ((UIComponent)this).MouseMove -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(MouseMove), value: value);
    }

    public new event KeyPressEvent<Window> KeyPress
    {
        add => ((UIComponent)this).KeyPress += EventSlot.Add<UIComponent, Window>(this, slot: nameof(KeyPress), value: value);
        remove => ((UIComponent)this).KeyPress -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(KeyPress), value: value);
    }

    public new event KeyEvent<Window> KeyDown
    {
        add => ((UIComponent)this).KeyDown += EventSlot.Add<UIComponent, Window>(this, slot: nameof(KeyDown), value: value);
        remove => ((UIComponent)this).KeyDown -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(KeyDown), value: value);
    }

    public new event AoWOnDrawEvent<Window> PreDraw
    {
        add => ((UIComponent)this).PreDraw += EventSlot.Add<UIComponent, Window>(this, slot: nameof(PreDraw), value: value);
        remove => ((UIComponent)this).PreDraw -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(PreDraw), value: value);
    }

    public new event AoWOnDrawEvent<Window> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, Window>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<Window> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, Window>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(Updated), value: value);
    }

    public new event AoWOnDrawBackEvent<Window> DrawBack
    {
        add => ((WindowControl)this).DrawBack += EventSlot.Add<WindowControl, Window>(this, slot: nameof(DrawBack), value: value);
        remove => ((WindowControl)this).DrawBack -= EventSlot.Remove<WindowControl, Window>(this, slot: nameof(DrawBack), value: value);
    }

    public new event NotifyEvent<Window> Enter
    {
        add => ((WindowControl)this).Enter += EventSlot.Add<WindowControl, Window>(this, slot: nameof(Enter), value: value);
        remove => ((WindowControl)this).Enter -= EventSlot.Remove<WindowControl, Window>(this, slot: nameof(Enter), value: value);
    }

    public new event NotifyEvent<Window> Exit
    {
        add => ((WindowControl)this).Exit += EventSlot.Add<WindowControl, Window>(this, slot: nameof(Exit), value: value);
        remove => ((WindowControl)this).Exit -= EventSlot.Remove<WindowControl, Window>(this, slot: nameof(Exit), value: value);
    }

    public new event NotifyEvent<Window> ChangeCursor
    {
        add => ((WindowControl)this).ChangeCursor += EventSlot.Add<WindowControl, Window>(this, slot: nameof(ChangeCursor), value: value);
        remove => ((WindowControl)this).ChangeCursor -= EventSlot.Remove<WindowControl, Window>(this, slot: nameof(ChangeCursor), value: value);
    }

    public new event AoWOnDrawSurface<Window> SurfaceDrawn
    {
        add => ((WindowControl)this).SurfaceDrawn += EventSlot.Add<WindowControl, Window>(this, slot: nameof(SurfaceDrawn), value: value);
        remove => ((WindowControl)this).SurfaceDrawn -= EventSlot.Remove<WindowControl, Window>(this, slot: nameof(SurfaceDrawn), value: value);
    }

    public new event AoWOnDrawSurface<Window> PreDrawSurface
    {
        add => ((WindowControl)this).PreDrawSurface += EventSlot.Add<WindowControl, Window>(this, slot: nameof(PreDrawSurface), value: value);
        remove => ((WindowControl)this).PreDrawSurface -= EventSlot.Remove<WindowControl, Window>(this, slot: nameof(PreDrawSurface), value: value);
    }

    public new event NotifyEvent<Window> Hide
    {
        add => ((BaseWindow)this).Hide += EventSlot.Add<BaseWindow, Window>(this, slot: nameof(Hide), value: value);
        remove => ((BaseWindow)this).Hide -= EventSlot.Remove<BaseWindow, Window>(this, slot: nameof(Hide), value: value);
    }

    public new event NotifyEvent<Window> Show
    {
        add => ((BaseWindow)this).Show += EventSlot.Add<BaseWindow, Window>(this, slot: nameof(Show), value: value);
        remove => ((BaseWindow)this).Show -= EventSlot.Remove<BaseWindow, Window>(this, slot: nameof(Show), value: value);
    }
}
