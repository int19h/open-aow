// Generated from AOWMenu.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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

namespace Aow.Units.AOWMenu
{
    // { The AOWMenu unit of aowInt.dpl: TAOWMenu, a pop-up menu window. It belongs to an item of a menu
//   bar (OwnerMenuBar and OwnerItem) or of another menu (OwnerMenu: a submenu), places itself under
//   the bar's item when it is shown, highlights the item under the mouse and shows the submenus of
//   the highlighted item after SubMenuShowDelay updates. }
    // { AOWMenu.TAOWMenu (instance size 596). Fields: +$1EC FPopupOwner, +$1F0 FILIndexTSelected,
//     +$1F4 FMenuLeft, +$1F8 FMenuTop (SetWinLeft and SetWinTop store the position here while the
//     menu is hidden), +$1FC FShowPending (SetVisible was called while hidden), +$200 FSelectedItem
//     (the item clicked last, -1 for none; read by OnSelect), +$204 FILTextureSelected, +$208
//     FItems, +$20C FItemTypes (never created: SetITStrings fails, original bug), +$210 FOnDrawItem,
//     +$218 FItemHeight, +$21C FOwnerMenuBar, +$220 FOwnerMenu, +$224 FSubMenuShowDelay, +$228
//     FOnSelect, +$230 FHighlighted (the item under the mouse, -1 for none), +$234 FSubMenuItem (the
//     item whose submenus are shown), +$238 FSubMenus (sorted by priority), +$23C FShowDelay
//     (updates since the highlight moved), +$240 FDrawItem, +$244 FDrawItemRect (the item drawn
//     last, for OnDrawItem).
//     VMT (new): SetMenu, SetMenuBar, SetILTSel }
    public partial class TAOWMenu : Aow.Units.AoWWinGeneric.TAOWGenericWindow
    {
        public Aow.Units.AoWComp.TAoWComponent FPopupOwner;
        public int FILIndexTSelected;
        public int FMenuLeft;
        public int FMenuTop;
        public bool FShowPending;
        public int FSelectedItem;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILTextureSelected;
        public Aow.Rtl.TStringList FItems;
        public Aow.Rtl.TStringList FItemTypes;
        public Aow.Rtl.TNotifyEvent FOnDrawItem;
        public int FItemHeight;
        public Aow.Units.AOWMenuBar.TAOWMenuBar FOwnerMenuBar;
        public Aow.Units.AOWMenu.TAOWMenu FOwnerMenu;
        public int FSubMenuShowDelay;
        public Aow.Rtl.TNotifyEvent FOnSelect;
        public int FHighlighted;
        public int FSubMenuItem;
        public Aow.Rtl.TList FSubMenus;
        public int FShowDelay;
        public int FDrawItem;
        public Aow.Rtl.TRect FDrawItemRect;
        // AOWMenu.pas:102
        // { TAOWMenu }
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            base.Create__init(AOwner);
            var call__1 = new Aow.Rtl.TStringList();
            FItems = call__1;
            FItems.OnChange = OnChangeText;
            FHighlighted = -1;
            FSelectedItem = -1;
            FSubMenuItem = -1;
            FItemHeight = 10;
            var call__2 = new Aow.Rtl.TList();
            FSubMenus = call__2;
            FVisible = false;
        }

