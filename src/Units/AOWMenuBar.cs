// Translated from AOWMenuBar.pas.
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
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;

namespace Aow.Units.AOWMenuBar
{
    // { The AOWMenuBar unit of aowInt.dpl: TAOWMenuBar, a horizontal bar of buttons, one per item, in a
//   window. Clicking an item selects it and shows the menus (TAOWMenu) that belong to the item
//   (their OwnerItem is the item's index); clicking it again closes it. }
    // Fields: FItems, FSelected (-1 for none), FILButton, FLastSelected, FILIndexUp, FILIndexDown,
    // FMenus (the TAOWMenu objects whose OwnerMenuBar is this bar), F130 (only cleared and tested:
    // a click on the item last selected then does nothing)
    public partial class TAOWMenuBar : Aow.Units.AoWControl.TAOWControl
    {
        public Aow.Rtl.TStringList FItems;
        public int FSelected;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILButton;
        public int FLastSelected;
        public int FILIndexUp;
        public int FILIndexDown;
        public Aow.Rtl.TList FMenus;
        public bool F130;
        // AOWMenuBar.pas:65
        public TAOWMenuBar(TComponent owner) : base(owner)
        {
            var call__1 = new Aow.Rtl.TStringList();
            FItems = call__1;
            FItems.OnChange = OnChangeText;
            var call__2 = new Aow.Rtl.TList();
            FMenus = call__2;
            FSelected = -1;
        }

        // AOWMenuBar.pas:74
        public override void Destroy()
        {
            int I = default;
            Aow.Units.AoWWinControl.TAOWWindowControl M = default;
            {
                I = 0;
                int end__3 = FMenus.Count - 1;
                for (; I <= end__3; I += 1)
                {
                    var call__4 = FMenus[I];
                    M = (Aow.Units.AoWWinControl.TAOWWindowControl)call__4;
                    M.DetachMenuBar();
                }
            }

            FMenus.Destroy();
            FItems.Destroy();
            base.Destroy();
        }

        // AOWMenuBar.pas:98
        public override void Validate()
        {
            if (FILButton != null)
            {
                var call__5 = FILButton.CheckIL(FILIndexUp, 3, this);
                FILButton = call__5;
            }

            if (FILButton != null)
            {
                var call__6 = FILButton.CheckIL(FILIndexDown, 3, this);
                FILButton = call__6;
            }

            if (FILButton != null)
            {
                FValid = true;
            }

            base.Validate();
        }

        // AOWMenuBar.pas:110
        // { The bar is at least as wide as one up and one down button per item }
        public override void SetSize()
        {
            int I = default;
            var call__7 = FILButton[FILIndexUp];
            var call__8 = FILButton[FILIndexUp + 2];
            var call__9 = FItems.Count;
            if (((call__7.Width + call__8.Width) * call__9) > FWinWidth)
            {
                var call__10 = FILButton[FILIndexUp];
                var call__11 = FILButton[FILIndexUp + 2];
                var call__12 = FItems.Count;
                FWinWidth = (call__10.Width + call__11.Width) * call__12;
            }

            var call__13 = FILButton[FILIndexUp];
            FWinHeight = call__13.Height;
            {
                I = 0;
                int end__14 = FMenus.Count - 1;
                for (; I <= end__14; I += 1)
                {
                    var call__15 = FMenus[I];
                    ((Aow.Units.AoWComp.TAoWComponent)call__15).FAligned = false;
                }
            }

            base.SetSize();
        }

        // AOWMenuBar.pas:122
        public override void ReAlign()
        {
            int I = default;
            {
                I = 0;
                int end__16 = FMenus.Count - 1;
                for (; I <= end__16; I += 1)
                {
                    var call__17 = FMenus[I];
                    ((Aow.Units.AoWComp.TAoWComponent)call__17).FAligned = false;
                }
            }

            base.ReAlign();
        }

