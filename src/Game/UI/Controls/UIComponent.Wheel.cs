
using System.Runtime.CompilerServices;
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// Port links for the mouse wheel. They are not instance fields. The construction recordings walk
// every instance field, and these links are state the original components do not have.
public partial class UIComponent
{
    private sealed class WheelLink
    {
        public VerticalScrollBar Bar { get; set; }
        public Button Back { get; set; }
        public Button Forward { get; set; }
    }

    private static readonly ConditionalWeakTable<UIComponent, WheelLink> s_wheelLinks = new();

    internal static bool PointInside(int X, int Y, int left, int top, int width, int height) =>
        X > left - 1 && X < left + width && Y > top - 1 && Y < top + height;

    public virtual bool ContainsPoint(int X, int Y) => false;

    // Visible and willing to take a click. A wheel follows that, then bubbles to the parent.
    public virtual bool WantsMouseWheel => _visible && _captureInput;

    public virtual bool CheckMouseWheel(int steps, Set<ShiftState> shift, int X, int Y) => false;

    internal VerticalScrollBar WheelScrollBar
    {
        get => s_wheelLinks.TryGetValue(this, out var link) ? link.Bar : null;
        set => Link().Bar = value;
    }

    internal Button PageBack
    {
        get => s_wheelLinks.TryGetValue(this, out var link) ? link.Back : null;
        set => Link().Back = value;
    }

    internal Button PageForward
    {
        get => s_wheelLinks.TryGetValue(this, out var link) ? link.Forward : null;
        set => Link().Forward = value;
    }

    private WheelLink Link() => s_wheelLinks.GetOrCreateValue(this);

    protected UIComponent TopWheelChild(int X, int Y)
    {
        for (int I = Children.Count - 1; I >= 0; I--)
        {
            var child = Children[I];
            if (child.WantsMouseWheel && child.ContainsPoint(X, Y)) return child;
        }

        return null;
    }

    // The named bar wins. A hidden named bar does not fall through to another bar. With no named
    // bar, exactly one visible enabled vertical scroll bar among the direct children is used.
    protected bool ScrollOwnedBar(int steps)
    {
        if (s_wheelLinks.TryGetValue(this, out var link) && link.Bar != null)
        {
            if (!link.Bar._visible || !link.Bar._captureInput) return false;
            link.Bar.ScrollSteps(steps);
            return true;
        }

        VerticalScrollBar bar = null;
        for (int I = 0; I < Children.Count; I++)
        {
            if (Children[I] is VerticalScrollBar candidate && candidate._visible && candidate._captureInput)
            {
                if (bar != null) return false;
                bar = candidate;
            }
        }

        if (bar == null) return false;
        bar.ScrollSteps(steps);
        return true;
    }
}
