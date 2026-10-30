// Translated from Ruin.pas.
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

namespace Aow.Units.Ruin
{
    // { TRuin (instance size 64, no added fields, VMT 557C68FC) }
    public partial class TRuin : Aow.Units.ItemExpS.TItemExplorationSite
    {
        public TRuin(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // Ruin.pas:34
        public new const int ClassID = 131994;

        public new partial class Meta : Aow.Units.ItemExpS.TItemExplorationSite.Meta
        {
            // Ruin.pas:34
            public override int ClassID() => Aow.Units.Ruin.TRuin.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Ruin.TRuin => (Meta)MetaRegistry.Of(typeof(T));
            // ItemExpS.pas:32
            public new Aow.Units.Ruin.TRuin Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Ruin.TRuin)base.Create(AOwner);
        }
    }

    // { TRuinResource (instance size 104, no added fields, VMT 557C6B68) }
    public partial class TRuinResource : Aow.Units.ItemExpS.TItemExplorationSiteResource
    {
        public TRuinResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // Ruin.pas:40
        public new const int ClassID = 131995;

        // Ruin.pas:43
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131994;
            return Result;
        }

        public new partial class Meta : Aow.Units.ItemExpS.TItemExplorationSiteResource.Meta
        {
            // Ruin.pas:40
            public override int ClassID() => Aow.Units.Ruin.TRuinResource.ClassID;

            // Ruin.pas:37
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CastleRuinRStr);
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Ruin.TRuinResource => (Meta)MetaRegistry.Of(typeof(T));
            // ExploreS.pas:61
            public new Aow.Units.Ruin.TRuinResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Ruin.TRuinResource)base.Create(AOwner);
        }
    }

    public static partial class Ruin
    {
        // Ruin.pas:46
        public static void RegisterRuin(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Ruin.TRuin>(), Aow.Units.Engine.EClass.Of<Aow.Units.Ruin.TRuinResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
