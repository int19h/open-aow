// Translated from AoWLenLine.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Ui.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Controls;

// vertical line image library
public partial class AoWLenLine : AOWControl
{
    public AoWLenLine(global::Aow.Rtl.Component owner) : base(owner)
    { }

    public InterfaceIL FILLine;

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
    public override void Draw(SurfaceDesc SurfDesc)
    {
        try
        {
            if (FValid == true && FVisible == true)
            {
                var rect = global::Aow.Rtl.RtlUnit.Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
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
    public override void CheckMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
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
    public override void CheckMouseUp(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
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
    public override void CheckMouseMove(Set<ShiftState> Shift, int X, int Y)
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
    public virtual void SetILLine(InterfaceIL Value)
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
    public InterfaceIL ILLine { get => FILLine; set => SetILLine(value); }

    public new NotifyEvent<AoWLenLine> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<AoWLenLine> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<AoWLenLine> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<AoWLenLine> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<AoWLenLine> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<AoWLenLine> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<AoWLenLine> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<AoWLenLine> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<AoWLenLine> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}
