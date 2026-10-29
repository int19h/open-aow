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
        // RandomNode.pas:35
        public new const int ClassID = 133154;

        public new partial class Meta : Aow.Units.PrdPlace.TProductionPlace.Meta
        {
            // RandomNode.pas:35
            public override int ClassID() => Aow.Units.RandomNode.TRandomNode.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.RandomNode.TRandomNode => (Meta)MetaRegistry.Of(typeof(T));
            // PrdPlace.pas:51
            public new Aow.Units.RandomNode.TRandomNode Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.RandomNode.TRandomNode)base.Create(AOwner);
        }
    }

    // { TRandomNodeResource (instance size 120, VMT 5580B68C) }
    public partial class TRandomNodeResource : Aow.Units.PrdPlace.TProductionPlaceResource
    {
        // RandomNode.pas:46
        public new const int ClassID = 133155;

        // RandomNode.pas:38
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
        }

        // RandomNode.pas:49
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 133154;
            return Result;
        }

        // RandomNode.pas:25
        public static new Aow.Units.RandomNode.TRandomNodeResource Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.RandomNode.TRandomNodeResource o = new Aow.Units.RandomNode.TRandomNodeResource();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.PrdPlace.TProductionPlaceResource.Meta
        {
            // RandomNode.pas:46
            public override int ClassID() => Aow.Units.RandomNode.TRandomNodeResource.ClassID;

            // RandomNode.pas:43
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RandomNodeRStr);
                return Result;
            }

            // RandomNode.pas:25
            public new Aow.Units.RandomNode.TRandomNodeResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.RandomNode.TRandomNodeResource)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.RandomNode.TRandomNodeResource => (Meta)MetaRegistry.Of(typeof(T));
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
