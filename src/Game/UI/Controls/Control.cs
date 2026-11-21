#nullable disable warnings
using Aow.Game.Draw;
using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// { The AoWControl unit of aowInt.dpl: TAOWControl, the ancestor of the controls that sit in a
//   window (buttons, edits, lists, scroll bars, ...). It knows its window and shows its tool tip. }
// Fields: FToolTipText, FToolTipShown (updates the tool tip has been visible), FAOWWindow,
// _toolTip
public partial class Control : UIComponent
{
    private int _toolTipShown;
    private BaseWindow? _window;

    // AoWControl.pas:46
    public Control(Component? owner) : base(owner)
    {
        ToolTipText = new DelphiStringList();
        CaptureInput = true;
    }

    // AoWControl.pas:53
    public override void Dispose()
    {
        ToolTipText.Dispose();
        base.Dispose();
    }

    // AoWControl.pas:84
    // { Update counts the tool tip delay and shows or hides the tool tip, and asks the owner to redraw
    //   the control's old and new places when it moved, changed size or became hidden. FOldRect holds
    //   the place of the last update as (Left, Top, Width, Height); the rectangles to redraw are in the
    //   coordinates of the window manager (the owner's position is added unless the owner is the
    //   manager's direct child... of a window placed in the manager) }
    public override void Update()
    {
        if (Valid)
        {
            Rect r;
            if (Visible)
            {
                base.Update();
                if (ToolTip is not null)
                {
                    if (ToolTipCount > 0)
                    {
                        if (_toolTipShown > 0 && MouseIn && ToolTip.ShowDuration > 0)
                        {
                            if (_toolTipShown < ToolTip.ShowDuration)
                            {
                                _toolTipShown += 1;
                            }
                            else
                            {
                                if (!ToolTip.ToolTipStatic && ToolTip is not null)
                                {
                                    ToolTip.SetVisible(false);
                                }

                                _toolTipShown = 0;
                            }
                        }

                        if (ToolTip.ToolTipDelay > ToolTipCount)
                        {
                            ToolTipCount += 1;
                        }
                        else
                        {
                            if (ToolTipText.Count > 0 && !MouseIn)
                            {
                                ToolTip.SetFStrings(ToolTipText);
                                if (!ToolTip.ToolTipStatic)
                                {
                                    ToolTip.SetVisible(true);
                                    _window.Redraw = true;
                                    ToolTip.Sized = false;
                                }

                                Rect rectangle = new(_window.AbsoluteLeft + WinLeft, _window.AbsoluteTop + WinTop, _window.AbsoluteLeft + ToolTipLeft, _window.AbsoluteTop + WinTop + WinHeight);
                                ToolTip.ControlRectangle = rectangle;

                                MouseIn = true;
                                _toolTipShown = 1;
                            }
                        }
                    }
                    else
                    {
                        if (MouseIn)
                        {
                            if (!ToolTip.ToolTipStatic && ToolTip is not null)
                            {
                                ToolTip.SetVisible(false);
                            }

                            MouseIn = false;
                        }
                    }
                }

                if (OldRectangle.Width > 0 && OldRectangle.Height > 0 && (OldRectangle.Left != WinLeft || OldRectangle.Top != WinTop || OldRectangle.Width != WinWidth || OldRectangle.Height != WinHeight))
                {
                    r = OwnerComponent.OwnerComponent.OwnerComponent is null
                        ? new Rect(OldRectangle.Left, OldRectangle.Top, OldRectangle.Left + OldRectangle.Width - 1, OldRectangle.Top + OldRectangle.Height - 1)
                        : new Rect(OwnerComponent.WindowRelativeLeft + OldRectangle.Left, OwnerComponent.WindowRelativeTop + OldRectangle.Top, OwnerComponent.WindowRelativeLeft + OldRectangle.Left + OldRectangle.Width - 1, OwnerComponent.WindowRelativeTop + OldRectangle.Top + OldRectangle.Height - 1);

                    OwnerComponent.AddDrawRectangle(ref r, sender: this);
                    IsDrawn = false;
                }
                else
                {
                    if (Masked && !IsDrawn)
                    {
                        r = OwnerComponent.OwnerComponent.OwnerComponent is null
                            ? new Rect(WinLeft, WinTop, WinLeft + WinWidth - 1, WinTop + WinHeight - 1)
                            : new Rect(OwnerComponent.WindowRelativeLeft + WinLeft, OwnerComponent.WindowRelativeTop + WinTop, OwnerComponent.WindowRelativeLeft + WinLeft + WinWidth - 1, OwnerComponent.WindowRelativeTop + WinTop + WinHeight - 1);

                        OwnerComponent.AddDrawRectangle(ref r, sender: this);
                    }
                }

                if (!IsDrawn)
                {
                    r = OwnerComponent.OwnerComponent.OwnerComponent is null
                        ? new Rect(WinLeft, WinTop, WinLeft + WinWidth - 1, WinTop + WinHeight - 1)
                        : new Rect(OwnerComponent.WindowRelativeLeft + WinLeft, OwnerComponent.WindowRelativeTop + WinTop, OwnerComponent.WindowRelativeLeft + WinLeft + WinWidth - 1, OwnerComponent.WindowRelativeTop + WinTop + WinHeight - 1);

                    int indexOf = OwnerComponent.Children.IndexOf(this);
                    OwnerComponent.InvalidateRectangle(indexOf, rectangle: ref r, sender: this);
                }

                OldRectangle = new SizedRectangle(WinLeft, WinTop, WinWidth, WinHeight);
            }
            else
            {
                if (Hidden)
                {
                    r = OwnerComponent.OwnerComponent.OwnerComponent is null
                        ? new Rect(WinLeft, WinTop, WinLeft + WinWidth - 1, WinTop + WinHeight - 1)
                        : new Rect(OwnerComponent.WindowRelativeLeft + WinLeft, OwnerComponent.WindowRelativeTop + WinTop, OwnerComponent.WindowRelativeLeft + WinLeft + WinWidth - 1, OwnerComponent.WindowRelativeTop + WinTop + WinHeight - 1);

                    OwnerComponent.AddDrawRectangle(ref r, sender: this);
                    Hidden = false;
                }
            }
        }
    }

