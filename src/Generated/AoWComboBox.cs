// Generated from AoWComboBox.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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

namespace Aow.Units.AoWComboBox
{
    // { The AoWComboBox unit of aowInt.dpl: TAOWComboBox, a drop-down list. Closed, it shows the
//   selected item in an edit field; a click opens the list below the field (at PriorityOpened, with
//   a vertical scroll bar when one is attached), a click on an item selects it and closes the list. }
    // { AoWComboBox.TAOWComboBox (instance size 416). Fields: +$114 F114 (the mouse button went down
//     on the field), +$118 FOpen (the list is shown), +$11C FItems, +$120 FVScrollBar, +$124 F124
//     (the rows the open list shows), +$128 FNumberVisible, +$12C F12C (the first item shown),
//     +$130 FListBorderWidth, +$134 FListBorderHeight, +$138 FSelected, +$13C F13C (the item under
//     the mouse, -1 for none), +$140 FPriorityOpened, +$144 F144 (the priority before the list was
//     opened), +$148 FOnDrawItem, +$150 F150 (the width the scroll bar takes from the list), +$154
//     F154 (the scroll bar is visible), +$158 FILEdit, +$15C FILFrame, +$160 FILTextureNormal, +$164
//     FILTextureSelected, +$168 FItemHeight, +$16C FOnOpen, +$174 FOnClose, +$17C FILEIndex, +$180
//     FILFIndex, +$184 FILTnIndex, +$188 FILTsIndex, +$18C FDrawItem, +$190 FDrawItemRect (the item
//     and its rectangle while OnDrawItem runs).
//     VMT (new): SetILEdit, SetILEIndex, SetILFrame, SetILFIndex, SetILTNorm, SetILTnIndex,
//     SetILTSel, SetILTsIndex, SetIndex }
    public partial class TAOWComboBox : Aow.Units.AoWControl.TAOWControl
    {
        public bool F114;
        public bool FOpen;
        public Aow.Rtl.TStringList FItems;
        public Aow.Units.AoWVScroll.TAOWVScrollBar FVScrollBar;
        public int F124;
        public int FNumberVisible;
        public int F12C;
        public int FListBorderWidth;
        public int FListBorderHeight;
        public int FSelected;
        public int F13C;
        public int FPriorityOpened;
        public int F144;
        public Aow.Rtl.TNotifyEvent FOnDrawItem;
        public int F150;
        public bool F154;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILEdit;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILFrame;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILTextureNormal;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILTextureSelected;
        public int FItemHeight;
        public Aow.Rtl.TNotifyEvent FOnOpen;
        public Aow.Rtl.TNotifyEvent FOnClose;
        public int FILEIndex;
        public int FILFIndex;
        public int FILTnIndex;
        public int FILTsIndex;
        public int FDrawItem;
        public Aow.Rtl.TRect FDrawItemRect;
        // AoWComboBox.pas:114
        // { TAOWComboBox }
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            base.Create__init(AOwner);
            var call__1 = new Aow.Rtl.TStringList();
            FItems = call__1;
            FItems.OnChange = OnChangeText;
            F13C = -1;
        }

        // AoWComboBox.pas:122
        public override void Destroy()
        {
            FItems.Destroy();
            base.Destroy();
        }

        // AoWComboBox.pas:128
        public override void Validate()
        {
            if (FILEdit != null)
            {
                var call__2 = FILEdit.CheckIL(FILEIndex, 3, this);
                FILEdit = call__2;
            }

            if (FILFrame != null)
            {
                var call__3 = FILFrame.CheckIL(FILFIndex, 8, this);
                FILFrame = call__3;
            }

            if (FILTextureNormal != null)
            {
                var call__4 = FILTextureNormal.CheckIL(FILTnIndex, 1, this);
                FILTextureNormal = call__4;
            }

            if (FILTextureSelected != null)
            {
                var call__5 = FILTextureSelected.CheckIL(FILTsIndex, 1, this);
                FILTextureSelected = call__5;
            }

            if (((((FAOWFont != null) && (FILEdit != null)) && (FILFrame != null)) && (FILTextureNormal != null)) && (FILTextureSelected != null))
            {
                FValid = true;
            }

            base.Validate();
        }

