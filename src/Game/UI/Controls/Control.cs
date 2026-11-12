using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// { The AoWControl unit of aowInt.dpl: TAOWControl, the ancestor of the controls that sit in a
//   window (buttons, edits, lists, scroll bars, ...). It knows its window and shows its tool tip. }
// Fields: FToolTipText, FToolTipShown (updates the tool tip has been visible), FAOWWindow,
// _toolTip
public partial class Control : UIComponent
{
    public StringList _toolTipText;
    public int ToolTipShown;
    public BaseWindow _window;
    public ToolTip _toolTip;

    // AoWControl.pas:46
    public Control(Component owner) : base(owner)
    {
        _toolTipText = new StringList();
        _captureInput = true;
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
            if (_visible)
            {
                base.Update();
                if (_toolTip != null)
                {
                    if (ToolTipCount > 0)
                    {
                        if (ToolTipShown > 0 && MouseIn == true && _toolTip._showDuration > 0)
                        {
                            if (ToolTipShown < _toolTip._showDuration)
                            {
                                ToolTipShown += 1;
                            }
                            else
                            {
                                if (!_toolTip._toolTipStatic && _toolTip != null)
                                {
                                    _toolTip.SetVisible(false);
                                }

                                ToolTipShown = 0;
                            }
                        }

                        if (_toolTip._toolTipDelay > ToolTipCount)
                        {
                            ToolTipCount += 1;
                        }
                        else
                        {
                            if (_toolTipText.Count > 0 && !MouseIn)
                            {
                                _toolTip.SetFStrings(_toolTipText);
                                if (!_toolTip._toolTipStatic)
                                {
                                    _toolTip.SetVisible(true);
                                    _window.Redraw = true;
                                    _toolTip.Sized = false;
                                }

                                W = _window;
                                while (W._ownerComponent is BaseWindow)
                                {
                                    W = (BaseWindow)W._ownerComponent;
                                }

                                if (_window != W)
                                {
                                    var rectangle = new Rect(_window._winLeft + W._winLeft + _winLeft, _window._winTop + W._winTop + _winTop, _window._winLeft + W._winLeft + ToolTipLeft, _window._winTop + W._winTop + _winTop + _winHeight);
                                    _toolTip.ControlRectangle = rectangle;
                                }
                                else
                                {
                                    var rectangle = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop, _window._winLeft + ToolTipLeft, _window._winTop + _winTop + _winHeight);
                                    _toolTip.ControlRectangle = rectangle;
                                }

                                MouseIn = true;
                                ToolTipShown = 1;
                            }
                        }
                    }
                    else
                    {
                        if (MouseIn == true)
                        {
                            if (!_toolTip._toolTipStatic && _toolTip != null)
                            {
                                _toolTip.SetVisible(false);
                            }

                            MouseIn = false;
                        }
                    }
                }

                if (OldRectangle.Right > 0 && OldRectangle.Bottom > 0 && (OldRectangle.Left != _winLeft || OldRectangle.Top != _winTop || OldRectangle.Right != _winWidth || OldRectangle.Bottom != _winHeight))
                {
                    if (_ownerComponent._ownerComponent._ownerComponent == null)
                    {
                        R = new Rect(OldRectangle.Left, OldRectangle.Top, OldRectangle.Left + OldRectangle.Right - 1, OldRectangle.Top + OldRectangle.Bottom - 1);
                    }
                    else
                    {
                        R = new Rect(_ownerComponent._winLeft + OldRectangle.Left, _ownerComponent._winTop + OldRectangle.Top, _ownerComponent._winLeft + OldRectangle.Left + OldRectangle.Right - 1, _ownerComponent._winTop + OldRectangle.Top + OldRectangle.Bottom - 1);
                    }

                    _ownerComponent.AddDrawRectangle(ref R, this);
                    Drawn = false;
                }
                else
                {
                    if (_masked && !Drawn)
                    {
                        if (_ownerComponent._ownerComponent._ownerComponent == null)
                        {
                            R = new Rect(_winLeft, _winTop, _winLeft + _winWidth - 1, _winTop + _winHeight - 1);
                        }
                        else
                        {
                            R = new Rect(_ownerComponent._winLeft + _winLeft, _ownerComponent._winTop + _winTop, _ownerComponent._winLeft + _winLeft + _winWidth - 1, _ownerComponent._winTop + _winTop + _winHeight - 1);
                        }

                        _ownerComponent.AddDrawRectangle(ref R, this);
                    }
                }

                if (!Drawn)
                {
                    if (_ownerComponent._ownerComponent._ownerComponent == null)
                    {
                        R = new Rect(_winLeft, _winTop, _winLeft + _winWidth - 1, _winTop + _winHeight - 1);
                    }
                    else
                    {
                        R = new Rect(_ownerComponent._winLeft + _winLeft, _ownerComponent._winTop + _winTop, _ownerComponent._winLeft + _winLeft + _winWidth - 1, _ownerComponent._winTop + _winTop + _winHeight - 1);
                    }

                    var indexOf = _ownerComponent.Children.IndexOf(this);
                    _ownerComponent.InvalidateRectangle(indexOf, ref R, this);
                }

                OldRectangle.Left = _winLeft;
                OldRectangle.Top = _winTop;
                OldRectangle.Right = _winWidth;
                OldRectangle.Bottom = _winHeight;
            }
            else
            {
                if (Hidden)
                {
                    if (_ownerComponent._ownerComponent._ownerComponent == null)
                    {
                        R = new Rect(_winLeft, _winTop, _winLeft + _winWidth - 1, _winTop + _winHeight - 1);
                    }
                    else
                    {
                        R = new Rect(_ownerComponent._winLeft + _winLeft, _ownerComponent._winTop + _winTop, _ownerComponent._winLeft + _winLeft + _winWidth - 1, _ownerComponent._winTop + _winTop + _winHeight - 1);
                    }

                    _ownerComponent.AddDrawRectangle(ref R, this);
                    Hidden = false;
                }
            }
        }
    }

    // AoWControl.pas:64
    public override void SetOwner(UIComponent value)
    {
        base.SetOwner(value);
        _window = (BaseWindow)_ownerComponent;
    }

    // AoWControl.pas:190
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (_toolTip != null)
        {
            _toolTip.SetVisible(false);
            ToolTipShown = _toolTip._showDuration;
            MouseIn = true;
            ToolTipCount = _toolTip._toolTipDelay;
        }
    }

    // AoWControl.pas:201
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        if (X < _window._winLeft + _winLeft || X > _window._winLeft + _winLeft + _winWidth - 1 || Y < _window._winTop + _winTop || Y > _window._winTop + _winTop + _winHeight - 1)
        {
            ToolTipCount = 0;
        }
        else
        {
            if (_toolTip != null && _toolTip._toolTipDelay > ToolTipCount)
            {
                ToolTipLeft = X - _window._winLeft;
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

    public new NotifyEvent<Control> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<Control> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<Control> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<Control> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<Control> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<Control> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<Control> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<Control> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<Control> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }
}
