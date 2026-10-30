// Translated from AoWListBox.pas.
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
using Aow.Units.AoWWinGeneric;
using static Aow.Units.AoWWinGeneric.AoWWinGeneric;

namespace Aow.Units.AoWListBox
{
    public enum TLBType : byte
    {
        lbNormal = 0,
        lbBlank = 1
    }

    // Fields: FItems, FVScrollBar, F11C (the rows shown), FSelected, F124 (set to -1 by SetSize,
    // otherwise unused), FOnPreDrawItem, FOnDrawItem, F138 (the width the frame or the scroll bar
    // takes at the right), F13C (the scroll bar is visible), FILFrame, FILTextureNormal,
    // FILTextureSelected, FItemHeight, FOnDoubleClick, FTextAlign, F15C (the first item shown),
    // F160 (0: drawn with the frame and the textures, 1: text only), FILIndexFrame,
    // FILIndexTNormal, FILIndexTSelected, FPopupMenu, FDrawItem, FDrawItemRect (the item and its
    // rectangle while OnPreDrawItem and OnDrawItem run), F188 (the item under the mouse, -1 for
    // none)
    public partial class TAOWListBox : Aow.Units.AoWControl.TAOWControl
    {
        public Aow.Rtl.TStringList FItems;
        public Aow.Units.AoWVScroll.TAOWVScrollBar FVScrollBar;
        public int F11C;
        public int FSelected;
        public int F124;
        public Aow.Units.AoWComp.TAoWOnDrawItemEvent FOnPreDrawItem;
        public Aow.Units.AoWComp.TAoWOnDrawItemEvent FOnDrawItem;
        public int F138;
        public bool F13C;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILFrame;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILTextureNormal;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILTextureSelected;
        public int FItemHeight;
        public Aow.Rtl.TNotifyEvent FOnDoubleClick;
        public Aow.Units.AoWComp.TaowTextAlign FTextAlign;
        public int F15C;
        public byte F160;
        public int FILIndexFrame;
        public int FILIndexTNormal;
        public int FILIndexTSelected;
        public Aow.Units.AOWMenu.TAOWMenu FPopupMenu;
        public int FDrawItem;
        public Aow.Rtl.TRect FDrawItemRect;
        public int F188;
        // AoWListBox.pas:107
        public TAOWListBox(TComponent owner) : base(owner)
        {
            var call__1 = new Aow.Rtl.TStringList();
            FItems = call__1;
            FItems.OnChange = OnChangeText;
            F11C = 3;
            FItemHeight = 10;
        }

        // AoWListBox.pas:116
        public override void Destroy()
        {
            FItems.Destroy();
            base.Destroy();
        }

        // AoWListBox.pas:124
        // { Validate: a list box needs only its font; without all three image libraries it is drawn as
//   text only }
        public override void Validate()
        {
            if (FAOWFont != null)
            {
                if (FILFrame != null)
                {
                    var call__2 = FILFrame.CheckIL(FILIndexFrame, 8, this);
                    FILFrame = call__2;
                }

                if (FILTextureNormal != null)
                {
                    var call__3 = FILTextureNormal.CheckIL(FILIndexTNormal, 1, this);
                    FILTextureNormal = call__3;
                }

                if (FILTextureSelected != null)
                {
                    var call__4 = FILTextureSelected.CheckIL(FILIndexTSelected, 1, this);
                    FILTextureSelected = call__4;
                }

                if (((FILFrame != null) && (FILTextureNormal != null)) && (FILTextureSelected != null))
                {
                    F160 = 0;
                }
                else
                {
                    F160 = 1;
                }

                FValid = true;
            }

            base.Validate();
        }

