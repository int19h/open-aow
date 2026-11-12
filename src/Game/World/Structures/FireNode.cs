// Translated from FireNode.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class FireNode : ProductionPlace
{
    public FireNode(ECustomNode owner) : base(owner)
    { }

    // FireNode.pas:36
    public new const int ClassId = 132062;

    // FireNode.pas:39
    public override MagicSphere GetMagicSphere() => MagicSphere.Fire;

    public new partial class Resource : ProductionPlaceResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            RazeDefenderBudget = 200;
            RebuildGold = 50;
            RebuildDays = 3;
        }

        // FireNode.pas:53
        public new const int ClassId = 132063;

        // FireNode.pas:42

        // FireNode.pas:56
        public override int ResourceUserClassId() => 132062;

        // FireNode.pas:26

        // FireNode.pas:50
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(s_fireNodeText);

        private static readonly string s_fireNodeText = "Fire Node";

    }

}
