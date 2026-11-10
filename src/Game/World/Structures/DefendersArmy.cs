// Translated from ExploreS.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

namespace Aow.Game.World.Structures;

// { ot=1 }
public partial class DefendersArmy : Army
{
    public DefendersArmy(ECustomNode owner) : base(owner)
    { }

    // ExploreS.pas:71
    public override bool CanAddUnit(AbstractUnit AUnit)
    {
        bool Result = base.CanAddUnit(AUnit);
        return Result && !AUnit.GetTransporter() && AUnit.ValidTerrain(1, 4);
    }

    // intf_P2_07.inc:445

}
