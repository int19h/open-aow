#nullable disable warnings

using System.Runtime.CompilerServices;
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// Port links for the mouse wheel. They are not instance fields. The construction recordings walk
// every instance field, and these links are state the original components do not have.
public partial class UIComponent
{
    private sealed class WheelLink
    {
        public VerticalScrollBar? Bar { get; set; }
        public Button? Back { get; set; }
        public Button? Forward { get; set; }
    }

    private static readonly ConditionalWeakTable<UIComponent, WheelLink?> s_wheelLinks = [];

    internal static bool PointInside(int X, int Y, int left, int top, int width, int height)
        => X > left - 1 && X < left + width && Y > top - 1 && Y < top + height;

    public virtual bool ContainsPoint(int X, int Y) => false;

    // Visible and willing to take a click. A wheel follows that, then bubbles to the parent.
    public virtual bool WantsMouseWheel => _visible && CaptureInput;

    public virtual bool CheckMouseWheel(int steps, ShiftStates shift, int X, int Y) => false;

    internal VerticalScrollBar? WheelScrollBar
    {
        get => s_wheelLinks.TryGetValue(this, out WheelLink link) ? link.Bar : null;
        set => Link().Bar = value;
    }

    internal Button? PageBack
    {
        get => s_wheelLinks.TryGetValue(this, out WheelLink link) ? link.Back : null;
        set => Link().Back = value;
    }

    internal Button? PageForward
    {
        get => s_wheelLinks.TryGetValue(this, out WheelLink link) ? link.Forward : null;
        set => Link().Forward = value;
    }

    private WheelLink? Link() => s_wheelLinks.GetOrCreateValue(this);

    protected UIComponent? TopWheelChild(int X, int Y) => Children.Reverse().FirstOrDefault(item => item.WantsMouseWheel && item.ContainsPoint(X, Y));

    // The named bar wins. A hidden named bar does not fall through to another bar. With no named
    // bar, exactly one visible enabled vertical scroll bar among the direct children is used.
    protected bool ScrollOwnedBar(int steps)
    {
        if (s_wheelLinks.TryGetValue(this, out WheelLink link) && link.Bar is not null)
        {
            if (!link.Bar._visible || !link.Bar.CaptureInput)
            {
                return false;
            }

            link.Bar.ScrollSteps(steps);
            return true;
        }

        VerticalScrollBar? bar = null;
        foreach (UIComponent? component in Children)
        {
            if (component is VerticalScrollBar candidate && candidate._visible && candidate.CaptureInput)
            {
                if (bar is not null)
                {
                    return false;
                }

                bar = candidate;
            }
        }

        if (bar is null)
        {
            return false;
        }

        bar.ScrollSteps(steps);
        return true;
    }
}
