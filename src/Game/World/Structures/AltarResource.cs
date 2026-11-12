// Translated from Altar.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Structures;

public partial class AltarResource : PlayerStructureResource
{
    public AltarResource(ECustomNode owner) : base(owner)
    {
        Claimed = false;
        Razeable = true;
        RazeDefenderBudget = 150;
        RebuildGold = 50;
        RebuildDays = 2;
    }

    // Altar.pas:307
    public new const int ClassId = 132082;

    // Altar.pas:294

    // Altar.pas:310
    public override int ResourceUserClassId() => 132081;

    // Altar.pas:49

    // Altar.pas:304
    public override string ResourceEditName() => "";

}
