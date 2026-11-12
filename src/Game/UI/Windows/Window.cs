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

        if (_onInternalResize != null)
        {
            _onInternalResize(this);
        }

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

                if (_onDrawBack != null)
                {
                    _onDrawBack(this, lockedSurface);
                }

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
                C = (UIComponent)Children[I];
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
                    if (!_windowManager.Handled && _onMouseDown != null)
                    {
                        _onMouseDown(this, button, shift, X, Y);
                    }
                }
                else
                {
                    if (_onMouseDown != null)
                    {
                        _onMouseDown(this, button, shift, X, Y);
                    }
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
                C = (UIComponent)Children[I];
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
                if (!_windowManager.Handled && _onMouseUp != null)
                {
                    _onMouseUp(this, button, shift, X, Y);
                }
            }
            else
            {
                if (_onMouseUp != null)
                {
                    _onMouseUp(this, button, shift, X, Y);
                }
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

                    if (_onChange != null)
                    {
                        _onChange(this);
                    }

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
                        if (_onResize != null)
                        {
                            _onResize(this);
                        }

                        Aligned = false;
                    }

                    break;
            }

            I = Children.Count - 1;
            for (; I >= 0; I -= 1)
            {
                C = (UIComponent)Children[I];
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
                    if (!_windowManager.Handled && _onMouseMove != null)
                    {
                        _onMouseMove(this, shift, X, Y);
                    }
                }
                else
                {
                    if (_onMouseMove != null)
                    {
                        _onMouseMove(this, shift, X, Y);
                    }
                }
            }

            if (_modal)
            {
                Handled = true;
            }
        }

        if (_cursor != OldCursor && _onChangeCursor != null)
        {
            _onChangeCursor(this);
        }
    }

    [Published]
    public int DragHeight { get => _dragHeight; set => _dragHeight = value; }

    [Published]
    public bool Draggable { get => _draggable; set => _draggable = value; }

    [Published]
    public bool Resizable { get => _resizable; set => _resizable = value; }

    [Published]
    public NotifyEvent<Window> OnDrag
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    [Published]
    public NotifyEvent<Window> OnResize
    {
        get => EventSlot.Get(this, _onResize);
        set => EventSlot.Set(this, ref _onResize, value);
    }

    [Published]
    public NotifyEvent<Window> OnInternalResize
    {
        get => EventSlot.Get(this, _onInternalResize);
        set => EventSlot.Set(this, ref _onInternalResize, value);
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

    public new NotifyEvent<Window> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<Window> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<Window> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<Window> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<Window> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<Window> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<Window> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<Window> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<Window> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }

    public new AoWOnDrawBackEvent<Window> OnDrawBack
    {
        get => EventSlot.Get(this, _onDrawBack);
        set => EventSlot.Set(this, ref _onDrawBack, value);
    }

    public new NotifyEvent<Window> OnEnter
    {
        get => EventSlot.Get(this, _onEnter);
        set => EventSlot.Set(this, ref _onEnter, value);
    }

    public new NotifyEvent<Window> OnExit
    {
        get => EventSlot.Get(this, _onExit);
        set => EventSlot.Set(this, ref _onExit, value);
    }

    public new NotifyEvent<Window> OnChangeCursor
    {
        get => EventSlot.Get(this, _onChangeCursor);
        set => EventSlot.Set(this, ref _onChangeCursor, value);
    }

    public new AoWOnDrawSurface<Window> OnDrawSurface
    {
        get => EventSlot.Get(this, _onDrawSurface);
        set => EventSlot.Set(this, ref _onDrawSurface, value);
    }

    public new AoWOnDrawSurface<Window> OnPreDrawSurface
    {
        get => EventSlot.Get(this, _onPreDrawSurface);
        set => EventSlot.Set(this, ref _onPreDrawSurface, value);
    }

    public new NotifyEvent<Window> OnHide
    {
        get => EventSlot.Get(this, _onHide);
        set => EventSlot.Set(this, ref _onHide, value);
    }

    public new NotifyEvent<Window> OnShow
    {
        get => EventSlot.Get(this, _onShow);
        set => EventSlot.Set(this, ref _onShow, value);
    }
}