        // AOWMenu.pas:117
        // { Destroy removes the menu from the lists of its owner menu and menu bar and detaches its
//   submenus }
        public override void Destroy()
        {
            int I = default;
            Aow.Units.AOWMenu.TAOWMenu M = default;
            I = 0;
            if (FOwnerMenu != null)
            {
                try
                {
                    while (I < FOwnerMenu.FSubMenus.Count)
                    {
                        var call__3 = FOwnerMenu.FSubMenus[I];
                        M = (Aow.Units.AOWMenu.TAOWMenu)call__3;
                        if (M == this)
                        {
                            FOwnerMenu.FSubMenus.Delete(I);
                            I = FOwnerMenu.FSubMenus.Count;
                        }

                        I += 1;
                    }
                }
                catch (System.Exception)
                {
                }
            }

            I = 0;
            if (FOwnerMenuBar != null)
            {
                try
                {
                    while (I < FOwnerMenuBar.FMenus.Count)
                    {
                        var call__4 = FOwnerMenuBar.FMenus[I];
                        M = (Aow.Units.AOWMenu.TAOWMenu)call__4;
                        if (M == this)
                        {
                            FOwnerMenuBar.FMenus.Delete(I);
                            I = FOwnerMenuBar.FMenus.Count;
                        }

                        I += 1;
                    }
                }
                catch (System.Exception)
                {
                }
            }

            FItems.Destroy();
            {
                I = 0;
                int end__5 = FSubMenus.Count - 1;
                for (; I <= end__5; I += 1)
                {
                    var call__6 = FSubMenus[I];
                    ((Aow.Units.AOWMenu.TAOWMenu)call__6).DetachMenu();
                }
            }

            FSubMenus.Destroy();
            base.Destroy();
        }

        // AOWMenu.pas:159
        public override void Validate()
        {
            if (FILFrame != null)
            {
                var call__7 = FILFrame.CheckIL(FILIndexFrame, 8, this);
                FILFrame = call__7;
            }

            if (FILTexture != null)
            {
                var call__8 = FILTexture.CheckIL(FILIndexTexture, 1, this);
                FILTexture = call__8;
            }

            if (FILTextureSelected != null)
            {
                var call__9 = FILTextureSelected.CheckIL(FILIndexTSelected, 1, this);
                FILTextureSelected = call__9;
            }

            if (((((FAOWOwner != null) && (FILFrame != null)) && (FILTexture != null)) && (FILTextureSelected != null)) && (FAOWFont != null))
            {
                FValid = true;
            }

            base.Validate();
        }

