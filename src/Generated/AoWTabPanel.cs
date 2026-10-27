// Generated from AoWTabPanel.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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

namespace Aow.Units.AoWTabPanel
{
    // { The AoWTabPanel unit of aowInt.dpl: TAOWTabPanel, a row of tabs, one per item, in a window.
//   Clicking a tab selects it; the panels (TAOWPanel) whose TabPanel is this control are shown when
//   their OwnerItem is the selected tab and hidden otherwise. }
    // { AoWTabPanel.TAOWTabPanel (instance size 304). Fields: +$114 FItems, +$118 FSelected, +$11C
//     FILButton, +$120 FBorderHeightSelected (the text offset of the selected tab), +$124
//     FILIndexUp, +$128 FILIndexDown, +$12C FPanels (the TAOWPanel objects of the tabs).
//     VMT (new): SetILButton, SetILUIndex, SetILDIndex }
    public partial class TAOWTabPanel : Aow.Units.AoWControl.TAOWControl
    {
        public Aow.Rtl.TStringList FItems;
        public int FSelected;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILButton;
        public int FBorderHeightSelected;
        public int FILIndexUp;
        public int FILIndexDown;
        public Aow.Rtl.TList FPanels;
        // AoWTabPanel.pas:62
        // { TAOWTabPanel }
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            base.Create__init(AOwner);
            var call__1 = new Aow.Rtl.TStringList();
            FItems = call__1;
            FItems.OnChange = OnChangeText;
            var call__2 = new Aow.Rtl.TList();
            FPanels = call__2;
        }

        // AoWTabPanel.pas:70
        public override void Destroy()
        {
            int I = default;
            Aow.Units.AoWWinControl.TAOWWindowControl P = default;
            {
                I = 0;
                int end__3 = FPanels.Count - 1;
                for (; I <= end__3; I += 1)
                {
                    var call__4 = FPanels[I];
                    P = (Aow.Units.AoWWinControl.TAOWWindowControl)call__4;
                    P.DetachMenuBar();
                }
            }

            FPanels.Destroy();
            FItems.Destroy();
            base.Destroy();
        }

        // AoWTabPanel.pas:112
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

            if ((FILButton != null) && (FAOWFont != null))
            {
                FValid = true;
            }

            base.Validate();
        }

        // AoWTabPanel.pas:123
        public override void SetSize()
        {
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
            var call__14 = FItems.Count;
            if (call__14 <= FSelected)
            {
                var call__15 = FItems.Count;
                FSelected = call__15 - 1;
            }

            base.SetSize();
        }

        // AoWTabPanel.pas:164
        // { Draw: the tabs side by side in equal shares of the width, the text centered; the selected tab
