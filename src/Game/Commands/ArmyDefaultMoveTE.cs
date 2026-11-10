// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Units;

using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Commands;

public partial class ArmyDefaultMoveTE : MoveArmyTE
{
    public ArmyDefaultMoveTE(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 135428;

    // MoveT.PAS:2394
    public override bool Setup(ArmyHS ArmyHS, byte Selection, Movepath Path, Set<ArmyMoveModeElement> Mode) => base.Setup(ArmyHS, Selection, Path, Mode);

    // intf_P5_07.inc:816

}
