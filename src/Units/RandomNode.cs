// Translated from RandomNode.pas.
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

namespace Aow.Units.RandomNode
{
    // { TRandomNode (instance size 76, VMT 5580B3D8) }
    public partial class TRandomNode : Aow.Units.PrdPlace.TProductionPlace
    {
        public TRandomNode(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // RandomNode.pas:35
        public new const int ClassID = 133154;

    }

    // { TRandomNodeResource (instance size 120, VMT 5580B68C) }
    public partial class TRandomNodeResource : Aow.Units.PrdPlace.TProductionPlaceResource
    {
        public TRandomNodeResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
        }

        // RandomNode.pas:46
        public new const int ClassID = 133155;

        // RandomNode.pas:38


        // RandomNode.pas:49
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 133154;
            return Result;
        }

        // RandomNode.pas:25


        // RandomNode.pas:43
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RandomNodeRStr);
            return Result;
        }

    }

    public static partial class RandomNode
    {
        // RandomNode.pas:52
        public static void RegisterRandomNode(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.RandomNode.TRandomNode>(), Aow.Units.Engine.EClass.Of<Aow.Units.RandomNode.TRandomNodeResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
