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
        F14C = FMax - FMin + 1;
        if (F14C < FNumberVisible)
        {
            FNumberVisible = F14C;
        }

        switch (FScrollType)
        {
            case Style.Scroll:
                FWinWidth = FILTBack[FILIndexBack].Width;
                var iliDecHeight = FILIDec[FILIndexDecUp];
                var iliIncHeight = FILIInc[FILIndexIncUp];
                if (iliDecHeight.Height + iliIncHeight.Height > FWinHeight)
                {
                    var iliDec = FILIDec[FILIndexDecUp];
                    var iliInc = FILIInc[FILIndexIncUp];
                    FWinHeight = iliDec.Height + iliInc.Height;
                }

                var iliDec2 = FILIDec[FILIndexDecUp];
                var iliInc2 = FILIInc[FILIndexIncUp];
                F130 = FWinHeight - iliDec2.Height - iliInc2.Height;
                if (F130 < 1)
                {
                    F130 = 1;
                }

                F134 = F130 * FNumberVisible / F14C;
                switch (FTabType)
                {
                    case ThumbStyle.Icon:
                        F134 = FILITab[FILIndexTab].Height;
                        break;
                    case ThumbStyle.Bar:
                        var illTabHeight = FILLTab[FILIndexTab];
                        var illTab = FILLTab[FILIndexTab + 2];
                        if (F134 < illTabHeight.Height + illTab.Height)
                        {
                            var illTab2 = FILLTab[FILIndexTab];
                            var illTab3 = FILLTab[FILIndexTab + 2];
                            F134 = illTab2.Height + illTab3.Height;
                        }

                        break;
                }

                break;
            case Style.Slider:
                FWinWidth = FILLBack[FILIndexBack].Width;
                F130 = FWinHeight;
                F134 = F130 * FNumberVisible / F14C;
                switch (FTabType)
                {
                    case ThumbStyle.Icon:
                        F134 = FILITab[FILIndexTab].Height;
                        break;
                    case ThumbStyle.Bar:
                        var illTabHeight = FILLTab[FILIndexTab];
                        var illTab = FILLTab[FILIndexTab + 2];
                        if (F134 < illTabHeight.Height + illTab.Height)
                        {
                            var illTab2 = FILLTab[FILIndexTab];
                            var illTab3 = FILLTab[FILIndexTab + 2];
                            F134 = illTab2.Height + illTab3.Height;
                        }

                        break;
                }

                break;
            case Style.IncDec:
                FWinWidth = FILIDec[FILIndexDecUp].Width;
                var iliDec3 = FILIDec[FILIndexDecUp];
                var iliInc3 = FILIInc[FILIndexIncUp];
                FWinHeight = iliDec3.Height + iliInc3.Height;
                break;
        }

        if (FPosition + FNumberVisible > F14C)
        {
            FPosition = F14C - FNumberVisible;
        }

        base.SetSize();
    }

    // AoWVScroll.pas:457
    // { Draw draws the buttons (pressed while held), the background and the tab. The top of the
    //   increment button is computed from the height of its pressed image in both states (original) }
    public override void Draw(LockedSurface SurfDesc)
    {
        int TabPos = default;
        try
        {
            if (FValid && FVisible)
            {
                if (FOnPreDraw != null)
                {
                    FOnPreDraw(this, SurfDesc);
                }

                switch (FScrollType)
                {
                    case Style.Scroll:
                        if (F151 == 4 && F150 > 0)
                        {
                            var iliIncHeight = FILIInc[FILIndexIncDown];
                            var iliInc = FILIInc[FILIndexIncDown];
                            var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + FWinHeight - iliInc.Height, FWinWidth, iliIncHeight.Height);
                            FILIInc.DrawILT(FILIndexIncDown, rect, ref SurfDesc);
                        }
                        else
                        {
                            var iliInc = FILIInc[FILIndexIncUp];
                            var iliIncHeight = FILIInc[FILIndexIncDown];
                            var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + FWinHeight - iliIncHeight.Height, FWinWidth, iliInc.Height);
                            FILIInc.DrawILT(FILIndexIncUp, rect, ref SurfDesc);
                        }

                        if (F151 == 1 && F150 > 0)
                        {
                            var iliDec = FILIDec[FILIndexDecDown];
                            var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, iliDec.Height);
                            FILIDec.DrawILT(FILIndexDecDown, rect, ref SurfDesc);
                        }
                        else
                        {
                            var iliDec = FILIDec[FILIndexDecUp];
                            var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, iliDec.Height);
                            FILIDec.DrawILT(FILIndexDecUp, rect, ref SurfDesc);
                        }

                        var iltBack = FILTBack[FILIndexBack];
                        var iliDecHeight = FILIDec[FILIndexDecUp];
                        var rect2 = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + iliDecHeight.Height, iltBack.Width, F130);
                        FILTBack.DrawILT(FILIndexBack, rect2, ref SurfDesc);
                        TabPos = F130 * FPosition / F14C;
                        if (F134 + TabPos > F130)
                        {
                            TabPos = F130 - F134;
                        }

                        switch (FTabType)
                        {
                            case ThumbStyle.Icon:
                                var iliDec = FILIDec[FILIndexDecUp];
                                var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + iliDec.Height + TabPos, FWinWidth, F134);
                                FILITab.DrawILT(FILIndexTab, rect, ref SurfDesc);
                                break;
                            case ThumbStyle.Bar:
                                var iliDec2 = FILIDec[FILIndexDecUp];
                                var rect3 = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + iliDec2.Height + TabPos, FWinWidth, F134);
                                FILLTab.DrawILLv(FILIndexTab, rect3, ref SurfDesc);
                                break;
                        }

                        break;
                    case Style.Slider:
                        var rect4 = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                        FILLBack.DrawILLv(FILIndexBack, rect4, ref SurfDesc);
                        TabPos = F130 * FPosition / F14C;
                        if (F134 + TabPos > F130)
                        {
                            TabPos = F130 - F134;
                        }

                        switch (FTabType)
                        {
                            case ThumbStyle.Icon:
                                var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + TabPos, FWinWidth, F134);
                                FILITab.DrawILT(FILIndexTab, rect, ref SurfDesc);
                                break;
                            case ThumbStyle.Bar:
                                var rect3 = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + TabPos, FWinWidth, F134);
                                FILLTab.DrawILLv(FILIndexTab, rect3, ref SurfDesc);
                                break;
                        }

                        break;
                    case Style.IncDec:
                        if (F151 == 4 && F150 > 0)
                        {
                            var iliIncHeight = FILIInc[FILIndexIncDown];
                            var iliInc = FILIInc[FILIndexIncDown];
                            var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + FWinHeight - iliInc.Height, FWinWidth, iliIncHeight.Height);
                            FILIInc.DrawILT(FILIndexIncDown, rect, ref SurfDesc);
                        }
                        else
                        {
                            var iliInc = FILIInc[FILIndexIncUp];
                            var iliIncHeight = FILIInc[FILIndexIncDown];
                            var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop + FWinHeight - iliIncHeight.Height, FWinWidth, iliInc.Height);
                            FILIInc.DrawILT(FILIndexIncUp, rect, ref SurfDesc);
                        }

                        if (F151 == 1 && F150 > 0)
                        {
                            var iliDec = FILIDec[FILIndexDecDown];
                            var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, iliDec.Height);
                            FILIDec.DrawILT(FILIndexDecDown, rect, ref SurfDesc);
                        }
                        else
                        {
                            var iliDec = FILIDec[FILIndexDecUp];
                            var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, iliDec.Height);
                            FILIDec.DrawILT(FILIndexDecUp, rect, ref SurfDesc);
                        }

                        break;
                }

                if (FOnDraw != null)
                {
                    FOnDraw(this, SurfDesc);
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
    public override void CheckMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        int TabPos = default;
        base.CheckMouseDown(Button, Shift, X, Y);
        FHandled = false;
        if (FVisible == true && X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (Button == MouseButton.Left)
            {
                F150 = 1;
                F151 = 5;
                TabPos = F130 * FPosition / F14C;
                if (TabPos + F134 > F130)
                {
                    TabPos = F130 - F134;
                }

                if (FScrollType != Style.Slider)
                {
                    var iliDec = FILIDec[FILIndexDecUp];
                    if (Y < FAOWWindow.FWinTop + FWinTop + iliDec.Height - 1)
                    {
                        F151 = 1;
                        ApplySmallStep(true);
                    }

                    var iliInc = FILIInc[FILIndexIncUp];
                    if (Y > FAOWWindow.FWinTop + FWinTop + FWinHeight - iliInc.Height)
                    {
                        F151 = 4;
                        ApplySmallStep(false);
                    }
                }

                if (F151 == 5)
                {
                    switch (FScrollType)
                    {
                        case Style.Scroll:
                            var iliDecHeight = FILIDec[FILIndexDecUp];
                            bool condition = Y > FAOWWindow.FWinTop + FWinTop + iliDecHeight.Height;
                            if (condition)
                            {
                                var iliInc = FILIInc[FILIndexIncUp];
                                condition = Y < FAOWWindow.FWinTop + FWinTop + TabPos + iliInc.Height;
                            }

                            if (condition)
                            {
                                var iliDec = FILIDec[FILIndexDecUp];
                                F154 = (Y - FAOWWindow.FWinTop - FWinTop - iliDec.Height) * F14C / F130 + FMin;
                                F151 = 2;
                                FPosition = FPosition - FLargeChange;
                                if (FMin > FPosition)
                                {
                                    FPosition = FMin;
                                }

                                FDrawn = false;
                                if (FOnChange != null)
                                {
                                    FOnChange(this);
                                }
                            }

                            var iliDec2 = FILIDec[FILIndexDecUp];
                            bool test = Y > FAOWWindow.FWinTop + FWinTop + iliDec2.Height + TabPos + F134;
                            if (test)
                            {
                                var iliInc = FILIInc[FILIndexIncUp];
                                test = Y < FAOWWindow.FWinTop + FWinTop + FWinHeight - iliInc.Height;
                            }

                            if (test)
                            {
                                var iliDec = FILIDec[FILIndexDecUp];
                                F154 = (Y - FAOWWindow.FWinTop - FWinTop - iliDec.Height) * F14C / F130 + FMin;
                                F151 = 3;
                                FPosition = FPosition + FLargeChange;
                                if (FMax + 1 - FNumberVisible < FPosition)
                                {
                                    FPosition = FMax + 1 - FNumberVisible;
                                }

                                FDrawn = false;
                                if (FOnChange != null)
                                {
                                    FOnChange(this);
                                }
                            }

                            if (F151 == 5)
                            {
                                F154 = Y - TabPos;
                            }

                            break;
                        case Style.Slider:
                            if (Y > FAOWWindow.FWinTop + FWinTop && Y < FAOWWindow.FWinTop + FWinTop + TabPos)
                            {
                                F154 = (Y - FAOWWindow.FWinTop - FWinTop) * F14C / F130 + FMin;
                                F151 = 2;
                                FPosition = FPosition - FLargeChange;
                                if (FMin > FPosition)
                                {
                                    FPosition = FMin;
                                }

                                FDrawn = false;
                                if (FOnChange != null)
                                {
                                    FOnChange(this);
                                }
                            }

                            if (Y > FAOWWindow.FWinTop + FWinTop + TabPos + F134 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
                            {
                                F154 = (Y - FAOWWindow.FWinTop - FWinTop) * F14C / F130 + FMin;
                                F151 = 3;
                                FPosition = FPosition + FLargeChange;
                                if (FMax + 1 - FNumberVisible < FPosition)
                                {
                                    FPosition = FMax + 1 - FNumberVisible;
                                }

                                FDrawn = false;
                                if (FOnChange != null)
                                {
                                    FOnChange(this);
                                }
                            }

                            if (F151 == 5)
                            {
                                F154 = Y - TabPos;
                            }

                            break;
                    }
                }
            }

            if (FOnMouseDown != null)
            {
                FOnMouseDown(this, Button, Shift, X, Y);
            }
        }
    }

    // AoWVScroll.pas:661
    public override void CheckMouseUp(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        FHandled = false;
        if (F151 != 0)
        {
            FDrawn = false;
        }

        F150 = 0;
        F151 = 0;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (FOnMouseUp != null)
            {
                FOnMouseUp(this, Button, Shift, X, Y);
            }
        }
    }

    // AoWVScroll.pas:680
    // { CheckMouseMove drags the tab }
    public override void CheckMouseMove(Set<ShiftState> Shift, int X, int Y)
    {
        base.CheckMouseMove(Shift, X, Y);
        FHandled = false;
        if (F151 == 5 && F150 > 0)
        {
            FPosition = (Y - F154) * F14C / F130 + FMin;
            if (FPosition < FMin)
            {
                FPosition = FMin;
            }

            if (FMax + 1 - FNumberVisible < FPosition)
            {
                FPosition = FMax + 1 - FNumberVisible;
            }

            FDrawn = false;
            if (FOnChange != null)
            {
                FOnChange(this);
            }
        }

        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (FOnMouseMove != null)
            {
                FOnMouseMove(this, Shift, X, Y);
            }
        }
    }

    public new NotifyEvent<VerticalScrollBar> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<VerticalScrollBar> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<VerticalScrollBar> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<VerticalScrollBar> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<VerticalScrollBar> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<VerticalScrollBar> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<VerticalScrollBar> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<VerticalScrollBar> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<VerticalScrollBar> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}
