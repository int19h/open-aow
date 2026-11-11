// Translated from Shipyard.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class Shipyard : ProductionPlace
{
    public Shipyard(ECustomNode owner) : base(owner)
    { }

    // Shipyard.pas:49
    public new const int ClassID = 132286;

    // Shipyard.pas:55
    public override void UpdateAITarget(in AITargetInfo Info, ref AITargetData Data)
    {
        base.UpdateAITarget(Info, ref Data);
    }

    // Shipyard.pas:52
    public override int GetBaseIncome() => 0;

    public new partial class Resource : ProductionPlaceResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            FTerrainBound = false;
            FRazeDefenderBudget = 150;
            FRebuildGold = 25;
            FRebuildDays = 2;
        }

        // Shipyard.pas:72
        public new const int ClassID = 132287;

        // Shipyard.pas:60

        // Shipyard.pas:75
        public override int ResourceUserClassID() => 132286;

        // Shipyard.pas:29

        // Shipyard.pas:69
        public override string ResourceEditName() => AbilityRules.TranslateRStr(GameText.ShipyardRStr);

    }

}
