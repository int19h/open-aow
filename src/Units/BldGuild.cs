// Translated from BldGuild.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Session;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class TBuildersGuild : TProductionPlace
{
    public TBuildersGuild(TECustomNode owner) : base(owner)
    { }

    // BldGuild.pas:37
    public new const int ClassID = 133422;

}

public partial class TBuildersGuildResource : TProductionPlaceResource
{
    public TBuildersGuildResource(TECustomNode owner) : base(owner)
    {
        FTerrainBound = true;
        FClaimed = false;
        FRazeDefenderBudget = 75;
        FRebuildGold = 25;
        FRebuildDays = 1;
    }

    // BldGuild.pas:53
    public new const int ClassID = 133423;

    // BldGuild.pas:40

    // BldGuild.pas:56
    public override int ResourceUserClassID() => 133422;

    // BldGuild.pas:26

    // BldGuild.pas:50
    public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TBuildersGuildResource.BuildersGuildRStr);

    private static readonly string BuildersGuildRStr = "Builders Guild";

}
