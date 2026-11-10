// Translated from RandomNode.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class RandomNode : ProductionPlace
{
    public RandomNode(ECustomNode owner) : base(owner)
    { }

    // RandomNode.pas:35
    public new const int ClassID = 133154;

    public new partial class Resource : ProductionPlaceResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
        }

        // RandomNode.pas:46
        public new const int ClassID = 133155;

        // RandomNode.pas:38

        // RandomNode.pas:49
        public override int ResourceUserClassID() => 133154;

        // RandomNode.pas:25

        // RandomNode.pas:43
        public override string ResourceEditName() => Magic.Abilities.AbilityRules.TranslateRStr(RandomNodeRStr);

        private static readonly string RandomNodeRStr = "Random Node";

    }

}
