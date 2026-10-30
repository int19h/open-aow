// Translated from Pyramid.pas.
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
using Aow.Units.ItemExpS;
using static Aow.Units.ItemExpS.ItemExpS;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Pyramid
{
    // { TPyramid (instance size 64, no added fields, VMT 557C6D20) }
    public partial class TPyramid : Aow.Units.ItemExpS.TItemExplorationSite
    {
        public TPyramid(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // Pyramid.pas:34
        public new const int ClassID = 131982;

        public new partial class Meta : Aow.Units.ItemExpS.TItemExplorationSite.Meta
        {
            // Pyramid.pas:34
            public override int ClassID() => Aow.Units.Pyramid.TPyramid.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Pyramid.TPyramid => (Meta)MetaRegistry.Of(typeof(T));
            // ItemExpS.pas:32
            public new Aow.Units.Pyramid.TPyramid Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Pyramid.TPyramid)base.Create(AOwner);
        }
    }

    // { TPyramidResource (instance size 104, no added fields, VMT 557C6F98) }
    public partial class TPyramidResource : Aow.Units.ItemExpS.TItemExplorationSiteResource
    {
        public TPyramidResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // Pyramid.pas:40
        public new const int ClassID = 131983;

        // Pyramid.pas:43
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131982;
            return Result;
        }

        // Pyramid.pas:37
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PyramidRStr);
            return Result;
        }

        public new partial class Meta : Aow.Units.ItemExpS.TItemExplorationSiteResource.Meta
        {
            // Pyramid.pas:40
            public override int ClassID() => Aow.Units.Pyramid.TPyramidResource.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Pyramid.TPyramidResource => (Meta)MetaRegistry.Of(typeof(T));
            // ExploreS.pas:61
            public new Aow.Units.Pyramid.TPyramidResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Pyramid.TPyramidResource)base.Create(AOwner);
        }
    }

    public static partial class Pyramid
    {
        // Pyramid.pas:46
        public static void RegisterPyramid(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Pyramid.TPyramid>(), Aow.Units.Engine.EClass.Of<Aow.Units.Pyramid.TPyramidResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
