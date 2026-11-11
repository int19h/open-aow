// Translated from AoWArnoLine.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// top, middle, bottom, copied line image library
public partial class ArnoLine : Control
{
    public LibraryImage FImageTop;
    public LibraryImage FImageMiddle;
    public LibraryImage FImageBottom;
    public InterfaceImageLibrary FLineLibrary;

    // AoWArnoLine.pas:47
    public override void Dispose()
    {
        FLineLibrary?.Dispose();
        base.Dispose();
    }

    // AoWArnoLine.pas:41
    public ArnoLine(Component owner) : base(owner)
    {
        FLineLibrary = new InterfaceImageLibrary(this);
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
    public override void Draw(LockedSurface SurfDesc)
    {
        Rect OldClip = default;
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

                var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
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
    public void SetITop(LibraryImage Value)
    {
        if (Value != FImageTop)
        {
            FImageTop = Value;
            FValid = false;
        }
    }

    // AoWArnoLine.pas:75
    public void SetIMiddle(LibraryImage Value)
    {
        if (Value != FImageMiddle)
        {
            FImageMiddle = Value;
            FValid = false;
        }
    }

    // AoWArnoLine.pas:84
    public void SetIBottom(LibraryImage Value)
    {
        if (Value != FImageBottom)
        {
            FImageBottom = Value;
            FValid = false;
        }
    }

    [Published]
    public LibraryImage ImageTop { get => FImageTop; set => SetITop(value); }

    [Published]
    public LibraryImage ImageMiddle { get => FImageMiddle; set => SetIMiddle(value); }

    [Published]
    public LibraryImage ImageBottom { get => FImageBottom; set => SetIBottom(value); }

    public new NotifyEvent<ArnoLine> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<ArnoLine> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<ArnoLine> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<ArnoLine> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<ArnoLine> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<ArnoLine> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<ArnoLine> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<ArnoLine> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<ArnoLine> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}