        // AOWMenuBar.pas:160
        // { Draw: the items side by side, each in an equal share of the width; the selected one with the up
//   image and the text one pixel lower }
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            int I = default;
            Aow.Rtl.TRect OldClip = default;
            try
            {
                if (FValid && FVisible)
                {
                    if (FOnPreDraw != null)
                    {
                        FOnPreDraw(this, SurfDesc);
                    }

                    OldClip = SurfDesc.ClipRect;
                    {
                        I = 0;
                        int end__18 = FItems.Count - 1;
                        for (; I <= end__18; I += 1)
                        {
                            if (I != FSelected)
                            {
                                var call__19 = FItems.Count;
                                var call__20 = FItems.Count;
                                var call__21 = Aow.Rtl.Rtl.Rect((FAOWWindow.FWinLeft + FWinLeft) + ((FWinWidth * I) / call__20), FAOWWindow.FWinTop + FWinTop, FWinWidth / call__19, FWinHeight);
                                FILButton.DrawILLh(FILIndexDown, call__21, ref SurfDesc);
                                if (FAOWFont != null)
                                {
                                    FAOWFont.Color = FSelColor;
                                    var call__22 = FItems[I];
                                    var call__23 = FItems.Count;
                                    FAOWFont.SetText(((FAOWWindow.FWinLeft + FWinLeft) + ((FWinWidth * I) / call__23)) + FBorderWidth, (FAOWWindow.FWinTop + FWinTop) + FBorderHeight, call__22, SurfDesc);
                                }
                            }
                            else
                            {
                                var call__24 = FItems.Count;
                                var call__25 = FItems.Count;
                                var call__26 = Aow.Rtl.Rtl.Rect((FAOWWindow.FWinLeft + FWinLeft) + ((FWinWidth * I) / call__25), FAOWWindow.FWinTop + FWinTop, FWinWidth / call__24, FWinHeight);
                                FILButton.DrawILLh(FILIndexUp, call__26, ref SurfDesc);
                                if (FAOWFont != null)
                                {
                                    FAOWFont.Color = FColor;
                                    var call__27 = FItems[I];
                                    var call__28 = FItems.Count;
                                    FAOWFont.SetText(((FAOWWindow.FWinLeft + FWinLeft) + ((FWinWidth * I) / call__28)) + FBorderWidth, ((FAOWWindow.FWinTop + FWinTop) + FBorderHeight) + 1, call__27, SurfDesc);
                                }
                            }
                        }
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

        // AOWMenuBar.pas:206
        // { CheckMouseDown: a left click on an item selects it and shows its menus, or closes it when it
//   was selected; a click outside the bar closes the selected item }
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int I = default;
            base.CheckMouseDown(Button, Shift, X, Y);
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = true;
                FRedraw = true;
                if (Button == Aow.Rtl.TMouseButton.mbLeft)
                {
                    var call__29 = FItems.Count;
                    I = ((X - FAOWWindow.FWinLeft) - FWinLeft) / (FWinWidth / call__29);
                    if (I == FSelected)
                    {
                        FSelected = -1;
                        if (FOnChange != null)
                        {
                            FOnChange(this);
                        }

                        FDrawn = false;
                    }
                    else
                    {
                        if (!F130 || ((I > -1) && (I != FLastSelected)))
                        {
                            FSelected = I;
                            FLastSelected = I;
                            {
                                I = 0;
                                int end__30 = FMenus.Count - 1;
                                for (; I <= end__30; I += 1)
                                {
                                    var component = (Aow.Units.AoWComp.TAoWComponent)FMenus[I];
                                    if (component.FOwnerItem == this.FSelected)
                                    {
                                        component.SetVisible(true);
                                    }
                                }
                            }

                            if (FOnChange != null)
                            {
                                FOnChange(this);
                            }

                            FDrawn = false;
                        }
                    }
                }

                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }
            }
            else
            {
                if (FSelected > -1)
                {
                    FSelected = -1;
                    FDrawn = false;
                }
            }

            F130 = false;
        }

        // AOWMenuBar.pas:251
        public override void CheckMouseUp(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            if ((((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight))) && (FValid == true)) && (FVisible == true))
            {
                FHandled = true;
                if (FOnMouseUp != null)
                {
                    FOnMouseUp(this, Button, Shift, X, Y);
                }
            }
        }

        // AOWMenuBar.pas:264
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseMove(Shift, X, Y);
            FHandled = false;
            if ((((X >= (FAOWWindow.FWinLeft + FWinLeft)) && (X <= (((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth) - 1))) && (Y >= (FAOWWindow.FWinTop + FWinTop))) && (Y <= (((FAOWWindow.FWinTop + FWinTop) + FWinHeight) - 1)))
            {
                FHandled = true;
                if (FOnMouseMove != null)
                {
                    FOnMouseMove(this, Shift, X, Y);
                }
            }
        }

        // AOWMenuBar.pas:277
        public override void CheckKeyPress(ref byte Key)
        {
            FHandled = false;
        }

        // AOWMenuBar.pas:89
        public virtual void SetILButton(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILButton)
            {
                FValid = false;
                FILButton = Value;
            }
        }

        // AOWMenuBar.pas:131
        public virtual void SetILUIndex(int Value)
        {
            if (Value != FILIndexUp)
            {
                FILIndexUp = Value;
                FValid = false;
            }
        }

        // AOWMenuBar.pas:140
        public virtual void SetILDIndex(int Value)
        {
            if (Value != FILIndexDown)
            {
                FILIndexDown = Value;
                FValid = false;
            }
        }

        // AOWMenuBar.pas:149
        public void SetFStrings(Aow.Rtl.TStringList Value)
        {
            int I = default;
            FItems.Clear();
            {
                I = 0;
                int end__32 = Value.Count - 1;
                for (; I <= end__32; I += 1)
                {
                    var call__33 = Value[I];
                    FItems.Add(call__33);
                }
            }
        }

        [Published]
        public Aow.Units.ImageLib.TImageLibraryFont AOWFont { get => FAOWFont; set => FAOWFont = value; }

        [Published]
        public Aow.Rtl.TStringList Items { get => FItems; set => SetFStrings(value); }

        [Published]
        public int BorderWidth { get => FBorderWidth; set => FBorderWidth = value; }

        [Published]
        public int BorderHeight { get => FBorderHeight; set => FBorderHeight = value; }

        [Published]
        public int Selected { get => FSelected; set => FSelected = value; }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILButton { get => FILButton; set => SetILButton(value); }

        [Published]
        public int ILIndexDown { get => FILIndexDown; set => SetILDIndex(value); }

        [Published]
        public int ILIndexUp { get => FILIndexUp; set => SetILUIndex(value); }
    }

    public static partial class AOWMenuBar
    {
        // AOWMenuBar.pas:282
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Interface", [typeof(Aow.Units.AOWMenuBar.TAOWMenuBar)]);
        }
    }
}
