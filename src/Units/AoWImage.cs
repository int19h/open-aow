// Translated from AoWImage.pas.
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
using Aow.Units.AoWWinGeneric;
using static Aow.Units.AoWWinGeneric.AoWWinGeneric;

namespace Aow.Units.AoWImage
{
    // { The AoWImage unit of aowInt.dpl: TAOWImage, a control that shows one image of an image
//   library as a texture filling its rectangle, or plays the images AnimStart..AnimEnd as an
//   animation, one image every AnimFrameDelay updates. Its mouse events get coordinates relative
//   to the control. }
    // { AoWImage.TAOWImage (instance size 304). Fields: +$114 FAnimate, +$118 FAnimFrameDelay, +$11C
//     FFrameCount (updates left until the next image), +$120 FAnimStart, +$124 FCurrentImage, +$128
//     FAnimEnd, +$12C FImageLib }
    public partial class TAOWImage : Aow.Units.AoWControl.TAOWControl
    {
        public bool FAnimate;
        public int FAnimFrameDelay;
        public int FFrameCount;
        public int FAnimStart;
        public int FCurrentImage;
        public int FAnimEnd;
        // AoWImage.pas:79
        public override void SetSize()
        {
            if (FWinWidth == 0)
            {
                var call__1 = FImageLib[FCurrentImage];
                FWinWidth = call__1.Width;
            }

            if (FWinHeight == 0)
            {
                var call__2 = FImageLib[FCurrentImage];
                FWinHeight = call__2.Height;
            }

            base.SetSize();
        }

        // AoWImage.pas:145
        public override void Update()
        {
            int I = default;
            if (FVisible && (FAnimate == true))
            {
                if (FFrameCount > 0)
                {
                    FFrameCount -= 1;
                }
                else
                {
                    FFrameCount = FAnimFrameDelay;
                    I = FCurrentImage + 1;
                    if (I > FAnimEnd)
                    {
                        I = FAnimStart;
                    }

                    SetCurrent(I);
                }
            }

            base.Update();
        }

        // AoWImage.pas:163
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseDown(Button, Shift, X, Y);
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = FCaptureInput;
                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, (X - FAOWWindow.FWinLeft) - FWinLeft, (Y - FAOWWindow.FWinTop) - FWinTop);
                }
            }
        }

        // AoWImage.pas:177
        public override void CheckMouseUp(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            FRedraw = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = FCaptureInput;
                if (FOnMouseUp != null)
                {
                    FOnMouseUp(this, Button, Shift, X, Y);
                }
            }
        }

        // AoWImage.pas:190
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseMove(Shift, X, Y);
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = FCaptureInput;
                if (FOnMouseMove != null)
                {
                    FOnMouseMove(this, Shift, (X - FAOWWindow.FWinLeft) - FWinLeft, (Y - FAOWWindow.FWinTop) - FWinTop);
                }
            }
        }

        // AoWImage.pas:203
        public override void CheckKeyPress(ref byte Key)
        {
            FHandled = false;
        }

        // AoWImage.pas:88
        public void SetAnim(bool Value)
        {
            if (Value != FAnimate)
            {
                FAnimate = Value;
                FValid = false;
            }
        }

        // AoWImage.pas:97
        public void SetStart(int Value)
        {
            if (Value < 0)
            {
                Value = 0;
            }

            if (Value != FAnimStart)
            {
                FAnimStart = Value;
                FValid = false;
            }
        }

        // AoWImage.pas:108
        public void SetCurrent(int Value)
        {
            if (Value < FAnimStart)
            {
                Value = FAnimStart;
            }

            if (Value != FCurrentImage)
            {
                FCurrentImage = Value;
                FDrawn = false;
            }
        }

        // AoWImage.pas:119
        public void SetEnd(int Value)
        {
            if (Value != FAnimEnd)
            {
                FAnimEnd = Value;
                FValid = false;
            }
        }

        [Published]
        public int AnimStart { get => FAnimStart; set => SetStart(value); }

        [Published]
        public int CurrentImage { get => FCurrentImage; set => SetCurrent(value); }

        [Published]
        public int AnimEnd { get => FAnimEnd; set => SetEnd(value); }

        [Published]
        public bool Animate { get => FAnimate; set => SetAnim(value); }

        [Published]
        public int AnimFrameDelay { get => FAnimFrameDelay; set => FAnimFrameDelay = value; }
    }

    public static partial class AoWImage
    {
        // AoWImage.pas:208
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Interface", [typeof(Aow.Units.AoWImage.TAOWImage)]);
        }
    }
}
