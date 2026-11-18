#nullable disable
// Translated from AOWMenuBar.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Controls;

// { The MenuBar unit of aowInt.dpl: TAOWMenuBar, a horizontal bar of buttons, one per item, in a
//   window. Clicking an item selects it and shows the menus (TAOWMenu) that belong to the item
//   (their OwnerItem is the item's index); clicking it again closes it. }
// Fields: _items, _selected (-1 for none), _imageLibraryButton, LastSelected, _imageLibraryIndexUp, _imageLibraryIndexDown,
// FMenus (the TAOWMenu objects whose OwnerMenuBar is this bar), F130 (only cleared and tested:
// a click on the item last selected then does nothing)
public partial class MenuBar : Control
{
    private readonly DelphiStringList _items;
    private InterfaceImageLibrary _imageLibraryButton;
    private int _lastSelected;
    private int _imageLibraryIndexUp;
    private int _imageLibraryIndexDown;

    public List<Menu> Menus { get; }

    private bool _ignoreClickOnLastSelected;

    // AOWMenuBar.pas:65
    public MenuBar(Component owner) : base(owner)
    {
        _items = new DelphiStringList
        {
            OnChange = OnChangeText
        };
        Menus = [];
        Selected = -1;
    }

    // AOWMenuBar.pas:74
    public override void Dispose()
    {
        WindowControl m;
        for (int i = 0, last = Menus.Count - 1; i <= last; i++)
        {
            m = Menus[i];
            m.DetachMenuBar();
        }

        Menus.Clear();
        _items.Dispose();
        base.Dispose();
    }

    // AOWMenuBar.pas:98
    public override void Validate()
    {
        if (_imageLibraryButton != null)
        {
            _imageLibraryButton = _imageLibraryButton.CheckImageLibrary(_imageLibraryIndexUp, 3, this);
        }

        if (_imageLibraryButton != null)
        {
            _imageLibraryButton = _imageLibraryButton.CheckImageLibrary(_imageLibraryIndexDown, 3, this);
        }

        if (_imageLibraryButton != null)
        {
            Valid = true;
        }

        base.Validate();
    }

    // AOWMenuBar.pas:110
    // { The bar is at least as wide as one up and one down button per item }
    public override void SetSize()
    {
        LibraryImage imageLibraryButtonWidth = _imageLibraryButton[_imageLibraryIndexUp];
        LibraryImage imageLibraryButton = _imageLibraryButton[_imageLibraryIndexUp + 2];
        int count = _items.Count;
        if ((imageLibraryButtonWidth.Width + imageLibraryButton.Width) * count > WinWidth)
        {
            LibraryImage imageLibraryButton2 = _imageLibraryButton[_imageLibraryIndexUp];
            LibraryImage imageLibraryButton3 = _imageLibraryButton[_imageLibraryIndexUp + 2];
            int fItemsCount = _items.Count;
            SetWinWidthWithoutInvalidate((imageLibraryButton2.Width + imageLibraryButton3.Width) * fItemsCount);
        }

        SetWinHeightWithoutInvalidate(_imageLibraryButton[_imageLibraryIndexUp].Height);
        for (int i = 0, last = Menus.Count - 1; i <= last; i++)
        {
            Menus[i].Aligned = false;
        }

        base.SetSize();
    }

    // AOWMenuBar.pas:122
    public override void ReAlign()
    {
        for (int i = 0, last = Menus.Count - 1; i <= last; i++)
        {
            Menus[i].Aligned = false;
        }

        base.ReAlign();
    }

