// Translated from AirNode.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class AirNode : ProductionPlace
{
    public AirNode(ECustomNode owner) : base(owner)
    { }

    // AirNode.pas:37
    public new const int ClassID = 133454;

    // AirNode.pas:40
    public override MagicSphere GetMagicSphere() => MagicSphere.Air;

    public new partial class Resource : ProductionPlaceResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            FRazeDefenderBudget = 200;
            FRebuildGold = 50;
            FRebuildDays = 2;
            FTerrainBound = false;
        }

        // AirNode.pas:55
        public new const int ClassID = 133455;

        // AirNode.pas:43

        // AirNode.pas:58
        public override int ResourceUserClassID() => 133454;

        // AirNode.pas:27

        // AirNode.pas:52
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(AirNodeRStr);

        private static readonly string AirNodeRStr = "Air Node";

    }

}
