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
                    if (imageLibraryFrameWidth.Width + imageLibraryFrame.Width > _winWidth)
                    {
                        var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                        var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                        _winWidth = imageLibraryFrame2.Width + imageLibraryFrame3.Width;
                    }

                    var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame];
                    var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                    if (imageLibraryFrameHeight.Height + imageLibraryFrame4.Height > _winHeight)
                    {
                        var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                        var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                        _winHeight = imageLibraryFrame2.Height + imageLibraryFrame3.Height;
                    }

                    if (_minWidth > 0 && _minWidth > _winWidth)
                    {
                        _winWidth = _minWidth;
                    }

                    if (_minHeight > 0 && _minHeight > _winHeight)
                    {
                        _winHeight = _minHeight;
                    }

                    if (_maxWidth > 0 && _maxWidth < _winWidth)
                    {
                        _winWidth = _maxWidth;
                    }

                    if (_maxHeight > 0 && _maxHeight < _winHeight)
                    {
                        _winHeight = _maxHeight;
                    }

                    if (_windowManager._winWidth > 0 && _windowManager._winHeight > 0)
                    {
                        if (_useManagerBorders)
                        {
                            if (_windowManager._winWidth - _windowManager._borderRight < _winLeft + _winWidth)
                            {
                                _winLeft = _windowManager._winWidth - _windowManager._borderRight - _winWidth;
                            }

                            if (_windowManager._borderLeft > _winLeft)
                            {
                                _winLeft = _windowManager._borderLeft;
                                if (_windowManager._winWidth - _windowManager._borderRight < _winLeft + _winWidth)
                                {
                                    _winWidth = _windowManager._winWidth - _windowManager._borderRight - _winLeft;
                                }
                            }

                            if (_windowManager._winHeight - _windowManager._borderBottom < _winTop + _winHeight)
                            {
                                _winTop = _windowManager._winHeight - _windowManager._borderBottom - _winHeight;
                            }

                            if (_windowManager._borderTop > _winTop)
                            {
                                _winTop = _windowManager._borderTop;
                                if (_windowManager._winHeight - _windowManager._borderBottom < _winTop + _winHeight)
                                {
                                    _winHeight = _windowManager._winHeight - _windowManager._borderBottom - _winTop;
                                }
                            }
                        }
                        else
                        {
                            if (_windowManager._winWidth < _winLeft + _winWidth)
                            {
                                _winLeft = _windowManager._winWidth - _winWidth;
                            }

                            if (_winLeft < 0)
                            {
                                _winLeft = 0;
                                if (_windowManager._winWidth < _winLeft + _winWidth)
                                {
                                    _winWidth = _windowManager._winWidth - _winLeft;
                                }
                            }

                            if (_windowManager._winHeight < _winTop + _winHeight)
                            {
                                _winTop = _windowManager._winHeight - _winHeight;
                            }

                            if (_winTop < 0)
                            {
                                _winTop = 0;
                                if (_windowManager._winHeight < _winTop + _winHeight)
                                {
                                    _winHeight = _windowManager._winHeight - _winTop;
                                }
                            }
                        }
                    }

                    if (_masked)
                    {
                        if (WindowStyle == Style.Hollow)
                        {
                            FrameLeft = _winWidth;
                            FrameRight = 0;
                            FrameTop = _winHeight;
                            FrameBottom = 0;
                        }
                        else
                        {
                            FrameLeft = _imageLibraryFrame[_imageLibraryIndexFrame + 3].Width;
                            var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                            FrameRight = _winWidth - imageLibraryFrame2.Width;
                            FrameTop = _imageLibraryFrame[_imageLibraryIndexFrame + 1].Height;
                            var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                            FrameBottom = _winHeight - imageLibraryFrame3.Height;
                        }
                    }
                }

                break;
            case Style.Borderless:
            case Style.Blank:
                if (_imageLibraryTexture != null || WindowStyle == Style.Blank)
                {
                    if (_minWidth > 0 && _minWidth > _winWidth)
                    {
                        _winWidth = _minWidth;
                    }

                    if (_minHeight > 0 && _minHeight > _winHeight)
                    {
                        _winHeight = _minHeight;
                    }

                    if (_maxWidth > 0 && _maxWidth < _winWidth)
                    {
                        _winWidth = _maxWidth;
                    }

                    if (_maxHeight > 0 && _maxHeight < _winHeight)
                    {
                        _winHeight = _maxHeight;
                    }

                    if (_windowManager._winWidth > 0 && _windowManager._winHeight > 0)
                    {
                        if (_useManagerBorders)
                        {
                            if (_windowManager._winWidth - _windowManager._borderRight < _winLeft + WinWidth)
                            {
                                _winLeft = _windowManager._winWidth - _windowManager._borderRight - _winWidth;
                            }

                            if (_windowManager.BorderLeft > _winLeft)
                            {
                                _winLeft = _windowManager._borderLeft;
                                if (_windowManager._winWidth - _windowManager._borderRight < _winLeft + _winWidth)
                                {
                                    _winWidth = _windowManager._winWidth - _windowManager._borderRight - _winLeft;
                                }
                            }

                            if (_windowManager._winHeight - _windowManager._borderBottom < _winTop + _winHeight)
                            {
                                _winTop = _windowManager._winHeight - _windowManager._borderBottom - _winHeight;
                            }

                            if (_windowManager.BorderTop > _winTop)
                            {
                                _winTop = _windowManager._borderTop;
                                if (_windowManager._winHeight - _windowManager._borderBottom < _winTop + _winHeight)
                                {
                                    _winHeight = _windowManager._winHeight - _windowManager._borderBottom - _winTop;
                                }
                            }
                        }
                        else
                        {
                            if (_windowManager._winWidth < _winLeft + WinWidth)
                            {
                                _winLeft = _windowManager._winWidth - _winWidth;
                            }

                            if (_winLeft < 0)
                            {
                                _winLeft = 0;
                                if (_windowManager._winWidth < _winLeft + _winWidth)
                                {
                                    _winWidth = _windowManager._winWidth - _winLeft;
                                }
                            }

                            if (_windowManager._winHeight < _winTop + _winHeight)
                            {
                                _winTop = _windowManager._winHeight - _winHeight;
                            }

                            if (_winTop < 0)
                            {
                                _winTop = 0;
                                if (_windowManager._winHeight < _winTop + _winHeight)
                                {
                                    _winHeight = _windowManager._winHeight - _winTop;
                                }
                            }
                        }
                    }

                    if (_masked)
                    {
                        FrameLeft = _winWidth;
                        FrameRight = 0;
                        FrameTop = _winHeight;
                        FrameBottom = 0;
                    }
                }

                break;
        }

        if (_doubleBuffered && BackSurface != null)
        {
            BackSurface.Resize(_winWidth, _winHeight);
            if (_masked)
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
            if (_alignment.AlignWidth == AlignWidth.Center)
            {
                _winLeft = (_ownerComponent._winWidth - _windowManager._borderLeft - _windowManager._borderRight) / 2 + _windowManager._borderLeft - _winWidth / 2;
            }

            if (_alignment.AlignHeight == AlignHeight.Center)
            {
                _winTop = (_ownerComponent._winHeight - _windowManager._borderTop - _windowManager._borderBottom) / 2 + _windowManager._borderTop - _winHeight / 2;
            }
        }
    }

    // AoWWindow.pas:222
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (Valid && _visible)
            {
                switch (WindowStyle)
                {
                    case Style.Normal:
                        var rectangle = new Rect(_winLeft, _winTop, _winWidth, _winHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle, ref lockedSurface);
                        var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                        var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var rectangle2 = new Rect(_winLeft + imageLibraryFrame4.Width, _winTop + imageLibraryFrame3.Height, _winWidth - imageLibraryFrameWidth.Width - imageLibraryFrame2.Width, _winHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                        _imageLibraryTexture.DrawTiled(_imageLibraryIndexTexture, rectangle2, ref lockedSurface);
                        break;
                    case Style.Hollow:
                        var rectangle3 = new Rect(_winLeft, _winTop, _winWidth, _winHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle3, ref lockedSurface);
                        break;
                    case Style.Borderless:
                        var rectangle4 = new Rect(_winLeft, _winTop, _winWidth, _winHeight);
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
        if (_windowManager != null && !_windowManager.Handled && _visible == true)
        {
            if (X > _winLeft - 1 && X < _winLeft + _winWidth && Y > _winTop - 1 && Y < _winTop + _winHeight)
            {
                SetPriority(_priority);
            }

            for (int I = Children.Count - 1; I >= 0; I--)
            {
                C = (UIComponent)Children[I];
                C.Handled = false;
                if ((!Handled || C.Redraw) && C._visible == true && C._captureInput)
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

            if (!Handled && X > _winLeft - 1 && X < _winLeft + _winWidth && Y > _winTop - 1 && Y < _winTop + _winHeight)
            {
                DragX = 0;
                DragY = 0;
                if (button == MouseButton.Left && _draggable == true && Y < _winTop + _borderHeight + _dragHeight - 1)
                {
                    Dragging = 1;
                    DragX = X - _winLeft;
                    DragY = Y - _winTop;
                }

                if (button == MouseButton.Left && _resizable == true)
                {
                    ResizingX = 0;
                    ResizingY = 0;
                    if (X < _winLeft + _borderWidth - 1)
                    {
                        Dragging = 2;
                        ResizingX = 1;
                        DragX = X - _winLeft;
                        if (Y < _borderHeight * 5 + _winTop - 1)
                        {
                            ResizingY = 1;
                            DragY = Y - _winTop;
                        }

                        if (Y > _winTop + _winHeight - _borderHeight * 5)
                        {
                            ResizingY = 2;
                            DragY = _winTop + _winHeight - Y;
                        }
                    }

                    if (X > _winLeft + _winWidth - _borderHeight)
                    {
                        Dragging = 2;
                        ResizingX = 2;
                        DragX = _winLeft + _winWidth - X;
                        if (Y < _borderHeight * 5 + _winTop - 1)
                        {
                            ResizingY = 1;
                            DragY = Y - _winTop;
                        }

                        if (Y > _winTop + _winHeight - _borderHeight * 5)
                        {
                            ResizingY = 2;
                            DragY = _winTop + _winHeight - Y;
                        }
                    }

                    if (Y < _winTop + _borderHeight - 1)
                    {
                        Dragging = 2;
                        ResizingY = 1;
                        DragY = Y - _winTop;
                        if (X < _borderWidth * 5 + _winLeft - 1)
                        {
                            ResizingX = 1;
                            DragX = X - _winLeft;
                        }

                        if (X > _winLeft + _winWidth - _borderWidth * 5)
                        {
                            ResizingX = 2;
                            DragX = _winLeft + _winWidth - X;
                        }
                    }

                    if (Y > _winTop + _winHeight - _borderHeight)
                    {
                        Dragging = 2;
                        ResizingY = 2;
                        DragY = _winTop + _winHeight - Y;
                        if (X < _borderWidth * 5 + _winLeft - 1)
                        {
                            ResizingX = 1;
                            DragX = X - _winLeft;
                        }

                        if (X > _winLeft + _winWidth - _borderWidth * 5)
                        {
                            ResizingX = 2;
                            DragX = _winLeft + _winWidth - X;
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
        if (_visible && Valid)
        {
            Dragging = 0;
            for (int I = Children.Count - 1; I >= 0; I--)
            {
                C = (UIComponent)Children[I];
                C.Handled = false;
                if ((!Handled || C.Redraw == true) && C._visible == true && C._captureInput)
                {
                    C.CheckMouseUp(button, shift, X, Y);
                }

                if (C.Handled == true)
                {
                    Handled = true;
                }
            }

            if (X > _winLeft - 1 && X < _winLeft + _winWidth && Y > _winTop - 1 && Y < _winTop + _winHeight)
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
        if (_visible == true)
        {
            switch (Dragging)
            {
                case 1:
                    _winLeft = X - DragX;
                    _winTop = Y - DragY;
                    if (_windowManager != null && _useManagerBorders)
                    {
                        if (_windowManager._borderLeft > _winLeft)
                        {
                            _winLeft = _windowManager._borderLeft;
                        }

                        if (_windowManager._winWidth - _windowManager._borderRight < _winLeft + _winWidth)
                        {
                            _winLeft = _windowManager._winWidth - _windowManager._borderRight - _winWidth;
                        }

                        if (_windowManager._borderTop > _winTop)
                        {
                            _winTop = _windowManager._borderTop;
                        }

                        if (_windowManager._winHeight - _windowManager._borderBottom < _winTop + _winHeight)
                        {
                            _winTop = _windowManager._winHeight - _windowManager._borderBottom - _winHeight;
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
                            if (_winLeft + DragX > X)
                            {
                                if (_windowManager != null && _useManagerBorders)
                                {
                                    if (_windowManager._borderLeft > X - DragX)
                                    {
                                        X = _windowManager._borderLeft + DragX;
                                    }
                                }
                            }

                            I = _winLeft + _winWidth;
                            _winWidth = _winWidth - X + _winLeft + DragX;
                            if (_winWidth < _minWidth)
                            {
                                _winWidth = _minWidth;
                            }

                            if (_maxWidth < _winWidth && _maxWidth > 0)
                            {
                                _winWidth = _maxWidth;
                            }

                            _winLeft = I - _winWidth;
                            resized = true;
                            break;
                        case 2:
                            _winWidth = X + DragX - _winLeft;
                            if (_windowManager != null && _useManagerBorders)
                            {
                                if (_windowManager._winWidth - _windowManager._borderRight < _winLeft + _winWidth)
                                {
                                    _winWidth = _windowManager._winWidth - _windowManager._borderRight - _winLeft;
                                }
                            }

                            if (_minWidth > _winWidth)
                            {
                                _winWidth = _minWidth;
                            }

                            if (_maxWidth < _winWidth && _maxWidth > 0)
                            {
                                _winWidth = _maxWidth;
                            }

                            resized = true;
                            break;
                    }

                    switch (ResizingY)
                    {
                        case 1:
                            if (_winTop + DragY > Y)
                            {
                                if (_windowManager != null && _useManagerBorders)
                                {
                                    if (_windowManager._borderTop > Y - DragY)
                                    {
                                        Y = _windowManager._borderTop + DragY;
                                    }
                                }
                            }

                            I = _winTop + _winHeight;
                            _winHeight = _winHeight - Y + _winTop + DragY;
                            if (_winHeight < _minHeight)
                            {
                                _winHeight = _minHeight;
                            }

                            if (_maxHeight < _winHeight && _maxHeight > 0)
                            {
                                _winHeight = _maxHeight;
                            }

                            _winTop = I - _winHeight;
                            resized = true;
                            break;
                        case 2:
                            _winHeight = Y + DragY - _winTop;
                            if (_windowManager != null && _useManagerBorders)
                            {
                                if (_windowManager._winHeight - _windowManager._borderBottom < _winTop + _winHeight)
                                {
                                    _winHeight = _windowManager._winHeight - _windowManager._borderBottom - _winTop;
                                }
                            }

                            if (_minHeight > _winHeight)
                            {
                                _winHeight = _minHeight;
                            }

                            if (_maxHeight < _winHeight && _maxHeight > 0)
                            {
                                _winHeight = _maxHeight;
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
                if ((!Handled || C.Redraw == true || C.ToolTipCount > 0) && C._visible == true && C._captureInput)
                {
                    C.CheckMouseMove(shift, X, Y);
                }

                if (C.Handled == true)
                {
                    Handled = true;
                }
            }

            if (!Handled && X > _winLeft - 1 && X < _winLeft + _winWidth && Y > _winTop - 1 && Y < _winTop + _winHeight)
            {
                Handled = true;
                if (_resizable == true)
                {
                    if (X < _winLeft + _borderWidth - 1)
                    {
                        _cursor = -9;
                        if (Y < _borderHeight * 5 + _winTop - 1)
                        {
                            _cursor = -8;
                        }

                        if (Y > _winTop + _winHeight - _borderHeight * 5)
                        {
                            _cursor = -6;
                        }
                    }
                    else
                    {
                        if (X > _winLeft + _winWidth - _borderWidth)
                        {
                            _cursor = -9;
                            if (Y < _borderHeight * 5 + _winTop - 1)
                            {
                                _cursor = -6;
                            }

                            if (Y > _winTop + _winHeight - _borderHeight * 5)
                            {
                                _cursor = -8;
                            }
                        }
                        else
                        {
                            if (Y < _winTop + _borderHeight - 1)
                            {
                                _cursor = -7;
                                if (X < _borderWidth * 5 + _winLeft - 1)
                                {
                                    _cursor = -8;
                                }

                                if (X > _winLeft + _winWidth - _borderWidth * 5)
                                {
                                    _cursor = -6;
                                }
                            }
                            else
                            {
                                if (Y > _winTop + _winHeight - _borderHeight)
                                {
                                    _cursor = -7;
                                    if (X < _borderWidth * 5 + _winLeft - 1)
                                    {
                                        _cursor = -6;
                                    }

                                    if (X > _winLeft + _winWidth - _borderWidth * 5)
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
