// Translated from AOWGauge.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// Fields: _min, _max, _position, _imageLibraryFrame, _imageLibraryTextureNormal, _imageLibraryTextureSelected, Style,
// _imageLibraryIndexFrame, _imageLibraryIndexTNormal, _imageLibraryIndexTSelected
public partial class Gauge : Control
{
    private int _min;
    private int _max;
    private int _position;
    private InterfaceImageLibrary _imageLibraryFrame;
    private InterfaceImageLibrary _imageLibraryTextureNormal;
    private InterfaceImageLibrary _imageLibraryTextureSelected;
    private GaugeType _style;
    private int _imageLibraryIndexFrame;
    private int _imageLibraryIndexTNormal;
    private int _imageLibraryIndexTSelected;

    // AOWGauge.pas:69
    public Gauge(Component owner) : base(owner)
    {
        _max = 1;
    }

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
                _imageLibraryFrame = _imageLibraryFrame.CheckImageLibrary(_imageLibraryIndexFrame, 8, this);
            }

            if (_imageLibraryFrame != null)
            {
                _style = GaugeType.Normal;
            }
            else
            {
                _style = GaugeType.Borderless;
            }

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
                var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame];
                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                if (imageLibraryFrameHeight.Height + imageLibraryFrame.Height > WinHeight)
                {
                    var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                    var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                    SetWinHeightWithoutInvalidate(imageLibraryFrame2.Height + imageLibraryFrame3.Height);
                }

                var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame];
                var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                if (imageLibraryFrameWidth.Width + imageLibraryFrame4.Width > WinWidth)
                {
                    var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                    var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                    SetWinWidthWithoutInvalidate(imageLibraryFrame2.Width + imageLibraryFrame3.Width);
                }

                break;
        }

        if (_min + 1 > _max)
        {
            _max = _min + 1;
        }

        if (_min > _position)
        {
            _position = _min;
        }

        if (_max < _position)
        {
            _position = _max;
        }

        base.SetSize();
    }

    // AOWGauge.pas:183
    // { Draw: the filled width P is computed from Position without subtracting Min (original bug:
    //   the gauge is only right for Min = 0) }
    public override void Draw(LockedSurface lockedSurface)
    {
        Rect oldClip = default;
        int P = default;
        try
        {
            if (Valid && Visible)
            {
                if (_onPreDraw != null)
                {
                    _onPreDraw(this, lockedSurface);
                }

                oldClip = lockedSurface.ClipRectangle;
                switch (_style)
                {
                    case GaugeType.Normal:
                        var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                        P = (WinWidth - imageLibraryFrameWidth.Width - imageLibraryFrame.Width) * _position / (_max - _min);
                        var rectangle = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle, ref lockedSurface);
                        var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        if (Window.WinLeft + P + WinLeft + imageLibraryFrame2.Width > lockedSurface.ClipRectangle.Left)
                        {
                            var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                            lockedSurface.ClipRectangle.Left = Window.WinLeft + P + WinLeft + imageLibraryFrame3.Width;
                        }

                        var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        var imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var imageLibraryFrame6 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                        var imageLibraryFrame7 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame8 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var rectangle2 = new Rect(Window.WinLeft + WinLeft + imageLibraryFrame8.Width, Window.WinTop + WinTop + imageLibraryFrame7.Height, WinWidth - imageLibraryFrame5.Width - imageLibraryFrame6.Width, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame4.Height);
                        _imageLibraryTextureNormal.DrawTiled(_imageLibraryIndexTNormal, rectangle2, ref lockedSurface);
                        lockedSurface.ClipRectangle = oldClip;
                        var imageLibraryFrame9 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var libraryImage = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        var filFrameItem = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrameWidth2 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var rectangle3 = new Rect(Window.WinLeft + WinLeft + imageLibraryFrameWidth2.Width, Window.WinTop + WinTop + filFrameItem.Height, P, WinHeight - imageLibraryFrame9.Height - libraryImage.Height);
                        _imageLibraryTextureSelected.DrawTiled(_imageLibraryIndexTSelected, rectangle3, ref lockedSurface);
                        break;
                    case GaugeType.Borderless:
                        P = _position * WinWidth / (_max - _min);
                        if (Window.WinLeft + P + WinLeft > lockedSurface.ClipRectangle.Left)
                        {
                            lockedSurface.ClipRectangle.Left = Window.WinLeft + P + WinLeft;
                        }

                        var rectangle4 = new Rect(Window.WinLeft + P + WinLeft, Window.WinTop + WinTop, WinWidth - P, WinHeight);
                        _imageLibraryTextureNormal.DrawTiled(_imageLibraryIndexTNormal, rectangle4, ref lockedSurface);
                        lockedSurface.ClipRectangle = oldClip;
                        var rectangle5 = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop, P, WinHeight);
                        _imageLibraryTextureSelected.DrawTiled(_imageLibraryIndexTSelected, rectangle5, ref lockedSurface);
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

    // AOWGauge.pas:237
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = true;
            if (_onMouseDown != null)
            {
                _onMouseDown(this, button, shift, X, Y);
            }
        }
    }

    // AOWGauge.pas:250
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = true;
            if (_onMouseUp != null)
            {
                _onMouseUp(this, button, shift, X, Y);
            }
        }
    }

    // AOWGauge.pas:262
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = true;
            if (_onMouseMove != null)
            {
                _onMouseMove(this, shift, X, Y);
            }
        }
    }

    // AOWGauge.pas:274
    public override void CheckKeyPress(ref byte key)
    {
        Handled = false;
    }

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
        if (value != _imageLibraryIndexFrame)
        {
            _imageLibraryIndexFrame = value;
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
            if (_position > _max)
            {
                _position = _max;
            }

            if (_min > _position)
            {
                _position = _min;
            }

            Drawn = false;
        }
    }

    [Published]
    public int Min { get => _min; set => _min = value; }

    [Published]
    public int Max { get => _max; set => _max = value; }

    [Published]
    public int Position { get => _position; set => SetPosition(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryFrame { get => _imageLibraryFrame; set => SetImageLibraryFrame(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryTextureNormal { get => _imageLibraryTextureNormal; set => SetImageLibraryTileNorm(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryTextureSelected { get => _imageLibraryTextureSelected; set => SetImageLibraryTileSelected(value); }

    [Published]
    public int ImageLibraryIndexFrame { get => _imageLibraryIndexFrame; set => _imageLibraryIndexFrame = value; }

    [Published]
    public int ImageLibraryIndexTNormal { get => _imageLibraryIndexTNormal; set => SetImageLibraryTnIndex(value); }

    [Published]
    public int ImageLibraryIndexTSelected { get => _imageLibraryIndexTSelected; set => SetImageLibraryTsIndex(value); }

    public new NotifyEvent<Gauge> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<Gauge> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<Gauge> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<Gauge> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<Gauge> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<Gauge> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<Gauge> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<Gauge> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<Gauge> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }

    public enum GaugeType : byte
    {
        Normal = 0, Borderless = 1
    }
}
