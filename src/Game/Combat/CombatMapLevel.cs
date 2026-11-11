// Translated from AoWTC.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Combat;

// Inherited world-map level layout
public partial class CombatMapLevel : AoWMapLevel
{
    public CombatMapLevel(ECustomNode owner) : base(owner)
    { }

    // AoWTC.pas:2173
    public new const int ClassID = 2228482;

    // AoWTC.pas:2178
    public override EngineClass MapFieldClass()
    {
        // { Original uses the world-map field class. }
        return EngineClass.Of<AoWMapField>();
    }

}
