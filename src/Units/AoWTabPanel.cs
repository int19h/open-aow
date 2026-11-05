// Translated from AoWTabPanel.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.AoWControl;
using Aow.Units.AoWComp;
using Aow.Units.AOWInterfaceImgLib;
using Aow.Units.AoWWinControl;

namespace Aow.Units.AoWTabPanel
{
    // { The AoWTabPanel unit of aowInt.dpl: TAOWTabPanel, a row of tabs, one per item, in a window.
//   Clicking a tab selects it; the panels (TAOWPanel) whose TabPanel is this control are shown when
//   their OwnerItem is the selected tab and hidden otherwise. }
    // Fields: FItems, FSelected, FILButton, FBorderHeightSelected (the text offset of the selected
    // tab), FILIndexUp, FILIndexDown, FPanels (the TAOWPanel objects of the tabs)
    public partial class TAOWTabPanel : TAOWControl
    {
        public TStringList FItems;
        public int FSelected;
        public TInterfaceIL FILButton;
        public int FBorderHeightSelected;
        public int FILIndexUp;
        public int FILIndexDown;
        public TList FPanels;
        // AoWTabPanel.pas:62
        public TAOWTabPanel(TComponent owner) : base(owner)
        {
            var call__1 = new TStringList();
            FItems = call__1;
            FItems.OnChange = OnChangeText;
            var call__2 = new TList();
            FPanels = call__2;
        }

        // AoWTabPanel.pas:70
        public override void Destroy()
        {
            int I = default;
            TAOWWindowControl P = default;
            {
                I = 0;
                int end__3 = FPanels.Count - 1;
                for (; I <= end__3; I += 1)
                {
                    var call__4 = FPanels[I];
                    P = (TAOWWindowControl)call__4;
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

            if (FILButton != null && FAOWFont != null)
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
            if ((call__7.Width + call__8.Width) * call__9 > FWinWidth)
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
        public override void Draw(TSurfaceDesc SurfDesc)
        {
            int I = default;
            TRect OldClip = default;
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
                                var call__19 = Rect(FAOWWindow.FWinLeft + FWinLeft + FWinWidth * I / call__18, FAOWWindow.FWinTop + FWinTop, FWinWidth / call__17, FWinHeight);
                                FILButton.DrawILLh(FILIndexUp, call__19, ref SurfDesc);
                                if (FAOWFont != null)
                                {
                                    FAOWFont.Color = FSelColor;
                                    var call__20 = FItems[I];
                                    var call__21 = FAOWFont.GetTextWidth(call__20);
                                    var call__22 = FItems.Count;
                                    var call__23 = FItems[I];
                                    FAOWFont.SetText(FAOWWindow.FWinLeft + FWinLeft + FWinWidth * (2 * I + 1) / (2 * call__22) - call__21 / 2, FAOWWindow.FWinTop + FWinTop + FBorderHeight, call__23, SurfDesc);
                                }
                            }
                            else
                            {
                                var call__24 = FItems.Count;
                                var call__25 = FItems.Count;
                                var call__26 = Rect(FAOWWindow.FWinLeft + FWinLeft + FWinWidth * I / call__25, FAOWWindow.FWinTop + FWinTop, FWinWidth / call__24, FWinHeight);
                                FILButton.DrawILLh(FILIndexDown, call__26, ref SurfDesc);
                                if (FAOWFont != null)
                                {
                                    FAOWFont.Color = FColor;
                                    var call__27 = FItems[I];
                                    var call__28 = FAOWFont.GetTextWidth(call__27);
                                    var call__29 = FItems.Count;
                                    var call__30 = FItems[I];
                                    FAOWFont.SetText(FAOWWindow.FWinLeft + FWinLeft + FWinWidth * (2 * I + 1) / (2 * call__29) - call__28 / 2, FAOWWindow.FWinTop + FWinTop + FBorderHeightSelected, call__30, SurfDesc);
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
                MessageBox(0, (AnsiString)"Exception during Draw", (AnsiString)"Error - " + Name, 0);
            }
        }

        // AoWTabPanel.pas:211
        // { CheckMouseDown: a left click on a tab selects it }
        public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseDown(Button, Shift, X, Y);
            FHandled = false;
            if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
            {
                FHandled = true;
                if (Button == TMouseButton.mbLeft)
                {
                    var call__31 = FItems.Count;
                    SetIndex((X - FAOWWindow.FWinLeft - FWinLeft) / (FWinWidth / call__31));
                }

                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }
            }
        }

        // AoWTabPanel.pas:226
        public override void CheckMouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight && FValid == true && FVisible == true)
            {
                FHandled = true;
                if (FOnMouseUp != null)
                {
                    FOnMouseUp(this, Button, Shift, X, Y);
                }
            }
        }

        // AoWTabPanel.pas:239
        public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseMove(Shift, X, Y);
            FHandled = false;
            if (X >= FAOWWindow.FWinLeft + FWinLeft && X <= FAOWWindow.FWinLeft + FWinLeft + FWinWidth - 1 && Y >= FAOWWindow.FWinTop + FWinTop && Y <= FAOWWindow.FWinTop + FWinTop + FWinHeight - 1)
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
        public virtual void SetILButton(TInterfaceIL Value)
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
        public void SetFStrings(TStringList Value)
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
                        var component = (TAoWComponent)FPanels[I];
                        if (component.FOwnerItem == this.FSelected)
                        {
                            component.SetVisible(true);
                        }
                        else
                        {
                            component.SetVisible(false);
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
        public TImageLibraryFont AOWFont { get => FAOWFont; set => FAOWFont = value; }

        [Published]
        public TStringList Items { get => FItems; set => SetFStrings(value); }

        [Published]
        public int BorderHeight { get => FBorderHeight; set => FBorderHeight = value; }

        [Published]
        public int Selected { get => FSelected; set => SetIndex(value); }

        [Published]
        public TInterfaceIL ILButton { get => FILButton; set => SetILButton(value); }

        [Published]
        public int ILIndexDown { get => FILIndexDown; set => SetILDIndex(value); }

        [Published]
        public int ILIndexUp { get => FILIndexUp; set => SetILUIndex(value); }

        [Published]
        public int BorderHeightSelected { get => FBorderHeightSelected; set => FBorderHeightSelected = value; }
    }

    public static partial class AoWTabPanel
    {
        // AoWTabPanel.pas:257
        public static void Register()
        {
            RegisterComponents((AnsiString)"AOW Interface", [typeof(TAOWTabPanel)]);
        }
    }
}
