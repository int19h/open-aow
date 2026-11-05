// Translated from AoWLenLine.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.AoWControl;
using Aow.Units.AOWInterfaceImgLib;

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
}

public static partial class AoWLenLine
{
    // AoWLenLine.pas:100
    public static void Register()
    {
        RegisterComponents("AOW Tools", [typeof(TAoWLenLine)]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
