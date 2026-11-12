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
        PositionCount = Max - Min + 1;
        if (PositionCount < NumberVisible)
        {
            SetNumberVisibleUnclamped(PositionCount);
        }

        switch (ScrollType)
        {
            case Style.Scroll:
                SetWinHeightWithoutInvalidate(ImageLibraryTileBack[ImageLibraryIndexBack].Height);
                var imageLibraryImageDecrementWidth = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                var imageLibraryImageIncrementWidth = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                if (imageLibraryImageDecrementWidth.Width + imageLibraryImageIncrementWidth.Width > WinWidth)
                {
                    var imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                    var imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                    SetWinWidthWithoutInvalidate(imageLibraryImageDecrement.Width + imageLibraryImageIncrement.Width);
                }

                var imageLibraryImageDecrement2 = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                var imageLibraryImageIncrement2 = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                TrackLength = WinWidth - imageLibraryImageDecrement2.Width - imageLibraryImageIncrement2.Width;
                ThumbLength = TrackLength * NumberVisible / PositionCount;
                switch (ThumbStyle)
                {
                    case ScrollBarThumbStyle.Icon:
                        ThumbLength = ThumbImage[ThumbImageIndex].Width;
                        break;
                    case ScrollBarThumbStyle.Bar:
                        var thumbStripWidth = ThumbStrip[ThumbImageIndex];
                        var thumbStrip = ThumbStrip[ThumbImageIndex + 2];
                        if (ThumbLength < thumbStripWidth.Width + thumbStrip.Width)
                        {
                            var imageLibraryLineTab2 = ThumbStrip[ThumbImageIndex];
                            var imageLibraryLineTab3 = ThumbStrip[ThumbImageIndex + 2];
                            ThumbLength = imageLibraryLineTab2.Width + imageLibraryLineTab3.Width;
                        }

                        break;
                }

                break;
            case Style.Slider:
                SetWinHeightWithoutInvalidate(ImageLibraryLineBack[ImageLibraryIndexBack].Height);
                TrackLength = WinWidth;
                ThumbLength = TrackLength * NumberVisible / PositionCount;
                switch (ThumbStyle)
                {
                    case ScrollBarThumbStyle.Icon:
                        ThumbLength = ThumbImage[ThumbImageIndex].Width;
                        break;
                    case ScrollBarThumbStyle.Bar:
                        var thumbStripWidth = ThumbStrip[ThumbImageIndex];
                        var thumbStrip = ThumbStrip[ThumbImageIndex + 2];
                        if (ThumbLength < thumbStripWidth.Width + thumbStrip.Width)
                        {
                            var imageLibraryLineTab2 = ThumbStrip[ThumbImageIndex];
                            var imageLibraryLineTab3 = ThumbStrip[ThumbImageIndex + 2];
                            ThumbLength = imageLibraryLineTab2.Width + imageLibraryLineTab3.Width;
                        }

                        break;
                }

                break;
            case Style.IncrementDecrement:
                SetWinHeightWithoutInvalidate(ImageLibraryIDecrement[ImageLibraryIndexDecrementUp].Height);
                var imageLibraryImageDecrement3 = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                var imageLibraryImageIncrement3 = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                SetWinWidthWithoutInvalidate(imageLibraryImageDecrement3.Width + imageLibraryImageIncrement3.Width);
                break;
        }

        if (Position + NumberVisible > PositionCount)
        {
            SetPositionUnclamped(PositionCount - NumberVisible);
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
            if (Valid && Visible)
            {
                RaisePreDraw(this, lockedSurface);

                switch (ScrollType)
                {
                    case Style.Scroll:
                        if (HeldPart == 4 && HeldUpdateCount > 0)
                        {
                            var imageLibraryImageIncrementWidth = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            var imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            var rectangle = new SizedRectangle(Window.WinLeft + WinLeft + WinWidth - imageLibraryImageIncrement.Width, Window.WinTop + WinTop, imageLibraryImageIncrementWidth.Width, WinHeight);
                            ImageLibraryImageIncrement.DrawTiled(ImageLibraryIndexIncrementDown, rectangle, ref lockedSurface);
                        }
                        else
                        {
                            var imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                            var imageLibraryImageIncrementWidth = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            var rectangle = new SizedRectangle(Window.WinLeft + WinLeft + WinWidth - imageLibraryImageIncrementWidth.Width, Window.WinTop + WinTop, imageLibraryImageIncrement.Width, WinHeight);
                            ImageLibraryImageIncrement.DrawTiled(ImageLibraryIndexIncrementUp, rectangle, ref lockedSurface);
                        }

                        if (HeldPart == 1 && HeldUpdateCount > 0)
                        {
                            var imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementDown];
                            var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop, imageLibraryImageDecrement.Width, WinHeight);
                            ImageLibraryIDecrement.DrawTiled(ImageLibraryIndexDecrementDown, rectangle, ref lockedSurface);
                        }
                        else
                        {
                            var imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                            var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop, imageLibraryImageDecrement.Width, WinHeight);
                            ImageLibraryIDecrement.DrawTiled(ImageLibraryIndexDecrementUp, rectangle, ref lockedSurface);
                        }

                        var imageLibraryTileBack = ImageLibraryTileBack[ImageLibraryIndexBack];
                        var imageLibraryImageDecrementWidth = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                        var rectangle2 = new SizedRectangle(Window.WinLeft + WinLeft + imageLibraryImageDecrementWidth.Width, Window.WinTop + WinTop, TrackLength, imageLibraryTileBack.Height);
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
                                var rectangle = new SizedRectangle(Window.WinLeft + WinLeft + imageLibraryImageDecrement.Width + thumbPosition, Window.WinTop + WinTop, ThumbLength, WinHeight);
                                ThumbImage.DrawTiled(ThumbImageIndex, rectangle, ref lockedSurface);
                                break;
                            case ScrollBarThumbStyle.Bar:
                                var imageLibraryImageDecrement2 = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                                var rectangle3 = new SizedRectangle(Window.WinLeft + WinLeft + imageLibraryImageDecrement2.Width + thumbPosition, Window.WinTop + WinTop, ThumbLength, WinHeight);
                                ThumbStrip.DrawHorizontalStrip(ThumbImageIndex, rectangle3, ref lockedSurface);
                                break;
                        }

                        break;
                    case Style.Slider:
                        var rectangle4 = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                        ImageLibraryLineBack.DrawHorizontalStrip(ImageLibraryIndexBack, rectangle4, ref lockedSurface);
                        thumbPosition = TrackLength * Position / PositionCount;
                        if (ThumbLength + thumbPosition > TrackLength)
                        {
                            thumbPosition = TrackLength - ThumbLength;
                        }

                        switch (ThumbStyle)
                        {
                            case ScrollBarThumbStyle.Icon:
                                var rectangle = new SizedRectangle(Window.WinLeft + WinLeft + thumbPosition, Window.WinTop + WinTop, ThumbLength, WinHeight);
                                ThumbImage.DrawTiled(ThumbImageIndex, rectangle, ref lockedSurface);
                                break;
                            case ScrollBarThumbStyle.Bar:
                                var rectangle3 = new SizedRectangle(Window.WinLeft + WinLeft + thumbPosition, Window.WinTop + WinTop, ThumbLength, WinHeight);
                                ThumbStrip.DrawHorizontalStrip(ThumbImageIndex, rectangle3, ref lockedSurface);
                                break;
                        }

                        break;
                    case Style.IncrementDecrement:
                        if (HeldPart == 4 && HeldUpdateCount > 0)
                        {
                            var imageLibraryImageIncrementWidth = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            var imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            var rectangle = new SizedRectangle(Window.WinLeft + WinLeft - imageLibraryImageIncrement.Width, Window.WinTop + WinTop + WinHeight, imageLibraryImageIncrementWidth.Width, WinHeight);
                            ImageLibraryImageIncrement.DrawTiled(ImageLibraryIndexIncrementDown, rectangle, ref lockedSurface);
                        }
                        else
                        {
                            var imageLibraryImageIncrementWidth = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                            var imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                            var rectangle = new SizedRectangle(Window.WinLeft + WinLeft - imageLibraryImageIncrement.Width, Window.WinTop + WinTop + WinHeight, imageLibraryImageIncrementWidth.Width, WinHeight);
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
        if (Visible == true && X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            oldPosition = Position;
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
                    if (X < Window.WinLeft + WinLeft + imageLibraryImageDecrement.Width - 1)
                    {
                        HeldPart = 1;
                        SetPositionUnclamped(Position - SmallChange);
                        if (Min > Position)
                        {
                            SetPositionUnclamped(Min);
                        }
                    }

                    var imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                    if (X > Window.WinLeft + WinLeft + WinWidth - imageLibraryImageIncrement.Width)
                    {
                        HeldPart = 4;
                        SetPositionUnclamped(Position + SmallChange);
                        if (Max + 1 - NumberVisible < Position)
                        {
                            SetPositionUnclamped(Max + 1 - NumberVisible);
                        }
                    }
                }

                if (HeldPart == 5)
                {
                    switch (ScrollType)
                    {
                        case Style.Scroll:
                            var imageLibraryImageDecrementWidth = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                            bool condition = X > Window.WinLeft + WinLeft + imageLibraryImageDecrementWidth.Width;
                            if (condition)
                            {
                                var imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                                condition = X < Window.WinLeft + WinLeft + thumbPosition + imageLibraryImageIncrement.Width;
                            }

                            if (condition)
                            {
                                var imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                                TrackClickTarget = (X - Window.WinLeft - WinLeft - imageLibraryImageDecrement.Width) * PositionCount / TrackLength + Min;
                                HeldPart = 2;
                                SetPositionUnclamped(Position - LargeChange);
                                if (Min > Position)
                                {
                                    SetPositionUnclamped(Min);
                                }
                            }

                            var imageLibraryImageDecrement2 = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                            bool test = X > Window.WinLeft + WinLeft + imageLibraryImageDecrement2.Width + thumbPosition + ThumbLength;
                            if (test)
                            {
                                var imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                                test = X < Window.WinLeft + WinLeft + WinWidth - imageLibraryImageDecrement.Width;
                            }

                            if (test)
                            {
                                var imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                                TrackClickTarget = (X - Window.WinLeft - WinLeft - imageLibraryImageDecrement.Width) * PositionCount / TrackLength + Min;
                                HeldPart = 3;
                                SetPositionUnclamped(Position + LargeChange);
                                if (Max + 1 - NumberVisible < Position)
                                {
                                    SetPositionUnclamped(Max + 1 - NumberVisible);
                                }
                            }

                            if (HeldPart == 5)
                            {
                                TrackClickTarget = X - thumbPosition;
                            }

                            break;
                        case Style.Slider:
                            if (X > Window.WinLeft + WinLeft && X < Window.WinLeft + WinLeft + thumbPosition)
                            {
                                TrackClickTarget = (X - Window.WinLeft - WinLeft) * PositionCount / TrackLength + Min;
                                HeldPart = 2;
                                SetPositionUnclamped(Position - LargeChange);
                                if (Min > Position)
                                {
                                    SetPositionUnclamped(Min);
                                }
                            }

                            if (X > Window.WinLeft + WinLeft + thumbPosition + ThumbLength && X < Window.WinLeft + WinLeft + WinWidth)
                            {
                                TrackClickTarget = (X - Window.WinLeft - WinLeft) * PositionCount / TrackLength + Min;
                                HeldPart = 3;
                                SetPositionUnclamped(Position + LargeChange);
                                if (Max + 1 - NumberVisible < Position)
                                {
                                    SetPositionUnclamped(Max + 1 - NumberVisible);
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

            if (oldPosition != Position)
            {
                Drawn = false;
                RaiseChange(this);
            }

            RaiseMouseDown(this, button, shift, X, Y);
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
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = true;
            RaiseMouseUp(this, button, shift, X, Y);
        }
    }

    // AoWHScroll.pas:291
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (HeldPart == 5 && HeldUpdateCount > 0)
        {
            SetPositionUnclamped((X - TrackClickTarget) * PositionCount / TrackLength + Min);
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

    public new event NotifyEvent<HorizontalScrollBar> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, HorizontalScrollBar>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, HorizontalScrollBar>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<HorizontalScrollBar> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, HorizontalScrollBar>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, HorizontalScrollBar>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<HorizontalScrollBar> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, HorizontalScrollBar>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, HorizontalScrollBar>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<HorizontalScrollBar> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, HorizontalScrollBar>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, HorizontalScrollBar>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<HorizontalScrollBar> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, HorizontalScrollBar>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, HorizontalScrollBar>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<HorizontalScrollBar> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, HorizontalScrollBar>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, HorizontalScrollBar>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<HorizontalScrollBar> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, HorizontalScrollBar>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, HorizontalScrollBar>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<HorizontalScrollBar> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, HorizontalScrollBar>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, HorizontalScrollBar>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<HorizontalScrollBar> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, HorizontalScrollBar>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, HorizontalScrollBar>(this, slot: nameof(OnUpdate), value: value);
    }
}
