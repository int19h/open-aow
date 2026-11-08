// Translated from AoWArnoLine.pas.
using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AoWControl;
using Aow.Units.AOWInterfaceImgLib;
using Aow.Units.GFXE;
using static Aow.Rtl.RtlUnit;

namespace Aow.Units.AoWArnoLine;

// top, middle, bottom, copied line image library
public partial class TAoWArnoLine : TAOWControl
{
    public ImageLib.TLibraryImage FImageTop;
    public ImageLib.TLibraryImage FImageMiddle;
    public ImageLib.TLibraryImage FImageBottom;
    public TInterfaceIL FLineLibrary;

    // AoWArnoLine.pas:47
    public override void Destroy()
    {
        FLineLibrary.Free();
        base.Destroy();
    }

    // AoWArnoLine.pas:41
    public TAoWArnoLine(TComponent owner) : base(owner)
    {
        FLineLibrary = new TInterfaceIL(this);
    }

    // AoWArnoLine.pas:53
    public override void Validate()
    {
        if (FImageTop != null && FImageMiddle != null && FImageBottom != null)
        {
            FLineLibrary.Clear();
            var copy = FImageTop.Copy();
            FLineLibrary.Add(copy);
            var fImageMiddleCopy = FImageMiddle.Copy();
            FLineLibrary.Add(fImageMiddleCopy);
            var fImageBottomCopy = FImageBottom.Copy();
            FLineLibrary.Add(fImageBottomCopy);
            FValid = true;
        }

        base.Validate();
    }

    // AoWArnoLine.pas:93
    public override void SetSize()
    {
        FWinWidth = FImageTop.Width;
        base.SetSize();
    }

    // AoWArnoLine.pas:99
    public override void Draw(TSurfaceDesc SurfDesc)
    {
        TRect OldClip = default;
        try
        {
            if (FValid && FVisible)
            {
                if (FOnPreDraw != null)
                {
                    FOnPreDraw(this, SurfDesc);
                }

                OldClip = SurfDesc.ClipRect;
                if (FWinTop > SurfDesc.ClipRect.Top)
                {
                    SurfDesc.ClipRect.Top = FWinTop;
                }

                var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                FLineLibrary.DrawILLv(0, rect, ref SurfDesc);
                if (FOnDraw != null)
                {
                    FOnDraw(this, SurfDesc);
                }

                SurfDesc.ClipRect = OldClip;
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AoWArnoLine.pas:66
    public void SetITop(ImageLib.TLibraryImage Value)
    {
        if (Value != FImageTop)
        {
            FImageTop = Value;
            FValid = false;
        }
    }

    // AoWArnoLine.pas:75
    public void SetIMiddle(ImageLib.TLibraryImage Value)
    {
        if (Value != FImageMiddle)
        {
            FImageMiddle = Value;
            FValid = false;
        }
    }

    // AoWArnoLine.pas:84
    public void SetIBottom(ImageLib.TLibraryImage Value)
    {
        if (Value != FImageBottom)
        {
            FImageBottom = Value;
            FValid = false;
        }
    }

    [Published]
    public ImageLib.TLibraryImage ImageTop { get => FImageTop; set => SetITop(value); }

    [Published]
    public ImageLib.TLibraryImage ImageMiddle { get => FImageMiddle; set => SetIMiddle(value); }

    [Published]
    public ImageLib.TLibraryImage ImageBottom { get => FImageBottom; set => SetIBottom(value); }

    public new TNotifyEvent<TAoWArnoLine> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new TMouseEvent<TAoWArnoLine> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new TMouseEvent<TAoWArnoLine> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new TMouseMoveEvent<TAoWArnoLine> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new TKeyPressEvent<TAoWArnoLine> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new TKeyEvent<TAoWArnoLine> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new TAoWOnDrawEvent<TAoWArnoLine> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new TAoWOnDrawEvent<TAoWArnoLine> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new TNotifyEvent<TAoWArnoLine> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}

public static partial class AoWArnoLineUnit
{
    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
