// Translated from EarthNode.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class EarthNode(CustomNode owner) : ProductionPlace(owner)
{

    // EarthNode.pas:36
    public new const int ClassId = 132_302;

    // EarthNode.pas:39
    public override MagicSphere GetMagicSphere() => MagicSphere.Earth;

    public new class Resource : ProductionPlaceResource
    {
        public Resource(CustomNode owner) : base(owner)
        {
            RazeDefenderBudget = 200;
            RebuildGold = 35;
            RebuildDays = 2;
        }

        // EarthNode.pas:53
        public new const int ClassId = 132_303;

        // EarthNode.pas:56
        public override int ResourceUserClassId() => EarthNode.ClassId;

        // EarthNode.pas:50
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_earthNodeText);

        private static readonly string s_earthNodeText = "Earth Node";

    }
}
