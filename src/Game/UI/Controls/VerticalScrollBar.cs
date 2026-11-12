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
                RaisePreDraw(this, lockedSurface);

                switch (ScrollType)
                {
                    case Style.Scroll:
                        if (HeldPart == 4 && HeldUpdateCount > 0)
                        {
                            var imageLibraryImageIncrementHeight = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            var imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop + WinHeight - imageLibraryImageIncrement.Height, WinWidth, imageLibraryImageIncrementHeight.Height);
                            ImageLibraryImageIncrement.DrawTiled(ImageLibraryIndexIncrementDown, rectangle, ref lockedSurface);
                        }
                        else
                        {
                            var imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                            var imageLibraryImageIncrementHeight = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop + WinHeight - imageLibraryImageIncrementHeight.Height, WinWidth, imageLibraryImageIncrement.Height);
                            ImageLibraryImageIncrement.DrawTiled(ImageLibraryIndexIncrementUp, rectangle, ref lockedSurface);
                        }

                        if (HeldPart == 1 && HeldUpdateCount > 0)
                        {
                            var imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementDown];
                            var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, imageLibraryImageDecrement.Height);
                            ImageLibraryIDecrement.DrawTiled(ImageLibraryIndexDecrementDown, rectangle, ref lockedSurface);
                        }
                        else
                        {
                            var imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                            var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, imageLibraryImageDecrement.Height);
                            ImageLibraryIDecrement.DrawTiled(ImageLibraryIndexDecrementUp, rectangle, ref lockedSurface);
                        }

                        var imageLibraryTileBack = ImageLibraryTileBack[ImageLibraryIndexBack];
                        var imageLibraryImageDecrementHeight = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                        var rectangle2 = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop + imageLibraryImageDecrementHeight.Height, imageLibraryTileBack.Width, TrackLength);
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
                                var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop + imageLibraryImageDecrement.Height + thumbPosition, WinWidth, ThumbLength);
                                ThumbImage.DrawTiled(ThumbImageIndex, rectangle, ref lockedSurface);
                                break;
                            case ScrollBarThumbStyle.Bar:
                                var imageLibraryImageDecrement2 = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                                var rectangle3 = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop + imageLibraryImageDecrement2.Height + thumbPosition, WinWidth, ThumbLength);
                                ThumbStrip.DrawVerticalStrip(ThumbImageIndex, rectangle3, ref lockedSurface);
                                break;
                        }

                        break;
                    case Style.Slider:
                        var rectangle4 = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                        ImageLibraryLineBack.DrawVerticalStrip(ImageLibraryIndexBack, rectangle4, ref lockedSurface);
                        thumbPosition = TrackLength * Position / PositionCount;
                        if (ThumbLength + thumbPosition > TrackLength)
                        {
                            thumbPosition = TrackLength - ThumbLength;
                        }

                        switch (ThumbStyle)
                        {
                            case ScrollBarThumbStyle.Icon:
                                var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop + thumbPosition, WinWidth, ThumbLength);
                                ThumbImage.DrawTiled(ThumbImageIndex, rectangle, ref lockedSurface);
                                break;
                            case ScrollBarThumbStyle.Bar:
                                var rectangle3 = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop + thumbPosition, WinWidth, ThumbLength);
                                ThumbStrip.DrawVerticalStrip(ThumbImageIndex, rectangle3, ref lockedSurface);
                                break;
                        }

                        break;
                    case Style.IncrementDecrement:
                        if (HeldPart == 4 && HeldUpdateCount > 0)
                        {
                            var imageLibraryImageIncrementHeight = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            var imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop + WinHeight - imageLibraryImageIncrement.Height, WinWidth, imageLibraryImageIncrementHeight.Height);
                            ImageLibraryImageIncrement.DrawTiled(ImageLibraryIndexIncrementDown, rectangle, ref lockedSurface);
                        }
                        else
                        {
                            var imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                            var imageLibraryImageIncrementHeight = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop + WinHeight - imageLibraryImageIncrementHeight.Height, WinWidth, imageLibraryImageIncrement.Height);
                            ImageLibraryImageIncrement.DrawTiled(ImageLibraryIndexIncrementUp, rectangle, ref lockedSurface);
                        }

                        if (HeldPart == 1 && HeldUpdateCount > 0)
                        {
                            var imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementDown];
                            var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, imageLibraryImageDecrement.Height);
                            ImageLibraryIDecrement.DrawTiled(ImageLibraryIndexDecrementDown, rectangle, ref lockedSurface);
                        }
                        else
                        {
                            var imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                            var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, imageLibraryImageDecrement.Height);
                            ImageLibraryIDecrement.DrawTiled(ImageLibraryIndexDecrementUp, rectangle, ref lockedSurface);
                        }

                        break;
                }

                RaiseDraw(this, lockedSurface);
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
                                RaiseChange(this);
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
                                RaiseChange(this);
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
                                RaiseChange(this);
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
                                RaiseChange(this);
                            }

                            if (HeldPart == 5)
                            {
                                TrackClickTarget = Y - thumbPosition;
                            }

                            break;
                    }
                }
            }

            RaiseMouseDown(this, button, shift, X, Y);
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
            RaiseMouseUp(this, button, shift, X, Y);
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
            RaiseChange(this);
        }

        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = true;
            RaiseMouseMove(this, shift, X, Y);
        }
    }

    public new event NotifyEvent<VerticalScrollBar> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, VerticalScrollBar>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, VerticalScrollBar>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<VerticalScrollBar> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, VerticalScrollBar>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, VerticalScrollBar>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<VerticalScrollBar> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, VerticalScrollBar>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, VerticalScrollBar>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<VerticalScrollBar> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, VerticalScrollBar>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, VerticalScrollBar>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<VerticalScrollBar> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, VerticalScrollBar>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, VerticalScrollBar>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<VerticalScrollBar> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, VerticalScrollBar>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, VerticalScrollBar>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<VerticalScrollBar> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, VerticalScrollBar>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, VerticalScrollBar>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<VerticalScrollBar> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, VerticalScrollBar>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, VerticalScrollBar>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<VerticalScrollBar> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, VerticalScrollBar>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, VerticalScrollBar>(this, slot: nameof(OnUpdate), value: value);
    }
}
