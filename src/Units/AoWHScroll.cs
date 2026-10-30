// Translated from AoWHScroll.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AOWToolTip;
using static Aow.Units.AOWToolTip.AOWToolTip;
using Aow.Units.AoWControl;
using static Aow.Units.AoWControl.AoWControl;
using Aow.Units.AOWWinManager;
using static Aow.Units.AOWWinManager.AOWWinManager;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AOWInterfaceImgLib;
using static Aow.Units.AOWInterfaceImgLib.AOWInterfaceImgLib;
using Aow.Units.AoWWinControl;
using static Aow.Units.AoWWinControl.AoWWinControl;
using Aow.Units.AoWBaseWin;
using static Aow.Units.AoWBaseWin.AoWBaseWin;

namespace Aow.Units.AoWHScroll
{
    // { The AoWHScroll unit of aowInt.dpl: TAOWHScrollBar, the horizontal scroll bar (TAOWScrollBar of
//   AoWVScroll with the decrement button at the left and the increment button at the right). }
    // { AoWHScroll.TAOWHScrollBar (instance size 380): no fields of its own }
    public partial class TAOWHScrollBar : Aow.Units.AoWVScroll.TAOWScrollBar
    {
        public TAOWHScrollBar(Aow.Rtl.TComponent AOwner) : base(AOwner) { }

        // AoWHScroll.pas:31
        // { TAOWHScrollBar }
        // { SetSize: as TAOWVScrollBar.SetSize across; the track length is not kept above zero here }
        public override void SetSize()
        {
            F14C = (FMax - FMin) + 1;
            if (F14C < FNumberVisible)
            {
                FNumberVisible = F14C;
            }

            switch (FScrollType)
            {
                case Aow.Units.AoWVScroll.TaowScrollType.stScroll:
                    var call__1 = FILTBack[FILIndexBack];
                    FWinHeight = call__1.Height;
                    var call__2 = FILIDec[FILIndexDecUp];
                    var call__3 = FILIInc[FILIndexIncUp];
                    if ((call__2.Width + call__3.Width) > FWinWidth)
                    {
                        var call__4 = FILIDec[FILIndexDecUp];
                        var call__5 = FILIInc[FILIndexIncUp];
                        FWinWidth = call__4.Width + call__5.Width;
                    }

                    var call__6 = FILIDec[FILIndexDecUp];
                    var call__7 = FILIInc[FILIndexIncUp];
                    F130 = (FWinWidth - call__6.Width) - call__7.Width;
                    F134 = (F130 * FNumberVisible) / F14C;
                    switch (FTabType)
                    {
                        case Aow.Units.AoWVScroll.TaowTabType.ttIcon:
                            var call__8 = FILITab[FILIndexTab];
                            F134 = call__8.Width;
                            break;
                        case Aow.Units.AoWVScroll.TaowTabType.ttBar:
                            var call__9 = FILLTab[FILIndexTab];
                            var call__10 = FILLTab[FILIndexTab + 2];
                            if (F134 < (call__9.Width + call__10.Width))
                            {
                                var call__11 = FILLTab[FILIndexTab];
                                var call__12 = FILLTab[FILIndexTab + 2];
                                F134 = call__11.Width + call__12.Width;
                            }

                            break;
                    }

                    break;
                case Aow.Units.AoWVScroll.TaowScrollType.stSlider:
                    var call__13 = FILLBack[FILIndexBack];
                    FWinHeight = call__13.Height;
                    F130 = FWinWidth;
                    F134 = (F130 * FNumberVisible) / F14C;
                    switch (FTabType)
                    {
                        case Aow.Units.AoWVScroll.TaowTabType.ttIcon:
                            var call__14 = FILITab[FILIndexTab];
                            F134 = call__14.Width;
                            break;
                        case Aow.Units.AoWVScroll.TaowTabType.ttBar:
                            var call__15 = FILLTab[FILIndexTab];
                            var call__16 = FILLTab[FILIndexTab + 2];
                            if (F134 < (call__15.Width + call__16.Width))
                            {
                                var call__17 = FILLTab[FILIndexTab];
                                var call__18 = FILLTab[FILIndexTab + 2];
                                F134 = call__17.Width + call__18.Width;
                            }

                            break;
                    }

                    break;
                case Aow.Units.AoWVScroll.TaowScrollType.stIncDec:
                    var call__19 = FILIDec[FILIndexDecUp];
                    FWinHeight = call__19.Height;
                    var call__20 = FILIDec[FILIndexDecUp];
                    var call__21 = FILIInc[FILIndexIncUp];
                    FWinWidth = call__20.Width + call__21.Width;
                    break;
            }

            if ((FPosition + FNumberVisible) > F14C)
            {
                FPosition = F14C - FNumberVisible;
            }

            base.SetSize();
        }

