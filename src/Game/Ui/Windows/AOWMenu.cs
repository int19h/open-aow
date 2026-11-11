// Translated from AOWMenu.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Ui.Controls;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Windows;

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
public partial class AOWMenu : AOWGenericWindow
{
    public AoWComponent FPopupOwner;
    public int FILIndexTSelected;
    public int FMenuLeft;
    public int FMenuTop;
    public bool FShowPending;
    public int FSelectedItem;
    public InterfaceIL FILTextureSelected;
    public StringList FItems;
    public StringList FItemTypes;
    public NotifyEvent<AOWMenu> FOnDrawItem;
    public int FItemHeight;
    public AOWMenuBar FOwnerMenuBar;
    public AOWMenu FOwnerMenu;
    public int FSubMenuShowDelay;
    public NotifyEvent<AOWMenu> FOnSelect;
    public int FHighlighted;
    public int FSubMenuItem;
    public List<AOWMenu> FSubMenus;
    public int FShowDelay;
    public int FDrawItem;
    public Rect FDrawItemRect;

    // AOWMenu.pas:102
    public AOWMenu(Component owner) : base(owner)
    {
        FItems = new StringList();
        FItems.OnChange = OnChangeText;
        FHighlighted = -1;
        FSelectedItem = -1;
        FSubMenuItem = -1;
        FItemHeight = 10;
        FSubMenus = new List<AOWMenu>();
        FVisible = false;
    }

