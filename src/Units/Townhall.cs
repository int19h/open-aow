// Translated from Townhall.pas.
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
using Aow.Units.IsoCombatHex;
using static Aow.Units.IsoCombatHex.IsoCombatHex;

namespace Aow.Units.Townhall
{
    // { TTownhall (instance size 40, VMT 004341F8) }
    public partial class TTownhall : Aow.Units.CombatStructure.TCombatStructure
    {
        // Townhall.pas:36
        public new const int ClassID = 2229520;

        public new partial class Meta : Aow.Units.CombatStructure.TCombatStructure.Meta
        {
            // Townhall.pas:36
            public override int ClassID() => Aow.Units.Townhall.TTownhall.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Townhall.TTownhall => (Meta)MetaRegistry.Of(typeof(T));
            // CombatStructure.pas:23
            public new Aow.Units.Townhall.TTownhall Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Townhall.TTownhall)base.Create(AOwner);
        }
    }

    // { TTownhallResource (instance size 72, VMT 004343A4) }
    public partial class TTownhallResource : Aow.Units.CombatStructure.TCombatStructureResource
    {
        // Townhall.pas:39
        public new const int ClassID = 2229521;

        // Townhall.pas:45
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229520;
            return Result;
        }

        public new partial class Meta : Aow.Units.CombatStructure.TCombatStructureResource.Meta
        {
            // Townhall.pas:39
            public override int ClassID() => Aow.Units.Townhall.TTownhallResource.ClassID;

            // Townhall.pas:42
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Combat Townhall";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Townhall.TTownhallResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.Townhall.TTownhallResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Townhall.TTownhallResource)base.Create(AOwner);
        }
    }

    public static partial class Townhall
    {
        // Townhall.pas:48
        public static void RegisterTownhall(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Townhall.TTownhallResource>(), Aow.Units.Engine.EClass.Of<Aow.Units.Townhall.TTownhall>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
