#nullable disable
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
    private readonly DelphiStringList _toolTipText;
    private int _toolTipShown;
    private BaseWindow _window;

    // AoWControl.pas:46
    public Control(Component owner) : base(owner)
    {
        _toolTipText = new DelphiStringList();
        CaptureInput = true;
    }

    // AoWControl.pas:53
    public override void Dispose()
    {
        _toolTipText.Dispose();
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
                            if (_toolTipText.Count > 0 && !MouseIn)
                            {
                                ToolTip.SetFStrings(_toolTipText);
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
                    Drawn = false;
                }
                else
                {
                    if (Masked && !Drawn)
                    {
                        r = OwnerComponent.OwnerComponent.OwnerComponent is null
                            ? new Rect(WinLeft, WinTop, WinLeft + WinWidth - 1, WinTop + WinHeight - 1)
                            : new Rect(OwnerComponent.WindowRelativeLeft + WinLeft, OwnerComponent.WindowRelativeTop + WinTop, OwnerComponent.WindowRelativeLeft + WinLeft + WinWidth - 1, OwnerComponent.WindowRelativeTop + WinTop + WinHeight - 1);

                        OwnerComponent.AddDrawRectangle(ref r, sender: this);
                    }
                }

                if (!Drawn)
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
    public override void SetOwner(UIComponent value)
    {
        base.SetOwner(value);
        // The owner is a UIComponent. This control sits in a window, and the cast stays.
        _window = (BaseWindow)OwnerComponent;
    }

    // AoWControl.pas:190
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
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
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
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
    public void SetWindow(BaseWindow value) => SetOwner(value);

    // AoWControl.pas:70
    public void SetToolStrings(DelphiStringList value)
    {
        _toolTipText.Clear();
        for (int i = 0, last = value.Count - 1; i <= last; i++)
        {
            string valueValue = value[i];
            _ = _toolTipText.Add(valueValue);
        }
    }

    [Published]
    public BaseWindow Window { get => _window; set => SetWindow(value); }

    [Published]
    public ToolTip ToolTip { get; set; }

    [Published]
    public DelphiStringList ToolTipText { get => _toolTipText; set => SetToolStrings(value); }

    public new event NotifyEvent<Control> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, Control>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, Control>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<Control> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, Control>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, Control>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<Control> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, Control>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, Control>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<Control> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, Control>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, Control>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<Control> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, Control>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, Control>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<Control> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, Control>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, Control>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<Control> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, Control>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, Control>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<Control> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, Control>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, Control>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<Control> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, Control>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, Control>(this, slot: nameof(OnUpdate), value: value);
    }
}