        // AoWListBox.pas:143
        public override void SetSize()
        {
            if (FItemHeight < 1)
            {
                FItemHeight = 10;
            }

            switch (F160)
            {
                case 0:
                    var call__5 = FILFrame[FILIndexFrame + 1];
                    var call__6 = FILFrame[FILIndexFrame + 6];
                    if ((call__5.Height + call__6.Height) > FWinHeight)
                    {
                        var call__7 = FILFrame[FILIndexFrame + 1];
                        var call__8 = FILFrame[FILIndexFrame + 6];
                        FWinHeight = call__7.Height + call__8.Height;
                    }

                    var call__9 = FILFrame[FILIndexFrame + 3];
                    var call__10 = FILFrame[FILIndexFrame + 4];
                    if ((call__9.Width + call__10.Width) > FWinWidth)
                    {
                        var call__11 = FILFrame[FILIndexFrame + 3];
                        var call__12 = FILFrame[FILIndexFrame + 4];
                        FWinWidth = call__11.Width + call__12.Width;
                    }

                    var call__13 = FILFrame[FILIndexFrame + 1];
                    var call__14 = FILFrame[FILIndexFrame + 6];
                    F11C = ((FWinHeight - call__13.Height) - call__14.Height) / FItemHeight;
                    var call__15 = FILFrame[FILIndexFrame + 4];
                    F138 = call__15.Width;
                    FMasked = false;
                    break;
                case 1:
                    F11C = FWinHeight / FItemHeight;
                    F138 = 0;
                    FMasked = true;
                    break;
            }

            var call__16 = FItems.Count;
            if (call__16 < F11C)
            {
                var call__17 = FItems.Count;
                F11C = call__17;
            }

            if (F11C < 1)
            {
                F11C = 1;
            }

            var call__18 = FItems.Count;
            if (call__18 <= FSelected)
            {
                var call__19 = FItems.Count;
                FSelected = call__19 - 1;
            }

            var call__20 = FItems.Count;
            if (call__20 < (F15C + F11C))
            {
                var call__21 = FItems.Count;
                F15C = call__21 - F11C;
            }

            if (F15C < 0)
            {
                F15C = 0;
            }

            F13C = false;
            if (FVScrollBar != null)
            {
                F124 = -1;
                if (FVScrollBar.FAOWWindow != FAOWWindow)
                {
                    FVScrollBar.SetAOWWindow(FAOWWindow);
                }

                FVScrollBar.Priority = FPriority + 1;
                FVScrollBar.SetFMin(0);
                var call__22 = FItems.Count;
                FVScrollBar.SetFMax(call__22 - 1);
                FVScrollBar.SetFPos(F15C);
                FVScrollBar.SetFNum(F11C);
                FVScrollBar.WinHeight = FWinHeight;
                FVScrollBar.SetLarge(F11C - 1);
                FVScrollBar.Visible = FVisible;
                bool bool__23 = FVScrollBar.FVanishWhenFull;
                if (bool__23)
                {
                    var call__24 = FItems.Count;
                    bool__23 = ((call__24 - 1) < F11C);
                }

                if (bool__23)
                {
                    FVScrollBar.Visible = false;
                }
                else
                {
                    F138 = FVScrollBar.FWinWidth;
                }

                FVScrollBar.FSized = false;
                FVScrollBar.WinLeft = (FWinLeft + FWinWidth) - FVScrollBar.FWinWidth;
                FVScrollBar.WinTop = FWinTop;
                F13C = FVScrollBar.FVisible;
            }

            base.SetSize();
        }

        // AoWListBox.pas:386
        // { Update keeps the first item shown in step with the scroll bar }
        public override void Update()
        {
            bool bool__25 = FVisible && (FVScrollBar != null);
            if (bool__25)
            {
                var call__26 = FItems.Count;
                bool__25 = (call__26 > 0);
            }

            if (bool__25)
            {
                if (FVScrollBar.FPosition != F15C)
                {
                    F15C = FVScrollBar.FPosition;
                    var call__27 = FItems.Count;
                    if ((F15C + F11C) > call__27)
                    {
                        var call__28 = FItems.Count;
                        F15C = call__28 - F11C;
                    }

                    if (F15C < 0)
                    {
                        F15C = 0;
                    }

                    FDrawn = false;
                }
            }

            base.Update();
        }

        // AoWListBox.pas:241
        public override void SetVisible(bool Value)
        {
            base.SetVisible(Value);
            if (FVScrollBar != null)
            {
                FVScrollBar.Visible = FVisible;
            }
        }

