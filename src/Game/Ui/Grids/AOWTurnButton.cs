// Translated from TurnButton.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Ui.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Grids;

// { ot=1 }
// library, current player, last player, animating, animation phase, frame offset, done event,
// change event, checked, animation frame
public partial class AOWTurnButton : AOWControl
{
    public ImageLibrary FImageLib;
    public int FCurPlayer;
    public int FLastPlayer;
    public bool FAnimating;
    public int FAnimationPhase;
    public int FFrameOffset;
    public NotifyEvent<AOWTurnButton> FOnAnimDone;
    public NotifyEvent<AOWTurnButton> FOnPlayerChange;
    public bool FButtonChecked;
    public int FAnimationFrame;

    // TurnButton.pas:54
    public AOWTurnButton(Component owner) : base(owner)
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
    public override void Draw(SurfaceDesc SurfDesc)
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
    public override void CheckMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        base.CheckMouseDown(Button, Shift, X, Y);
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (Button == MouseButton.Left && Press())
            {
                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }
            }
        }
    }

    // TurnButton.pas:169
    public override void CheckMouseUp(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
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
    public override void CheckMouseMove(Set<ShiftState> Shift, int X, int Y)
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
    public virtual void SetILB(ImageLibrary Value)
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
    public ImageLibrary ImageLib { get => FImageLib; set => SetILB(value); }

    [Published]
    public int CurPlayer { get => FCurPlayer; set => FCurPlayer = value; }

    [Published]
    public int LastPlayer { get => FLastPlayer; set => FLastPlayer = value; }

    [Published]
    public bool Animating { get => FAnimating; set => FAnimating = value; }

    [Published]
    public NotifyEvent<AOWTurnButton> OnAnimDone { get => FOnAnimDone; set => FOnAnimDone = value; }

    [Published]
    public new NotifyEvent<AOWTurnButton> OnChange { get => FOnPlayerChange; set => FOnPlayerChange = value; }

    [Published]
    public bool Checked { get => FButtonChecked; set => FButtonChecked = value; }

    public new MouseEvent<AOWTurnButton> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<AOWTurnButton> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<AOWTurnButton> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<AOWTurnButton> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<AOWTurnButton> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<AOWTurnButton> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<AOWTurnButton> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<AOWTurnButton> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }

    public new enum ButtonType : byte
    {
        Text = 0, Icon = 1
    }
}
