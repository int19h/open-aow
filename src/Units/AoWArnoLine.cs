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
        get => EventSlot.Get(this, "TAoWArnoLine.FOnChange", FOnChange, static slot => slot);
        set => EventSlot.Set(this, "TAoWArnoLine.FOnChange", ref FOnChange, value, handler => sender => handler(this));
    }

    public new TMouseEvent<TAoWArnoLine> OnMouseDown
    {
        get => EventSlot.Get(this, "TAoWArnoLine.FOnMouseDown", FOnMouseDown, static slot => slot);
        set => EventSlot.Set(this, "TAoWArnoLine.FOnMouseDown", ref FOnMouseDown, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseEvent<TAoWArnoLine> OnMouseUp
    {
        get => EventSlot.Get(this, "TAoWArnoLine.FOnMouseUp", FOnMouseUp, static slot => slot);
        set => EventSlot.Set(this, "TAoWArnoLine.FOnMouseUp", ref FOnMouseUp, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseMoveEvent<TAoWArnoLine> OnMouseMove
    {
        get => EventSlot.Get(this, "TAoWArnoLine.FOnMouseMove", FOnMouseMove, static slot => slot);
        set => EventSlot.Set(this, "TAoWArnoLine.FOnMouseMove", ref FOnMouseMove, value, handler => (sender, shift, x, y) => handler(this, shift, x, y));
    }

    public new TKeyPressEvent<TAoWArnoLine> OnKeyPress
    {
        get => EventSlot.Get(this, "TAoWArnoLine.FOnKeyPress", FOnKeyPress, static slot => slot);
        set => EventSlot.Set(this, "TAoWArnoLine.FOnKeyPress", ref FOnKeyPress, value, handler => (sender, ref key) => handler(this, ref key));
    }

    public new TKeyEvent<TAoWArnoLine> OnKeyDown
    {
        get => EventSlot.Get(this, "TAoWArnoLine.FOnKeyDown", FOnKeyDown, static slot => slot);
        set => EventSlot.Set(this, "TAoWArnoLine.FOnKeyDown", ref FOnKeyDown, value, handler => (sender, ref key, shift) => handler(this, ref key, shift));
    }

    public new TAoWOnDrawEvent<TAoWArnoLine> OnPreDraw
    {
        get => EventSlot.Get(this, "TAoWArnoLine.FOnPreDraw", FOnPreDraw, static slot => slot);
        set => EventSlot.Set(this, "TAoWArnoLine.FOnPreDraw", ref FOnPreDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TAoWOnDrawEvent<TAoWArnoLine> OnDraw
    {
        get => EventSlot.Get(this, "TAoWArnoLine.FOnDraw", FOnDraw, static slot => slot);
        set => EventSlot.Set(this, "TAoWArnoLine.FOnDraw", ref FOnDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TNotifyEvent<TAoWArnoLine> OnUpdate
    {
        get => EventSlot.Get(this, "TAoWArnoLine.FOnUpdate", FOnUpdate, static slot => slot);
        set => EventSlot.Set(this, "TAoWArnoLine.FOnUpdate", ref FOnUpdate, value, handler => sender => handler(this));
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
