// Translated from AOWGauge.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Ui.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Controls;

public enum TAOWGaugeType : byte
{
    gtNormal = 0,
    gtBorderless = 1
}

// Fields: FMin, FMax, FPosition, FILFrame, FILTextureNormal, FILTextureSelected, FStyle,
// FILIndexFrame, FILIndexTNormal, FILIndexTSelected
public partial class TAoWGauge : TAOWControl
{
    public int FMin;
    public int FMax;
    public int FPosition;
    public TInterfaceIL FILFrame;
    public TInterfaceIL FILTextureNormal;
    public TInterfaceIL FILTextureSelected;
    public TAOWGaugeType FStyle;
    public int FILIndexFrame;
    public int FILIndexTNormal;
    public int FILIndexTSelected;

    // AOWGauge.pas:69
    public TAoWGauge(TComponent owner) : base(owner)
    {
        FMax = 1;
    }

    // AOWGauge.pas:75
    public override void Validate()
    {
        if (FILTextureNormal != null)
        {
            FILTextureNormal = FILTextureNormal.CheckIL(FILIndexTNormal, 1, this);
        }

        if (FILTextureSelected != null)
        {
            FILTextureSelected = FILTextureSelected.CheckIL(FILIndexTSelected, 1, this);
        }

        if (FILTextureNormal != null && FILTextureSelected != null)
        {
            if (FILFrame != null)
            {
                FILFrame = FILFrame.CheckIL(FILIndexFrame, 8, this);
            }

            if (FILFrame != null)
            {
                FStyle = TAOWGaugeType.gtNormal;
            }
            else
            {
                FStyle = TAOWGaugeType.gtBorderless;
            }

            FValid = true;
        }

        base.Validate();
    }

    // AOWGauge.pas:94
    public override void SetSize()
    {
        switch (FStyle)
        {
            case TAOWGaugeType.gtNormal:
                var ilFrameHeight = FILFrame[FILIndexFrame];
                var ilFrame = FILFrame[FILIndexFrame + 5];
                if (ilFrameHeight.Height + ilFrame.Height > FWinHeight)
                {
                    var ilFrame2 = FILFrame[FILIndexFrame];
                    var ilFrame3 = FILFrame[FILIndexFrame + 5];
                    FWinHeight = ilFrame2.Height + ilFrame3.Height;
                }

                var ilFrameWidth = FILFrame[FILIndexFrame];
                var ilFrame4 = FILFrame[FILIndexFrame + 2];
                if (ilFrameWidth.Width + ilFrame4.Width > FWinWidth)
                {
                    var ilFrame2 = FILFrame[FILIndexFrame];
                    var ilFrame3 = FILFrame[FILIndexFrame + 2];
                    FWinWidth = ilFrame2.Width + ilFrame3.Width;
                }

                break;
        }

        if (FMin + 1 > FMax)
        {
            FMax = FMin + 1;
        }

        if (FMin > FPosition)
        {
            FPosition = FMin;
        }

        if (FMax < FPosition)
        {
            FPosition = FMax;
        }

        base.SetSize();
    }

