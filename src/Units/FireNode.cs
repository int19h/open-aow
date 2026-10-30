// Translated from FireNode.pas.
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

namespace Aow.Units.FireNode
{
    // { TFireNode (instance size 76, VMT 557BF624) }
    public partial class TFireNode : Aow.Units.PrdPlace.TProductionPlace
    {
        public TFireNode(TECustomNode owner) : base(owner)
        {}

        // FireNode.pas:36
        public new const int ClassID = 132062;

        // FireNode.pas:39
        public override Aow.Units.AoWE.TMagicSphere GetMagicSphere()
        {
            Aow.Units.AoWE.TMagicSphere Result = default;
            Result = Aow.Units.AoWE.TMagicSphere.msFire;
            return Result;
        }

    }

    // { TFireNodeResource (instance size 120, VMT 557BF8D4) }
    public partial class TFireNodeResource : Aow.Units.PrdPlace.TProductionPlaceResource
    {
        public TFireNodeResource(TECustomNode owner) : base(owner)
        {
            FRazeDefenderBudget = 200;
            FRebuildGold = 50;
            FRebuildDays = 3;
        }

        // FireNode.pas:53
        public new const int ClassID = 132063;

        // FireNode.pas:42


        // FireNode.pas:56
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132062;
            return Result;
        }

        // FireNode.pas:26


        // FireNode.pas:50
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FireNodeRStr);
            return Result;
        }

    }

    public static partial class FireNode
    {
        // FireNode.pas:59
        public static void RegisterFireNode(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.FireNode.TFireNode>(), Aow.Units.Engine.EClass.Of<Aow.Units.FireNode.TFireNodeResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
