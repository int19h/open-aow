#nullable disable warnings
// Translated from AoWButton.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlHost;

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
public class BaseButton(Component? owner) : Control(owner)
{
    private UIComponentButtonType _buttonType;
    private InterfaceImageLibrary? _imageLibraryButton;
    private InterfaceImageLibrary? _imageLibraryIcon;
    private int _imageLibraryIndexUp;
    private int _imageLibraryIndexDown;
    private int _imageLibraryIndexLit;

    public bool Lit { get; protected set; }

    // AoWButton.pas:85
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

        if (_imageLibraryButton is not null && _imageLibraryIndexLit != _imageLibraryIndexUp)
        {
            _imageLibraryButton = _imageLibraryButton.CheckImageLibrary(_imageLibraryIndexLit, count: 3, sender: this);
        }

        if (_imageLibraryIcon is not null)
        {
            _imageLibraryIcon = _imageLibraryIcon.CheckImageLibrary(_imageLibraryIndexUp, count: 1, sender: this);
        }

        if (_imageLibraryIcon is not null)
        {
            _imageLibraryIcon = _imageLibraryIcon.CheckImageLibrary(_imageLibraryIndexDown, count: 1, sender: this);
        }

        if (_imageLibraryIcon is not null && _imageLibraryIndexLit != _imageLibraryIndexUp)
        {
            _imageLibraryIcon = _imageLibraryIcon.CheckImageLibrary(_imageLibraryIndexLit, count: 1, sender: this);
        }

        if (_imageLibraryButton is not null)
        {
            _buttonType = UIComponentButtonType.Text;
            Valid = true;
        }
        else
        {
            if (_imageLibraryIcon is not null)
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
                LibraryImage? imageLibraryButtonWidth = _imageLibraryButton[_imageLibraryIndexUp];
                LibraryImage? imageLibraryButton = _imageLibraryButton[_imageLibraryIndexUp + 2];
                if (imageLibraryButtonWidth.Width + imageLibraryButton.Width > WinWidth)
                {
                    LibraryImage? imageLibraryButton2 = _imageLibraryButton[_imageLibraryIndexUp];
                    LibraryImage? imageLibraryButton3 = _imageLibraryButton[_imageLibraryIndexUp + 2];
                    SetWinWidthWithoutInvalidate(imageLibraryButton2.Width + imageLibraryButton3.Width);
                }

                SetWinHeightWithoutInvalidate(_imageLibraryButton[_imageLibraryIndexUp].Height);
                break;
            case UIComponentButtonType.Icon:
                SetWinWidthWithoutInvalidate(_imageLibraryIcon[_imageLibraryIndexUp].Width);
                SetWinHeightWithoutInvalidate(_imageLibraryIcon[_imageLibraryIndexUp].Height);
                break;
            default:
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
                RaisePreDraw(this, surfDesc: lockedSurface);

                if (Checked)
                {
                    if (_buttonType == UIComponentButtonType.Text)
                    {
                        SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, WinWidth, WinHeight);
                        _imageLibraryButton.DrawHorizontalStrip(_imageLibraryIndexDown, area: rectangle, ref lockedSurface);
                    }
                    else
                    {
                        SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, WinWidth, WinHeight);
                        _imageLibraryIcon.DrawImage(_imageLibraryIndexDown, area: rectangle, ref lockedSurface);
                    }

