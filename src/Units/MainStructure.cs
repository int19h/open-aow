// Translated from MainStructure.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
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
using Aow.Units.CombatStructure;
using static Aow.Units.CombatStructure.CombatStructure;

namespace Aow.Units.MainStructure
{
    // { TMainStructure (instance size 40, VMT 00435310) }
    public partial class TMainStructure : Aow.Units.CombatStructure.TCombatStructure
    {
        // MainStructure.pas:40
        public override int GetTerrainTypeImage(int TerrainType)
        {
            int Result = default;
            Result = TerrainType;
            return Result;
        }

        public new partial class Meta : Aow.Units.CombatStructure.TCombatStructure.Meta
        {
            // MainStructure.pas:37
            public override int ClassID()
            {
                int Result = default;
                Result = 2229532;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.MainStructure.TMainStructure => (Meta)MetaRegistry.Of(typeof(T));
            // CombatStructure.pas:23
            public new Aow.Units.MainStructure.TMainStructure Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.MainStructure.TMainStructure o = (Aow.Units.MainStructure.TMainStructure)NewInstance();
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

    // { TMainStructureResource (instance size 72, VMT 004354CC) }
    public partial class TMainStructureResource : Aow.Units.CombatStructure.TCombatStructureResource
    {
        // MainStructure.pas:49
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229532;
            return Result;
        }

        public new partial class Meta : Aow.Units.CombatStructure.TCombatStructureResource.Meta
        {
            // MainStructure.pas:43
            public override int ClassID()
            {
                int Result = default;
                Result = 2229533;
                return Result;
            }

            // MainStructure.pas:46
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Combat Main Structure";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.MainStructure.TMainStructureResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.MainStructure.TMainStructureResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.MainStructure.TMainStructureResource o = (Aow.Units.MainStructure.TMainStructureResource)NewInstance();
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

    public static partial class MainStructure
    {
        // MainStructure.pas:52
        public static void RegisterMainStructure(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.MainStructure.TMainStructureResource>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.MainStructure.TMainStructure>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
