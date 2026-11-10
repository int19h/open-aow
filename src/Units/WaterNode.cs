// Translated from WaterNode.pas.
using Aow.Engine.Nodes;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class WaterNode : ProductionPlace
{
    public WaterNode(ECustomNode owner) : base(owner)
    { }

    // WaterNode.pas:37
    public new const int ClassID = 132275;

    // WaterNode.pas:40
    public override MagicSphere GetMagicSphere() => MagicSphere.Water;

    public new partial class Resource : ProductionPlaceResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            FRazeDefenderBudget = 200;
            FTerrainBound = false;
            FRebuildGold = 50;
            FRebuildDays = 2;
        }

        // WaterNode.pas:55
        public new const int ClassID = 132276;

        // WaterNode.pas:43

        // WaterNode.pas:58
        public override int ResourceUserClassID() => 132275;

        // WaterNode.pas:27

        // WaterNode.pas:52
        public override string ResourceEditName() => AbilityRules.TranslateRStr(WaterNodeRStr);

        private static readonly string WaterNodeRStr = "Water Node";

    }

}
