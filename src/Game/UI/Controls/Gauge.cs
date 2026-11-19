#nullable disable
// Translated from AOWGauge.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Controls;

// Fields: _min, _max, _position, _imageLibraryFrame, _imageLibraryTextureNormal, _imageLibraryTextureSelected, Style,
// _imageLibraryIndexFrame, _imageLibraryIndexTNormal, _imageLibraryIndexTSelected
public partial class Gauge(Component owner) : Control(owner)
{

    private int _position;
    private InterfaceImageLibrary _imageLibraryFrame;
    private InterfaceImageLibrary _imageLibraryTextureNormal;
    private InterfaceImageLibrary _imageLibraryTextureSelected;
    private GaugeType _style;
    private int _imageLibraryIndexTNormal;
    private int _imageLibraryIndexTSelected;

    // AOWGauge.pas:75
    public override void Validate()
    {
        if (_imageLibraryTextureNormal != null)
        {
            _imageLibraryTextureNormal = _imageLibraryTextureNormal.CheckImageLibrary(_imageLibraryIndexTNormal, 1, this);
        }

        if (_imageLibraryTextureSelected != null)
        {
            _imageLibraryTextureSelected = _imageLibraryTextureSelected.CheckImageLibrary(_imageLibraryIndexTSelected, 1, this);
        }

        if (_imageLibraryTextureNormal != null && _imageLibraryTextureSelected != null)
        {
            if (_imageLibraryFrame != null)
            {
                _imageLibraryFrame = _imageLibraryFrame.CheckImageLibrary(ImageLibraryIndexFrame, 8, this);
            }

            _style = _imageLibraryFrame != null ? GaugeType.Normal : GaugeType.Borderless;

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
                LibraryImage imageLibraryFrameHeight = _imageLibraryFrame[ImageLibraryIndexFrame];
                LibraryImage imageLibraryFrame = _imageLibraryFrame[ImageLibraryIndexFrame + 5];
                if (imageLibraryFrameHeight.Height + imageLibraryFrame.Height > WinHeight)
                {
                    LibraryImage imageLibraryFrame2 = _imageLibraryFrame[ImageLibraryIndexFrame];
                    LibraryImage imageLibraryFrame3 = _imageLibraryFrame[ImageLibraryIndexFrame + 5];
                    SetWinHeightWithoutInvalidate(imageLibraryFrame2.Height + imageLibraryFrame3.Height);
                }

                LibraryImage imageLibraryFrameWidth = _imageLibraryFrame[ImageLibraryIndexFrame];
                LibraryImage imageLibraryFrame4 = _imageLibraryFrame[ImageLibraryIndexFrame + 2];
                if (imageLibraryFrameWidth.Width + imageLibraryFrame4.Width > WinWidth)
                {
                    LibraryImage imageLibraryFrame2 = _imageLibraryFrame[ImageLibraryIndexFrame];
                    LibraryImage imageLibraryFrame3 = _imageLibraryFrame[ImageLibraryIndexFrame + 2];
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
                RaisePreDraw(this, lockedSurface);

                Rect oldClip = lockedSurface.ClipRectangle;
                int p;
                switch (_style)
                {
                    case GaugeType.Normal:
                        LibraryImage imageLibraryFrameWidth = _imageLibraryFrame[ImageLibraryIndexFrame + 3];
                        LibraryImage imageLibraryFrame = _imageLibraryFrame[ImageLibraryIndexFrame + 4];
                        p = (WinWidth - imageLibraryFrameWidth.Width - imageLibraryFrame.Width) * _position / (Max - Min);
                        SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, WinWidth, WinHeight);
                        _imageLibraryFrame.DrawFrame(ImageLibraryIndexFrame, rectangle, ref lockedSurface);
                        LibraryImage imageLibraryFrame2 = _imageLibraryFrame[ImageLibraryIndexFrame + 3];
                        if (Window.AbsoluteLeft + p + WinLeft + imageLibraryFrame2.Width > lockedSurface.ClipRectangle.Left)
                        {
                            LibraryImage imageLibraryFrame3 = _imageLibraryFrame[ImageLibraryIndexFrame + 3];
                            Rect clipRectangle = lockedSurface.ClipRectangle;
                            clipRectangle.Left = Window.AbsoluteLeft + p + WinLeft + imageLibraryFrame3.Width;
                            lockedSurface.ClipRectangle = clipRectangle;
                        }

                        LibraryImage imageLibraryFrameHeight = _imageLibraryFrame[ImageLibraryIndexFrame + 1];
                        LibraryImage imageLibraryFrame4 = _imageLibraryFrame[ImageLibraryIndexFrame + 6];
                        LibraryImage imageLibraryFrame5 = _imageLibraryFrame[ImageLibraryIndexFrame + 3];
                        LibraryImage imageLibraryFrame6 = _imageLibraryFrame[ImageLibraryIndexFrame + 4];
                        LibraryImage imageLibraryFrame7 = _imageLibraryFrame[ImageLibraryIndexFrame + 1];
                        LibraryImage imageLibraryFrame8 = _imageLibraryFrame[ImageLibraryIndexFrame + 3];
                        SizedRectangle rectangle2 = new(Window.AbsoluteLeft + WinLeft + imageLibraryFrame8.Width, Window.AbsoluteTop + WinTop + imageLibraryFrame7.Height, WinWidth - imageLibraryFrame5.Width - imageLibraryFrame6.Width, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame4.Height);
                        _imageLibraryTextureNormal.DrawTiled(_imageLibraryIndexTNormal, rectangle2, ref lockedSurface);
                        lockedSurface.ClipRectangle = oldClip;
                        LibraryImage imageLibraryFrame9 = _imageLibraryFrame[ImageLibraryIndexFrame + 1];
                        LibraryImage libraryImage = _imageLibraryFrame[ImageLibraryIndexFrame + 6];
                        LibraryImage filFrameItem = _imageLibraryFrame[ImageLibraryIndexFrame + 1];
                        LibraryImage imageLibraryFrameWidth2 = _imageLibraryFrame[ImageLibraryIndexFrame + 3];
                        SizedRectangle rectangle3 = new(Window.AbsoluteLeft + WinLeft + imageLibraryFrameWidth2.Width, Window.AbsoluteTop + WinTop + filFrameItem.Height, p, WinHeight - imageLibraryFrame9.Height - libraryImage.Height);
                        _imageLibraryTextureSelected.DrawTiled(_imageLibraryIndexTSelected, rectangle3, ref lockedSurface);
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
                        _imageLibraryTextureNormal.DrawTiled(_imageLibraryIndexTNormal, rectangle4, ref lockedSurface);
                        lockedSurface.ClipRectangle = oldClip;
                        SizedRectangle rectangle5 = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, p, WinHeight);
                        _imageLibraryTextureSelected.DrawTiled(_imageLibraryIndexTSelected, rectangle5, ref lockedSurface);
                        break;
                    default:
                        break;
                }

                RaiseDraw(this, lockedSurface);
            }
        }
        catch
        {
            _ = MessageBox(0, "Exception during Draw", $"Error - {Name}", 0);
        }
    }

    // AOWGauge.pas:237
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
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
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = true;
            RaiseMouseUp(this, button, shift, X, Y);
        }
    }

    // AOWGauge.pas:262
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
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
    public virtual void SetImageLibraryFrame(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryFrame)
        {
            Valid = false;
            _imageLibraryFrame = value;
        }
    }

    // AOWGauge.pas:150
    public virtual void SetImageLibraryTileNorm(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryTextureNormal)
        {
            Valid = false;
            _imageLibraryTextureNormal = value;
        }
    }

    // AOWGauge.pas:159
    public virtual void SetImageLibraryTileSelected(InterfaceImageLibrary value)
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
    public InterfaceImageLibrary ImageLibraryFrame { get => _imageLibraryFrame; set => SetImageLibraryFrame(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryTextureNormal { get => _imageLibraryTextureNormal; set => SetImageLibraryTileNorm(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryTextureSelected { get => _imageLibraryTextureSelected; set => SetImageLibraryTileSelected(value); }

    [Published]
    public int ImageLibraryIndexFrame { get; set; }

    [Published]
    public int ImageLibraryIndexTNormal { get => _imageLibraryIndexTNormal; set => SetImageLibraryTnIndex(value); }

    [Published]
    public int ImageLibraryIndexTSelected { get => _imageLibraryIndexTSelected; set => SetImageLibraryTsIndex(value); }

    public new event NotifyEvent<Gauge> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, Gauge>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, Gauge>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<Gauge> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, Gauge>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, Gauge>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<Gauge> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, Gauge>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, Gauge>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<Gauge> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, Gauge>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, Gauge>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<Gauge> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, Gauge>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, Gauge>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<Gauge> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, Gauge>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, Gauge>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<Gauge> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, Gauge>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, Gauge>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<Gauge> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, Gauge>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, Gauge>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<Gauge> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, Gauge>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, Gauge>(this, slot: nameof(OnUpdate), value: value);
    }

    public enum GaugeType : byte
    {
        Normal = 0, Borderless = 1
    }
}
