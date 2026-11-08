// Translated from TurnButton.pas.
using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AoWControl;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using static Aow.Rtl.RtlUnit;

namespace Aow.Units.TurnButton;

// { ot=1 }
// library, current player, last player, animating, animation phase, frame offset, done event,
// change event, checked, animation frame
public partial class TAOWTurnButton : TAOWControl
{
    public TImageLibrary FImageLib;
    public int FCurPlayer;
    public int FLastPlayer;
    public bool FAnimating;
    public int FAnimationPhase;
    public int FFrameOffset;
    public TNotifyEvent<TAOWTurnButton> FOnAnimDone;
    public TNotifyEvent<TAOWTurnButton> FOnPlayerChange;
    public bool FButtonChecked;
    public int FAnimationFrame;

    // TurnButton.pas:54
    public TAOWTurnButton(TComponent owner) : base(owner)
    {
        FFrameOffset = 11;
    }

    // TurnButton.pas:60
    public override void Validate()
    {
        if (FImageLib != null)
        {
            FValid = true;
        }

        base.Validate();
    }

    // TurnButton.pas:66
    public override void SetSize()
    {
        FWinWidth = FImageLib[0].Width;
        FWinHeight = FImageLib[0].Height;
        base.SetSize();
    }

    // TurnButton.pas:79
    public override void Update()
    {
        base.Update();
        if (FVisible)
        {
            switch (FAnimationPhase)
            {
                case 0:
                    if (FLastPlayer != FCurPlayer)
                    {
                        FAnimationFrame = FLastPlayer * 7;
                        FFrameOffset = 11;
                        FAnimationPhase = 1;
                        FAnimating = true;
                        FDrawn = false;
                    }

                    break;
                case 1:
                    FAnimationFrame += 1;
                    FDrawn = false;
                    if (FLastPlayer * 7 + 5 < FAnimationFrame)
                    {
                        FAnimationFrame = 0;
                        FAnimationPhase = 2;
                        FFrameOffset = 0;
                    }

                    break;
                case 2:
                    FAnimationFrame += 1;
                    FDrawn = false;
                    if (FAnimationFrame > 10)
                    {
                        FAnimationPhase = 3;
                        FAnimationFrame = FCurPlayer * 7 + 5;
                        FFrameOffset = 11;
                    }

                    break;
                case 3:
                    FAnimationFrame -= 1;
                    FDrawn = false;
                    if (FCurPlayer * 7 > FAnimationFrame)
                    {
                        FAnimationFrame = FCurPlayer * 7;
                        FAnimationPhase = 0;
                        FFrameOffset = 11;
                        FLastPlayer = FCurPlayer;
                        FAnimating = false;
                        if (FOnAnimDone != null)
                        {
                            FOnAnimDone(this);
                        }
                    }

                    break;
            }
        }
    }

