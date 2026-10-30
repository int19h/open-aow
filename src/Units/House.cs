// Translated from House.pas.
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.City;
using static Aow.Units.City.City;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.House
{
    // { THouse (instance size 48, VMT 00434584) }
    public partial class THouse : Aow.Units.CombatStructure.TCombatStructure
    {
        public THouse(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        public new const int ClassID = 2229522;

        public int FRace;
        // { $28 }
        public int FFlagID;
        // House.pas:45
        public override void Activate()
        {
            Aow.Units.AoWE.TPlayerStructure Structure = default;
            base.Activate();
            if (!Aow.Units.AoWTC.AoWTC.AoWCombatMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                var call__1 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(Aow.Units.AoWTC.AoWTC.CombatLLoc);
                var call__2 = call__1.GetField(Aow.Units.AoWTC.AoWTC.CombatXLoc, Aow.Units.AoWTC.AoWTC.CombatYLoc);
                var call__3 = call__2.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TPlayerStructure));
                Structure = (Aow.Units.AoWE.TPlayerStructure)call__3;
                if (Structure != null)
                {
                    if (Structure is Aow.Units.City.TCity)
                    {
                        FRace = ((Aow.Units.City.TCity)Structure).FRace;
                    }
                    else
                    {
                        FRace = 0;
                    }

                    if (Structure.FPlayer == -1)
                    {
                        FFlagID = -1;
                    }
                    else
                    {
                        var call__4 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Structure.FPlayer);
                        FFlagID = call__4.FPlayerRace;
                    }

                    if (FFlagID == -1)
                    {
                        FFlagID = 32;
                    }
                }
            }
        }

        public new partial class Meta : Aow.Units.CombatStructure.TCombatStructure.Meta
        {
            // House.pas:42
            // { $2C }
            public override int ClassID() => Aow.Units.House.THouse.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.House.THouse => (Meta)MetaRegistry.Of(typeof(T));
            // CombatStructure.pas:23
            public new Aow.Units.House.THouse Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.House.THouse)base.Create(AOwner);
        }
    }

    // { THouseResource (instance size 72, VMT 00434728) }
    public partial class THouseResource : Aow.Units.CombatStructure.TCombatStructureResource
    {
        public THouseResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // House.pas:145
        public new const int ClassID = 2229523;

        // House.pas:151
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229522;
            return Result;
        }

        // House.pas:148
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"Combat House";
            return Result;
        }

        public new partial class Meta : Aow.Units.CombatStructure.TCombatStructureResource.Meta
        {
            // House.pas:145
            public override int ClassID() => Aow.Units.House.THouseResource.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.House.THouseResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.House.THouseResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.House.THouseResource)base.Create(AOwner);
        }
    }

    public static partial class House
    {
        // House.pas:154
        public static void RegisterHouse(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.House.THouseResource>(), Aow.Units.Engine.EClass.Of<Aow.Units.House.THouse>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
