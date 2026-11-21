#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from ExploreS.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

namespace Aow.Game.World.Structures;

// { ot=1 }
public sealed class DefendersArmy(CustomNode owner) : Army(owner)
{

    // ExploreS.pas:71
    public override bool CanAddUnit(Troop troop)
    {
        bool result = base.CanAddUnit(troop);
        return result && !troop.GetTransporter() && troop.ValidTerrain(1, overlayType: 4);
    }
}