//   with the down image and its text BorderHeightSelected pixels down }
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
                        int end__16 = FItems.Count - 1;
                        for (; I <= end__16; I += 1)
                        {
                            if (I != FSelected)
                            {
                                var call__17 = FItems.Count;
                                var call__18 = FItems.Count;
                                var call__19 = Aow.Rtl.Rtl.Rect((FAOWWindow.FWinLeft + FWinLeft) + ((FWinWidth * I) / call__18), FAOWWindow.FWinTop + FWinTop, FWinWidth / call__17, FWinHeight);
                                FILButton.DrawILLh(FILIndexUp, call__19, ref SurfDesc);
                                if (FAOWFont != null)
                                {
                                    FAOWFont.Color = FSelColor;
                                    var call__20 = FItems[I];
                                    var call__21 = FAOWFont.GetTextWidth(call__20);
                                    var call__22 = FItems.Count;
                                    var call__23 = FItems[I];
                                    FAOWFont.SetText(((FAOWWindow.FWinLeft + FWinLeft) + ((FWinWidth * ((2 * I) + 1)) / (2 * call__22))) - (call__21 / 2), (FAOWWindow.FWinTop + FWinTop) + FBorderHeight, call__23, SurfDesc);
                                }
                            }
                            else
                            {
                                var call__24 = FItems.Count;
                                var call__25 = FItems.Count;
                                var call__26 = Aow.Rtl.Rtl.Rect((FAOWWindow.FWinLeft + FWinLeft) + ((FWinWidth * I) / call__25), FAOWWindow.FWinTop + FWinTop, FWinWidth / call__24, FWinHeight);
                                FILButton.DrawILLh(FILIndexDown, call__26, ref SurfDesc);
                                if (FAOWFont != null)
                                {
                                    FAOWFont.Color = FColor;
                                    var call__27 = FItems[I];
                                    var call__28 = FAOWFont.GetTextWidth(call__27);
                                    var call__29 = FItems.Count;
                                    var call__30 = FItems[I];
                                    FAOWFont.SetText(((FAOWWindow.FWinLeft + FWinLeft) + ((FWinWidth * ((2 * I) + 1)) / (2 * call__29))) - (call__28 / 2), (FAOWWindow.FWinTop + FWinTop) + FBorderHeightSelected, call__30, SurfDesc);
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

        // AoWTabPanel.pas:211
        // { CheckMouseDown: a left click on a tab selects it }
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseDown(Button, Shift, X, Y);
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = true;
                if (Button == Aow.Rtl.TMouseButton.mbLeft)
                {
                    var call__31 = FItems.Count;
                    SetIndex(((X - FAOWWindow.FWinLeft) - FWinLeft) / (FWinWidth / call__31));
                }

                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }
            }
        }

        // AoWTabPanel.pas:226
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

        // AoWTabPanel.pas:239
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

        // AoWTabPanel.pas:252
        public override void CheckKeyPress(ref byte Key)
        {
            FHandled = false;
        }

        // AoWTabPanel.pas:85
        public virtual void SetILButton(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILButton)
            {
                FValid = false;
                FILButton = Value;
            }
        }

        // AoWTabPanel.pas:94
        public virtual void SetILUIndex(int Value)
        {
            if (Value != FILIndexUp)
            {
                FILIndexUp = Value;
                FValid = false;
            }
        }

        // AoWTabPanel.pas:103
        public virtual void SetILDIndex(int Value)
        {
            if (Value != FILIndexDown)
            {
                FILIndexDown = Value;
                FValid = false;
            }
        }

        // AoWTabPanel.pas:133
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

        // AoWTabPanel.pas:143
        // { SetIndex selects a tab: the panels of the tab are shown, the others hidden }
        public void SetIndex(int Value)
        {
            int I = default;
            if (Value != FSelected)
            {
                FSelected = Value;
                {
                    I = 0;
                    int end__34 = FPanels.Count - 1;
                    for (; I <= end__34; I += 1)
                    {
                        {
                            var w__35 = (Aow.Units.AoWComp.TAoWComponent)FPanels[I];
                            if (w__35.FOwnerItem == this.FSelected)
                            {
                                w__35.SetVisible(true);
                            }
                            else
                            {
                                w__35.SetVisible(false);
                            }
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

        [Published]
        public Aow.Units.ImageLib.TImageLibraryFont AOWFont { get => FAOWFont; set => FAOWFont = value; }

        [Published]
        public Aow.Rtl.TStringList Items { get => FItems; set => SetFStrings(value); }

        [Published]
        public int BorderHeight { get => FBorderHeight; set => FBorderHeight = value; }

        [Published]
        public int Selected { get => FSelected; set => SetIndex(value); }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILButton { get => FILButton; set => SetILButton(value); }

        [Published]
        public int ILIndexDown { get => FILIndexDown; set => SetILDIndex(value); }

        [Published]
        public int ILIndexUp { get => FILIndexUp; set => SetILUIndex(value); }

        [Published]
        public int BorderHeightSelected { get => FBorderHeightSelected; set => FBorderHeightSelected = value; }

        // AoWTabPanel.pas:27
        public static new Aow.Units.AoWTabPanel.TAOWTabPanel Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.AoWTabPanel.TAOWTabPanel o = new Aow.Units.AoWTabPanel.TAOWTabPanel();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public static partial class AoWTabPanel
    {
        // AoWTabPanel.pas:257
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Interface", [typeof(Aow.Units.AoWTabPanel.TAOWTabPanel)]);
        }
    }
}
