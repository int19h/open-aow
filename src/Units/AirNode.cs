// Translated from AirNode.pas.
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

namespace Aow.Units.AirNode
{
    // { TAirNode (instance size 76, no added fields, VMT 557C0DF4) }
    public partial class TAirNode : Aow.Units.PrdPlace.TProductionPlace
    {
        public TAirNode(TECustomNode owner) : base(owner)
        {}

        // AirNode.pas:37
        public new const int ClassID = 133454;

        // AirNode.pas:40
        public override Aow.Units.AoWE.TMagicSphere GetMagicSphere()
        {
            Aow.Units.AoWE.TMagicSphere Result = default;
            Result = Aow.Units.AoWE.TMagicSphere.msAir;
            return Result;
        }

    }

    // { TAirNodeResource (instance size 120, no added fields, VMT 557C10A4) }
    public partial class TAirNodeResource : Aow.Units.PrdPlace.TProductionPlaceResource
    {
        public TAirNodeResource(TECustomNode owner) : base(owner)
        {
            FRazeDefenderBudget = 200;
            FRebuildGold = 50;
            FRebuildDays = 2;
            FTerrainBound = false;
        }

        // AirNode.pas:55
        public new const int ClassID = 133455;

        // AirNode.pas:43


        // AirNode.pas:58
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 133454;
            return Result;
        }

        // AirNode.pas:27


        // AirNode.pas:52
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AirNodeRStr);
            return Result;
        }

    }

    public static partial class AirNode
    {
        // AirNode.pas:61
        public static void RegisterAirNode(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.AirNode.TAirNode>(), Aow.Units.Engine.EClass.Of<Aow.Units.AirNode.TAirNodeResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
