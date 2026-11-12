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
    public Window(Component owner) : base(owner)
    {
        _useManagerBorders = true;
    }

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
                if (_imageLibraryFrame != null && (_imageLibraryTexture != null || WindowStyle == Style.Hollow))
                {
                    var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame];
                    var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                    if (imageLibraryFrameWidth.Width + imageLibraryFrame.Width > WinWidth)
                    {
                        var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                        var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                        SetWinWidthWithoutInvalidate(imageLibraryFrame2.Width + imageLibraryFrame3.Width);
                    }

                    var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame];
                    var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                    if (imageLibraryFrameHeight.Height + imageLibraryFrame4.Height > WinHeight)
                    {
                        var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                        var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                        SetWinHeightWithoutInvalidate(imageLibraryFrame2.Height + imageLibraryFrame3.Height);
                    }

                    if (_minWidth > 0 && _minWidth > WinWidth)
                    {
                        SetWinWidthWithoutInvalidate(_minWidth);
                    }

                    if (_minHeight > 0 && _minHeight > WinHeight)
                    {
                        SetWinHeightWithoutInvalidate(_minHeight);
                    }

                    if (_maxWidth > 0 && _maxWidth < WinWidth)
                    {
                        SetWinWidthWithoutInvalidate(_maxWidth);
                    }

                    if (_maxHeight > 0 && _maxHeight < WinHeight)
                    {
                        SetWinHeightWithoutInvalidate(_maxHeight);
                    }

                    if (_windowManager.WinWidth > 0 && _windowManager.WinHeight > 0)
                    {
                        if (_useManagerBorders)
                        {
                            if (_windowManager.WinWidth - _windowManager.BorderRight < WinLeft + WinWidth)
                            {
                                SetWinLeftWithoutInvalidate(_windowManager.WinWidth - _windowManager.BorderRight - WinWidth);
                            }

                            if (_windowManager.BorderLeft > WinLeft)
                            {
                                SetWinLeftWithoutInvalidate(_windowManager.BorderLeft);
                                if (_windowManager.WinWidth - _windowManager.BorderRight < WinLeft + WinWidth)
                                {
                                    SetWinWidthWithoutInvalidate(_windowManager.WinWidth - _windowManager.BorderRight - WinLeft);
                                }
                            }

                            if (_windowManager.WinHeight - _windowManager.BorderBottom < WinTop + WinHeight)
                            {
                                SetWinTopWithoutInvalidate(_windowManager.WinHeight - _windowManager.BorderBottom - WinHeight);
                            }

                            if (_windowManager.BorderTop > WinTop)
                            {
                                SetWinTopWithoutInvalidate(_windowManager.BorderTop);
                                if (_windowManager.WinHeight - _windowManager.BorderBottom < WinTop + WinHeight)
                                {
                                    SetWinHeightWithoutInvalidate(_windowManager.WinHeight - _windowManager.BorderBottom - WinTop);
                                }
                            }
                        }
                        else
                        {
                            if (_windowManager.WinWidth < WinLeft + WinWidth)
                            {
                                SetWinLeftWithoutInvalidate(_windowManager.WinWidth - WinWidth);
                            }

                            if (WinLeft < 0)
                            {
                                SetWinLeftWithoutInvalidate(0);
                                if (_windowManager.WinWidth < WinLeft + WinWidth)
                                {
                                    SetWinWidthWithoutInvalidate(_windowManager.WinWidth - WinLeft);
                                }
                            }

                            if (_windowManager.WinHeight < WinTop + WinHeight)
                            {
                                SetWinTopWithoutInvalidate(_windowManager.WinHeight - WinHeight);
                            }

                            if (WinTop < 0)
                            {
                                SetWinTopWithoutInvalidate(0);
                                if (_windowManager.WinHeight < WinTop + WinHeight)
                                {
                                    SetWinHeightWithoutInvalidate(_windowManager.WinHeight - WinTop);
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
                            FrameLeft = _imageLibraryFrame[_imageLibraryIndexFrame + 3].Width;
                            var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                            FrameRight = WinWidth - imageLibraryFrame2.Width;
                            FrameTop = _imageLibraryFrame[_imageLibraryIndexFrame + 1].Height;
                            var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                            FrameBottom = WinHeight - imageLibraryFrame3.Height;
                        }
                    }
                }

                break;
            case Style.Borderless:
            case Style.Blank:
                if (_imageLibraryTexture != null || WindowStyle == Style.Blank)
                {
                    if (_minWidth > 0 && _minWidth > WinWidth)
                    {
                        SetWinWidthWithoutInvalidate(_minWidth);
                    }

                    if (_minHeight > 0 && _minHeight > WinHeight)
                    {
                        SetWinHeightWithoutInvalidate(_minHeight);
                    }

                    if (_maxWidth > 0 && _maxWidth < WinWidth)
                    {
                        SetWinWidthWithoutInvalidate(_maxWidth);
                    }

                    if (_maxHeight > 0 && _maxHeight < WinHeight)
                    {
                        SetWinHeightWithoutInvalidate(_maxHeight);
                    }

                    if (_windowManager.WinWidth > 0 && _windowManager.WinHeight > 0)
                    {
                        if (_useManagerBorders)
                        {
                            if (_windowManager.WinWidth - _windowManager.BorderRight < WinLeft + WinWidth)
                            {
                                SetWinLeftWithoutInvalidate(_windowManager.WinWidth - _windowManager.BorderRight - WinWidth);
                            }

                            if (_windowManager.BorderLeft > WinLeft)
                            {
                                SetWinLeftWithoutInvalidate(_windowManager.BorderLeft);
                                if (_windowManager.WinWidth - _windowManager.BorderRight < WinLeft + WinWidth)
                                {
                                    SetWinWidthWithoutInvalidate(_windowManager.WinWidth - _windowManager.BorderRight - WinLeft);
                                }
                            }

                            if (_windowManager.WinHeight - _windowManager.BorderBottom < WinTop + WinHeight)
                            {
                                SetWinTopWithoutInvalidate(_windowManager.WinHeight - _windowManager.BorderBottom - WinHeight);
                            }

                            if (_windowManager.BorderTop > WinTop)
                            {
                                SetWinTopWithoutInvalidate(_windowManager.BorderTop);
                                if (_windowManager.WinHeight - _windowManager.BorderBottom < WinTop + WinHeight)
                                {
                                    SetWinHeightWithoutInvalidate(_windowManager.WinHeight - _windowManager.BorderBottom - WinTop);
                                }
                            }
                        }
                        else
                        {
                            if (_windowManager.WinWidth < WinLeft + WinWidth)
                            {
                                SetWinLeftWithoutInvalidate(_windowManager.WinWidth - WinWidth);
                            }

                            if (WinLeft < 0)
                            {
                                SetWinLeftWithoutInvalidate(0);
                                if (_windowManager.WinWidth < WinLeft + WinWidth)
                                {
                                    SetWinWidthWithoutInvalidate(_windowManager.WinWidth - WinLeft);
                                }
                            }

                            if (_windowManager.WinHeight < WinTop + WinHeight)
                            {
                                SetWinTopWithoutInvalidate(_windowManager.WinHeight - WinHeight);
                            }

                            if (WinTop < 0)
                            {
                                SetWinTopWithoutInvalidate(0);
                                if (_windowManager.WinHeight < WinTop + WinHeight)
                                {
                                    SetWinHeightWithoutInvalidate(_windowManager.WinHeight - WinTop);
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

        if (_doubleBuffered && BackSurface != null)
        {
            BackSurface.Resize(WinWidth, WinHeight);
            if (Masked)
            {
                BackSurface.Clear(_maskColor);
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
        if (_useManagerBorders)
        {
            if (Alignment.AlignWidth == AlignWidth.Center)
            {
                SetWinLeftWithoutInvalidate((OwnerComponent.WinWidth - _windowManager.BorderLeft - _windowManager.BorderRight) / 2 + _windowManager.BorderLeft - WinWidth / 2);
            }

            if (Alignment.AlignHeight == AlignHeight.Center)
            {
                SetWinTopWithoutInvalidate((OwnerComponent.WinHeight - _windowManager.BorderTop - _windowManager.BorderBottom) / 2 + _windowManager.BorderTop - WinHeight / 2);
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
                        var rectangle = new Rect(WinLeft, WinTop, WinWidth, WinHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle, ref lockedSurface);
                        var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                        var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var rectangle2 = new Rect(WinLeft + imageLibraryFrame4.Width, WinTop + imageLibraryFrame3.Height, WinWidth - imageLibraryFrameWidth.Width - imageLibraryFrame2.Width, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                        _imageLibraryTexture.DrawTiled(_imageLibraryIndexTexture, rectangle2, ref lockedSurface);
                        break;
                    case Style.Hollow:
                        var rectangle3 = new Rect(WinLeft, WinTop, WinWidth, WinHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle3, ref lockedSurface);
                        break;
                    case Style.Borderless:
                        var rectangle4 = new Rect(WinLeft, WinTop, WinWidth, WinHeight);
                        _imageLibraryTexture.DrawTiled(_imageLibraryIndexTexture, rectangle4, ref lockedSurface);
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
        UIComponent C = default;
        Handled = false;
        Redraw = false;
        Dragging = 0;
        if (_windowManager != null && !_windowManager.Handled && Visible == true)
        {
            if (X > WinLeft - 1 && X < WinLeft + WinWidth && Y > WinTop - 1 && Y < WinTop + WinHeight)
            {
                SetPriority(Priority);
            }

            for (int I = Children.Count - 1; I >= 0; I--)
            {
                C = Children[I];
                C.Handled = false;
                if ((!Handled || C.Redraw) && C.Visible == true && C.CaptureInput)
                {
                    C.CheckMouseDown(button, shift, X, Y);
                }

                if (C.Handled == true)
                {
                    C.Redraw = true;
                    Handled = true;
                    Redraw = true;
                }
            }

            if (!Handled && X > WinLeft - 1 && X < WinLeft + WinWidth && Y > WinTop - 1 && Y < WinTop + WinHeight)
            {
                DragX = 0;
                DragY = 0;
                if (button == MouseButton.Left && _draggable == true && Y < WinTop + _borderHeight + _dragHeight - 1)
                {
                    Dragging = 1;
                    DragX = X - WinLeft;
                    DragY = Y - WinTop;
                }

                if (button == MouseButton.Left && _resizable == true)
                {
                    ResizingX = 0;
                    ResizingY = 0;
                    if (X < WinLeft + _borderWidth - 1)
                    {
                        Dragging = 2;
                        ResizingX = 1;
                        DragX = X - WinLeft;
                        if (Y < _borderHeight * 5 + WinTop - 1)
                        {
                            ResizingY = 1;
                            DragY = Y - WinTop;
                        }

                        if (Y > WinTop + WinHeight - _borderHeight * 5)
                        {
                            ResizingY = 2;
                            DragY = WinTop + WinHeight - Y;
                        }
                    }

                    if (X > WinLeft + WinWidth - _borderHeight)
                    {
                        Dragging = 2;
                        ResizingX = 2;
                        DragX = WinLeft + WinWidth - X;
                        if (Y < _borderHeight * 5 + WinTop - 1)
                        {
                            ResizingY = 1;
                            DragY = Y - WinTop;
                        }

                        if (Y > WinTop + WinHeight - _borderHeight * 5)
                        {
                            ResizingY = 2;
                            DragY = WinTop + WinHeight - Y;
                        }
                    }

                    if (Y < WinTop + _borderHeight - 1)
                    {
                        Dragging = 2;
                        ResizingY = 1;
                        DragY = Y - WinTop;
                        if (X < _borderWidth * 5 + WinLeft - 1)
                        {
                            ResizingX = 1;
                            DragX = X - WinLeft;
                        }

                        if (X > WinLeft + WinWidth - _borderWidth * 5)
                        {
                            ResizingX = 2;
                            DragX = WinLeft + WinWidth - X;
                        }
                    }

                    if (Y > WinTop + WinHeight - _borderHeight)
                    {
                        Dragging = 2;
                        ResizingY = 2;
                        DragY = WinTop + WinHeight - Y;
                        if (X < _borderWidth * 5 + WinLeft - 1)
                        {
                            ResizingX = 1;
                            DragX = X - WinLeft;
                        }

                        if (X > WinLeft + WinWidth - _borderWidth * 5)
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
                if (_windowManager != null)
                {
                    if (!_windowManager.Handled)
                    {
                        RaiseMouseDown(this, button, shift, X, Y);
                    }
                }
                else
                {
                    RaiseMouseDown(this, button, shift, X, Y);
                }
            }

            if (_modal)
            {
                Handled = true;
            }
        }
    }

    // AoWWindow.pas:376
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        UIComponent C = default;
        Handled = false;
        if (Visible && Valid)
        {
            Dragging = 0;
            for (int I = Children.Count - 1; I >= 0; I--)
            {
                C = Children[I];
                C.Handled = false;
                if ((!Handled || C.Redraw == true) && C.Visible == true && C.CaptureInput)
                {
                    C.CheckMouseUp(button, shift, X, Y);
                }

                if (C.Handled == true)
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
                if (!_windowManager.Handled)
                {
                    RaiseMouseUp(this, button, shift, X, Y);
                }
            }
            else
            {
                RaiseMouseUp(this, button, shift, X, Y);
            }

            if (_modal)
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
        int I = default;
        UIComponent C = default;
        Handled = false;
        OldCursor = _cursor;
        _cursor = WindowCursor;
        InResizeArea = true;
        resized = false;
        if (Visible == true)
        {
            switch (Dragging)
            {
                case 1:
                    SetWinLeftWithoutInvalidate(X - DragX);
                    SetWinTopWithoutInvalidate(Y - DragY);
                    if (_windowManager != null && _useManagerBorders)
                    {
                        if (_windowManager.BorderLeft > WinLeft)
                        {
                            SetWinLeftWithoutInvalidate(_windowManager.BorderLeft);
                        }

                        if (_windowManager.WinWidth - _windowManager.BorderRight < WinLeft + WinWidth)
                        {
                            SetWinLeftWithoutInvalidate(_windowManager.WinWidth - _windowManager.BorderRight - WinWidth);
                        }

                        if (_windowManager.BorderTop > WinTop)
                        {
                            SetWinTopWithoutInvalidate(_windowManager.BorderTop);
                        }

                        if (_windowManager.WinHeight - _windowManager.BorderBottom < WinTop + WinHeight)
                        {
                            SetWinTopWithoutInvalidate(_windowManager.WinHeight - _windowManager.BorderBottom - WinHeight);
                        }
                    }

                    RaiseChange(this);

                    if (!_doubleBuffered)
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
                                if (_windowManager != null && _useManagerBorders)
                                {
                                    if (_windowManager.BorderLeft > X - DragX)
                                    {
                                        X = _windowManager.BorderLeft + DragX;
                                    }
                                }
                            }

                            I = WinLeft + WinWidth;
                            SetWinWidthWithoutInvalidate(WinWidth - X + WinLeft + DragX);
                            if (WinWidth < _minWidth)
                            {
                                SetWinWidthWithoutInvalidate(_minWidth);
                            }

                            if (_maxWidth < WinWidth && _maxWidth > 0)
                            {
                                SetWinWidthWithoutInvalidate(_maxWidth);
                            }

                            SetWinLeftWithoutInvalidate(I - WinWidth);
                            resized = true;
                            break;
                        case 2:
                            SetWinWidthWithoutInvalidate(X + DragX - WinLeft);
                            if (_windowManager != null && _useManagerBorders)
                            {
                                if (_windowManager.WinWidth - _windowManager.BorderRight < WinLeft + WinWidth)
                                {
                                    SetWinWidthWithoutInvalidate(_windowManager.WinWidth - _windowManager.BorderRight - WinLeft);
                                }
                            }

                            if (_minWidth > WinWidth)
                            {
                                SetWinWidthWithoutInvalidate(_minWidth);
                            }

                            if (_maxWidth < WinWidth && _maxWidth > 0)
                            {
                                SetWinWidthWithoutInvalidate(_maxWidth);
                            }

                            resized = true;
                            break;
                    }

                    switch (ResizingY)
                    {
                        case 1:
                            if (WinTop + DragY > Y)
                            {
                                if (_windowManager != null && _useManagerBorders)
                                {
                                    if (_windowManager.BorderTop > Y - DragY)
                                    {
                                        Y = _windowManager.BorderTop + DragY;
                                    }
                                }
                            }

                            I = WinTop + WinHeight;
                            SetWinHeightWithoutInvalidate(WinHeight - Y + WinTop + DragY);
                            if (WinHeight < _minHeight)
                            {
                                SetWinHeightWithoutInvalidate(_minHeight);
                            }

                            if (_maxHeight < WinHeight && _maxHeight > 0)
                            {
                                SetWinHeightWithoutInvalidate(_maxHeight);
                            }

                            SetWinTopWithoutInvalidate(I - WinHeight);
                            resized = true;
                            break;
                        case 2:
                            SetWinHeightWithoutInvalidate(Y + DragY - WinTop);
                            if (_windowManager != null && _useManagerBorders)
                            {
                                if (_windowManager.WinHeight - _windowManager.BorderBottom < WinTop + WinHeight)
                                {
                                    SetWinHeightWithoutInvalidate(_windowManager.WinHeight - _windowManager.BorderBottom - WinTop);
                                }
                            }

                            if (_minHeight > WinHeight)
                            {
                                SetWinHeightWithoutInvalidate(_minHeight);
                            }

                            if (_maxHeight < WinHeight && _maxHeight > 0)
                            {
                                SetWinHeightWithoutInvalidate(_maxHeight);
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

            I = Children.Count - 1;
            for (; I >= 0; I -= 1)
            {
                C = Children[I];
                C.Handled = false;
                if ((!Handled || C.Redraw == true || C.ToolTipCount > 0) && C.Visible == true && C.CaptureInput)
                {
                    C.CheckMouseMove(shift, X, Y);
                }

                if (C.Handled == true)
                {
                    Handled = true;
                }
            }

            if (!Handled && X > WinLeft - 1 && X < WinLeft + WinWidth && Y > WinTop - 1 && Y < WinTop + WinHeight)
            {
                Handled = true;
                if (_resizable == true)
                {
                    if (X < WinLeft + _borderWidth - 1)
                    {
                        _cursor = -9;
                        if (Y < _borderHeight * 5 + WinTop - 1)
                        {
                            _cursor = -8;
                        }

                        if (Y > WinTop + WinHeight - _borderHeight * 5)
                        {
                            _cursor = -6;
                        }
                    }
                    else
                    {
                        if (X > WinLeft + WinWidth - _borderWidth)
                        {
                            _cursor = -9;
                            if (Y < _borderHeight * 5 + WinTop - 1)
                            {
                                _cursor = -6;
                            }

                            if (Y > WinTop + WinHeight - _borderHeight * 5)
                            {
                                _cursor = -8;
                            }
                        }
                        else
                        {
                            if (Y < WinTop + _borderHeight - 1)
                            {
                                _cursor = -7;
                                if (X < _borderWidth * 5 + WinLeft - 1)
                                {
                                    _cursor = -8;
                                }

                                if (X > WinLeft + WinWidth - _borderWidth * 5)
                                {
                                    _cursor = -6;
                                }
                            }
                            else
                            {
                                if (Y > WinTop + WinHeight - _borderHeight)
                                {
                                    _cursor = -7;
                                    if (X < _borderWidth * 5 + WinLeft - 1)
                                    {
                                        _cursor = -6;
                                    }

                                    if (X > WinLeft + WinWidth - _borderWidth * 5)
                                    {
                                        _cursor = -8;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (Handled)
            {
                if (_windowManager != null)
                {
                    if (!_windowManager.Handled)
                    {
                        RaiseMouseMove(this, shift, X, Y);
                    }
                }
                else
                {
                    RaiseMouseMove(this, shift, X, Y);
                }
            }

            if (_modal)
            {
                Handled = true;
            }
        }

        if (_cursor != OldCursor)
        {
            RaiseChangeCursor(this);
        }
    }

    [Published]
    public int DragHeight { get => _dragHeight; set => _dragHeight = value; }

    [Published]
    public bool Draggable { get => _draggable; set => _draggable = value; }

    [Published]
    public bool Resizable { get => _resizable; set => _resizable = value; }

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

    [Published]
    public int MinWidth { get => _minWidth; set => _minWidth = value; }

    [Published]
    public int MinHeight { get => _minHeight; set => _minHeight = value; }

    [Published]
    public int MaxWidth { get => _maxWidth; set => _maxWidth = value; }

    [Published]
    public int MaxHeight { get => _maxHeight; set => _maxHeight = value; }

    [Published]
    public bool UseManagerBorders { get => _useManagerBorders; set => _useManagerBorders = value; }

    [Published]
    public bool Modal { get => _modal; set => _modal = value; }

    [Published]
    public bool Dockable { get => _dockable; set => _dockable = value; }

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