    // AOWMenu.pas:117
    // { Destroy removes the menu from the lists of its owner menu and menu bar and detaches its
    //   submenus }
    public override void Dispose()
    {
        int I;
        AOWMenu M;
        I = 0;
        if (FOwnerMenu != null)
        {
            try
            {
                while (I < FOwnerMenu.FSubMenus.Count)
                {
                    M = FOwnerMenu.FSubMenus[I];
                    if (M == this)
                    {
                        FOwnerMenu.FSubMenus.RemoveAt(I);
                        I = FOwnerMenu.FSubMenus.Count;
                    }

                    I += 1;
                }
            }
            catch
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
                    M = FOwnerMenuBar.FMenus[I];
                    if (M == this)
                    {
                        FOwnerMenuBar.FMenus.RemoveAt(I);
                        I = FOwnerMenuBar.FMenus.Count;
                    }

                    I += 1;
                }
            }
            catch
            {
            }
        }

        FItems.Dispose();
        I = 0;
        int last = FSubMenus.Count - 1;
        for (; I <= last; I += 1)
        {
            (FSubMenus[I]).DetachMenu();
        }

        FSubMenus.Clear();
        base.Dispose();
    }

    // AOWMenu.pas:159
    public override void Validate()
    {
        if (FILFrame != null)
        {
            FILFrame = FILFrame.CheckIL(FILIndexFrame, 8, this);
        }

        if (FILTexture != null)
        {
            FILTexture = FILTexture.CheckIL(FILIndexTexture, 1, this);
        }

        if (FILTextureSelected != null)
        {
            FILTextureSelected = FILTextureSelected.CheckIL(FILIndexTSelected, 1, this);
        }

        if (FAOWOwner != null && FILFrame != null && FILTexture != null && FILTextureSelected != null && FAOWFont != null)
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
        AOWMenuBar MB;
        AOWBaseWindow W;
        if (FItemHeight == 0)
        {
            FItemHeight = FILFrame[FILIndexFrame + 3].Height;
        }

        if (FWinHeight == 0)
        {
            var ilFrame = FILFrame[FILIndexFrame];
            var ilFrameHeight = FILFrame[FILIndexFrame + 7];
            var count = FItems.Count;
            FWinHeight = ilFrame.Height + ilFrameHeight.Height + count * FItemHeight;
        }

        var ilFrameWidth = FILFrame[FILIndexFrame];
        var ilFrame2 = FILFrame[FILIndexFrame + 2];
        if (ilFrameWidth.Width + ilFrame2.Width > FWinWidth)
        {
            var ilFrame = FILFrame[FILIndexFrame];
            var ilFrame3 = FILFrame[FILIndexFrame + 2];
            FWinWidth = ilFrame.Width + ilFrame3.Width;
        }

        var ilFrame4 = FILFrame[FILIndexFrame];
        var fItemsCount = FItems.Count;
        var ilFrame5 = FILFrame[FILIndexFrame + 5];
        if (ilFrame4.Height + fItemsCount * FItemHeight + ilFrame5.Height > FWinHeight)
        {
            var ilFrame = FILFrame[FILIndexFrame];
            var count = FItems.Count;
            var ilFrameHeight = FILFrame[FILIndexFrame + 5];
            FWinHeight = ilFrame.Height + count * FItemHeight + ilFrameHeight.Height;
        }

        if (FWinManager != null && FOwnerMenuBar != null)
        {
            MB = FOwnerMenuBar;
            W = MB.FAOWWindow;
            if (W != null)
            {
                var count = MB.FItems.Count;
                FWinLeft = W.FWinLeft + MB.FWinLeft + MB.FWinWidth / count * FOwnerMenuBar.FSelected;
                FWinTop = FOwnerMenuBar.FAOWWindow.FWinTop + FOwnerMenuBar.FWinTop + FOwnerMenuBar.FWinHeight;
                if (FWinManager.FWinWidth < FWinLeft + FWinWidth)
                {
                    FWinLeft = FWinManager.FWinWidth - FWinWidth;
                }

                if (FWinTop + FWinHeight > FWinManager.FWinHeight)
                {
                    FWinTop = FOwnerMenuBar.FAOWWindow.FWinTop + FOwnerMenuBar.FWinTop - FWinHeight;
                }
            }
        }

        if (FUseManagerBorders)
        {
            if (FWinManager.FBorderLeft > FWinLeft)
            {
                FWinLeft = FWinManager.FBorderLeft;
            }

            if (FWinManager.FWinWidth - FWinManager.FBorderRight < FWinLeft + FWinWidth)
            {
                FWinLeft = FWinManager.FWinWidth - FWinManager.FBorderRight - FWinWidth;
            }

            if (FWinManager.FBorderTop > FWinTop)
            {
                FWinTop = FWinManager.FBorderTop;
            }

            if (FWinManager.FWinHeight - FWinManager.FBorderBottom < FWinTop + FWinHeight)
            {
                FWinTop = FWinManager.FWinHeight - FWinManager.FBorderBottom - FWinHeight;
            }
        }

        if (FMasked)
        {
            FFrameLeft = FILFrame[FILIndexFrame + 3].Width;
            var ilFrame = FILFrame[FILIndexFrame + 4];
            FFrameRight = FWinWidth - ilFrame.Width;
            FFrameTop = FILFrame[FILIndexFrame + 1].Height;
            var ilFrameHeight = FILFrame[FILIndexFrame + 6];
            FFrameBottom = FWinHeight - ilFrameHeight.Height;
        }

        if (FDoubleBuffered && FBackSurface != null)
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
        AOWMenu M;
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
                        for (int I = 0, last = FSubMenus.Count - 1; I <= last; I++)
                        {
                            M = FSubMenus[I];
                            if (M.FOwnerItem == FSubMenuItem)
                            {
                                M.SetVisible(false);
                            }
                        }
                    }

                    for (int I = 0, last = FSubMenus.Count - 1; I <= last; I++)
                    {
                        M = FSubMenus[I];
                        if (M.FOwnerItem == FHighlighted)
                        {
                            var ilFrame = FILFrame[FILIndexFrame + 2];
                            M.SetWinLeft(FWinLeft + FWinWidth - ilFrame.Width);
                            var ilFrameHeight = FILFrame[FILIndexFrame];
                            M.SetWinTop(FWinTop + ilFrameHeight.Height + FItemHeight * FHighlighted);
                            M.SetVisible(true);
                            FSubMenuItem = FHighlighted;
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
        AOWMenuBar Bar = default;
        AOWBaseWindow W = default;
        AOWMenuBar MB = default;
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
                    if (FWinManager != null && FOwnerMenuBar != null)
                    {
                        MB = FOwnerMenuBar;
                        W = MB.FAOWWindow;
                        if (W != null)
                        {
                            var count = MB.FItems.Count;
                            FWinLeft = W.FWinLeft + MB.FWinLeft + MB.FWinWidth / count * FOwnerMenuBar.FSelected;
                            FWinTop = FOwnerMenuBar.FAOWWindow.FWinTop + FOwnerMenuBar.FWinTop + FOwnerMenuBar.FWinHeight;
                            if (FWinManager.FWinWidth < FWinLeft + FWinWidth)
                            {
                                FWinLeft = FWinManager.FWinWidth - FWinWidth;
                            }

                            if (FWinTop + FWinHeight > FWinManager.FWinHeight)
                            {
                                FWinTop = FOwnerMenuBar.FAOWWindow.FWinTop + FOwnerMenuBar.FWinTop - FWinHeight;
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
            if (!FValid || !FAligned || !FSized)
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
    public override void Draw(SurfaceDesc SurfDesc)
    {
        Rect OldClip;
        try
        {
            if (FValid && FVisible)
            {
                var rect = new Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                FILFrame.DrawILF(FILIndexFrame, rect, ref SurfDesc);
                var ilFrameHeight = FILFrame[FILIndexFrame + 1];
                var ilFrame = FILFrame[FILIndexFrame + 6];
                var ilFrameWidth = FILFrame[FILIndexFrame + 3];
                var ilFrame2 = FILFrame[FILIndexFrame + 4];
                var ilFrame3 = FILFrame[FILIndexFrame + 1];
                var ilFrame4 = FILFrame[FILIndexFrame + 3];
                var rect2 = new Rect(FWinLeft + ilFrame4.Width, FWinTop + ilFrame3.Height, FWinWidth - ilFrameWidth.Width - ilFrame2.Width, FWinHeight - ilFrameHeight.Height - ilFrame.Height);
                FILTexture.DrawILT(FILIndexTexture, rect2, ref SurfDesc);
                FAOWFont.Color = FColor;
                OldClip = SurfDesc.ClipRect;
                if (FItems.Count > 0)
                {
                    for (int I = 0, last = FItems.Count - 1; I <= last; I++)
                    {
                        var ilFrame5 = FILFrame[FILIndexFrame + 4];
                        if (FWinLeft + FWinWidth - ilFrame5.Width < OldClip.Right)
                        {
                            var ilFrame6 = FILFrame[FILIndexFrame + 4];
                            SurfDesc.ClipRect.Right = FWinLeft + FWinWidth - ilFrame6.Width;
                        }

                        if (I < FItems.Count && FItems.Count > 0)
                        {
                            if (I == FHighlighted)
                            {
                                FAOWFont.Color = FSelColor;
                                var ilFrame6 = FILFrame[FILIndexFrame + 3];
                                var ilFrame7 = FILFrame[FILIndexFrame + 4];
                                var ilFrame8 = FILFrame[FILIndexFrame];
                                var ilFrame9 = FILFrame[FILIndexFrame + 3];
                                var rect3 = new Rect(FWinLeft + ilFrame9.Width, FWinTop + ilFrame8.Height + FItemHeight * I, FWinWidth - ilFrame6.Width - ilFrame7.Width, FItemHeight);
                                FILTextureSelected.DrawILT(FILIndexTSelected, rect3, ref SurfDesc);
                            }
                            else
                            {
                                FAOWFont.Color = FColor;
                            }

                            var item = FItems[I];
                            var filFrameItem = FILFrame[FILIndexFrame];
                            var ilFrameWidth2 = FILFrame[FILIndexFrame + 3];
                            FAOWFont.SetText(FWinLeft + FBorderWidth + ilFrameWidth2.Width, FWinTop + FBorderHeight + filFrameItem.Height + FItemHeight * I, item, SurfDesc);
                            FDrawItem = I;
                            var ilFrameHeight2 = FILFrame[FILIndexFrame + 1];
                            var ilFrameHeight3 = FILFrame[FILIndexFrame + 3];
                            var ilFrameHeight4 = FILFrame[FILIndexFrame + 1];
                            var ilFrameHeight5 = FILFrame[FILIndexFrame + 3];
                            var ilFrameWidth3 = FILFrame[FILIndexFrame + 3];
                            FDrawItemRect = new Rect(FWinLeft + ilFrameWidth3.Width, FWinTop + ilFrameHeight4.Height + ilFrameHeight5.Height * I, FWinLeft + FWinWidth, FWinTop + ilFrameHeight2.Height + ilFrameHeight3.Height * (I + 1));
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
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AOWMenu.pas:555
    // { CheckMouseDown: the controls of the menu first; a click on an item selects it (OnSelect) and
    //   shows its submenus; a click outside hides the menu unless a submenu took it }
    public override void CheckMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        AoWComponent C = default;
        bool Show;
        bool Keep;
        AOWMenu M = default;
        FHandled = false;
        FRedraw = false;
        FSelectedItem = -1;
        Keep = true;
        if (FVisible == true)
        {
            for (int I = FChildren.Count - 1; I >= 0; I--)
            {
                C = (AoWComponent)FChildren[I];
                if (!FHandled && C.FCaptureInput)
                {
                    C.CheckMouseDown(Button, Shift, X, Y);
                }

                if (C.FHandled == true)
                {
                    FHandled = true;
                }
            }

            Show = FVisible;
            if (X > FWinLeft - 1 && X < FWinLeft + FWinWidth && Y > FWinTop - 1 && Y < FWinTop + FWinHeight)
            {
                if (!FHandled)
                {
                    FRedraw = true;
                    FHandled = true;
                    if (FHighlighted > -1)
                    {
                        Show = false;
                        for (int I = 0, last = FSubMenus.Count - 1; I <= last; I++)
                        {
                            M = FSubMenus[I];
                            if (M.FHandled && M.FVisible)
                            {
                                Show = true;
                            }

                            if (M.FOwnerItem == FHighlighted && !M.FHandled)
                            {
                                var ilFrame = FILFrame[FILIndexFrame + 2];
                                M.SetWinLeft(FWinLeft + FWinWidth - ilFrame.Width);
                                var ilFrameHeight = FILFrame[FILIndexFrame];
                                M.SetWinTop(FWinTop + ilFrameHeight.Height + FItemHeight * FHighlighted);
                                M.SetVisible(true);
                                Show = true;
                                FRedraw = true;
                                FSubMenuItem = FHighlighted;
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
                for (int I = 0, last = FSubMenus.Count - 1; I <= last; I++)
                {
                    M = FSubMenus[I];
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
                    }
                }
            }

            SetVisible(Show);
            if (FHandled && FOnMouseDown != null)
            {
                FOnMouseDown(this, Button, Shift, X, Y);
            }
        }
    }

    // AOWMenu.pas:634
    public override void CheckMouseUp(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        AoWComponent C = default;
        FHandled = false;
        if (FVisible == true)
        {
            for (int I = FChildren.Count - 1; I >= 0; I--)
            {
                C = (AoWComponent)FChildren[I];
                if ((!FHandled || C.FRedraw == true) && C.FCaptureInput)
                {
                    C.CheckMouseUp(Button, Shift, X, Y);
                }

                if (C.FHandled == true)
                {
                    FHandled = true;
                }
            }

            if (X > FWinLeft - 1 && X < FWinLeft + FWinWidth && Y > FWinTop - 1 && Y < FWinTop + FWinHeight)
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
    public override void CheckMouseMove(Set<ShiftState> Shift, int X, int Y)
    {
        AoWComponent C = default;
        int OldHighlighted;
        int I;
        // { Keep the child on the stack and I in a register. }
        FHandled = false;
        FInResizeArea = true;
        if (FVisible == true)
        {
            FRedraw = true;
            OldHighlighted = FHighlighted;
            FCursor = -2;
            I = FChildren.Count - 1;
            for (; I >= 0; I -= 1)
            {
                C = (AoWComponent)FChildren[I];
                if ((!FHandled || C.FRedraw == true || C.FToolTipCount > 0) && C.FCaptureInput)
                {
                    C.CheckMouseMove(Shift, X, Y);
                }

                if (C.FHandled == true)
                {
                    FHandled = true;
                }
            }

            FHandled = true;
            I = -1;
            var ilFrameWidth = FILFrame[FILIndexFrame];
            bool condition = X > FWinLeft + ilFrameWidth.Width - 1;
            if (condition)
            {
                var ilFrame = FILFrame[FILIndexFrame + 7];
                condition = X < FWinLeft + FWinWidth - ilFrame.Width;
            }

            bool test = condition;
            if (test)
            {
                var ilFrame = FILFrame[FILIndexFrame];
                test = Y > FWinTop + ilFrame.Height - 1;
            }

            bool passed = test;
            if (passed)
            {
                var ilFrame = FILFrame[FILIndexFrame + 7];
                passed = Y < FWinTop + FWinHeight - ilFrame.Height;
            }

            if (passed)
            {
                var ilFrame = FILFrame[FILIndexFrame];
                I = (Y - FWinTop - ilFrame.Height) / FItemHeight;
                if (FOnMouseMove != null)
                {
                    FOnMouseMove(this, Shift, X, Y);
                }
            }

            if (I > -2 && I < FItems.Count)
            {
                FHighlighted = I;
            }

            if (I == -1)
            {
                if (FSubMenuItem > -1)
                {
                    I = 0;
                    int last = FSubMenus.Count - 1;
                    for (; I <= last; I += 1)
                    {
                        var component = FSubMenus[I];
                        if (component.FHandled == true)
                        {
                            this.FHighlighted = component.FOwnerItem;
                        }
                    }
                }
            }

            if (OldHighlighted != FHighlighted)
            {
                FShowDelay = 0;
                if (OldHighlighted > -1 && FSubMenus.Count > OldHighlighted)
                {
                    (FSubMenus[OldHighlighted]).SetVisible(false);
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
        AoWComponent C = default;
        FHandled = false;
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            C = (AoWComponent)FChildren[I];
            C.CheckKeyPress(ref Key);
            if (C.FHandled == true)
            {
                FHandled = true;
            }
        }

        if (FOnKeyPress != null)
        {
            FOnKeyPress(this, ref Key);
        }
    }

    // AOWMenu.pas:731
    public override void CheckKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        AoWComponent C = default;
        FHandled = false;
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            C = (AoWComponent)FChildren[I];
            C.CheckKeyDown(ref Key, Shift);
            if (C.FHandled == true)
            {
                FHandled = true;
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
    public virtual void SetMenu(AOWMenu Value)
    {
        int I;
        int NewIndex;
        AOWMenu M;
        if (Value != FOwnerMenu)
        {
            if (FOwnerMenu != null)
            {
                I = 0;
                while (I < FOwnerMenu.FSubMenus.Count)
                {
                    M = FOwnerMenu.FSubMenus[I];
                    if (M == this)
                    {
                        FOwnerMenu.FSubMenus.RemoveAt(I);
                        I = FOwnerMenu.FSubMenus.Count;
                    }

                    I += 1;
                }
            }

            FOwnerMenu = Value;
            if (FOwnerMenu != this && FOwnerMenu != null)
            {
                I = 0;
                NewIndex = -1;
                while (I < FOwnerMenu.FSubMenus.Count)
                {
                    M = FOwnerMenu.FSubMenus[I];
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
    public virtual void SetMenuBar(AOWMenuBar Value)
    {
        int I;
        int NewIndex;
        AOWMenu M;
        if (Value != FOwnerMenuBar)
        {
            if (FOwnerMenuBar != null)
            {
                I = 0;
                while (I < FOwnerMenuBar.FMenus.Count)
                {
                    M = FOwnerMenuBar.FMenus[I];
                    if (M == this)
                    {
                        FOwnerMenuBar.FMenus.RemoveAt(I);
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
                    M = FOwnerMenuBar.FMenus[I];
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
    public virtual void SetILTSel(InterfaceIL Value)
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
    public void SetFStrings(StringList Value)
    {
        FItems.Clear();
        for (int I = 0, last = Value.Count - 1; I <= last; I++)
        {
            var valueValue = Value[I];
            FItems.Add(valueValue);
        }
    }

    // AOWMenu.pas:449
    public void SetITStrings(StringList Value)
    {
        FItemTypes.Clear();
        for (int I = 0, last = Value.Count - 1; I <= last; I++)
        {
            var valueValue = Value[I];
            FItemTypes.Add(valueValue);
        }
    }

    // AOWMenu.pas:458
    public void SetPopupOwner(AoWComponent Value)
    {
        FPopupOwner = Value;
    }

    [Published]
    public InterfaceIL ILTextureNormal { get => FILTexture; set => SetILTexture(value); }

    [Published]
    public InterfaceIL ILTextureSelected { get => FILTextureSelected; set => SetILTSel(value); }

    [Published]
    public new int ILIndexFrame { get => FILIndexFrame; set => FILIndexFrame = value; }

    [Published]
    public int ILIndexTNormal { get => FILIndexTexture; set => SetILTIndex(value); }

    [Published]
    public int ILIndexTSelected { get => FILIndexTSelected; set => SetILTsIndex(value); }

    [Published]
    public StringList Items { get => FItems; set => SetFStrings(value); }

    [Published]
    public StringList ItemTypes { get => FItemTypes; set => SetITStrings(value); }

    [Published]
    public NotifyEvent<AOWMenu> OnDrawItem { get => FOnDrawItem; set => FOnDrawItem = value; }

    [Published]
    public ImageLibraryFont AOWFont { get => FAOWFont; set => FAOWFont = value; }

    [Published]
    public int ItemHeight { get => FItemHeight; set => FItemHeight = value; }

    [Published]
    public AOWMenuBar OwnerMenuBar { get => FOwnerMenuBar; set => SetMenuBar(value); }

    [Published]
    public AOWMenu OwnerMenu { get => FOwnerMenu; set => SetMenu(value); }

    [Published]
    public int OwnerItem { get => FOwnerItem; set => FOwnerItem = value; }

    [Published]
    public int SubMenuShowDelay { get => FSubMenuShowDelay; set => FSubMenuShowDelay = value; }

    [Published]
    public NotifyEvent<AOWMenu> OnSelect { get => FOnSelect; set => FOnSelect = value; }

    public new NotifyEvent<AOWMenu> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<AOWMenu> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<AOWMenu> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<AOWMenu> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<AOWMenu> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<AOWMenu> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<AOWMenu> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<AOWMenu> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<AOWMenu> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }

    public new AoWOnDrawBackEvent<AOWMenu> OnDrawBack
    {
        get => EventSlot.Get(this, FOnDrawBack);
        set => EventSlot.Set(this, ref FOnDrawBack, value);
    }

    public new NotifyEvent<AOWMenu> OnEnter
    {
        get => EventSlot.Get(this, FOnEnter);
        set => EventSlot.Set(this, ref FOnEnter, value);
    }

    public new NotifyEvent<AOWMenu> OnExit
    {
        get => EventSlot.Get(this, FOnExit);
        set => EventSlot.Set(this, ref FOnExit, value);
    }

    public new NotifyEvent<AOWMenu> OnChangeCursor
    {
        get => EventSlot.Get(this, FOnChangeCursor);
        set => EventSlot.Set(this, ref FOnChangeCursor, value);
    }

    public new AoWOnDrawSurface<AOWMenu> OnDrawSurface
    {
        get => EventSlot.Get(this, FOnDrawSurface);
        set => EventSlot.Set(this, ref FOnDrawSurface, value);
    }

    public new AoWOnDrawSurface<AOWMenu> OnPreDrawSurface
    {
        get => EventSlot.Get(this, FOnPreDrawSurface);
        set => EventSlot.Set(this, ref FOnPreDrawSurface, value);
    }

    public new NotifyEvent<AOWMenu> OnHide
    {
        get => EventSlot.Get(this, FOnHide);
        set => EventSlot.Set(this, ref FOnHide, value);
    }

    public new NotifyEvent<AOWMenu> OnShow
    {
        get => EventSlot.Get(this, FOnShow);
        set => EventSlot.Set(this, ref FOnShow, value);
    }
}
