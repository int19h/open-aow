#nullable disable warnings
// Translated from AOWGauge.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Controls;

// Fields: _min, _max, _position, _imageLibraryFrame, _imageLibraryTextureNormal, _imageLibraryTextureSelected, Style,
// _imageLibraryIndexFrame, _imageLibraryIndexTNormal, _imageLibraryIndexTSelected
public sealed partial class Gauge(Component owner) : Control(owner)
{

    private int _position;
    private InterfaceImageLibrary? _imageLibraryFrame;
    private InterfaceImageLibrary? _imageLibraryTextureNormal;
    private InterfaceImageLibrary? _imageLibraryTextureSelected;
    private GaugeType _style;
    private int _imageLibraryIndexTNormal;
    private int _imageLibraryIndexTSelected;

    // AOWGauge.pas:75
    public override void Validate()
    {
        if (_imageLibraryTextureNormal is not null)
        {
            _imageLibraryTextureNormal = _imageLibraryTextureNormal.CheckImageLibrary(_imageLibraryIndexTNormal, count: 1, sender: this);
        }

        if (_imageLibraryTextureSelected is not null)
        {
            _imageLibraryTextureSelected = _imageLibraryTextureSelected.CheckImageLibrary(_imageLibraryIndexTSelected, count: 1, sender: this);
        }

        if (_imageLibraryTextureNormal is not null && _imageLibraryTextureSelected is not null)
        {
            if (_imageLibraryFrame is not null)
            {
                _imageLibraryFrame = _imageLibraryFrame.CheckImageLibrary(ImageLibraryIndexFrame, count: 8, sender: this);
            }

            _style = _imageLibraryFrame is not null ? GaugeType.Normal : GaugeType.Borderless;

            Valid = true;
        }

        base.Validate();
    }

    // AOWGauge.pas:94
    public override void SetSize()
    {
        switch (_style)
        {
            case GaugeType.Normal:
                LibraryImage? imageLibraryFrameHeight = _imageLibraryFrame[ImageLibraryIndexFrame];
                LibraryImage? imageLibraryFrame = _imageLibraryFrame[ImageLibraryIndexFrame + 5];
                if (imageLibraryFrameHeight.Height + imageLibraryFrame.Height > WinHeight)
                {
                    LibraryImage? imageLibraryFrame2 = _imageLibraryFrame[ImageLibraryIndexFrame];
                    LibraryImage? imageLibraryFrame3 = _imageLibraryFrame[ImageLibraryIndexFrame + 5];
                    SetWinHeightWithoutInvalidate(imageLibraryFrame2.Height + imageLibraryFrame3.Height);
                }

                LibraryImage? imageLibraryFrameWidth = _imageLibraryFrame[ImageLibraryIndexFrame];
                LibraryImage? imageLibraryFrame4 = _imageLibraryFrame[ImageLibraryIndexFrame + 2];
                if (imageLibraryFrameWidth.Width + imageLibraryFrame4.Width > WinWidth)
                {
                    LibraryImage? imageLibraryFrame2 = _imageLibraryFrame[ImageLibraryIndexFrame];
                    LibraryImage? imageLibraryFrame3 = _imageLibraryFrame[ImageLibraryIndexFrame + 2];
                    SetWinWidthWithoutInvalidate(imageLibraryFrame2.Width + imageLibraryFrame3.Width);
                }

                break;
            case GaugeType.Borderless:
            default:
                break;
        }

        if (Min + 1 > Max)
        {
            Max = Min + 1;
        }

        if (Min > _position)
        {
            _position = Min;
        }

        if (Max < _position)
        {
            _position = Max;
        }

        base.SetSize();
    }

