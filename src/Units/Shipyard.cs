// Translated from Shipyard.pas.
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

namespace Aow.Units.Shipyard
{
    // { TShipyard (instance size 76, no added fields, VMT 557C715C) }
    public partial class TShipyard : Aow.Units.PrdPlace.TProductionPlace
    {
        public TShipyard(TECustomNode owner) : base(owner)
        {}

        // Shipyard.pas:49
        public new const int ClassID = 132286;

        // Shipyard.pas:55
        public override void UpdateAITarget(in Aow.Units.AoWE.TAITargetInfo Info, ref Aow.Units.AoWE.TAITargetData Data)
        {
            base.UpdateAITarget(Info, ref Data);
        }

        // Shipyard.pas:52
        public override int GetBaseIncome()
        {
            int Result = default;
            Result = 0;
            return Result;
        }

    }

    // { TShipyardResource (instance size 120, no added fields, VMT 557C740C) }
    public partial class TShipyardResource : Aow.Units.PrdPlace.TProductionPlaceResource
    {
        public TShipyardResource(TECustomNode owner) : base(owner)
        {
            FTerrainBound = false;
            FRazeDefenderBudget = 150;
            FRebuildGold = 25;
            FRebuildDays = 2;
        }

        // Shipyard.pas:72
        public new const int ClassID = 132287;

        // Shipyard.pas:60


        // Shipyard.pas:75
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132286;
            return Result;
        }

        // Shipyard.pas:29


        // Shipyard.pas:69
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ShipyardRStr);
            return Result;
        }

    }

    // { TShipyardConstructionControl (instance size 8, no added fields, VMT 557C750C) }
    public partial class TShipyardConstructionControl : Aow.Units.AoWE.TStructureConstructionControl
    {
        public TShipyardConstructionControl(TECustomNode owner) : base(owner)
        { }

        // Shipyard.pas:93
        public override void GetInfo(Aow.Units.AoWE.TAbstractUnit AUnit, ref Aow.Rtl.AnsiString Text, ref int Gold, ref int Days)
        {
            var call__1 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ShipyardRStr);
            var call__2 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.BuildXRStr);
            var call__3 = Aow.Rtl.Rtl.Format(call__2, [call__1]);
            Text = call__3;
            Gold = 50;
            Days = 2;
        }

        // Shipyard.pas:100
        public override bool ValidateLocation(Aow.Units.AoWE.TAbstractUnit AUnit, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Result = base.ValidateLocation(AUnit, ref Reason);
            if (Result)
            {
                Result = false;
                AUnit.GetLocation(ref X, ref Y, ref L);
                var call__4 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(L);
                MapLevel = call__4;
                Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, X, Y, 0);
                while ((Walk.HN < 7) && !Result)
                {
                    if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < MapLevel.Width)) && (Walk.Y < MapLevel.Height))
                    {
                        var call__5 = MapLevel.GetField(Walk.X, Walk.Y);
                        Result = TSet<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)call__5.TerrainType);
                    }

                    Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
                }

                if (!Result)
                {
                    Reason = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CanOnlyBuildNextToWaterRStr);
                }
            }

            return Result;
        }

        // Shipyard.pas:81
        public override Aow.Units.AoWE.TStructureResource StructureResource(Aow.Units.AoWE.TAbstractUnit AUnit)
        {
            Aow.Units.AoWE.TStructureResource Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            AUnit.GetLocation(ref X, ref Y, ref L);
            if (L == 0)
            {
                var call__6 = Aow.Units.AoWE.AoWE.AoWHSSet.MapObjectResourceList.FindResource(60);
                Result = (Aow.Units.AoWE.TStructureResource)call__6;
            }
            else
            {
                var call__7 = Aow.Units.AoWE.AoWE.AoWHSSet.MapObjectResourceList.FindResource(107);
                Result = (Aow.Units.AoWE.TStructureResource)call__7;
            }

            return Result;
        }

        // Shipyard.pas:78
        public override int ID()
        {
            int Result = default;
            Result = 4;
            return Result;
        }

    }

    public static partial class Shipyard
    {
        // Shipyard.pas:125
        // { conflict: function StructureResource: TStructureResource; override; }
        public static void RegisterShipyard(Aow.Units.Engine.TEngine AEngine)
        {
            // { conflict: procedure RegisterShipyard; }
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Shipyard.TShipyard>(), Aow.Units.Engine.EClass.Of<Aow.Units.Shipyard.TShipyardResource>()]);
            var call__8 = new Aow.Units.Shipyard.TShipyardConstructionControl(null);
            var call__9 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(125);
            ((Aow.Units.AoWE.TConstructAbility)call__9).RegisterConstructionControl(call__8);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
