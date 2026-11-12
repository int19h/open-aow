// Translated from AoWArnoLine.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// top, middle, bottom, copied line image library
public partial class ArnoLine : Control
{
    public LibraryImage _imageTop;
    public LibraryImage _imageMiddle;
    public LibraryImage _imageBottom;
    public InterfaceImageLibrary LineLibrary;

    // AoWArnoLine.pas:47
    public override void Dispose()
    {
        LineLibrary?.Dispose();
        base.Dispose();
    }

    // AoWArnoLine.pas:41
    public ArnoLine(Component owner) : base(owner)
    {
        LineLibrary = new InterfaceImageLibrary(this);
    }

    // AoWArnoLine.pas:53
    public override void Validate()
    {
        if (_imageTop != null && _imageMiddle != null && _imageBottom != null)
        {
            LineLibrary.Clear();
            var copy = _imageTop.Copy();
            LineLibrary.Add(copy);
            var fImageMiddleCopy = _imageMiddle.Copy();
            LineLibrary.Add(fImageMiddleCopy);
            var fImageBottomCopy = _imageBottom.Copy();
            LineLibrary.Add(fImageBottomCopy);
            Valid = true;
        }

        base.Validate();
    }

    // AoWArnoLine.pas:93
    public override void SetSize()
    {
        _winWidth = _imageTop.Width;
        base.SetSize();
    }

    // AoWArnoLine.pas:99
    public override void Draw(LockedSurface lockedSurface)
    {
        Rect oldClip = default;
        try
        {
            if (Valid && _visible)
            {
                if (_onPreDraw != null)
                {
                    _onPreDraw(this, lockedSurface);
                }

                oldClip = lockedSurface.ClipRectangle;
                if (_winTop > lockedSurface.ClipRectangle.Top)
                {
                    lockedSurface.ClipRectangle.Top = _winTop;
                }

                var rectangle = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop, _winWidth, _winHeight);
                LineLibrary.DrawVerticalStrip(0, rectangle, ref lockedSurface);
                if (_onDraw != null)
                {
                    _onDraw(this, lockedSurface);
                }

                lockedSurface.ClipRectangle = oldClip;
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AoWArnoLine.pas:66
    public void SetITop(LibraryImage value)
    {
        if (value != _imageTop)
        {
            _imageTop = value;
            Valid = false;
        }
    }

    // AoWArnoLine.pas:75
    public void SetIMiddle(LibraryImage value)
    {
        if (value != _imageMiddle)
        {
            _imageMiddle = value;
            Valid = false;
        }
    }

    // AoWArnoLine.pas:84
    public void SetIBottom(LibraryImage value)
    {
        if (value != _imageBottom)
        {
            _imageBottom = value;
            Valid = false;
        }
    }

    [Published]
    public LibraryImage ImageTop { get => _imageTop; set => SetITop(value); }

    [Published]
    public LibraryImage ImageMiddle { get => _imageMiddle; set => SetIMiddle(value); }

    [Published]
    public LibraryImage ImageBottom { get => _imageBottom; set => SetIBottom(value); }

    public new NotifyEvent<ArnoLine> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<ArnoLine> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<ArnoLine> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<ArnoLine> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<ArnoLine> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<ArnoLine> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<ArnoLine> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<ArnoLine> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<ArnoLine> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }
}
