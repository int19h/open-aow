// Translated from AirNode.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class AirNode(CustomNode owner) : ProductionPlace(owner)
{

    // AirNode.pas:37
    public new const int ClassId = 133_454;

    // AirNode.pas:40
    public override MagicSphere GetMagicSphere() => MagicSphere.Air;

    public new class Resource : ProductionPlaceResource
    {
        public Resource(CustomNode owner) : base(owner)
        {
            RazeDefenderBudget = 200;
            RebuildGold = 50;
            RebuildDays = 2;
            TerrainBound = false;
        }

        // AirNode.pas:55
        public new const int ClassId = 133_455;

        // AirNode.pas:58
        public override int ResourceUserClassId() => AirNode.ClassId;

        // AirNode.pas:52
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_airNodeText);

        private static readonly string s_airNodeText = "Air Node";

    }
}
