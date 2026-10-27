// Generated from AoWLabel.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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

namespace Aow.Units.AoWLabel
{
    // { The AoWLabel unit of aowInt.dpl: TAOWLabel, a line of text in an image library font; it
//   sizes itself to its text. }
    // { AoWLabel.TAOWLabel (instance size 276): no fields of its own }
    public partial class TAOWLabel : Aow.Units.AoWControl.TAOWControl
    {
        // AoWLabel.pas:38
        // { TAOWLabel }
        public override void Validate()
        {
            if (FAOWFont != null)
            {
                FValid = true;
            }

            FMasked = true;
            base.Validate();
        }

        // AoWLabel.pas:46
        public override void SetSize()
        {
            var call__1 = FAOWFont.GetTextWidth(FText);
            FWinWidth = call__1;
            var call__2 = FAOWFont.GetTextHeight(FText);
            FWinHeight = call__2;
            base.SetSize();
        }

        // AoWLabel.pas:72
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            try
            {
                if (FValid && FVisible)
                {
                    if (FOnPreDraw != null)
                    {
                        FOnPreDraw(this, SurfDesc);
                    }

                    FAOWFont.Color = FColor;
                    FAOWFont.SetText(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FText, SurfDesc);
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

        // AoWLabel.pas:90
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseDown(Button, Shift, X, Y);
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = FCaptureInput;
                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }
            }
        }

        // AoWLabel.pas:103
        public override void CheckMouseUp(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = FCaptureInput;
                if (FOnMouseUp != null)
                {
                    FOnMouseUp(this, Button, Shift, X, Y);
                }
            }
        }

        // AoWLabel.pas:115
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseMove(Shift, X, Y);
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = FCaptureInput;
                if (FOnMouseMove != null)
                {
                    FOnMouseMove(this, Shift, X, Y);
                }
            }
        }

        // AoWLabel.pas:128
        public override void CheckKeyPress(ref byte Key)
        {
            FHandled = false;
        }

        // AoWLabel.pas:53
        public void SetGFont(Aow.Units.ImageLib.TImageLibraryFont Value)
        {
            if (Value != FAOWFont)
            {
                FAOWFont = Value;
                FValid = false;
            }
        }

        // AoWLabel.pas:62
        public void SetGText(Aow.Rtl.AnsiString Value)
        {
            if (FText != Value)
            {
                FText = Value;
                FSized = false;
                FAligned = false;
            }
        }

        [Published]
        public Aow.Units.ImageLib.TImageLibraryFont AOWFont { get => FAOWFont; set => SetGFont(value); }

        [Published]
        public Aow.Rtl.AnsiString Text { get => FText; set => SetGText(value); }
    }

    public static partial class AoWLabel
    {
        // AoWLabel.pas:133
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Interface", [typeof(Aow.Units.AoWLabel.TAOWLabel)]);
        }
    }
}