        // AoWHScroll.pas:79
        // { Draw: as TAOWVScrollBar.Draw across. The buttons of an increment/decrement pair keep parts of
//   the vertical code: the increment button is drawn left of the control and below it, and the
//   decrement button is FWinWidth wide and as high as its image (original) }
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
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
                        case Aow.Units.AoWVScroll.TaowScrollType.stScroll:
                            if ((F151 == 4) && (F150 > 0))
                            {
                                var call__22 = FILIInc[FILIndexIncDown];
                                var call__23 = FILIInc[FILIndexIncDown];
                                var call__24 = Aow.Rtl.Rtl.Rect(((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth) - call__23.Width, FAOWWindow.FWinTop + FWinTop, call__22.Width, FWinHeight);
                                FILIInc.DrawILT(FILIndexIncDown, call__24, ref SurfDesc);
                            }
                            else
                            {
                                var call__25 = FILIInc[FILIndexIncUp];
                                var call__26 = FILIInc[FILIndexIncDown];
                                var call__27 = Aow.Rtl.Rtl.Rect(((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth) - call__26.Width, FAOWWindow.FWinTop + FWinTop, call__25.Width, FWinHeight);
                                FILIInc.DrawILT(FILIndexIncUp, call__27, ref SurfDesc);
                            }

                            if ((F151 == 1) && (F150 > 0))
                            {
                                var call__28 = FILIDec[FILIndexDecDown];
                                var call__29 = Aow.Rtl.Rtl.Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, call__28.Width, FWinHeight);
                                FILIDec.DrawILT(FILIndexDecDown, call__29, ref SurfDesc);
                            }
                            else
                            {
                                var call__30 = FILIDec[FILIndexDecUp];
                                var call__31 = Aow.Rtl.Rtl.Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, call__30.Width, FWinHeight);
                                FILIDec.DrawILT(FILIndexDecUp, call__31, ref SurfDesc);
                            }

                            var call__32 = FILTBack[FILIndexBack];
                            var call__33 = FILIDec[FILIndexDecUp];
                            var call__34 = Aow.Rtl.Rtl.Rect((FAOWWindow.FWinLeft + FWinLeft) + call__33.Width, FAOWWindow.FWinTop + FWinTop, F130, call__32.Height);
                            FILTBack.DrawILT(FILIndexBack, call__34, ref SurfDesc);
                            TabPos = (F130 * FPosition) / F14C;
                            if ((F134 + TabPos) > F130)
                            {
                                TabPos = F130 - F134;
                            }

                            switch (FTabType)
                            {
                                case Aow.Units.AoWVScroll.TaowTabType.ttIcon:
                                    var call__35 = FILIDec[FILIndexDecUp];
                                    var call__36 = Aow.Rtl.Rtl.Rect(((FAOWWindow.FWinLeft + FWinLeft) + call__35.Width) + TabPos, FAOWWindow.FWinTop + FWinTop, F134, FWinHeight);
                                    FILITab.DrawILT(FILIndexTab, call__36, ref SurfDesc);
                                    break;
                                case Aow.Units.AoWVScroll.TaowTabType.ttBar:
                                    var call__37 = FILIDec[FILIndexDecUp];
                                    var call__38 = Aow.Rtl.Rtl.Rect(((FAOWWindow.FWinLeft + FWinLeft) + call__37.Width) + TabPos, FAOWWindow.FWinTop + FWinTop, F134, FWinHeight);
                                    FILLTab.DrawILLh(FILIndexTab, call__38, ref SurfDesc);
                                    break;
                            }

                            break;
                        case Aow.Units.AoWVScroll.TaowScrollType.stSlider:
                            var call__39 = Aow.Rtl.Rtl.Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                            FILLBack.DrawILLh(FILIndexBack, call__39, ref SurfDesc);
                            TabPos = (F130 * FPosition) / F14C;
                            if ((F134 + TabPos) > F130)
                            {
                                TabPos = F130 - F134;
                            }