        // AOWMenu.pas:381
        // { SetSize: the height fits the items between the top and bottom frame pieces; the menu is placed
//   under its menu bar item and inside the window manager's borders; the frame rectangle is set
//   for masked menus }
        public override void SetSize()
        {
            Aow.Units.AOWMenuBar.TAOWMenuBar MB = default;
            Aow.Units.AoWBaseWin.TAOWBaseWindow W = default;
            if (FItemHeight == 0)
            {
                var call__10 = FILFrame[FILIndexFrame + 3];
                FItemHeight = call__10.Height;
            }

            if (FWinHeight == 0)
            {
                var call__11 = FILFrame[FILIndexFrame];
                var call__12 = FILFrame[FILIndexFrame + 7];
                var call__13 = FItems.Count;
                FWinHeight = (call__11.Height + call__12.Height) + (call__13 * FItemHeight);
            }

            var call__14 = FILFrame[FILIndexFrame];
            var call__15 = FILFrame[FILIndexFrame + 2];
            if ((call__14.Width + call__15.Width) > FWinWidth)
            {
                var call__16 = FILFrame[FILIndexFrame];
                var call__17 = FILFrame[FILIndexFrame + 2];
                FWinWidth = call__16.Width + call__17.Width;
            }

            var call__18 = FILFrame[FILIndexFrame];
            var call__19 = FItems.Count;
            var call__20 = FILFrame[FILIndexFrame + 5];
            if (((call__18.Height + (call__19 * FItemHeight)) + call__20.Height) > FWinHeight)
            {
                var call__21 = FILFrame[FILIndexFrame];
                var call__22 = FItems.Count;
                var call__23 = FILFrame[FILIndexFrame + 5];
                FWinHeight = (call__21.Height + (call__22 * FItemHeight)) + call__23.Height;
            }

            if ((FWinManager != null) && (FOwnerMenuBar != null))
            {
                MB = FOwnerMenuBar;
                W = MB.FAOWWindow;
                if (W != null)
                {
                    var call__24 = MB.FItems.Count;
                    FWinLeft = (W.FWinLeft + MB.FWinLeft) + ((MB.FWinWidth / call__24) * FOwnerMenuBar.FSelected);
                    FWinTop = (FOwnerMenuBar.FAOWWindow.FWinTop + FOwnerMenuBar.FWinTop) + FOwnerMenuBar.FWinHeight;
                    if (FWinManager.FWinWidth < (FWinLeft + FWinWidth))
                    {
                        FWinLeft = FWinManager.FWinWidth - FWinWidth;
                    }

                    if ((FWinTop + FWinHeight) > FWinManager.FWinHeight)
                    {
                        FWinTop = (FOwnerMenuBar.FAOWWindow.FWinTop + FOwnerMenuBar.FWinTop) - FWinHeight;
                    }
                }
            }

            if (FUseManagerBorders)
            {
                if (FWinManager.FBorderLeft > FWinLeft)
                {
                    FWinLeft = FWinManager.FBorderLeft;
                }

                if ((FWinManager.FWinWidth - FWinManager.FBorderRight) < (FWinLeft + FWinWidth))
                {
                    FWinLeft = (FWinManager.FWinWidth - FWinManager.FBorderRight) - FWinWidth;
                }

                if (FWinManager.FBorderTop > FWinTop)
                {
                    FWinTop = FWinManager.FBorderTop;
                }

                if ((FWinManager.FWinHeight - FWinManager.FBorderBottom) < (FWinTop + FWinHeight))
                {
                    FWinTop = (FWinManager.FWinHeight - FWinManager.FBorderBottom) - FWinHeight;
                }
            }

            if (FMasked)
            {
                var call__25 = FILFrame[FILIndexFrame + 3];
                FFrameLeft = call__25.Width;
                var call__26 = FILFrame[FILIndexFrame + 4];
                FFrameRight = FWinWidth - call__26.Width;
                var call__27 = FILFrame[FILIndexFrame + 1];
                FFrameTop = call__27.Height;
                var call__28 = FILFrame[FILIndexFrame + 6];
                FFrameBottom = FWinHeight - call__28.Height;
            }

            if (FDoubleBuffered && (FBackSurface != null))
            {
                FBackSurface.Resize(FWinWidth, FWinHeight);
                if (FMasked)
                {
                    FBackSurface.Cls(FMaskColor);
                }
            }

            base.SetSize();
        }

        // AOWMenu.pas:465
        // { Update: after the highlight rested SubMenuShowDelay updates on an item, the submenus of the
//   previous item are hidden and those of the highlighted item are shown beside it }
        public override void Update()
        {
            int I = default;
            Aow.Units.AOWMenu.TAOWMenu M = default;
            if (FValid && FVisible)
            {
                base.Update();
                if (FHighlighted > -1)
                {
                    if (FShowDelay < FSubMenuShowDelay)
                    {
                        FShowDelay += 1;
                    }
                    else
                    {
                        if (FHighlighted != FSubMenuItem)
                        {
                            {
                                I = 0;
                                int end__29 = FSubMenus.Count - 1;
                                for (; I <= end__29; I += 1)
                                {
                                    var call__30 = FSubMenus[I];
                                    M = (Aow.Units.AOWMenu.TAOWMenu)call__30;
                                    if (M.FOwnerItem == FSubMenuItem)
                                    {
                                        M.SetVisible(false);
                                    }
                                }
                            }
                        }

                        {
                            I = 0;
                            int end__31 = FSubMenus.Count - 1;
                            for (; I <= end__31; I += 1)
                            {
                                var call__32 = FSubMenus[I];
                                M = (Aow.Units.AOWMenu.TAOWMenu)call__32;
                                if (M.FOwnerItem == FHighlighted)
                                {
                                    var call__33 = FILFrame[FILIndexFrame + 2];
                                    M.SetWinLeft((FWinLeft + FWinWidth) - call__33.Width);
                                    var call__34 = FILFrame[FILIndexFrame];
                                    M.SetWinTop((FWinTop + call__34.Height) + (FItemHeight * FHighlighted));
                                    M.SetVisible(true);
                                    FSubMenuItem = FHighlighted;
                                }
                            }
                        }
                    }
                }
            }
        }