    // AOWGauge.pas:183
    // { Draw: the filled width P is computed from Position without subtracting Min (original bug:
    //   the gauge is only right for Min = 0) }
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (Valid && Visible)
            {
                RaisePreDraw(this, surfDesc: lockedSurface);

                Rect oldClip = lockedSurface.ClipRectangle;
                int p;
                switch (_style)
                {
                    case GaugeType.Normal:
                        LibraryImage? imageLibraryFrameWidth = _imageLibraryFrame[ImageLibraryIndexFrame + 3];
                        LibraryImage? imageLibraryFrame = _imageLibraryFrame[ImageLibraryIndexFrame + 4];
                        p = (WinWidth - imageLibraryFrameWidth.Width - imageLibraryFrame.Width) * _position / (Max - Min);
                        SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, WinWidth, WinHeight);
                        _imageLibraryFrame.DrawFrame(ImageLibraryIndexFrame, area: rectangle, ref lockedSurface);
                        LibraryImage? imageLibraryFrame2 = _imageLibraryFrame[ImageLibraryIndexFrame + 3];
                        if (Window.AbsoluteLeft + p + WinLeft + imageLibraryFrame2.Width > lockedSurface.ClipRectangle.Left)
                        {
                            LibraryImage? imageLibraryFrame3 = _imageLibraryFrame[ImageLibraryIndexFrame + 3];
                            Rect clipRectangle = lockedSurface.ClipRectangle;
                            clipRectangle.Left = Window.AbsoluteLeft + p + WinLeft + imageLibraryFrame3.Width;
                            lockedSurface.ClipRectangle = clipRectangle;
                        }

                        LibraryImage? imageLibraryFrameHeight = _imageLibraryFrame[ImageLibraryIndexFrame + 1];
                        LibraryImage? imageLibraryFrame4 = _imageLibraryFrame[ImageLibraryIndexFrame + 6];
                        LibraryImage? imageLibraryFrame5 = _imageLibraryFrame[ImageLibraryIndexFrame + 3];
                        LibraryImage? imageLibraryFrame6 = _imageLibraryFrame[ImageLibraryIndexFrame + 4];
                        LibraryImage? imageLibraryFrame7 = _imageLibraryFrame[ImageLibraryIndexFrame + 1];
                        LibraryImage? imageLibraryFrame8 = _imageLibraryFrame[ImageLibraryIndexFrame + 3];
                        SizedRectangle rectangle2 = new(Window.AbsoluteLeft + WinLeft + imageLibraryFrame8.Width, Window.AbsoluteTop + WinTop + imageLibraryFrame7.Height, WinWidth - imageLibraryFrame5.Width - imageLibraryFrame6.Width, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame4.Height);
                        _imageLibraryTextureNormal.DrawTiled(_imageLibraryIndexTNormal, area: rectangle2, ref lockedSurface);
                        lockedSurface.ClipRectangle = oldClip;
                        LibraryImage? imageLibraryFrame9 = _imageLibraryFrame[ImageLibraryIndexFrame + 1];
                        LibraryImage? libraryImage = _imageLibraryFrame[ImageLibraryIndexFrame + 6];
                        LibraryImage? filFrameItem = _imageLibraryFrame[ImageLibraryIndexFrame + 1];
                        LibraryImage? imageLibraryFrameWidth2 = _imageLibraryFrame[ImageLibraryIndexFrame + 3];
                        SizedRectangle rectangle3 = new(Window.AbsoluteLeft + WinLeft + imageLibraryFrameWidth2.Width, Window.AbsoluteTop + WinTop + filFrameItem.Height, p, WinHeight - imageLibraryFrame9.Height - libraryImage.Height);
                        _imageLibraryTextureSelected.DrawTiled(_imageLibraryIndexTSelected, area: rectangle3, ref lockedSurface);
                        break;
                    case GaugeType.Borderless:
                        p = _position * WinWidth / (Max - Min);
                        if (Window.AbsoluteLeft + p + WinLeft > lockedSurface.ClipRectangle.Left)
                        {
                            Rect clipRectangle2 = lockedSurface.ClipRectangle;
                            clipRectangle2.Left = Window.AbsoluteLeft + p + WinLeft;
                            lockedSurface.ClipRectangle = clipRectangle2;
                        }

                        SizedRectangle rectangle4 = new(Window.AbsoluteLeft + p + WinLeft, Window.AbsoluteTop + WinTop, WinWidth - p, WinHeight);
                        _imageLibraryTextureNormal.DrawTiled(_imageLibraryIndexTNormal, area: rectangle4, ref lockedSurface);
                        lockedSurface.ClipRectangle = oldClip;
                        SizedRectangle rectangle5 = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, p, WinHeight);
                        _imageLibraryTextureSelected.DrawTiled(_imageLibraryIndexTSelected, area: rectangle5, ref lockedSurface);
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

