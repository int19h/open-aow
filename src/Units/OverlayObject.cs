// Translated from OverlayObject.pas.
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.OverlayObject
{
    // { TOverlayObject (instance size 28, VMT 557D3C78) }
    public partial class TOverlayObject : Aow.Units.ILTer.TFixedILTerrainMO
    {
        public new partial class Meta : Aow.Units.ILTer.TFixedILTerrainMO.Meta
        {
            // OverlayObject.pas:36
            public override int ClassID()
            {
                int Result = default;
                Result = 131228;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.OverlayObject.TOverlayObject => (Meta)MetaRegistry.Of(typeof(T));
            // ILTer.pas:20
            public new Aow.Units.OverlayObject.TOverlayObject Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.OverlayObject.TOverlayObject o = (Aow.Units.OverlayObject.TOverlayObject)NewInstance();
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

    // { TOverlayObjectResource (instance size 72, VMT 557D3E30): $44 FOverlayType. }
    public partial class TOverlayObjectResource : Aow.Units.ILTer.TFixedILTerrainMOR
    {
        public int FOverlayType;
        // OverlayObject.pas:48
        // { $44 }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FOverlayType, 40);
        }

        // OverlayObject.pas:45
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131228;
            return Result;
        }

        public new partial class Meta : Aow.Units.ILTer.TFixedILTerrainMOR.Meta
        {
            // OverlayObject.pas:42
            public override int ClassID()
            {
                int Result = default;
                Result = 131229;
                return Result;
            }

            // OverlayObject.pas:39
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                var call__1 = Aow.Units.AoWE.AoWE.OverlayObjectRStr;
                var call__2 = Aow.Units.AoWE.AoWE.TranslateRStr(call__1);
                Result = call__2;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.OverlayObject.TOverlayObjectResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.OverlayObject.TOverlayObjectResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.OverlayObject.TOverlayObjectResource o = (Aow.Units.OverlayObject.TOverlayObjectResource)NewInstance();
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

    public static partial class OverlayObject
    {
        // OverlayObject.pas:54
        public static void RegisterOverlayObject(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.OverlayObject.TOverlayObject>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.OverlayObject.TOverlayObjectResource>()]);
        }

        // OverlayObject.pas:59
        public static void UnRegisterOverlayObject(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.UnregisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.OverlayObject.TOverlayObject>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.OverlayObject.TOverlayObjectResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
