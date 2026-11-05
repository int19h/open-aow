// Translated from AoWHScroll.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.AoWVScroll;

namespace Aow.Units.AoWHScroll
{
    // { The AoWHScroll unit of aowInt.dpl: TAOWHScrollBar, the horizontal scroll bar (TAOWScrollBar of
//   AoWVScroll with the decrement button at the left and the increment button at the right). }
    // no fields of its own
    public partial class TAOWHScrollBar : TAOWScrollBar
    {
        public TAOWHScrollBar(TComponent owner) : base(owner)
        { }

        // AoWHScroll.pas:31
        // { SetSize: as TAOWVScrollBar.SetSize across; the track length is not kept above zero here }
        public override void SetSize()
        {
            F14C = FMax - FMin + 1;
            if (F14C < FNumberVisible)
            {
                FNumberVisible = F14C;
            }

            switch (FScrollType)
            {
                case TaowScrollType.stScroll:
                    FWinHeight = FILTBack[FILIndexBack].Height;
                    var iliDecWidth = FILIDec[FILIndexDecUp];
                    var iliIncWidth = FILIInc[FILIndexIncUp];
                    if (iliDecWidth.Width + iliIncWidth.Width > FWinWidth)
                    {
                        var iliDec = FILIDec[FILIndexDecUp];
                        var iliInc = FILIInc[FILIndexIncUp];
                        FWinWidth = iliDec.Width + iliInc.Width;
                    }

                    var iliDec2 = FILIDec[FILIndexDecUp];
                    var iliInc2 = FILIInc[FILIndexIncUp];
                    F130 = FWinWidth - iliDec2.Width - iliInc2.Width;
                    F134 = F130 * FNumberVisible / F14C;
                    switch (FTabType)
                    {
                        case TaowTabType.ttIcon:
                            F134 = FILITab[FILIndexTab].Width;
                            break;
                        case TaowTabType.ttBar:
                            var illTabWidth = FILLTab[FILIndexTab];
                            var illTab = FILLTab[FILIndexTab + 2];
                            if (F134 < illTabWidth.Width + illTab.Width)
                            {
                                var illTab2 = FILLTab[FILIndexTab];
                                var illTab3 = FILLTab[FILIndexTab + 2];
                                F134 = illTab2.Width + illTab3.Width;
                            }

                            break;
                    }

                    break;
                case TaowScrollType.stSlider:
                    FWinHeight = FILLBack[FILIndexBack].Height;
                    F130 = FWinWidth;
                    F134 = F130 * FNumberVisible / F14C;
                    switch (FTabType)
                    {
                        case TaowTabType.ttIcon:
                            F134 = FILITab[FILIndexTab].Width;
                            break;
                        case TaowTabType.ttBar:
                            var illTabWidth = FILLTab[FILIndexTab];
                            var illTab = FILLTab[FILIndexTab + 2];
                            if (F134 < illTabWidth.Width + illTab.Width)
                            {
                                var illTab2 = FILLTab[FILIndexTab];
                                var illTab3 = FILLTab[FILIndexTab + 2];
                                F134 = illTab2.Width + illTab3.Width;
                            }

                            break;
                    }

                    break;
                case TaowScrollType.stIncDec:
                    FWinHeight = FILIDec[FILIndexDecUp].Height;
                    var iliDec3 = FILIDec[FILIndexDecUp];
                    var iliInc3 = FILIInc[FILIndexIncUp];
                    FWinWidth = iliDec3.Width + iliInc3.Width;
                    break;
            }

            if (FPosition + FNumberVisible > F14C)
            {
                FPosition = F14C - FNumberVisible;
            }

            base.SetSize();
        }

        // AoWHScroll.pas:79
        // { Draw: as TAOWVScrollBar.Draw across. The buttons of an increment/decrement pair keep parts of
//   the vertical code: the increment button is drawn left of the control and below it, and the
//   decrement button is FWinWidth wide and as high as its image (original) }
        public override void Draw(TSurfaceDesc SurfDesc)
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
                        case TaowScrollType.stScroll:
                            if (F151 == 4 && F150 > 0)
                            {
                                var iliIncWidth = FILIInc[FILIndexIncDown];
                                var iliInc = FILIInc[FILIndexIncDown];
                                var rect = Rect(FAOWWindow.FWinLeft + FWinLeft + FWinWidth - iliInc.Width, FAOWWindow.FWinTop + FWinTop, iliIncWidth.Width, FWinHeight);
                                FILIInc.DrawILT(FILIndexIncDown, rect, ref SurfDesc);
                            }
                            else
                            {
                                var iliInc = FILIInc[FILIndexIncUp];
                                var iliIncWidth = FILIInc[FILIndexIncDown];
                                var rect = Rect(FAOWWindow.FWinLeft + FWinLeft + FWinWidth - iliIncWidth.Width, FAOWWindow.FWinTop + FWinTop, iliInc.Width, FWinHeight);
                                FILIInc.DrawILT(FILIndexIncUp, rect, ref SurfDesc);
                            }

