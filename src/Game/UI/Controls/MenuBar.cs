// Translated from AOWMenuBar.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// { The MenuBar unit of aowInt.dpl: TAOWMenuBar, a horizontal bar of buttons, one per item, in a
//   window. Clicking an item selects it and shows the menus (TAOWMenu) that belong to the item
//   (their OwnerItem is the item's index); clicking it again closes it. }
// Fields: _items, _selected (-1 for none), _imageLibraryButton, LastSelected, _imageLibraryIndexUp, _imageLibraryIndexDown,
// FMenus (the TAOWMenu objects whose OwnerMenuBar is this bar), F130 (only cleared and tested:
// a click on the item last selected then does nothing)
public partial class MenuBar : Control
{
    public StringList _items;
    public int _selected;
    public InterfaceImageLibrary _imageLibraryButton;
    public int LastSelected;
    public int _imageLibraryIndexUp;
    public int _imageLibraryIndexDown;
    public List<Menu> Menus;
    public bool IgnoreClickOnLastSelected;

    // AOWMenuBar.pas:65
    public MenuBar(Component owner) : base(owner)
    {
        _items = new StringList();
        _items.OnChange = OnChangeText;
        Menus = new List<Menu>();
        _selected = -1;
    }

    // AOWMenuBar.pas:74
    public override void Dispose()
    {
        WindowControl M;
        for (int I = 0, last = Menus.Count - 1; I <= last; I++)
        {
            M = Menus[I];
            M.DetachMenuBar();
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
        var imageLibraryButtonWidth = _imageLibraryButton[_imageLibraryIndexUp];
        var imageLibraryButton = _imageLibraryButton[_imageLibraryIndexUp + 2];
        var count = _items.Count;
        if ((imageLibraryButtonWidth.Width + imageLibraryButton.Width) * count > _winWidth)
        {
            var imageLibraryButton2 = _imageLibraryButton[_imageLibraryIndexUp];
            var imageLibraryButton3 = _imageLibraryButton[_imageLibraryIndexUp + 2];
            var fItemsCount = _items.Count;
            _winWidth = (imageLibraryButton2.Width + imageLibraryButton3.Width) * fItemsCount;
        }

        _winHeight = _imageLibraryButton[_imageLibraryIndexUp].Height;
        for (int I = 0, last = Menus.Count - 1; I <= last; I++)
        {
            (Menus[I]).Aligned = false;
        }

        base.SetSize();
    }

    // AOWMenuBar.pas:122
    public override void ReAlign()
    {
        for (int I = 0, last = Menus.Count - 1; I <= last; I++)
        {
            (Menus[I]).Aligned = false;
        }

        base.ReAlign();
    }

    // AOWMenuBar.pas:160
    // { Draw: the items side by side, each in an equal share of the width; the selected one with the up
    //   image and the text one pixel lower }
    public override void Draw(LockedSurface lockedSurface)
    {
        Rect oldClip = default;
        try
        {
            if (Valid && _visible)
            {
                if (_onPreDraw != null)
                {
                    _onPreDraw(this, lockedSurface);
                }

                oldClip = lockedSurface.ClipRectangle;
                for (int I = 0, last = _items.Count - 1; I <= last; I++)
                {
                    if (I != _selected)
                    {
                        var count = _items.Count;
                        var fItemsCount = _items.Count;
                        var rectangle = new Rect(_window._winLeft + _winLeft + _winWidth * I / fItemsCount, _window._winTop + _winTop, _winWidth / count, _winHeight);
                        _imageLibraryButton.DrawHorizontalStrip(_imageLibraryIndexDown, rectangle, ref lockedSurface);
                        if (_font != null)
                        {
                            _font.Color = SelectedColor;
                            var item = _items[I];
                            var count2 = _items.Count;
                            _font.SetText(_window._winLeft + _winLeft + _winWidth * I / count2 + _borderWidth, _window._winTop + _winTop + _borderHeight, item, lockedSurface);
                        }
                    }
                    else
                    {
                        var count = _items.Count;
                        var fItemsCount = _items.Count;
                        var rectangle = new Rect(_window._winLeft + _winLeft + _winWidth * I / fItemsCount, _window._winTop + _winTop, _winWidth / count, _winHeight);
                        _imageLibraryButton.DrawHorizontalStrip(_imageLibraryIndexUp, rectangle, ref lockedSurface);
                        if (_font != null)
                        {
                            _font.Color = Color;
                            var item = _items[I];
                            var count2 = _items.Count;
                            _font.SetText(_window._winLeft + _winLeft + _winWidth * I / count2 + _borderWidth, _window._winTop + _winTop + _borderHeight + 1, item, lockedSurface);
                        }
                    }
                }

                if (_onDraw != null)
                {
                    _onDraw(this, lockedSurface);
                }
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AOWMenuBar.pas:206
    // { CheckMouseDown: a left click on an item selects it and shows its menus, or closes it when it
    //   was selected; a click outside the bar closes the selected item }
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        int I = default;
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
            Handled = true;
            Redraw = true;
            if (button == MouseButton.Left)
            {
                var count = _items.Count;
                I = (X - _window._winLeft - _winLeft) / (_winWidth / count);
                if (I == _selected)
                {
                    _selected = -1;
                    if (_onChange != null)
                    {
                        _onChange(this);
                    }

                    Drawn = false;
                }
                else
                {
                    if (!IgnoreClickOnLastSelected || (I > -1 && I != LastSelected))
                    {
                        _selected = I;
                        LastSelected = I;
                        I = 0;
                        int last = Menus.Count - 1;
                        for (; I <= last; I += 1)
                        {
                            var component = Menus[I];
                            if (component._ownerItem == this._selected)
                            {
                                component.SetVisible(true);
                            }
                        }

                        if (_onChange != null)
                        {
                            _onChange(this);
                        }

                        Drawn = false;
                    }
                }
            }

            if (_onMouseDown != null)
            {
                _onMouseDown(this, button, shift, X, Y);
            }
        }
        else
        {
            if (_selected > -1)
            {
                _selected = -1;
                Drawn = false;
            }
        }

        IgnoreClickOnLastSelected = false;
    }

    // AOWMenuBar.pas:251
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight && Valid == true && _visible == true)
        {
            Handled = true;
            if (_onMouseUp != null)
            {
                _onMouseUp(this, button, shift, X, Y);
            }
        }
    }

