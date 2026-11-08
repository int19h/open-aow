using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWVScroll;

namespace Aow.Units.AoWComp;

// Port links for the mouse wheel. They are not instance fields. The construction recordings walk
// every instance field, and these links are state the original components do not have.
public partial class TAoWComponent
{
    private sealed class WheelLink
    {
        public TAOWVScrollBar Bar;
        public TAOWButton Back;
        public TAOWButton Forward;
    }

    private static readonly System.Runtime.CompilerServices.ConditionalWeakTable<TAoWComponent, WheelLink> wheelLinks = new();

    public static bool PointInside(int X, int Y, int Left, int Top, int Width, int Height) =>
        X > Left - 1 && X < Left + Width && Y > Top - 1 && Y < Top + Height;

    public virtual bool ContainsPoint(int X, int Y) => false;

    // Visible and willing to take a click. A wheel follows that, then bubbles to the parent.
    public virtual bool WantsMouseWheel => FVisible && FCaptureInput;

    public virtual bool CheckMouseWheel(int Steps, TSet<TShiftState> Shift, int X, int Y) => false;

    public TAOWVScrollBar WheelScrollBar
    {
        get => wheelLinks.TryGetValue(this, out var link) ? link.Bar : null;
        set => Link().Bar = value;
    }

    public TAOWButton PageBack
    {
        get => wheelLinks.TryGetValue(this, out var link) ? link.Back : null;
        set => Link().Back = value;
    }

    public TAOWButton PageForward
    {
        get => wheelLinks.TryGetValue(this, out var link) ? link.Forward : null;
        set => Link().Forward = value;
    }

    private WheelLink Link() => wheelLinks.GetOrCreateValue(this);

    protected TAoWComponent TopWheelChild(int X, int Y)
    {
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            var child = (TAoWComponent)FChildren[I];
            if (child.WantsMouseWheel && child.ContainsPoint(X, Y)) return child;
        }

        return null;
    }

    // The named bar wins. A hidden named bar does not fall through to another bar. With no named
    // bar, exactly one visible enabled vertical scroll bar among the direct children is used.
    protected bool ScrollOwnedBar(int Steps)
    {
        if (wheelLinks.TryGetValue(this, out var link) && link.Bar != null)
        {
            if (!link.Bar.FVisible || !link.Bar.FCaptureInput) return false;
            link.Bar.ScrollSteps(Steps);
            return true;
        }

        TAOWVScrollBar bar = null;
        for (int I = 0; I < FChildren.Count; I++)
        {
            if (FChildren[I] is TAOWVScrollBar candidate && candidate.FVisible && candidate.FCaptureInput)
            {
                if (bar != null) return false;
                bar = candidate;
            }
        }

        if (bar == null) return false;
        bar.ScrollSteps(Steps);
        return true;
    }
}
