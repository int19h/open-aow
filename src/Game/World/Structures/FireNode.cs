// Translated from FireNode.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class FireNode(CustomNode owner) : ProductionPlace(owner)
{

    // FireNode.pas:36
    public new const int ClassId = 132_062;

    // FireNode.pas:39
    public override MagicSphere GetMagicSphere() => MagicSphere.Fire;

    public new class Resource : ProductionPlaceResource
    {
        public Resource(CustomNode owner) : base(owner)
        {
            RazeDefenderBudget = 200;
            RebuildGold = 50;
            RebuildDays = 3;
        }

        // FireNode.pas:53
        public new const int ClassId = 132_063;

        // FireNode.pas:56
        public override int ResourceUserClassId() => FireNode.ClassId;

        // FireNode.pas:50
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_fireNodeText);

        private static readonly string s_fireNodeText = "Fire Node";

    }
}
