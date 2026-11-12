using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game;

/// <summary>
/// A child of the engine: its control node, its sprite set or one of its maps. The engine takes
/// no other child, so a read of a child slot is one of these or empty.
/// </summary>
public union GameEngineChild(EControlStyleNode, HSSet, HSMap)
{
    /// <summary>
    /// The child an engine slot holds, read from the untyped storage. The engine admits only
    /// these kinds, so another kind is an error in the engine and throws.
    /// </summary>
    internal static GameEngineChild Of(object child) => child switch
    {
        null => default,
        EControlStyleNode node => node,
        HSSet set => set,
        HSMap map => map,
        _ => throw new InvalidCastException("An engine does not hold a child of type " + child.GetType().Name + "."),
    };

    /// <summary>The child as the engine object the untyped child list stores.</summary>
    public EngineObject Node => (EngineObject)Value;
}