        // AOWMenu.pas:173
        public override void SetWinLeft(int Value)
        {
            if (FHidden)
            {
                FMenuLeft = Value;
            }
            else
            {
                FWinLeft = Value;
            }

            FDrawn = false;
        }

        // AOWMenu.pas:182
        public override void SetWinTop(int Value)
        {
            if (FHidden)
            {
                FMenuTop = Value;
            }
            else
            {
                FWinTop = Value;
            }

            FDrawn = false;
        }

        // AOWMenu.pas:203
        // { SetVisible: a menu shown under a menu bar places itself under the bar's selected item, kept
//   inside the window manager's screen; hiding it clears the bar's selection }
        public override void SetVisible(bool Value)
        {
            Aow.Units.AOWMenuBar.TAOWMenuBar Bar = default;
            Aow.Units.AoWBaseWin.TAOWBaseWindow W = default;
            Aow.Units.AOWMenuBar.TAOWMenuBar MB = default;
            // { Tried Self/menu identities and receiver scope; owner temporaries still differ. }
            if (Value != FVisible)
            {
                if (FHidden)
                {
                    FShowPending = true;
                }
                else
                {
                    FVisible = Value;
                    if (FVisible == true)
                    {
                        FHighlighted = -1;
                        if ((FWinManager != null) && (FOwnerMenuBar != null))
                        {
                            MB = FOwnerMenuBar;
                            W = MB.FAOWWindow;
                            if (W != null)
                            {
                                var call__35 = MB.FItems.Count;
                                FWinLeft = (W.FWinLeft + MB.FWinLeft) + ((MB.FWinWidth / call__35) * FOwnerMenuBar.FSelected);
                                FWinTop = (FOwnerMenuBar.FAOWWindow.FWinTop + FOwnerMenuBar.FWinTop) + FOwnerMenuBar.FWinHeight;
                                if (FWinManager.FWinWidth < (FWinLeft + FWinWidth))
                                {
                                    FWinLeft = FWinManager.FWinWidth - FWinWidth;
                                }

                                if ((FWinTop + FWinHeight) > FWinManager.FWinHeight)
                                {
                                    FWinTop = (FOwnerMenuBar.FAOWWindow.FWinTop + FOwnerMenuBar.FWinTop) - FWinHeight;
                                }
                            }
                        }

                        FDrawn = false;
                        if (!FHidden)
                        {
                            FBackRect.Right = FWinWidth;
                            FBackRect.Bottom = FWinHeight;
                            FBackRect.Top = FWinTop;
                            FBackRect.Left = FWinLeft;
                        }

                        FHidden = false;
                        FShowPending = false;
                        if (FOnShow != null)
                        {
                            FOnShow(this);
                        }
                    }
                    else
                    {
                        Bar = FOwnerMenuBar;
                        if (Bar != null)
                        {
                            Bar.FSelected = -1;
                            Bar.FDrawn = false;
                        }

                        FHidden = true;
                        if (FOnHide != null)
                        {
                            FOnHide(this);
                        }
                    }
                }
            }

            if (FVisible)
            {
                if ((!FValid || !FAligned) || !FSized)
                {
                    Setup();
                }

                ClearBltList();
                ClearDrawList();
                FDrawn = false;
                SetPriority(FPriority);
            }
        }

