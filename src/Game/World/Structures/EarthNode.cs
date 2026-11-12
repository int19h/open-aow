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
    public new const int ClassId = 132302;

    // EarthNode.pas:39
    public override MagicSphere GetMagicSphere() => MagicSphere.Earth;

    public new partial class Resource : ProductionPlaceResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            RazeDefenderBudget = 200;
            RebuildGold = 35;
            RebuildDays = 2;
        }

        // EarthNode.pas:53
        public new const int ClassId = 132303;

        // EarthNode.pas:42

        // EarthNode.pas:56
        public override int ResourceUserClassId() => 132302;

        // EarthNode.pas:26

        // EarthNode.pas:50
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_earthNodeText);

        private static readonly string s_earthNodeText = "Earth Node";

    }

}
