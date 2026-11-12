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
    public new const int ClassId = 132286;

    // Shipyard.pas:55
    public override void UpdateAITarget(in AITargetInfo info, ref AITargetData data)
    {
        base.UpdateAITarget(info, ref data);
    }

    // Shipyard.pas:52
    public override int GetBaseIncome() => 0;

    public new partial class Resource : ProductionPlaceResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            TerrainBound = false;
            RazeDefenderBudget = 150;
            RebuildGold = 25;
            RebuildDays = 2;
        }

        // Shipyard.pas:72
        public new const int ClassId = 132287;

        // Shipyard.pas:60

        // Shipyard.pas:75
        public override int ResourceUserClassId() => 132286;

        // Shipyard.pas:29

        // Shipyard.pas:69
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GameText.s_shipyardText);

    }

}
