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
// _onDrawItem, _itemHeight, _ownerMenuBar, _ownerMenu, _subMenuShowDelay, _onSelect,
// FHighlighted (the item under the mouse, -1 for none), FSubMenuItem (the item whose submenus
// are shown), FSubMenus (sorted by priority), FShowDelay (updates since the highlight moved),
// FDrawItem, FDrawItemRect (the item drawn last, for OnDrawItem)
public partial class Menu : GenericWindow
{
    public UIComponent PopupOwner { get; private set; }

    private int _imageLibraryIndexTSelected;

    public int MenuLeft { get; private set; }
    public int MenuTop { get; private set; }
    public bool ShowPending { get; private set; }
    public int SelectedItem { get; set; }

    private InterfaceImageLibrary _imageLibraryTextureSelected;
    private StringList _items;
    private StringList _itemTypes;
    public NotifyEvent<Menu> _onDrawItem;
    private int _itemHeight;
    private MenuBar _ownerMenuBar;
    private Menu _ownerMenu;
    private int _subMenuShowDelay;
    public NotifyEvent<Menu> _onSelect;

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
        int I;
        Menu M;
        I = 0;
        if (_ownerMenu != null)
        {
            try
            {
                while (I < _ownerMenu.SubMenus.Count)
                {
                    M = _ownerMenu.SubMenus[I];
                    if (M == this)
                    {
                        _ownerMenu.SubMenus.RemoveAt(I);
                        I = _ownerMenu.SubMenus.Count;
                    }

                    I += 1;
                }
            }
            catch
            {
            }
        }

        I = 0;
        if (_ownerMenuBar != null)
        {
            try
            {
                while (I < _ownerMenuBar.Menus.Count)
                {
                    M = _ownerMenuBar.Menus[I];
                    if (M == this)
                    {
                        _ownerMenuBar.Menus.RemoveAt(I);
                        I = _ownerMenuBar.Menus.Count;
                    }

                    I += 1;
                }
            }
            catch
            {
            }
        }

        _items.Dispose();
        I = 0;
        int last = SubMenus.Count - 1;
        for (; I <= last; I += 1)
        {
            (SubMenus[I]).DetachMenu();
        }

