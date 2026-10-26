// Generated from AoWButton.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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

namespace Aow.Units.AoWButton
{
    // { The AoWButton unit of aowInt.dpl: TAOWBaseButton, the common ancestor of the push button and
//   the check box, which draws itself from a button library (three images: the left end, the
//   middle and the right end of a horizontal line) or from an icon library (one image), in the up,
//   down or lit (mouse over) state, with its text centred on it; and TAOWButton, the push button,
//   which calls OnClick when the left button is released over it. }
    // { AoWButton.TAOWBaseButton (instance size 312). Fields: +$114 FBorderHeightSelected (the text
//     offset from the top when the button is down; FBorderHeight when it is up), +$118 FButtonType
//     (btText: drawn from FILButton, btIcon: from FILIcon; set by Validate), +$11C FILButton, +$120
//     FILIcon, +$124 FILIndexUp, +$128 FILIndexDown, +$12C FILIndexLit, +$130 FPopupMenu, +$134
//     FLit (the mouse is over the button). FChecked (+$B0) is the down state.
//     VMT (new): SetILButton, SetILIcon, SetILUIndex, SetILDIndex, SetILHIndex }
    public partial class TAOWBaseButton : Aow.Units.AoWControl.TAOWControl
    {
        public int FBorderHeightSelected;
        public Aow.Units.AoWComp.TButtonType FButtonType;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILButton;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILIcon;
        public int FILIndexUp;
        public int FILIndexDown;
        public int FILIndexLit;
        public Aow.Units.AOWMenu.TAOWMenu FPopupMenu;
        public bool FLit;
        // AoWButton.pas:85
        public override void Validate()
        {
            if (FILButton != null)
            {
                var call__1 = FILButton.CheckIL(FILIndexUp, 3, this);
                FILButton = call__1;
            }

            if (FILButton != null)
            {
                var call__2 = FILButton.CheckIL(FILIndexDown, 3, this);
                FILButton = call__2;
            }

            if ((FILButton != null) && (FILIndexLit != FILIndexUp))
            {
                var call__3 = FILButton.CheckIL(FILIndexLit, 3, this);
                FILButton = call__3;
            }

            if (FILIcon != null)
            {
                var call__4 = FILIcon.CheckIL(FILIndexUp, 1, this);
                FILIcon = call__4;
            }

            if (FILIcon != null)
            {
                var call__5 = FILIcon.CheckIL(FILIndexDown, 1, this);
                FILIcon = call__5;
            }

            if ((FILIcon != null) && (FILIndexLit != FILIndexUp))
            {
                var call__6 = FILIcon.CheckIL(FILIndexLit, 1, this);
                FILIcon = call__6;
            }

            if (FILButton != null)
            {
                FButtonType = Aow.Units.AoWComp.TButtonType.btText;
                FValid = true;
            }
            else
            {
                if (FILIcon != null)
                {
                    FButtonType = Aow.Units.AoWComp.TButtonType.btIcon;
                    FValid = true;
                }
            }

            base.Validate();
        }

        // AoWButton.pas:112
        public override void SetSize()
        {
            switch (FButtonType)
            {
                case Aow.Units.AoWComp.TButtonType.btText:
                    var call__7 = FILButton[FILIndexUp];
                    var call__8 = FILButton[FILIndexUp + 2];
                    if ((call__7.Width + call__8.Width) > FWinWidth)
                    {
                        var call__9 = FILButton[FILIndexUp];
                        var call__10 = FILButton[FILIndexUp + 2];
                        FWinWidth = call__9.Width + call__10.Width;
                    }

                    var call__11 = FILButton[FILIndexUp];
                    FWinHeight = call__11.Height;
                    break;
                case Aow.Units.AoWComp.TButtonType.btIcon:
                    var call__12 = FILIcon[FILIndexUp];
                    FWinWidth = call__12.Width;
                    var call__13 = FILIcon[FILIndexUp];
                    FWinHeight = call__13.Height;
                    break;
            }

            base.SetSize();
        }

        // AoWButton.pas:78
        // { TAOWBaseButton }
        public override void SetVisible(bool Value)
        {
            if (Value != FVisible)
            {
                FLit = false;
            }

            base.SetVisible(Value);
        }

        // AoWButton.pas:183
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // AoWButton.pas:136
        public virtual void SetILButton(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILButton)
            {
                FILIcon = null;
                FValid = false;
                FILButton = Value;
            }
        }