    // AOWGauge.pas:183
    // { Draw: the filled width P is computed from Position without subtracting Min (original bug:
    //   the gauge is only right for Min = 0) }
    public override void Draw(TSurfaceDesc SurfDesc)
    {
        TRect OldClip = default;
        int P = default;
        try
        {
            if (FValid && FVisible)
            {
                if (FOnPreDraw != null)
                {
                    FOnPreDraw(this, SurfDesc);
                }

                OldClip = SurfDesc.ClipRect;
                switch (FStyle)
                {
                    case TAOWGaugeType.gtNormal:
                        var ilFrameWidth = FILFrame[FILIndexFrame + 3];
                        var ilFrame = FILFrame[FILIndexFrame + 4];
                        P = (FWinWidth - ilFrameWidth.Width - ilFrame.Width) * FPosition / (FMax - FMin);
                        var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                        FILFrame.DrawILF(FILIndexFrame, rect, ref SurfDesc);
                        var ilFrame2 = FILFrame[FILIndexFrame + 3];
                        if (FAOWWindow.FWinLeft + P + FWinLeft + ilFrame2.Width > SurfDesc.ClipRect.Left)
                        {
                            var ilFrame3 = FILFrame[FILIndexFrame + 3];
                            SurfDesc.ClipRect.Left = FAOWWindow.FWinLeft + P + FWinLeft + ilFrame3.Width;
                        }

                        var ilFrameHeight = FILFrame[FILIndexFrame + 1];
                        var ilFrame4 = FILFrame[FILIndexFrame + 6];
                        var ilFrame5 = FILFrame[FILIndexFrame + 3];
                        var ilFrame6 = FILFrame[FILIndexFrame + 4];
                        var ilFrame7 = FILFrame[FILIndexFrame + 1];
                        var ilFrame8 = FILFrame[FILIndexFrame + 3];
                        var rect2 = Rect(FAOWWindow.FWinLeft + FWinLeft + ilFrame8.Width, FAOWWindow.FWinTop + FWinTop + ilFrame7.Height, FWinWidth - ilFrame5.Width - ilFrame6.Width, FWinHeight - ilFrameHeight.Height - ilFrame4.Height);
                        FILTextureNormal.DrawILT(FILIndexTNormal, rect2, ref SurfDesc);
                        SurfDesc.ClipRect = OldClip;
                        var ilFrame9 = FILFrame[FILIndexFrame + 1];
                        var libraryImage = FILFrame[FILIndexFrame + 6];
                        var filFrameItem = FILFrame[FILIndexFrame + 1];
                        var ilFrameWidth2 = FILFrame[FILIndexFrame + 3];
                        var rect3 = Rect(FAOWWindow.FWinLeft + FWinLeft + ilFrameWidth2.Width, FAOWWindow.FWinTop + FWinTop + filFrameItem.Height, P, FWinHeight - ilFrame9.Height - libraryImage.Height);
                        FILTextureSelected.DrawILT(FILIndexTSelected, rect3, ref SurfDesc);
                        break;
                    case TAOWGaugeType.gtBorderless:
                        P = FPosition * FWinWidth / (FMax - FMin);
                        if (FAOWWindow.FWinLeft + P + FWinLeft > SurfDesc.ClipRect.Left)
                        {
                            SurfDesc.ClipRect.Left = FAOWWindow.FWinLeft + P + FWinLeft;
                        }

                        var rect4 = Rect(FAOWWindow.FWinLeft + P + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth - P, FWinHeight);
                        FILTextureNormal.DrawILT(FILIndexTNormal, rect4, ref SurfDesc);
                        SurfDesc.ClipRect = OldClip;
                        var rect5 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, P, FWinHeight);
                        FILTextureSelected.DrawILT(FILIndexTSelected, rect5, ref SurfDesc);
                        break;
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

    // AOWGauge.pas:237
    public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        base.CheckMouseDown(Button, Shift, X, Y);
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (FOnMouseDown != null)
            {
                FOnMouseDown(this, Button, Shift, X, Y);
            }
        }
    }

    // AOWGauge.pas:250
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

    // AOWGauge.pas:262
    public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (FOnMouseMove != null)
            {
                FOnMouseMove(this, Shift, X, Y);
            }
        }
    }

    // AOWGauge.pas:274
    public override void CheckKeyPress(ref byte Key)
    {
        FHandled = false;
    }

    // AOWGauge.pas:141
    public virtual void SetILFrame(TInterfaceIL Value)
    {
        if (Value != FILFrame)
        {
            FValid = false;
            FILFrame = Value;
        }
    }

    // AOWGauge.pas:150
    public virtual void SetILTNorm(TInterfaceIL Value)
    {
        if (Value != FILTextureNormal)
        {
            FValid = false;
            FILTextureNormal = Value;
        }
    }

    // AOWGauge.pas:159
    public virtual void SetILTSel(TInterfaceIL Value)
    {
        if (Value != FILTextureSelected)
        {
            FValid = false;
            FILTextureSelected = Value;
        }
    }

    // AOWGauge.pas:114
    public void SetILFIndex(int Value)
    {
        if (Value != FILIndexFrame)
        {
            FILIndexFrame = Value;
            FValid = false;
        }
    }

    // AOWGauge.pas:123
    public void SetILTnIndex(int Value)
    {
        if (Value != FILIndexTNormal)
        {
            FILIndexTNormal = Value;
            FValid = false;
        }
    }

    // AOWGauge.pas:132
    public void SetILTsIndex(int Value)
    {
        if (Value != FILIndexTSelected)
        {
            FILIndexTSelected = Value;
            FValid = false;
        }
    }

    // AOWGauge.pas:168
    public void SetPos(int Value)
    {
        if (Value != FPosition)
        {
            FPosition = Value;
            if (FPosition > FMax)
            {
                FPosition = FMax;
            }

            if (FMin > FPosition)
            {
                FPosition = FMin;
            }

            FDrawn = false;
        }
    }

    [Published]
    public int Min { get => FMin; set => FMin = value; }

    [Published]
    public int Max { get => FMax; set => FMax = value; }

    [Published]
    public int Position { get => FPosition; set => SetPos(value); }

    [Published]
    public TInterfaceIL ILFrame { get => FILFrame; set => SetILFrame(value); }

    [Published]
    public TInterfaceIL ILTextureNormal { get => FILTextureNormal; set => SetILTNorm(value); }

    [Published]
    public TInterfaceIL ILTextureSelected { get => FILTextureSelected; set => SetILTSel(value); }

    [Published]
    public int ILIndexFrame { get => FILIndexFrame; set => FILIndexFrame = value; }

    [Published]
    public int ILIndexTNormal { get => FILIndexTNormal; set => SetILTnIndex(value); }

    [Published]
    public int ILIndexTSelected { get => FILIndexTSelected; set => SetILTsIndex(value); }

    public new TNotifyEvent<TAoWGauge> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new TMouseEvent<TAoWGauge> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new TMouseEvent<TAoWGauge> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new TMouseMoveEvent<TAoWGauge> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new TKeyPressEvent<TAoWGauge> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new TKeyEvent<TAoWGauge> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new TAoWOnDrawEvent<TAoWGauge> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new TAoWOnDrawEvent<TAoWGauge> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new TNotifyEvent<TAoWGauge> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}

public static partial class AOWGaugeUnit
{
}
