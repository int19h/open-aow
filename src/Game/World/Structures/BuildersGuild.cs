// Translated from BldGuild.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class BuildersGuild : ProductionPlace
{
    public BuildersGuild(ECustomNode owner) : base(owner)
    { }

    // BldGuild.pas:37
    public new const int ClassId = 133422;

    public new partial class Resource : ProductionPlaceResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            TerrainBound = true;
            Claimed = false;
            RazeDefenderBudget = 75;
            RebuildGold = 25;
            RebuildDays = 1;
        }

        // BldGuild.pas:53
        public new const int ClassId = 133423;

        // BldGuild.pas:40

        // BldGuild.pas:56
        public override int ResourceUserClassId() => 133422;

        // BldGuild.pas:26

        // BldGuild.pas:50
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_buildersGuildText);

        private static readonly string s_buildersGuildText = "Builders Guild";

    }

}