        // AoWButton.pas:146
        public virtual void SetILIcon(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILIcon)
            {
                FILButton = null;
                FValid = false;
                FILIcon = Value;
            }
        }

        // AoWButton.pas:156
        public virtual void SetILUIndex(int Value)
        {
            if (Value != FILIndexUp)
            {
                FILIndexUp = Value;
                FValid = false;
            }
        }

        // AoWButton.pas:165
        public virtual void SetILDIndex(int Value)
        {
            if (Value != FILIndexDown)
            {
                FILIndexDown = Value;
                FValid = false;
            }
        }

        // AoWButton.pas:174
        public virtual void SetILHIndex(int Value)
        {
            if (Value != FILIndexLit)
            {
                FILIndexLit = Value;
                FValid = false;
            }
        }

        // AoWButton.pas:130
        public void SetGText(Aow.Rtl.AnsiString Value)
        {
            FText = Value;
            FDrawn = false;
        }

        [Published]
        public Aow.Units.ImageLib.TImageLibraryFont AOWFont { get => FAOWFont; set => FAOWFont = value; }

        [Published]
        public int BorderHeight { get => FBorderHeight; set => FBorderHeight = value; }

        [Published]
        public int BorderHeightSelected { get => FBorderHeightSelected; set => FBorderHeightSelected = value; }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILButton { get => FILButton; set => SetILButton(value); }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILIcon { get => FILIcon; set => SetILIcon(value); }

        [Published]
        public int ILIndexDown { get => FILIndexDown; set => SetILDIndex(value); }

        [Published]
        public int ILIndexLit { get => FILIndexLit; set => SetILHIndex(value); }

        [Published]
        public int ILIndexUp { get => FILIndexUp; set => SetILUIndex(value); }

        [Published]
        public Aow.Units.AOWMenu.TAOWMenu PopupMenu { get => FPopupMenu; set => FPopupMenu = value; }

        [Published]
        public Aow.Rtl.AnsiString Text { get => FText; set => SetGText(value); }
    }

    // { AoWButton.TAOWButton (instance size 320). Fields: +$138 FOnClick }
    public partial class TAOWButton : Aow.Units.AoWButton.TAOWBaseButton
    {
        public Aow.Rtl.TNotifyEvent FOnClick;
        // AoWButton.pas:239
        // { TAOWButton }
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseDown(Button, Shift, X, Y);
            FHandled = false;
            FLit = false;
            if (!FAOWWindow.FHandled)
            {
                if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
                {
                    FHandled = true;
                    if (Button == Aow.Rtl.TMouseButton.mbLeft)
                    {
                        FMouseDown = true;
                        FChecked = true;
                        FDrawn = false;
                        FRedraw = true;
                    }

                    if (FOnMouseDown != null)
                    {
                        FOnMouseDown(this, Button, Shift, X, Y);
                    }
                }
            }
        }

        // AoWButton.pas:261
        public override void CheckMouseUp(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            FChecked = false;
            FLit = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = true;
                if (Button == Aow.Rtl.TMouseButton.mbLeft)
                {
                    if (FMouseDown && (FOnClick != null))
                    {
                        FOnClick(this);
                    }

                    FDrawn = false;
                }

                if (FOnMouseUp != null)
                {
                    FOnMouseUp(this, Button, Shift, X, Y);
                }
            }

            if (FMouseDown)
            {
                FDrawn = false;
            }

            FMouseDown = false;
        }

        // AoWButton.pas:284
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            bool OldChecked = default;
            bool OldLit = default;
            base.CheckMouseMove(Shift, X, Y);
            FHandled = false;
            OldChecked = FChecked;
            OldLit = FLit;
            FRedraw = false;
            if ((((FAOWWindow.FHandled || (X < (FAOWWindow.FWinLeft + FWinLeft))) || (X > (((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth) - 1))) || (Y < (FAOWWindow.FWinTop + FWinTop))) || (Y > (((FAOWWindow.FWinTop + FWinTop) + FWinHeight) - 1)))
            {
                FChecked = false;
                FLit = false;
            }
            else
            {
                FHandled = true;
                FChecked = FMouseDown;
                FLit = !FMouseDown;
                FRedraw = true;
                if (FOnMouseMove != null)
                {
                    FOnMouseMove(this, Shift, X, Y);
                }
            }

            if ((OldLit != FLit) || (OldChecked != FChecked))
            {
                FDrawn = false;
            }
        }

        [Published]
        public Aow.Rtl.TNotifyEvent OnClick { get => FOnClick; set => FOnClick = value; }
    }

    public static partial class AoWButton
    {
        // AoWButton.pas:313
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Interface", [typeof(Aow.Units.AoWButton.TAOWButton)]);
        }
    }
}
