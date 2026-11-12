// Translated from AoWFrame.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// Fields: _frameType, _imageLibraryFrame, _imageLibraryTexture, _imageLibraryLine, _imageLibraryIndexFrame, _imageLibraryIndexLine,
// _imageLibraryIndexTexture, _popupMenu
public partial class Frame : Control
{
    public Frame(Component owner) : base(owner)
    { }

    public FrameType _frameType;
    public InterfaceImageLibrary _imageLibraryFrame;
    public InterfaceImageLibrary _imageLibraryTexture;
    public InterfaceImageLibrary _imageLibraryLine;
    public int _imageLibraryIndexFrame;
    public int _imageLibraryIndexLine;
    public int _imageLibraryIndexTexture;
    public Menu _popupMenu;

    // AoWFrame.pas:63
    public override void Validate()
    {
        if (_imageLibraryFrame != null)
        {
            _imageLibraryFrame = _imageLibraryFrame.CheckImageLibrary(_imageLibraryIndexFrame, 8, this);
        }

        if (_imageLibraryTexture != null)
        {
            _imageLibraryTexture = _imageLibraryTexture.CheckImageLibrary(_imageLibraryIndexTexture, 1, this);
        }

        if (_imageLibraryLine != null)
        {
            _imageLibraryLine = _imageLibraryLine.CheckImageLibrary(_imageLibraryIndexLine, 3, this);
        }

        if (_imageLibraryFrame != null || _imageLibraryLine != null)
        {
            Valid = true;
            if (_imageLibraryFrame != null)
            {
                if (_imageLibraryTexture != null)
                {
                    _frameType = FrameType.FilledBox;
                }
                else
                {
                    _frameType = FrameType.HollowBox;
                }
            }
            else
            {
                if (_imageLibraryLine != null)
                {
                    if (_frameType != FrameType.HorizontalLine)
                    {
                        _frameType = FrameType.VertLine;
                    }
                }
            }
        }

        base.Validate();
    }

    // AoWFrame.pas:113
    public override void SetSize()
    {
        switch (_frameType)
        {
            case FrameType.FilledBox:
            case FrameType.HollowBox:
                var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame];
                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                if (imageLibraryFrameHeight.Height + imageLibraryFrame.Height > _winHeight)
                {
                    var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                    var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                    _winHeight = imageLibraryFrame2.Height + imageLibraryFrame3.Height;
                }

                var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame];
                var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                if (imageLibraryFrameWidth.Width + imageLibraryFrame4.Width > _winWidth)
                {
                    var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                    var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                    _winWidth = imageLibraryFrame2.Width + imageLibraryFrame3.Width;
                }

                break;
            case FrameType.HorizontalLine:
                _winHeight = _imageLibraryLine[_imageLibraryIndexLine].Height;
                var imageLibraryLineWidth = _imageLibraryLine[_imageLibraryIndexLine];
                var imageLibraryLine = _imageLibraryLine[_imageLibraryIndexLine + 2];
                if (imageLibraryLineWidth.Width + imageLibraryLine.Width > _winWidth)
                {
                    var imageLibraryLine2 = _imageLibraryLine[_imageLibraryIndexLine];
                    var imageLibraryLine3 = _imageLibraryLine[_imageLibraryIndexLine + 2];
                    _winWidth = imageLibraryLine2.Width + imageLibraryLine3.Width;
                }

                break;
            case FrameType.VertLine:
                var imageLibraryLineHeight = _imageLibraryLine[_imageLibraryIndexLine];
                var imageLibraryLine4 = _imageLibraryLine[_imageLibraryIndexLine + 2];
                if (imageLibraryLineHeight.Height + imageLibraryLine4.Height > _winHeight)
                {
                    var imageLibraryLine2 = _imageLibraryLine[_imageLibraryIndexLine];
                    var imageLibraryLine3 = _imageLibraryLine[_imageLibraryIndexLine + 2];
                    _winHeight = imageLibraryLine2.Height + imageLibraryLine3.Height;
                }

