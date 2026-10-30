// Translated from DeathNode.pas.
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

namespace Aow.Units.DeathNode
{
    // { TDeathNode (instance size 76, VMT 557BFFA4) }
    public partial class TDeathNode : Aow.Units.PrdPlace.TProductionPlace
    {
        public TDeathNode(TECustomNode owner) : base(owner)
        {}

        // DeathNode.pas:37
        public new const int ClassID = 132270;

        // DeathNode.pas:40
        public override Aow.Units.AoWE.TMagicSphere GetMagicSphere()
        {
            Aow.Units.AoWE.TMagicSphere Result = default;
            Result = Aow.Units.AoWE.TMagicSphere.msDeath;
            return Result;
        }

    }

    // { TDeathNodeResource (instance size 120, VMT 557C0258) }
    public partial class TDeathNodeResource : Aow.Units.PrdPlace.TProductionPlaceResource
    {
        public TDeathNodeResource(TECustomNode owner) : base(owner)
        {
            FRazeDefenderBudget = 200;
            FRebuildGold = 25;
            FRebuildDays = 2;
            FTerrainBound = false;
        }

        // DeathNode.pas:55
        public new const int ClassID = 132271;

        // DeathNode.pas:43


        // DeathNode.pas:58
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132270;
            return Result;
        }

        // DeathNode.pas:27


        // DeathNode.pas:52
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DeathNodeRStr);
            return Result;
        }

    }

    public static partial class DeathNode
    {
        // DeathNode.pas:61
        public static void RegisterDeathNode(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.DeathNode.TDeathNode>(), Aow.Units.Engine.EClass.Of<Aow.Units.DeathNode.TDeathNodeResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