                            switch (FTabType)
                            {
                                case Aow.Units.AoWVScroll.TaowTabType.ttIcon:
                                    var call__40 = Aow.Rtl.Rtl.Rect((FAOWWindow.FWinLeft + FWinLeft) + TabPos, FAOWWindow.FWinTop + FWinTop, F134, FWinHeight);
                                    FILITab.DrawILT(FILIndexTab, call__40, ref SurfDesc);
                                    break;
                                case Aow.Units.AoWVScroll.TaowTabType.ttBar:
                                    var call__41 = Aow.Rtl.Rtl.Rect((FAOWWindow.FWinLeft + FWinLeft) + TabPos, FAOWWindow.FWinTop + FWinTop, F134, FWinHeight);
                                    FILLTab.DrawILLh(FILIndexTab, call__41, ref SurfDesc);
                                    break;
                            }

                            break;
                        case Aow.Units.AoWVScroll.TaowScrollType.stIncDec:
                            if ((F151 == 4) && (F150 > 0))
                            {
                                var call__42 = FILIInc[FILIndexIncDown];
                                var call__43 = FILIInc[FILIndexIncDown];
                                var call__44 = Aow.Rtl.Rtl.Rect((FAOWWindow.FWinLeft + FWinLeft) - call__43.Width, (FAOWWindow.FWinTop + FWinTop) + FWinHeight, call__42.Width, FWinHeight);
                                FILIInc.DrawILT(FILIndexIncDown, call__44, ref SurfDesc);
                            }
                            else
                            {
                                var call__45 = FILIInc[FILIndexIncUp];
                                var call__46 = FILIInc[FILIndexIncUp];
                                var call__47 = Aow.Rtl.Rtl.Rect((FAOWWindow.FWinLeft + FWinLeft) - call__46.Width, (FAOWWindow.FWinTop + FWinTop) + FWinHeight, call__45.Width, FWinHeight);
                                FILIInc.DrawILT(FILIndexIncUp, call__47, ref SurfDesc);
                            }

                            if ((F151 == 1) && (F150 > 0))
                            {
                                var call__48 = FILIDec[FILIndexDecDown];
                                var call__49 = Aow.Rtl.Rtl.Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, call__48.Height);
                                FILIDec.DrawILT(FILIndexDecDown, call__49, ref SurfDesc);
                            }
                            else
                            {
                                var call__50 = FILIDec[FILIndexDecUp];
                                var call__51 = Aow.Rtl.Rtl.Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, call__50.Height);
                                FILIDec.DrawILT(FILIndexDecUp, call__51, ref SurfDesc);
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
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Exception during Draw", ((AnsiString)"Error - ") + Name, (int)0);
            }
        }

        // AoWHScroll.pas:171
        // { CheckMouseDown: as TAOWVScrollBar.CheckMouseDown across, but OnChange is called once at the end
