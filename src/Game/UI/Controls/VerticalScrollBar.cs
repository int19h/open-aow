// Translated from AoWVScroll.pas.
using Aow.Game.Draw;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// the vertical scroll bar, the decrement button at the top and the increment button at the
// bottom
public partial class VerticalScrollBar : ScrollBar
{
    public VerticalScrollBar(Component owner) : base(owner)
    { }

    // AoWVScroll.pas:408
    // { SetSize takes the width from the images and sizes the track and the tab: the tab of a line is
    //   proportional to the visible part of the range, but no shorter than its two end images }
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
                _winWidth = _imageLibraryTileBack[_imageLibraryIndexBack].Width;
                var imageLibraryImageDecrementHeight = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                var imageLibraryImageIncrementHeight = _imageLibraryImageIncrement[_imageLibraryIndexIncrementUp];
                if (imageLibraryImageDecrementHeight.Height + imageLibraryImageIncrementHeight.Height > _winHeight)
                {
                    var imageLibraryImageDecrement = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                    var imageLibraryImageIncrement = _imageLibraryImageIncrement[_imageLibraryIndexIncrementUp];
                    _winHeight = imageLibraryImageDecrement.Height + imageLibraryImageIncrement.Height;
                }

                var imageLibraryImageDecrement2 = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                var imageLibraryImageIncrement2 = _imageLibraryImageIncrement[_imageLibraryIndexIncrementUp];
                TrackLength = _winHeight - imageLibraryImageDecrement2.Height - imageLibraryImageIncrement2.Height;
                if (TrackLength < 1)
                {
                    TrackLength = 1;
                }

                ThumbLength = TrackLength * _numberVisible / PositionCount;
                switch (ThumbStyle)
                {
                    case ScrollBarThumbStyle.Icon:
                        ThumbLength = _thumbImage[_thumbImageIndex].Height;
                        break;
                    case ScrollBarThumbStyle.Bar:
                        var thumbStripHeight = _thumbStrip[_thumbImageIndex];
                        var thumbStrip = _thumbStrip[_thumbImageIndex + 2];
                        if (ThumbLength < thumbStripHeight.Height + thumbStrip.Height)
                        {
                            var imageLibraryLineTab2 = _thumbStrip[_thumbImageIndex];
                            var imageLibraryLineTab3 = _thumbStrip[_thumbImageIndex + 2];
                            ThumbLength = imageLibraryLineTab2.Height + imageLibraryLineTab3.Height;
                        }

                        break;
                }

                break;
            case Style.Slider:
                _winWidth = _imageLibraryLineBack[_imageLibraryIndexBack].Width;
                TrackLength = _winHeight;
                ThumbLength = TrackLength * _numberVisible / PositionCount;
                switch (ThumbStyle)
                {
                    case ScrollBarThumbStyle.Icon:
                        ThumbLength = _thumbImage[_thumbImageIndex].Height;
                        break;
                    case ScrollBarThumbStyle.Bar:
                        var thumbStripHeight = _thumbStrip[_thumbImageIndex];
                        var thumbStrip = _thumbStrip[_thumbImageIndex + 2];
                        if (ThumbLength < thumbStripHeight.Height + thumbStrip.Height)
                        {
                            var imageLibraryLineTab2 = _thumbStrip[_thumbImageIndex];
                            var imageLibraryLineTab3 = _thumbStrip[_thumbImageIndex + 2];
                            ThumbLength = imageLibraryLineTab2.Height + imageLibraryLineTab3.Height;
                        }

                        break;
                }

