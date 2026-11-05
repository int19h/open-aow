// Translated from BldGuild.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.PrdPlace;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.BldGuild;

public partial class TBuildersGuild : TProductionPlace
{
    public TBuildersGuild(TECustomNode owner) : base(owner)
    {}

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
    public override string ResourceEditName() => TranslateRStr(BuildersGuildRStr);

}

public static partial class BldGuild
{
    // BldGuild.pas:59
    public static int BuildersGuildIncome(sbyte Player)
    {
        int Result;
        TPlayer P;
        TPlayerStructure S;
        if (AoWHSMap != null)
        {
            P = AoWHSMap.FPlayers.GetPlayers(Player);
            Result = 0;
            for (int I = 0, last = P.FStructures.Count - 1; I <= last; I++)
            {
                S = P.FStructures.GetStructure(I);
                if (S.EClass.ClassID == 133422 && S.FPlayer == Player)
                {
                    Result += S.GetIncome();
                }
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // BldGuild.pas:79
    public static void RegisterBuildersGuild(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TBuildersGuild>(), EClass.Of<TBuildersGuildResource>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