    // AoWControl.pas:64
    public override void SetOwner(UIComponent? value)
    {
        base.SetOwner(value);
        // The owner is a UIComponent. This control sits in a window, and the cast stays.
        _window = (BaseWindow?)OwnerComponent;
    }

    // AoWControl.pas:190
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        if (ToolTip is not null)
        {
            ToolTip.SetVisible(false);
            _toolTipShown = ToolTip.ShowDuration;
            MouseIn = true;
            ToolTipCount = ToolTip.ToolTipDelay;
        }
    }

    // AoWControl.pas:201
    public override void CheckMouseMove(ShiftStates shift, int X, int Y)
    {
        if (X < _window.WinLeft + WinLeft || X > _window.WinLeft + WinLeft + WinWidth - 1 || Y < _window.WinTop + WinTop || Y > _window.WinTop + WinTop + WinHeight - 1)
        {
            ToolTipCount = 0;
        }
        else
        {
            if (ToolTip is not null && ToolTip.ToolTipDelay > ToolTipCount)
            {
                ToolTipLeft = X - _window.WinLeft;
                ToolTipCount = 1;
            }
        }
    }

    // AoWControl.pas:59
    public void SetWindow(BaseWindow? value) => SetOwner(value);

    // AoWControl.pas:70
    public void SetToolStrings(DelphiStringList value)
    {
        ToolTipText.Clear();
        for (int i = 0, last = value.Count - 1; i <= last; i++)
        {
            string valueValue = value[i];
            _ = ToolTipText.Add(valueValue);
        }
    }

    [Published]
    public BaseWindow? Window { get => _window; set => SetWindow(value); }

    [Published]
    public ToolTip ToolTip { get; set; }

    [Published]
    public DelphiStringList ToolTipText { get; }

    public new event NotifyEvent<Control> Change
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, Control>(this, slot: nameof(Change), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, Control>(this, slot: nameof(Change), value: value);
    }

    public new event MouseEvent<Control> MouseDown
    {
        add => ((UIComponent)this).MouseDown += EventSlot.Add<UIComponent, Control>(this, slot: nameof(MouseDown), value: value);
        remove => ((UIComponent)this).MouseDown -= EventSlot.Remove<UIComponent, Control>(this, slot: nameof(MouseDown), value: value);
    }

    public new event MouseEvent<Control> MouseUp
    {
        add => ((UIComponent)this).MouseUp += EventSlot.Add<UIComponent, Control>(this, slot: nameof(MouseUp), value: value);
        remove => ((UIComponent)this).MouseUp -= EventSlot.Remove<UIComponent, Control>(this, slot: nameof(MouseUp), value: value);
    }

    public new event MouseMoveEvent<Control> MouseMove
    {
        add => ((UIComponent)this).MouseMove += EventSlot.Add<UIComponent, Control>(this, slot: nameof(MouseMove), value: value);
        remove => ((UIComponent)this).MouseMove -= EventSlot.Remove<UIComponent, Control>(this, slot: nameof(MouseMove), value: value);
    }

    public new event KeyPressEvent<Control> KeyPress
    {
        add => ((UIComponent)this).KeyPress += EventSlot.Add<UIComponent, Control>(this, slot: nameof(KeyPress), value: value);
        remove => ((UIComponent)this).KeyPress -= EventSlot.Remove<UIComponent, Control>(this, slot: nameof(KeyPress), value: value);
    }

    public new event KeyEvent<Control> KeyDown
    {
        add => ((UIComponent)this).KeyDown += EventSlot.Add<UIComponent, Control>(this, slot: nameof(KeyDown), value: value);
        remove => ((UIComponent)this).KeyDown -= EventSlot.Remove<UIComponent, Control>(this, slot: nameof(KeyDown), value: value);
    }

    public new event AoWOnDrawEvent<Control> PreDraw
    {
        add => ((UIComponent)this).PreDraw += EventSlot.Add<UIComponent, Control>(this, slot: nameof(PreDraw), value: value);
        remove => ((UIComponent)this).PreDraw -= EventSlot.Remove<UIComponent, Control>(this, slot: nameof(PreDraw), value: value);
    }

    public new event AoWOnDrawEvent<Control> Drawn
    {
        add => ((UIComponent)this).Drawn += EventSlot.Add<UIComponent, Control>(this, slot: nameof(Drawn), value: value);
        remove => ((UIComponent)this).Drawn -= EventSlot.Remove<UIComponent, Control>(this, slot: nameof(Drawn), value: value);
    }

    public new event NotifyEvent<Control> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, Control>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, Control>(this, slot: nameof(Updated), value: value);
    }
}
