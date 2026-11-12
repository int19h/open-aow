// Translated from AoWTabPanel.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// { The AoWTabPanel unit of aowInt.dpl: TAOWTabPanel, a row of tabs, one per item, in a window.
//   Clicking a tab selects it; the panels (TAOWPanel) whose TabPanel is this control are shown when
//   their OwnerItem is the selected tab and hidden otherwise. }
// Fields: FItems, FSelected, FILButton, FBorderHeightSelected (the text offset of the selected
// tab), FILIndexUp, FILIndexDown, FPanels (the TAOWPanel objects of the tabs)
public partial class TabPanel : Control
{
    private StringList _items;
    private int _selected;
    private InterfaceImageLibrary _imageLibraryButton;
    private int _borderHeightSelected;
    private int _imageLibraryIndexUp;
    private int _imageLibraryIndexDown;

    public List<Panel> Panels { get; }

    // AoWTabPanel.pas:62
    public TabPanel(Component owner) : base(owner)
    {
        _items = new StringList();
        _items.OnChange = OnChangeText;
        Panels = new List<Panel>();
    }

    // AoWTabPanel.pas:70
    public override void Dispose()
    {
        WindowControl P;
        for (int I = 0, last = Panels.Count - 1; I <= last; I++)
        {
            P = Panels[I];
            P.DetachMenuBar();
        }

        Panels.Clear();
        _items.Dispose();
        base.Dispose();
    }

    // AoWTabPanel.pas:112
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

        if (_imageLibraryButton != null && _font != null)
        {
            Valid = true;
        }

