// Translated from AoWHScroll.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Rtl;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Controls;

// { The AoWHScroll unit of aowInt.dpl: TAOWHScrollBar, the horizontal scroll bar (TAOWScrollBar of
//   AoWVScroll with the decrement button at the left and the increment button at the right). }
// no fields of its own
public partial class HorizontalScrollBar(Component owner) : ScrollBar(owner)
{

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
                LibraryImage imageLibraryImageDecrementWidth = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                LibraryImage imageLibraryImageIncrementWidth = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                if (imageLibraryImageDecrementWidth.Width + imageLibraryImageIncrementWidth.Width > WinWidth)
                {
                    LibraryImage imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                    LibraryImage imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                    SetWinWidthWithoutInvalidate(imageLibraryImageDecrement.Width + imageLibraryImageIncrement.Width);
                }

                LibraryImage imageLibraryImageDecrement2 = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                LibraryImage imageLibraryImageIncrement2 = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                TrackLength = WinWidth - imageLibraryImageDecrement2.Width - imageLibraryImageIncrement2.Width;
                ThumbLength = TrackLength * NumberVisible / PositionCount;
                switch (ThumbStyle)
                {
                    case ScrollBarThumbStyle.Icon:
                        ThumbLength = ThumbImage[ThumbImageIndex].Width;
                        break;
                    case ScrollBarThumbStyle.Bar:
                        LibraryImage thumbStripWidth = ThumbStrip[ThumbImageIndex];
                        LibraryImage thumbStrip = ThumbStrip[ThumbImageIndex + 2];
                        if (ThumbLength < thumbStripWidth.Width + thumbStrip.Width)
                        {
                            LibraryImage imageLibraryLineTab2 = ThumbStrip[ThumbImageIndex];
                            LibraryImage imageLibraryLineTab3 = ThumbStrip[ThumbImageIndex + 2];
                            ThumbLength = imageLibraryLineTab2.Width + imageLibraryLineTab3.Width;
                        }

                        break;
                    default:
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
                        LibraryImage thumbStripWidth = ThumbStrip[ThumbImageIndex];
                        LibraryImage thumbStrip = ThumbStrip[ThumbImageIndex + 2];
                        if (ThumbLength < thumbStripWidth.Width + thumbStrip.Width)
                        {
                            LibraryImage imageLibraryLineTab2 = ThumbStrip[ThumbImageIndex];
                            LibraryImage imageLibraryLineTab3 = ThumbStrip[ThumbImageIndex + 2];
                            ThumbLength = imageLibraryLineTab2.Width + imageLibraryLineTab3.Width;
                        }

                        break;
                    default:
                        break;
                }

