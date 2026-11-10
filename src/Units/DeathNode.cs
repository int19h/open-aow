// Translated from DeathNode.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class DeathNode : ProductionPlace
{
    public DeathNode(ECustomNode owner) : base(owner)
    { }

    // DeathNode.pas:37
    public new const int ClassID = 132270;

    // DeathNode.pas:40
    public override MagicSphere GetMagicSphere() => MagicSphere.Death;

    public new partial class Resource : ProductionPlaceResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            FRazeDefenderBudget = 200;
            FRebuildGold = 25;
            FRebuildDays = 2;
            FTerrainBound = false;
        }

        // DeathNode.pas:55
        public new const int ClassID = 132271;

        // DeathNode.pas:43

        // DeathNode.pas:58
        public override int ResourceUserClassID() => 132270;

        // DeathNode.pas:27

        // DeathNode.pas:52
        public override string ResourceEditName() => Magic.Abilities.AbilityRules.TranslateRStr(DeathNodeRStr);

        private static readonly string DeathNodeRStr = "Death Node";

    }

}