                _winWidth = _imageLibraryLine[_imageLibraryIndexLine].Width;
                break;
        }

        base.SetSize();
    }

    // AoWFrame.pas:166
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (Valid && _visible)
            {
                if (_onPreDraw != null)
                {
                    _onPreDraw(this, lockedSurface);
                }

                switch (_frameType)
                {
                    case FrameType.VertLine:
                        var rectangle = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop, _winWidth, _winHeight);
                        _imageLibraryLine.DrawVerticalStrip(_imageLibraryIndexLine, rectangle, ref lockedSurface);
                        break;
                    case FrameType.HorizontalLine:
                        var rectangle2 = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop, _winWidth, _winHeight);
                        _imageLibraryLine.DrawHorizontalStrip(_imageLibraryIndexLine, rectangle2, ref lockedSurface);
                        break;
                    case FrameType.HollowBox:
                        var rectangle3 = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop, _winWidth, _winHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle3, ref lockedSurface);
                        break;
                    case FrameType.FilledBox:
                        var rectangle4 = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop, _winWidth, _winHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle4, ref lockedSurface);
                        var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                        var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var rectangle5 = new Rect(_window._winLeft + _winLeft + imageLibraryFrame4.Width, _window._winTop + _winTop + imageLibraryFrame3.Height, _winWidth - imageLibraryFrameWidth.Width - imageLibraryFrame2.Width, _winHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                        _imageLibraryTexture.DrawTiled(_imageLibraryIndexTexture, rectangle5, ref lockedSurface);
                        break;
                }

                if (_onDraw != null)
                {
                    _onDraw(this, lockedSurface);
                }
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AoWFrame.pas:202
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
            Handled = true;
            if (_onMouseDown != null)
            {
                _onMouseDown(this, button, shift, X, Y);
            }

            if (button == MouseButton.Right && _popupMenu != null)
            {
                _popupMenu.SetPopupOwner(this);
                _popupMenu.WinLeft = X;
                _popupMenu.WinTop = Y;
                if (_popupMenu._windowManager != null)
                {
                    if (_popupMenu.Hidden)
                    {
                        if (_popupMenu.MenuLeft + _popupMenu._winWidth > _popupMenu._windowManager._winWidth)
                        {
                            _popupMenu.WinLeft = X - _popupMenu._winWidth;
                        }

                        if (_popupMenu.MenuTop + _popupMenu._winHeight > _popupMenu._windowManager._winHeight)
                        {
                            _popupMenu.WinTop = Y - _popupMenu._winHeight;
                        }
                    }
                    else
                    {
                        if (_popupMenu._winLeft + _popupMenu._winWidth > _popupMenu._windowManager._winWidth)
                        {
                            _popupMenu.WinLeft = X - _popupMenu._winWidth;
                        }

                        if (_popupMenu._winTop + _popupMenu._winHeight > _popupMenu._windowManager._winHeight)
                        {
                            _popupMenu.WinTop = Y - _popupMenu._winHeight;
                        }
                    }
                }

                _popupMenu.Visible = true;
            }
        }
    }

    // AoWFrame.pas:237
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
            if (_onMouseUp != null)
            {
                _onMouseUp(this, button, shift, X, Y);
            }
        }
    }

    // AoWFrame.pas:246
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
            if (_onMouseMove != null)
            {
                _onMouseMove(this, shift, X, Y);
            }
        }
    }

    // AoWFrame.pas:255
    public override void CheckKeyPress(ref byte key)
    {
        Handled = false;
    }

    // AoWFrame.pas:139
    public virtual void SetImageLibraryFrame(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryFrame)
        {
            Valid = false;
            _imageLibraryFrame = value;
        }
    }

    // AoWFrame.pas:148
    public virtual void SetImageLibraryTexture(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryTexture)
        {
            Valid = false;
            _imageLibraryTexture = value;
        }
    }

    // AoWFrame.pas:157
    public virtual void SetImageLibraryLine(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryLine)
        {
            Valid = false;
            _imageLibraryLine = value;
        }
    }

    // AoWFrame.pas:86
    public void SetILFIndex(int value)
    {
        if (value != _imageLibraryIndexFrame)
        {
            _imageLibraryIndexFrame = value;
            Valid = false;
        }
    }

    // AoWFrame.pas:95
    public void SetImageLibraryLineIndex(int value)
    {
        if (value != _imageLibraryIndexLine)
        {
            _imageLibraryIndexLine = value;
            Valid = false;
        }
    }

    // AoWFrame.pas:104
    public void SetImageLibraryTileIndex(int value)
    {
        if (value != _imageLibraryIndexTexture)
        {
            _imageLibraryIndexTexture = value;
            Valid = false;
        }
    }

    [Published]
    public FrameType FrameType { get => _frameType; set => _frameType = value; }

    [Published]
    public InterfaceImageLibrary ImageLibraryFrame { get => _imageLibraryFrame; set => SetImageLibraryFrame(value); }

    [Published]
    public int ImageLibraryIndexFrame { get => _imageLibraryIndexFrame; set => SetILFIndex(value); }

    [Published]
    public int ImageLibraryIndexLine { get => _imageLibraryIndexLine; set => SetImageLibraryLineIndex(value); }

    [Published]
    public int ImageLibraryIndexTexture { get => _imageLibraryIndexTexture; set => SetImageLibraryTileIndex(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryLine { get => _imageLibraryLine; set => SetImageLibraryLine(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryTexture { get => _imageLibraryTexture; set => SetImageLibraryTexture(value); }

    [Published]
    public Menu PopupMenu { get => _popupMenu; set => _popupMenu = value; }

    public new NotifyEvent<Frame> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<Frame> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<Frame> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<Frame> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<Frame> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<Frame> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<Frame> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<Frame> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<Frame> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }
}
