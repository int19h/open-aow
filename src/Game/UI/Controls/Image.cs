// Translated from AoWImage.pas.
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// { The AoWImage unit of aowInt.dpl: TAOWImage, a control that shows one image of an image
//   library as a texture filling its rectangle, or plays the images AnimStart..AnimEnd as an
//   animation, one image every AnimFrameDelay updates. Its mouse events get coordinates relative
//   to the control. }
// Fields: FAnimate, FAnimFrameDelay, FFrameCount (updates left until the next image),
// FAnimStart, FCurrentImage, FAnimEnd, FImageLib
public partial class Image : Control
{
    public Image(Component owner) : base(owner)
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
    public override void CheckMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
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
    public override void CheckMouseUp(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
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
    public override void CheckMouseMove(Set<ShiftState> Shift, int X, int Y)
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

    public new NotifyEvent<Image> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<Image> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<Image> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<Image> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<Image> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<Image> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<Image> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<Image> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<Image> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}