        // AOWMenu.pas:501
        // { Draw: the frame, the texture, and the items; the highlighted item on the selected texture }
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // AOWMenu.pas:555
        // { CheckMouseDown: the controls of the menu first; a click on an item selects it (OnSelect) and
//   shows its submenus; a click outside hides the menu unless a submenu took it }
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            bool Show = default;
            bool Keep = default;
            Aow.Units.AOWMenu.TAOWMenu M = default;
            FHandled = false;
            FRedraw = false;
            FSelectedItem = -1;
            Keep = true;
            if (FVisible == true)
            {
                {
                    I = FChildren.Count - 1;
                    byte end__36 = 0;
                    for (; I >= end__36; I -= 1)
                    {
                        var call__37 = FChildren[I];
                        C = (Aow.Units.AoWComp.TAoWComponent)call__37;
                        if ((FHandled == false) && C.FCaptureInput)
                        {
                            C.CheckMouseDown(Button, Shift, X, Y);
                        }

                        if (C.FHandled == true)
                        {
                            FHandled = true;
                        }
                    }
                }

                Show = FVisible;
                if ((((X > (FWinLeft - 1)) && (X < (FWinLeft + FWinWidth))) && (Y > (FWinTop - 1))) && (Y < (FWinTop + FWinHeight)))
                {
                    if (FHandled == false)
                    {
                        FRedraw = true;
                        FHandled = true;
                        if (FHighlighted > -1)
                        {
                            Show = false;
                            {
                                I = 0;
                                int end__38 = FSubMenus.Count - 1;
                                for (; I <= end__38; I += 1)
                                {
                                    var call__39 = FSubMenus[I];
                                    M = (Aow.Units.AOWMenu.TAOWMenu)call__39;
                                    if (M.FHandled && M.FVisible)
                                    {
                                        Show = true;
                                    }

                                    if ((M.FOwnerItem == FHighlighted) && (M.FHandled == false))
                                    {
                                        var call__40 = FILFrame[FILIndexFrame + 2];
                                        M.SetWinLeft((FWinLeft + FWinWidth) - call__40.Width);
                                        var call__41 = FILFrame[FILIndexFrame];
                                        M.SetWinTop((FWinTop + call__41.Height) + (FItemHeight * FHighlighted));
                                        M.SetVisible(true);
                                        Show = true;
                                        FRedraw = true;
                                        FSubMenuItem = FHighlighted;
                                    }
                                }
                            }

                            FSelectedItem = FHighlighted;
                            if (FOnSelect != null)
                            {
                                FOnSelect(this);
                            }
                        }
                    }
                }
                else
                {
                    Show = false;
                    {
                        I = 0;
                        int end__42 = FSubMenus.Count - 1;
                        for (; I <= end__42; I += 1)
                        {
                            var call__43 = FSubMenus[I];
                            M = (Aow.Units.AOWMenu.TAOWMenu)call__43;
                            if (M.FHandled == true)
                            {
                                if (M.FVisible == true)
                                {
                                    Show = true;
                                    FRedraw = true;
                                }
                                else
                                {
                                    Keep = false;
                                }
                            }

                            if (!Show)
                            {
                                FSubMenuItem = -1;
                            }

                            if (Keep == true)
                            {
                                ;
                            }
                        }
                    }
                }

                SetVisible(Show);
                if (FHandled && (FOnMouseDown != null))
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }
            }
        }

        // AOWMenu.pas:634
        public override void CheckMouseUp(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            FHandled = false;
            if (FVisible == true)
            {
                {
                    I = FChildren.Count - 1;
                    byte end__44 = 0;
                    for (; I >= end__44; I -= 1)
                    {
                        var call__45 = FChildren[I];
                        C = (Aow.Units.AoWComp.TAoWComponent)call__45;
                        if (((FHandled == false) || (C.FRedraw == true)) && C.FCaptureInput)
                        {
                            C.CheckMouseUp(Button, Shift, X, Y);
                        }

                        if (C.FHandled == true)
                        {
                            FHandled = true;
                        }
                    }
                }

                if ((((X > (FWinLeft - 1)) && (X < (FWinLeft + FWinWidth))) && (Y > (FWinTop - 1))) && (Y < (FWinTop + FWinHeight)))
                {
                    FHandled = true;
                    if (FOnMouseUp != null)
                    {
                        FOnMouseUp(this, Button, Shift, X, Y);
                    }
                }
            }
        }

        // AOWMenu.pas:662
        // { CheckMouseMove highlights the item under the mouse; when the highlight moves, the submenus of
