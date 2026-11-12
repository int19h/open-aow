// Translated from AoWLabel.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// { The AoWLabel unit of aowInt.dpl: TAOWLabel, a line of text in an image library font; it
//   sizes itself to its text. }
// no fields of its own
public partial class Label : Control
{
    public Label(Component owner) : base(owner)
    { }

    // AoWLabel.pas:38
    public override void Validate()
    {
        if (_font != null)
        {
            Valid = true;
        }

        Masked = true;
        base.Validate();
    }

    // AoWLabel.pas:46
    public override void SetSize()
    {
        SetWinWidthWithoutInvalidate(_font.GetTextWidth(_text));
        SetWinHeightWithoutInvalidate(_font.GetTextHeight(_text));
        base.SetSize();
    }

    // AoWLabel.pas:72
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (Valid && Visible)
            {
                RaisePreDraw(this, lockedSurface);

                _font.Color = Color;
                _font.SetText(Window.WinLeft + WinLeft, Window.WinTop + WinTop, _text, lockedSurface);
                RaiseDraw(this, lockedSurface);
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AoWLabel.pas:90
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = CaptureInput;
            RaiseMouseDown(this, button, shift, X, Y);
        }
    }

    // AoWLabel.pas:103
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = CaptureInput;
            RaiseMouseUp(this, button, shift, X, Y);
        }
    }

    // AoWLabel.pas:115
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = CaptureInput;
            RaiseMouseMove(this, shift, X, Y);
        }
    }

    // AoWLabel.pas:128
    public override void CheckKeyPress(ref byte key)
    {
        Handled = false;
    }

    // AoWLabel.pas:53
    public void SetGFont(ImageLibraryFont value)
    {
        if (value != _font)
        {
            _font = value;
            Valid = false;
        }
    }

    // AoWLabel.pas:62
    public void SetGText(string value)
    {
        if (_text != value)
        {
            _text = value;
            Sized = false;
            Aligned = false;
        }
    }

    [Published]
    public ImageLibraryFont Font { get => _font; set => SetGFont(value); }

    [Published]
    public string Text { get => _text; set => SetGText(value); }

    public new event NotifyEvent<Label> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, Label>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, Label>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<Label> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, Label>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, Label>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<Label> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, Label>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, Label>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<Label> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, Label>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, Label>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<Label> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, Label>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, Label>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<Label> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, Label>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, Label>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<Label> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, Label>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, Label>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<Label> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, Label>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, Label>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<Label> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, Label>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, Label>(this, slot: nameof(OnUpdate), value: value);
    }
}
