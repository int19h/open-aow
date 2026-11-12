using System.Runtime.CompilerServices;
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.Tests;

// Test plumbing: a test swaps the creation delegate of a descriptor and restores the previous one.
// The product keeps the delegate read-only and has no setter for it.
internal static class EngineClassCreation
{
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_create")]
    private static extern ref Delegate CreateField(EngineClass engineClass);

    internal static Delegate Substitute(EngineClass engineClass, Delegate next)
    {
        ArgumentNullException.ThrowIfNull(next);
        ref var create = ref CreateField(engineClass);
        var previous = create;
        create = next;
        return previous;
    }

    // The public constructor that takes the owner, and with it the holder where the class's
    // constructor reads the engine (issue 224). The holder is the fixture's engine.
    internal static System.Reflection.ConstructorInfo? OwnerConstructor(Type type) =>
        type.GetConstructor([typeof(ECustomNode)]) ?? type.GetConstructor([typeof(ECustomNode), typeof(EngineObject)]);

    internal static object?[] OwnerArguments(System.Reflection.ConstructorInfo constructor, ECustomNode? owner) =>
        constructor.GetParameters().Length == 1 ? [owner] : [owner, EngineFixture.Engine];
}