                            if (F151 == 1 && F150 > 0)
                            {
                                var iliDec = FILIDec[FILIndexDecDown];
                                var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, iliDec.Width, FWinHeight);
                                FILIDec.DrawILT(FILIndexDecDown, rect, ref SurfDesc);
                            }
                            else
                            {
                                var iliDec = FILIDec[FILIndexDecUp];
                                var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, iliDec.Width, FWinHeight);
                                FILIDec.DrawILT(FILIndexDecUp, rect, ref SurfDesc);
                            }

                            var iltBack = FILTBack[FILIndexBack];
                            var iliDecWidth = FILIDec[FILIndexDecUp];
                            var rect2 = Rect(FAOWWindow.FWinLeft + FWinLeft + iliDecWidth.Width, FAOWWindow.FWinTop + FWinTop, F130, iltBack.Height);
                            FILTBack.DrawILT(FILIndexBack, rect2, ref SurfDesc);
                            TabPos = F130 * FPosition / F14C;
                            if (F134 + TabPos > F130)
                            {
                                TabPos = F130 - F134;
                            }

                            switch (FTabType)
                            {
                                case TaowTabType.ttIcon:
                                    var iliDec = FILIDec[FILIndexDecUp];
                                    var rect = Rect(FAOWWindow.FWinLeft + FWinLeft + iliDec.Width + TabPos, FAOWWindow.FWinTop + FWinTop, F134, FWinHeight);
                                    FILITab.DrawILT(FILIndexTab, rect, ref SurfDesc);
                                    break;
                                case TaowTabType.ttBar:
                                    var iliDec2 = FILIDec[FILIndexDecUp];
                                    var rect3 = Rect(FAOWWindow.FWinLeft + FWinLeft + iliDec2.Width + TabPos, FAOWWindow.FWinTop + FWinTop, F134, FWinHeight);
                                    FILLTab.DrawILLh(FILIndexTab, rect3, ref SurfDesc);
                                    break;
                            }

                            break;
                        case TaowScrollType.stSlider:
                            var rect4 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                            FILLBack.DrawILLh(FILIndexBack, rect4, ref SurfDesc);
                            TabPos = F130 * FPosition / F14C;
                            if (F134 + TabPos > F130)
                            {
                                TabPos = F130 - F134;
                            }

                            switch (FTabType)
                            {
                                case TaowTabType.ttIcon:
                                    var rect = Rect(FAOWWindow.FWinLeft + FWinLeft + TabPos, FAOWWindow.FWinTop + FWinTop, F134, FWinHeight);
                                    FILITab.DrawILT(FILIndexTab, rect, ref SurfDesc);
                                    break;
                                case TaowTabType.ttBar:
                                    var rect3 = Rect(FAOWWindow.FWinLeft + FWinLeft + TabPos, FAOWWindow.FWinTop + FWinTop, F134, FWinHeight);
                                    FILLTab.DrawILLh(FILIndexTab, rect3, ref SurfDesc);
                                    break;
                            }

                            break;
                        case TaowScrollType.stIncDec:
                            if (F151 == 4 && F150 > 0)
                            {
                                var iliIncWidth = FILIInc[FILIndexIncDown];
                                var iliInc = FILIInc[FILIndexIncDown];
                                var rect = Rect(FAOWWindow.FWinLeft + FWinLeft - iliInc.Width, FAOWWindow.FWinTop + FWinTop + FWinHeight, iliIncWidth.Width, FWinHeight);
                                FILIInc.DrawILT(FILIndexIncDown, rect, ref SurfDesc);
                            }
                            else
                            {
                                var iliIncWidth = FILIInc[FILIndexIncUp];
                                var iliInc = FILIInc[FILIndexIncUp];
                                var rect = Rect(FAOWWindow.FWinLeft + FWinLeft - iliInc.Width, FAOWWindow.FWinTop + FWinTop + FWinHeight, iliIncWidth.Width, FWinHeight);
                                FILIInc.DrawILT(FILIndexIncUp, rect, ref SurfDesc);
                            }

                            if (F151 == 1 && F150 > 0)
                            {
                                var iliDec = FILIDec[FILIndexDecDown];
                                var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, iliDec.Height);
                                FILIDec.DrawILT(FILIndexDecDown, rect, ref SurfDesc);
                            }
                            else
                            {
                                var iliDec = FILIDec[FILIndexDecUp];
                                var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, iliDec.Height);
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
            catch (System.Exception)
            {
                MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
            }
        }

        // AoWHScroll.pas:171
        // { CheckMouseDown: as TAOWVScrollBar.CheckMouseDown across, but OnChange is called once at the end
