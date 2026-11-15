#nullable disable
// Translated from AoWWindow.pas.
using Aow.Game.Draw;
using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

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
    public Window(Component owner) : base(owner) => UseManagerBorders = true;

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
                if (ImageLibraryFrame != null && (ImageLibraryTexture != null || WindowStyle == Style.Hollow))
                {
                    var imageLibraryFrameWidth = ImageLibraryFrame[ImageLibraryIndexFrame];
                    var imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
                    if (imageLibraryFrameWidth.Width + imageLibraryFrame.Width > WinWidth)
                    {
                        var imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame];
                        var imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
                        SetWinWidthWithoutInvalidate(imageLibraryFrame2.Width + imageLibraryFrame3.Width);
                    }

                    var imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame];
                    var imageLibraryFrame4 = ImageLibraryFrame[ImageLibraryIndexFrame + 5];
                    if (imageLibraryFrameHeight.Height + imageLibraryFrame4.Height > WinHeight)
                    {
                        var imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame];
                        var imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 5];
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
                            var imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
                            FrameRight = WinWidth - imageLibraryFrame2.Width;
                            FrameTop = ImageLibraryFrame[ImageLibraryIndexFrame + 1].Height;
                            var imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
                            FrameBottom = WinHeight - imageLibraryFrame3.Height;
                        }
                    }
                }

                break;
            case Style.Borderless:
            case Style.Blank:
                if (ImageLibraryTexture != null || WindowStyle == Style.Blank)
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
        }

        if (DoubleBuffered && BackSurface != null)
        {
            BackSurface.Resize(WinWidth, WinHeight);
            if (Masked)
            {
                BackSurface.Clear(MaskColor);
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
                        var rectangle = new SizedRectangle(WinLeft, WinTop, WinWidth, WinHeight);
                        ImageLibraryFrame.DrawFrame(ImageLibraryIndexFrame, rectangle, ref lockedSurface);
                        var imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                        var imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
                        var imageLibraryFrameWidth = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                        var imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
                        var imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                        var imageLibraryFrame4 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                        var rectangle2 = new SizedRectangle(WinLeft + imageLibraryFrame4.Width, WinTop + imageLibraryFrame3.Height, WinWidth - imageLibraryFrameWidth.Width - imageLibraryFrame2.Width, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                        ImageLibraryTexture.DrawTiled(ImageLibraryIndexTexture, rectangle2, ref lockedSurface);
                        break;
                    case Style.Hollow:
                        var rectangle3 = new SizedRectangle(WinLeft, WinTop, WinWidth, WinHeight);
                        ImageLibraryFrame.DrawFrame(ImageLibraryIndexFrame, rectangle3, ref lockedSurface);
                        break;
                    case Style.Borderless:
                        var rectangle4 = new SizedRectangle(WinLeft, WinTop, WinWidth, WinHeight);
                        ImageLibraryTexture.DrawTiled(ImageLibraryIndexTexture, rectangle4, ref lockedSurface);
                        break;
                }

                RaiseDrawBack(this, lockedSurface);

                SurfaceValid = false;
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
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        UIComponent c = default;
        Handled = false;
        Redraw = false;
        Dragging = 0;
        if (WindowManager != null && !WindowManager.Handled && Visible)
        {
            if (X > WinLeft - 1 && X < WinLeft + WinWidth && Y > WinTop - 1 && Y < WinTop + WinHeight)
            {
                SetPriority(Priority);
            }

            for (int i = Children.Count - 1; i >= 0; i--)
            {
                c = Children[i];
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
                if (WindowManager != null)
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
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        UIComponent c = default;
        Handled = false;
        if (Visible && Valid)
        {
            Dragging = 0;
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                c = Children[i];
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
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        bool resized;
        int i = default;
        UIComponent c = default;
        Handled = false;
        OldCursor = Cursor;
        Cursor = WindowCursor;
        InResizeArea = true;
        resized = false;
        if (Visible)
        {
            switch (Dragging)
            {
                case 1:
                    SetWinLeftWithoutInvalidate(X - DragX);
                    SetWinTopWithoutInvalidate(Y - DragY);
                    if (WindowManager != null && UseManagerBorders)
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
                    switch (ResizingX)
                    {
                        case 1:
                            if (WinLeft + DragX > X)
                            {
                                if (WindowManager != null && UseManagerBorders)
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
                            if (WindowManager != null && UseManagerBorders)
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
                    }

                    switch (ResizingY)
                    {
                        case 1:
                            if (WinTop + DragY > Y)
                            {
                                if (WindowManager != null && UseManagerBorders)
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
                            if (WindowManager != null && UseManagerBorders)
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
                    }

                    if (resized)
                    {
                        RaiseResize(this);

                        Aligned = false;
                    }

                    break;
            }

            i = Children.Count - 1;
            for (; i >= 0; i -= 1)
            {
                c = Children[i];
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
                if (WindowManager != null)
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
    public event NotifyEvent<Window> OnDrag
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, Window>(this, slot: nameof(OnDrag), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(OnDrag), value: value);
    }

    [Published]
    public new event NotifyEvent<Window> OnResize
    {
        add => ((WindowControl)this).OnResize += EventSlot.Add<WindowControl, Window>(this, slot: nameof(OnResize), value: value);
        remove => ((WindowControl)this).OnResize -= EventSlot.Remove<WindowControl, Window>(this, slot: nameof(OnResize), value: value);
    }

    [Published]
    public new event NotifyEvent<Window> OnInternalResize
    {
        add => ((WindowControl)this).OnInternalResize += EventSlot.Add<WindowControl, Window>(this, slot: nameof(OnInternalResize), value: value);
        remove => ((WindowControl)this).OnInternalResize -= EventSlot.Remove<WindowControl, Window>(this, slot: nameof(OnInternalResize), value: value);
    }

    public new event NotifyEvent<Window> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, Window>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<Window> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, Window>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<Window> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, Window>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<Window> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, Window>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<Window> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, Window>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<Window> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, Window>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<Window> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, Window>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<Window> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, Window>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<Window> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, Window>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, Window>(this, slot: nameof(OnUpdate), value: value);
    }

    public new event AoWOnDrawBackEvent<Window> OnDrawBack
    {
        add => ((WindowControl)this).OnDrawBack += EventSlot.Add<WindowControl, Window>(this, slot: nameof(OnDrawBack), value: value);
        remove => ((WindowControl)this).OnDrawBack -= EventSlot.Remove<WindowControl, Window>(this, slot: nameof(OnDrawBack), value: value);
    }

    public new event NotifyEvent<Window> OnEnter
    {
        add => ((WindowControl)this).OnEnter += EventSlot.Add<WindowControl, Window>(this, slot: nameof(OnEnter), value: value);
        remove => ((WindowControl)this).OnEnter -= EventSlot.Remove<WindowControl, Window>(this, slot: nameof(OnEnter), value: value);
    }

    public new event NotifyEvent<Window> OnExit
    {
        add => ((WindowControl)this).OnExit += EventSlot.Add<WindowControl, Window>(this, slot: nameof(OnExit), value: value);
        remove => ((WindowControl)this).OnExit -= EventSlot.Remove<WindowControl, Window>(this, slot: nameof(OnExit), value: value);
    }

    public new event NotifyEvent<Window> OnChangeCursor
    {
        add => ((WindowControl)this).OnChangeCursor += EventSlot.Add<WindowControl, Window>(this, slot: nameof(OnChangeCursor), value: value);
        remove => ((WindowControl)this).OnChangeCursor -= EventSlot.Remove<WindowControl, Window>(this, slot: nameof(OnChangeCursor), value: value);
    }

    public new event AoWOnDrawSurface<Window> OnDrawSurface
    {
        add => ((WindowControl)this).OnDrawSurface += EventSlot.Add<WindowControl, Window>(this, slot: nameof(OnDrawSurface), value: value);
        remove => ((WindowControl)this).OnDrawSurface -= EventSlot.Remove<WindowControl, Window>(this, slot: nameof(OnDrawSurface), value: value);
    }

    public new event AoWOnDrawSurface<Window> OnPreDrawSurface
    {
        add => ((WindowControl)this).OnPreDrawSurface += EventSlot.Add<WindowControl, Window>(this, slot: nameof(OnPreDrawSurface), value: value);
        remove => ((WindowControl)this).OnPreDrawSurface -= EventSlot.Remove<WindowControl, Window>(this, slot: nameof(OnPreDrawSurface), value: value);
    }

    public new event NotifyEvent<Window> OnHide
    {
        add => ((BaseWindow)this).OnHide += EventSlot.Add<BaseWindow, Window>(this, slot: nameof(OnHide), value: value);
        remove => ((BaseWindow)this).OnHide -= EventSlot.Remove<BaseWindow, Window>(this, slot: nameof(OnHide), value: value);
    }

    public new event NotifyEvent<Window> OnShow
    {
        add => ((BaseWindow)this).OnShow += EventSlot.Add<BaseWindow, Window>(this, slot: nameof(OnShow), value: value);
        remove => ((BaseWindow)this).OnShow -= EventSlot.Remove<BaseWindow, Window>(this, slot: nameof(OnShow), value: value);
    }
}
