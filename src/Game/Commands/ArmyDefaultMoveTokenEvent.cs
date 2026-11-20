// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Units;

using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Commands;

public class ArmyDefaultMoveTokenEvent(CustomNode owner) : MoveArmyTokenEvent(owner)
{
    public new const int ClassId = 135428;

    // MoveT.PAS:2394
    public override bool Setup(ArmyHexSprite armyHexSprite, byte selection, MovePath path, Set<ArmyMoveModeElement> mode) => base.Setup(armyHexSprite, selection, path, mode);
}
