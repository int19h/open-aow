// Translated from DeathNode.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class DeathNode : ProductionPlace
{
    public DeathNode(ECustomNode owner) : base(owner)
    { }

    // DeathNode.pas:37
    public new const int ClassId = 132270;

    // DeathNode.pas:40
    public override MagicSphere GetMagicSphere() => MagicSphere.Death;

    public new partial class Resource : ProductionPlaceResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            RazeDefenderBudget = 200;
            RebuildGold = 25;
            RebuildDays = 2;
            TerrainBound = false;
        }

        // DeathNode.pas:55
        public new const int ClassId = 132271;

        // DeathNode.pas:43

        // DeathNode.pas:58
        public override int ResourceUserClassId() => 132270;

        // DeathNode.pas:27

        // DeathNode.pas:52
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(s_deathNodeText);

        private static readonly string s_deathNodeText = "Death Node";

    }

}
