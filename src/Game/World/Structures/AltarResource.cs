// Translated from Altar.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Structures;

public partial class AltarResource : PlayerStructureResource
{
    public AltarResource(ECustomNode owner) : base(owner)
    {
        FClaimed = false;
        FRazeable = true;
        FRazeDefenderBudget = 150;
        FRebuildGold = 50;
        FRebuildDays = 2;
    }

    // Altar.pas:307
    public new const int ClassID = 132082;

    // Altar.pas:294

    // Altar.pas:310
    public override int ResourceUserClassID() => 132081;

    // Altar.pas:49

    // Altar.pas:304
    public override string ResourceEditName() => "";

}
