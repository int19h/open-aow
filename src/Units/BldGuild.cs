// Translated from BldGuild.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.PrdPlace;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.BldGuild
{
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
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 133422;
            return Result;
        }

        // BldGuild.pas:26


        // BldGuild.pas:50
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = TranslateRStr(BuildersGuildRStr);
            return Result;
        }

    }

    public static partial class BldGuild
    {
        // BldGuild.pas:59
        public static int BuildersGuildIncome(sbyte Player)
        {
            int Result = default;
            TPlayer P = default;
            int I = default;
            TPlayerStructure S = default;
            if (AoWHSMap != null)
            {
                var call__1 = AoWHSMap.FPlayers.GetPlayers(Player);
                P = call__1;
                Result = 0;
                {
                    I = 0;
                    int end__2 = P.FStructures.Count - 1;
                    for (; I <= end__2; I += 1)
                    {
                        var call__3 = P.FStructures.GetStructure(I);
                        S = call__3;
                        var call__4 = S.EClass.ClassID;
                        if (call__4 == 133422 && S.FPlayer == Player)
                        {
                            Result += S.GetIncome();
                        }
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
}