        base.Validate();
    }

    // AoWTabPanel.pas:123
    public override void SetSize()
    {
        var imageLibraryButtonWidth = _imageLibraryButton[_imageLibraryIndexUp];
        var imageLibraryButton = _imageLibraryButton[_imageLibraryIndexUp + 2];
        var count = _items.Count;
        if ((imageLibraryButtonWidth.Width + imageLibraryButton.Width) * count > WinWidth)
        {
            var imageLibraryButton2 = _imageLibraryButton[_imageLibraryIndexUp];
            var imageLibraryButton3 = _imageLibraryButton[_imageLibraryIndexUp + 2];
            var fItemsCount = _items.Count;
            SetWinWidthWithoutInvalidate((imageLibraryButton2.Width + imageLibraryButton3.Width) * fItemsCount);
        }

        SetWinHeightWithoutInvalidate(_imageLibraryButton[_imageLibraryIndexUp].Height);
        if (_items.Count <= _selected)
        {
            _selected = _items.Count - 1;
        }

        base.SetSize();
    }

    // AoWTabPanel.pas:164
    // { Draw: the tabs side by side in equal shares of the width, the text centered; the selected tab
    //   with the down image and its text BorderHeightSelected pixels down }
    public override void Draw(LockedSurface lockedSurface)
    {
        Rect oldClip = default;
        try
        {
            if (Valid && Visible)
            {
                RaisePreDraw(this, lockedSurface);

                oldClip = lockedSurface.ClipRectangle;
                for (int I = 0, last = _items.Count - 1; I <= last; I++)
                {
                    if (I != _selected)
                    {
                        var count = _items.Count;
                        var fItemsCount = _items.Count;
                        var rectangle = new Rect(Window.WinLeft + WinLeft + WinWidth * I / fItemsCount, Window.WinTop + WinTop, WinWidth / count, WinHeight);
                        _imageLibraryButton.DrawHorizontalStrip(_imageLibraryIndexUp, rectangle, ref lockedSurface);
                        if (_font != null)
                        {
                            _font.Color = SelectedColor;
                            var item = _items[I];
                            var textWidth = _font.GetTextWidth(item);
                            var count2 = _items.Count;
                            var item2 = _items[I];
                            _font.SetText(Window.WinLeft + WinLeft + WinWidth * (2 * I + 1) / (2 * count2) - textWidth / 2, Window.WinTop + WinTop + _borderHeight, item2, lockedSurface);
                        }
                    }
                    else
                    {
                        var count = _items.Count;
                        var fItemsCount = _items.Count;
                        var rectangle = new Rect(Window.WinLeft + WinLeft + WinWidth * I / fItemsCount, Window.WinTop + WinTop, WinWidth / count, WinHeight);
                        _imageLibraryButton.DrawHorizontalStrip(_imageLibraryIndexDown, rectangle, ref lockedSurface);
                        if (_font != null)
                        {
                            _font.Color = Color;
                            var item = _items[I];
                            var textWidth = _font.GetTextWidth(item);
                            var count2 = _items.Count;
                            var item2 = _items[I];
                            _font.SetText(Window.WinLeft + WinLeft + WinWidth * (2 * I + 1) / (2 * count2) - textWidth / 2, Window.WinTop + WinTop + _borderHeightSelected, item2, lockedSurface);
                        }
                    }
                }

                RaiseDraw(this, lockedSurface);
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AoWTabPanel.pas:211
    // { CheckMouseDown: a left click on a tab selects it }
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = true;
            if (button == MouseButton.Left)
            {
                var count = _items.Count;
                SetIndex((X - Window.WinLeft - WinLeft) / (WinWidth / count));
            }

            RaiseMouseDown(this, button, shift, X, Y);
        }
    }

    // AoWTabPanel.pas:226
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight && Valid == true && Visible == true)
        {
            Handled = true;
            RaiseMouseUp(this, button, shift, X, Y);
        }
    }

    // AoWTabPanel.pas:239
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (X >= Window.WinLeft + WinLeft && X <= Window.WinLeft + WinLeft + WinWidth - 1 && Y >= Window.WinTop + WinTop && Y <= Window.WinTop + WinTop + WinHeight - 1)
        {
            Handled = true;
            RaiseMouseMove(this, shift, X, Y);
        }
    }

    // AoWTabPanel.pas:252
    public override void CheckKeyPress(ref byte key)
    {
        Handled = false;
    }

    // AoWTabPanel.pas:85
    public virtual void SetImageLibraryButton(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryButton)
        {
            Valid = false;
            _imageLibraryButton = value;
        }
    }

    // AoWTabPanel.pas:94
    public virtual void SetImageLibraryUIndex(int value)
    {
        if (value != _imageLibraryIndexUp)
        {
            _imageLibraryIndexUp = value;
            Valid = false;
        }
    }

    // AoWTabPanel.pas:103
    public virtual void SetILDIndex(int value)
    {
        if (value != _imageLibraryIndexDown)
        {
            _imageLibraryIndexDown = value;
            Valid = false;
        }
    }

    // AoWTabPanel.pas:133
    public void SetFStrings(StringList value)
    {
        _items.Clear();
        for (int I = 0, last = value.Count - 1; I <= last; I++)
        {
            var valueValue = value[I];
            _items.Add(valueValue);
        }
    }

    // AoWTabPanel.pas:143
    // { SetIndex selects a tab: the panels of the tab are shown, the others hidden }
    public void SetIndex(int value)
    {
        if (value != _selected)
        {
            _selected = value;
            for (int I = 0, last = Panels.Count - 1; I <= last; I++)
            {
                var component = Panels[I];
                if (component._ownerItem == this._selected)
                {
                    component.SetVisible(true);
                }
                else
                {
                    component.SetVisible(false);
                }
            }

            RaiseChange(this);

            Drawn = false;
        }
    }

    [Published]
    public ImageLibraryFont Font { get => _font; set => _font = value; }

    [Published]
    public StringList Items { get => _items; set => SetFStrings(value); }

    [Published]
    public int BorderHeight { get => _borderHeight; set => _borderHeight = value; }

    [Published]
    public int Selected { get => _selected; set => SetIndex(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryButton { get => _imageLibraryButton; set => SetImageLibraryButton(value); }

    [Published]
    public int ImageLibraryIndexDown { get => _imageLibraryIndexDown; set => SetILDIndex(value); }

    [Published]
    public int ImageLibraryIndexUp { get => _imageLibraryIndexUp; set => SetImageLibraryUIndex(value); }

    [Published]
    public int BorderHeightSelected { get => _borderHeightSelected; set => _borderHeightSelected = value; }

    public new event NotifyEvent<TabPanel> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, TabPanel>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, TabPanel>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<TabPanel> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, TabPanel>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, TabPanel>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<TabPanel> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, TabPanel>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, TabPanel>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<TabPanel> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, TabPanel>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, TabPanel>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<TabPanel> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, TabPanel>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, TabPanel>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<TabPanel> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, TabPanel>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, TabPanel>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<TabPanel> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, TabPanel>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, TabPanel>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<TabPanel> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, TabPanel>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, TabPanel>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<TabPanel> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, TabPanel>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, TabPanel>(this, slot: nameof(OnUpdate), value: value);
    }
}