        // AoWComboBox.pas:146
        // { SetSize closes the list and sets up the scroll bar. The scroll bar's range and page are one
//   item short of the open list's, and its top is taken from the first edit image (original) }
        public override void SetSize()
        {
            FOpen = false;
            var call__6 = FILEdit[FILEIndex];
            var call__7 = FILEdit[FILEIndex + 2];
            if ((call__6.Width + call__7.Width) > FWinWidth)
            {
                var call__8 = FILEdit[FILEIndex];
                var call__9 = FILEdit[FILEIndex + 2];
                FWinWidth = call__8.Width + call__9.Width;
            }

            F124 = FNumberVisible;
            var call__10 = FItems.Count;
            if (F124 > call__10)
            {
                var call__11 = FItems.Count;
                F124 = call__11;
            }

            var call__12 = FItems.Count;
            if (call__12 <= FSelected)
            {
                var call__13 = FItems.Count;
                FSelected = call__13 - 1;
            }

            var call__14 = FILEdit[FILEIndex];
            FWinHeight = call__14.Height;
            if (FVScrollBar != null)
            {
                if (FVScrollBar.FAOWWindow != FAOWWindow)
                {
                    FVScrollBar.SetAOWWindow(FAOWWindow);
                }

                FVScrollBar.Priority = FPriority + 1;
                FVScrollBar.SetFMin(0);
                var call__15 = FItems.Count;
                FVScrollBar.SetFMax(call__15 - 2);
                FVScrollBar.SetFPos(F12C);
                FVScrollBar.SetFNum(F124 - 1);
                var call__16 = FILFrame[FILFIndex + 6];
                FVScrollBar.WinHeight = call__16.Height + ((FItemHeight * F124) + FBorderHeight);
                FVScrollBar.SetLarge(F124 - 1);
                FVScrollBar.Visible = FOpen;
                bool bool__17 = FVScrollBar.FVanishWhenFull;
                if (bool__17)
                {
                    var call__18 = FItems.Count;
                    bool__17 = ((call__18 - 1) < F124);
                }

                if (bool__17)
                {
                    FVScrollBar.Visible = false;
                }

                FVScrollBar.FSized = false;
                FVScrollBar.WinLeft = (FWinLeft + FWinWidth) - FVScrollBar.FWinWidth;
                var call__19 = FILEdit[0];
                FVScrollBar.WinTop = call__19.Height + FWinTop;
                F154 = FVScrollBar.FVisible;
            }

            base.SetSize();
        }

        // AoWComboBox.pas:356
        // { Update keeps the first item shown in step with the scroll bar }
        public override void Update()
        {
            base.Update();
            if (FVisible)
            {
                FRedraw = FOpen;
                bool bool__20 = (FVScrollBar != null);
                if (bool__20)
                {
                    var call__21 = FItems.Count;
                    bool__20 = (call__21 > 0);
                }

                if (bool__20)
                {
                    if (FVScrollBar.FPosition != F12C)
                    {
                        F12C = FVScrollBar.FPosition;
                        var call__22 = FItems.Count;
                        if ((F12C + F124) > call__22)
                        {
                            var call__23 = FItems.Count;
                            F12C = call__23 - F124;
                        }

                        FDrawn = false;
                    }
                }
            }
        }

        // AoWComboBox.pas:204
        public override void SetVisible(bool Value)
        {
            base.SetVisible(Value);
            if (FOpen && (FVScrollBar != null))
            {
                FVScrollBar.Visible = FVisible;
            }
        }

        // AoWComboBox.pas:286
        // { Draw draws the edit field with the selected item and, when the list is open, the frame, the
//   texture of the list, the highlight of the item under the mouse and the items, clipped short of
//   the scroll bar; OnDrawItem is called for every item with FDrawItem and FDrawItemRect set }
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // AoWComboBox.pas:375
        // { CheckMouseDown opens or closes the list with a click on the edit field and closes it with a
//   click outside the control }
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseDown(Button, Shift, X, Y);
            FHandled = false;
            bool bool__24 = ((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1));
            if (bool__24)
            {
                var call__25 = FILEdit[FILEIndex];
                bool__24 = (Y < ((FAOWWindow.FWinTop + FWinTop) + call__25.Height));
            }

