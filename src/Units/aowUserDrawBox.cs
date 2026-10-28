// Translated from aowUserDrawBox.pas.
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
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;

namespace Aow.Units.aowUserDrawBox
{
    // { The aowUserDrawBox unit of aowInt.dpl: TaowUserDrawBox, an empty control that its owner draws
//   in OnPreDraw and OnDraw; its mouse events get coordinates relative to the control. }
    // { aowUserDrawBox.TaowUserDrawBox (instance size 276): no fields of its own }
    public partial class TaowUserDrawBox : Aow.Units.AoWControl.TAOWControl
    {
        // aowUserDrawBox.pas:34
        // { TaowUserDrawBox }
        public override void Validate()
        {
            FValid = true;
            base.Validate();
        }

        // aowUserDrawBox.pas:40
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

        // aowUserDrawBox.pas:55
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

        // aowUserDrawBox.pas:69
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

        // aowUserDrawBox.pas:81
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

        // aowUserDrawBox.pas:94
        public override void CheckKeyPress(ref byte Key)
        {
            FHandled = false;
        }
    }

    public static partial class aowUserDrawBox
    {
        // aowUserDrawBox.pas:99
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Interface", [typeof(Aow.Units.aowUserDrawBox.TaowUserDrawBox)]);
        }
    }
}