//   the old item are hidden (by the item index into the submenu list: original bug) }
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Units.AoWComp.TAoWComponent C = default;
            int OldHighlighted = default;
            int I = default;
            // { Keep the child on the stack and I in a register. }
            FHandled = false;
            FInResizeArea = true;
            if (FVisible == true)
            {
                FRedraw = true;
                OldHighlighted = FHighlighted;
                FCursor = -2;
                {
                    I = FChildren.Count - 1;
                    byte end__46 = 0;
                    for (; I >= end__46; I -= 1)
                    {
                        var call__47 = FChildren[I];
                        C = (Aow.Units.AoWComp.TAoWComponent)call__47;
                        if ((((FHandled == false) || (C.FRedraw == true)) || (C.FToolTipCount > 0)) && C.FCaptureInput)
                        {
                            C.CheckMouseMove(Shift, X, Y);
                        }

                        if (C.FHandled == true)
                        {
                            FHandled = true;
                        }
                    }
                }

                FHandled = true;
                I = -1;
                var call__48 = FILFrame[FILIndexFrame];
                bool bool__49 = (X > ((FWinLeft + call__48.Width) - 1));
                if (bool__49)
                {
                    var call__50 = FILFrame[FILIndexFrame + 7];
                    bool__49 = (X < ((FWinLeft + FWinWidth) - call__50.Width));
                }

                bool bool__51 = bool__49;
                if (bool__51)
                {
                    var call__52 = FILFrame[FILIndexFrame];
                    bool__51 = (Y > ((FWinTop + call__52.Height) - 1));
                }

                bool bool__53 = bool__51;
                if (bool__53)
                {
                    var call__54 = FILFrame[FILIndexFrame + 7];
                    bool__53 = (Y < ((FWinTop + FWinHeight) - call__54.Height));
                }

                if (bool__53)
                {
                    var call__55 = FILFrame[FILIndexFrame];
                    I = ((Y - FWinTop) - call__55.Height) / FItemHeight;
                    if (FOnMouseMove != null)
                    {
                        FOnMouseMove(this, Shift, X, Y);
                    }
                }

                bool bool__56 = (I > -2);
                if (bool__56)
                {
                    var call__57 = FItems.Count;
                    bool__56 = (I < call__57);
                }

                if (bool__56)
                {
                    FHighlighted = I;
                }

                if (I == -1)
                {
                    if (FSubMenuItem > -1)
                    {
                        {
                            I = 0;
                            int end__58 = FSubMenus.Count - 1;
                            for (; I <= end__58; I += 1)
                            {
                                {
                                    var w__59 = (Aow.Units.AoWComp.TAoWComponent)FSubMenus[I];
                                    if (w__59.FHandled == true)
                                    {
                                        this.FHighlighted = w__59.FOwnerItem;
                                    }
                                }
                            }
                        }
                    }
                }

                if (OldHighlighted != FHighlighted)
                {
                    FShowDelay = 0;
                    if ((OldHighlighted > -1) && (FSubMenus.Count > OldHighlighted))
                    {
                        var call__60 = FSubMenus[OldHighlighted];
                        ((Aow.Units.AOWMenu.TAOWMenu)call__60).SetVisible(false);
                    }

                    FDrawn = false;
                    if (FOnChange != null)
                    {
                        FOnChange(this);
                    }
                }
            }
        }

        // AOWMenu.pas:714
        public override void CheckKeyPress(ref byte Key)
        {
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            FHandled = false;
            {
                I = 0;
                int end__61 = FChildren.Count - 1;
                for (; I <= end__61; I += 1)
                {
                    var call__62 = FChildren[I];
                    C = (Aow.Units.AoWComp.TAoWComponent)call__62;
                    C.CheckKeyPress(ref Key);
                    if (C.FHandled == true)
                    {
                        FHandled = true;
                    }
                }
            }

            if (FOnKeyPress != null)
            {
                FOnKeyPress(this, ref Key);
            }
        }

        // AOWMenu.pas:731
        public override void CheckKeyDown(ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            FHandled = false;
            {
                I = 0;
                int end__63 = FChildren.Count - 1;
                for (; I <= end__63; I += 1)
                {
                    var call__64 = FChildren[I];
                    C = (Aow.Units.AoWComp.TAoWComponent)call__64;
                    C.CheckKeyDown(ref Key, Shift);
                    if (C.FHandled == true)
                    {
                        FHandled = true;
                    }
                }
            }

            if (FOnKeyDown != null)
            {
                FOnKeyDown(this, ref Key, Shift);
            }
        }

        // AOWMenu.pas:196
        public override void DetachMenuBar()
        {
            FOwnerMenuBar = null;
        }

        // AOWMenu.pas:271
        // { SetMenu moves the menu from the submenu list of its old owner menu to the list of the new one,