    // TurnButton.pas:132
    public override void Draw(TSurfaceDesc SurfDesc)
    {
        try
        {
            if (FVisible == true && FValid == true)
            {
                if (FOnPreDraw != null)
                {
                    FOnPreDraw(this, SurfDesc);
                }

                if (!FAnimating && FButtonChecked == true)
                {
                    FImageLib[FFrameOffset + FCurPlayer * 7 + 6].ShowClipped(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, SurfDesc);
                }
                else
                {
                    FImageLib[FFrameOffset + FAnimationFrame].ShowClipped(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, SurfDesc);
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

    // TurnButton.pas:151
    public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        base.CheckMouseDown(Button, Shift, X, Y);
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (Button == TMouseButton.mbLeft && !FAnimating)
            {
                if (FButtonChecked)
                {
                    FButtonChecked = false;
                }
                else
                {
                    FButtonChecked = true;
                }

                FDrawn = false;
                if (FOnPlayerChange != null)
                {
                    FOnPlayerChange(this);
                }

                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }
            }
        }
    }

    // TurnButton.pas:169
    public override void CheckMouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (FOnMouseUp != null)
            {
                FOnMouseUp(this, Button, Shift, X, Y);
            }
        }
    }

    // TurnButton.pas:180
    public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        base.CheckMouseMove(Shift, X, Y);
        FHandled = false;
        if (X >= FAOWWindow.FWinLeft + FWinLeft && X <= FAOWWindow.FWinLeft + FWinLeft + FWinWidth - 1 && Y >= FAOWWindow.FWinTop + FWinTop && Y <= FAOWWindow.FWinTop + FWinTop + FWinHeight - 1)
        {
            FHandled = true;
            if (FOnMouseMove != null)
            {
                FOnMouseMove(this, Shift, X, Y);
            }
        }
    }

    // TurnButton.pas:73
    public virtual void SetILB(TImageLibrary Value)
    {
        if (Value != null)
        {
            FImageLib = Value;
        }
        else
        {
            FImageLib = null;
        }
    }

    [Published]
    public TImageLibrary ImageLib { get => FImageLib; set => SetILB(value); }

    [Published]
    public int CurPlayer { get => FCurPlayer; set => FCurPlayer = value; }

    [Published]
    public int LastPlayer { get => FLastPlayer; set => FLastPlayer = value; }

    [Published]
    public bool Animating { get => FAnimating; set => FAnimating = value; }

    [Published]
    public TNotifyEvent<TAOWTurnButton> OnAnimDone { get => FOnAnimDone; set => FOnAnimDone = value; }

    [Published]
    public new TNotifyEvent<TAOWTurnButton> OnChange { get => FOnPlayerChange; set => FOnPlayerChange = value; }

    [Published]
    public bool Checked { get => FButtonChecked; set => FButtonChecked = value; }

    public new TMouseEvent<TAOWTurnButton> OnMouseDown
    {
        get => EventSlot.Get(this, "TAOWTurnButton.FOnMouseDown", FOnMouseDown, static slot => slot);
        set => EventSlot.Set(this, "TAOWTurnButton.FOnMouseDown", ref FOnMouseDown, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseEvent<TAOWTurnButton> OnMouseUp
    {
        get => EventSlot.Get(this, "TAOWTurnButton.FOnMouseUp", FOnMouseUp, static slot => slot);
        set => EventSlot.Set(this, "TAOWTurnButton.FOnMouseUp", ref FOnMouseUp, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseMoveEvent<TAOWTurnButton> OnMouseMove
    {
        get => EventSlot.Get(this, "TAOWTurnButton.FOnMouseMove", FOnMouseMove, static slot => slot);
        set => EventSlot.Set(this, "TAOWTurnButton.FOnMouseMove", ref FOnMouseMove, value, handler => (sender, shift, x, y) => handler(this, shift, x, y));
    }

    public new TKeyPressEvent<TAOWTurnButton> OnKeyPress
    {
        get => EventSlot.Get(this, "TAOWTurnButton.FOnKeyPress", FOnKeyPress, static slot => slot);
        set => EventSlot.Set(this, "TAOWTurnButton.FOnKeyPress", ref FOnKeyPress, value, handler => (sender, ref key) => handler(this, ref key));
    }

    public new TKeyEvent<TAOWTurnButton> OnKeyDown
    {
        get => EventSlot.Get(this, "TAOWTurnButton.FOnKeyDown", FOnKeyDown, static slot => slot);
        set => EventSlot.Set(this, "TAOWTurnButton.FOnKeyDown", ref FOnKeyDown, value, handler => (sender, ref key, shift) => handler(this, ref key, shift));
    }

    public new TAoWOnDrawEvent<TAOWTurnButton> OnPreDraw
    {
        get => EventSlot.Get(this, "TAOWTurnButton.FOnPreDraw", FOnPreDraw, static slot => slot);
        set => EventSlot.Set(this, "TAOWTurnButton.FOnPreDraw", ref FOnPreDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TAoWOnDrawEvent<TAOWTurnButton> OnDraw
    {
        get => EventSlot.Get(this, "TAOWTurnButton.FOnDraw", FOnDraw, static slot => slot);
        set => EventSlot.Set(this, "TAOWTurnButton.FOnDraw", ref FOnDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TNotifyEvent<TAOWTurnButton> OnUpdate
    {
        get => EventSlot.Get(this, "TAOWTurnButton.FOnUpdate", FOnUpdate, static slot => slot);
        set => EventSlot.Set(this, "TAOWTurnButton.FOnUpdate", ref FOnUpdate, value, handler => sender => handler(this));
    }
}

public enum TButtonType : byte
{
    btText = 0,
    btIcon = 1
}

public static partial class TurnButtonUnit
{
    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
