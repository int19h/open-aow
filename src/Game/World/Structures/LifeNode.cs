// Translated from LifeNode.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class LifeNode(CustomNode owner) : ProductionPlace(owner)
{

    // LifeNode.pas:37
    public new const int ClassId = 131_953;

    // LifeNode.pas:40
    public override MagicSphere GetMagicSphere() => MagicSphere.Life;

    public new class Resource : ProductionPlaceResource
    {
        public Resource(CustomNode owner) : base(owner)
        {
            RazeDefenderBudget = 200;
            TerrainBound = false;
            RebuildGold = 25;
            RebuildDays = 2;
        }

        // LifeNode.pas:55
        public new const int ClassId = 131_954;

        // LifeNode.pas:58
        public override int ResourceUserClassId() => LifeNode.ClassId;

        // LifeNode.pas:52
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_lifeNodeText);

        private static readonly string s_lifeNodeText = "Life Node";

    }
}
