// Translated from Altar.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Structures;

public class AltarResource : PlayerStructureResource
{
    public AltarResource(CustomNode owner) : base(owner)
    {
        Claimed = false;
        Razeable = true;
        RazeDefenderBudget = 150;
        RebuildGold = 50;
        RebuildDays = 2;
    }

    // Altar.pas:307
    public new const int ClassId = 132_082;

    // Altar.pas:310
    public override int ResourceUserClassId() => Altar.ClassId;

    // Altar.pas:304
    public override string ResourceEditName() => "";

}
