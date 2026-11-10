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
    public override bool CanAddUnit(AbstractUnit AUnit)
    {
        bool Result = base.CanAddUnit(AUnit);
        return Result && !AUnit.GetTransporter() && AUnit.ValidTerrain(1, 4);
    }

    // intf_P2_07.inc:445

}
