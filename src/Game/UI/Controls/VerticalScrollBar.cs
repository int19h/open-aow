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
        PositionCount = Max - Min + 1;
        if (PositionCount < NumberVisible)
        {
            SetNumberVisibleUnclamped(PositionCount);
        }

        switch (ScrollType)
        {
            case Style.Scroll:
                SetWinWidthWithoutInvalidate(ImageLibraryTileBack[ImageLibraryIndexBack].Width);
                var imageLibraryImageDecrementHeight = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                var imageLibraryImageIncrementHeight = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                if (imageLibraryImageDecrementHeight.Height + imageLibraryImageIncrementHeight.Height > WinHeight)
                {
                    var imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                    var imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                    SetWinHeightWithoutInvalidate(imageLibraryImageDecrement.Height + imageLibraryImageIncrement.Height);
                }

                var imageLibraryImageDecrement2 = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                var imageLibraryImageIncrement2 = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                TrackLength = WinHeight - imageLibraryImageDecrement2.Height - imageLibraryImageIncrement2.Height;
                if (TrackLength < 1)
                {
                    TrackLength = 1;
                }

                ThumbLength = TrackLength * NumberVisible / PositionCount;
                switch (ThumbStyle)
                {
                    case ScrollBarThumbStyle.Icon:
                        ThumbLength = ThumbImage[ThumbImageIndex].Height;
                        break;
                    case ScrollBarThumbStyle.Bar:
                        var thumbStripHeight = ThumbStrip[ThumbImageIndex];
                        var thumbStrip = ThumbStrip[ThumbImageIndex + 2];
                        if (ThumbLength < thumbStripHeight.Height + thumbStrip.Height)
                        {
                            var imageLibraryLineTab2 = ThumbStrip[ThumbImageIndex];
                            var imageLibraryLineTab3 = ThumbStrip[ThumbImageIndex + 2];
                            ThumbLength = imageLibraryLineTab2.Height + imageLibraryLineTab3.Height;
                        }

                        break;
                }

                break;
            case Style.Slider:
                SetWinWidthWithoutInvalidate(ImageLibraryLineBack[ImageLibraryIndexBack].Width);
                TrackLength = WinHeight;
                ThumbLength = TrackLength * NumberVisible / PositionCount;
                switch (ThumbStyle)
                {
                    case ScrollBarThumbStyle.Icon:
                        ThumbLength = ThumbImage[ThumbImageIndex].Height;
                        break;
                    case ScrollBarThumbStyle.Bar:
                        var thumbStripHeight = ThumbStrip[ThumbImageIndex];
                        var thumbStrip = ThumbStrip[ThumbImageIndex + 2];
                        if (ThumbLength < thumbStripHeight.Height + thumbStrip.Height)
                        {
                            var imageLibraryLineTab2 = ThumbStrip[ThumbImageIndex];
                            var imageLibraryLineTab3 = ThumbStrip[ThumbImageIndex + 2];
                            ThumbLength = imageLibraryLineTab2.Height + imageLibraryLineTab3.Height;
                        }

                        break;
                }

                break;
            case Style.IncrementDecrement:
                SetWinWidthWithoutInvalidate(ImageLibraryIDecrement[ImageLibraryIndexDecrementUp].Width);
                var imageLibraryImageDecrement3 = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                var imageLibraryImageIncrement3 = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                SetWinHeightWithoutInvalidate(imageLibraryImageDecrement3.Height + imageLibraryImageIncrement3.Height);
                break;
        }

        if (Position + NumberVisible > PositionCount)
        {
            SetPositionUnclamped(PositionCount - NumberVisible);
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
            if (Valid && Visible)
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
                            var imageLibraryImageIncrementHeight = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            var imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            var rectangle = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop + WinHeight - imageLibraryImageIncrement.Height, WinWidth, imageLibraryImageIncrementHeight.Height);
                            ImageLibraryImageIncrement.DrawTiled(ImageLibraryIndexIncrementDown, rectangle, ref lockedSurface);
                        }
                        else
                        {
                            var imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                            var imageLibraryImageIncrementHeight = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            var rectangle = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop + WinHeight - imageLibraryImageIncrementHeight.Height, WinWidth, imageLibraryImageIncrement.Height);
                            ImageLibraryImageIncrement.DrawTiled(ImageLibraryIndexIncrementUp, rectangle, ref lockedSurface);
                        }

                        if (HeldPart == 1 && HeldUpdateCount > 0)
                        {
                            var imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementDown];
                            var rectangle = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, imageLibraryImageDecrement.Height);
                            ImageLibraryIDecrement.DrawTiled(ImageLibraryIndexDecrementDown, rectangle, ref lockedSurface);
                        }
                        else
                        {
                            var imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                            var rectangle = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, imageLibraryImageDecrement.Height);
                            ImageLibraryIDecrement.DrawTiled(ImageLibraryIndexDecrementUp, rectangle, ref lockedSurface);
                        }

                        var imageLibraryTileBack = ImageLibraryTileBack[ImageLibraryIndexBack];
                        var imageLibraryImageDecrementHeight = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                        var rectangle2 = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop + imageLibraryImageDecrementHeight.Height, imageLibraryTileBack.Width, TrackLength);
                        ImageLibraryTileBack.DrawTiled(ImageLibraryIndexBack, rectangle2, ref lockedSurface);
                        thumbPosition = TrackLength * Position / PositionCount;
                        if (ThumbLength + thumbPosition > TrackLength)
                        {
                            thumbPosition = TrackLength - ThumbLength;
                        }

                        switch (ThumbStyle)
                        {
                            case ScrollBarThumbStyle.Icon:
                                var imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                                var rectangle = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop + imageLibraryImageDecrement.Height + thumbPosition, WinWidth, ThumbLength);
                                ThumbImage.DrawTiled(ThumbImageIndex, rectangle, ref lockedSurface);
                                break;
                            case ScrollBarThumbStyle.Bar:
                                var imageLibraryImageDecrement2 = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                                var rectangle3 = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop + imageLibraryImageDecrement2.Height + thumbPosition, WinWidth, ThumbLength);
                                ThumbStrip.DrawVerticalStrip(ThumbImageIndex, rectangle3, ref lockedSurface);
                                break;
                        }

                        break;
                    case Style.Slider:
                        var rectangle4 = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                        ImageLibraryLineBack.DrawVerticalStrip(ImageLibraryIndexBack, rectangle4, ref lockedSurface);
                        thumbPosition = TrackLength * Position / PositionCount;
                        if (ThumbLength + thumbPosition > TrackLength)
                        {
                            thumbPosition = TrackLength - ThumbLength;
                        }

                        switch (ThumbStyle)
                        {
                            case ScrollBarThumbStyle.Icon:
                                var rectangle = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop + thumbPosition, WinWidth, ThumbLength);
                                ThumbImage.DrawTiled(ThumbImageIndex, rectangle, ref lockedSurface);
                                break;
                            case ScrollBarThumbStyle.Bar:
                                var rectangle3 = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop + thumbPosition, WinWidth, ThumbLength);
                                ThumbStrip.DrawVerticalStrip(ThumbImageIndex, rectangle3, ref lockedSurface);
                                break;
                        }

                        break;
                    case Style.IncrementDecrement:
                        if (HeldPart == 4 && HeldUpdateCount > 0)
                        {
                            var imageLibraryImageIncrementHeight = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            var imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            var rectangle = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop + WinHeight - imageLibraryImageIncrement.Height, WinWidth, imageLibraryImageIncrementHeight.Height);
                            ImageLibraryImageIncrement.DrawTiled(ImageLibraryIndexIncrementDown, rectangle, ref lockedSurface);
                        }
                        else
                        {
                            var imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                            var imageLibraryImageIncrementHeight = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            var rectangle = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop + WinHeight - imageLibraryImageIncrementHeight.Height, WinWidth, imageLibraryImageIncrement.Height);
                            ImageLibraryImageIncrement.DrawTiled(ImageLibraryIndexIncrementUp, rectangle, ref lockedSurface);
                        }

                        if (HeldPart == 1 && HeldUpdateCount > 0)
                        {
                            var imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementDown];
                            var rectangle = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, imageLibraryImageDecrement.Height);
                            ImageLibraryIDecrement.DrawTiled(ImageLibraryIndexDecrementDown, rectangle, ref lockedSurface);
                        }
                        else
                        {
                            var imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                            var rectangle = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, imageLibraryImageDecrement.Height);
                            ImageLibraryIDecrement.DrawTiled(ImageLibraryIndexDecrementUp, rectangle, ref lockedSurface);
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
        if (Visible == true && X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = true;
            if (button == MouseButton.Left)
            {
                HeldUpdateCount = 1;
                HeldPart = 5;
                thumbPosition = TrackLength * Position / PositionCount;
                if (thumbPosition + ThumbLength > TrackLength)
                {
                    thumbPosition = TrackLength - ThumbLength;
                }

                if (ScrollType != Style.Slider)
                {
                    var imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                    if (Y < Window.WinTop + WinTop + imageLibraryImageDecrement.Height - 1)
                    {
                        HeldPart = 1;
                        ApplySmallStep(true);
                    }

                    var imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                    if (Y > Window.WinTop + WinTop + WinHeight - imageLibraryImageIncrement.Height)
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
                            var imageLibraryImageDecrementHeight = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                            bool condition = Y > Window.WinTop + WinTop + imageLibraryImageDecrementHeight.Height;
                            if (condition)
                            {
                                var imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                                condition = Y < Window.WinTop + WinTop + thumbPosition + imageLibraryImageIncrement.Height;
                            }

                            if (condition)
                            {
                                var imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                                TrackClickTarget = (Y - Window.WinTop - WinTop - imageLibraryImageDecrement.Height) * PositionCount / TrackLength + Min;
                                HeldPart = 2;
                                SetPositionUnclamped(Position - LargeChange);
                                if (Min > Position)
                                {
                                    SetPositionUnclamped(Min);
                                }

                                Drawn = false;
                                if (_onChange != null)
                                {
                                    _onChange(this);
                                }
                            }

                            var imageLibraryImageDecrement2 = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                            bool test = Y > Window.WinTop + WinTop + imageLibraryImageDecrement2.Height + thumbPosition + ThumbLength;
                            if (test)
                            {
                                var imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                                test = Y < Window.WinTop + WinTop + WinHeight - imageLibraryImageIncrement.Height;
                            }

                            if (test)
                            {
                                var imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                                TrackClickTarget = (Y - Window.WinTop - WinTop - imageLibraryImageDecrement.Height) * PositionCount / TrackLength + Min;
                                HeldPart = 3;
                                SetPositionUnclamped(Position + LargeChange);
                                if (Max + 1 - NumberVisible < Position)
                                {
                                    SetPositionUnclamped(Max + 1 - NumberVisible);
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
                            if (Y > Window.WinTop + WinTop && Y < Window.WinTop + WinTop + thumbPosition)
                            {
                                TrackClickTarget = (Y - Window.WinTop - WinTop) * PositionCount / TrackLength + Min;
                                HeldPart = 2;
                                SetPositionUnclamped(Position - LargeChange);
                                if (Min > Position)
                                {
                                    SetPositionUnclamped(Min);
                                }

                                Drawn = false;
                                if (_onChange != null)
                                {
                                    _onChange(this);
                                }
                            }

                            if (Y > Window.WinTop + WinTop + thumbPosition + ThumbLength && Y < Window.WinTop + WinTop + WinHeight)
                            {
                                TrackClickTarget = (Y - Window.WinTop - WinTop) * PositionCount / TrackLength + Min;
                                HeldPart = 3;
                                SetPositionUnclamped(Position + LargeChange);
                                if (Max + 1 - NumberVisible < Position)
                                {
                                    SetPositionUnclamped(Max + 1 - NumberVisible);
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
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
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
            SetPositionUnclamped((Y - TrackClickTarget) * PositionCount / TrackLength + Min);
            if (Position < Min)
            {
                SetPositionUnclamped(Min);
            }

            if (Max + 1 - NumberVisible < Position)
            {
                SetPositionUnclamped(Max + 1 - NumberVisible);
            }

            Drawn = false;
            if (_onChange != null)
            {
                _onChange(this);
            }
        }

        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
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