        // AoWListBox.pas:304
        // { Draw draws the frame and the texture, then the visible items (one more than fit: the loop runs
//   to F11C, original) clipped to the inside, the selected one on its highlight texture }
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            int L = default;
            int W = default;
            int T = default;
            int H = default;
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
                    switch (F160)
                    {
                        case 0:
                            var call__29 = Aow.Rtl.Rtl.Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                            FILFrame.DrawILF(FILIndexFrame, call__29, ref SurfDesc);
                            var call__30 = FILFrame[FILIndexFrame + 1];
                            var call__31 = FILFrame[FILIndexFrame + 6];
                            var call__32 = FILFrame[FILIndexFrame + 3];
                            var call__33 = FILFrame[FILIndexFrame + 1];
                            var call__34 = FILFrame[FILIndexFrame + 3];
                            var call__35 = Aow.Rtl.Rtl.Rect((FAOWWindow.FWinLeft + FWinLeft) + call__34.Width, (FAOWWindow.FWinTop + FWinTop) + call__33.Height, (FWinWidth - call__32.Width) - F138, (FWinHeight - call__30.Height) - call__31.Height);
                            FILTextureNormal.DrawILT(FILIndexTNormal, call__35, ref SurfDesc);
                            var call__36 = FILFrame[FILIndexFrame + 3];
                            L = (FAOWWindow.FWinLeft + FWinLeft) + call__36.Width;
                            var call__37 = FILFrame[FILIndexFrame + 1];
                            T = (FAOWWindow.FWinTop + FWinTop) + call__37.Height;
                            var call__38 = FILFrame[FILIndexFrame + 1];
                            var call__39 = FILFrame[FILIndexFrame + 6];
                            H = (FWinHeight - call__38.Height) - call__39.Height;
                            var call__40 = FILFrame[FILIndexFrame + 3];
                            W = (FWinWidth - call__40.Width) - F138;
                            break;
                        default:
                            L = FAOWWindow.FWinLeft + FWinLeft;
                            T = FAOWWindow.FWinTop + FWinTop;
                            H = FWinHeight;
                            W = FWinWidth - F138;
                            break;
                    }

                    if ((L + W) < OldClip.Right)
                    {
                        SurfDesc.ClipRect.Right = L + W;
                    }

                    if ((T + H) < OldClip.Bottom)
                    {
                        SurfDesc.ClipRect.Bottom = T + H;
                    }

                    {
                        I = 0;
                        int end__41 = F11C;
                        for (; I <= end__41; I += 1)
                        {
                            var call__42 = FItems.Count;
                            if ((F15C + I) < call__42)
                            {
                                FDrawItem = F15C + I;
                                var call__43 = Aow.Rtl.Rtl.Rect(L, (FItemHeight * I) + T, L + W, ((I + 1) * FItemHeight) + T);
                                FDrawItemRect = call__43;
                                if (FDrawItemRect.Bottom > SurfDesc.ClipRect.Bottom)
                                {
                                    FDrawItemRect.Bottom = SurfDesc.ClipRect.Bottom;
                                }

                                if (FOnPreDrawItem != null)
                                {
                                    FOnPreDrawItem(this, FDrawItemRect, FDrawItem, ref SurfDesc);
                                }

                                if ((F15C + I) == FSelected)
                                {
                                    FAOWFont.Color = FSelColor;
                                    if (F160 == 0)
                                    {
                                        var call__44 = Aow.Rtl.Rtl.Rect(L, (FItemHeight * I) + T, W, FItemHeight);
                                        FILTextureSelected.DrawILT(FILIndexTSelected, call__44, ref SurfDesc);
                                    }
                                }
                                else
                                {
                                    FAOWFont.Color = FColor;
                                }

                                switch (FTextAlign)
                                {
                                    case Aow.Units.AoWComp.TaowTextAlign.taLeft:
                                        var call__45 = FItems[F15C + I];
                                        FAOWFont.SetText(FBorderWidth + L, (FBorderHeight + T) + (FItemHeight * I), call__45, SurfDesc);
                                        break;
                                    case Aow.Units.AoWComp.TaowTextAlign.taRight:
                                        var call__46 = FItems[F15C + I];
                                        var call__47 = FAOWFont.GetTextWidth(call__46);
                                        var call__48 = FItems[F15C + I];
                                        FAOWFont.SetText(((L + W) - FBorderWidth) - call__47, (FBorderHeight + T) + (FItemHeight * I), call__48, SurfDesc);
                                        break;
                                    default:
                                        var call__49 = FItems[F15C + I];
                                        var call__50 = FAOWFont.GetTextWidth(call__49);
                                        var call__51 = FItems[F15C + I];
                                        FAOWFont.SetText((L + (W / 2)) - (call__50 / 2), (FBorderHeight + T) + (FItemHeight * I), call__51, SurfDesc);
                                        break;
                                }

                                if (FOnDrawItem != null)
                                {
                                    FOnDrawItem(this, FDrawItemRect, FDrawItem, ref SurfDesc);
                                }
                            }
                        }
                    }

                    SurfDesc.ClipRect = OldClip;
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

