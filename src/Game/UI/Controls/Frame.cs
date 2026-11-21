#nullable disable warnings
// Translated from AoWFrame.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Controls;

// Fields: _frameType, _imageLibraryFrame, _imageLibraryTexture, _imageLibraryLine, _imageLibraryIndexFrame, _imageLibraryIndexLine,
// _imageLibraryIndexTexture, _popupMenu
public sealed class Frame(Component? owner) : Control(owner)
{
    private InterfaceImageLibrary? _imageLibraryFrame;
    private InterfaceImageLibrary? _imageLibraryTexture;
    private InterfaceImageLibrary? _imageLibraryLine;
    private int _imageLibraryIndexFrame;
    private int _imageLibraryIndexLine;
    private int _imageLibraryIndexTexture;

    // AoWFrame.pas:63
    public override void Validate()
    {
        if (_imageLibraryFrame is not null)
        {
            _imageLibraryFrame = _imageLibraryFrame.CheckImageLibrary(_imageLibraryIndexFrame, count: 8, sender: this);
        }

        if (_imageLibraryTexture is not null)
        {
            _imageLibraryTexture = _imageLibraryTexture.CheckImageLibrary(_imageLibraryIndexTexture, count: 1, sender: this);
        }

        if (_imageLibraryLine is not null)
        {
            _imageLibraryLine = _imageLibraryLine.CheckImageLibrary(_imageLibraryIndexLine, count: 3, sender: this);
        }

        if (_imageLibraryFrame is not null || _imageLibraryLine is not null)
        {
            Valid = true;
            if (_imageLibraryFrame is not null)
            {
                FrameType = _imageLibraryTexture is not null ? FrameType.FilledBox : FrameType.HollowBox;
            }
            else
            {
                if (_imageLibraryLine is not null)
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
                LibraryImage? imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame];
                LibraryImage? imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                if (imageLibraryFrameHeight.Height + imageLibraryFrame.Height > WinHeight)
                {
                    LibraryImage? imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                    LibraryImage? imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                    SetWinHeightWithoutInvalidate(imageLibraryFrame2.Height + imageLibraryFrame3.Height);
                }

                LibraryImage? imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame];
                LibraryImage? imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                if (imageLibraryFrameWidth.Width + imageLibraryFrame4.Width > WinWidth)
                {
                    LibraryImage? imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                    LibraryImage? imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                    SetWinWidthWithoutInvalidate(imageLibraryFrame2.Width + imageLibraryFrame3.Width);
                }

                break;
            case FrameType.HorizontalLine:
                SetWinHeightWithoutInvalidate(_imageLibraryLine[_imageLibraryIndexLine].Height);
                LibraryImage? imageLibraryLineWidth = _imageLibraryLine[_imageLibraryIndexLine];
                LibraryImage? imageLibraryLine = _imageLibraryLine[_imageLibraryIndexLine + 2];
                if (imageLibraryLineWidth.Width + imageLibraryLine.Width > WinWidth)
                {
                    LibraryImage? imageLibraryLine2 = _imageLibraryLine[_imageLibraryIndexLine];
                    LibraryImage? imageLibraryLine3 = _imageLibraryLine[_imageLibraryIndexLine + 2];
                    SetWinWidthWithoutInvalidate(imageLibraryLine2.Width + imageLibraryLine3.Width);
                }

                break;
            case FrameType.VertLine:
                LibraryImage? imageLibraryLineHeight = _imageLibraryLine[_imageLibraryIndexLine];
                LibraryImage? imageLibraryLine4 = _imageLibraryLine[_imageLibraryIndexLine + 2];
                if (imageLibraryLineHeight.Height + imageLibraryLine4.Height > WinHeight)
                {
                    LibraryImage? imageLibraryLine2 = _imageLibraryLine[_imageLibraryIndexLine];
                    LibraryImage? imageLibraryLine3 = _imageLibraryLine[_imageLibraryIndexLine + 2];
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
                RaisePreDraw(this, surfDesc: lockedSurface);

                switch (FrameType)
                {
                    case FrameType.VertLine:
                        SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, WinWidth, WinHeight);
                        _imageLibraryLine.DrawVerticalStrip(_imageLibraryIndexLine, area: rectangle, ref lockedSurface);
                        break;
                    case FrameType.HorizontalLine:
                        SizedRectangle rectangle2 = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, WinWidth, WinHeight);
                        _imageLibraryLine.DrawHorizontalStrip(_imageLibraryIndexLine, area: rectangle2, ref lockedSurface);
                        break;
                    case FrameType.HollowBox:
                        SizedRectangle rectangle3 = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, WinWidth, WinHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, area: rectangle3, ref lockedSurface);
                        break;
                    case FrameType.FilledBox:
                        SizedRectangle rectangle4 = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, WinWidth, WinHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, area: rectangle4, ref lockedSurface);
                        LibraryImage? imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        LibraryImage? imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        LibraryImage? imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        LibraryImage? imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                        LibraryImage? imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        LibraryImage? imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        SizedRectangle rectangle5 = new(Window.AbsoluteLeft + WinLeft + imageLibraryFrame4.Width, Window.AbsoluteTop + WinTop + imageLibraryFrame3.Height, WinWidth - imageLibraryFrameWidth.Width - imageLibraryFrame2.Width, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                        _imageLibraryTexture.DrawTiled(_imageLibraryIndexTexture, area: rectangle5, ref lockedSurface);
                        break;
                    default:
                        break;
                }

                RaiseDraw(this, surfDesc: lockedSurface);
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during Draw", caption: $"Error - {Name}", flags: 0);
        }
    }

    // AoWFrame.pas:202
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = true;
            RaiseMouseDown(this, button, shift, X, Y);

            if (button == MouseButton.Right && PopupMenu is not null)
            {
                PopupMenu.SetPopupOwner(this);
                PopupMenu.WinLeft = X;
                PopupMenu.WinTop = Y;
                if (PopupMenu.WindowManager is not null)
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
    public override void CheckMouseUp(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            RaiseMouseUp(this, button, shift, X, Y);
        }
    }

    // AoWFrame.pas:246
    public override void CheckMouseMove(ShiftStates shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            RaiseMouseMove(this, shift, X, Y);
        }
    }

    // AoWFrame.pas:255
    public override void CheckKeyPress(ref byte key) => Handled = false;

    // AoWFrame.pas:139
    public void SetImageLibraryFrame(InterfaceImageLibrary? value)
    {
        if (value != _imageLibraryFrame)
        {
            Valid = false;
            _imageLibraryFrame = value;
        }
    }

    // AoWFrame.pas:148
    public void SetImageLibraryTexture(InterfaceImageLibrary? value)
    {
        if (value != _imageLibraryTexture)
        {
            Valid = false;
            _imageLibraryTexture = value;
        }
    }

    // AoWFrame.pas:157
    public void SetImageLibraryLine(InterfaceImageLibrary? value)
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
    public InterfaceImageLibrary? ImageLibraryFrame { get => _imageLibraryFrame; set => SetImageLibraryFrame(value); }

    [Published]
    public int ImageLibraryIndexFrame { get => _imageLibraryIndexFrame; set => SetILFIndex(value); }

    [Published]
    public int ImageLibraryIndexLine { get => _imageLibraryIndexLine; set => SetImageLibraryLineIndex(value); }

    [Published]
    public int ImageLibraryIndexTexture { get => _imageLibraryIndexTexture; set => SetImageLibraryTileIndex(value); }

    [Published]
    public InterfaceImageLibrary? ImageLibraryLine { get => _imageLibraryLine; set => SetImageLibraryLine(value); }

    [Published]
    public InterfaceImageLibrary? ImageLibraryTexture { get => _imageLibraryTexture; set => SetImageLibraryTexture(value); }

    [Published]
    public Menu PopupMenu { get; set; }

    public new event NotifyEvent<Frame> Change
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, Frame>(this, slot: nameof(Change), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, Frame>(this, slot: nameof(Change), value: value);
    }

    public new event MouseEvent<Frame> MouseDown
    {
        add => ((UIComponent)this).MouseDown += EventSlot.Add<UIComponent, Frame>(this, slot: nameof(MouseDown), value: value);
        remove => ((UIComponent)this).MouseDown -= EventSlot.Remove<UIComponent, Frame>(this, slot: nameof(MouseDown), value: value);
    }

    public new event MouseEvent<Frame> MouseUp
    {
        add => ((UIComponent)this).MouseUp += EventSlot.Add<UIComponent, Frame>(this, slot: nameof(MouseUp), value: value);
        remove => ((UIComponent)this).MouseUp -= EventSlot.Remove<UIComponent, Frame>(this, slot: nameof(MouseUp), value: value);
    }

    public new event MouseMoveEvent<Frame> MouseMove
    {
        add => ((UIComponent)this).MouseMove += EventSlot.Add<UIComponent, Frame>(this, slot: nameof(MouseMove), value: value);
        remove => ((UIComponent)this).MouseMove -= EventSlot.Remove<UIComponent, Frame>(this, slot: nameof(MouseMove), value: value);
    }

    public new event KeyPressEvent<Frame> KeyPress
    {
        add => ((UIComponent)this).KeyPress += EventSlot.Add<UIComponent, Frame>(this, slot: nameof(KeyPress), value: value);
        remove => ((UIComponent)this).KeyPress -= EventSlot.Remove<UIComponent, Frame>(this, slot: nameof(KeyPress), value: value);
    }

    public new event KeyEvent<Frame> KeyDown
    {
        add => ((UIComponent)this).KeyDown += EventSlot.Add<UIComponent, Frame>(this, slot: nameof(KeyDown), value: value);
        remove => ((UIComponent)this).KeyDown -= EventSlot.Remove<UIComponent, Frame>(this, slot: nameof(KeyDown), value: value);
    }

    public new event AoWOnDrawEvent<Frame> PreDraw
    {
        add => ((UIComponent)this).PreDraw += EventSlot.Add<UIComponent, Frame>(this, slot: nameof(PreDraw), value: value);
        remove => ((UIComponent)this).PreDraw -= EventSlot.Remove<UIComponent, Frame>(this, slot: nameof(PreDraw), value: value);
    }

    public new event AoWOnDrawEvent<Frame> Drawn
    {
        add => ((UIComponent)this).Drawn += EventSlot.Add<UIComponent, Frame>(this, slot: nameof(Drawn), value: value);
        remove => ((UIComponent)this).Drawn -= EventSlot.Remove<UIComponent, Frame>(this, slot: nameof(Drawn), value: value);
    }

    public new event NotifyEvent<Frame> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, Frame>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, Frame>(this, slot: nameof(Updated), value: value);
    }
}
