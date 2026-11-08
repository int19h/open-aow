// Translated from AoWLenLine.pas.
using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AoWControl;
using Aow.Units.AOWInterfaceImgLib;
using Aow.Units.GFXE;
using static Aow.Rtl.RtlUnit;

namespace Aow.Units.AoWLenLine;

// vertical line image library
public partial class TAoWLenLine : TAOWControl
{
    public TAoWLenLine(TComponent owner) : base(owner)
    { }

    public TInterfaceIL FILLine;

    // AoWLenLine.pas:36
    public override void Validate()
    {
        if (FILLine != null)
        {
            FValid = true;
        }

        // { Original bug: Validate calls SetSize. }
        base.SetSize();
    }

    // AoWLenLine.pas:42
    public override void SetSize()
    {
        FWinWidth = FILLine[0].Width;
        base.SetSize();
    }

    // AoWLenLine.pas:56
    public override void Draw(TSurfaceDesc SurfDesc)
    {
        try
        {
            if (FValid == true && FVisible == true)
            {
                var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                FILLine.DrawILLv(0, rect, ref SurfDesc);
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

    // AoWLenLine.pas:70
    public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        base.CheckMouseDown(Button, Shift, X, Y);
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            if (FOnMouseDown != null)
            {
                FOnMouseDown(this, Button, Shift, X, Y);
            }
        }
    }

    // AoWLenLine.pas:79
    public override void CheckMouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            if (FOnMouseUp != null)
            {
                FOnMouseUp(this, Button, Shift, X, Y);
            }
        }
    }

    // AoWLenLine.pas:87
    public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            if (FOnMouseMove != null)
            {
                FOnMouseMove(this, Shift, X, Y);
            }
        }
    }

    // AoWLenLine.pas:95
    public override void CheckKeyPress(ref byte Key)
    {
        FHandled = false;
    }

    // AoWLenLine.pas:48
    public virtual void SetILLine(TInterfaceIL Value)
    {
        if (Value != null)
        {
            if (Value.Count == 3)
            {
                FILLine = Value;
            }
            else
            {
                MessageBox(0, "Lines must have 3 Images", "Error - " + Name, 0);
            }
        }
        else
        {
            FILLine = null;
        }
    }

    [Published]
    public new int WinHeight { get => FWinHeight; set => SetHeight(value); }

    [Published]
    public TInterfaceIL ILLine { get => FILLine; set => SetILLine(value); }

    public new TNotifyEvent<TAoWLenLine> OnChange
    {
        get => EventSlot.Get(this, "TAoWLenLine.FOnChange", FOnChange, static slot => slot);
        set => EventSlot.Set(this, "TAoWLenLine.FOnChange", ref FOnChange, value, handler => sender => handler(this));
    }

    public new TMouseEvent<TAoWLenLine> OnMouseDown
    {
        get => EventSlot.Get(this, "TAoWLenLine.FOnMouseDown", FOnMouseDown, static slot => slot);
        set => EventSlot.Set(this, "TAoWLenLine.FOnMouseDown", ref FOnMouseDown, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseEvent<TAoWLenLine> OnMouseUp
    {
        get => EventSlot.Get(this, "TAoWLenLine.FOnMouseUp", FOnMouseUp, static slot => slot);
        set => EventSlot.Set(this, "TAoWLenLine.FOnMouseUp", ref FOnMouseUp, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseMoveEvent<TAoWLenLine> OnMouseMove
    {
        get => EventSlot.Get(this, "TAoWLenLine.FOnMouseMove", FOnMouseMove, static slot => slot);
        set => EventSlot.Set(this, "TAoWLenLine.FOnMouseMove", ref FOnMouseMove, value, handler => (sender, shift, x, y) => handler(this, shift, x, y));
    }

    public new TKeyPressEvent<TAoWLenLine> OnKeyPress
    {
        get => EventSlot.Get(this, "TAoWLenLine.FOnKeyPress", FOnKeyPress, static slot => slot);
        set => EventSlot.Set(this, "TAoWLenLine.FOnKeyPress", ref FOnKeyPress, value, handler => (sender, ref key) => handler(this, ref key));
    }

    public new TKeyEvent<TAoWLenLine> OnKeyDown
    {
        get => EventSlot.Get(this, "TAoWLenLine.FOnKeyDown", FOnKeyDown, static slot => slot);
        set => EventSlot.Set(this, "TAoWLenLine.FOnKeyDown", ref FOnKeyDown, value, handler => (sender, ref key, shift) => handler(this, ref key, shift));
    }

    public new TAoWOnDrawEvent<TAoWLenLine> OnPreDraw
    {
        get => EventSlot.Get(this, "TAoWLenLine.FOnPreDraw", FOnPreDraw, static slot => slot);
        set => EventSlot.Set(this, "TAoWLenLine.FOnPreDraw", ref FOnPreDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TAoWOnDrawEvent<TAoWLenLine> OnDraw
    {
        get => EventSlot.Get(this, "TAoWLenLine.FOnDraw", FOnDraw, static slot => slot);
        set => EventSlot.Set(this, "TAoWLenLine.FOnDraw", ref FOnDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TNotifyEvent<TAoWLenLine> OnUpdate
    {
        get => EventSlot.Get(this, "TAoWLenLine.FOnUpdate", FOnUpdate, static slot => slot);
        set => EventSlot.Set(this, "TAoWLenLine.FOnUpdate", ref FOnUpdate, value, handler => sender => handler(this));
    }
}

public static partial class AoWLenLineUnit
{
    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
