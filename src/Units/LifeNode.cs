// Translated from LifeNode.pas.
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

namespace Aow.Units.LifeNode
{
    public partial class TLifeNode : Aow.Units.PrdPlace.TProductionPlace
    {
        public TLifeNode(TECustomNode owner) : base(owner)
        {}

        // LifeNode.pas:37
        public new const int ClassID = 131953;

        // LifeNode.pas:40
        public override Aow.Units.AoWE.TMagicSphere GetMagicSphere()
        {
            Aow.Units.AoWE.TMagicSphere Result = default;
            Result = Aow.Units.AoWE.TMagicSphere.msLife;
            return Result;
        }

    }

    public partial class TLifeNodeResource : Aow.Units.PrdPlace.TProductionPlaceResource
    {
        public TLifeNodeResource(TECustomNode owner) : base(owner)
        {
            FRazeDefenderBudget = 200;
            FTerrainBound = false;
            FRebuildGold = 25;
            FRebuildDays = 2;
        }

        // LifeNode.pas:55
        public new const int ClassID = 131954;

        // LifeNode.pas:43


        // LifeNode.pas:58
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131953;
            return Result;
        }

        // LifeNode.pas:27


        // LifeNode.pas:52
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LifeNodeRStr);
            return Result;
        }

    }

    public static partial class LifeNode
    {
        // LifeNode.pas:61
        public static void RegisterLifeNode(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.LifeNode.TLifeNode>(), Aow.Units.Engine.EClass.Of<Aow.Units.LifeNode.TLifeNodeResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
