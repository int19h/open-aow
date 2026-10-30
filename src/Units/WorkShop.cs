// Translated from WorkShop.pas.
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

namespace Aow.Units.WorkShop
{
    // { TWorkshop (instance size 40, VMT 004334E0) }
    public partial class TWorkshop : Aow.Units.CombatStructure.TCombatStructure
    {
        public TWorkshop(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // WorkShop.pas:36
        public new const int ClassID = 2229512;

        public new partial class Meta : Aow.Units.CombatStructure.TCombatStructure.Meta
        {
            // WorkShop.pas:36
            public override int ClassID() => Aow.Units.WorkShop.TWorkshop.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.WorkShop.TWorkshop => (Meta)MetaRegistry.Of(typeof(T));
            // CombatStructure.pas:23
            public new Aow.Units.WorkShop.TWorkshop Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.WorkShop.TWorkshop)base.Create(AOwner);
        }
    }

    // { TWorkshopResource (instance size 72, VMT 0043368C) }
    public partial class TWorkshopResource : Aow.Units.CombatStructure.TCombatStructureResource
    {
        public TWorkshopResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // WorkShop.pas:39
        public new const int ClassID = 2229513;

        // WorkShop.pas:45
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229512;
            return Result;
        }

        public new partial class Meta : Aow.Units.CombatStructure.TCombatStructureResource.Meta
        {
            // WorkShop.pas:39
            public override int ClassID() => Aow.Units.WorkShop.TWorkshopResource.ClassID;

            // WorkShop.pas:42
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Combat Workshop";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.WorkShop.TWorkshopResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.WorkShop.TWorkshopResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.WorkShop.TWorkshopResource)base.Create(AOwner);
        }
    }

    public static partial class WorkShop
    {
        // WorkShop.pas:48
        public static void RegisterWorkshop(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.WorkShop.TWorkshopResource>(), Aow.Units.Engine.EClass.Of<Aow.Units.WorkShop.TWorkshop>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