//   when the position changed. The track after the tab ends at the width of the decrement button
//   (original) }
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int TabPos = default;
            int OldPos = default;
            base.CheckMouseDown(Button, Shift, X, Y);
            FHandled = false;
            if (((((FVisible == true) && (X > ((FAOWWindow.FWinLeft + FWinLeft) - 1))) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                OldPos = FPosition;
                FHandled = true;
                if (Button == Aow.Rtl.TMouseButton.mbLeft)
                {
                    F150 = 1;
                    F151 = 5;
                    TabPos = (F130 * FPosition) / F14C;
                    if ((TabPos + F134) > F130)
                    {
                        TabPos = F130 - F134;
                    }

                    if (FScrollType != Aow.Units.AoWVScroll.TaowScrollType.stSlider)
                    {
                        var call__52 = FILIDec[FILIndexDecUp];
                        if (X < (((FAOWWindow.FWinLeft + FWinLeft) + call__52.Width) - 1))
                        {
                            F151 = 1;
                            FPosition = FPosition - FSmallChange;
                            if (FMin > FPosition)
                            {
                                FPosition = FMin;
                            }
                        }

                        var call__53 = FILIInc[FILIndexIncUp];
                        if (X > (((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth) - call__53.Width))
                        {
                            F151 = 4;
                            FPosition = FPosition + FSmallChange;
                            if (((FMax + 1) - FNumberVisible) < FPosition)
                            {
                                FPosition = (FMax + 1) - FNumberVisible;
                            }
                        }
                    }

                    if (F151 == 5)
                    {
                        switch (FScrollType)
                        {
                            case Aow.Units.AoWVScroll.TaowScrollType.stScroll:
                                var call__54 = FILIDec[FILIndexDecUp];
                                bool bool__55 = (X > ((FAOWWindow.FWinLeft + FWinLeft) + call__54.Width));
                                if (bool__55)
                                {
                                    var call__56 = FILIInc[FILIndexIncUp];
                                    bool__55 = (X < (((FAOWWindow.FWinLeft + FWinLeft) + TabPos) + call__56.Width));
                                }

                                if (bool__55)
                                {
                                    var call__57 = FILIDec[FILIndexDecUp];
                                    F154 = (((((X - FAOWWindow.FWinLeft) - FWinLeft) - call__57.Width) * F14C) / F130) + FMin;
                                    F151 = 2;
                                    FPosition = FPosition - FLargeChange;
                                    if (FMin > FPosition)
                                    {
                                        FPosition = FMin;
                                    }
                                }

                                var call__58 = FILIDec[FILIndexDecUp];
                                bool bool__59 = (X > ((((FAOWWindow.FWinLeft + FWinLeft) + call__58.Width) + TabPos) + F134));
                                if (bool__59)
                                {
                                    var call__60 = FILIDec[FILIndexDecUp];
                                    bool__59 = (X < (((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth) - call__60.Width));
                                }

                                if (bool__59)
                                {
                                    var call__61 = FILIDec[FILIndexDecUp];
                                    F154 = (((((X - FAOWWindow.FWinLeft) - FWinLeft) - call__61.Width) * F14C) / F130) + FMin;
                                    F151 = 3;
                                    FPosition = FPosition + FLargeChange;
                                    if (((FMax + 1) - FNumberVisible) < FPosition)
                                    {
                                        FPosition = (FMax + 1) - FNumberVisible;
                                    }
                                }

                                if (F151 == 5)
                                {
                                    F154 = X - TabPos;
                                }

                                break;
                            case Aow.Units.AoWVScroll.TaowScrollType.stSlider:
                                if ((X > (FAOWWindow.FWinLeft + FWinLeft)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + TabPos)))
                                {
                                    F154 = ((((X - FAOWWindow.FWinLeft) - FWinLeft) * F14C) / F130) + FMin;
                                    F151 = 2;
                                    FPosition = FPosition - FLargeChange;
                                    if (FMin > FPosition)
                                    {
                                        FPosition = FMin;
                                    }
                                }

                                if ((X > (((FAOWWindow.FWinLeft + FWinLeft) + TabPos) + F134)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth)))
                                {
                                    F154 = ((((X - FAOWWindow.FWinLeft) - FWinLeft) * F14C) / F130) + FMin;
                                    F151 = 3;
                                    FPosition = FPosition + FLargeChange;
                                    if (((FMax + 1) - FNumberVisible) < FPosition)
                                    {
                                        FPosition = (FMax + 1) - FNumberVisible;
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
        public override void CheckMouseUp(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            if ((F151 == 1) || (F151 == 4))
            {
                FDrawn = false;
            }

            F150 = 0;
            F151 = 0;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = true;
                if (FOnMouseUp != null)
                {
                    FOnMouseUp(this, Button, Shift, X, Y);
                }
            }
        }

        // AoWHScroll.pas:291
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseMove(Shift, X, Y);
            FHandled = false;
            if ((F151 == 5) && (F150 > 0))
            {
                FPosition = (((X - F154) * F14C) / F130) + FMin;
                if (FPosition < FMin)
                {
                    FPosition = FMin;
                }

                if (((FMax + 1) - FNumberVisible) < FPosition)
                {
                    FPosition = (FMax + 1) - FNumberVisible;
                }

                FDrawn = false;
                if (FOnChange != null)
                {
                    FOnChange(this);
                }
            }

            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
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
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Interface", [typeof(Aow.Units.AoWHScroll.TAOWHScrollBar)]);
        }
    }
}