        SubMenus.Clear();
        base.Dispose();
    }

    // AOWMenu.pas:159
    public override void Validate()
    {
        if (_imageLibraryFrame != null)
        {
            _imageLibraryFrame = _imageLibraryFrame.CheckImageLibrary(_imageLibraryIndexFrame, 8, this);
        }

        if (_imageLibraryTexture != null)
        {
            _imageLibraryTexture = _imageLibraryTexture.CheckImageLibrary(_imageLibraryIndexTexture, 1, this);
        }

        if (_imageLibraryTextureSelected != null)
        {
            _imageLibraryTextureSelected = _imageLibraryTextureSelected.CheckImageLibrary(_imageLibraryIndexTSelected, 1, this);
        }

        if (OwnerComponent != null && _imageLibraryFrame != null && _imageLibraryTexture != null && _imageLibraryTextureSelected != null && _font != null)
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
        BaseWindow W;
        if (_itemHeight == 0)
        {
            _itemHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 3].Height;
        }

        if (WinHeight == 0)
        {
            var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame];
            var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 7];
            var count = _items.Count;
            SetWinHeightWithoutInvalidate(imageLibraryFrame.Height + imageLibraryFrameHeight.Height + count * _itemHeight);
        }

        var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame];
        var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
        if (imageLibraryFrameWidth.Width + imageLibraryFrame2.Width > WinWidth)
        {
            var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame];
            var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
            SetWinWidthWithoutInvalidate(imageLibraryFrame.Width + imageLibraryFrame3.Width);
        }

        var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame];
        var fItemsCount = _items.Count;
        var imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
        if (imageLibraryFrame4.Height + fItemsCount * _itemHeight + imageLibraryFrame5.Height > WinHeight)
        {
            var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame];
            var count = _items.Count;
            var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
            SetWinHeightWithoutInvalidate(imageLibraryFrame.Height + count * _itemHeight + imageLibraryFrameHeight.Height);
        }

        if (_windowManager != null && _ownerMenuBar != null)
        {
            mb = _ownerMenuBar;
            W = mb.Window;
            if (W != null)
            {
                var count = mb.Items.Count;
                SetWinLeftWithoutInvalidate(W.WinLeft + mb.WinLeft + mb.WinWidth / count * _ownerMenuBar.Selected);
                SetWinTopWithoutInvalidate(_ownerMenuBar.Window.WinTop + _ownerMenuBar.WinTop + _ownerMenuBar.WinHeight);
                if (_windowManager.WinWidth < WinLeft + WinWidth)
                {
                    SetWinLeftWithoutInvalidate(_windowManager.WinWidth - WinWidth);
                }

                if (WinTop + WinHeight > _windowManager.WinHeight)
                {
                    SetWinTopWithoutInvalidate(_ownerMenuBar.Window.WinTop + _ownerMenuBar.WinTop - WinHeight);
                }
            }
        }

        if (_useManagerBorders)
        {
            if (_windowManager.BorderLeft > WinLeft)
            {
                SetWinLeftWithoutInvalidate(_windowManager.BorderLeft);
            }

            if (_windowManager.WinWidth - _windowManager.BorderRight < WinLeft + WinWidth)
            {
                SetWinLeftWithoutInvalidate(_windowManager.WinWidth - _windowManager.BorderRight - WinWidth);
            }

            if (_windowManager.BorderTop > WinTop)
            {
                SetWinTopWithoutInvalidate(_windowManager.BorderTop);
            }

            if (_windowManager.WinHeight - _windowManager.BorderBottom < WinTop + WinHeight)
            {
                SetWinTopWithoutInvalidate(_windowManager.WinHeight - _windowManager.BorderBottom - WinHeight);
            }
        }

        if (Masked)
        {
            FrameLeft = _imageLibraryFrame[_imageLibraryIndexFrame + 3].Width;
            var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
            FrameRight = WinWidth - imageLibraryFrame.Width;
            FrameTop = _imageLibraryFrame[_imageLibraryIndexFrame + 1].Height;
            var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
            FrameBottom = WinHeight - imageLibraryFrameHeight.Height;
        }

        if (_doubleBuffered && BackSurface != null)
        {
            BackSurface.Resize(WinWidth, WinHeight);
            if (Masked)
            {
                BackSurface.Clear(_maskColor);
            }
        }

        base.SetSize();
    }

    // AOWMenu.pas:465
    // { Update: after the highlight rested SubMenuShowDelay updates on an item, the submenus of the
    //   previous item are hidden and those of the highlighted item are shown beside it }
    public override void Update()
    {
        Menu M;
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
                        for (int I = 0, last = SubMenus.Count - 1; I <= last; I++)
                        {
                            M = SubMenus[I];
                            if (M._ownerItem == _subMenuItem)
                            {
                                M.SetVisible(false);
                            }
                        }
                    }

                    for (int I = 0, last = SubMenus.Count - 1; I <= last; I++)
                    {
                        M = SubMenus[I];
                        if (M._ownerItem == Highlighted)
                        {
                            var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                            M.SetWinLeft(WinLeft + WinWidth - imageLibraryFrame.Width);
                            var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame];
                            M.SetWinTop(WinTop + imageLibraryFrameHeight.Height + _itemHeight * Highlighted);
                            M.SetVisible(true);
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
        BaseWindow W = default;
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
                if (Visible == true)
                {
                    Highlighted = -1;
                    if (_windowManager != null && _ownerMenuBar != null)
                    {
                        mb = _ownerMenuBar;
                        W = mb.Window;
                        if (W != null)
                        {
                            var count = mb.Items.Count;
                            SetWinLeftWithoutInvalidate(W.WinLeft + mb.WinLeft + mb.WinWidth / count * _ownerMenuBar.Selected);
                            SetWinTopWithoutInvalidate(_ownerMenuBar.Window.WinTop + _ownerMenuBar.WinTop + _ownerMenuBar.WinHeight);
                            if (_windowManager.WinWidth < WinLeft + WinWidth)
                            {
                                SetWinLeftWithoutInvalidate(_windowManager.WinWidth - WinWidth);
                            }

                            if (WinTop + WinHeight > _windowManager.WinHeight)
                            {
                                SetWinTopWithoutInvalidate(_ownerMenuBar.Window.WinTop + _ownerMenuBar.WinTop - WinHeight);
                            }
                        }
                    }

                    Drawn = false;
                    if (!Hidden)
                    {
                        BackRectangle.Right = WinWidth;
                        BackRectangle.Bottom = WinHeight;
                        BackRectangle.Top = WinTop;
                        BackRectangle.Left = WinLeft;
                    }

                    Hidden = false;
                    ShowPending = false;
                    if (_onShow != null)
                    {
                        _onShow(this);
                    }
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
                    if (_onHide != null)
                    {
                        _onHide(this);
                    }
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
                var rectangle = new Rect(WinLeft, WinTop, WinWidth, WinHeight);
                _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle, ref lockedSurface);
                var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                var rectangle2 = new Rect(WinLeft + imageLibraryFrame4.Width, WinTop + imageLibraryFrame3.Height, WinWidth - imageLibraryFrameWidth.Width - imageLibraryFrame2.Width, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                _imageLibraryTexture.DrawTiled(_imageLibraryIndexTexture, rectangle2, ref lockedSurface);
                _font.Color = Color;
                oldClip = lockedSurface.ClipRectangle;
                if (_items.Count > 0)
                {
                    for (int I = 0, last = _items.Count - 1; I <= last; I++)
                    {
                        var imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                        if (WinLeft + WinWidth - imageLibraryFrame5.Width < oldClip.Right)
                        {
                            var imageLibraryFrame6 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                            lockedSurface.ClipRectangle.Right = WinLeft + WinWidth - imageLibraryFrame6.Width;
                        }

                        if (I < _items.Count && _items.Count > 0)
                        {
                            if (I == Highlighted)
                            {
                                _font.Color = SelectedColor;
                                var imageLibraryFrame6 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                                var imageLibraryFrame7 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                                var imageLibraryFrame8 = _imageLibraryFrame[_imageLibraryIndexFrame];
                                var imageLibraryFrame9 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                                var rectangle3 = new Rect(WinLeft + imageLibraryFrame9.Width, WinTop + imageLibraryFrame8.Height + _itemHeight * I, WinWidth - imageLibraryFrame6.Width - imageLibraryFrame7.Width, _itemHeight);
                                _imageLibraryTextureSelected.DrawTiled(_imageLibraryIndexTSelected, rectangle3, ref lockedSurface);
                            }
                            else
                            {
                                _font.Color = Color;
                            }

                            var item = _items[I];
                            var filFrameItem = _imageLibraryFrame[_imageLibraryIndexFrame];
                            var imageLibraryFrameWidth2 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                            _font.SetText(WinLeft + _borderWidth + imageLibraryFrameWidth2.Width, WinTop + _borderHeight + filFrameItem.Height + _itemHeight * I, item, lockedSurface);
                            DrawItem = I;
                            var imageLibraryFrameHeight2 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                            var imageLibraryFrameHeight3 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                            var imageLibraryFrameHeight4 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                            var imageLibraryFrameHeight5 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                            var imageLibraryFrameWidth3 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                            DrawItemRectangle = new Rect(WinLeft + imageLibraryFrameWidth3.Width, WinTop + imageLibraryFrameHeight4.Height + imageLibraryFrameHeight5.Height * I, WinLeft + WinWidth, WinTop + imageLibraryFrameHeight2.Height + imageLibraryFrameHeight3.Height * (I + 1));
                            if (_onDrawItem != null)
                            {
                                _onDrawItem(this);
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
        UIComponent C = default;
        bool show;
        bool keep;
        Menu M = default;
        Handled = false;
        Redraw = false;
        SelectedItem = -1;
        keep = true;
        if (Visible == true)
        {
            for (int I = Children.Count - 1; I >= 0; I--)
            {
                C = (UIComponent)Children[I];
                if (!Handled && C.CaptureInput)
                {
                    C.CheckMouseDown(button, shift, X, Y);
                }

                if (C.Handled == true)
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
                        for (int I = 0, last = SubMenus.Count - 1; I <= last; I++)
                        {
                            M = SubMenus[I];
                            if (M.Handled && M.Visible)
                            {
                                show = true;
                            }

                            if (M._ownerItem == Highlighted && !M.Handled)
                            {
                                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                                M.SetWinLeft(WinLeft + WinWidth - imageLibraryFrame.Width);
                                var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame];
                                M.SetWinTop(WinTop + imageLibraryFrameHeight.Height + _itemHeight * Highlighted);
                                M.SetVisible(true);
                                show = true;
                                Redraw = true;
                                _subMenuItem = Highlighted;
                            }
                        }

                        SelectedItem = Highlighted;
                        if (_onSelect != null)
                        {
                            _onSelect(this);
                        }
                    }
                }
            }
            else
            {
                show = false;
                for (int I = 0, last = SubMenus.Count - 1; I <= last; I++)
                {
                    M = SubMenus[I];
                    if (M.Handled == true)
                    {
                        if (M.Visible == true)
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

                    if (keep == true)
                    {
                    }
                }
            }

            SetVisible(show);
            if (Handled && _onMouseDown != null)
            {
                _onMouseDown(this, button, shift, X, Y);
            }
        }
    }

    // AOWMenu.pas:634
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        UIComponent C = default;
        Handled = false;
        if (Visible == true)
        {
            for (int I = Children.Count - 1; I >= 0; I--)
            {
                C = (UIComponent)Children[I];
                if ((!Handled || C.Redraw == true) && C.CaptureInput)
                {
                    C.CheckMouseUp(button, shift, X, Y);
                }

                if (C.Handled == true)
                {
                    Handled = true;
                }
            }

            if (X > WinLeft - 1 && X < WinLeft + WinWidth && Y > WinTop - 1 && Y < WinTop + WinHeight)
            {
                Handled = true;
                if (_onMouseUp != null)
                {
                    _onMouseUp(this, button, shift, X, Y);
                }
            }
        }
    }

    // AOWMenu.pas:662
    // { CheckMouseMove highlights the item under the mouse; when the highlight moves, the submenus of
    //   the old item are hidden (by the item index into the submenu list: original bug) }
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        UIComponent C = default;
        int oldHighlighted;
        int I;
        // { Keep the child on the stack and I in a register. }
        Handled = false;
        InResizeArea = true;
        if (Visible == true)
        {
            Redraw = true;
            oldHighlighted = Highlighted;
            _cursor = -2;
            I = Children.Count - 1;
            for (; I >= 0; I -= 1)
            {
                C = (UIComponent)Children[I];
                if ((!Handled || C.Redraw == true || C.ToolTipCount > 0) && C.CaptureInput)
                {
                    C.CheckMouseMove(shift, X, Y);
                }

                if (C.Handled == true)
                {
                    Handled = true;
                }
            }

            Handled = true;
            I = -1;
            var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame];
            bool condition = X > WinLeft + imageLibraryFrameWidth.Width - 1;
            if (condition)
            {
                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 7];
                condition = X < WinLeft + WinWidth - imageLibraryFrame.Width;
            }

            bool test = condition;
            if (test)
            {
                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame];
                test = Y > WinTop + imageLibraryFrame.Height - 1;
            }

            bool passed = test;
            if (passed)
            {
                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 7];
                passed = Y < WinTop + WinHeight - imageLibraryFrame.Height;
            }

            if (passed)
            {
                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame];
                I = (Y - WinTop - imageLibraryFrame.Height) / _itemHeight;
                if (_onMouseMove != null)
                {
                    _onMouseMove(this, shift, X, Y);
                }
            }

            if (I > -2 && I < _items.Count)
            {
                Highlighted = I;
            }

            if (I == -1)
            {
                if (_subMenuItem > -1)
                {
                    I = 0;
                    int last = SubMenus.Count - 1;
                    for (; I <= last; I += 1)
                    {
                        var component = SubMenus[I];
                        if (component.Handled == true)
                        {
                            this.Highlighted = component._ownerItem;
                        }
                    }
                }
            }

            if (oldHighlighted != Highlighted)
            {
                _showDelay = 0;
                if (oldHighlighted > -1 && SubMenus.Count > oldHighlighted)
                {
                    (SubMenus[oldHighlighted]).SetVisible(false);
                }

                Drawn = false;
                if (_onChange != null)
                {
                    _onChange(this);
                }
            }
        }
    }

    // AOWMenu.pas:714
    public override void CheckKeyPress(ref byte key)
    {
        UIComponent C = default;
        Handled = false;
        for (int I = 0, last = Children.Count - 1; I <= last; I++)
        {
            C = (UIComponent)Children[I];
            C.CheckKeyPress(ref key);
            if (C.Handled == true)
            {
                Handled = true;
            }
        }

        if (_onKeyPress != null)
        {
            _onKeyPress(this, ref key);
        }
    }

    // AOWMenu.pas:731
    public override void CheckKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        UIComponent C = default;
        Handled = false;
        for (int I = 0, last = Children.Count - 1; I <= last; I++)
        {
            C = (UIComponent)Children[I];
            C.CheckKeyDown(ref key, shift);
            if (C.Handled == true)
            {
                Handled = true;
            }
        }

        if (_onKeyDown != null)
        {
            _onKeyDown(this, ref key, shift);
        }
    }

    // AOWMenu.pas:196
    public override void DetachMenuBar()
    {
        _ownerMenuBar = null;
    }

    // AOWMenu.pas:271
    // { SetMenu moves the menu from the submenu list of its old owner menu to the list of the new one,
    //   in priority order }
    public virtual void SetMenu(Menu value)
    {
        int I;
        int newIndex;
        Menu M;
        if (value != _ownerMenu)
        {
            if (_ownerMenu != null)
            {
                I = 0;
                while (I < _ownerMenu.SubMenus.Count)
                {
                    M = _ownerMenu.SubMenus[I];
                    if (M == this)
                    {
                        _ownerMenu.SubMenus.RemoveAt(I);
                        I = _ownerMenu.SubMenus.Count;
                    }

                    I += 1;
                }
            }

            _ownerMenu = value;
            if (_ownerMenu != this && _ownerMenu != null)
            {
                I = 0;
                newIndex = -1;
                while (I < _ownerMenu.SubMenus.Count)
                {
                    M = _ownerMenu.SubMenus[I];
                    if (M.Priority > Priority)
                    {
                        newIndex = I;
                        I = _ownerMenu.SubMenus.Count;
                    }

                    I += 1;
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
        int I;
        int newIndex;
        Menu M;
        if (value != _ownerMenuBar)
        {
            if (_ownerMenuBar != null)
            {
                I = 0;
                while (I < _ownerMenuBar.Menus.Count)
                {
                    M = _ownerMenuBar.Menus[I];
                    if (M == this)
                    {
                        _ownerMenuBar.Menus.RemoveAt(I);
                        I = _ownerMenuBar.Menus.Count;
                    }

                    I += 1;
                }
            }

            _ownerMenuBar = value;
            if (_ownerMenuBar != null)
            {
                I = 0;
                newIndex = -1;
                while (I < _ownerMenuBar.Menus.Count)
                {
                    M = _ownerMenuBar.Menus[I];
                    if (M.Priority > Priority)
                    {
                        newIndex = I;
                        I = _ownerMenuBar.Menus.Count;
                    }

                    I += 1;
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
    public void DetachMenu()
    {
        _ownerMenu = null;
    }

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
        for (int I = 0, last = value.Count - 1; I <= last; I++)
        {
            var valueValue = value[I];
            _items.Add(valueValue);
        }
    }

    // AOWMenu.pas:449
    public void SetITStrings(StringList value)
    {
        _itemTypes.Clear();
        for (int I = 0, last = value.Count - 1; I <= last; I++)
        {
            var valueValue = value[I];
            _itemTypes.Add(valueValue);
        }
    }

    // AOWMenu.pas:458
    public void SetPopupOwner(UIComponent value)
    {
        PopupOwner = value;
    }

    [Published]
    public InterfaceImageLibrary ImageLibraryTextureNormal { get => _imageLibraryTexture; set => SetImageLibraryTexture(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryTextureSelected { get => _imageLibraryTextureSelected; set => SetImageLibraryTileSelected(value); }

    [Published]
    public new int ImageLibraryIndexFrame { get => _imageLibraryIndexFrame; set => _imageLibraryIndexFrame = value; }

    [Published]
    public int ImageLibraryIndexTNormal { get => _imageLibraryIndexTexture; set => SetImageLibraryTileIndex(value); }

    [Published]
    public int ImageLibraryIndexTSelected { get => _imageLibraryIndexTSelected; set => SetImageLibraryTsIndex(value); }

    [Published]
    public StringList Items { get => _items; set => SetFStrings(value); }

    [Published]
    public StringList ItemTypes { get => _itemTypes; set => SetITStrings(value); }

    [Published]
    public NotifyEvent<Menu> OnDrawItem { get => _onDrawItem; set => _onDrawItem = value; }

    [Published]
    public ImageLibraryFont Font { get => _font; set => _font = value; }

    [Published]
    public int ItemHeight { get => _itemHeight; set => _itemHeight = value; }

    [Published]
    public MenuBar OwnerMenuBar { get => _ownerMenuBar; set => SetMenuBar(value); }

    [Published]
    public Menu OwnerMenu { get => _ownerMenu; set => SetMenu(value); }

    [Published]
    public int OwnerItem { get => _ownerItem; set => _ownerItem = value; }

    [Published]
    public int SubMenuShowDelay { get => _subMenuShowDelay; set => _subMenuShowDelay = value; }

    [Published]
    public NotifyEvent<Menu> OnSelect { get => _onSelect; set => _onSelect = value; }

    public new NotifyEvent<Menu> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<Menu> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<Menu> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<Menu> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<Menu> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<Menu> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<Menu> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<Menu> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<Menu> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }

    public new AoWOnDrawBackEvent<Menu> OnDrawBack
    {
        get => EventSlot.Get(this, _onDrawBack);
        set => EventSlot.Set(this, ref _onDrawBack, value);
    }

    public new NotifyEvent<Menu> OnEnter
    {
        get => EventSlot.Get(this, _onEnter);
        set => EventSlot.Set(this, ref _onEnter, value);
    }

    public new NotifyEvent<Menu> OnExit
    {
        get => EventSlot.Get(this, _onExit);
        set => EventSlot.Set(this, ref _onExit, value);
    }

    public new NotifyEvent<Menu> OnChangeCursor
    {
        get => EventSlot.Get(this, _onChangeCursor);
        set => EventSlot.Set(this, ref _onChangeCursor, value);
    }

    public new AoWOnDrawSurface<Menu> OnDrawSurface
    {
        get => EventSlot.Get(this, _onDrawSurface);
        set => EventSlot.Set(this, ref _onDrawSurface, value);
    }

    public new AoWOnDrawSurface<Menu> OnPreDrawSurface
    {
        get => EventSlot.Get(this, _onPreDrawSurface);
        set => EventSlot.Set(this, ref _onPreDrawSurface, value);
    }

    public new NotifyEvent<Menu> OnHide
    {
        get => EventSlot.Get(this, _onHide);
        set => EventSlot.Set(this, ref _onHide, value);
    }

    public new NotifyEvent<Menu> OnShow
    {
        get => EventSlot.Get(this, _onShow);
        set => EventSlot.Set(this, ref _onShow, value);
    }
}
