using System.Runtime.CompilerServices;
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.Tests;

// Test plumbing: a test swaps the creation delegate of a descriptor and restores the previous one.
// The product keeps the delegate read-only and has no setter for it.
internal static class EngineClassCreation
{
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_create")]
    private static extern ref Func<ECustomNode, EngineObject> CreateField(EngineClass engineClass);

    internal static Func<ECustomNode, EngineObject> Substitute(EngineClass engineClass, Func<ECustomNode, EngineObject> next)
    {
        ArgumentNullException.ThrowIfNull(next);
        ref var create = ref CreateField(engineClass);
        var previous = create;
        create = next;
        return previous;
    }
}
