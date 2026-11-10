// Translated from FireNode.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class FireNode : ProductionPlace
{
    public FireNode(ECustomNode owner) : base(owner)
    { }

    // FireNode.pas:36
    public new const int ClassID = 132062;

    // FireNode.pas:39
    public override MagicSphere GetMagicSphere() => MagicSphere.Fire;

    public new partial class Resource : ProductionPlaceResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            FRazeDefenderBudget = 200;
            FRebuildGold = 50;
            FRebuildDays = 3;
        }

        // FireNode.pas:53
        public new const int ClassID = 132063;

        // FireNode.pas:42

        // FireNode.pas:56
        public override int ResourceUserClassID() => 132062;

        // FireNode.pas:26

        // FireNode.pas:50
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(FireNodeRStr);

        private static readonly string FireNodeRStr = "Fire Node";

    }

}
