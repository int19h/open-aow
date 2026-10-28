// Translated from EarthNode.pas.
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

namespace Aow.Units.EarthNode
{
    // { TEarthNode (instance size 76, VMT 557BFAE0) }
    public partial class TEarthNode : Aow.Units.PrdPlace.TProductionPlace
    {
        // EarthNode.pas:39
        public override Aow.Units.AoWE.TMagicSphere GetMagicSphere()
        {
            Aow.Units.AoWE.TMagicSphere Result = default;
            Result = Aow.Units.AoWE.TMagicSphere.msEarth;
            return Result;
        }

        public new partial class Meta : Aow.Units.PrdPlace.TProductionPlace.Meta
        {
            // EarthNode.pas:36
            public override int ClassID()
            {
                int Result = default;
                Result = 132302;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.EarthNode.TEarthNode => (Meta)MetaRegistry.Of(typeof(T));
            // PrdPlace.pas:51
            public new Aow.Units.EarthNode.TEarthNode Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.EarthNode.TEarthNode o = (Aow.Units.EarthNode.TEarthNode)NewInstance();
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
        }
    }

    // { TEarthNodeResource (instance size 120, VMT 557BFD94) }
    public partial class TEarthNodeResource : Aow.Units.PrdPlace.TProductionPlaceResource
    {
        // EarthNode.pas:42
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FRazeDefenderBudget = 200;
            FRebuildGold = 35;
            FRebuildDays = 2;
        }

        // EarthNode.pas:56
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132302;
            return Result;
        }

        // EarthNode.pas:26
        public static new Aow.Units.EarthNode.TEarthNodeResource Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.EarthNode.TEarthNodeResource o = new Aow.Units.EarthNode.TEarthNodeResource();
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
            // EarthNode.pas:53
            public override int ClassID()
            {
                int Result = default;
                Result = 132303;
                return Result;
            }

            // EarthNode.pas:50
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EarthNodeRStr);
                return Result;
            }

            // EarthNode.pas:26
            public new Aow.Units.EarthNode.TEarthNodeResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.EarthNode.TEarthNodeResource o = (Aow.Units.EarthNode.TEarthNodeResource)NewInstance();
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

            public static new Meta Of<T>()
                where T : Aow.Units.EarthNode.TEarthNodeResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public static partial class EarthNode
    {
        // EarthNode.pas:59
        public static void RegisterEarthNode(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.EarthNode.TEarthNode>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.EarthNode.TEarthNodeResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
