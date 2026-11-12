// Translated from AirNode.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class AirNode : ProductionPlace
{
    public AirNode(ECustomNode owner) : base(owner)
    { }

    // AirNode.pas:37
    public new const int ClassId = 133454;

    // AirNode.pas:40
    public override MagicSphere GetMagicSphere() => MagicSphere.Air;

    public new partial class Resource : ProductionPlaceResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            RazeDefenderBudget = 200;
            RebuildGold = 50;
            RebuildDays = 2;
            TerrainBound = false;
        }

        // AirNode.pas:55
        public new const int ClassId = 133455;

        // AirNode.pas:43

        // AirNode.pas:58
        public override int ResourceUserClassId() => 133454;

        // AirNode.pas:27

        // AirNode.pas:52
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(s_airNodeText);

        private static readonly string s_airNodeText = "Air Node";

    }

}