//   in priority order }
        public virtual void SetMenu(Aow.Units.AOWMenu.TAOWMenu Value)
        {
            int I = default;
            int NewIndex = default;
            Aow.Units.AOWMenu.TAOWMenu M = default;
            if (Value != FOwnerMenu)
            {
                if (FOwnerMenu != null)
                {
                    I = 0;
                    while (I < FOwnerMenu.FSubMenus.Count)
                    {
                        var call__65 = FOwnerMenu.FSubMenus[I];
                        M = (Aow.Units.AOWMenu.TAOWMenu)call__65;
                        if (M == this)
                        {
                            FOwnerMenu.FSubMenus.Delete(I);
                            I = FOwnerMenu.FSubMenus.Count;
                        }

                        I += 1;
                    }
                }

                FOwnerMenu = Value;
                if ((FOwnerMenu != this) && (FOwnerMenu != null))
                {
                    I = 0;
                    NewIndex = -1;
                    while (I < FOwnerMenu.FSubMenus.Count)
                    {
                        var call__66 = FOwnerMenu.FSubMenus[I];
                        M = (Aow.Units.AOWMenu.TAOWMenu)call__66;
                        if (M.FPriority > FPriority)
                        {
                            NewIndex = I;
                            I = FOwnerMenu.FSubMenus.Count;
                        }

                        I += 1;
                    }

                    if (NewIndex == -1)
                    {
                        FOwnerMenu.FSubMenus.Add(this);
                    }
                    else
                    {
                        FOwnerMenu.FSubMenus.Insert(NewIndex, this);
                    }
                }
            }
        }

        // AOWMenu.pas:316
        // { SetMenuBar: the same for the menu list of the menu bar }
        public virtual void SetMenuBar(Aow.Units.AOWMenuBar.TAOWMenuBar Value)
        {
            int I = default;
            int NewIndex = default;
            Aow.Units.AOWMenu.TAOWMenu M = default;
            if (Value != FOwnerMenuBar)
            {
                if (FOwnerMenuBar != null)
                {
                    I = 0;
                    while (I < FOwnerMenuBar.FMenus.Count)
                    {
                        var call__67 = FOwnerMenuBar.FMenus[I];
                        M = (Aow.Units.AOWMenu.TAOWMenu)call__67;
                        if (M == this)
                        {
                            FOwnerMenuBar.FMenus.Delete(I);
                            I = FOwnerMenuBar.FMenus.Count;
                        }

                        I += 1;
                    }
                }

                FOwnerMenuBar = Value;
                if (FOwnerMenuBar != null)
                {
                    I = 0;
                    NewIndex = -1;
                    while (I < FOwnerMenuBar.FMenus.Count)
                    {
                        var call__68 = FOwnerMenuBar.FMenus[I];
                        M = (Aow.Units.AOWMenu.TAOWMenu)call__68;
                        if (M.FPriority > FPriority)
                        {
                            NewIndex = I;
                            I = FOwnerMenuBar.FMenus.Count;
                        }

                        I += 1;
                    }

                    if (NewIndex == -1)
                    {
                        FOwnerMenuBar.FMenus.Add(this);
                    }
                    else
                    {
                        FOwnerMenuBar.FMenus.Insert(NewIndex, this);
                    }
                }
            }
        }

        // AOWMenu.pas:360
        public virtual void SetILTSel(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILTextureSelected)
            {
                FValid = false;
                FILTextureSelected = Value;
            }
        }

        // AOWMenu.pas:191
        public void DetachMenu()
        {
            FOwnerMenu = null;
        }

        // AOWMenu.pas:369
        public void SetILTsIndex(int Value)
        {
            if (Value != FILIndexTSelected)
            {
                FILIndexTSelected = Value;
                FValid = false;
            }
        }

        // AOWMenu.pas:440
        public void SetFStrings(Aow.Rtl.TStringList Value)
        {
            int I = default;
            FItems.Clear();
            {
                I = 0;
                int end__69 = Value.Count - 1;
                for (; I <= end__69; I += 1)
                {
                    var call__70 = Value[I];
                    FItems.Add(call__70);
                }
            }
        }

        // AOWMenu.pas:449
        public void SetITStrings(Aow.Rtl.TStringList Value)
        {
            int I = default;
            FItemTypes.Clear();
            {
                I = 0;
                int end__71 = Value.Count - 1;
                for (; I <= end__71; I += 1)
                {
                    var call__72 = Value[I];
                    FItemTypes.Add(call__72);
                }
            }
        }

        // AOWMenu.pas:458
        public void SetPopupOwner(Aow.Units.AoWComp.TAoWComponent Value)
        {
            FPopupOwner = Value;
        }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILTextureNormal { get => FILTexture; set => SetILTexture(value); }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILTextureSelected { get => FILTextureSelected; set => SetILTSel(value); }

        [Published]
        public new int ILIndexFrame { get => FILIndexFrame; set => FILIndexFrame = value; }

        [Published]
        public int ILIndexTNormal { get => FILIndexTexture; set => SetILTIndex(value); }

        [Published]
        public int ILIndexTSelected { get => FILIndexTSelected; set => SetILTsIndex(value); }

        [Published]
        public Aow.Rtl.TStringList Items { get => FItems; set => SetFStrings(value); }

        [Published]
        public Aow.Rtl.TStringList ItemTypes { get => FItemTypes; set => SetITStrings(value); }

        [Published]
        public Aow.Rtl.TNotifyEvent OnDrawItem { get => FOnDrawItem; set => FOnDrawItem = value; }

        [Published]
        public Aow.Units.ImageLib.TImageLibraryFont AOWFont { get => FAOWFont; set => FAOWFont = value; }

        [Published]
        public int ItemHeight { get => FItemHeight; set => FItemHeight = value; }

        [Published]
        public Aow.Units.AOWMenuBar.TAOWMenuBar OwnerMenuBar { get => FOwnerMenuBar; set => SetMenuBar(value); }

        [Published]
        public Aow.Units.AOWMenu.TAOWMenu OwnerMenu { get => FOwnerMenu; set => SetMenu(value); }

        [Published]
        public int OwnerItem { get => FOwnerItem; set => FOwnerItem = value; }

        [Published]
        public int SubMenuShowDelay { get => FSubMenuShowDelay; set => FSubMenuShowDelay = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnSelect { get => FOnSelect; set => FOnSelect = value; }

        // AOWMenu.pas:50
        public static new Aow.Units.AOWMenu.TAOWMenu Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.AOWMenu.TAOWMenu o = new Aow.Units.AOWMenu.TAOWMenu();
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

    public static partial class AOWMenu
    {
        // AOWMenu.pas:748
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Interface", [typeof(Aow.Units.AOWMenu.TAOWMenu)]);
        }
    }
}