    // AOWGauge.pas:237
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = true;
            RaiseMouseDown(this, button, shift, X, Y);
        }
    }

    // AOWGauge.pas:250
    public override void CheckMouseUp(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = true;
            RaiseMouseUp(this, button, shift, X, Y);
        }
    }

    // AOWGauge.pas:262
    public override void CheckMouseMove(ShiftStates shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = true;
            RaiseMouseMove(this, shift, X, Y);
        }
    }

    // AOWGauge.pas:274
    public override void CheckKeyPress(ref byte key) => Handled = false;

    // AOWGauge.pas:141
    public void SetImageLibraryFrame(InterfaceImageLibrary? value)
    {
        if (value != _imageLibraryFrame)
        {
            Valid = false;
            _imageLibraryFrame = value;
        }
    }

    // AOWGauge.pas:150
    public void SetImageLibraryTileNorm(InterfaceImageLibrary? value)
    {
        if (value != _imageLibraryTextureNormal)
        {
            Valid = false;
            _imageLibraryTextureNormal = value;
        }
    }

    // AOWGauge.pas:159
    public void SetImageLibraryTileSelected(InterfaceImageLibrary? value)
    {
        if (value != _imageLibraryTextureSelected)
        {
            Valid = false;
            _imageLibraryTextureSelected = value;
        }
    }

    // AOWGauge.pas:114
    public void SetILFIndex(int value)
    {
        if (value != ImageLibraryIndexFrame)
        {
            ImageLibraryIndexFrame = value;
            Valid = false;
        }
    }

    // AOWGauge.pas:123
    public void SetImageLibraryTnIndex(int value)
    {
        if (value != _imageLibraryIndexTNormal)
        {
            _imageLibraryIndexTNormal = value;
            Valid = false;
        }
    }

    // AOWGauge.pas:132
    public void SetImageLibraryTsIndex(int value)
    {
        if (value != _imageLibraryIndexTSelected)
        {
            _imageLibraryIndexTSelected = value;
            Valid = false;
        }
    }

    // AOWGauge.pas:168
    public void SetPosition(int value)
    {
        if (value != _position)
        {
            _position = value;
            if (_position > Max)
            {
                _position = Max;
            }

            if (Min > _position)
            {
                _position = Min;
            }

            Drawn = false;
        }
    }

    [Published]
    public int Min { get; set; }

    [Published]
    public int Max { get; set; } = 1;

    [Published]
    public int Position { get => _position; set => SetPosition(value); }

    [Published]
    public InterfaceImageLibrary? ImageLibraryFrame { get => _imageLibraryFrame; set => SetImageLibraryFrame(value); }

    [Published]
    public InterfaceImageLibrary? ImageLibraryTextureNormal { get => _imageLibraryTextureNormal; set => SetImageLibraryTileNorm(value); }

    [Published]
    public InterfaceImageLibrary? ImageLibraryTextureSelected { get => _imageLibraryTextureSelected; set => SetImageLibraryTileSelected(value); }

    [Published]
    public int ImageLibraryIndexFrame { get; set; }

    [Published]
    public int ImageLibraryIndexTNormal { get => _imageLibraryIndexTNormal; set => SetImageLibraryTnIndex(value); }

    [Published]
    public int ImageLibraryIndexTSelected { get => _imageLibraryIndexTSelected; set => SetImageLibraryTsIndex(value); }

    public new event NotifyEvent<Gauge> Change
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, Gauge>(this, slot: nameof(Change), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, Gauge>(this, slot: nameof(Change), value: value);
    }

    public new event MouseEvent<Gauge> MouseDown
    {
        add => ((UIComponent)this).MouseDown += EventSlot.Add<UIComponent, Gauge>(this, slot: nameof(MouseDown), value: value);
        remove => ((UIComponent)this).MouseDown -= EventSlot.Remove<UIComponent, Gauge>(this, slot: nameof(MouseDown), value: value);
    }

    public new event MouseEvent<Gauge> MouseUp
    {
        add => ((UIComponent)this).MouseUp += EventSlot.Add<UIComponent, Gauge>(this, slot: nameof(MouseUp), value: value);
        remove => ((UIComponent)this).MouseUp -= EventSlot.Remove<UIComponent, Gauge>(this, slot: nameof(MouseUp), value: value);
    }

    public new event MouseMoveEvent<Gauge> MouseMove
    {
        add => ((UIComponent)this).MouseMove += EventSlot.Add<UIComponent, Gauge>(this, slot: nameof(MouseMove), value: value);
        remove => ((UIComponent)this).MouseMove -= EventSlot.Remove<UIComponent, Gauge>(this, slot: nameof(MouseMove), value: value);
    }

    public new event KeyPressEvent<Gauge> KeyPress
    {
        add => ((UIComponent)this).KeyPress += EventSlot.Add<UIComponent, Gauge>(this, slot: nameof(KeyPress), value: value);
        remove => ((UIComponent)this).KeyPress -= EventSlot.Remove<UIComponent, Gauge>(this, slot: nameof(KeyPress), value: value);
    }

    public new event KeyEvent<Gauge> KeyDown
    {
        add => ((UIComponent)this).KeyDown += EventSlot.Add<UIComponent, Gauge>(this, slot: nameof(KeyDown), value: value);
        remove => ((UIComponent)this).KeyDown -= EventSlot.Remove<UIComponent, Gauge>(this, slot: nameof(KeyDown), value: value);
    }

    public new event AoWOnDrawEvent<Gauge> PreDraw
    {
        add => ((UIComponent)this).PreDraw += EventSlot.Add<UIComponent, Gauge>(this, slot: nameof(PreDraw), value: value);
        remove => ((UIComponent)this).PreDraw -= EventSlot.Remove<UIComponent, Gauge>(this, slot: nameof(PreDraw), value: value);
    }

    public new event AoWOnDrawEvent<Gauge> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, Gauge>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, Gauge>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<Gauge> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, Gauge>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, Gauge>(this, slot: nameof(Updated), value: value);
    }

    public enum GaugeType : byte
    {
        Normal = 0, Borderless = 1
    }
}