        // AoWListBox.pas:404
        // { CheckMouseDown selects the item under the mouse unless the owner already handled the click;
//   a double click calls OnDoubleClick, the right button opens the popup menu at the mouse, moved
//   back inside the window manager when it would stick out }
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int I = default;
            base.CheckMouseDown(Button, Shift, X, Y);
            FHandled = false;
            if ((((!FAOWOwner.FHandled && (X > ((FAOWWindow.FWinLeft + FWinLeft) - 1))) && (X < (((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth) - F138))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = true;
                if (F160 == 0)
                {
                    var call__52 = FILFrame[FILIndexFrame];
                    I = ((((Y - FAOWWindow.FWinTop) - FWinTop) - call__52.Height) / FItemHeight) + F15C;
                }
                else
                {
                    I = (((Y - FAOWWindow.FWinTop) - FWinTop) / FItemHeight) + F15C;
                }

                var call__53 = FItems.Count;
                if (call__53 > I)
                {
                    if (I != FSelected)
                    {
                        FSelected = I;
                        FDrawn = false;
                        if (FOnChange != null)
                        {
                            FOnChange(this);
                        }
                    }

                    if (((Button == Aow.Rtl.TMouseButton.mbLeft) && ((TSet<Aow.Rtl.TShiftState>.Of(Aow.Rtl.TShiftState.ssDouble) & Shift) == TSet<Aow.Rtl.TShiftState>.Of(Aow.Rtl.TShiftState.ssDouble))) && (FOnDoubleClick != null))
                    {
                        FOnDoubleClick(this);
                    }

                    if (((FSelected > -1) && (Button == Aow.Rtl.TMouseButton.mbRight)) && (FPopupMenu != null))
                    {
                        FPopupMenu.SetPopupOwner(this);
                        FPopupMenu.WinLeft = X;
                        FPopupMenu.WinTop = Y;
                        if (FPopupMenu.WinManager != null)
                        {
                            if (FPopupMenu.FHidden)
                            {
                                if ((FPopupMenu.FMenuLeft + FPopupMenu.WinWidth) > FPopupMenu.WinManager.WinWidth)
                                {
                                    FPopupMenu.WinLeft = X - FPopupMenu.WinWidth;
                                }

                                if ((FPopupMenu.FMenuTop + FPopupMenu.WinHeight) > FPopupMenu.WinManager.WinHeight)
                                {
                                    FPopupMenu.WinTop = Y - FPopupMenu.WinHeight;
                                }
                            }
                            else
                            {
                                if ((FPopupMenu.WinLeft + FPopupMenu.WinWidth) > FPopupMenu.WinManager.WinWidth)
                                {
                                    FPopupMenu.WinLeft = X - FPopupMenu.WinWidth;
                                }

                                if ((FPopupMenu.WinTop + FPopupMenu.WinHeight) > FPopupMenu.WinManager.WinHeight)
                                {
                                    FPopupMenu.WinTop = Y - FPopupMenu.WinHeight;
                                }
                            }
                        }

                        FPopupMenu.Visible = true;
                    }
                }

                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }
            }
        }

        // AoWListBox.pas:460
        public override void CheckMouseUp(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < (((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth) - F138))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = true;
                if (FOnMouseUp != null)
                {
                    FOnMouseUp(this, Button, Shift, X, Y);
                }
            }
        }

        // AoWListBox.pas:475
        // { CheckMouseMove tracks the item under the mouse }
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseMove(Shift, X, Y);
            FHandled = false;
            if ((((X < (FAOWWindow.FWinLeft + FWinLeft)) || (X > ((((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth) - F138) - 1))) || (Y < (FAOWWindow.FWinTop + FWinTop))) || (Y > (((FAOWWindow.FWinTop + FWinTop) + FWinHeight) - 1)))
            {
                F188 = -1;
            }
            else
            {
                FHandled = true;
                if (F160 == 0)
                {
                    var call__54 = FILFrame[FILIndexFrame];
                    F188 = ((((Y - FAOWWindow.FWinTop) - FWinTop) - call__54.Height) / FItemHeight) + F15C;
                }
                else
                {
                    F188 = (((Y - FAOWWindow.FWinTop) - FWinTop) / FItemHeight) + F15C;
                }

                if (FOnMouseMove != null)
                {
                    FOnMouseMove(this, Shift, X, Y);
                }
            }
        }

        // AoWListBox.pas:497
        public override void CheckKeyPress(ref byte Key)
        {
            FHandled = false;
        }

        // AoWListBox.pas:275
        public virtual void SetILFrame(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILFrame)
            {
                FValid = false;
                FILFrame = Value;
            }
        }

