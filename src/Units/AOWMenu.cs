// Translated from AOWMenu.pas.
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
    // Fields: FPopupOwner, FILIndexTSelected, FMenuLeft, FMenuTop (SetWinLeft and SetWinTop store
    // the position here while the menu is hidden), FShowPending (SetVisible was called while
    // hidden), FSelectedItem (the item clicked last, -1 for none. read by OnSelect),
    // FILTextureSelected, FItems, FItemTypes (never created: SetITStrings fails, original bug),
    // FOnDrawItem, FItemHeight, FOwnerMenuBar, FOwnerMenu, FSubMenuShowDelay, FOnSelect,
    // FHighlighted (the item under the mouse, -1 for none), FSubMenuItem (the item whose submenus
    // are shown), FSubMenus (sorted by priority), FShowDelay (updates since the highlight moved),
    // FDrawItem, FDrawItemRect (the item drawn last, for OnDrawItem)
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
        public TAOWMenu(TComponent owner) : base(owner)
        {
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
            int I = default;
            Aow.Rtl.TRect OldClip = default;
            try
            {
                if (FValid && FVisible)
                {
                    var call__36 = Aow.Rtl.Rtl.Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                    FILFrame.DrawILF(FILIndexFrame, call__36, ref SurfDesc);
                    var call__37 = FILFrame[FILIndexFrame + 1];
                    var call__38 = FILFrame[FILIndexFrame + 6];
                    var call__39 = FILFrame[FILIndexFrame + 3];
                    var call__40 = FILFrame[FILIndexFrame + 4];
                    var call__41 = FILFrame[FILIndexFrame + 1];
                    var call__42 = FILFrame[FILIndexFrame + 3];
                    var call__43 = Aow.Rtl.Rtl.Rect(FWinLeft + call__42.Width, FWinTop + call__41.Height, (FWinWidth - call__39.Width) - call__40.Width, (FWinHeight - call__37.Height) - call__38.Height);
                    FILTexture.DrawILT(FILIndexTexture, call__43, ref SurfDesc);
                    FAOWFont.Color = FColor;
                    OldClip = SurfDesc.ClipRect;
                    var call__44 = FItems.Count;
                    if (call__44 > 0)
                    {
                        {
                            I = 0;
                            int end__45 = FItems.Count - 1;
                            for (; I <= end__45; I += 1)
                            {
                                var call__46 = FILFrame[FILIndexFrame + 4];
                                if (((FWinLeft + FWinWidth) - call__46.Width) < OldClip.Right)
                                {
                                    var call__47 = FILFrame[FILIndexFrame + 4];
                                    SurfDesc.ClipRect.Right = (FWinLeft + FWinWidth) - call__47.Width;
                                }

                                var call__48 = FItems.Count;
                                bool bool__49 = (I < call__48);
                                if (bool__49)
                                {
                                    var call__50 = FItems.Count;
                                    bool__49 = (call__50 > 0);
                                }

                                if (bool__49)
                                {
                                    if (I == FHighlighted)
                                    {
                                        FAOWFont.Color = FSelColor;
                                        var call__51 = FILFrame[FILIndexFrame + 3];
                                        var call__52 = FILFrame[FILIndexFrame + 4];
                                        var call__53 = FILFrame[FILIndexFrame];
                                        var call__54 = FILFrame[FILIndexFrame + 3];
                                        var call__55 = Aow.Rtl.Rtl.Rect(FWinLeft + call__54.Width, (FWinTop + call__53.Height) + (FItemHeight * I), (FWinWidth - call__51.Width) - call__52.Width, FItemHeight);
                                        FILTextureSelected.DrawILT(FILIndexTSelected, call__55, ref SurfDesc);
                                    }
                                    else
                                    {
                                        FAOWFont.Color = FColor;
                                    }

                                    var call__56 = FItems[I];
                                    var call__57 = FILFrame[FILIndexFrame];
                                    var call__58 = FILFrame[FILIndexFrame + 3];
                                    FAOWFont.SetText((FWinLeft + FBorderWidth) + call__58.Width, ((FWinTop + FBorderHeight) + call__57.Height) + (FItemHeight * I), call__56, SurfDesc);
                                    FDrawItem = I;
                                    var call__59 = FILFrame[FILIndexFrame + 1];
                                    var call__60 = FILFrame[FILIndexFrame + 3];
                                    var call__61 = FILFrame[FILIndexFrame + 1];
                                    var call__62 = FILFrame[FILIndexFrame + 3];
                                    var call__63 = FILFrame[FILIndexFrame + 3];
                                    var call__64 = Aow.Rtl.Rtl.Rect(FWinLeft + call__63.Width, (FWinTop + call__61.Height) + (call__62.Height * I), FWinLeft + FWinWidth, (FWinTop + call__59.Height) + (call__60.Height * (I + 1)));
                                    FDrawItemRect = call__64;
                                    if (FOnDrawItem != null)
                                    {
                                        FOnDrawItem(this);
                                    }
                                }

                                SurfDesc.ClipRect = OldClip;
                            }
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Exception during Draw", ((AnsiString)"Error - ") + Name, (int)0);
            }
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
                    byte end__65 = 0;
                    for (; I >= end__65; I -= 1)
                    {
                        var call__66 = FChildren[I];
                        C = (Aow.Units.AoWComp.TAoWComponent)call__66;
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
                                int end__67 = FSubMenus.Count - 1;
                                for (; I <= end__67; I += 1)
                                {
                                    var call__68 = FSubMenus[I];
                                    M = (Aow.Units.AOWMenu.TAOWMenu)call__68;
                                    if (M.FHandled && M.FVisible)
                                    {
                                        Show = true;
                                    }

                                    if ((M.FOwnerItem == FHighlighted) && (M.FHandled == false))
                                    {
                                        var call__69 = FILFrame[FILIndexFrame + 2];
                                        M.SetWinLeft((FWinLeft + FWinWidth) - call__69.Width);
                                        var call__70 = FILFrame[FILIndexFrame];
                                        M.SetWinTop((FWinTop + call__70.Height) + (FItemHeight * FHighlighted));
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
                        int end__71 = FSubMenus.Count - 1;
                        for (; I <= end__71; I += 1)
                        {
                            var call__72 = FSubMenus[I];
                            M = (Aow.Units.AOWMenu.TAOWMenu)call__72;
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
                    byte end__73 = 0;
                    for (; I >= end__73; I -= 1)
                    {
                        var call__74 = FChildren[I];
                        C = (Aow.Units.AoWComp.TAoWComponent)call__74;
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
                    byte end__75 = 0;
                    for (; I >= end__75; I -= 1)
                    {
                        var call__76 = FChildren[I];
                        C = (Aow.Units.AoWComp.TAoWComponent)call__76;
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
                var call__77 = FILFrame[FILIndexFrame];
                bool bool__78 = (X > ((FWinLeft + call__77.Width) - 1));
                if (bool__78)
                {
                    var call__79 = FILFrame[FILIndexFrame + 7];
                    bool__78 = (X < ((FWinLeft + FWinWidth) - call__79.Width));
                }

                bool bool__80 = bool__78;
                if (bool__80)
                {
                    var call__81 = FILFrame[FILIndexFrame];
                    bool__80 = (Y > ((FWinTop + call__81.Height) - 1));
                }

                bool bool__82 = bool__80;
                if (bool__82)
                {
                    var call__83 = FILFrame[FILIndexFrame + 7];
                    bool__82 = (Y < ((FWinTop + FWinHeight) - call__83.Height));
                }

                if (bool__82)
                {
                    var call__84 = FILFrame[FILIndexFrame];
                    I = ((Y - FWinTop) - call__84.Height) / FItemHeight;
                    if (FOnMouseMove != null)
                    {
                        FOnMouseMove(this, Shift, X, Y);
                    }
                }

                bool bool__85 = (I > -2);
                if (bool__85)
                {
                    var call__86 = FItems.Count;
                    bool__85 = (I < call__86);
                }

                if (bool__85)
                {
                    FHighlighted = I;
                }

                if (I == -1)
                {
                    if (FSubMenuItem > -1)
                    {
                        {
                            I = 0;
                            int end__87 = FSubMenus.Count - 1;
                            for (; I <= end__87; I += 1)
                            {
                                var component = (Aow.Units.AoWComp.TAoWComponent)FSubMenus[I];
                                if (component.FHandled == true)
                                {
                                    this.FHighlighted = component.FOwnerItem;
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
                        var call__89 = FSubMenus[OldHighlighted];
                        ((Aow.Units.AOWMenu.TAOWMenu)call__89).SetVisible(false);
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
                int end__90 = FChildren.Count - 1;
                for (; I <= end__90; I += 1)
                {
                    var call__91 = FChildren[I];
                    C = (Aow.Units.AoWComp.TAoWComponent)call__91;
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
                int end__92 = FChildren.Count - 1;
                for (; I <= end__92; I += 1)
                {
                    var call__93 = FChildren[I];
                    C = (Aow.Units.AoWComp.TAoWComponent)call__93;
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
                        var call__94 = FOwnerMenu.FSubMenus[I];
                        M = (Aow.Units.AOWMenu.TAOWMenu)call__94;
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
                        var call__95 = FOwnerMenu.FSubMenus[I];
                        M = (Aow.Units.AOWMenu.TAOWMenu)call__95;
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
                        var call__96 = FOwnerMenuBar.FMenus[I];
                        M = (Aow.Units.AOWMenu.TAOWMenu)call__96;
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
                        var call__97 = FOwnerMenuBar.FMenus[I];
                        M = (Aow.Units.AOWMenu.TAOWMenu)call__97;
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
                int end__98 = Value.Count - 1;
                for (; I <= end__98; I += 1)
                {
                    var call__99 = Value[I];
                    FItems.Add(call__99);
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
                int end__100 = Value.Count - 1;
                for (; I <= end__100; I += 1)
                {
                    var call__101 = Value[I];
                    FItemTypes.Add(call__101);
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