                break;
            case Style.IncrementDecrement:
                _winWidth = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp].Width;
                var imageLibraryImageDecrement3 = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                var imageLibraryImageIncrement3 = _imageLibraryImageIncrement[_imageLibraryIndexIncrementUp];
                _winHeight = imageLibraryImageDecrement3.Height + imageLibraryImageIncrement3.Height;
                break;
        }

        if (_position + _numberVisible > PositionCount)
        {
            _position = PositionCount - _numberVisible;
        }

        base.SetSize();
    }

    // AoWVScroll.pas:457
    // { Draw draws the buttons (pressed while held), the background and the tab. The top of the
    //   increment button is computed from the height of its pressed image in both states (original) }
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
                            var imageLibraryImageIncrementHeight = _imageLibraryImageIncrement[_imageLibraryIndexIncrementDown];
                            var imageLibraryImageIncrement = _imageLibraryImageIncrement[_imageLibraryIndexIncrementDown];
                            var rectangle = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop + _winHeight - imageLibraryImageIncrement.Height, _winWidth, imageLibraryImageIncrementHeight.Height);
                            _imageLibraryImageIncrement.DrawTiled(_imageLibraryIndexIncrementDown, rectangle, ref lockedSurface);
                        }
                        else
                        {
                            var imageLibraryImageIncrement = _imageLibraryImageIncrement[_imageLibraryIndexIncrementUp];
                            var imageLibraryImageIncrementHeight = _imageLibraryImageIncrement[_imageLibraryIndexIncrementDown];
                            var rectangle = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop + _winHeight - imageLibraryImageIncrementHeight.Height, _winWidth, imageLibraryImageIncrement.Height);
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

                        var imageLibraryTileBack = _imageLibraryTileBack[_imageLibraryIndexBack];
                        var imageLibraryImageDecrementHeight = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                        var rectangle2 = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop + imageLibraryImageDecrementHeight.Height, imageLibraryTileBack.Width, TrackLength);
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
                                var rectangle = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop + imageLibraryImageDecrement.Height + thumbPosition, _winWidth, ThumbLength);
                                _thumbImage.DrawTiled(_thumbImageIndex, rectangle, ref lockedSurface);
                                break;
                            case ScrollBarThumbStyle.Bar:
                                var imageLibraryImageDecrement2 = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                                var rectangle3 = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop + imageLibraryImageDecrement2.Height + thumbPosition, _winWidth, ThumbLength);
                                _thumbStrip.DrawVerticalStrip(_thumbImageIndex, rectangle3, ref lockedSurface);
                                break;
                        }

                        break;
                    case Style.Slider:
                        var rectangle4 = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop, _winWidth, _winHeight);
                        _imageLibraryLineBack.DrawVerticalStrip(_imageLibraryIndexBack, rectangle4, ref lockedSurface);
                        thumbPosition = TrackLength * _position / PositionCount;
                        if (ThumbLength + thumbPosition > TrackLength)
                        {
                            thumbPosition = TrackLength - ThumbLength;
                        }

                        switch (ThumbStyle)
                        {
                            case ScrollBarThumbStyle.Icon:
                                var rectangle = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop + thumbPosition, _winWidth, ThumbLength);
                                _thumbImage.DrawTiled(_thumbImageIndex, rectangle, ref lockedSurface);
                                break;
                            case ScrollBarThumbStyle.Bar:
                                var rectangle3 = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop + thumbPosition, _winWidth, ThumbLength);
                                _thumbStrip.DrawVerticalStrip(_thumbImageIndex, rectangle3, ref lockedSurface);
                                break;
                        }

                        break;
                    case Style.IncrementDecrement:
                        if (HeldPart == 4 && HeldUpdateCount > 0)
                        {
                            var imageLibraryImageIncrementHeight = _imageLibraryImageIncrement[_imageLibraryIndexIncrementDown];
                            var imageLibraryImageIncrement = _imageLibraryImageIncrement[_imageLibraryIndexIncrementDown];
                            var rectangle = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop + _winHeight - imageLibraryImageIncrement.Height, _winWidth, imageLibraryImageIncrementHeight.Height);
                            _imageLibraryImageIncrement.DrawTiled(_imageLibraryIndexIncrementDown, rectangle, ref lockedSurface);
                        }
                        else
                        {
                            var imageLibraryImageIncrement = _imageLibraryImageIncrement[_imageLibraryIndexIncrementUp];
                            var imageLibraryImageIncrementHeight = _imageLibraryImageIncrement[_imageLibraryIndexIncrementDown];
                            var rectangle = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop + _winHeight - imageLibraryImageIncrementHeight.Height, _winWidth, imageLibraryImageIncrement.Height);
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

    // AoWVScroll.pas:549
    // { CheckMouseDown: a click on a button steps the position, a click in the track pages towards
    //   the click, a click on the tab starts dragging it (F151 stays 5). The track before the tab ends
    //   at the height of the increment button (original) }
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        int thumbPosition = default;
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (_visible == true && X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
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
                    if (Y < _window._winTop + _winTop + imageLibraryImageDecrement.Height - 1)
                    {
                        HeldPart = 1;
                        ApplySmallStep(true);
                    }

                    var imageLibraryImageIncrement = _imageLibraryImageIncrement[_imageLibraryIndexIncrementUp];
                    if (Y > _window._winTop + _winTop + _winHeight - imageLibraryImageIncrement.Height)
                    {
                        HeldPart = 4;
                        ApplySmallStep(false);
                    }
                }

                if (HeldPart == 5)
                {
                    switch (ScrollType)
                    {
                        case Style.Scroll:
                            var imageLibraryImageDecrementHeight = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                            bool condition = Y > _window._winTop + _winTop + imageLibraryImageDecrementHeight.Height;
                            if (condition)
                            {
                                var imageLibraryImageIncrement = _imageLibraryImageIncrement[_imageLibraryIndexIncrementUp];
                                condition = Y < _window._winTop + _winTop + thumbPosition + imageLibraryImageIncrement.Height;
                            }

                            if (condition)
                            {
                                var imageLibraryImageDecrement = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                                TrackClickTarget = (Y - _window._winTop - _winTop - imageLibraryImageDecrement.Height) * PositionCount / TrackLength + _min;
                                HeldPart = 2;
                                _position = _position - _largeChange;
                                if (_min > _position)
                                {
                                    _position = _min;
                                }

                                Drawn = false;
                                if (_onChange != null)
                                {
                                    _onChange(this);
                                }
                            }

                            var imageLibraryImageDecrement2 = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                            bool test = Y > _window._winTop + _winTop + imageLibraryImageDecrement2.Height + thumbPosition + ThumbLength;
                            if (test)
                            {
                                var imageLibraryImageIncrement = _imageLibraryImageIncrement[_imageLibraryIndexIncrementUp];
                                test = Y < _window._winTop + _winTop + _winHeight - imageLibraryImageIncrement.Height;
                            }

                            if (test)
                            {
                                var imageLibraryImageDecrement = _imageLibraryIDecrement[_imageLibraryIndexDecrementUp];
                                TrackClickTarget = (Y - _window._winTop - _winTop - imageLibraryImageDecrement.Height) * PositionCount / TrackLength + _min;
                                HeldPart = 3;
                                _position = _position + _largeChange;
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

                            if (HeldPart == 5)
                            {
                                TrackClickTarget = Y - thumbPosition;
                            }

                            break;
                        case Style.Slider:
                            if (Y > _window._winTop + _winTop && Y < _window._winTop + _winTop + thumbPosition)
                            {
                                TrackClickTarget = (Y - _window._winTop - _winTop) * PositionCount / TrackLength + _min;
                                HeldPart = 2;
                                _position = _position - _largeChange;
                                if (_min > _position)
                                {
                                    _position = _min;
                                }

                                Drawn = false;
                                if (_onChange != null)
                                {
                                    _onChange(this);
                                }
                            }

                            if (Y > _window._winTop + _winTop + thumbPosition + ThumbLength && Y < _window._winTop + _winTop + _winHeight)
                            {
                                TrackClickTarget = (Y - _window._winTop - _winTop) * PositionCount / TrackLength + _min;
                                HeldPart = 3;
                                _position = _position + _largeChange;
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

                            if (HeldPart == 5)
                            {
                                TrackClickTarget = Y - thumbPosition;
                            }

                            break;
                    }
                }
            }

            if (_onMouseDown != null)
            {
                _onMouseDown(this, button, shift, X, Y);
            }
        }
    }

    // AoWVScroll.pas:661
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (HeldPart != 0)
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

    // AoWVScroll.pas:680
    // { CheckMouseMove drags the tab }
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (HeldPart == 5 && HeldUpdateCount > 0)
        {
            _position = (Y - TrackClickTarget) * PositionCount / TrackLength + _min;
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

    public new NotifyEvent<VerticalScrollBar> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<VerticalScrollBar> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<VerticalScrollBar> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<VerticalScrollBar> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<VerticalScrollBar> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<VerticalScrollBar> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<VerticalScrollBar> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<VerticalScrollBar> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<VerticalScrollBar> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }
}