            if (bool__24)
            {
                FHandled = true;
                if (Button == Aow.Rtl.TMouseButton.mbLeft)
                {
                    FMouseDown = true;
                    F114 = true;
                    if ((FValid == true) && (FVisible == true))
                    {
                        if (FOpen)
                        {
                            if (F144 != FPriority)
                            {
                                Priority = F144;
                            }

                            FOpen = false;
                            var call__26 = FILEdit[FILEIndex];
                            FWinHeight = call__26.Height;
                            if (FVScrollBar != null)
                            {
                                FVScrollBar.Visible = false;
                            }

                            if (FOnClose != null)
                            {
                                FOnClose(this);
                            }
                        }
                        else
                        {
                            F144 = FPriority;
                            if (FPriorityOpened != FPriority)
                            {
                                Priority = FPriorityOpened;
                            }

                            FOpen = true;
                            var call__27 = FItems.Count;
                            if (call__27 < F124)
                            {
                                var call__28 = FItems.Count;
                                F124 = call__28;
                            }

                            var call__29 = FILEdit[FILEIndex];
                            var call__30 = FILFrame[FILFIndex + 1];
                            var call__31 = FILFrame[FILFIndex + 6];
                            FWinHeight = (((call__29.Height + FListBorderHeight) + (FItemHeight * F124)) + call__30.Height) + call__31.Height;
                            var call__32 = FItems.Count;
                            if (call__32 < (F12C + F124))
                            {
                                var call__33 = FItems.Count;
                                F12C = call__33 - F124;
                            }

                            FDrawn = false;
                            var call__34 = FILFrame[FILFIndex + 4];
                            F150 = call__34.Width;
                            F154 = false;
                            if ((FVScrollBar != null) && (F124 > 0))
                            {
                                if ((FPriority + 1) != FVScrollBar.FPriority)
                                {
                                    FVScrollBar.Priority = FPriority + 1;
                                }

                                FVScrollBar.SetFMin(0);
                                var call__35 = FItems.Count;
                                FVScrollBar.SetFMax(call__35 - 1);
                                FVScrollBar.SetFPos(F12C);
                                FVScrollBar.SetFNum(F124);
                                var call__36 = FILFrame[FILFIndex + 6];
                                FVScrollBar.WinHeight = call__36.Height + ((FItemHeight * F124) + FBorderHeight);
                                FVScrollBar.Visible = true;
                                bool bool__37 = (FVScrollBar.FVanishWhenFull == true);
                                if (bool__37)
                                {
                                    var call__38 = FItems.Count;
                                    bool__37 = ((call__38 - 1) < F124);
                                }

                                if (bool__37)
                                {
                                    FVScrollBar.Visible = false;
                                }
                                else
                                {
                                    F150 = FVScrollBar.FWinWidth;
                                }

                                F154 = FVScrollBar.FVisible;
                                FVScrollBar.FSized = false;
                                FVScrollBar.WinLeft = (FWinLeft + FWinWidth) - FVScrollBar.FWinWidth;
                                var call__39 = FILEdit[FILEIndex];
                                FVScrollBar.WinTop = call__39.Height + FWinTop;
                                FVScrollBar.FDrawn = false;
                            }

                            if (FOnOpen != null)
                            {
                                FOnOpen(this);
                            }
                        }
                    }
                    else
                    {
                        if (FOpen == true)
                        {
                            if (F144 != FPriority)
                            {
                                Priority = F144;
                            }

                            FOpen = false;
                            var call__40 = FILEdit[FILEIndex];
                            FWinHeight = call__40.Height;
                            if (FVScrollBar != null)
                            {
                                FVScrollBar.Visible = false;
                            }

                            if (FOnClose != null)
                            {
                                FOnClose(this);
                            }
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
                if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
                {
                    FHandled = true;
                }
                else
                {
                    if (FOpen)
                    {
                        if (F144 != FPriority)
                        {
                            Priority = F144;
                        }

                        FOpen = false;
                        var call__41 = FILEdit[FILEIndex];
                        FWinHeight = call__41.Height;
                        if (FVScrollBar != null)
                        {
                            FVScrollBar.Visible = false;
                        }

                        if (FOnClose != null)
                        {
                            FOnClose(this);
                        }
                    }
                }
            }
        }

        // AoWComboBox.pas:476
        // { CheckMouseUp selects the item under the mouse in the open list and closes the list }
        public override void CheckMouseUp(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int I = default;
            FHandled = false;
            FMouseDown = false;
            F114 = false;
            FRedraw = false;
            bool bool__42 = ((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1));
            if (bool__42)
            {
                var call__43 = FILEdit[FILEIndex];
                bool__42 = (Y < ((FAOWWindow.FWinTop + FWinTop) + call__43.Height));
            }

            if (bool__42)
            {
                FHandled = true;
                if (FOnMouseUp != null)
                {
                    FOnMouseUp(this, Button, Shift, X, Y);
                }
            }
            else
            {
                if ((((Button == Aow.Rtl.TMouseButton.mbLeft) && (FValid == true)) && (FVisible == true)) && (FOpen == true))
                {
                    bool bool__44 = (X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth));
                    if (bool__44)
                    {
                        var call__45 = FILEdit[FILEIndex];
                        bool__44 = (Y > (((FAOWWindow.FWinTop + FWinTop) + call__45.Height) - 1));
                    }

                    if (bool__44 && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
                    {
                        if (X < (((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth) - F150))
                        {
                            var call__46 = FILEdit[FILEIndex];
                            I = (((((Y - FAOWWindow.FWinTop) - FWinTop) - FBorderHeight) - call__46.Height) / FItemHeight) + F12C;
                            var call__47 = FItems.Count;
                            if (I < call__47)
                            {
                                FSelected = I;
                            }

                            FOpen = false;
                            FRedraw = false;
                            var call__48 = FILEdit[FILEIndex];
                            FWinHeight = call__48.Height;
                            if (FVScrollBar != null)
                            {
                                FVScrollBar.Visible = false;
                            }

                            FHandled = true;
                            if (FOnChange != null)
                            {
                                FOnChange(this);
                            }

                            if (FOnClose != null)
                            {
                                FOnClose(this);
                            }
                        }
                    }
                    else
                    {
                        FOpen = false;
                        FRedraw = false;
                        var call__49 = FILEdit[FILEIndex];
                        FWinHeight = call__49.Height;
                        if (FVScrollBar != null)
                        {
                            FVScrollBar.Visible = false;
                        }

                        if (FOnClose != null)
                        {
                            FOnClose(this);
                        }
                    }
                }
            }
        }

        // AoWComboBox.pas:530
        // { CheckMouseMove tracks the item under the mouse in the open list }
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int OldItem = default;
            base.CheckMouseMove(Shift, X, Y);
            FHandled = false;
            OldItem = F13C;
            F13C = -1;
            if ((((X >= (FAOWWindow.FWinLeft + FWinLeft)) && (X < (((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth) - F150))) && (Y >= (FAOWWindow.FWinTop + FWinTop))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = true;
                if (FOpen)
                {
                    var call__50 = FILEdit[FILEIndex];
                    bool bool__51 = (Y >= (((FAOWWindow.FWinTop + FWinTop) + FBorderHeight) + call__50.Height));
                    if (bool__51)
                    {
                        var call__52 = FILEdit[FILEIndex];
                        bool__51 = (Y < ((((FAOWWindow.FWinTop + FWinTop) + call__52.Height) + FBorderHeight) + (FItemHeight * F124)));
                    }

                    if (bool__51)
                    {
                        var call__53 = FILEdit[FILEIndex];
                        F13C = (((((Y - FAOWWindow.FWinTop) - FWinTop) - FBorderHeight) - call__53.Height) / FItemHeight) + F12C;
                    }
                }

                if (FOnMouseMove != null)
                {
                    FOnMouseMove(this, Shift, X, Y);
                }
            }

            if ((FOpen == true) && (OldItem != F13C))
            {
                FDrawn = false;
                if (FVScrollBar != null)
                {
                    FVScrollBar.FDrawn = false;
                }
            }
        }

        // AoWComboBox.pas:211
        public virtual void SetILEdit(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILEdit)
            {
                FValid = false;
                FILEdit = Value;
            }
        }

        // AoWComboBox.pas:220
        public virtual void SetILEIndex(int Value)
        {
            if (Value != FILEIndex)
            {
                FILEIndex = Value;
                FValid = false;
            }
        }

        // AoWComboBox.pas:229
        public virtual void SetILFrame(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILFrame)
            {
                FValid = false;
                FILFrame = Value;
            }
        }

        // AoWComboBox.pas:238
        public virtual void SetILFIndex(int Value)
        {
            if (Value != FILFIndex)
            {
                FILFIndex = Value;
                FValid = false;
            }
        }

        // AoWComboBox.pas:247
        public virtual void SetILTNorm(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILTextureNormal)
            {
                FValid = false;
                FILTextureNormal = Value;
            }
        }

        // AoWComboBox.pas:256
        public virtual void SetILTnIndex(int Value)
        {
            if (Value != FILTnIndex)
            {
                FILTnIndex = Value;
                FValid = false;
            }
        }

        // AoWComboBox.pas:265
        public virtual void SetILTSel(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILTextureSelected)
            {
                FValid = false;
                FILTextureSelected = Value;
            }
        }

