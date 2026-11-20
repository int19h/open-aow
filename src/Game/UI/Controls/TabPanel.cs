#nullable disable
// Translated from AoWTabPanel.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Controls;

// { The AoWTabPanel unit of aowInt.dpl: TAOWTabPanel, a row of tabs, one per item, in a window.
//   Clicking a tab selects it; the panels (TAOWPanel) whose TabPanel is this control are shown when
//   their OwnerItem is the selected tab and hidden otherwise. }
// Fields: FItems, FSelected, FILButton, FBorderHeightSelected (the text offset of the selected
// tab), FILIndexUp, FILIndexDown, FPanels (the TAOWPanel objects of the tabs)
public partial class TabPanel : Control
{
    private int _selected;
    private InterfaceImageLibrary _imageLibraryButton;
    private int _imageLibraryIndexUp;
    private int _imageLibraryIndexDown;

    public List<Panel> Panels { get; }

    // AoWTabPanel.pas:62
    public TabPanel(Component owner) : base(owner)
    {
        Items = new DelphiStringList
        {
            OnChange = OnChangeText
        };
        Panels = [];
    }

    // AoWTabPanel.pas:70
    public override void Dispose()
    {
        for (int i = 0, last = Panels.Count - 1; i <= last; i++)
        {
            WindowControl p = Panels[i];
            p.DetachMenuBar();
        }

        Panels.Clear();
        Items.Dispose();
        base.Dispose();
    }

    // AoWTabPanel.pas:112
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

        if (_imageLibraryButton is not null && base.Font is not null)
        {
            Valid = true;
        }

        base.Validate();
    }

    // AoWTabPanel.pas:123
    public override void SetSize()
    {
        LibraryImage imageLibraryButtonWidth = _imageLibraryButton[_imageLibraryIndexUp];
        LibraryImage imageLibraryButton = _imageLibraryButton[_imageLibraryIndexUp + 2];
        int count = Items.Count;
        if ((imageLibraryButtonWidth.Width + imageLibraryButton.Width) * count > WinWidth)
        {
            LibraryImage imageLibraryButton2 = _imageLibraryButton[_imageLibraryIndexUp];
            LibraryImage imageLibraryButton3 = _imageLibraryButton[_imageLibraryIndexUp + 2];
            int fItemsCount = Items.Count;
            SetWinWidthWithoutInvalidate((imageLibraryButton2.Width + imageLibraryButton3.Width) * fItemsCount);
        }

        SetWinHeightWithoutInvalidate(_imageLibraryButton[_imageLibraryIndexUp].Height);
        if (Items.Count <= _selected)
        {
            _selected = Items.Count - 1;
        }

        base.SetSize();
    }

    // AoWTabPanel.pas:164
    // { Draw: the tabs side by side in equal shares of the width, the text centered; the selected tab
    //   with the down image and its text BorderHeightSelected pixels down }
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (Valid && Visible)
            {
                RaisePreDraw(this, surfDesc: lockedSurface);

                Rect oldClip = lockedSurface.ClipRectangle;
                for (int i = 0, last = Items.Count - 1; i <= last; i++)
                {
                    if (i != _selected)
                    {
                        int count = Items.Count;
                        int fItemsCount = Items.Count;
                        SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft + (WinWidth * i / fItemsCount), Window.AbsoluteTop + WinTop, WinWidth / count, WinHeight);
                        _imageLibraryButton.DrawHorizontalStrip(_imageLibraryIndexUp, area: rectangle, ref lockedSurface);
                        if (base.Font is not null)
                        {
                            base.Font.Color = SelectedColor;
                            string item = Items[i];
                            int textWidth = base.Font.GetTextWidth(item);
                            int count2 = Items.Count;
                            string item2 = Items[i];
                            base.Font.SetText(Window.AbsoluteLeft + WinLeft + (WinWidth * ((2 * i) + 1) / (2 * count2)) - (textWidth / 2), Window.AbsoluteTop + WinTop + BorderHeight, text: item2, lockedSurface);
                        }
                    }
                    else
                    {
                        int count = Items.Count;
                        int fItemsCount = Items.Count;
                        SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft + (WinWidth * i / fItemsCount), Window.AbsoluteTop + WinTop, WinWidth / count, WinHeight);
                        _imageLibraryButton.DrawHorizontalStrip(_imageLibraryIndexDown, area: rectangle, ref lockedSurface);
                        if (base.Font is not null)
                        {
                            base.Font.Color = Color;
                            string item = Items[i];
                            int textWidth = base.Font.GetTextWidth(item);
                            int count2 = Items.Count;
                            string item2 = Items[i];
                            base.Font.SetText(Window.AbsoluteLeft + WinLeft + (WinWidth * ((2 * i) + 1) / (2 * count2)) - (textWidth / 2), Window.AbsoluteTop + WinTop + BorderHeightSelected, text: item2, lockedSurface);
                        }
                    }
                }

                RaiseDraw(this, surfDesc: lockedSurface);
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during Draw", caption: $"Error - {Name}", flags: 0);
        }
    }

    // AoWTabPanel.pas:211
    // { CheckMouseDown: a left click on a tab selects it }
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = true;
            if (button == MouseButton.Left)
            {
                int count = Items.Count;
                SetIndex((X - Window.AbsoluteLeft - WinLeft) / (WinWidth / count));
            }

            RaiseMouseDown(this, button, shift, X, Y);
        }
    }

    // AoWTabPanel.pas:226
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight && Valid && Visible)
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
        if (X >= Window.AbsoluteLeft + WinLeft && X <= Window.AbsoluteLeft + WinLeft + WinWidth - 1 && Y >= Window.AbsoluteTop + WinTop && Y <= Window.AbsoluteTop + WinTop + WinHeight - 1)
        {
            Handled = true;
            RaiseMouseMove(this, shift, X, Y);
        }
    }

    // AoWTabPanel.pas:252
    public override void CheckKeyPress(ref byte key) => Handled = false;

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
    public void SetFStrings(DelphiStringList value)
    {
        Items.Clear();
        for (int i = 0, last = value.Count - 1; i <= last; i++)
        {
            string valueValue = value[i];
            _ = Items.Add(valueValue);
        }
    }

    // AoWTabPanel.pas:143
    // { SetIndex selects a tab: the panels of the tab are shown, the others hidden }
    public void SetIndex(int value)
    {
        if (value != _selected)
        {
            _selected = value;
            for (int i = 0, last = Panels.Count - 1; i <= last; i++)
            {
                Panel component = Panels[i];
                if (component.OwnerItem == _selected)
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
    public DelphiStringList Items { get; }

    [Published]
    public int Selected { get => _selected; set => SetIndex(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryButton { get => _imageLibraryButton; set => SetImageLibraryButton(value); }

    [Published]
    public int ImageLibraryIndexDown { get => _imageLibraryIndexDown; set => SetILDIndex(value); }

    [Published]
    public int ImageLibraryIndexUp { get => _imageLibraryIndexUp; set => SetImageLibraryUIndex(value); }

    [Published]
    public int BorderHeightSelected { get; set; }

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
