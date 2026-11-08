// Translated from AoWCheckBox.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWComp;
using static Aow.Rtl.RtlUnit;

namespace Aow.Units.AoWCheckBox;

// { The AoWCheckBox unit of aowInt.dpl: TAOWCheckBox, a button that toggles its down state
//   (Checked) on a left click; with a RadioIndex above 0 it is a radio button, which only turns
//   on, and its window turns off the other radio buttons of the group (RadioChange). A right
//   click opens its PopupMenu at the mouse, kept inside the window manager. }
// Fields: FOnDoubleClick
public partial class TAOWCheckBox : TAOWBaseButton
{
    public TAOWCheckBox(TComponent owner) : base(owner)
    { }

    public TNotifyEvent<TAOWCheckBox> FOnDoubleClick;

    // AoWCheckBox.pas:51
    public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
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

            if (Button == TMouseButton.mbLeft)
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

                if ((Shift & TSet<TShiftState>.Of(TShiftState.ssDouble)) == TSet<TShiftState>.Of(TShiftState.ssDouble))
                {
                    if (FOnDoubleClick != null)
                    {
                        FOnDoubleClick(this);
                    }
                }
            }
            else
            {
                if (Button == TMouseButton.mbRight && FPopupMenu != null)
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
    public override void CheckMouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
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
    public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
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
    public TNotifyEvent<TAOWCheckBox> OnDoubleClick { get => FOnDoubleClick; set => FOnDoubleClick = value; }

    public new TNotifyEvent<TAOWCheckBox> OnChange
    {
        get => EventSlot.Get(this, FOnChange, static slot => (TNotifyEvent<TAOWCheckBox>)slot);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new TMouseEvent<TAOWCheckBox> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown, static slot => (TMouseEvent<TAOWCheckBox>)slot);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new TMouseEvent<TAOWCheckBox> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp, static slot => (TMouseEvent<TAOWCheckBox>)slot);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new TMouseMoveEvent<TAOWCheckBox> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove, static slot => (TMouseMoveEvent<TAOWCheckBox>)slot);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new TKeyPressEvent<TAOWCheckBox> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress, static slot => (TKeyPressEvent<TAOWCheckBox>)slot);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new TKeyEvent<TAOWCheckBox> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown, static slot => (TKeyEvent<TAOWCheckBox>)slot);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new TAoWOnDrawEvent<TAOWCheckBox> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw, static slot => (TAoWOnDrawEvent<TAOWCheckBox>)slot);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new TAoWOnDrawEvent<TAOWCheckBox> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw, static slot => (TAoWOnDrawEvent<TAOWCheckBox>)slot);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new TNotifyEvent<TAOWCheckBox> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate, static slot => (TNotifyEvent<TAOWCheckBox>)slot);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}

public static partial class AoWCheckBoxUnit
{
}
