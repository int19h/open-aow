// Translated from AoWButton.pas.
using Aow.Rtl;

namespace Aow.Game.Ui.Controls;

// Fields: FOnClick
public partial class AOWButton : AOWBaseButton
{
    public AOWButton(Component owner) : base(owner)
    { }

    public NotifyEvent<AOWButton> FOnClick;

    // AoWButton.pas:239
    public override void CheckMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        base.CheckMouseDown(Button, Shift, X, Y);
        FHandled = false;
        FLit = false;
        if (!FAOWWindow.FHandled)
        {
            if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
            {
                FHandled = true;
                if (Button == MouseButton.Left)
                {
                    FMouseDown = true;
                    FChecked = true;
                    FDrawn = false;
                    FRedraw = true;
                }

                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }
            }
        }
    }

    // AoWButton.pas:261
    public override void CheckMouseUp(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        FHandled = false;
        FChecked = false;
        FLit = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (Button == MouseButton.Left)
            {
                if (FMouseDown && FOnClick != null)
                {
                    FOnClick(this);
                }

                FDrawn = false;
            }

            if (FOnMouseUp != null)
            {
                FOnMouseUp(this, Button, Shift, X, Y);
            }
        }

        if (FMouseDown)
        {
            FDrawn = false;
        }

        FMouseDown = false;
    }

    // AoWButton.pas:284
    public override void CheckMouseMove(Set<ShiftState> Shift, int X, int Y)
    {
        bool OldChecked;
        bool OldLit;
        base.CheckMouseMove(Shift, X, Y);
        FHandled = false;
        OldChecked = FChecked;
        OldLit = FLit;
        FRedraw = false;
        if (FAOWWindow.FHandled || X < FAOWWindow.FWinLeft + FWinLeft || X > FAOWWindow.FWinLeft + FWinLeft + FWinWidth - 1 || Y < FAOWWindow.FWinTop + FWinTop || Y > FAOWWindow.FWinTop + FWinTop + FWinHeight - 1)
        {
            FChecked = false;
            FLit = false;
        }
        else
        {
            FHandled = true;
            FChecked = FMouseDown;
            FLit = !FMouseDown;
            FRedraw = true;
            if (FOnMouseMove != null)
            {
                FOnMouseMove(this, Shift, X, Y);
            }
        }

        if (OldLit != FLit || OldChecked != FChecked)
        {
            FDrawn = false;
        }
    }

    [Published]
    public NotifyEvent<AOWButton> OnClick { get => FOnClick; set => FOnClick = value; }

    public new NotifyEvent<AOWButton> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<AOWButton> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<AOWButton> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<AOWButton> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<AOWButton> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<AOWButton> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<AOWButton> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<AOWButton> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<AOWButton> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}
