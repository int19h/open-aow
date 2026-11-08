// Translated from AoWImage.pas.
using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AoWControl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Units.AoWImage;

// { The AoWImage unit of aowInt.dpl: TAOWImage, a control that shows one image of an image
//   library as a texture filling its rectangle, or plays the images AnimStart..AnimEnd as an
//   animation, one image every AnimFrameDelay updates. Its mouse events get coordinates relative
//   to the control. }
// Fields: FAnimate, FAnimFrameDelay, FFrameCount (updates left until the next image),
// FAnimStart, FCurrentImage, FAnimEnd, FImageLib
public partial class TAOWImage : TAOWControl
{
    public TAOWImage(TComponent owner) : base(owner)
    { }

    public bool FAnimate;
    public int FAnimFrameDelay;
    public int FFrameCount;
    public int FAnimStart;
    public int FCurrentImage;
    public int FAnimEnd;

    // AoWImage.pas:79
    public override void SetSize()
    {
        if (FWinWidth == 0)
        {
            FWinWidth = FImageLib[FCurrentImage].Width;
        }

        if (FWinHeight == 0)
        {
            FWinHeight = FImageLib[FCurrentImage].Height;
        }

        base.SetSize();
    }

    // AoWImage.pas:145
    public override void Update()
    {
        int I;
        if (FVisible && FAnimate == true)
        {
            if (FFrameCount > 0)
            {
                FFrameCount -= 1;
            }
            else
            {
                FFrameCount = FAnimFrameDelay;
                I = FCurrentImage + 1;
                if (I > FAnimEnd)
                {
                    I = FAnimStart;
                }

                SetCurrent(I);
            }
        }

        base.Update();
    }

    // AoWImage.pas:163
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

    // AoWImage.pas:177
    public override void CheckMouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        FHandled = false;
        FRedraw = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = FCaptureInput;
            if (FOnMouseUp != null)
            {
                FOnMouseUp(this, Button, Shift, X, Y);
            }
        }
    }

    // AoWImage.pas:190
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

    // AoWImage.pas:203
    public override void CheckKeyPress(ref byte Key)
    {
        FHandled = false;
    }

    // AoWImage.pas:88
    public void SetAnim(bool Value)
    {
        if (Value != FAnimate)
        {
            FAnimate = Value;
            FValid = false;
        }
    }

    // AoWImage.pas:97
    public void SetStart(int Value)
    {
        if (Value < 0)
        {
            Value = 0;
        }

        if (Value != FAnimStart)
        {
            FAnimStart = Value;
            FValid = false;
        }
    }

    // AoWImage.pas:108
    public void SetCurrent(int Value)
    {
        if (Value < FAnimStart)
        {
            Value = FAnimStart;
        }

        if (Value != FCurrentImage)
        {
            FCurrentImage = Value;
            FDrawn = false;
        }
    }

    // AoWImage.pas:119
    public void SetEnd(int Value)
    {
        if (Value != FAnimEnd)
        {
            FAnimEnd = Value;
            FValid = false;
        }
    }

    [Published]
    public int AnimStart { get => FAnimStart; set => SetStart(value); }

    [Published]
    public int CurrentImage { get => FCurrentImage; set => SetCurrent(value); }

    [Published]
    public int AnimEnd { get => FAnimEnd; set => SetEnd(value); }

    [Published]
    public bool Animate { get => FAnimate; set => SetAnim(value); }

    [Published]
    public int AnimFrameDelay { get => FAnimFrameDelay; set => FAnimFrameDelay = value; }

    public new TNotifyEvent<TAOWImage> OnChange
    {
        get => EventSlot.Get(this, "TAOWImage.FOnChange", FOnChange, static slot => slot);
        set => EventSlot.Set(this, "TAOWImage.FOnChange", ref FOnChange, value, handler => sender => handler(this));
    }

    public new TMouseEvent<TAOWImage> OnMouseDown
    {
        get => EventSlot.Get(this, "TAOWImage.FOnMouseDown", FOnMouseDown, static slot => slot);
        set => EventSlot.Set(this, "TAOWImage.FOnMouseDown", ref FOnMouseDown, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseEvent<TAOWImage> OnMouseUp
    {
        get => EventSlot.Get(this, "TAOWImage.FOnMouseUp", FOnMouseUp, static slot => slot);
        set => EventSlot.Set(this, "TAOWImage.FOnMouseUp", ref FOnMouseUp, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseMoveEvent<TAOWImage> OnMouseMove
    {
        get => EventSlot.Get(this, "TAOWImage.FOnMouseMove", FOnMouseMove, static slot => slot);
        set => EventSlot.Set(this, "TAOWImage.FOnMouseMove", ref FOnMouseMove, value, handler => (sender, shift, x, y) => handler(this, shift, x, y));
    }

    public new TKeyPressEvent<TAOWImage> OnKeyPress
    {
        get => EventSlot.Get(this, "TAOWImage.FOnKeyPress", FOnKeyPress, static slot => slot);
        set => EventSlot.Set(this, "TAOWImage.FOnKeyPress", ref FOnKeyPress, value, handler => (sender, ref key) => handler(this, ref key));
    }

    public new TKeyEvent<TAOWImage> OnKeyDown
    {
        get => EventSlot.Get(this, "TAOWImage.FOnKeyDown", FOnKeyDown, static slot => slot);
        set => EventSlot.Set(this, "TAOWImage.FOnKeyDown", ref FOnKeyDown, value, handler => (sender, ref key, shift) => handler(this, ref key, shift));
    }

    public new TAoWOnDrawEvent<TAOWImage> OnPreDraw
    {
        get => EventSlot.Get(this, "TAOWImage.FOnPreDraw", FOnPreDraw, static slot => slot);
        set => EventSlot.Set(this, "TAOWImage.FOnPreDraw", ref FOnPreDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TAoWOnDrawEvent<TAOWImage> OnDraw
    {
        get => EventSlot.Get(this, "TAOWImage.FOnDraw", FOnDraw, static slot => slot);
        set => EventSlot.Set(this, "TAOWImage.FOnDraw", ref FOnDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TNotifyEvent<TAOWImage> OnUpdate
    {
        get => EventSlot.Get(this, "TAOWImage.FOnUpdate", FOnUpdate, static slot => slot);
        set => EventSlot.Set(this, "TAOWImage.FOnUpdate", ref FOnUpdate, value, handler => sender => handler(this));
    }
}

public static partial class AoWImageUnit
{
}
