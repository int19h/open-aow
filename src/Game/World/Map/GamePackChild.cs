using Aow.Engine.Nodes;

namespace Aow.Game.World.Map;

/// <summary>
/// A child of a sprite set: a resource list, a hexagon or road terrain control, or a control
/// object (the resource edit settings, the pad and flag controls). The set takes no other child,
/// so a read of a child slot is one of these or empty.
/// </summary>
public union GamePackChild(EResourceList, MapObjectResourceTerrainControl, ControlStyleObject)
{
    /// <summary>
    /// The child a set slot holds, read from the untyped storage. The set admits only these
    /// kinds, so another kind is an error in the set and throws.
    /// </summary>
    internal static GamePackChild Of(object child) => child switch
    {
        null => default,
        EResourceList list => list,
        MapObjectResourceTerrainControl control => control,
        ControlStyleObject controlObject => controlObject,
        _ => throw new InvalidCastException("A sprite set does not hold a child of type " + child.GetType().Name + "."),
    };

    /// <summary>The child as the engine object the untyped child list stores.</summary>
    public EngineObject Node => (EngineObject)Value;
}