                break;
            case Style.IncrementDecrement:
                SetWinHeightWithoutInvalidate(ImageLibraryIDecrement[ImageLibraryIndexDecrementUp].Height);
                LibraryImage imageLibraryImageDecrement3 = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                LibraryImage imageLibraryImageIncrement3 = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                SetWinWidthWithoutInvalidate(imageLibraryImageDecrement3.Width + imageLibraryImageIncrement3.Width);
                break;
            default:
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
        try
        {
            if (Valid && Visible)
            {
                RaisePreDraw(this, surfDesc: lockedSurface);

                int thumbPosition;
                switch (ScrollType)
                {
                    case Style.Scroll:
                        if (HeldPart == 4 && HeldUpdateCount > 0)
                        {
                            LibraryImage imageLibraryImageIncrementWidth = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            LibraryImage imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft + WinWidth - imageLibraryImageIncrement.Width, Window.AbsoluteTop + WinTop, imageLibraryImageIncrementWidth.Width, WinHeight);
                            ImageLibraryImageIncrement.DrawTiled(ImageLibraryIndexIncrementDown, area: rectangle, ref lockedSurface);
                        }
                        else
                        {
                            LibraryImage imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                            LibraryImage imageLibraryImageIncrementWidth = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft + WinWidth - imageLibraryImageIncrementWidth.Width, Window.AbsoluteTop + WinTop, imageLibraryImageIncrement.Width, WinHeight);
                            ImageLibraryImageIncrement.DrawTiled(ImageLibraryIndexIncrementUp, area: rectangle, ref lockedSurface);
                        }

                        if (HeldPart == 1 && HeldUpdateCount > 0)
                        {
                            LibraryImage imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementDown];
                            SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, imageLibraryImageDecrement.Width, WinHeight);
                            ImageLibraryIDecrement.DrawTiled(ImageLibraryIndexDecrementDown, area: rectangle, ref lockedSurface);
                        }
                        else
                        {
                            LibraryImage imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                            SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, imageLibraryImageDecrement.Width, WinHeight);
                            ImageLibraryIDecrement.DrawTiled(ImageLibraryIndexDecrementUp, area: rectangle, ref lockedSurface);
                        }

                        LibraryImage imageLibraryTileBack = ImageLibraryTileBack[ImageLibraryIndexBack];
                        LibraryImage imageLibraryImageDecrementWidth = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                        SizedRectangle rectangle2 = new(Window.AbsoluteLeft + WinLeft + imageLibraryImageDecrementWidth.Width, Window.AbsoluteTop + WinTop, TrackLength, imageLibraryTileBack.Height);
                        ImageLibraryTileBack.DrawTiled(ImageLibraryIndexBack, area: rectangle2, ref lockedSurface);
                        thumbPosition = TrackLength * Position / PositionCount;
                        if (ThumbLength + thumbPosition > TrackLength)
                        {
                            thumbPosition = TrackLength - ThumbLength;
                        }

                        switch (ThumbStyle)
                        {
                            case ScrollBarThumbStyle.Icon:
                                LibraryImage imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                                SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft + imageLibraryImageDecrement.Width + thumbPosition, Window.AbsoluteTop + WinTop, ThumbLength, WinHeight);
                                ThumbImage.DrawTiled(ThumbImageIndex, area: rectangle, ref lockedSurface);
                                break;
                            case ScrollBarThumbStyle.Bar:
                                LibraryImage imageLibraryImageDecrement2 = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                                SizedRectangle rectangle3 = new(Window.AbsoluteLeft + WinLeft + imageLibraryImageDecrement2.Width + thumbPosition, Window.AbsoluteTop + WinTop, ThumbLength, WinHeight);
                                ThumbStrip.DrawHorizontalStrip(ThumbImageIndex, area: rectangle3, ref lockedSurface);
                                break;
                            default:
                                break;
                        }

                        break;
                    case Style.Slider:
                        SizedRectangle rectangle4 = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, WinWidth, WinHeight);
                        ImageLibraryLineBack.DrawHorizontalStrip(ImageLibraryIndexBack, area: rectangle4, ref lockedSurface);
                        thumbPosition = TrackLength * Position / PositionCount;
                        if (ThumbLength + thumbPosition > TrackLength)
                        {
                            thumbPosition = TrackLength - ThumbLength;
                        }

                        switch (ThumbStyle)
                        {
                            case ScrollBarThumbStyle.Icon:
                                SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft + thumbPosition, Window.AbsoluteTop + WinTop, ThumbLength, WinHeight);
                                ThumbImage.DrawTiled(ThumbImageIndex, area: rectangle, ref lockedSurface);
                                break;
                            case ScrollBarThumbStyle.Bar:
                                SizedRectangle rectangle3 = new(Window.AbsoluteLeft + WinLeft + thumbPosition, Window.AbsoluteTop + WinTop, ThumbLength, WinHeight);
                                ThumbStrip.DrawHorizontalStrip(ThumbImageIndex, area: rectangle3, ref lockedSurface);
                                break;
                            default:
                                break;
                        }

                        break;
                    case Style.IncrementDecrement:
                        if (HeldPart == 4 && HeldUpdateCount > 0)
                        {
                            LibraryImage imageLibraryImageIncrementWidth = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            LibraryImage imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementDown];
                            SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft - imageLibraryImageIncrement.Width, Window.AbsoluteTop + WinTop + WinHeight, imageLibraryImageIncrementWidth.Width, WinHeight);
                            ImageLibraryImageIncrement.DrawTiled(ImageLibraryIndexIncrementDown, area: rectangle, ref lockedSurface);
                        }
                        else
                        {
                            LibraryImage imageLibraryImageIncrementWidth = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                            LibraryImage imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                            SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft - imageLibraryImageIncrement.Width, Window.AbsoluteTop + WinTop + WinHeight, imageLibraryImageIncrementWidth.Width, WinHeight);
                            ImageLibraryImageIncrement.DrawTiled(ImageLibraryIndexIncrementUp, area: rectangle, ref lockedSurface);
                        }

                        if (HeldPart == 1 && HeldUpdateCount > 0)
                        {
                            LibraryImage imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementDown];
                            SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, WinWidth, imageLibraryImageDecrement.Height);
                            ImageLibraryIDecrement.DrawTiled(ImageLibraryIndexDecrementDown, area: rectangle, ref lockedSurface);
                        }
                        else
                        {
                            LibraryImage imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                            SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, WinWidth, imageLibraryImageDecrement.Height);
                            ImageLibraryIDecrement.DrawTiled(ImageLibraryIndexDecrementUp, area: rectangle, ref lockedSurface);
                        }

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

    // AoWHScroll.pas:171
    // { CheckMouseDown: as TAOWVScrollBar.CheckMouseDown across, but OnChange is called once at the end
    //   when the position changed. The track after the tab ends at the width of the decrement button
    //   (original) }
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (Visible && X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            int oldPosition = Position;
            Handled = true;
            if (button == MouseButton.Left)
            {
                HeldUpdateCount = 1;
                HeldPart = 5;
                int thumbPosition = TrackLength * Position / PositionCount;
                if (thumbPosition + ThumbLength > TrackLength)
                {
                    thumbPosition = TrackLength - ThumbLength;
                }

                if (ScrollType != Style.Slider)
                {
                    LibraryImage imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                    if (X < Window.AbsoluteLeft + WinLeft + imageLibraryImageDecrement.Width - 1)
                    {
                        HeldPart = 1;
                        SetPositionUnclamped(Position - SmallChange);
                        if (Min > Position)
                        {
                            SetPositionUnclamped(Min);
                        }
                    }

                    LibraryImage imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                    if (X > Window.AbsoluteLeft + WinLeft + WinWidth - imageLibraryImageIncrement.Width)
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
                            LibraryImage imageLibraryImageDecrementWidth = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                            bool condition = X > Window.AbsoluteLeft + WinLeft + imageLibraryImageDecrementWidth.Width;
                            if (condition)
                            {
                                LibraryImage imageLibraryImageIncrement = ImageLibraryImageIncrement[ImageLibraryIndexIncrementUp];
                                condition = X < Window.AbsoluteLeft + WinLeft + thumbPosition + imageLibraryImageIncrement.Width;
                            }

                            if (condition)
                            {
                                LibraryImage imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                                TrackClickTarget = ((X - Window.AbsoluteLeft - WinLeft - imageLibraryImageDecrement.Width) * PositionCount / TrackLength) + Min;
                                HeldPart = 2;
                                SetPositionUnclamped(Position - LargeChange);
                                if (Min > Position)
                                {
                                    SetPositionUnclamped(Min);
                                }
                            }

                            LibraryImage imageLibraryImageDecrement2 = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                            bool test = X > Window.AbsoluteLeft + WinLeft + imageLibraryImageDecrement2.Width + thumbPosition + ThumbLength;
                            if (test)
                            {
                                LibraryImage imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                                test = X < Window.AbsoluteLeft + WinLeft + WinWidth - imageLibraryImageDecrement.Width;
                            }

                            if (test)
                            {
                                LibraryImage imageLibraryImageDecrement = ImageLibraryIDecrement[ImageLibraryIndexDecrementUp];
                                TrackClickTarget = ((X - Window.AbsoluteLeft - WinLeft - imageLibraryImageDecrement.Width) * PositionCount / TrackLength) + Min;
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
                            if (X > Window.AbsoluteLeft + WinLeft && X < Window.AbsoluteLeft + WinLeft + thumbPosition)
                            {
                                TrackClickTarget = ((X - Window.AbsoluteLeft - WinLeft) * PositionCount / TrackLength) + Min;
                                HeldPart = 2;
                                SetPositionUnclamped(Position - LargeChange);
                                if (Min > Position)
                                {
                                    SetPositionUnclamped(Min);
                                }
                            }

                            if (X > Window.AbsoluteLeft + WinLeft + thumbPosition + ThumbLength && X < Window.AbsoluteLeft + WinLeft + WinWidth)
                            {
                                TrackClickTarget = ((X - Window.AbsoluteLeft - WinLeft) * PositionCount / TrackLength) + Min;
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
                        case Style.IncrementDecrement:
                        default:
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
        if (HeldPart is 1 or 4)
        {
            Drawn = false;
        }

        HeldUpdateCount = 0;
        HeldPart = 0;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
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
            SetPositionUnclamped(((X - TrackClickTarget) * PositionCount / TrackLength) + Min);
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

        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
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
