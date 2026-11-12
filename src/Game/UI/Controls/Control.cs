using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// { The AoWControl unit of aowInt.dpl: TAOWControl, the ancestor of the controls that sit in a
//   window (buttons, edits, lists, scroll bars, ...). It knows its window and shows its tool tip. }
// Fields: FToolTipText, FToolTipShown (updates the tool tip has been visible), FAOWWindow,
// _toolTip
public partial class Control : UIComponent
{
    private readonly StringList _toolTipText;
    private int _toolTipShown;
    private BaseWindow _window;
    private ToolTip _toolTip;

    // AoWControl.pas:46
    public Control(Component owner) : base(owner)
    {
        _toolTipText = new StringList();
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
        Rect R = default;
        BaseWindow W;
        if (Valid)
        {
            if (Visible)
            {
                base.Update();
                if (_toolTip != null)
                {
                    if (ToolTipCount > 0)
                    {
                        if (_toolTipShown > 0 && MouseIn == true && _toolTip.ShowDuration > 0)
                        {
                            if (_toolTipShown < _toolTip.ShowDuration)
                            {
                                _toolTipShown += 1;
                            }
                            else
                            {
                                if (!_toolTip.ToolTipStatic && _toolTip != null)
                                {
                                    _toolTip.SetVisible(false);
                                }

                                _toolTipShown = 0;
                            }
                        }

                        if (_toolTip.ToolTipDelay > ToolTipCount)
                        {
                            ToolTipCount += 1;
                        }
                        else
                        {
                            if (_toolTipText.Count > 0 && !MouseIn)
                            {
                                _toolTip.SetFStrings(_toolTipText);
                                if (!_toolTip.ToolTipStatic)
                                {
                                    _toolTip.SetVisible(true);
                                    _window.Redraw = true;
                                    _toolTip.Sized = false;
                                }

                                W = _window;
                                while (W.OwnerComponent is BaseWindow)
                                {
                                    W = (BaseWindow)W.OwnerComponent;
                                }

                                if (_window != W)
                                {
                                    var rectangle = new Rect(_window.WinLeft + W.WinLeft + WinLeft, _window.WinTop + W.WinTop + WinTop, _window.WinLeft + W.WinLeft + ToolTipLeft, _window.WinTop + W.WinTop + WinTop + WinHeight);
                                    _toolTip.ControlRectangle = rectangle;
                                }
                                else
                                {
                                    var rectangle = new Rect(_window.WinLeft + WinLeft, _window.WinTop + WinTop, _window.WinLeft + ToolTipLeft, _window.WinTop + WinTop + WinHeight);
                                    _toolTip.ControlRectangle = rectangle;
                                }

                                MouseIn = true;
                                _toolTipShown = 1;
                            }
                        }
                    }
                    else
                    {
                        if (MouseIn == true)
                        {
                            if (!_toolTip.ToolTipStatic && _toolTip != null)
                            {
                                _toolTip.SetVisible(false);
                            }

                            MouseIn = false;
                        }
                    }
                }

                if (OldRectangle.Right > 0 && OldRectangle.Bottom > 0 && (OldRectangle.Left != WinLeft || OldRectangle.Top != WinTop || OldRectangle.Right != WinWidth || OldRectangle.Bottom != WinHeight))
                {
                    if (OwnerComponent.OwnerComponent.OwnerComponent == null)
                    {
                        R = new Rect(OldRectangle.Left, OldRectangle.Top, OldRectangle.Left + OldRectangle.Right - 1, OldRectangle.Top + OldRectangle.Bottom - 1);
                    }
                    else
                    {
                        R = new Rect(OwnerComponent.WinLeft + OldRectangle.Left, OwnerComponent.WinTop + OldRectangle.Top, OwnerComponent.WinLeft + OldRectangle.Left + OldRectangle.Right - 1, OwnerComponent.WinTop + OldRectangle.Top + OldRectangle.Bottom - 1);
                    }

                    OwnerComponent.AddDrawRectangle(ref R, this);
                    Drawn = false;
                }
                else
                {
                    if (Masked && !Drawn)
                    {
                        if (OwnerComponent.OwnerComponent.OwnerComponent == null)
                        {
                            R = new Rect(WinLeft, WinTop, WinLeft + WinWidth - 1, WinTop + WinHeight - 1);
                        }
                        else
                        {
                            R = new Rect(OwnerComponent.WinLeft + WinLeft, OwnerComponent.WinTop + WinTop, OwnerComponent.WinLeft + WinLeft + WinWidth - 1, OwnerComponent.WinTop + WinTop + WinHeight - 1);
                        }

                        OwnerComponent.AddDrawRectangle(ref R, this);
                    }
                }

                if (!Drawn)
                {
                    if (OwnerComponent.OwnerComponent.OwnerComponent == null)
                    {
                        R = new Rect(WinLeft, WinTop, WinLeft + WinWidth - 1, WinTop + WinHeight - 1);
                    }
                    else
                    {
                        R = new Rect(OwnerComponent.WinLeft + WinLeft, OwnerComponent.WinTop + WinTop, OwnerComponent.WinLeft + WinLeft + WinWidth - 1, OwnerComponent.WinTop + WinTop + WinHeight - 1);
                    }

                    var indexOf = OwnerComponent.Children.IndexOf(this);
                    OwnerComponent.InvalidateRectangle(indexOf, ref R, this);
                }

                OldRectangle.Left = WinLeft;
                OldRectangle.Top = WinTop;
                OldRectangle.Right = WinWidth;
                OldRectangle.Bottom = WinHeight;
            }
            else
            {
                if (Hidden)
                {
                    if (OwnerComponent.OwnerComponent.OwnerComponent == null)
                    {
                        R = new Rect(WinLeft, WinTop, WinLeft + WinWidth - 1, WinTop + WinHeight - 1);
                    }
                    else
                    {
                        R = new Rect(OwnerComponent.WinLeft + WinLeft, OwnerComponent.WinTop + WinTop, OwnerComponent.WinLeft + WinLeft + WinWidth - 1, OwnerComponent.WinTop + WinTop + WinHeight - 1);
                    }

                    OwnerComponent.AddDrawRectangle(ref R, this);
                    Hidden = false;
                }
            }
        }
    }

    // AoWControl.pas:64
    public override void SetOwner(UIComponent value)
    {
        base.SetOwner(value);
        _window = (BaseWindow)OwnerComponent;
    }

    // AoWControl.pas:190
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (_toolTip != null)
        {
            _toolTip.SetVisible(false);
            _toolTipShown = _toolTip.ShowDuration;
            MouseIn = true;
            ToolTipCount = _toolTip.ToolTipDelay;
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
            if (_toolTip != null && _toolTip.ToolTipDelay > ToolTipCount)
            {
                ToolTipLeft = X - _window.WinLeft;
                ToolTipCount = 1;
            }
        }
    }

    // AoWControl.pas:59
    public void SetWindow(BaseWindow value)
    {
        SetOwner(value);
    }

    // AoWControl.pas:70
    public void SetToolStrings(StringList value)
    {
        _toolTipText.Clear();
        for (int I = 0, last = value.Count - 1; I <= last; I++)
        {
            var valueValue = value[I];
            _toolTipText.Add(valueValue);
        }
    }

    [Published]
    public BaseWindow Window { get => _window; set => SetWindow(value); }

    [Published]
    public ToolTip ToolTip { get => _toolTip; set => _toolTip = value; }

    [Published]
    public StringList ToolTipText { get => _toolTipText; set => SetToolStrings(value); }

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
