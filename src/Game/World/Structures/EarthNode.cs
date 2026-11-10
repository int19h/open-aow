// Translated from EarthNode.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class EarthNode : ProductionPlace
{
    public EarthNode(ECustomNode owner) : base(owner)
    { }

    // EarthNode.pas:36
    public new const int ClassID = 132302;

    // EarthNode.pas:39
    public override MagicSphere GetMagicSphere() => MagicSphere.Earth;

    public new partial class Resource : ProductionPlaceResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            FRazeDefenderBudget = 200;
            FRebuildGold = 35;
            FRebuildDays = 2;
        }

        // EarthNode.pas:53
        public new const int ClassID = 132303;

        // EarthNode.pas:42

        // EarthNode.pas:56
        public override int ResourceUserClassID() => 132302;

        // EarthNode.pas:26

        // EarthNode.pas:50
        public override string ResourceEditName() => AbilityRules.TranslateRStr(EarthNodeRStr);

        private static readonly string EarthNodeRStr = "Earth Node";

    }

}
