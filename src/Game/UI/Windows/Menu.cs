#nullable disable
// Translated from AOWMenu.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

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

    private InterfaceImageLibrary _imageLibraryTextureSelected;
    private readonly StringList _items;
    private readonly StringList _itemTypes;
    [Published]
    public event NotifyEvent<Menu> OnDrawItem;

    protected void RaiseDrawItem(Menu sender)
    {
        if (OnDrawItem != null)
        {
            OnDrawItem(sender);
        }
    }

    private int _itemHeight;
    private MenuBar _ownerMenuBar;
    private Menu _ownerMenu;
    private int _subMenuShowDelay;
    [Published]
    public event NotifyEvent<Menu> OnSelect;

    public int Highlighted { get; protected set; }

    private int _subMenuItem;

    public List<Menu> SubMenus { get; }

    private int _showDelay;

    public int DrawItem { get; protected set; }
    public Rect DrawItemRectangle { get; protected set; }

    // AOWMenu.pas:102
    public Menu(Component owner) : base(owner)
    {
        _items = new StringList();
        _items.OnChange = OnChangeText;
        Highlighted = -1;
        SelectedItem = -1;
        _subMenuItem = -1;
        _itemHeight = 10;
        SubMenus = new List<Menu>();
        SetVisibleWithoutUpdate(false);
    }

    // AOWMenu.pas:117
    // { Destroy removes the menu from the lists of its owner menu and menu bar and detaches its
    //   submenus }
    public override void Dispose()
    {
        int i;
        Menu m;
        i = 0;
        if (_ownerMenu != null)
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
        if (_ownerMenuBar != null)
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

        _items.Dispose();
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
        if (ImageLibraryFrame != null)
        {
            ImageLibraryFrame = ImageLibraryFrame.CheckImageLibrary(ImageLibraryIndexFrame, 8, this);
        }

        if (ImageLibraryTexture != null)
        {
            ImageLibraryTexture = ImageLibraryTexture.CheckImageLibrary(ImageLibraryIndexTexture, 1, this);
        }

        if (_imageLibraryTextureSelected != null)
        {
            _imageLibraryTextureSelected = _imageLibraryTextureSelected.CheckImageLibrary(_imageLibraryIndexTSelected, 1, this);
        }

        if (OwnerComponent != null && ImageLibraryFrame != null && ImageLibraryTexture != null && _imageLibraryTextureSelected != null && base.Font != null)
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
        MenuBar mb;
        BaseWindow w;
        if (_itemHeight == 0)
        {
            _itemHeight = ImageLibraryFrame[ImageLibraryIndexFrame + 3].Height;
        }

        if (WinHeight == 0)
        {
            LibraryImage imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame];
            LibraryImage imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame + 7];
            int count = _items.Count;
            SetWinHeightWithoutInvalidate(imageLibraryFrame.Height + imageLibraryFrameHeight.Height + (count * _itemHeight));
        }

        LibraryImage imageLibraryFrameWidth = ImageLibraryFrame[ImageLibraryIndexFrame];
        LibraryImage imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
        if (imageLibraryFrameWidth.Width + imageLibraryFrame2.Width > WinWidth)
        {
            LibraryImage imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame];
            LibraryImage imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
            SetWinWidthWithoutInvalidate(imageLibraryFrame.Width + imageLibraryFrame3.Width);
        }

        LibraryImage imageLibraryFrame4 = ImageLibraryFrame[ImageLibraryIndexFrame];
        int fItemsCount = _items.Count;
        LibraryImage imageLibraryFrame5 = ImageLibraryFrame[ImageLibraryIndexFrame + 5];
        if (imageLibraryFrame4.Height + (fItemsCount * _itemHeight) + imageLibraryFrame5.Height > WinHeight)
        {
            LibraryImage imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame];
            int count = _items.Count;
            LibraryImage imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame + 5];
            SetWinHeightWithoutInvalidate(imageLibraryFrame.Height + (count * _itemHeight) + imageLibraryFrameHeight.Height);
        }

        if (WindowManager != null && _ownerMenuBar != null)
        {
            mb = _ownerMenuBar;
            w = mb.Window;
            if (w != null)
            {
                int count = mb.Items.Count;
                SetWinLeftWithoutInvalidate(w.WinLeft + mb.WinLeft + (mb.WinWidth / count * _ownerMenuBar.Selected));
                SetWinTopWithoutInvalidate(_ownerMenuBar.Window.WinTop + _ownerMenuBar.WinTop + _ownerMenuBar.WinHeight);
                if (WindowManager.WinWidth < WinLeft + WinWidth)
                {
                    SetWinLeftWithoutInvalidate(WindowManager.WinWidth - WinWidth);
                }

                if (WinTop + WinHeight > WindowManager.WinHeight)
                {
                    SetWinTopWithoutInvalidate(_ownerMenuBar.Window.WinTop + _ownerMenuBar.WinTop - WinHeight);
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
            LibraryImage imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
            FrameRight = WinWidth - imageLibraryFrame.Width;
            FrameTop = ImageLibraryFrame[ImageLibraryIndexFrame + 1].Height;
            LibraryImage imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
            FrameBottom = WinHeight - imageLibraryFrameHeight.Height;
        }

        if (DoubleBuffered && BackSurface != null)
        {
            BackSurface.Resize(WinWidth, WinHeight);
            if (Masked)
            {
                BackSurface.Clear(MaskColor);
            }
        }

        base.SetSize();
    }

    // AOWMenu.pas:465
    // { Update: after the highlight rested SubMenuShowDelay updates on an item, the submenus of the
    //   previous item are hidden and those of the highlighted item are shown beside it }
    public override void Update()
    {
        Menu m;
        if (Valid && Visible)
        {
            base.Update();
            if (Highlighted > -1)
            {
                if (_showDelay < _subMenuShowDelay)
                {
                    _showDelay += 1;
                }
                else
                {
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
                            LibraryImage imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
                            m.SetWinLeft(WinLeft + WinWidth - imageLibraryFrame.Width);
                            LibraryImage imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame];
                            m.SetWinTop(WinTop + imageLibraryFrameHeight.Height + (_itemHeight * Highlighted));
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

        Drawn = false;
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

        Drawn = false;
    }

    // AOWMenu.pas:203
    // { SetVisible: a menu shown under a menu bar places itself under the bar's selected item, kept
    //   inside the window manager's screen; hiding it clears the bar's selection }
    public override void SetVisible(bool value)
    {
        MenuBar bar = default;
        BaseWindow w = default;
        MenuBar mb = default;
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
                    if (WindowManager != null && _ownerMenuBar != null)
                    {
                        mb = _ownerMenuBar;
                        w = mb.Window;
                        if (w != null)
                        {
                            int count = mb.Items.Count;
                            SetWinLeftWithoutInvalidate(w.WinLeft + mb.WinLeft + (mb.WinWidth / count * _ownerMenuBar.Selected));
                            SetWinTopWithoutInvalidate(_ownerMenuBar.Window.WinTop + _ownerMenuBar.WinTop + _ownerMenuBar.WinHeight);
                            if (WindowManager.WinWidth < WinLeft + WinWidth)
                            {
                                SetWinLeftWithoutInvalidate(WindowManager.WinWidth - WinWidth);
                            }

                            if (WinTop + WinHeight > WindowManager.WinHeight)
                            {
                                SetWinTopWithoutInvalidate(_ownerMenuBar.Window.WinTop + _ownerMenuBar.WinTop - WinHeight);
                            }
                        }
                    }

                    Drawn = false;
                    if (!Hidden)
                    {
                        Rect backRectangle = BackRectangle;
                        backRectangle.Right = WinWidth;
                        BackRectangle = backRectangle;
                        Rect backRectangle2 = BackRectangle;
                        backRectangle2.Bottom = WinHeight;
                        BackRectangle = backRectangle2;
                        Rect backRectangle3 = BackRectangle;
                        backRectangle3.Top = WinTop;
                        BackRectangle = backRectangle3;
                        Rect backRectangle4 = BackRectangle;
                        backRectangle4.Left = WinLeft;
                        BackRectangle = backRectangle4;
                    }

                    Hidden = false;
                    ShowPending = false;
                    RaiseShow(this);
                }
                else
                {
                    bar = _ownerMenuBar;
                    if (bar != null)
                    {
                        bar.Selected = -1;
                        bar.Drawn = false;
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
            Drawn = false;
            SetPriority(Priority);
        }
    }

    // AOWMenu.pas:501
    // { Draw: the frame, the texture, and the items; the highlighted item on the selected texture }
    public override void Draw(LockedSurface lockedSurface)
    {
        Rect oldClip;
        try
        {
            if (Valid && Visible)
            {
                SizedRectangle rectangle = new(WinLeft, WinTop, WinWidth, WinHeight);
                ImageLibraryFrame.DrawFrame(ImageLibraryIndexFrame, rectangle, ref lockedSurface);
                LibraryImage imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                LibraryImage imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
                LibraryImage imageLibraryFrameWidth = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                LibraryImage imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
                LibraryImage imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                LibraryImage imageLibraryFrame4 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                SizedRectangle rectangle2 = new(WinLeft + imageLibraryFrame4.Width, WinTop + imageLibraryFrame3.Height, WinWidth - imageLibraryFrameWidth.Width - imageLibraryFrame2.Width, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                ImageLibraryTexture.DrawTiled(ImageLibraryIndexTexture, rectangle2, ref lockedSurface);
                base.Font.Color = Color;
                oldClip = lockedSurface.ClipRectangle;
                if (_items.Count > 0)
                {
                    for (int i = 0, last = _items.Count - 1; i <= last; i++)
                    {
                        LibraryImage imageLibraryFrame5 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
                        if (WinLeft + WinWidth - imageLibraryFrame5.Width < oldClip.Right)
                        {
                            LibraryImage imageLibraryFrame6 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
                            Rect clipRectangle = lockedSurface.ClipRectangle;
                            clipRectangle.Right = WinLeft + WinWidth - imageLibraryFrame6.Width;
                            lockedSurface.ClipRectangle = clipRectangle;
                        }

                        if (i < _items.Count && _items.Count > 0)
                        {
                            if (i == Highlighted)
                            {
                                base.Font.Color = SelectedColor;
                                LibraryImage imageLibraryFrame6 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                                LibraryImage imageLibraryFrame7 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
                                LibraryImage imageLibraryFrame8 = ImageLibraryFrame[ImageLibraryIndexFrame];
                                LibraryImage imageLibraryFrame9 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                                SizedRectangle rectangle3 = new(WinLeft + imageLibraryFrame9.Width, WinTop + imageLibraryFrame8.Height + (_itemHeight * i), WinWidth - imageLibraryFrame6.Width - imageLibraryFrame7.Width, _itemHeight);
                                _imageLibraryTextureSelected.DrawTiled(_imageLibraryIndexTSelected, rectangle3, ref lockedSurface);
                            }
                            else
                            {
                                base.Font.Color = Color;
                            }

                            string item = _items[i];
                            LibraryImage filFrameItem = ImageLibraryFrame[ImageLibraryIndexFrame];
                            LibraryImage imageLibraryFrameWidth2 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                            base.Font.SetText(WinLeft + BorderWidth + imageLibraryFrameWidth2.Width, WinTop + BorderHeight + filFrameItem.Height + (_itemHeight * i), item, lockedSurface);
                            DrawItem = i;
                            LibraryImage imageLibraryFrameHeight2 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                            LibraryImage imageLibraryFrameHeight3 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                            LibraryImage imageLibraryFrameHeight4 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                            LibraryImage imageLibraryFrameHeight5 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                            LibraryImage imageLibraryFrameWidth3 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                            DrawItemRectangle = new Rect(WinLeft + imageLibraryFrameWidth3.Width, WinTop + imageLibraryFrameHeight4.Height + (imageLibraryFrameHeight5.Height * i), WinLeft + WinWidth, WinTop + imageLibraryFrameHeight2.Height + (imageLibraryFrameHeight3.Height * (i + 1)));
                            if (OnDrawItem != null)
                            {
                                OnDrawItem(this);
                            }
                        }

                        lockedSurface.ClipRectangle = oldClip;
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
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        UIComponent c = default;
        bool show;
        bool keep;
        Menu m = default;
        Handled = false;
        Redraw = false;
        SelectedItem = -1;
        keep = true;
        if (Visible)
        {
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                c = Children[i];
                if (!Handled && c.CaptureInput)
                {
                    c.CheckMouseDown(button, shift, X, Y);
                }

                if (c.Handled)
                {
                    Handled = true;
                }
            }

            show = Visible;
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
                                LibraryImage imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
                                m.SetWinLeft(WinLeft + WinWidth - imageLibraryFrame.Width);
                                LibraryImage imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame];
                                m.SetWinTop(WinTop + imageLibraryFrameHeight.Height + (_itemHeight * Highlighted));
                                m.SetVisible(true);
                                show = true;
                                Redraw = true;
                                _subMenuItem = Highlighted;
                            }
                        }

                        SelectedItem = Highlighted;
                        if (OnSelect != null)
                        {
                            OnSelect(this);
                        }
                    }
                }
            }
            else
            {
                show = false;
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
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        UIComponent c = default;
        Handled = false;
        if (Visible)
        {
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                c = Children[i];
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
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        UIComponent c = default;
        int oldHighlighted;
        int i;
        // { Keep the child on the stack and I in a register. }
        Handled = false;
        InResizeArea = true;
        if (Visible)
        {
            Redraw = true;
            oldHighlighted = Highlighted;
            Cursor = -2;
            i = Children.Count - 1;
            for (; i >= 0; i -= 1)
            {
                c = Children[i];
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
            LibraryImage imageLibraryFrameWidth = ImageLibraryFrame[ImageLibraryIndexFrame];
            bool condition = X > WinLeft + imageLibraryFrameWidth.Width - 1;
            if (condition)
            {
                LibraryImage imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 7];
                condition = X < WinLeft + WinWidth - imageLibraryFrame.Width;
            }

            bool test = condition;
            if (test)
            {
                LibraryImage imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame];
                test = Y > WinTop + imageLibraryFrame.Height - 1;
            }

            bool passed = test;
            if (passed)
            {
                LibraryImage imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 7];
                passed = Y < WinTop + WinHeight - imageLibraryFrame.Height;
            }

            if (passed)
            {
                LibraryImage imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame];
                i = (Y - WinTop - imageLibraryFrame.Height) / _itemHeight;
                RaiseMouseMove(this, shift, X, Y);
            }

            if (i > -2 && i < _items.Count)
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

                Drawn = false;
                RaiseChange(this);
            }
        }
    }

    // AOWMenu.pas:714
    public override void CheckKeyPress(ref byte key)
    {
        UIComponent c = default;
        Handled = false;
        for (int i = 0, last = Children.Count - 1; i <= last; i++)
        {
            c = Children[i];
            c.CheckKeyPress(ref key);
            if (c.Handled)
            {
                Handled = true;
            }
        }

        RaiseKeyPress(this, ref key);
    }

    // AOWMenu.pas:731
    public override void CheckKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        UIComponent c = default;
        Handled = false;
        for (int i = 0, last = Children.Count - 1; i <= last; i++)
        {
            c = Children[i];
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
    public virtual void SetMenu(Menu value)
    {
        int i;
        int newIndex;
        Menu m;
        if (value != _ownerMenu)
        {
            if (_ownerMenu != null)
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
            if (_ownerMenu != this && _ownerMenu != null)
            {
                i = 0;
                newIndex = -1;
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
                    _ownerMenu.SubMenus.Insert(newIndex, this);
                }
            }
        }
    }

    // AOWMenu.pas:316
    // { SetMenuBar: the same for the menu list of the menu bar }
    public virtual void SetMenuBar(MenuBar value)
    {
        int i;
        int newIndex;
        Menu m;
        if (value != _ownerMenuBar)
        {
            if (_ownerMenuBar != null)
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
            if (_ownerMenuBar != null)
            {
                i = 0;
                newIndex = -1;
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
                    _ownerMenuBar.Menus.Insert(newIndex, this);
                }
            }
        }
    }

    // AOWMenu.pas:360
    public virtual void SetImageLibraryTileSelected(InterfaceImageLibrary value)
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
    public void SetFStrings(StringList value)
    {
        _items.Clear();
        for (int i = 0, last = value.Count - 1; i <= last; i++)
        {
            string valueValue = value[i];
            _items.Add(valueValue);
        }
    }

    // AOWMenu.pas:449
    public void SetITStrings(StringList value)
    {
        _itemTypes.Clear();
        for (int i = 0, last = value.Count - 1; i <= last; i++)
        {
            string valueValue = value[i];
            _itemTypes.Add(valueValue);
        }
    }

    // AOWMenu.pas:458
    public void SetPopupOwner(UIComponent value)
    {
    }

    [Published]
    public InterfaceImageLibrary ImageLibraryTextureNormal { get => ImageLibraryTexture; set => SetImageLibraryTexture(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryTextureSelected { get => _imageLibraryTextureSelected; set => SetImageLibraryTileSelected(value); }

    [Published]
    public int ImageLibraryIndexTNormal { get => ImageLibraryIndexTexture; set => SetImageLibraryTileIndex(value); }

    [Published]
    public int ImageLibraryIndexTSelected { get => _imageLibraryIndexTSelected; set => SetImageLibraryTsIndex(value); }

    [Published]
    public StringList Items { get => _items; set => SetFStrings(value); }

    [Published]
    public StringList ItemTypes { get => _itemTypes; set => SetITStrings(value); }

    [Published]
    public int ItemHeight { get => _itemHeight; set => _itemHeight = value; }

    [Published]
    public MenuBar OwnerMenuBar { get => _ownerMenuBar; set => SetMenuBar(value); }

    [Published]
    public Menu OwnerMenu { get => _ownerMenu; set => SetMenu(value); }

    [Published]
    public int SubMenuShowDelay { get => _subMenuShowDelay; set => _subMenuShowDelay = value; }

    public new event NotifyEvent<Menu> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, Menu>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, Menu>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<Menu> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, Menu>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, Menu>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<Menu> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, Menu>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, Menu>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<Menu> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, Menu>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, Menu>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<Menu> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, Menu>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, Menu>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<Menu> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, Menu>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, Menu>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<Menu> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, Menu>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, Menu>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<Menu> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, Menu>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, Menu>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<Menu> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, Menu>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, Menu>(this, slot: nameof(OnUpdate), value: value);
    }

    public new event AoWOnDrawBackEvent<Menu> OnDrawBack
    {
        add => ((WindowControl)this).OnDrawBack += EventSlot.Add<WindowControl, Menu>(this, slot: nameof(OnDrawBack), value: value);
        remove => ((WindowControl)this).OnDrawBack -= EventSlot.Remove<WindowControl, Menu>(this, slot: nameof(OnDrawBack), value: value);
    }

    public new event NotifyEvent<Menu> OnEnter
    {
        add => ((WindowControl)this).OnEnter += EventSlot.Add<WindowControl, Menu>(this, slot: nameof(OnEnter), value: value);
        remove => ((WindowControl)this).OnEnter -= EventSlot.Remove<WindowControl, Menu>(this, slot: nameof(OnEnter), value: value);
    }

    public new event NotifyEvent<Menu> OnExit
    {
        add => ((WindowControl)this).OnExit += EventSlot.Add<WindowControl, Menu>(this, slot: nameof(OnExit), value: value);
        remove => ((WindowControl)this).OnExit -= EventSlot.Remove<WindowControl, Menu>(this, slot: nameof(OnExit), value: value);
    }

    public new event NotifyEvent<Menu> OnChangeCursor
    {
        add => ((WindowControl)this).OnChangeCursor += EventSlot.Add<WindowControl, Menu>(this, slot: nameof(OnChangeCursor), value: value);
        remove => ((WindowControl)this).OnChangeCursor -= EventSlot.Remove<WindowControl, Menu>(this, slot: nameof(OnChangeCursor), value: value);
    }

    public new event AoWOnDrawSurface<Menu> OnDrawSurface
    {
        add => ((WindowControl)this).OnDrawSurface += EventSlot.Add<WindowControl, Menu>(this, slot: nameof(OnDrawSurface), value: value);
        remove => ((WindowControl)this).OnDrawSurface -= EventSlot.Remove<WindowControl, Menu>(this, slot: nameof(OnDrawSurface), value: value);
    }

    public new event AoWOnDrawSurface<Menu> OnPreDrawSurface
    {
        add => ((WindowControl)this).OnPreDrawSurface += EventSlot.Add<WindowControl, Menu>(this, slot: nameof(OnPreDrawSurface), value: value);
        remove => ((WindowControl)this).OnPreDrawSurface -= EventSlot.Remove<WindowControl, Menu>(this, slot: nameof(OnPreDrawSurface), value: value);
    }

    public new event NotifyEvent<Menu> OnHide
    {
        add => ((BaseWindow)this).OnHide += EventSlot.Add<BaseWindow, Menu>(this, slot: nameof(OnHide), value: value);
        remove => ((BaseWindow)this).OnHide -= EventSlot.Remove<BaseWindow, Menu>(this, slot: nameof(OnHide), value: value);
    }

    public new event NotifyEvent<Menu> OnShow
    {
        add => ((BaseWindow)this).OnShow += EventSlot.Add<BaseWindow, Menu>(this, slot: nameof(OnShow), value: value);
        remove => ((BaseWindow)this).OnShow -= EventSlot.Remove<BaseWindow, Menu>(this, slot: nameof(OnShow), value: value);
    }
}