        // AoWComboBox.pas:274
        public virtual void SetILTsIndex(int Value)
        {
            if (Value != FILTsIndex)
            {
                FILTsIndex = Value;
                FValid = false;
            }
        }

        // AoWComboBox.pas:179
        public virtual void SetIndex(int Value)
        {
            var call__54 = FItems.Count;
            if (Value >= call__54)
            {
                var call__55 = FItems.Count;
                Value = call__55 - 1;
            }

            if (Value != FSelected)
            {
                FSelected = Value;
                FDrawn = false;
            }
        }

        // AoWComboBox.pas:190
        public void SetFStrings(Aow.Rtl.TStringList Value)
        {
            FItems.Clear();
            FItems.AddStrings(Value);
        }

        // AoWComboBox.pas:196
        public void SetVScroll(Aow.Units.AoWVScroll.TAOWVScrollBar Value)
        {
            // { Tried parameter identity, with and typed/TObject locals; original scrollbar save still differs. }
            FVScrollBar = Value;
            if (Value != null)
            {
                Value.Priority = FPriority;
            }

            FSized = false;
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
        public int ListBorderWidth { get => FListBorderWidth; set => FListBorderWidth = value; }

        [Published]
        public int ListBorderHeight { get => FListBorderHeight; set => FListBorderHeight = value; }

        [Published]
        public Aow.Units.AoWVScroll.TAOWVScrollBar VScrollBar { get => FVScrollBar; set => SetVScroll(value); }

        [Published]
        public int NumberVisible { get => FNumberVisible; set => FNumberVisible = value; }

        [Published]
        public int Selected { get => FSelected; set => SetIndex(value); }

        [Published]
        public int PriorityOpened { get => FPriorityOpened; set => FPriorityOpened = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnDrawItem { get => FOnDrawItem; set => FOnDrawItem = value; }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILEdit { get => FILEdit; set => SetILEdit(value); }

        [Published]
        public int ILEIndex { get => FILEIndex; set => SetILEIndex(value); }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILFrame { get => FILFrame; set => SetILFrame(value); }

        [Published]
        public int ILFIndex { get => FILFIndex; set => SetILFIndex(value); }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILTextureNormal { get => FILTextureNormal; set => SetILTNorm(value); }

        [Published]
        public int ILTnIndex { get => FILTnIndex; set => SetILTnIndex(value); }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILTextureSelected { get => FILTextureSelected; set => SetILTSel(value); }

        [Published]
        public int ILTsIndex { get => FILTsIndex; set => SetILTsIndex(value); }

        [Published]
        public int ItemHeight { get => FItemHeight; set => FItemHeight = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnOpen { get => FOnOpen; set => FOnOpen = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnClose { get => FOnClose; set => FOnClose = value; }

        // AoWComboBox.pas:58
        public static new Aow.Units.AoWComboBox.TAOWComboBox Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.AoWComboBox.TAOWComboBox o = new Aow.Units.AoWComboBox.TAOWComboBox();
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

    public static partial class AoWComboBox
    {
        // AoWComboBox.pas:561
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Interface", [typeof(Aow.Units.AoWComboBox.TAOWComboBox)]);
        }
    }
}
