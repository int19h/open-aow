// Translated from BldGuild.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class BuildersGuild(CustomNode owner) : ProductionPlace(owner)
{

    // BldGuild.pas:37
    public new const int ClassId = 133_422;

    public new class Resource : ProductionPlaceResource
    {
        public Resource(CustomNode owner) : base(owner)
        {
            TerrainBound = true;
            Claimed = false;
            RazeDefenderBudget = 75;
            RebuildGold = 25;
            RebuildDays = 1;
        }

        // BldGuild.pas:53
        public new const int ClassId = 133_423;

        // BldGuild.pas:56
        public override int ResourceUserClassId() => BuildersGuild.ClassId;

        // BldGuild.pas:50
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_buildersGuildText);

        private static readonly string s_buildersGuildText = "Builders Guild";

    }
}
