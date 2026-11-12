// Translated from Dungeon.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

// { ot=1 }
public partial class PrisonersArmy : Army
{
    public PrisonersArmy(ECustomNode owner) : base(owner)
    { }

    // Dungeon.pas:89
    public override bool CanAddUnit(Troop troop)
    {
        bool result = base.CanAddUnit(troop);
        return result && !troop.GetTransporter() && troop.ValidTerrain(1, 4);
    }

    // intf_P2_07.inc:445

}
