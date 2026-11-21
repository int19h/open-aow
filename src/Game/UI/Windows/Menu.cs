#nullable disable warnings
// Translated from AOWMenu.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Windows;

// { The Menu unit of aowInt.dpl: TAOWMenu, a pop-up menu window. It belongs to an item of a menu
//   bar (OwnerMenuBar and OwnerItem) or of another menu (OwnerMenu: a submenu), places itself under
//   the bar's item when it is shown, highlights the item under the mouse and shows the submenus of
//   the highlighted item after SubMenuShowDelay updates. }
// Fields: PopupOwner, _imageLibraryIndexTSelected, MenuLeft, MenuTop (SetWinLeft and SetWinTop store
// the position here while the menu is hidden), FShowPending (SetVisible was called while
// hidden), FSelectedItem (the item clicked last, -1 for none. read by OnSelect),
// FILTextureSelected, FItems, FItemTypes (never created: SetITStrings fails, original bug),
// OnDrawItem, _itemHeight, _ownerMenuBar, _ownerMenu, _subMenuShowDelay, OnSelect,
// FHighlighted (the item under the mouse, -1 for none), FSubMenuItem (the item whose submenus
// are shown), FSubMenus (sorted by priority), FShowDelay (updates since the highlight moved),
// FDrawItem, FDrawItemRect (the item drawn last, for OnDrawItem)
public partial class Menu : GenericWindow
{

    private int _imageLibraryIndexTSelected;

    public int MenuLeft { get; private set; }
    public int MenuTop { get; private set; }
    public bool ShowPending { get; private set; }
    public int SelectedItem { get; set; }

    private InterfaceImageLibrary? _imageLibraryTextureSelected;

    [Published]
    public event AoWOnDrawItemEvent<Menu>? DrawItem;

    protected void RaiseDrawItem(Menu sender, in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface) => DrawItem?.Invoke(sender, DrawRect: drawRectangle, DrawItem: drawItem, SurfDesc: ref lockedSurface);

    private MenuBar? _ownerMenuBar;
    private Menu? _ownerMenu;

    [Published]
    public event NotifyEvent<Menu>? Select;

    public int Highlighted { get; protected set; }

    private int _subMenuItem;

    public List<Menu> SubMenus { get; }

    private int _showDelay;

    // AOWMenu.pas:102
    public Menu(Component? owner) : base(owner)
    {
        Items = new DelphiStringList
        {
            OnChange = OnChangeText
        };
        Highlighted = -1;
        SelectedItem = -1;
        _subMenuItem = -1;
        ItemHeight = 10;
        SubMenus = [];
        SetVisibleWithoutUpdate(false);
    }

    // AOWMenu.pas:117
    // { Destroy removes the menu from the lists of its owner menu and menu bar and detaches its
    //   submenus }
    public override void Dispose()
    {
        int i = 0;
        Menu m;
        if (_ownerMenu is not null)
        {
            try
            {
                while (i < _ownerMenu.SubMenus.Count)
                {
                    m = _ownerMenu.SubMenus[i];
                    if (m == this)
                    {
                        _ownerMenu.SubMenus.RemoveAt(i);
                        i = _ownerMenu.SubMenus.Count;
                    }

                    i += 1;
                }
            }
            catch
            {
            }
        }

        i = 0;
        if (_ownerMenuBar is not null)
        {
            try
            {
                while (i < _ownerMenuBar.Menus.Count)
                {
                    m = _ownerMenuBar.Menus[i];
                    if (m == this)
                    {
                        _ownerMenuBar.Menus.RemoveAt(i);
                        i = _ownerMenuBar.Menus.Count;
                    }

                    i += 1;
                }
            }
            catch
            {
            }
        }

        Items.Dispose();
        i = 0;
        int last = SubMenus.Count - 1;
        for (; i <= last; i += 1)
        {
            SubMenus[i].DetachMenu();
        }

        SubMenus.Clear();
        base.Dispose();
    }

