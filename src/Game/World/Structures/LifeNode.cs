// Translated from LifeNode.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class LifeNode : ProductionPlace
{
    public LifeNode(ECustomNode owner) : base(owner)
    { }

    // LifeNode.pas:37
    public new const int ClassID = 131953;

    // LifeNode.pas:40
    public override MagicSphere GetMagicSphere() => MagicSphere.Life;

    public new partial class Resource : ProductionPlaceResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            FRazeDefenderBudget = 200;
            FTerrainBound = false;
            FRebuildGold = 25;
            FRebuildDays = 2;
        }

        // LifeNode.pas:55
        public new const int ClassID = 131954;

        // LifeNode.pas:43

        // LifeNode.pas:58
        public override int ResourceUserClassID() => 131953;

        // LifeNode.pas:27

        // LifeNode.pas:52
        public override string ResourceEditName() => AbilityRules.TranslateRStr(LifeNodeRStr);

        private static readonly string LifeNodeRStr = "Life Node";

    }

}
