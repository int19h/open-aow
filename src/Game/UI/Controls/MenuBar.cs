#nullable disable warnings
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
public sealed partial class MenuBar : Control
{
    private InterfaceImageLibrary? _imageLibraryButton;
    private int _lastSelected;
    private int _imageLibraryIndexUp;
    private int _imageLibraryIndexDown;

    public List<Menu> Menus { get; }

    private bool _ignoreClickOnLastSelected;

    // AOWMenuBar.pas:65
    public MenuBar(Component owner) : base(owner)
    {
        Items = new DelphiStringList
        {
            OnChange = OnChangeText
        };
        Menus = [];
        Selected = -1;
    }

    // AOWMenuBar.pas:74
    public override void Dispose()
    {
        foreach (Menu menu in Menus)
        {
            WindowControl m = menu;
            m.DetachMenuBar();
        }

        Menus.Clear();
        base.Dispose();
    }

    // AOWMenuBar.pas:98
    public override void Validate()
    {
        if (_imageLibraryButton is not null)
        {
            _imageLibraryButton = _imageLibraryButton.CheckImageLibrary(_imageLibraryIndexUp, count: 3, sender: this);
        }

        if (_imageLibraryButton is not null)
        {
            _imageLibraryButton = _imageLibraryButton.CheckImageLibrary(_imageLibraryIndexDown, count: 3, sender: this);
        }

        if (_imageLibraryButton is not null)
        {
            Valid = true;
        }

        base.Validate();
    }

    // AOWMenuBar.pas:110
    // { The bar is at least as wide as one up and one down button per item }
    public override void SetSize()
    {
        LibraryImage? imageLibraryButtonWidth = _imageLibraryButton[_imageLibraryIndexUp];
        LibraryImage? imageLibraryButton = _imageLibraryButton[_imageLibraryIndexUp + 2];
        int count = Items.Count;
        if ((imageLibraryButtonWidth.Width + imageLibraryButton.Width) * count > WinWidth)
        {
            LibraryImage? imageLibraryButton2 = _imageLibraryButton[_imageLibraryIndexUp];
            LibraryImage? imageLibraryButton3 = _imageLibraryButton[_imageLibraryIndexUp + 2];
            int fItemsCount = Items.Count;
            SetWinWidthWithoutInvalidate((imageLibraryButton2.Width + imageLibraryButton3.Width) * fItemsCount);
        }

        SetWinHeightWithoutInvalidate(_imageLibraryButton[_imageLibraryIndexUp].Height);
        foreach (Menu menu in Menus)
        {
            menu.Aligned = false;
        }

        base.SetSize();
    }

    // AOWMenuBar.pas:122
    public override void ReAlign()
    {
        foreach (Menu menu in Menus)
        {
            menu.Aligned = false;
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
                RaisePreDraw(surfDesc: lockedSurface);

                Rect oldClip = lockedSurface.ClipRectangle;
                for (int i = 0, last = Items.Count - 1; i <= last; i++)
                {
                    if (i != Selected)
                    {
                        int count = Items.Count;
                        int fItemsCount = Items.Count;
                        SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft + (WinWidth * i / fItemsCount), Window.AbsoluteTop + WinTop, WinWidth / count, WinHeight);
                        _imageLibraryButton.DrawHorizontalStrip(_imageLibraryIndexDown, area: rectangle, ref lockedSurface);
                        if (Font is not null)
                        {
                            Font.Color = SelectedColor;
                            string item = Items[i];
                            int count2 = Items.Count;
                            Font.SetText(Window.AbsoluteLeft + WinLeft + (WinWidth * i / count2) + BorderWidth, Window.AbsoluteTop + WinTop + BorderHeight, text: item, lockedSurface);
                        }
                    }
                    else
                    {
                        int count = Items.Count;
                        int fItemsCount = Items.Count;
                        SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft + (WinWidth * i / fItemsCount), Window.AbsoluteTop + WinTop, WinWidth / count, WinHeight);
                        _imageLibraryButton.DrawHorizontalStrip(_imageLibraryIndexUp, area: rectangle, ref lockedSurface);
                        if (Font is not null)
                        {
                            Font.Color = Color;
                            string item = Items[i];
                            int count2 = Items.Count;
                            Font.SetText(Window.AbsoluteLeft + WinLeft + (WinWidth * i / count2) + BorderWidth, Window.AbsoluteTop + WinTop + BorderHeight + 1, text: item, lockedSurface);
                        }
                    }
                }

                RaiseDraw(surfDesc: lockedSurface);
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during Draw", caption: $"Error - {Name}", flags: 0);
        }
    }

    // AOWMenuBar.pas:206
    // { CheckMouseDown: a left click on an item selects it and shows its menus, or closes it when it
    //   was selected; a click outside the bar closes the selected item }
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = true;
            Redraw = true;
            if (button == MouseButton.Left)
            {
                int count = Items.Count;
                int i = (X - Window.AbsoluteLeft - WinLeft) / (WinWidth / count);
                if (i == Selected)
                {
                    Selected = -1;
                    RaiseChange();

                    IsDrawn = false;
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

                        RaiseChange();

                        IsDrawn = false;
                    }
                }
            }

            RaiseMouseDown(button, shift, X, Y);
        }
        else
        {
            if (Selected > -1)
            {
                Selected = -1;
                IsDrawn = false;
            }
        }

        _ignoreClickOnLastSelected = false;
    }

    // AOWMenuBar.pas:251
    public override void CheckMouseUp(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight && Valid && Visible)
        {
            Handled = true;
            RaiseMouseUp(button, shift, X, Y);
        }
    }

    // AOWMenuBar.pas:264
    public override void CheckMouseMove(ShiftStates shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (X >= Window.AbsoluteLeft + WinLeft && X <= Window.AbsoluteLeft + WinLeft + WinWidth - 1 && Y >= Window.AbsoluteTop + WinTop && Y <= Window.AbsoluteTop + WinTop + WinHeight - 1)
        {
            Handled = true;
            RaiseMouseMove(shift, X, Y);
        }
    }

    // AOWMenuBar.pas:277
    public override void CheckKeyPress(ref byte key) => Handled = false;

    // AOWMenuBar.pas:89
    public void SetImageLibraryButton(InterfaceImageLibrary? value)
    {
        if (value != _imageLibraryButton)
        {
            Valid = false;
            _imageLibraryButton = value;
        }
    }

    // AOWMenuBar.pas:131
    public void SetImageLibraryUIndex(int value)
    {
        if (value != _imageLibraryIndexUp)
        {
            _imageLibraryIndexUp = value;
            Valid = false;
        }
    }

    // AOWMenuBar.pas:140
    public void SetILDIndex(int value)
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
        Items.Clear();
        for (int i = 0, last = value.Count - 1; i <= last; i++)
        {
            string valueValue = value[i];
            _ = Items.Add(valueValue);
        }
    }

    [Published]
    public DelphiStringList Items { get; }

    [Published]
    public int Selected { get; set; }

    [Published]
    public InterfaceImageLibrary? ImageLibraryButton { get => _imageLibraryButton; set => SetImageLibraryButton(value); }

    [Published]
    public int ImageLibraryIndexDown { get => _imageLibraryIndexDown; set => SetILDIndex(value); }

    [Published]
    public int ImageLibraryIndexUp { get => _imageLibraryIndexUp; set => SetImageLibraryUIndex(value); }
}