    // AOWMenu.pas:159
    public override void Validate()
    {
        if (ImageLibraryFrame is not null)
        {
            ImageLibraryFrame = ImageLibraryFrame.CheckImageLibrary(ImageLibraryIndexFrame, count: 8, sender: this);
        }

        if (ImageLibraryTexture is not null)
        {
            ImageLibraryTexture = ImageLibraryTexture.CheckImageLibrary(ImageLibraryIndexTexture, count: 1, sender: this);
        }

        if (_imageLibraryTextureSelected is not null)
        {
            _imageLibraryTextureSelected = _imageLibraryTextureSelected.CheckImageLibrary(_imageLibraryIndexTSelected, count: 1, sender: this);
        }

        if (OwnerComponent is not null && ImageLibraryFrame is not null && ImageLibraryTexture is not null && _imageLibraryTextureSelected is not null && base.Font is not null)
        {
            Valid = true;
        }

        base.Validate();
    }

    // AOWMenu.pas:381
    // { SetSize: the height fits the items between the top and bottom frame pieces; the menu is placed
    //   under its menu bar item and inside the window manager's borders; the frame rectangle is set
    //   for masked menus }
    public override void SetSize()
    {
        if (ItemHeight == 0)
        {
            ItemHeight = ImageLibraryFrame[ImageLibraryIndexFrame + 3].Height;
        }

        if (WinHeight == 0)
        {
            LibraryImage? imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame];
            LibraryImage? imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame + 7];
            int count = Items.Count;
            SetWinHeightWithoutInvalidate(imageLibraryFrame.Height + imageLibraryFrameHeight.Height + (count * ItemHeight));
        }

        LibraryImage? imageLibraryFrameWidth = ImageLibraryFrame[ImageLibraryIndexFrame];
        LibraryImage? imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
        if (imageLibraryFrameWidth.Width + imageLibraryFrame2.Width > WinWidth)
        {
            LibraryImage? imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame];
            LibraryImage? imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
            SetWinWidthWithoutInvalidate(imageLibraryFrame.Width + imageLibraryFrame3.Width);
        }

        LibraryImage? imageLibraryFrame4 = ImageLibraryFrame[ImageLibraryIndexFrame];
        int fItemsCount = Items.Count;
        LibraryImage? imageLibraryFrame5 = ImageLibraryFrame[ImageLibraryIndexFrame + 5];
        if (imageLibraryFrame4.Height + (fItemsCount * ItemHeight) + imageLibraryFrame5.Height > WinHeight)
        {
            LibraryImage? imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame];
            int count = Items.Count;
            LibraryImage? imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame + 5];
            SetWinHeightWithoutInvalidate(imageLibraryFrame.Height + (count * ItemHeight) + imageLibraryFrameHeight.Height);
        }

        if (WindowManager is not null && _ownerMenuBar is not null)
        {
            MenuBar mb = _ownerMenuBar;
            BaseWindow? w = mb.Window;
            if (w is not null)
            {
                int count = mb.Items.Count;
                SetWinLeftWithoutInvalidate(w.WinLeft + mb.WinLeft + (mb.WinWidth / count * _ownerMenuBar.Selected));
                SetWinTopWithoutInvalidate(_ownerMenuBar.Window.AbsoluteTop + _ownerMenuBar.WinTop + _ownerMenuBar.WinHeight);
                if (WindowManager.WinWidth < WinLeft + WinWidth)
                {
                    SetWinLeftWithoutInvalidate(WindowManager.WinWidth - WinWidth);
                }

                if (WinTop + WinHeight > WindowManager.WinHeight)
                {
                    SetWinTopWithoutInvalidate(_ownerMenuBar.Window.AbsoluteTop + _ownerMenuBar.WinTop - WinHeight);
                }
            }
        }

        if (UseManagerBorders)
        {
            if (WindowManager.BorderLeft > WinLeft)
            {
                SetWinLeftWithoutInvalidate(WindowManager.BorderLeft);
            }

            if (WindowManager.WinWidth - WindowManager.BorderRight < WinLeft + WinWidth)
            {
                SetWinLeftWithoutInvalidate(WindowManager.WinWidth - WindowManager.BorderRight - WinWidth);
            }

            if (WindowManager.BorderTop > WinTop)
            {
                SetWinTopWithoutInvalidate(WindowManager.BorderTop);
            }

            if (WindowManager.WinHeight - WindowManager.BorderBottom < WinTop + WinHeight)
            {
                SetWinTopWithoutInvalidate(WindowManager.WinHeight - WindowManager.BorderBottom - WinHeight);
            }
        }

        if (Masked)
        {
            FrameLeft = ImageLibraryFrame[ImageLibraryIndexFrame + 3].Width;
            LibraryImage? imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
            FrameRight = WinWidth - imageLibraryFrame.Width;
            FrameTop = ImageLibraryFrame[ImageLibraryIndexFrame + 1].Height;
            LibraryImage? imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
            FrameBottom = WinHeight - imageLibraryFrameHeight.Height;
        }

        if (DoubleBuffered && BackSurface is not null)
        {
            _ = BackSurface.Resize(WinWidth, WinHeight);
            if (Masked)
            {
                _ = BackSurface.Clear(MaskColor);
            }
        }

        base.SetSize();
    }

    // AOWMenu.pas:465
    // { Update: after the highlight rested SubMenuShowDelay updates on an item, the submenus of the
    //   previous item are hidden and those of the highlighted item are shown beside it }
    public override void Update()
    {
        if (Valid && Visible)
        {
            base.Update();
            if (Highlighted > -1)
            {
                if (_showDelay < SubMenuShowDelay)
                {
                    _showDelay += 1;
                }
                else
                {
                    Menu m;
                    if (Highlighted != _subMenuItem)
                    {
                        for (int i = 0, last = SubMenus.Count - 1; i <= last; i++)
                        {
                            m = SubMenus[i];
                            if (m.OwnerItem == _subMenuItem)
                            {
                                m.SetVisible(false);
                            }
                        }
                    }

                    for (int i = 0, last = SubMenus.Count - 1; i <= last; i++)
                    {
                        m = SubMenus[i];
                        if (m.OwnerItem == Highlighted)
                        {
                            LibraryImage? imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
                            m.SetWinLeft(WinLeft + WinWidth - imageLibraryFrame.Width);
                            LibraryImage? imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame];
                            m.SetWinTop(WinTop + imageLibraryFrameHeight.Height + (ItemHeight * Highlighted));
                            m.SetVisible(true);
                            _subMenuItem = Highlighted;
                        }
                    }
                }
            }
        }
    }

    // AOWMenu.pas:173
    public override void SetWinLeft(int value)
    {
        if (Hidden)
        {
            MenuLeft = value;
        }
        else
        {
            SetWinLeftWithoutInvalidate(value);
        }

        IsDrawn = false;
    }

    // AOWMenu.pas:182
    public override void SetWinTop(int value)
    {
        if (Hidden)
        {
            MenuTop = value;
        }
        else
        {
            SetWinTopWithoutInvalidate(value);
        }

        IsDrawn = false;
    }

    // AOWMenu.pas:203
    // { SetVisible: a menu shown under a menu bar places itself under the bar's selected item, kept
    //   inside the window manager's screen; hiding it clears the bar's selection }
    public override void SetVisible(bool value)
    {
        if (value != Visible)
        {
            if (Hidden)
            {
                ShowPending = true;
            }
            else
            {
                SetVisibleWithoutUpdate(value);
                if (Visible)
                {
                    Highlighted = -1;
                    if (WindowManager is not null && _ownerMenuBar is not null)
                    {
                        MenuBar mb = _ownerMenuBar;
                        BaseWindow? w = mb.Window;
                        if (w is not null)
                        {
                            int count = mb.Items.Count;
                            SetWinLeftWithoutInvalidate(w.WinLeft + mb.WinLeft + (mb.WinWidth / count * _ownerMenuBar.Selected));
                            SetWinTopWithoutInvalidate(_ownerMenuBar.Window.AbsoluteTop + _ownerMenuBar.WinTop + _ownerMenuBar.WinHeight);
                            if (WindowManager.WinWidth < WinLeft + WinWidth)
                            {
                                SetWinLeftWithoutInvalidate(WindowManager.WinWidth - WinWidth);
                            }

                            if (WinTop + WinHeight > WindowManager.WinHeight)
                            {
                                SetWinTopWithoutInvalidate(_ownerMenuBar.Window.AbsoluteTop + _ownerMenuBar.WinTop - WinHeight);
                            }
                        }
                    }

                    IsDrawn = false;
                    if (!Hidden)
                    {
                        BackRectangle = new SizedRectangle(WinLeft, WinTop, WinWidth, WinHeight);
                    }

                    Hidden = false;
                    ShowPending = false;
                    RaiseShow(this);
                }
                else
                {
                    MenuBar? bar = _ownerMenuBar;
                    if (bar is not null)
                    {
                        bar.Selected = -1;
                        bar.IsDrawn = false;
                    }

                    Hidden = true;
                    RaiseHide(this);
                }
            }
        }

        if (Visible)
        {
            if (!Valid || !Aligned || !Sized)
            {
                Setup();
            }

            ClearBlitList();
            ClearDrawList();
            IsDrawn = false;
            SetPriority(Priority);
        }
    }

    // AOWMenu.pas:501
    // { Draw: the frame, the texture, and the items; the highlighted item on the selected texture }
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (Valid && Visible)
            {
                SizedRectangle rectangle = new(WinLeft, WinTop, WinWidth, WinHeight);
                ImageLibraryFrame.DrawFrame(ImageLibraryIndexFrame, area: rectangle, ref lockedSurface);
                LibraryImage? imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                LibraryImage? imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
                LibraryImage? imageLibraryFrameWidth = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                LibraryImage? imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
                LibraryImage? imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                LibraryImage? imageLibraryFrame4 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                SizedRectangle rectangle2 = new(WinLeft + imageLibraryFrame4.Width, WinTop + imageLibraryFrame3.Height, WinWidth - imageLibraryFrameWidth.Width - imageLibraryFrame2.Width, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                ImageLibraryTexture.DrawTiled(ImageLibraryIndexTexture, area: rectangle2, ref lockedSurface);
                base.Font.Color = Color;
                Rect oldClip = lockedSurface.ClipRectangle;
                if (Items.Count > 0)
                {
                    for (int i = 0, last = Items.Count - 1; i <= last; i++)
                    {
                        LibraryImage? imageLibraryFrame5 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
                        if (WinLeft + WinWidth - imageLibraryFrame5.Width < oldClip.Right)
                        {
                            LibraryImage? imageLibraryFrame6 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
                            Rect clipRectangle = lockedSurface.ClipRectangle;
                            clipRectangle.Right = WinLeft + WinWidth - imageLibraryFrame6.Width;
                            lockedSurface.ClipRectangle = clipRectangle;
                        }

                        if (i < Items.Count && Items.Count > 0)
                        {
                            if (i == Highlighted)
                            {
                                base.Font.Color = SelectedColor;
                                LibraryImage? imageLibraryFrame6 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                                LibraryImage? imageLibraryFrame7 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
                                LibraryImage? imageLibraryFrame8 = ImageLibraryFrame[ImageLibraryIndexFrame];
                                LibraryImage? imageLibraryFrame9 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                                SizedRectangle rectangle3 = new(WinLeft + imageLibraryFrame9.Width, WinTop + imageLibraryFrame8.Height + (ItemHeight * i), WinWidth - imageLibraryFrame6.Width - imageLibraryFrame7.Width, ItemHeight);
                                _imageLibraryTextureSelected.DrawTiled(_imageLibraryIndexTSelected, area: rectangle3, ref lockedSurface);
                            }
                            else
                            {
                                base.Font.Color = Color;
                            }

                            string item = Items[i];
                            LibraryImage? filFrameItem = ImageLibraryFrame[ImageLibraryIndexFrame];
                            LibraryImage? imageLibraryFrameWidth2 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                            base.Font.SetText(WinLeft + BorderWidth + imageLibraryFrameWidth2.Width, WinTop + BorderHeight + filFrameItem.Height + (ItemHeight * i), text: item, lockedSurface);
                            int drawItem = i;
                            LibraryImage? imageLibraryFrameHeight2 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                            LibraryImage? imageLibraryFrameHeight3 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                            LibraryImage? imageLibraryFrameHeight4 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                            LibraryImage? imageLibraryFrameHeight5 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                            LibraryImage? imageLibraryFrameWidth3 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                            Rect drawItemRectangle = new(WinLeft + imageLibraryFrameWidth3.Width, WinTop + imageLibraryFrameHeight4.Height + (imageLibraryFrameHeight5.Height * i), WinLeft + WinWidth, WinTop + imageLibraryFrameHeight2.Height + (imageLibraryFrameHeight3.Height * (i + 1)));
                            DrawItem?.Invoke(this, DrawRect: drawItemRectangle, DrawItem: drawItem, SurfDesc: ref lockedSurface);
                        }

                        lockedSurface.ClipRectangle = oldClip;
                    }
                }
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during Draw", caption: $"Error - {Name}", flags: 0);
        }
    }

    // AOWMenu.pas:555
    // { CheckMouseDown: the controls of the menu first; a click on an item selects it (OnSelect) and
    //   shows its submenus; a click outside hides the menu unless a submenu took it }
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = false;
        Redraw = false;
        SelectedItem = -1;
        if (Visible)
        {
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                UIComponent? c = Children[i];
                if (!Handled && c.CaptureInput)
                {
                    c.CheckMouseDown(button, shift, X, Y);
                }

                if (c.Handled)
                {
                    Handled = true;
                }
            }

            bool show = Visible;
            Menu m;
            if (X > WinLeft - 1 && X < WinLeft + WinWidth && Y > WinTop - 1 && Y < WinTop + WinHeight)
            {
                if (!Handled)
                {
                    Redraw = true;
                    Handled = true;
                    if (Highlighted > -1)
                    {
                        show = false;
                        for (int i = 0, last = SubMenus.Count - 1; i <= last; i++)
                        {
                            m = SubMenus[i];
                            if (m.Handled && m.Visible)
                            {
                                show = true;
                            }

                            if (m.OwnerItem == Highlighted && !m.Handled)
                            {
                                LibraryImage? imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
                                m.SetWinLeft(WinLeft + WinWidth - imageLibraryFrame.Width);
                                LibraryImage? imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame];
                                m.SetWinTop(WinTop + imageLibraryFrameHeight.Height + (ItemHeight * Highlighted));
                                m.SetVisible(true);
                                show = true;
                                Redraw = true;
                                _subMenuItem = Highlighted;
                            }
                        }

                        SelectedItem = Highlighted;
                        Select?.Invoke(this);
                    }
                }
            }
            else
            {
                show = false;
                bool keep = true;
                for (int i = 0, last = SubMenus.Count - 1; i <= last; i++)
                {
                    m = SubMenus[i];
                    if (m.Handled)
                    {
                        if (m.Visible)
                        {
                            show = true;
                            Redraw = true;
                        }
                        else
                        {
                            keep = false;
                        }
                    }

                    if (!show)
                    {
                        _subMenuItem = -1;
                    }

                    if (keep)
                    {
                    }
                }
            }

            SetVisible(show);
            if (Handled)
            {
                RaiseMouseDown(this, button, shift, X, Y);
            }
        }
    }

    // AOWMenu.pas:634
    public override void CheckMouseUp(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = false;
        if (Visible)
        {
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                UIComponent? c = Children[i];
                if ((!Handled || c.Redraw) && c.CaptureInput)
                {
                    c.CheckMouseUp(button, shift, X, Y);
                }

                if (c.Handled)
                {
                    Handled = true;
                }
            }

            if (X > WinLeft - 1 && X < WinLeft + WinWidth && Y > WinTop - 1 && Y < WinTop + WinHeight)
            {
                Handled = true;
                RaiseMouseUp(this, button, shift, X, Y);
            }
        }
    }

    // AOWMenu.pas:662
    // { CheckMouseMove highlights the item under the mouse; when the highlight moves, the submenus of
    //   the old item are hidden (by the item index into the submenu list: original bug) }
    public override void CheckMouseMove(ShiftStates shift, int X, int Y)
    {
        // { Keep the child on the stack and I in a register. }
        Handled = false;
        InResizeArea = true;
        if (Visible)
        {
            Redraw = true;
            int oldHighlighted = Highlighted;
            Cursor = -2;
            int i = Children.Count - 1;
            for (; i >= 0; i -= 1)
            {
                UIComponent? c = Children[i];
                if ((!Handled || c.Redraw || c.ToolTipCount > 0) && c.CaptureInput)
                {
                    c.CheckMouseMove(shift, X, Y);
                }

                if (c.Handled)
                {
                    Handled = true;
                }
            }

            Handled = true;
            i = -1;
            LibraryImage? imageLibraryFrameWidth = ImageLibraryFrame[ImageLibraryIndexFrame];
            bool condition = X > WinLeft + imageLibraryFrameWidth.Width - 1;
            if (condition)
            {
                LibraryImage? imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 7];
                condition = X < WinLeft + WinWidth - imageLibraryFrame.Width;
            }

            bool test = condition;
            if (test)
            {
                LibraryImage? imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame];
                test = Y > WinTop + imageLibraryFrame.Height - 1;
            }

            bool passed = test;
            if (passed)
            {
                LibraryImage? imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 7];
                passed = Y < WinTop + WinHeight - imageLibraryFrame.Height;
            }

            if (passed)
            {
                LibraryImage? imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame];
                i = (Y - WinTop - imageLibraryFrame.Height) / ItemHeight;
                RaiseMouseMove(this, shift, X, Y);
            }

            if (i > -2 && i < Items.Count)
            {
                Highlighted = i;
            }

            if (i == -1)
            {
                if (_subMenuItem > -1)
                {
                    i = 0;
                    int last = SubMenus.Count - 1;
                    for (; i <= last; i += 1)
                    {
                        Menu component = SubMenus[i];
                        if (component.Handled)
                        {
                            Highlighted = component.OwnerItem;
                        }
                    }
                }
            }

            if (oldHighlighted != Highlighted)
            {
                _showDelay = 0;
                if (oldHighlighted > -1 && SubMenus.Count > oldHighlighted)
                {
                    SubMenus[oldHighlighted].SetVisible(false);
                }

                IsDrawn = false;
                RaiseChange(this);
            }
        }
    }

    // AOWMenu.pas:714
    public override void CheckKeyPress(ref byte key)
    {
        Handled = false;
        for (int i = 0, last = Children.Count - 1; i <= last; i++)
        {
            UIComponent? c = Children[i];
            c.CheckKeyPress(ref key);
            if (c.Handled)
            {
                Handled = true;
            }
        }

        RaiseKeyPress(this, ref key);
    }

    // AOWMenu.pas:731
    public override void CheckKeyDown(ref ushort key, ShiftStates shift)
    {
        Handled = false;
        for (int i = 0, last = Children.Count - 1; i <= last; i++)
        {
            UIComponent? c = Children[i];
            c.CheckKeyDown(ref key, shift);
            if (c.Handled)
            {
                Handled = true;
            }
        }

        RaiseKeyDown(this, ref key, shift);
    }

    // AOWMenu.pas:196
    public override void DetachMenuBar() => _ownerMenuBar = null;

    // AOWMenu.pas:271
    // { SetMenu moves the menu from the submenu list of its old owner menu to the list of the new one,
    //   in priority order }
    public virtual void SetMenu(Menu? value)
    {
        if (value != _ownerMenu)
        {
            int i;
            Menu m;
            if (_ownerMenu is not null)
            {
                i = 0;
                while (i < _ownerMenu.SubMenus.Count)
                {
                    m = _ownerMenu.SubMenus[i];
                    if (m == this)
                    {
                        _ownerMenu.SubMenus.RemoveAt(i);
                        i = _ownerMenu.SubMenus.Count;
                    }

                    i += 1;
                }
            }

            _ownerMenu = value;
            if (_ownerMenu != this && _ownerMenu is not null)
            {
                i = 0;
                int newIndex = -1;
                while (i < _ownerMenu.SubMenus.Count)
                {
                    m = _ownerMenu.SubMenus[i];
                    if (m.Priority > Priority)
                    {
                        newIndex = i;
                        i = _ownerMenu.SubMenus.Count;
                    }

                    i += 1;
                }

                if (newIndex == -1)
                {
                    _ownerMenu.SubMenus.Add(this);
                }
                else
                {
                    _ownerMenu.SubMenus.Insert(newIndex, item: this);
                }
            }
        }
    }

    // AOWMenu.pas:316
    // { SetMenuBar: the same for the menu list of the menu bar }
    public virtual void SetMenuBar(MenuBar? value)
    {
        if (value != _ownerMenuBar)
        {
            int i;
            Menu m;
            if (_ownerMenuBar is not null)
            {
                i = 0;
                while (i < _ownerMenuBar.Menus.Count)
                {
                    m = _ownerMenuBar.Menus[i];
                    if (m == this)
                    {
                        _ownerMenuBar.Menus.RemoveAt(i);
                        i = _ownerMenuBar.Menus.Count;
                    }

                    i += 1;
                }
            }

            _ownerMenuBar = value;
            if (_ownerMenuBar is not null)
            {
                i = 0;
                int newIndex = -1;
                while (i < _ownerMenuBar.Menus.Count)
                {
                    m = _ownerMenuBar.Menus[i];
                    if (m.Priority > Priority)
                    {
                        newIndex = i;
                        i = _ownerMenuBar.Menus.Count;
                    }

                    i += 1;
                }

                if (newIndex == -1)
                {
                    _ownerMenuBar.Menus.Add(this);
                }
                else
                {
                    _ownerMenuBar.Menus.Insert(newIndex, item: this);
                }
            }
        }
    }

    // AOWMenu.pas:360
    public virtual void SetImageLibraryTileSelected(InterfaceImageLibrary? value)
    {
        if (value != _imageLibraryTextureSelected)
        {
            Valid = false;
            _imageLibraryTextureSelected = value;
        }
    }

    // AOWMenu.pas:191
    public void DetachMenu() => _ownerMenu = null;

    // AOWMenu.pas:369
    public void SetImageLibraryTsIndex(int value)
    {
        if (value != _imageLibraryIndexTSelected)
        {
            _imageLibraryIndexTSelected = value;
            Valid = false;
        }
    }

    // AOWMenu.pas:440
    public void SetFStrings(DelphiStringList value)
    {
        Items.Clear();
        for (int i = 0, last = value.Count - 1; i <= last; i++)
        {
            string valueValue = value[i];
            _ = Items.Add(valueValue);
        }
    }

    // AOWMenu.pas:449
    public void SetITStrings(DelphiStringList value)
    {
        ItemTypes.Clear();
        for (int i = 0, last = value.Count - 1; i <= last; i++)
        {
            string valueValue = value[i];
            _ = ItemTypes.Add(valueValue);
        }
    }

    // AOWMenu.pas:458
    public void SetPopupOwner(UIComponent value)
    {
    }

    [Published]
    public InterfaceImageLibrary? ImageLibraryTextureNormal { get => ImageLibraryTexture; set => SetImageLibraryTexture(value); }

    [Published]
    public InterfaceImageLibrary? ImageLibraryTextureSelected { get => _imageLibraryTextureSelected; set => SetImageLibraryTileSelected(value); }

    [Published]
    public int ImageLibraryIndexTNormal { get => ImageLibraryIndexTexture; set => SetImageLibraryTileIndex(value); }

    [Published]
    public int ImageLibraryIndexTSelected { get => _imageLibraryIndexTSelected; set => SetImageLibraryTsIndex(value); }

    [Published]
    public DelphiStringList Items { get; }

    [Published]
    public DelphiStringList ItemTypes { get; }

    [Published]
    public int ItemHeight { get; set; }

    [Published]
    public MenuBar? OwnerMenuBar { get => _ownerMenuBar; set => SetMenuBar(value); }

    [Published]
    public Menu? OwnerMenu { get => _ownerMenu; set => SetMenu(value); }

    [Published]
    public int SubMenuShowDelay { get; set; }

    public new event NotifyEvent<Menu> Change
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, Menu>(this, slot: nameof(Change), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, Menu>(this, slot: nameof(Change), value: value);
    }

    public new event MouseEvent<Menu> MouseDown
    {
        add => ((UIComponent)this).MouseDown += EventSlot.Add<UIComponent, Menu>(this, slot: nameof(MouseDown), value: value);
        remove => ((UIComponent)this).MouseDown -= EventSlot.Remove<UIComponent, Menu>(this, slot: nameof(MouseDown), value: value);
    }

    public new event MouseEvent<Menu> MouseUp
    {
        add => ((UIComponent)this).MouseUp += EventSlot.Add<UIComponent, Menu>(this, slot: nameof(MouseUp), value: value);
        remove => ((UIComponent)this).MouseUp -= EventSlot.Remove<UIComponent, Menu>(this, slot: nameof(MouseUp), value: value);
    }

    public new event MouseMoveEvent<Menu> MouseMove
    {
        add => ((UIComponent)this).MouseMove += EventSlot.Add<UIComponent, Menu>(this, slot: nameof(MouseMove), value: value);
        remove => ((UIComponent)this).MouseMove -= EventSlot.Remove<UIComponent, Menu>(this, slot: nameof(MouseMove), value: value);
    }

    public new event KeyPressEvent<Menu> KeyPress
    {
        add => ((UIComponent)this).KeyPress += EventSlot.Add<UIComponent, Menu>(this, slot: nameof(KeyPress), value: value);
        remove => ((UIComponent)this).KeyPress -= EventSlot.Remove<UIComponent, Menu>(this, slot: nameof(KeyPress), value: value);
    }

    public new event KeyEvent<Menu> KeyDown
    {
        add => ((UIComponent)this).KeyDown += EventSlot.Add<UIComponent, Menu>(this, slot: nameof(KeyDown), value: value);
        remove => ((UIComponent)this).KeyDown -= EventSlot.Remove<UIComponent, Menu>(this, slot: nameof(KeyDown), value: value);
    }

    public new event AoWOnDrawEvent<Menu> PreDraw
    {
        add => ((UIComponent)this).PreDraw += EventSlot.Add<UIComponent, Menu>(this, slot: nameof(PreDraw), value: value);
        remove => ((UIComponent)this).PreDraw -= EventSlot.Remove<UIComponent, Menu>(this, slot: nameof(PreDraw), value: value);
    }

    public new event AoWOnDrawEvent<Menu> Drawn
    {
        add => ((UIComponent)this).Drawn += EventSlot.Add<UIComponent, Menu>(this, slot: nameof(Drawn), value: value);
        remove => ((UIComponent)this).Drawn -= EventSlot.Remove<UIComponent, Menu>(this, slot: nameof(Drawn), value: value);
    }

    public new event NotifyEvent<Menu> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, Menu>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, Menu>(this, slot: nameof(Updated), value: value);
    }

    public new event AoWOnDrawBackEvent<Menu> DrawBack
    {
        add => ((WindowControl)this).DrawBack += EventSlot.Add<WindowControl, Menu>(this, slot: nameof(DrawBack), value: value);
        remove => ((WindowControl)this).DrawBack -= EventSlot.Remove<WindowControl, Menu>(this, slot: nameof(DrawBack), value: value);
    }

    public new event NotifyEvent<Menu> Enter
    {
        add => ((WindowControl)this).Enter += EventSlot.Add<WindowControl, Menu>(this, slot: nameof(Enter), value: value);
        remove => ((WindowControl)this).Enter -= EventSlot.Remove<WindowControl, Menu>(this, slot: nameof(Enter), value: value);
    }

    public new event NotifyEvent<Menu> Exit
    {
        add => ((WindowControl)this).Exit += EventSlot.Add<WindowControl, Menu>(this, slot: nameof(Exit), value: value);
        remove => ((WindowControl)this).Exit -= EventSlot.Remove<WindowControl, Menu>(this, slot: nameof(Exit), value: value);
    }

    public new event NotifyEvent<Menu> ChangeCursor
    {
        add => ((WindowControl)this).ChangeCursor += EventSlot.Add<WindowControl, Menu>(this, slot: nameof(ChangeCursor), value: value);
        remove => ((WindowControl)this).ChangeCursor -= EventSlot.Remove<WindowControl, Menu>(this, slot: nameof(ChangeCursor), value: value);
    }

    public new event AoWOnDrawSurface<Menu> Blitted
    {
        add => ((WindowControl)this).Blitted += EventSlot.Add<WindowControl, Menu>(this, slot: nameof(Blitted), value: value);
        remove => ((WindowControl)this).Blitted -= EventSlot.Remove<WindowControl, Menu>(this, slot: nameof(Blitted), value: value);
    }

    public new event AoWOnDrawSurface<Menu> Blitting
    {
        add => ((WindowControl)this).Blitting += EventSlot.Add<WindowControl, Menu>(this, slot: nameof(Blitting), value: value);
        remove => ((WindowControl)this).Blitting -= EventSlot.Remove<WindowControl, Menu>(this, slot: nameof(Blitting), value: value);
    }

    public new event NotifyEvent<Menu> Hide
    {
        add => ((BaseWindow)this).Hide += EventSlot.Add<BaseWindow, Menu>(this, slot: nameof(Hide), value: value);
        remove => ((BaseWindow)this).Hide -= EventSlot.Remove<BaseWindow, Menu>(this, slot: nameof(Hide), value: value);
    }

    public new event NotifyEvent<Menu> Show
    {
        add => ((BaseWindow)this).Show += EventSlot.Add<BaseWindow, Menu>(this, slot: nameof(Show), value: value);
        remove => ((BaseWindow)this).Show -= EventSlot.Remove<BaseWindow, Menu>(this, slot: nameof(Show), value: value);
    }
}
