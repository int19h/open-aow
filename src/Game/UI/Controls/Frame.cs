#nullable disable
// Translated from AoWFrame.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// Fields: _frameType, _imageLibraryFrame, _imageLibraryTexture, _imageLibraryLine, _imageLibraryIndexFrame, _imageLibraryIndexLine,
// _imageLibraryIndexTexture, _popupMenu
public partial class Frame(Component owner) : Control(owner)
{
    private InterfaceImageLibrary _imageLibraryFrame;
    private InterfaceImageLibrary _imageLibraryTexture;
    private InterfaceImageLibrary _imageLibraryLine;
    private int _imageLibraryIndexFrame;
    private int _imageLibraryIndexLine;
    private int _imageLibraryIndexTexture;

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
                FrameType = _imageLibraryTexture != null ? FrameType.FilledBox : FrameType.HollowBox;
            }
            else
            {
                if (_imageLibraryLine != null)
                {
                    if (FrameType != FrameType.HorizontalLine)
                    {
                        FrameType = FrameType.VertLine;
                    }
                }
            }
        }

        base.Validate();
    }

    // AoWFrame.pas:113
    public override void SetSize()
    {
        switch (FrameType)
        {
            case FrameType.FilledBox:
            case FrameType.HollowBox:
                LibraryImage imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame];
                LibraryImage imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                if (imageLibraryFrameHeight.Height + imageLibraryFrame.Height > WinHeight)
                {
                    LibraryImage imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                    LibraryImage imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                    SetWinHeightWithoutInvalidate(imageLibraryFrame2.Height + imageLibraryFrame3.Height);
                }

                LibraryImage imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame];
                LibraryImage imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                if (imageLibraryFrameWidth.Width + imageLibraryFrame4.Width > WinWidth)
                {
                    LibraryImage imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                    LibraryImage imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                    SetWinWidthWithoutInvalidate(imageLibraryFrame2.Width + imageLibraryFrame3.Width);
                }

                break;
            case FrameType.HorizontalLine:
                SetWinHeightWithoutInvalidate(_imageLibraryLine[_imageLibraryIndexLine].Height);
                LibraryImage imageLibraryLineWidth = _imageLibraryLine[_imageLibraryIndexLine];
                LibraryImage imageLibraryLine = _imageLibraryLine[_imageLibraryIndexLine + 2];
                if (imageLibraryLineWidth.Width + imageLibraryLine.Width > WinWidth)
                {
                    LibraryImage imageLibraryLine2 = _imageLibraryLine[_imageLibraryIndexLine];
                    LibraryImage imageLibraryLine3 = _imageLibraryLine[_imageLibraryIndexLine + 2];
                    SetWinWidthWithoutInvalidate(imageLibraryLine2.Width + imageLibraryLine3.Width);
                }

                break;
            case FrameType.VertLine:
                LibraryImage imageLibraryLineHeight = _imageLibraryLine[_imageLibraryIndexLine];
                LibraryImage imageLibraryLine4 = _imageLibraryLine[_imageLibraryIndexLine + 2];
                if (imageLibraryLineHeight.Height + imageLibraryLine4.Height > WinHeight)
                {
                    LibraryImage imageLibraryLine2 = _imageLibraryLine[_imageLibraryIndexLine];
                    LibraryImage imageLibraryLine3 = _imageLibraryLine[_imageLibraryIndexLine + 2];
                    SetWinHeightWithoutInvalidate(imageLibraryLine2.Height + imageLibraryLine3.Height);
                }

                SetWinWidthWithoutInvalidate(_imageLibraryLine[_imageLibraryIndexLine].Width);
                break;
            default:
                break;
        }

        base.SetSize();
    }

    // AoWFrame.pas:166
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (Valid && Visible)
            {
                RaisePreDraw(this, lockedSurface);

                switch (FrameType)
                {
                    case FrameType.VertLine:
                        SizedRectangle rectangle = new(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                        _imageLibraryLine.DrawVerticalStrip(_imageLibraryIndexLine, rectangle, ref lockedSurface);
                        break;
                    case FrameType.HorizontalLine:
                        SizedRectangle rectangle2 = new(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                        _imageLibraryLine.DrawHorizontalStrip(_imageLibraryIndexLine, rectangle2, ref lockedSurface);
                        break;
                    case FrameType.HollowBox:
                        SizedRectangle rectangle3 = new(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle3, ref lockedSurface);
                        break;
                    case FrameType.FilledBox:
                        SizedRectangle rectangle4 = new(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle4, ref lockedSurface);
                        LibraryImage imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        LibraryImage imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        LibraryImage imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        LibraryImage imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                        LibraryImage imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        LibraryImage imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        SizedRectangle rectangle5 = new(Window.WinLeft + WinLeft + imageLibraryFrame4.Width, Window.WinTop + WinTop + imageLibraryFrame3.Height, WinWidth - imageLibraryFrameWidth.Width - imageLibraryFrame2.Width, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                        _imageLibraryTexture.DrawTiled(_imageLibraryIndexTexture, rectangle5, ref lockedSurface);
                        break;
                    default:
                        break;
                }

                RaiseDraw(this, lockedSurface);
            }
        }
        catch
        {
            _ = MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AoWFrame.pas:202
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = true;
            RaiseMouseDown(this, button, shift, X, Y);

            if (button == MouseButton.Right && PopupMenu != null)
            {
                PopupMenu.SetPopupOwner(this);
                PopupMenu.WinLeft = X;
                PopupMenu.WinTop = Y;
                if (PopupMenu.WindowManager != null)
                {
                    if (PopupMenu.Hidden)
                    {
                        if (PopupMenu.MenuLeft + PopupMenu.WinWidth > PopupMenu.WindowManager.WinWidth)
                        {
                            PopupMenu.WinLeft = X - PopupMenu.WinWidth;
                        }

                        if (PopupMenu.MenuTop + PopupMenu.WinHeight > PopupMenu.WindowManager.WinHeight)
                        {
                            PopupMenu.WinTop = Y - PopupMenu.WinHeight;
                        }
                    }
                    else
                    {
                        if (PopupMenu.WinLeft + PopupMenu.WinWidth > PopupMenu.WindowManager.WinWidth)
                        {
                            PopupMenu.WinLeft = X - PopupMenu.WinWidth;
                        }

                        if (PopupMenu.WinTop + PopupMenu.WinHeight > PopupMenu.WindowManager.WinHeight)
                        {
                            PopupMenu.WinTop = Y - PopupMenu.WinHeight;
                        }
                    }
                }

                PopupMenu.Visible = true;
            }
        }
    }

    // AoWFrame.pas:237
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            RaiseMouseUp(this, button, shift, X, Y);
        }
    }

    // AoWFrame.pas:246
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            RaiseMouseMove(this, shift, X, Y);
        }
    }

    // AoWFrame.pas:255
    public override void CheckKeyPress(ref byte key) => Handled = false;

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
    public FrameType FrameType { get; set; }

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
    public Menu PopupMenu { get; set; }

    public new event NotifyEvent<Frame> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, Frame>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, Frame>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<Frame> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, Frame>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, Frame>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<Frame> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, Frame>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, Frame>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<Frame> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, Frame>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, Frame>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<Frame> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, Frame>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, Frame>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<Frame> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, Frame>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, Frame>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<Frame> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, Frame>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, Frame>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<Frame> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, Frame>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, Frame>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<Frame> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, Frame>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, Frame>(this, slot: nameof(OnUpdate), value: value);
    }
}
