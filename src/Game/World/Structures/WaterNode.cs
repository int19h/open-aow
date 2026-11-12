// Translated from WaterNode.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class WaterNode : ProductionPlace
{
    public WaterNode(ECustomNode owner) : base(owner)
    { }

    // WaterNode.pas:37
    public new const int ClassId = 132275;

    // WaterNode.pas:40
    public override MagicSphere GetMagicSphere() => MagicSphere.Water;

    public new partial class Resource : ProductionPlaceResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            RazeDefenderBudget = 200;
            TerrainBound = false;
            RebuildGold = 50;
            RebuildDays = 2;
        }

        // WaterNode.pas:55
        public new const int ClassId = 132276;

        // WaterNode.pas:43

        // WaterNode.pas:58
        public override int ResourceUserClassId() => 132275;

        // WaterNode.pas:27

        // WaterNode.pas:52
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_waterNodeText);

        private static readonly string s_waterNodeText = "Water Node";

    }

}
