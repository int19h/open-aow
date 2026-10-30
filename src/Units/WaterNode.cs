// Translated from WaterNode.pas.
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

namespace Aow.Units.WaterNode
{
    // { TWaterNode (instance size 76, no added fields, VMT 557C092C) }
    public partial class TWaterNode : Aow.Units.PrdPlace.TProductionPlace
    {
        public TWaterNode(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // WaterNode.pas:37
        public new const int ClassID = 132275;

        // WaterNode.pas:40
        public override Aow.Units.AoWE.TMagicSphere GetMagicSphere()
        {
            Aow.Units.AoWE.TMagicSphere Result = default;
            Result = Aow.Units.AoWE.TMagicSphere.msWater;
            return Result;
        }

    }

    // { TWaterNodeResource (instance size 120, no added fields, VMT 557C0BE0) }
    public partial class TWaterNodeResource : Aow.Units.PrdPlace.TProductionPlaceResource
    {
        public TWaterNodeResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FRazeDefenderBudget = 200;
            FTerrainBound = false;
            FRebuildGold = 50;
            FRebuildDays = 2;
        }

        // WaterNode.pas:55
        public new const int ClassID = 132276;

        // WaterNode.pas:43


        // WaterNode.pas:58
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132275;
            return Result;
        }

        // WaterNode.pas:27


        // WaterNode.pas:52
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WaterNodeRStr);
            return Result;
        }

    }

    public static partial class WaterNode
    {
        // WaterNode.pas:61
        public static void RegisterWaterNode(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.WaterNode.TWaterNode>(), Aow.Units.Engine.EClass.Of<Aow.Units.WaterNode.TWaterNodeResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
