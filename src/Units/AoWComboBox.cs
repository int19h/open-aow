// Translated from AoWComboBox.pas.
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
    // Fields: F114 (the mouse button went down on the field), FOpen (the list is shown), FItems,
    // FVScrollBar, F124 (the rows the open list shows), FNumberVisible, F12C (the first item
    // shown), FListBorderWidth, FListBorderHeight, FSelected, F13C (the item under the mouse, -1
    // for none), FPriorityOpened, F144 (the priority before the list was opened), FOnDrawItem, F150
    // (the width the scroll bar takes from the list), F154 (the scroll bar is visible), FILEdit,
    // FILFrame, FILTextureNormal, FILTextureSelected, FItemHeight, FOnOpen, FOnClose, FILEIndex,
    // FILFIndex, FILTnIndex, FILTsIndex, FDrawItem, FDrawItemRect (the item and its rectangle while
    // OnDrawItem runs)
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
        public TAOWComboBox(TComponent owner) : base(owner)
        {
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
            Aow.Rtl.TRect OldClip = default;
            int I = default;
            try
            {
                if (FValid && FVisible)
                {
                    if (FOnPreDraw != null)
                    {
                        FOnPreDraw(this, SurfDesc);
                    }

                    FAOWFont.Color = FColor;
                    OldClip = SurfDesc.ClipRect;
                    var call__24 = Aow.Rtl.Rtl.Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                    FILEdit.DrawILLh(FILEIndex, call__24, ref SurfDesc);
                    bool bool__25 = (FSelected > -1);
                    if (bool__25)
                    {
                        var call__26 = FItems.Count;
                        bool__25 = (call__26 > FSelected);
                    }

                    if (bool__25)
                    {
                        var call__27 = FItems[FSelected];
                        FAOWFont.SetText((FAOWWindow.FWinLeft + FWinLeft) + FBorderWidth, (FAOWWindow.FWinTop + FWinTop) + FBorderHeight, call__27, SurfDesc);
                    }

                    SurfDesc.ClipRect = OldClip;
                    if (FOpen == true)
                    {
                        var call__28 = FILFrame[FILFIndex + 1];
                        var call__29 = FILFrame[FILFIndex + 6];
                        var call__30 = FILEdit[FILEIndex];
                        var call__31 = Aow.Rtl.Rtl.Rect(FAOWWindow.FWinLeft + FWinLeft, (FAOWWindow.FWinTop + FWinTop) + call__30.Height, FWinWidth, ((call__28.Height + call__29.Height) + (FItemHeight * F124)) + FListBorderHeight);
                        FILFrame.DrawILF(FILFIndex, call__31, ref SurfDesc);
                        var call__32 = FILFrame[FILFIndex + 3];
                        var call__33 = FILFrame[FILFIndex + 4];
                        var call__34 = FILEdit[FILEIndex];
                        var call__35 = FILFrame[FILFIndex + 1];
                        var call__36 = FILFrame[FILFIndex + 3];
                        var call__37 = Aow.Rtl.Rtl.Rect((FAOWWindow.FWinLeft + FWinLeft) + call__36.Width, ((FAOWWindow.FWinTop + FWinTop) + call__34.Height) + call__35.Height, (FWinWidth - call__32.Width) - call__33.Width, (FItemHeight * F124) + FListBorderHeight);
                        FILTextureNormal.DrawILT(FILTnIndex, call__37, ref SurfDesc);
                        if ((F13C >= F12C) && (F13C < (F124 + F12C)))
                        {
                            var call__38 = FILFrame[FILFIndex + 3];
                            var call__39 = FILFrame[FILFIndex + 4];
                            var call__40 = FILEdit[FILEIndex];
                            var call__41 = FILFrame[FILFIndex + 1];
                            var call__42 = FILFrame[FILFIndex + 3];
                            var call__43 = Aow.Rtl.Rtl.Rect((FAOWWindow.FWinLeft + FWinLeft) + call__42.Width, (((FAOWWindow.FWinTop + FWinTop) + call__40.Height) + call__41.Height) + ((F13C - F12C) * FItemHeight), (FWinWidth - call__38.Width) - call__39.Width, FItemHeight);
                            FILTextureSelected.DrawILT(FILTsIndex, call__43, ref SurfDesc);
                        }

                        if ((((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth) - F150) < OldClip.Right)
                        {
                            SurfDesc.ClipRect.Right = ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth) - F150;
                        }

                        {
                            I = 0;
                            int end__44 = F124 - 1;
                            for (; I <= end__44; I += 1)
                            {
                                var call__45 = FItems.Count;
                                if (I < call__45)
                                {
                                    if ((F12C + I) == F13C)
                                    {
                                        FAOWFont.Color = FSelColor;
                                    }

                                    var call__46 = FILEdit[FILEIndex];
                                    var call__47 = FILFrame[FILFIndex + 1];
                                    var call__48 = FItems[F12C + I];
                                    FAOWFont.SetText((FAOWWindow.FWinLeft + FWinLeft) + FListBorderWidth, ((((FAOWWindow.FWinTop + FWinTop) + call__46.Height) + call__47.Height) + FListBorderHeight) + (FItemHeight * I), call__48, SurfDesc);
                                    if ((F12C + I) == F13C)
                                    {
                                        FAOWFont.Color = FColor;
                                    }

                                    FDrawItem = F12C + I;
                                    var call__49 = FILEdit[FILEIndex + 1];
                                    var call__50 = FILFrame[FILFIndex + 1];
                                    var call__51 = FILEdit[FILEIndex + 1];
                                    var call__52 = FILFrame[FILFIndex + 1];
                                    var call__53 = FILFrame[FILFIndex + 3];
                                    var call__54 = FILFrame[FILFIndex + 3];
                                    var call__55 = Aow.Rtl.Rtl.Rect((FAOWWindow.FWinLeft + FWinLeft) + call__54.Width, (((FAOWWindow.FWinTop + FWinTop) + call__51.Height) + call__52.Height) + (FItemHeight * I), ((FAOWWindow.FWinLeft + FWinLeft) + call__53.Width) + FWinWidth, (((FAOWWindow.FWinTop + FWinTop) + call__49.Height) + call__50.Height) + ((I + 1) * FItemHeight));
                                    FDrawItemRect = call__55;
                                    if (FOnDrawItem != null)
                                    {
                                        FOnDrawItem(this);
                                    }
                                }
                            }
                        }

                        SurfDesc.ClipRect = OldClip;
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

        // AoWComboBox.pas:375
        // { CheckMouseDown opens or closes the list with a click on the edit field and closes it with a
//   click outside the control }
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseDown(Button, Shift, X, Y);
            FHandled = false;
            bool bool__56 = ((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1));
            if (bool__56)
            {
                var call__57 = FILEdit[FILEIndex];
                bool__56 = (Y < ((FAOWWindow.FWinTop + FWinTop) + call__57.Height));
            }

            if (bool__56)
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
                            var call__58 = FILEdit[FILEIndex];
                            FWinHeight = call__58.Height;
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
                            var call__59 = FItems.Count;
                            if (call__59 < F124)
                            {
                                var call__60 = FItems.Count;
                                F124 = call__60;
                            }

                            var call__61 = FILEdit[FILEIndex];
                            var call__62 = FILFrame[FILFIndex + 1];
                            var call__63 = FILFrame[FILFIndex + 6];
                            FWinHeight = (((call__61.Height + FListBorderHeight) + (FItemHeight * F124)) + call__62.Height) + call__63.Height;
                            var call__64 = FItems.Count;
                            if (call__64 < (F12C + F124))
                            {
                                var call__65 = FItems.Count;
                                F12C = call__65 - F124;
                            }

                            FDrawn = false;
                            var call__66 = FILFrame[FILFIndex + 4];
                            F150 = call__66.Width;
                            F154 = false;
                            if ((FVScrollBar != null) && (F124 > 0))
                            {
                                if ((FPriority + 1) != FVScrollBar.FPriority)
                                {
                                    FVScrollBar.Priority = FPriority + 1;
                                }

                                FVScrollBar.SetFMin(0);
                                var call__67 = FItems.Count;
                                FVScrollBar.SetFMax(call__67 - 1);
                                FVScrollBar.SetFPos(F12C);
                                FVScrollBar.SetFNum(F124);
                                var call__68 = FILFrame[FILFIndex + 6];
                                FVScrollBar.WinHeight = call__68.Height + ((FItemHeight * F124) + FBorderHeight);
                                FVScrollBar.Visible = true;
                                bool bool__69 = (FVScrollBar.FVanishWhenFull == true);
                                if (bool__69)
                                {
                                    var call__70 = FItems.Count;
                                    bool__69 = ((call__70 - 1) < F124);
                                }

                                if (bool__69)
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
                                var call__71 = FILEdit[FILEIndex];
                                FVScrollBar.WinTop = call__71.Height + FWinTop;
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
                            var call__72 = FILEdit[FILEIndex];
                            FWinHeight = call__72.Height;
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
                        var call__73 = FILEdit[FILEIndex];
                        FWinHeight = call__73.Height;
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
            bool bool__74 = ((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1));
            if (bool__74)
            {
                var call__75 = FILEdit[FILEIndex];
                bool__74 = (Y < ((FAOWWindow.FWinTop + FWinTop) + call__75.Height));
            }

            if (bool__74)
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
                    bool bool__76 = (X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth));
                    if (bool__76)
                    {
                        var call__77 = FILEdit[FILEIndex];
                        bool__76 = (Y > (((FAOWWindow.FWinTop + FWinTop) + call__77.Height) - 1));
                    }

                    if (bool__76 && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
                    {
                        if (X < (((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth) - F150))
                        {
                            var call__78 = FILEdit[FILEIndex];
                            I = (((((Y - FAOWWindow.FWinTop) - FWinTop) - FBorderHeight) - call__78.Height) / FItemHeight) + F12C;
                            var call__79 = FItems.Count;
                            if (I < call__79)
                            {
                                FSelected = I;
                            }

                            FOpen = false;
                            FRedraw = false;
                            var call__80 = FILEdit[FILEIndex];
                            FWinHeight = call__80.Height;
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
                        var call__81 = FILEdit[FILEIndex];
                        FWinHeight = call__81.Height;
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
                    var call__82 = FILEdit[FILEIndex];
                    bool bool__83 = (Y >= (((FAOWWindow.FWinTop + FWinTop) + FBorderHeight) + call__82.Height));
                    if (bool__83)
                    {
                        var call__84 = FILEdit[FILEIndex];
                        bool__83 = (Y < ((((FAOWWindow.FWinTop + FWinTop) + call__84.Height) + FBorderHeight) + (FItemHeight * F124)));
                    }

                    if (bool__83)
                    {
                        var call__85 = FILEdit[FILEIndex];
                        F13C = (((((Y - FAOWWindow.FWinTop) - FWinTop) - FBorderHeight) - call__85.Height) / FItemHeight) + F12C;
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
            var call__86 = FItems.Count;
            if (Value >= call__86)
            {
                var call__87 = FItems.Count;
                Value = call__87 - 1;
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
