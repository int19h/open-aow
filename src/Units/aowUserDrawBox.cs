// Translated from aowUserDrawBox.pas.
using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AoWControl;
using Aow.Units.GFXE;
using static Aow.Rtl.RtlUnit;

namespace Aow.Units.aowUserDrawBox;

// { The aowUserDrawBox unit of aowInt.dpl: TaowUserDrawBox, an empty control that its owner draws
//   in OnPreDraw and OnDraw; its mouse events get coordinates relative to the control. }
// no fields of its own
public partial class TaowUserDrawBox : TAOWControl
{
    public TaowUserDrawBox(TComponent owner) : base(owner)
    { }

    // aowUserDrawBox.pas:34
    public override void Validate()
    {
        FValid = true;
        base.Validate();
    }

    // aowUserDrawBox.pas:40
    public override void Draw(TSurfaceDesc SurfDesc)
    {
        try
        {
            if (FValid && FVisible)
            {
                if (FOnPreDraw != null)
                {
                    FOnPreDraw(this, SurfDesc);
                }

                if (FOnDraw != null)
                {
                    FOnDraw(this, SurfDesc);
                }
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // aowUserDrawBox.pas:55
    public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        base.CheckMouseDown(Button, Shift, X, Y);
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = FCaptureInput;
            if (FOnMouseDown != null)
            {
                FOnMouseDown(this, Button, Shift, X - FAOWWindow.FWinLeft - FWinLeft, Y - FAOWWindow.FWinTop - FWinTop);
            }
        }
    }

    // aowUserDrawBox.pas:69
    public override void CheckMouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = FCaptureInput;
            if (FOnMouseUp != null)
            {
                FOnMouseUp(this, Button, Shift, X, Y);
            }
        }
    }

    // aowUserDrawBox.pas:81
    public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        base.CheckMouseMove(Shift, X, Y);
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = FCaptureInput;
            if (FOnMouseMove != null)
            {
                FOnMouseMove(this, Shift, X - FAOWWindow.FWinLeft - FWinLeft, Y - FAOWWindow.FWinTop - FWinTop);
            }
        }
    }

    // aowUserDrawBox.pas:94
    public override void CheckKeyPress(ref byte Key)
    {
        FHandled = false;
    }

    public new TNotifyEvent<TaowUserDrawBox> OnChange
    {
        get => EventSlot.Get(this, "TaowUserDrawBox.FOnChange", FOnChange, static slot => slot);
        set => EventSlot.Set(this, "TaowUserDrawBox.FOnChange", ref FOnChange, value, handler => sender => handler(this));
    }

    public new TMouseEvent<TaowUserDrawBox> OnMouseDown
    {
        get => EventSlot.Get(this, "TaowUserDrawBox.FOnMouseDown", FOnMouseDown, static slot => slot);
        set => EventSlot.Set(this, "TaowUserDrawBox.FOnMouseDown", ref FOnMouseDown, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseEvent<TaowUserDrawBox> OnMouseUp
    {
        get => EventSlot.Get(this, "TaowUserDrawBox.FOnMouseUp", FOnMouseUp, static slot => slot);
        set => EventSlot.Set(this, "TaowUserDrawBox.FOnMouseUp", ref FOnMouseUp, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseMoveEvent<TaowUserDrawBox> OnMouseMove
    {
        get => EventSlot.Get(this, "TaowUserDrawBox.FOnMouseMove", FOnMouseMove, static slot => slot);
        set => EventSlot.Set(this, "TaowUserDrawBox.FOnMouseMove", ref FOnMouseMove, value, handler => (sender, shift, x, y) => handler(this, shift, x, y));
    }

    public new TKeyPressEvent<TaowUserDrawBox> OnKeyPress
    {
        get => EventSlot.Get(this, "TaowUserDrawBox.FOnKeyPress", FOnKeyPress, static slot => slot);
        set => EventSlot.Set(this, "TaowUserDrawBox.FOnKeyPress", ref FOnKeyPress, value, handler => (sender, ref key) => handler(this, ref key));
    }

    public new TKeyEvent<TaowUserDrawBox> OnKeyDown
    {
        get => EventSlot.Get(this, "TaowUserDrawBox.FOnKeyDown", FOnKeyDown, static slot => slot);
        set => EventSlot.Set(this, "TaowUserDrawBox.FOnKeyDown", ref FOnKeyDown, value, handler => (sender, ref key, shift) => handler(this, ref key, shift));
    }

    public new TAoWOnDrawEvent<TaowUserDrawBox> OnPreDraw
    {
        get => EventSlot.Get(this, "TaowUserDrawBox.FOnPreDraw", FOnPreDraw, static slot => slot);
        set => EventSlot.Set(this, "TaowUserDrawBox.FOnPreDraw", ref FOnPreDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TAoWOnDrawEvent<TaowUserDrawBox> OnDraw
    {
        get => EventSlot.Get(this, "TaowUserDrawBox.FOnDraw", FOnDraw, static slot => slot);
        set => EventSlot.Set(this, "TaowUserDrawBox.FOnDraw", ref FOnDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TNotifyEvent<TaowUserDrawBox> OnUpdate
    {
        get => EventSlot.Get(this, "TaowUserDrawBox.FOnUpdate", FOnUpdate, static slot => slot);
        set => EventSlot.Set(this, "TaowUserDrawBox.FOnUpdate", ref FOnUpdate, value, handler => sender => handler(this));
    }
}

public static partial class aowUserDrawBoxUnit
{
}