        // AoWListBox.pas:284
        public virtual void SetILTNorm(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILTextureNormal)
            {
                FValid = false;
                FILTextureNormal = Value;
            }
        }

        // AoWListBox.pas:293
        public virtual void SetILTSel(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILTextureSelected)
            {
                FValid = false;
                FILTextureSelected = Value;
            }
        }

        // AoWListBox.pas:202
        public void SetFStrings(Aow.Rtl.TStringList Value)
        {
            FItems.Clear();
            FItems.AddStrings(Value);
        }

        // AoWListBox.pas:208
        public void SetIndex(int Value)
        {
            var call__55 = FItems.Count;
            if (Value >= call__55)
            {
                var call__56 = FItems.Count;
                Value = call__56 - 1;
            }

            if (Value != FSelected)
            {
                FSelected = Value;
                FDrawn = false;
            }
        }

        // AoWListBox.pas:219
        public void SetListOff(int Value)
        {
            var call__57 = FItems.Count;
            if (Value >= call__57)
            {
                var call__58 = FItems.Count;
                Value = call__58 - 1;
            }

            if (Value < 0)
            {
                Value = 0;
            }

            if (Value != F15C)
            {
                F15C = Value;
                FDrawn = false;
            }
        }

        // AoWListBox.pas:232
        public void SetVScroll(Aow.Units.AoWVScroll.TAOWVScrollBar Value)
        {
            FVScrollBar = Value;
            if (Value != null)
            {
                Value.Priority = FPriority;
            }

            Priority = FPriority;
            FSized = false;
        }

        // AoWListBox.pas:248
        public void SetILFIndex(int Value)
        {
            if (Value != FILIndexFrame)
            {
                FILIndexFrame = Value;
                FValid = false;
            }
        }

        // AoWListBox.pas:257
        public void SetILTnIndex(int Value)
        {
            if (Value != FILIndexTNormal)
            {
                FILIndexTNormal = Value;
                FValid = false;
            }
        }

        // AoWListBox.pas:266
        public void SetILTsIndex(int Value)
        {
            if (Value != FILIndexTSelected)
            {
                FILIndexTSelected = Value;
                FValid = false;
            }
        }

        public int ListOffset { get => F15C; set => SetListOff(value); }

        [Published]
        public Aow.Units.ImageLib.TImageLibraryFont AOWFont { get => FAOWFont; set => FAOWFont = value; }

        [Published]
        public Aow.Rtl.TStringList Items { get => FItems; set => SetFStrings(value); }

        [Published]
        public int BorderWidth { get => FBorderWidth; set => FBorderWidth = value; }

        [Published]
        public int BorderHeight { get => FBorderHeight; set => FBorderHeight = value; }

        [Published]
        public Aow.Units.AoWVScroll.TAOWVScrollBar VScrollBar { get => FVScrollBar; set => SetVScroll(value); }

        [Published]
        public Aow.Rtl.TNotifyEvent OnDoubleClick { get => FOnDoubleClick; set => FOnDoubleClick = value; }

        [Published]
        public int Selected { get => FSelected; set => SetIndex(value); }

        [Published]
        public Aow.Units.AoWComp.TAoWOnDrawItemEvent OnDrawItem { get => FOnDrawItem; set => FOnDrawItem = value; }

        [Published]
        public Aow.Units.AoWComp.TAoWOnDrawItemEvent OnPreDrawItem { get => FOnPreDrawItem; set => FOnPreDrawItem = value; }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILFrame { get => FILFrame; set => SetILFrame(value); }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILTextureNormal { get => FILTextureNormal; set => SetILTNorm(value); }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILTextureSelected { get => FILTextureSelected; set => SetILTSel(value); }

        [Published]
        public int ILIndexFrame { get => FILIndexFrame; set => SetILFIndex(value); }

        [Published]
        public int ILIndexTNormal { get => FILIndexTNormal; set => SetILTnIndex(value); }

        [Published]
        public int ILIndexTSelected { get => FILIndexTSelected; set => SetILTsIndex(value); }

        [Published]
        public int ItemHeight { get => FItemHeight; set => FItemHeight = value; }

        [Published]
        public Aow.Units.AOWMenu.TAOWMenu PopupMenu { get => FPopupMenu; set => FPopupMenu = value; }

        [Published]
        public Aow.Units.AoWComp.TaowTextAlign TextAlign { get => FTextAlign; set => FTextAlign = value; }
    }

    public static partial class AoWListBox
    {
        // AoWListBox.pas:502
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Interface", [typeof(Aow.Units.AoWListBox.TAOWListBox)]);
        }
    }
}