    // AOWMenuBar.pas:264
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (X >= _window._winLeft + _winLeft && X <= _window._winLeft + _winLeft + _winWidth - 1 && Y >= _window._winTop + _winTop && Y <= _window._winTop + _winTop + _winHeight - 1)
        {
            Handled = true;
            if (_onMouseMove != null)
            {
                _onMouseMove(this, shift, X, Y);
            }
        }
    }

    // AOWMenuBar.pas:277
    public override void CheckKeyPress(ref byte key)
    {
        Handled = false;
    }

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
    public void SetFStrings(StringList value)
    {
        _items.Clear();
        for (int I = 0, last = value.Count - 1; I <= last; I++)
        {
            var valueValue = value[I];
            _items.Add(valueValue);
        }
    }

    [Published]
    public ImageLibraryFont Font { get => _font; set => _font = value; }

    [Published]
    public StringList Items { get => _items; set => SetFStrings(value); }

    [Published]
    public int BorderWidth { get => _borderWidth; set => _borderWidth = value; }

    [Published]
    public int BorderHeight { get => _borderHeight; set => _borderHeight = value; }

    [Published]
    public int Selected { get => _selected; set => _selected = value; }

    [Published]
    public InterfaceImageLibrary ImageLibraryButton { get => _imageLibraryButton; set => SetImageLibraryButton(value); }

    [Published]
    public int ImageLibraryIndexDown { get => _imageLibraryIndexDown; set => SetILDIndex(value); }

    [Published]
    public int ImageLibraryIndexUp { get => _imageLibraryIndexUp; set => SetImageLibraryUIndex(value); }

    public new NotifyEvent<MenuBar> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<MenuBar> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<MenuBar> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<MenuBar> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<MenuBar> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<MenuBar> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<MenuBar> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<MenuBar> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<MenuBar> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }
}
