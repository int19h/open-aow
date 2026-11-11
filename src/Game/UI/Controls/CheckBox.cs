// Translated from AoWCheckBox.pas.
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// { The AoWCheckBox unit of aowInt.dpl: TAOWCheckBox, a button that toggles its down state
//   (Checked) on a left click; with a RadioIndex above 0 it is a radio button, which only turns
//   on, and its window turns off the other radio buttons of the group (RadioChange). A right
//   click opens its PopupMenu at the mouse, kept inside the window manager. }
// Fields: FOnDoubleClick
public partial class CheckBox : BaseButton
{
    public CheckBox(Component owner) : base(owner)
    { }

    public NotifyEvent<CheckBox> FOnDoubleClick;

    // AoWCheckBox.pas:51
    public override void CheckMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        base.CheckMouseDown(Button, Shift, X, Y);
        FHandled = false;
        FLit = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight && FValid == true && FVisible == true)
        {
            FHandled = true;
            if (FOnMouseDown != null)
            {
                FOnMouseDown(this, Button, Shift, X, Y);
            }

            if (Button == MouseButton.Left)
            {
                if (FRadioIndex > 0)
                {
                    if (!FChecked)
                    {
                        FChecked = true;
                        if (FOnChange != null)
                        {
                            FOnChange(this);
                        }

                        FChecked = !FChecked;
                        Checked = !FChecked;
                    }
                }
                else
                {
                    FChecked = !FChecked;
                    if (FOnChange != null)
                    {
                        FOnChange(this);
                    }

                    FChecked = !FChecked;
                    Checked = !FChecked;
                }

                if ((Shift & Set<ShiftState>.Of(ShiftState.Double)) == Set<ShiftState>.Of(ShiftState.Double))
                {
                    if (FOnDoubleClick != null)
                    {
                        FOnDoubleClick(this);
                    }
                }
            }
            else
            {
                if (Button == MouseButton.Right && FPopupMenu != null)
                {
                    FPopupMenu.WinLeft = X;
                    FPopupMenu.WinTop = Y;
                    FPopupMenu.SetPopupOwner(this);
                    if (FPopupMenu.FWinManager != null)
                    {
                        if (FPopupMenu.FWinLeft + FPopupMenu.FWinWidth > FPopupMenu.FWinManager.FWinWidth)
                        {
                            FPopupMenu.FWinLeft = X - FPopupMenu.FWinWidth;
                        }

                        if (FPopupMenu.FWinTop + FPopupMenu.FWinHeight > FPopupMenu.FWinManager.FWinHeight)
                        {
                            FPopupMenu.FWinTop = Y - FPopupMenu.FWinHeight;
                        }
                    }

                    FPopupMenu.Visible = true;
                }
            }
        }
    }

    // AoWCheckBox.pas:105
    public override void CheckMouseUp(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        FHandled = false;
        FLit = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight && FValid == true && FVisible == true)
        {
            FHandled = true;
            if (FOnMouseUp != null)
            {
                FOnMouseUp(this, Button, Shift, X, Y);
            }
        }
    }

    // AoWCheckBox.pas:119
    public override void CheckMouseMove(Set<ShiftState> Shift, int X, int Y)
    {
        bool OldLit;
        base.CheckMouseMove(Shift, X, Y);
        FHandled = false;
        OldLit = FLit;
        if (X < FAOWWindow.FWinLeft + FWinLeft || X > FAOWWindow.FWinLeft + FWinLeft + FWinWidth - 1 || Y < FAOWWindow.FWinTop + FWinTop || Y > FAOWWindow.FWinTop + FWinTop + FWinHeight - 1)
        {
            FLit = false;
        }
        else
        {
            FHandled = true;
            FLit = !FChecked && !FAOWOwner.FHandled;
            if (FOnMouseMove != null)
            {
                FOnMouseMove(this, Shift, X, Y);
            }
        }

        FRedraw = FLit;
        if (OldLit != FLit)
        {
            FDrawn = false;
        }
    }

    // AoWCheckBox.pas:38
    public virtual void SetChecked(bool Value)
    {
        if (Value != FChecked)
        {
            FChecked = Value;
            FDrawn = false;
            if (FRadioIndex > 0)
            {
                if (FAOWWindow != null)
                {
                    if (FChecked)
                    {
                        FAOWWindow.RadioChange(this);
                    }
                }
            }
        }
    }

    [Published]
    public bool Checked { get => FChecked; set => SetChecked(value); }

    [Published]
    public int RadioIndex { get => FRadioIndex; set => FRadioIndex = value; }

    [Published]
    public NotifyEvent<CheckBox> OnDoubleClick { get => FOnDoubleClick; set => FOnDoubleClick = value; }

    public new NotifyEvent<CheckBox> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<CheckBox> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<CheckBox> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<CheckBox> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<CheckBox> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<CheckBox> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<CheckBox> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<CheckBox> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<CheckBox> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}
