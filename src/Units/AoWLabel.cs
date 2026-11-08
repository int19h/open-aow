// Translated from AoWLabel.pas.
using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AoWControl;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using static Aow.Rtl.RtlUnit;

namespace Aow.Units.AoWLabel;

// { The AoWLabel unit of aowInt.dpl: TAOWLabel, a line of text in an image library font; it
//   sizes itself to its text. }
// no fields of its own
public partial class TAOWLabel : TAOWControl
{
    public TAOWLabel(TComponent owner) : base(owner)
    { }

    // AoWLabel.pas:38
    public override void Validate()
    {
        if (FAOWFont != null)
        {
            FValid = true;
        }

        FMasked = true;
        base.Validate();
    }

    // AoWLabel.pas:46
    public override void SetSize()
    {
        FWinWidth = FAOWFont.GetTextWidth(FText);
        FWinHeight = FAOWFont.GetTextHeight(FText);
        base.SetSize();
    }

    // AoWLabel.pas:72
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

                FAOWFont.Color = FColor;
                FAOWFont.SetText(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FText, SurfDesc);
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

    // AoWLabel.pas:90
    public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        base.CheckMouseDown(Button, Shift, X, Y);
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = FCaptureInput;
            if (FOnMouseDown != null)
            {
                FOnMouseDown(this, Button, Shift, X, Y);
            }
        }
    }

    // AoWLabel.pas:103
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

    // AoWLabel.pas:115
    public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        base.CheckMouseMove(Shift, X, Y);
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = FCaptureInput;
            if (FOnMouseMove != null)
            {
                FOnMouseMove(this, Shift, X, Y);
            }
        }
    }

    // AoWLabel.pas:128
    public override void CheckKeyPress(ref byte Key)
    {
        FHandled = false;
    }

    // AoWLabel.pas:53
    public void SetGFont(TImageLibraryFont Value)
    {
        if (Value != FAOWFont)
        {
            FAOWFont = Value;
            FValid = false;
        }
    }

    // AoWLabel.pas:62
    public void SetGText(string Value)
    {
        if (FText != Value)
        {
            FText = Value;
            FSized = false;
            FAligned = false;
        }
    }

    [Published]
    public TImageLibraryFont AOWFont { get => FAOWFont; set => SetGFont(value); }

    [Published]
    public string Text { get => FText; set => SetGText(value); }

    public new TNotifyEvent<TAOWLabel> OnChange
    {
        get => EventSlot.Get(this, "TAOWLabel.FOnChange", FOnChange, static slot => slot);
        set => EventSlot.Set(this, "TAOWLabel.FOnChange", ref FOnChange, value, handler => sender => handler(this));
    }

    public new TMouseEvent<TAOWLabel> OnMouseDown
    {
        get => EventSlot.Get(this, "TAOWLabel.FOnMouseDown", FOnMouseDown, static slot => slot);
        set => EventSlot.Set(this, "TAOWLabel.FOnMouseDown", ref FOnMouseDown, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseEvent<TAOWLabel> OnMouseUp
    {
        get => EventSlot.Get(this, "TAOWLabel.FOnMouseUp", FOnMouseUp, static slot => slot);
        set => EventSlot.Set(this, "TAOWLabel.FOnMouseUp", ref FOnMouseUp, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseMoveEvent<TAOWLabel> OnMouseMove
    {
        get => EventSlot.Get(this, "TAOWLabel.FOnMouseMove", FOnMouseMove, static slot => slot);
        set => EventSlot.Set(this, "TAOWLabel.FOnMouseMove", ref FOnMouseMove, value, handler => (sender, shift, x, y) => handler(this, shift, x, y));
    }

    public new TKeyPressEvent<TAOWLabel> OnKeyPress
    {
        get => EventSlot.Get(this, "TAOWLabel.FOnKeyPress", FOnKeyPress, static slot => slot);
        set => EventSlot.Set(this, "TAOWLabel.FOnKeyPress", ref FOnKeyPress, value, handler => (sender, ref key) => handler(this, ref key));
    }

    public new TKeyEvent<TAOWLabel> OnKeyDown
    {
        get => EventSlot.Get(this, "TAOWLabel.FOnKeyDown", FOnKeyDown, static slot => slot);
        set => EventSlot.Set(this, "TAOWLabel.FOnKeyDown", ref FOnKeyDown, value, handler => (sender, ref key, shift) => handler(this, ref key, shift));
    }

    public new TAoWOnDrawEvent<TAOWLabel> OnPreDraw
    {
        get => EventSlot.Get(this, "TAOWLabel.FOnPreDraw", FOnPreDraw, static slot => slot);
        set => EventSlot.Set(this, "TAOWLabel.FOnPreDraw", ref FOnPreDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TAoWOnDrawEvent<TAOWLabel> OnDraw
    {
        get => EventSlot.Get(this, "TAOWLabel.FOnDraw", FOnDraw, static slot => slot);
        set => EventSlot.Set(this, "TAOWLabel.FOnDraw", ref FOnDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TNotifyEvent<TAOWLabel> OnUpdate
    {
        get => EventSlot.Get(this, "TAOWLabel.FOnUpdate", FOnUpdate, static slot => slot);
        set => EventSlot.Set(this, "TAOWLabel.FOnUpdate", ref FOnUpdate, value, handler => sender => handler(this));
    }
}

public static partial class AoWLabelUnit
{
}
