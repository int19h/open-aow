// Translated from AoWHScroll.pas.
using Aow.Game.Draw;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// { The AoWHScroll unit of aowInt.dpl: TAOWHScrollBar, the horizontal scroll bar (TAOWScrollBar of
//   AoWVScroll with the decrement button at the left and the increment button at the right). }
// no fields of its own
public partial class HorizontalScrollBar : ScrollBar
{
    public HorizontalScrollBar(Component owner) : base(owner)
    { }

    // AoWHScroll.pas:31
    // { SetSize: as TAOWVScrollBar.SetSize across; the track length is not kept above zero here }
    public override void SetSize()
    {
        PositionCount = _max - _min + 1;
        if (PositionCount < _numberVisible)
        {
            _numberVisible = PositionCount;
        }

        switch (ScrollType)
        {
            case Style.Scroll:
                _winHeight = _imageLibraryTileBack[_imageLibraryIndexBack].Height;
                var imageLibraryImageDecrementWidth = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                var imageLibraryImageIncrementWidth = _imageLibraryImageIncrement[_imageLibraryIndexIncrementUp];
                if (imageLibraryImageDecrementWidth.Width + imageLibraryImageIncrementWidth.Width > _winWidth)
                {
                    var imageLibraryImageDecrement = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                    var imageLibraryImageIncrement = _imageLibraryImageIncrement[_imageLibraryIndexIncrementUp];
                    _winWidth = imageLibraryImageDecrement.Width + imageLibraryImageIncrement.Width;
                }

                var imageLibraryImageDecrement2 = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                var imageLibraryImageIncrement2 = _imageLibraryImageIncrement[_imageLibraryIndexIncrementUp];
                TrackLength = _winWidth - imageLibraryImageDecrement2.Width - imageLibraryImageIncrement2.Width;
                ThumbLength = TrackLength * _numberVisible / PositionCount;
                switch (ThumbStyle)
                {
                    case ScrollBarThumbStyle.Icon:
                        ThumbLength = _thumbImage[_thumbImageIndex].Width;
                        break;
                    case ScrollBarThumbStyle.Bar:
                        var thumbStripWidth = _thumbStrip[_thumbImageIndex];
                        var thumbStrip = _thumbStrip[_thumbImageIndex + 2];
                        if (ThumbLength < thumbStripWidth.Width + thumbStrip.Width)
                        {
                            var imageLibraryLineTab2 = _thumbStrip[_thumbImageIndex];
                            var imageLibraryLineTab3 = _thumbStrip[_thumbImageIndex + 2];
                            ThumbLength = imageLibraryLineTab2.Width + imageLibraryLineTab3.Width;
                        }

                        break;
                }

                break;
            case Style.Slider:
                _winHeight = _imageLibraryLineBack[_imageLibraryIndexBack].Height;
                TrackLength = _winWidth;
                ThumbLength = TrackLength * _numberVisible / PositionCount;
                switch (ThumbStyle)
                {
                    case ScrollBarThumbStyle.Icon:
                        ThumbLength = _thumbImage[_thumbImageIndex].Width;
                        break;
                    case ScrollBarThumbStyle.Bar:
                        var thumbStripWidth = _thumbStrip[_thumbImageIndex];
                        var thumbStrip = _thumbStrip[_thumbImageIndex + 2];
                        if (ThumbLength < thumbStripWidth.Width + thumbStrip.Width)
                        {
                            var imageLibraryLineTab2 = _thumbStrip[_thumbImageIndex];
                            var imageLibraryLineTab3 = _thumbStrip[_thumbImageIndex + 2];
                            ThumbLength = imageLibraryLineTab2.Width + imageLibraryLineTab3.Width;
                        }

                        break;
                }

                break;
            case Style.IncrementDecrement:
                _winHeight = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp].Height;
                var imageLibraryImageDecrement3 = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                var imageLibraryImageIncrement3 = _imageLibraryImageIncrement[_imageLibraryIndexIncrementUp];
                _winWidth = imageLibraryImageDecrement3.Width + imageLibraryImageIncrement3.Width;
                break;
        }

        if (_position + _numberVisible > PositionCount)
        {
            _position = PositionCount - _numberVisible;
        }

