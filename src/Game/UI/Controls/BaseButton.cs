// Translated from AoWButton.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// { The AoWButton unit of aowInt.dpl: TAOWBaseButton, the common ancestor of the push button and
//   the check box, which draws itself from a button library (three images: the left end, the
//   middle and the right end of a horizontal line) or from an icon library (one image), in the up,
//   down or lit (mouse over) state, with its text centred on it; and TAOWButton, the push button,
//   which calls OnClick when the left button is released over it. }
// Fields: FBorderHeightSelected (the text offset from the top when the button is down.
// FBorderHeight when it is up), ButtonType (btText: drawn from _imageLibraryButton, btIcon: from
// _imageLibraryIcon. set by Validate), _imageLibraryButton, _imageLibraryIcon, _imageLibraryIndexUp, _imageLibraryIndexDown, _imageLibraryIndexLit,
// FPopupMenu, FLit (the mouse is over the button). FChecked is the down state
public partial class BaseButton : Control
{
    public BaseButton(Component owner) : base(owner)
    { }

    private int _borderHeightSelected;
    private UIComponentButtonType _buttonType;
    private InterfaceImageLibrary _imageLibraryButton;
    private InterfaceImageLibrary _imageLibraryIcon;
    private int _imageLibraryIndexUp;
    private int _imageLibraryIndexDown;
    private int _imageLibraryIndexLit;
    private Menu _popupMenu;

    public bool Lit { get; protected set; }

    // AoWButton.pas:85
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

        if (_imageLibraryButton != null && _imageLibraryIndexLit != _imageLibraryIndexUp)
        {
            _imageLibraryButton = _imageLibraryButton.CheckImageLibrary(_imageLibraryIndexLit, 3, this);
        }

        if (_imageLibraryIcon != null)
        {
            _imageLibraryIcon = _imageLibraryIcon.CheckImageLibrary(_imageLibraryIndexUp, 1, this);
        }

        if (_imageLibraryIcon != null)
        {
            _imageLibraryIcon = _imageLibraryIcon.CheckImageLibrary(_imageLibraryIndexDown, 1, this);
        }

        if (_imageLibraryIcon != null && _imageLibraryIndexLit != _imageLibraryIndexUp)
        {
            _imageLibraryIcon = _imageLibraryIcon.CheckImageLibrary(_imageLibraryIndexLit, 1, this);
        }

        if (_imageLibraryButton != null)
        {
            _buttonType = UIComponentButtonType.Text;
            Valid = true;
        }
        else
        {
            if (_imageLibraryIcon != null)
            {
                _buttonType = UIComponentButtonType.Icon;
                Valid = true;
            }
        }

