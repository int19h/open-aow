// The typed view of a component event that a derived class publishes. The base class declares
// the event with its own sender type. A derived class redeclares it with its own type, and the
// accessors subscribe a view to the base event: the view drops the base sender and calls the
// typed handler with the owner. Remove finds the view of that owner, slot and handler again, so
// the same delegate leaves the base event that joined it.
//
// Invoke is an ordinary instance method of the view class. A snapshot records
// DeclaringType.FullName + "." + Method.Name for that method.
using System.Runtime.CompilerServices;
using Aow.Game.Draw;

using Aow.Rtl;

namespace Aow.Game.UI.Controls;

internal static class EventSlot
{
    private static readonly ConditionalWeakTable<object, List<SlotView>> s_views = new();

    public static NotifyEvent<TBase> Add<TBase, TOwner>(TOwner owner, string slot, NotifyEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => value is null ? null : Keep(owner, new NotifyView<TBase, TOwner>(owner, slot, value)).Invoke;

    public static NotifyEvent<TBase> Remove<TBase, TOwner>(TOwner owner, string slot, NotifyEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => Find<NotifyView<TBase, TOwner>>(owner, slot, value) is { } view ? view.Invoke : null;

    public static MouseEvent<TBase> Add<TBase, TOwner>(TOwner owner, string slot, MouseEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => value is null ? null : Keep(owner, new MouseView<TBase, TOwner>(owner, slot, value)).Invoke;

    public static MouseEvent<TBase> Remove<TBase, TOwner>(TOwner owner, string slot, MouseEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => Find<MouseView<TBase, TOwner>>(owner, slot, value) is { } view ? view.Invoke : null;

    public static MouseMoveEvent<TBase> Add<TBase, TOwner>(TOwner owner, string slot, MouseMoveEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => value is null ? null : Keep(owner, new MouseMoveView<TBase, TOwner>(owner, slot, value)).Invoke;

    public static MouseMoveEvent<TBase> Remove<TBase, TOwner>(TOwner owner, string slot, MouseMoveEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => Find<MouseMoveView<TBase, TOwner>>(owner, slot, value) is { } view ? view.Invoke : null;

    public static KeyPressEvent<TBase> Add<TBase, TOwner>(TOwner owner, string slot, KeyPressEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => value is null ? null : Keep(owner, new KeyPressView<TBase, TOwner>(owner, slot, value)).Invoke;

    public static KeyPressEvent<TBase> Remove<TBase, TOwner>(TOwner owner, string slot, KeyPressEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => Find<KeyPressView<TBase, TOwner>>(owner, slot, value) is { } view ? view.Invoke : null;

    public static KeyEvent<TBase> Add<TBase, TOwner>(TOwner owner, string slot, KeyEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => value is null ? null : Keep(owner, new KeyView<TBase, TOwner>(owner, slot, value)).Invoke;

    public static KeyEvent<TBase> Remove<TBase, TOwner>(TOwner owner, string slot, KeyEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => Find<KeyView<TBase, TOwner>>(owner, slot, value) is { } view ? view.Invoke : null;

    public static AoWOnDrawEvent<TBase> Add<TBase, TOwner>(TOwner owner, string slot, AoWOnDrawEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => value is null ? null : Keep(owner, new DrawView<TBase, TOwner>(owner, slot, value)).Invoke;

    public static AoWOnDrawEvent<TBase> Remove<TBase, TOwner>(TOwner owner, string slot, AoWOnDrawEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => Find<DrawView<TBase, TOwner>>(owner, slot, value) is { } view ? view.Invoke : null;

    public static AoWOnDrawBackEvent<TBase> Add<TBase, TOwner>(TOwner owner, string slot, AoWOnDrawBackEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => value is null ? null : Keep(owner, new DrawBackView<TBase, TOwner>(owner, slot, value)).Invoke;

    public static AoWOnDrawBackEvent<TBase> Remove<TBase, TOwner>(TOwner owner, string slot, AoWOnDrawBackEvent<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => Find<DrawBackView<TBase, TOwner>>(owner, slot, value) is { } view ? view.Invoke : null;

    public static AoWOnDrawSurface<TBase> Add<TBase, TOwner>(TOwner owner, string slot, AoWOnDrawSurface<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => value is null ? null : Keep(owner, new DrawSurfaceView<TBase, TOwner>(owner, slot, value)).Invoke;

    public static AoWOnDrawSurface<TBase> Remove<TBase, TOwner>(TOwner owner, string slot, AoWOnDrawSurface<TOwner> value)
        where TBase : class
        where TOwner : class, TBase
        => Find<DrawSurfaceView<TBase, TOwner>>(owner, slot, value) is { } view ? view.Invoke : null;

    // A null handler adds nothing, as += null does, so only a handler gets a view.
    private static TView Keep<TView>(object owner, TView view)
        where TView : SlotView
    {
        s_views.GetOrCreateValue(owner).Add(view);
        return view;
    }

    // The last view of that slot and handler, as -= removes the last occurrence.
    private static TView Find<TView>(object owner, string slot, Delegate value)
        where TView : SlotView
    {
        if (value is null || !s_views.TryGetValue(owner, out var views)) return null;
        for (int i = views.Count - 1; i >= 0; i--)
        {
            if (views[i] is TView view && view.Slot == slot && view.Handler.Equals(value))
            {
                views.RemoveAt(i);
                return view;
            }
        }

        return null;
    }

    internal sealed class NotifyView<TBase, TOwner>(TOwner owner, string slot, NotifyEvent<TOwner> typed)
        : SlotView<TOwner, NotifyEvent<TOwner>>(owner, slot, typed)
        where TBase : class
        where TOwner : class, TBase
    {
        internal void Invoke(TBase _) => Typed(Owner);
    }

    internal sealed class MouseView<TBase, TOwner>(TOwner owner, string slot, MouseEvent<TOwner> typed)
        : SlotView<TOwner, MouseEvent<TOwner>>(owner, slot, typed)
        where TBase : class
        where TOwner : class, TBase
    {
        internal void Invoke(TBase _, MouseButton button, Set<ShiftState> shift, int x, int y) => Typed(Owner, button, shift, x, y);
    }

    internal sealed class MouseMoveView<TBase, TOwner>(TOwner owner, string slot, MouseMoveEvent<TOwner> typed)
        : SlotView<TOwner, MouseMoveEvent<TOwner>>(owner, slot, typed)
        where TBase : class
        where TOwner : class, TBase
    {
        internal void Invoke(TBase _, Set<ShiftState> shift, int x, int y) => Typed(Owner, shift, x, y);
    }

    internal sealed class KeyPressView<TBase, TOwner>(TOwner owner, string slot, KeyPressEvent<TOwner> typed)
        : SlotView<TOwner, KeyPressEvent<TOwner>>(owner, slot, typed)
        where TBase : class
        where TOwner : class, TBase
    {
        internal void Invoke(TBase _, ref byte key) => Typed(Owner, ref key);
    }

    internal sealed class KeyView<TBase, TOwner>(TOwner owner, string slot, KeyEvent<TOwner> typed)
        : SlotView<TOwner, KeyEvent<TOwner>>(owner, slot, typed)
        where TBase : class
        where TOwner : class, TBase
    {
        internal void Invoke(TBase _, ref ushort key, Set<ShiftState> shift) => Typed(Owner, ref key, shift);
    }

    internal sealed class DrawView<TBase, TOwner>(TOwner owner, string slot, AoWOnDrawEvent<TOwner> typed)
        : SlotView<TOwner, AoWOnDrawEvent<TOwner>>(owner, slot, typed)
        where TBase : class
        where TOwner : class, TBase
    {
        internal void Invoke(TBase _, LockedSurface surface) => Typed(Owner, surface);
    }

    internal sealed class DrawBackView<TBase, TOwner>(TOwner owner, string slot, AoWOnDrawBackEvent<TOwner> typed)
        : SlotView<TOwner, AoWOnDrawBackEvent<TOwner>>(owner, slot, typed)
        where TBase : class
        where TOwner : class, TBase
    {
        internal void Invoke(TBase _, LockedSurface surface) => Typed(Owner, surface);
    }

    internal sealed class DrawSurfaceView<TBase, TOwner>(TOwner owner, string slot, AoWOnDrawSurface<TOwner> typed)
        : SlotView<TOwner, AoWOnDrawSurface<TOwner>>(owner, slot, typed)
        where TBase : class
        where TOwner : class, TBase
    {
        internal void Invoke(TBase _, Surface surface, int left, int top) => Typed(Owner, surface, left, top);
    }
}