    // AOWMenuBar.pas:160
    // { Draw: the items side by side, each in an equal share of the width; the selected one with the up
    //   image and the text one pixel lower }
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (Valid && Visible)
            {
                RaisePreDraw(this, lockedSurface);

                Rect oldClip = lockedSurface.ClipRectangle;
                for (int i = 0, last = _items.Count - 1; i <= last; i++)
                {
                    if (i != Selected)
                    {
                        int count = _items.Count;
                        int fItemsCount = _items.Count;
                        SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft + (WinWidth * i / fItemsCount), Window.AbsoluteTop + WinTop, WinWidth / count, WinHeight);
                        _imageLibraryButton.DrawHorizontalStrip(_imageLibraryIndexDown, rectangle, ref lockedSurface);
                        if (base.Font != null)
                        {
                            base.Font.Color = SelectedColor;
                            string item = _items[i];
                            int count2 = _items.Count;
                            base.Font.SetText(Window.AbsoluteLeft + WinLeft + (WinWidth * i / count2) + BorderWidth, Window.AbsoluteTop + WinTop + BorderHeight, item, lockedSurface);
                        }
                    }
                    else
                    {
                        int count = _items.Count;
                        int fItemsCount = _items.Count;
                        SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft + (WinWidth * i / fItemsCount), Window.AbsoluteTop + WinTop, WinWidth / count, WinHeight);
                        _imageLibraryButton.DrawHorizontalStrip(_imageLibraryIndexUp, rectangle, ref lockedSurface);
                        if (base.Font != null)
                        {
                            base.Font.Color = Color;
                            string item = _items[i];
                            int count2 = _items.Count;
                            base.Font.SetText(Window.AbsoluteLeft + WinLeft + (WinWidth * i / count2) + BorderWidth, Window.AbsoluteTop + WinTop + BorderHeight + 1, item, lockedSurface);
                        }
                    }
                }

                RaiseDraw(this, lockedSurface);
            }
        }
        catch
        {
            _ = MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AOWMenuBar.pas:206
    // { CheckMouseDown: a left click on an item selects it and shows its menus, or closes it when it
    //   was selected; a click outside the bar closes the selected item }
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = true;
            Redraw = true;
            if (button == MouseButton.Left)
            {
                int count = _items.Count;
                int i = (X - Window.AbsoluteLeft - WinLeft) / (WinWidth / count);
                if (i == Selected)
                {
                    Selected = -1;
                    RaiseChange(this);

                    Drawn = false;
                }
                else
                {
                    if (!_ignoreClickOnLastSelected || (i > -1 && i != _lastSelected))
                    {
                        Selected = i;
                        _lastSelected = i;
                        i = 0;
                        int last = Menus.Count - 1;
                        for (; i <= last; i += 1)
                        {
                            Menu component = Menus[i];
                            if (component.OwnerItem == Selected)
                            {
                                component.SetVisible(true);
                            }
                        }

                        RaiseChange(this);

                        Drawn = false;
                    }
                }
            }

            RaiseMouseDown(this, button, shift, X, Y);
        }
        else
        {
            if (Selected > -1)
            {
                Selected = -1;
                Drawn = false;
            }
        }

        _ignoreClickOnLastSelected = false;
    }

    // AOWMenuBar.pas:251
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight && Valid && Visible)
        {
            Handled = true;
            RaiseMouseUp(this, button, shift, X, Y);
        }
    }

    // AOWMenuBar.pas:264
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (X >= Window.AbsoluteLeft + WinLeft && X <= Window.AbsoluteLeft + WinLeft + WinWidth - 1 && Y >= Window.AbsoluteTop + WinTop && Y <= Window.AbsoluteTop + WinTop + WinHeight - 1)
        {
            Handled = true;
            RaiseMouseMove(this, shift, X, Y);
        }
    }

    // AOWMenuBar.pas:277
    public override void CheckKeyPress(ref byte key) => Handled = false;

    // AOWMenuBar.pas:89
    public virtual void SetImageLibraryButton(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryButton)
        {
            Valid = false;
            _imageLibraryButton = value;
        }
    }

    // AOWMenuBar.pas:131
    public virtual void SetImageLibraryUIndex(int value)
    {
        if (value != _imageLibraryIndexUp)
        {
            _imageLibraryIndexUp = value;
            Valid = false;
        }
    }

    // AOWMenuBar.pas:140
    public virtual void SetILDIndex(int value)
    {
        if (value != _imageLibraryIndexDown)
        {
            _imageLibraryIndexDown = value;
            Valid = false;
        }
    }

    // AOWMenuBar.pas:149
    public void SetFStrings(DelphiStringList value)
    {
        _items.Clear();
        for (int i = 0, last = value.Count - 1; i <= last; i++)
        {
            string valueValue = value[i];
            _ = _items.Add(valueValue);
        }
    }

    [Published]
    public DelphiStringList Items { get => _items; set => SetFStrings(value); }

    [Published]
    public int Selected { get; set; }

    [Published]
    public InterfaceImageLibrary ImageLibraryButton { get => _imageLibraryButton; set => SetImageLibraryButton(value); }

    [Published]
    public int ImageLibraryIndexDown { get => _imageLibraryIndexDown; set => SetILDIndex(value); }

    [Published]
    public int ImageLibraryIndexUp { get => _imageLibraryIndexUp; set => SetImageLibraryUIndex(value); }

    public new event NotifyEvent<MenuBar> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, MenuBar>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, MenuBar>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<MenuBar> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, MenuBar>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, MenuBar>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<MenuBar> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, MenuBar>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, MenuBar>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<MenuBar> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, MenuBar>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, MenuBar>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<MenuBar> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, MenuBar>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, MenuBar>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<MenuBar> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, MenuBar>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, MenuBar>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<MenuBar> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, MenuBar>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, MenuBar>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<MenuBar> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, MenuBar>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, MenuBar>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<MenuBar> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, MenuBar>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, MenuBar>(this, slot: nameof(OnUpdate), value: value);
    }
}
