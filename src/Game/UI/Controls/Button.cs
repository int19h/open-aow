// Translated from AoWButton.pas.
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// Fields: OnClick
public partial class Button : BaseButton
{
    public Button(Component owner) : base(owner)
    { }

    [Published]
    public event NotifyEvent<Button> OnClick;

    internal bool HasClickHandlers => OnClick != null;

    internal void RaiseClick(Button sender)
    {
        if (OnClick != null)
        {
            OnClick(sender);
        }
    }

    // AoWButton.pas:239
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        Lit = false;
        if (!Window.Handled)
        {
            if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
            {
                Handled = true;
                if (button == MouseButton.Left)
                {
                    MouseDown = true;
                    _checked = true;
                    Drawn = false;
                    Redraw = true;
                }

                RaiseMouseDown(this, button, shift, X, Y);
            }
        }
    }

    // AoWButton.pas:261
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        _checked = false;
        Lit = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = true;
            if (button == MouseButton.Left)
            {
                if (MouseDown && OnClick != null)
                {
                    OnClick(this);
                }

                Drawn = false;
            }

            RaiseMouseUp(this, button, shift, X, Y);
        }

        if (MouseDown)
        {
            Drawn = false;
        }

        MouseDown = false;
    }

    // AoWButton.pas:284
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        bool oldChecked;
        bool oldLit;
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        oldChecked = _checked;
        oldLit = Lit;
        Redraw = false;
        if (Window.Handled || X < Window.WinLeft + WinLeft || X > Window.WinLeft + WinLeft + WinWidth - 1 || Y < Window.WinTop + WinTop || Y > Window.WinTop + WinTop + WinHeight - 1)
        {
            _checked = false;
            Lit = false;
        }
        else
        {
            Handled = true;
            _checked = MouseDown;
            Lit = !MouseDown;
            Redraw = true;
            RaiseMouseMove(this, shift, X, Y);
        }

        if (oldLit != Lit || oldChecked != _checked)
        {
            Drawn = false;
        }
    }

    public new event NotifyEvent<Button> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, Button>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<Button> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, Button>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<Button> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, Button>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<Button> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, Button>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<Button> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, Button>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<Button> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, Button>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<Button> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, Button>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<Button> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, Button>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<Button> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, Button>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(OnUpdate), value: value);
    }
}
