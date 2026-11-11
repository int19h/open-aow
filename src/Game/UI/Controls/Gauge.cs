// Translated from AOWGauge.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// Fields: FMin, FMax, FPosition, FILFrame, FILTextureNormal, FILTextureSelected, FStyle,
// FILIndexFrame, FILIndexTNormal, FILIndexTSelected
public partial class Gauge : Control
{
    public int FMin;
    public int FMax;
    public int FPosition;
    public InterfaceImageLibrary FILFrame;
    public InterfaceImageLibrary FILTextureNormal;
    public InterfaceImageLibrary FILTextureSelected;
    public GaugeType FStyle;
    public int FILIndexFrame;
    public int FILIndexTNormal;
    public int FILIndexTSelected;

    // AOWGauge.pas:69
    public Gauge(Component owner) : base(owner)
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
                FStyle = GaugeType.Normal;
            }
            else
            {
                FStyle = GaugeType.Borderless;
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
            case GaugeType.Normal:
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
    public override void Draw(LockedSurface SurfDesc)
    {
        Rect OldClip = default;
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
                    case GaugeType.Normal:
                        var ilFrameWidth = FILFrame[FILIndexFrame + 3];
                        var ilFrame = FILFrame[FILIndexFrame + 4];
                        P = (FWinWidth - ilFrameWidth.Width - ilFrame.Width) * FPosition / (FMax - FMin);
                        var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
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
                        var rect2 = new Rect(FAOWWindow.FWinLeft + FWinLeft + ilFrame8.Width, FAOWWindow.FWinTop + FWinTop + ilFrame7.Height, FWinWidth - ilFrame5.Width - ilFrame6.Width, FWinHeight - ilFrameHeight.Height - ilFrame4.Height);
                        FILTextureNormal.DrawILT(FILIndexTNormal, rect2, ref SurfDesc);
                        SurfDesc.ClipRect = OldClip;
                        var ilFrame9 = FILFrame[FILIndexFrame + 1];
                        var libraryImage = FILFrame[FILIndexFrame + 6];
                        var filFrameItem = FILFrame[FILIndexFrame + 1];
                        var ilFrameWidth2 = FILFrame[FILIndexFrame + 3];
                        var rect3 = new Rect(FAOWWindow.FWinLeft + FWinLeft + ilFrameWidth2.Width, FAOWWindow.FWinTop + FWinTop + filFrameItem.Height, P, FWinHeight - ilFrame9.Height - libraryImage.Height);
                        FILTextureSelected.DrawILT(FILIndexTSelected, rect3, ref SurfDesc);
                        break;
                    case GaugeType.Borderless:
                        P = FPosition * FWinWidth / (FMax - FMin);
                        if (FAOWWindow.FWinLeft + P + FWinLeft > SurfDesc.ClipRect.Left)
                        {
                            SurfDesc.ClipRect.Left = FAOWWindow.FWinLeft + P + FWinLeft;
                        }

                        var rect4 = new Rect(FAOWWindow.FWinLeft + P + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth - P, FWinHeight);
                        FILTextureNormal.DrawILT(FILIndexTNormal, rect4, ref SurfDesc);
                        SurfDesc.ClipRect = OldClip;
                        var rect5 = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, P, FWinHeight);
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
    public override void CheckMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
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

    // AOWGauge.pas:262
    public override void CheckMouseMove(Set<ShiftState> Shift, int X, int Y)
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
    public virtual void SetILFrame(InterfaceImageLibrary Value)
    {
        if (Value != FILFrame)
        {
            FValid = false;
            FILFrame = Value;
        }
    }

    // AOWGauge.pas:150
    public virtual void SetILTNorm(InterfaceImageLibrary Value)
    {
        if (Value != FILTextureNormal)
        {
            FValid = false;
            FILTextureNormal = Value;
        }
    }

    // AOWGauge.pas:159
    public virtual void SetILTSel(InterfaceImageLibrary Value)
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
    public InterfaceImageLibrary ILFrame { get => FILFrame; set => SetILFrame(value); }

    [Published]
    public InterfaceImageLibrary ILTextureNormal { get => FILTextureNormal; set => SetILTNorm(value); }

    [Published]
    public InterfaceImageLibrary ILTextureSelected { get => FILTextureSelected; set => SetILTSel(value); }

    [Published]
    public int ILIndexFrame { get => FILIndexFrame; set => FILIndexFrame = value; }

    [Published]
    public int ILIndexTNormal { get => FILIndexTNormal; set => SetILTnIndex(value); }

    [Published]
    public int ILIndexTSelected { get => FILIndexTSelected; set => SetILTsIndex(value); }

    public new NotifyEvent<Gauge> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<Gauge> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<Gauge> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<Gauge> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<Gauge> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<Gauge> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<Gauge> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<Gauge> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<Gauge> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }

    public enum GaugeType : byte
    {
        Normal = 0, Borderless = 1
    }
}