        base.Validate();
    }

    // AoWButton.pas:112
    public override void SetSize()
    {
        switch (_buttonType)
        {
            case UIComponentButtonType.Text:
                var imageLibraryButtonWidth = _imageLibraryButton[_imageLibraryIndexUp];
                var imageLibraryButton = _imageLibraryButton[_imageLibraryIndexUp + 2];
                if (imageLibraryButtonWidth.Width + imageLibraryButton.Width > WinWidth)
                {
                    var imageLibraryButton2 = _imageLibraryButton[_imageLibraryIndexUp];
                    var imageLibraryButton3 = _imageLibraryButton[_imageLibraryIndexUp + 2];
                    SetWinWidthWithoutInvalidate(imageLibraryButton2.Width + imageLibraryButton3.Width);
                }

                SetWinHeightWithoutInvalidate(_imageLibraryButton[_imageLibraryIndexUp].Height);
                break;
            case UIComponentButtonType.Icon:
                SetWinWidthWithoutInvalidate(_imageLibraryIcon[_imageLibraryIndexUp].Width);
                SetWinHeightWithoutInvalidate(_imageLibraryIcon[_imageLibraryIndexUp].Height);
                break;
        }

        base.SetSize();
    }

    // AoWButton.pas:78
    public override void SetVisible(bool value)
    {
        if (value != Visible)
        {
            Lit = false;
        }

        base.SetVisible(value);
    }

    // AoWButton.pas:183
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (Visible && Valid)
            {
                RaisePreDraw(this, lockedSurface);

                if (_checked)
                {
                    if (_buttonType == UIComponentButtonType.Text)
                    {
                        var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                        _imageLibraryButton.DrawHorizontalStrip(_imageLibraryIndexDown, rectangle, ref lockedSurface);
                    }
                    else
                    {
                        var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                        _imageLibraryIcon.DrawImage(_imageLibraryIndexDown, rectangle, ref lockedSurface);
                    }

                    if (_font != null)
                    {
                        _font.Color = SelectedColor;
                        var textWidth = _font.GetTextWidth(_text);
                        _font.SetText(Window.WinLeft + WinLeft + WinWidth / 2 - textWidth / 2, Window.WinTop + WinTop + _borderHeightSelected, _text, lockedSurface);
                    }
                }
                else
                {
                    if (_buttonType == UIComponentButtonType.Text)
                    {
                        if (Lit)
                        {
                            var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                            _imageLibraryButton.DrawHorizontalStrip(_imageLibraryIndexLit, rectangle, ref lockedSurface);
                        }
                        else
                        {
                            var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                            _imageLibraryButton.DrawHorizontalStrip(_imageLibraryIndexUp, rectangle, ref lockedSurface);
                        }
                    }
                    else
                    {
                        if (Lit)
                        {
                            var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                            _imageLibraryIcon.DrawImage(_imageLibraryIndexLit, rectangle, ref lockedSurface);
                        }
                        else
                        {
                            var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                            _imageLibraryIcon.DrawImage(_imageLibraryIndexUp, rectangle, ref lockedSurface);
                        }
                    }

                    if (_font != null)
                    {
                        _font.Color = Color;
                        var textWidth = _font.GetTextWidth(_text);
                        _font.SetText(Window.WinLeft + WinLeft + WinWidth / 2 - textWidth / 2, Window.WinTop + WinTop + _borderHeight, _text, lockedSurface);
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

    // AoWButton.pas:136
    public virtual void SetImageLibraryButton(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryButton)
        {
            _imageLibraryIcon = null;
            Valid = false;
            _imageLibraryButton = value;
        }
    }

    // AoWButton.pas:146
    public virtual void SetImageLibraryIcon(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryIcon)
        {
            _imageLibraryButton = null;
            Valid = false;
            _imageLibraryIcon = value;
        }
    }

    // AoWButton.pas:156
    public virtual void SetImageLibraryUIndex(int value)
    {
        if (value != _imageLibraryIndexUp)
        {
            _imageLibraryIndexUp = value;
            Valid = false;
        }
    }

    // AoWButton.pas:165
    public virtual void SetILDIndex(int value)
    {
        if (value != _imageLibraryIndexDown)
        {
            _imageLibraryIndexDown = value;
            Valid = false;
        }
    }

    // AoWButton.pas:174
    public virtual void SetILHIndex(int value)
    {
        if (value != _imageLibraryIndexLit)
        {
            _imageLibraryIndexLit = value;
            Valid = false;
        }
    }

    // AoWButton.pas:130
    public void SetGText(string value)
    {
        _text = value;
        Drawn = false;
    }

    [Published]
    public ImageLibraryFont Font { get => _font; set => _font = value; }

    [Published]
    public int BorderHeight { get => _borderHeight; set => _borderHeight = value; }

    [Published]
    public int BorderHeightSelected { get => _borderHeightSelected; set => _borderHeightSelected = value; }

    [Published]
    public InterfaceImageLibrary ImageLibraryButton { get => _imageLibraryButton; set => SetImageLibraryButton(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryIcon { get => _imageLibraryIcon; set => SetImageLibraryIcon(value); }

    [Published]
    public int ImageLibraryIndexDown { get => _imageLibraryIndexDown; set => SetILDIndex(value); }

    [Published]
    public int ImageLibraryIndexLit { get => _imageLibraryIndexLit; set => SetILHIndex(value); }

    [Published]
    public int ImageLibraryIndexUp { get => _imageLibraryIndexUp; set => SetImageLibraryUIndex(value); }

    [Published]
    public Menu PopupMenu { get => _popupMenu; set => _popupMenu = value; }

    [Published]
    public string Text { get => _text; set => SetGText(value); }

    public new event NotifyEvent<BaseButton> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, BaseButton>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, BaseButton>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<BaseButton> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, BaseButton>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, BaseButton>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<BaseButton> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, BaseButton>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, BaseButton>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<BaseButton> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, BaseButton>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, BaseButton>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<BaseButton> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, BaseButton>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, BaseButton>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<BaseButton> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, BaseButton>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, BaseButton>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<BaseButton> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, BaseButton>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, BaseButton>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<BaseButton> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, BaseButton>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, BaseButton>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<BaseButton> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, BaseButton>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, BaseButton>(this, slot: nameof(OnUpdate), value: value);
    }
}