        base.SetSize();
    }

    // AoWHScroll.pas:79
    // { Draw: as TAOWVScrollBar.Draw across. The buttons of an increment/decrement pair keep parts of
    //   the vertical code: the increment button is drawn left of the control and below it, and the
    //   decrement button is FWinWidth wide and as high as its image (original) }
    public override void Draw(LockedSurface lockedSurface)
    {
        int thumbPosition = default;
        try
        {
            if (Valid && _visible)
            {
                if (_onPreDraw != null)
                {
                    _onPreDraw(this, lockedSurface);
                }

                switch (ScrollType)
                {
                    case Style.Scroll:
                        if (HeldPart == 4 && HeldUpdateCount > 0)
                        {
                            var imageLibraryImageIncrementWidth = _imageLibraryImageIncrement[_imageLibraryIndexIncrementDown];
                            var imageLibraryImageIncrement = _imageLibraryImageIncrement[_imageLibraryIndexIncrementDown];
                            var rectangle = new Rect(_window._winLeft + _winLeft + _winWidth - imageLibraryImageIncrement.Width, _window._winTop + _winTop, imageLibraryImageIncrementWidth.Width, _winHeight);
                            _imageLibraryImageIncrement.DrawTiled(_imageLibraryIndexIncrementDown, rectangle, ref lockedSurface);
                        }
                        else
                        {
                            var imageLibraryImageIncrement = _imageLibraryImageIncrement[_imageLibraryIndexIncrementUp];
                            var imageLibraryImageIncrementWidth = _imageLibraryImageIncrement[_imageLibraryIndexIncrementDown];
                            var rectangle = new Rect(_window._winLeft + _winLeft + _winWidth - imageLibraryImageIncrementWidth.Width, _window._winTop + _winTop, imageLibraryImageIncrement.Width, _winHeight);
                            _imageLibraryImageIncrement.DrawTiled(_imageLibraryIndexIncrementUp, rectangle, ref lockedSurface);
                        }

                        if (HeldPart == 1 && HeldUpdateCount > 0)
                        {
                            var imageLibraryImageDecrement = _imageLibraryIDecrement[_imageLibraryIndexDecrementDown];
                            var rectangle = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop, imageLibraryImageDecrement.Width, _winHeight);
                            _imageLibraryIDecrement.DrawTiled(_imageLibraryIndexDecrementDown, rectangle, ref lockedSurface);
                        }
                        else
                        {
                            var imageLibraryImageDecrement = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                            var rectangle = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop, imageLibraryImageDecrement.Width, _winHeight);
                            _imageLibraryIDecrement.DrawTiled(_imageLibraryIndexDecrementUp, rectangle, ref lockedSurface);
                        }

                        var imageLibraryTileBack = _imageLibraryTileBack[_imageLibraryIndexBack];
                        var imageLibraryImageDecrementWidth = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                        var rectangle2 = new Rect(_window._winLeft + _winLeft + imageLibraryImageDecrementWidth.Width, _window._winTop + _winTop, TrackLength, imageLibraryTileBack.Height);
                        _imageLibraryTileBack.DrawTiled(_imageLibraryIndexBack, rectangle2, ref lockedSurface);
                        thumbPosition = TrackLength * _position / PositionCount;
                        if (ThumbLength + thumbPosition > TrackLength)
                        {
                            thumbPosition = TrackLength - ThumbLength;
                        }

                        switch (ThumbStyle)
                        {
                            case ScrollBarThumbStyle.Icon:
                                var imageLibraryImageDecrement = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                                var rectangle = new Rect(_window._winLeft + _winLeft + imageLibraryImageDecrement.Width + thumbPosition, _window._winTop + _winTop, ThumbLength, _winHeight);
                                _thumbImage.DrawTiled(_thumbImageIndex, rectangle, ref lockedSurface);
                                break;
                            case ScrollBarThumbStyle.Bar:
                                var imageLibraryImageDecrement2 = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                                var rectangle3 = new Rect(_window._winLeft + _winLeft + imageLibraryImageDecrement2.Width + thumbPosition, _window._winTop + _winTop, ThumbLength, _winHeight);
                                _thumbStrip.DrawHorizontalStrip(_thumbImageIndex, rectangle3, ref lockedSurface);
                                break;
                        }

                        break;
                    case Style.Slider:
                        var rectangle4 = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop, _winWidth, _winHeight);
                        _imageLibraryLineBack.DrawHorizontalStrip(_imageLibraryIndexBack, rectangle4, ref lockedSurface);
                        thumbPosition = TrackLength * _position / PositionCount;
                        if (ThumbLength + thumbPosition > TrackLength)
                        {
                            thumbPosition = TrackLength - ThumbLength;
                        }

                        switch (ThumbStyle)
                        {
                            case ScrollBarThumbStyle.Icon:
                                var rectangle = new Rect(_window._winLeft + _winLeft + thumbPosition, _window._winTop + _winTop, ThumbLength, _winHeight);
                                _thumbImage.DrawTiled(_thumbImageIndex, rectangle, ref lockedSurface);
                                break;
                            case ScrollBarThumbStyle.Bar:
                                var rectangle3 = new Rect(_window._winLeft + _winLeft + thumbPosition, _window._winTop + _winTop, ThumbLength, _winHeight);
                                _thumbStrip.DrawHorizontalStrip(_thumbImageIndex, rectangle3, ref lockedSurface);
                                break;
                        }

                        break;
                    case Style.IncrementDecrement:
                        if (HeldPart == 4 && HeldUpdateCount > 0)
                        {
                            var imageLibraryImageIncrementWidth = _imageLibraryImageIncrement[_imageLibraryIndexIncrementDown];
                            var imageLibraryImageIncrement = _imageLibraryImageIncrement[_imageLibraryIndexIncrementDown];
                            var rectangle = new Rect(_window._winLeft + _winLeft - imageLibraryImageIncrement.Width, _window._winTop + _winTop + _winHeight, imageLibraryImageIncrementWidth.Width, _winHeight);
                            _imageLibraryImageIncrement.DrawTiled(_imageLibraryIndexIncrementDown, rectangle, ref lockedSurface);
                        }
                        else
                        {
                            var imageLibraryImageIncrementWidth = _imageLibraryImageIncrement[_imageLibraryIndexIncrementUp];
                            var imageLibraryImageIncrement = _imageLibraryImageIncrement[_imageLibraryIndexIncrementUp];
                            var rectangle = new Rect(_window._winLeft + _winLeft - imageLibraryImageIncrement.Width, _window._winTop + _winTop + _winHeight, imageLibraryImageIncrementWidth.Width, _winHeight);
                            _imageLibraryImageIncrement.DrawTiled(_imageLibraryIndexIncrementUp, rectangle, ref lockedSurface);
                        }

                        if (HeldPart == 1 && HeldUpdateCount > 0)
                        {
                            var imageLibraryImageDecrement = _imageLibraryIDecrement[_imageLibraryIndexDecrementDown];
                            var rectangle = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop, _winWidth, imageLibraryImageDecrement.Height);
                            _imageLibraryIDecrement.DrawTiled(_imageLibraryIndexDecrementDown, rectangle, ref lockedSurface);
                        }
                        else
                        {
                            var imageLibraryImageDecrement = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                            var rectangle = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop, _winWidth, imageLibraryImageDecrement.Height);
                            _imageLibraryIDecrement.DrawTiled(_imageLibraryIndexDecrementUp, rectangle, ref lockedSurface);
                        }

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

    // AoWHScroll.pas:171
    // { CheckMouseDown: as TAOWVScrollBar.CheckMouseDown across, but OnChange is called once at the end
    //   when the position changed. The track after the tab ends at the width of the decrement button
    //   (original) }
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        int thumbPosition = default;
        int oldPosition;
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (_visible == true && X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
            oldPosition = _position;
            Handled = true;
            if (button == MouseButton.Left)
            {
                HeldUpdateCount = 1;
                HeldPart = 5;
                thumbPosition = TrackLength * _position / PositionCount;
                if (thumbPosition + ThumbLength > TrackLength)
                {
                    thumbPosition = TrackLength - ThumbLength;
                }

                if (ScrollType != Style.Slider)
                {
                    var imageLibraryImageDecrement = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                    if (X < _window._winLeft + _winLeft + imageLibraryImageDecrement.Width - 1)
                    {
                        HeldPart = 1;
                        _position = _position - _smallChange;
                        if (_min > _position)
                        {
                            _position = _min;
                        }
                    }

                    var imageLibraryImageIncrement = _imageLibraryImageIncrement[_imageLibraryIndexIncrementUp];
                    if (X > _window._winLeft + _winLeft + _winWidth - imageLibraryImageIncrement.Width)
                    {
                        HeldPart = 4;
                        _position = _position + _smallChange;
                        if (_max + 1 - _numberVisible < _position)
                        {
                            _position = _max + 1 - _numberVisible;
                        }
                    }
                }

                if (HeldPart == 5)
                {
                    switch (ScrollType)
                    {
                        case Style.Scroll:
                            var imageLibraryImageDecrementWidth = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                            bool condition = X > _window._winLeft + _winLeft + imageLibraryImageDecrementWidth.Width;
                            if (condition)
                            {
                                var imageLibraryImageIncrement = _imageLibraryImageIncrement[_imageLibraryIndexIncrementUp];
                                condition = X < _window._winLeft + _winLeft + thumbPosition + imageLibraryImageIncrement.Width;
                            }

                            if (condition)
                            {
                                var imageLibraryImageDecrement = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                                TrackClickTarget = (X - _window._winLeft - _winLeft - imageLibraryImageDecrement.Width) * PositionCount / TrackLength + _min;
                                HeldPart = 2;
                                _position = _position - _largeChange;
                                if (_min > _position)
                                {
                                    _position = _min;
                                }
                            }

                            var imageLibraryImageDecrement2 = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                            bool test = X > _window._winLeft + _winLeft + imageLibraryImageDecrement2.Width + thumbPosition + ThumbLength;
                            if (test)
                            {
                                var imageLibraryImageDecrement = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                                test = X < _window._winLeft + _winLeft + _winWidth - imageLibraryImageDecrement.Width;
                            }

                            if (test)
                            {
                                var imageLibraryImageDecrement = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                                TrackClickTarget = (X - _window._winLeft - _winLeft - imageLibraryImageDecrement.Width) * PositionCount / TrackLength + _min;
                                HeldPart = 3;
                                _position = _position + _largeChange;
                                if (_max + 1 - _numberVisible < _position)
                                {
                                    _position = _max + 1 - _numberVisible;
                                }
                            }

                            if (HeldPart == 5)
                            {
                                TrackClickTarget = X - thumbPosition;
                            }

                            break;
                        case Style.Slider:
                            if (X > _window._winLeft + _winLeft && X < _window._winLeft + _winLeft + thumbPosition)
                            {
                                TrackClickTarget = (X - _window._winLeft - _winLeft) * PositionCount / TrackLength + _min;
                                HeldPart = 2;
                                _position = _position - _largeChange;
                                if (_min > _position)
                                {
                                    _position = _min;
                                }
                            }

                            if (X > _window._winLeft + _winLeft + thumbPosition + ThumbLength && X < _window._winLeft + _winLeft + _winWidth)
                            {
                                TrackClickTarget = (X - _window._winLeft - _winLeft) * PositionCount / TrackLength + _min;
                                HeldPart = 3;
                                _position = _position + _largeChange;
                                if (_max + 1 - _numberVisible < _position)
                                {
                                    _position = _max + 1 - _numberVisible;
                                }
                            }

                            if (HeldPart == 5)
                            {
                                TrackClickTarget = X - thumbPosition;
                            }

                            break;
                    }
                }
            }

            if (oldPosition != _position)
            {
                Drawn = false;
                if (_onChange != null)
                {
                    _onChange(this);
                }
            }

            if (_onMouseDown != null)
            {
                _onMouseDown(this, button, shift, X, Y);
            }
        }
    }

    // AoWHScroll.pas:273
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (HeldPart == 1 || HeldPart == 4)
        {
            Drawn = false;
        }

        HeldUpdateCount = 0;
        HeldPart = 0;
        if (X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
            Handled = true;
            if (_onMouseUp != null)
            {
                _onMouseUp(this, button, shift, X, Y);
            }
        }
    }

    // AoWHScroll.pas:291
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (HeldPart == 5 && HeldUpdateCount > 0)
        {
            _position = (X - TrackClickTarget) * PositionCount / TrackLength + _min;
            if (_position < _min)
            {
                _position = _min;
            }

            if (_max + 1 - _numberVisible < _position)
            {
                _position = _max + 1 - _numberVisible;
            }

            Drawn = false;
            if (_onChange != null)
            {
                _onChange(this);
            }
        }

        if (X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
            Handled = true;
            if (_onMouseMove != null)
            {
                _onMouseMove(this, shift, X, Y);
            }
        }
    }

    public new NotifyEvent<HorizontalScrollBar> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<HorizontalScrollBar> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<HorizontalScrollBar> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<HorizontalScrollBar> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<HorizontalScrollBar> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<HorizontalScrollBar> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<HorizontalScrollBar> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<HorizontalScrollBar> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<HorizontalScrollBar> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }
}
