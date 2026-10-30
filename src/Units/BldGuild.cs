// Translated from BldGuild.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.PrdPlace;
using static Aow.Units.PrdPlace.PrdPlace;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.BldGuild
{
    // { TBuildersGuild (instance size 76, no added fields, VMT 557CA0DC) }
    public partial class TBuildersGuild : Aow.Units.PrdPlace.TProductionPlace
    {
        public TBuildersGuild(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // BldGuild.pas:37
        public new const int ClassID = 133422;

        public new partial class Meta : Aow.Units.PrdPlace.TProductionPlace.Meta
        {
            // BldGuild.pas:37
            public override int ClassID() => Aow.Units.BldGuild.TBuildersGuild.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.BldGuild.TBuildersGuild => (Meta)MetaRegistry.Of(typeof(T));
            // PrdPlace.pas:51
            public new Aow.Units.BldGuild.TBuildersGuild Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.BldGuild.TBuildersGuild)base.Create(AOwner);
        }
    }

    // { TBuildersGuildResource (instance size 120, no added fields, VMT 557CA398) }
    public partial class TBuildersGuildResource : Aow.Units.PrdPlace.TProductionPlaceResource
    {
        public TBuildersGuildResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
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


        public new partial class Meta : Aow.Units.PrdPlace.TProductionPlaceResource.Meta
        {
            // BldGuild.pas:53
            public override int ClassID() => Aow.Units.BldGuild.TBuildersGuildResource.ClassID;

            // BldGuild.pas:50
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.BuildersGuildRStr);
                return Result;
            }

            // BldGuild.pas:26
            public new Aow.Units.BldGuild.TBuildersGuildResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.BldGuild.TBuildersGuildResource)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.BldGuild.TBuildersGuildResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public static partial class BldGuild
    {
        // BldGuild.pas:59
        public static int BuildersGuildIncome(sbyte Player)
        {
            int Result = default;
            Aow.Units.AoWE.TPlayer P = default;
            int I = default;
            Aow.Units.AoWE.TPlayerStructure S = default;
            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
            {
                var call__1 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
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
                        if ((call__4 == 133422) && (S.FPlayer == Player))
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
        public static void RegisterBuildersGuild(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.BldGuild.TBuildersGuild>(), Aow.Units.Engine.EClass.Of<Aow.Units.BldGuild.TBuildersGuildResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
