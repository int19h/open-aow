using System.Runtime.CompilerServices;

namespace Aow.Rtl;

// Present on every former direct subclass of the removed root. Descendants inherit it.
[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
internal sealed class LegacyObjectAttribute : Attribute;

// Fixture and smoke processes pin construction order so a captured frame keeps the pixels the old counter produced.
// Every other process reads the identity hash. There is no public instance counter.
internal static class CaptureAddress
{
    private static int pinned;
    private static int next;
    private static readonly ConditionalWeakTable<object, StrongBox<int>> ordinals = new();

    internal static bool Pinned => Volatile.Read(ref pinned) != 0;

    internal static void PinAllocationOrder() => Volatile.Write(ref pinned, 1);

    internal static void Assign(object instance)
    {
        if (!Pinned) return;
        ordinals.Add(instance, new StrongBox<int>(Interlocked.Increment(ref next)));
    }

    // A string key used to take an id without constructing an object. Fixture order keeps that gap.
    internal static int Burn() => Interlocked.Increment(ref next);

    internal static int Of(object? instance)
    {
        if (instance is null) return 0;
        // Masked so the modulo phases never go negative. The runtime does not promise a non-negative hash.
        if (!Pinned) return RuntimeHelpers.GetHashCode(instance) & int.MaxValue;
        return ordinals.TryGetValue(instance, out StrongBox<int>? box) ? box.Value : 0;
    }
}
