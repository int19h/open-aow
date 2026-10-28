// Translated from AoWCheckBox.pas.
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
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AOWMenuBar;
using static Aow.Units.AOWMenuBar.AOWMenuBar;
using Aow.Units.AOWMenu;
using static Aow.Units.AOWMenu.AOWMenu;
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

namespace Aow.Units.AoWCheckBox
{
    // { The AoWCheckBox unit of aowInt.dpl: TAOWCheckBox, a button that toggles its down state
//   (Checked) on a left click; with a RadioIndex above 0 it is a radio button, which only turns
//   on, and its window turns off the other radio buttons of the group (RadioChange). A right
//   click opens its PopupMenu at the mouse, kept inside the window manager. }
    // { AoWCheckBox.TAOWCheckBox (instance size 320). Fields: +$138 FOnDoubleClick.
//     VMT (new): SetChecked }
    public partial class TAOWCheckBox : Aow.Units.AoWButton.TAOWBaseButton
    {
        public Aow.Rtl.TNotifyEvent FOnDoubleClick;
        // AoWCheckBox.pas:51
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseDown(Button, Shift, X, Y);
            FHandled = false;
            FLit = false;
            if ((((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight))) && (FValid == true)) && (FVisible == true))
            {
                FHandled = true;
                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }

                if (Button == Aow.Rtl.TMouseButton.mbLeft)
                {
                    if (FRadioIndex > 0)
                    {
                        if (!FChecked)
                        {
                            FChecked = true;
                            if (FOnChange != null)
                            {
                                FOnChange(this);
                            }

                            FChecked = !FChecked;
                            Checked = !FChecked;
                        }
                    }
                    else
                    {
                        FChecked = !FChecked;
                        if (FOnChange != null)
                        {
                            FOnChange(this);
                        }

                        FChecked = !FChecked;
                        Checked = !FChecked;
                    }

                    if ((Shift & TSet<Aow.Rtl.TShiftState>.Of(Aow.Rtl.TShiftState.ssDouble)) == TSet<Aow.Rtl.TShiftState>.Of(Aow.Rtl.TShiftState.ssDouble))
                    {
                        if (FOnDoubleClick != null)
                        {
                            FOnDoubleClick(this);
                        }
                    }
                }
                else
                {
                    if ((Button == Aow.Rtl.TMouseButton.mbRight) && (FPopupMenu != null))
                    {
                        FPopupMenu.WinLeft = X;
                        FPopupMenu.WinTop = Y;
                        FPopupMenu.SetPopupOwner(this);
                        if (FPopupMenu.FWinManager != null)
                        {
                            if ((FPopupMenu.FWinLeft + FPopupMenu.FWinWidth) > FPopupMenu.FWinManager.FWinWidth)
                            {
                                FPopupMenu.FWinLeft = X - FPopupMenu.FWinWidth;
                            }

                            if ((FPopupMenu.FWinTop + FPopupMenu.FWinHeight) > FPopupMenu.FWinManager.FWinHeight)
                            {
                                FPopupMenu.FWinTop = Y - FPopupMenu.FWinHeight;
                            }
                        }

                        FPopupMenu.Visible = true;
                    }
                }
            }
        }

        // AoWCheckBox.pas:105
        public override void CheckMouseUp(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            FLit = false;
            if ((((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight))) && (FValid == true)) && (FVisible == true))
            {
                FHandled = true;
                if (FOnMouseUp != null)
                {
                    FOnMouseUp(this, Button, Shift, X, Y);
                }
            }
        }

        // AoWCheckBox.pas:119
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            bool OldLit = default;
            base.CheckMouseMove(Shift, X, Y);
            FHandled = false;
            OldLit = FLit;
            if ((((X < (FAOWWindow.FWinLeft + FWinLeft)) || (X > (((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth) - 1))) || (Y < (FAOWWindow.FWinTop + FWinTop))) || (Y > (((FAOWWindow.FWinTop + FWinTop) + FWinHeight) - 1)))
            {
                FLit = false;
            }
            else
            {
                FHandled = true;
                FLit = !FChecked && !FAOWOwner.FHandled;
                if (FOnMouseMove != null)
                {
                    FOnMouseMove(this, Shift, X, Y);
                }
            }

            FRedraw = FLit;
            if (OldLit != FLit)
            {
                FDrawn = false;
            }
        }

        // AoWCheckBox.pas:38
        // { TAOWCheckBox }
        public virtual void SetChecked(bool Value)
        {
            if (Value != FChecked)
            {
                FChecked = Value;
                FDrawn = false;
                if (FRadioIndex > 0)
                {
                    if (FAOWWindow != null)
                    {
                        if (FChecked)
                        {
                            FAOWWindow.RadioChange(this);
                        }
                    }
                }
            }
        }

        [Published]
        public bool Checked { get => FChecked; set => SetChecked(value); }

        [Published]
        public int RadioIndex { get => FRadioIndex; set => FRadioIndex = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnDoubleClick { get => FOnDoubleClick; set => FOnDoubleClick = value; }
    }

    public static partial class AoWCheckBox
    {
        // AoWCheckBox.pas:141
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Interface", [typeof(Aow.Units.AoWCheckBox.TAOWCheckBox)]);
        }
    }
}