//   when the position changed. The track after the tab ends at the width of the decrement button
//   (original) }
        public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            int TabPos = default;
            int OldPos;
            base.CheckMouseDown(Button, Shift, X, Y);
            FHandled = false;
            if (FVisible == true && X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
            {
                OldPos = FPosition;
                FHandled = true;
                if (Button == TMouseButton.mbLeft)
                {
                    F150 = 1;
                    F151 = 5;
                    TabPos = F130 * FPosition / F14C;
                    if (TabPos + F134 > F130)
                    {
                        TabPos = F130 - F134;
                    }

                    if (FScrollType != TaowScrollType.stSlider)
                    {
                        var iliDec = FILIDec[FILIndexDecUp];
                        if (X < FAOWWindow.FWinLeft + FWinLeft + iliDec.Width - 1)
                        {
                            F151 = 1;
                            FPosition = FPosition - FSmallChange;
                            if (FMin > FPosition)
                            {
                                FPosition = FMin;
                            }
                        }

                        var iliInc = FILIInc[FILIndexIncUp];
                        if (X > FAOWWindow.FWinLeft + FWinLeft + FWinWidth - iliInc.Width)
                        {
                            F151 = 4;
                            FPosition = FPosition + FSmallChange;
                            if (FMax + 1 - FNumberVisible < FPosition)
                            {
                                FPosition = FMax + 1 - FNumberVisible;
                            }
                        }
                    }

                    if (F151 == 5)
                    {
                        switch (FScrollType)
                        {
                            case TaowScrollType.stScroll:
                                var iliDecWidth = FILIDec[FILIndexDecUp];
                                bool condition = X > FAOWWindow.FWinLeft + FWinLeft + iliDecWidth.Width;
                                if (condition)
                                {
                                    var iliInc = FILIInc[FILIndexIncUp];
                                    condition = X < FAOWWindow.FWinLeft + FWinLeft + TabPos + iliInc.Width;
                                }

                                if (condition)
                                {
                                    var iliDec = FILIDec[FILIndexDecUp];
                                    F154 = (X - FAOWWindow.FWinLeft - FWinLeft - iliDec.Width) * F14C / F130 + FMin;
                                    F151 = 2;
                                    FPosition = FPosition - FLargeChange;
                                    if (FMin > FPosition)
                                    {
                                        FPosition = FMin;
                                    }
                                }

                                var iliDec2 = FILIDec[FILIndexDecUp];
                                bool test = X > FAOWWindow.FWinLeft + FWinLeft + iliDec2.Width + TabPos + F134;
                                if (test)
                                {
                                    var iliDec = FILIDec[FILIndexDecUp];
                                    test = X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth - iliDec.Width;
                                }

                                if (test)
                                {
                                    var iliDec = FILIDec[FILIndexDecUp];
                                    F154 = (X - FAOWWindow.FWinLeft - FWinLeft - iliDec.Width) * F14C / F130 + FMin;
                                    F151 = 3;
                                    FPosition = FPosition + FLargeChange;
                                    if (FMax + 1 - FNumberVisible < FPosition)
                                    {
                                        FPosition = FMax + 1 - FNumberVisible;
                                    }
                                }

                                if (F151 == 5)
                                {
                                    F154 = X - TabPos;
                                }

                                break;
                            case TaowScrollType.stSlider:
                                if (X > FAOWWindow.FWinLeft + FWinLeft && X < FAOWWindow.FWinLeft + FWinLeft + TabPos)
                                {
                                    F154 = (X - FAOWWindow.FWinLeft - FWinLeft) * F14C / F130 + FMin;
                                    F151 = 2;
                                    FPosition = FPosition - FLargeChange;
                                    if (FMin > FPosition)
                                    {
                                        FPosition = FMin;
                                    }
                                }

                                if (X > FAOWWindow.FWinLeft + FWinLeft + TabPos + F134 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth)
                                {
                                    F154 = (X - FAOWWindow.FWinLeft - FWinLeft) * F14C / F130 + FMin;
                                    F151 = 3;
                                    FPosition = FPosition + FLargeChange;
                                    if (FMax + 1 - FNumberVisible < FPosition)
                                    {
                                        FPosition = FMax + 1 - FNumberVisible;
                                    }
                                }

                                if (F151 == 5)
                                {
                                    F154 = X - TabPos;
                                }

                                break;
                        }
                    }
                }

                if (OldPos != FPosition)
                {
                    FDrawn = false;
                    if (FOnChange != null)
                    {
                        FOnChange(this);
                    }
                }

                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }
            }
        }

        // AoWHScroll.pas:273
        public override void CheckMouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            if (F151 == 1 || F151 == 4)
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

        // AoWHScroll.pas:291
        public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseMove(Shift, X, Y);
            FHandled = false;
            if (F151 == 5 && F150 > 0)
            {
                FPosition = (X - F154) * F14C / F130 + FMin;
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
    }

    public static partial class AoWHScroll
    {
        // AoWHScroll.pas:317
        public static void Register()
        {
            RegisterComponents("AOW Interface", [typeof(TAOWHScrollBar)]);
        }
    }
}
