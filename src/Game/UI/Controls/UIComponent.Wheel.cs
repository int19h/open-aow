
using System.Runtime.CompilerServices;
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// Port links for the mouse wheel. They are not instance fields. The construction recordings walk
// every instance field, and these links are state the original components do not have.
public partial class UIComponent
{
    private sealed class WheelLink
    {
        public VerticalScrollBar Bar;
        public Button Back;
        public Button Forward;
    }

    private static readonly ConditionalWeakTable<UIComponent, WheelLink> wheelLinks = new();

    internal static bool PointInside(int X, int Y, int Left, int Top, int Width, int Height) =>
        X > Left - 1 && X < Left + Width && Y > Top - 1 && Y < Top + Height;

    public virtual bool ContainsPoint(int X, int Y) => false;

    // Visible and willing to take a click. A wheel follows that, then bubbles to the parent.
    public virtual bool WantsMouseWheel => FVisible && FCaptureInput;

    public virtual bool CheckMouseWheel(int Steps, Set<ShiftState> Shift, int X, int Y) => false;

    internal VerticalScrollBar WheelScrollBar
    {
        get => wheelLinks.TryGetValue(this, out var link) ? link.Bar : null;
        set => Link().Bar = value;
    }

    internal Button PageBack
    {
        get => wheelLinks.TryGetValue(this, out var link) ? link.Back : null;
        set => Link().Back = value;
    }

    internal Button PageForward
    {
        get => wheelLinks.TryGetValue(this, out var link) ? link.Forward : null;
        set => Link().Forward = value;
    }

    private WheelLink Link() => wheelLinks.GetOrCreateValue(this);

    protected UIComponent TopWheelChild(int X, int Y)
    {
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            var child = (UIComponent)FChildren[I];
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

        VerticalScrollBar bar = null;
        for (int I = 0; I < FChildren.Count; I++)
        {
            if (FChildren[I] is VerticalScrollBar candidate && candidate.FVisible && candidate.FCaptureInput)
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