                    if (base.Font is not null)
                    {
                        base.Font.Color = SelectedColor;
                        int textWidth = base.Font.GetTextWidth(base.Text);
                        base.Font.SetText(Window.AbsoluteLeft + WinLeft + (WinWidth / 2) - (textWidth / 2), Window.AbsoluteTop + WinTop + BorderHeightSelected, base.Text, lockedSurface);
                    }
                }
                else
                {
                    if (_buttonType == UIComponentButtonType.Text)
                    {
                        if (Lit)
                        {
                            SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, WinWidth, WinHeight);
                            _imageLibraryButton.DrawHorizontalStrip(_imageLibraryIndexLit, area: rectangle, ref lockedSurface);
                        }
                        else
                        {
                            SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, WinWidth, WinHeight);
                            _imageLibraryButton.DrawHorizontalStrip(_imageLibraryIndexUp, area: rectangle, ref lockedSurface);
                        }
                    }
                    else
                    {
                        if (Lit)
                        {
                            SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, WinWidth, WinHeight);
                            _imageLibraryIcon.DrawImage(_imageLibraryIndexLit, area: rectangle, ref lockedSurface);
                        }
                        else
                        {
                            SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, WinWidth, WinHeight);
                            _imageLibraryIcon.DrawImage(_imageLibraryIndexUp, area: rectangle, ref lockedSurface);
                        }
                    }

                    if (base.Font is not null)
                    {
                        base.Font.Color = Color;
                        int textWidth = base.Font.GetTextWidth(base.Text);
                        base.Font.SetText(Window.AbsoluteLeft + WinLeft + (WinWidth / 2) - (textWidth / 2), Window.AbsoluteTop + WinTop + BorderHeight, base.Text, lockedSurface);
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

    // AoWButton.pas:136
    public virtual void SetImageLibraryButton(InterfaceImageLibrary? value)
    {
        if (value != _imageLibraryButton)
        {
            _imageLibraryIcon = null;
            Valid = false;
            _imageLibraryButton = value;
        }
    }

    // AoWButton.pas:146
    public virtual void SetImageLibraryIcon(InterfaceImageLibrary? value)
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
        base.Text = value;
        IsDrawn = false;
    }

    [Published]
    public int BorderHeightSelected { get; set; }

    [Published]
    public InterfaceImageLibrary? ImageLibraryButton { get => _imageLibraryButton; set => SetImageLibraryButton(value); }

    [Published]
    public InterfaceImageLibrary? ImageLibraryIcon { get => _imageLibraryIcon; set => SetImageLibraryIcon(value); }

    [Published]
    public int ImageLibraryIndexDown { get => _imageLibraryIndexDown; set => SetILDIndex(value); }

    [Published]
    public int ImageLibraryIndexLit { get => _imageLibraryIndexLit; set => SetILHIndex(value); }

    [Published]
    public int ImageLibraryIndexUp { get => _imageLibraryIndexUp; set => SetImageLibraryUIndex(value); }

    [Published]
    public Menu? PopupMenu { get; set; }

    [Published]
    public override string Text { get => base.Text; set => SetGText(value); }

    public new event NotifyEvent<BaseButton> Changed
    {
        add => ((UIComponent)this).Changed += EventSlot.Add<UIComponent, BaseButton>(this, slot: nameof(Changed), value: value);
        remove => ((UIComponent)this).Changed -= EventSlot.Remove<UIComponent, BaseButton>(this, slot: nameof(Changed), value: value);
    }

    public new event MouseEvent<BaseButton> MousePressed
    {
        add => ((UIComponent)this).MousePressed += EventSlot.Add<UIComponent, BaseButton>(this, slot: nameof(MousePressed), value: value);
        remove => ((UIComponent)this).MousePressed -= EventSlot.Remove<UIComponent, BaseButton>(this, slot: nameof(MousePressed), value: value);
    }

    public new event MouseEvent<BaseButton> MouseReleased
    {
        add => ((UIComponent)this).MouseReleased += EventSlot.Add<UIComponent, BaseButton>(this, slot: nameof(MouseReleased), value: value);
        remove => ((UIComponent)this).MouseReleased -= EventSlot.Remove<UIComponent, BaseButton>(this, slot: nameof(MouseReleased), value: value);
    }

    public new event MouseMoveEvent<BaseButton> MouseMoved
    {
        add => ((UIComponent)this).MouseMoved += EventSlot.Add<UIComponent, BaseButton>(this, slot: nameof(MouseMoved), value: value);
        remove => ((UIComponent)this).MouseMoved -= EventSlot.Remove<UIComponent, BaseButton>(this, slot: nameof(MouseMoved), value: value);
    }

    public new event KeyPressEvent<BaseButton> CharacterTyped
    {
        add => ((UIComponent)this).CharacterTyped += EventSlot.Add<UIComponent, BaseButton>(this, slot: nameof(CharacterTyped), value: value);
        remove => ((UIComponent)this).CharacterTyped -= EventSlot.Remove<UIComponent, BaseButton>(this, slot: nameof(CharacterTyped), value: value);
    }

    public new event KeyEvent<BaseButton> KeyPressed
    {
        add => ((UIComponent)this).KeyPressed += EventSlot.Add<UIComponent, BaseButton>(this, slot: nameof(KeyPressed), value: value);
        remove => ((UIComponent)this).KeyPressed -= EventSlot.Remove<UIComponent, BaseButton>(this, slot: nameof(KeyPressed), value: value);
    }

    public new event AoWOnDrawEvent<BaseButton> Drawing
    {
        add => ((UIComponent)this).Drawing += EventSlot.Add<UIComponent, BaseButton>(this, slot: nameof(Drawing), value: value);
        remove => ((UIComponent)this).Drawing -= EventSlot.Remove<UIComponent, BaseButton>(this, slot: nameof(Drawing), value: value);
    }

    public new event AoWOnDrawEvent<BaseButton> Drawn
    {
        add => ((UIComponent)this).Drawn += EventSlot.Add<UIComponent, BaseButton>(this, slot: nameof(Drawn), value: value);
        remove => ((UIComponent)this).Drawn -= EventSlot.Remove<UIComponent, BaseButton>(this, slot: nameof(Drawn), value: value);
    }

    public new event NotifyEvent<BaseButton> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, BaseButton>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, BaseButton>(this, slot: nameof(Updated), value: value);
    }
}
