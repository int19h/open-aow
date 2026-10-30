// Translated from OverviewUnit.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.OverviewUnit
{
    // { TOverviewUnitHS (instance size 32, VMT 0043765C): $1C FUnit. }
    public partial class TOverviewUnitHS : Aow.Units.AoWE.TUnitHS
    {
        public TOverviewUnitHS(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        public Aow.Units.AoWE.TAbstractUnit FUnit;
        // OverviewUnit.pas:31
        // { $1C }
        public override void Destroy()
        {
            SetU(null);
            base.Destroy();
        }

        // OverviewUnit.pas:52
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            base.Show(XhpSurf, YhpSurf, SurfaceDesc);
        }

        // OverviewUnit.pas:47
        public override void ShowShadow(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            base.ShowShadow(X, Y, SurfaceDesc);
        }

        // OverviewUnit.pas:37
        public void SetU(Aow.Units.AoWE.TAbstractUnit U)
        {
            if (FUnit != null)
            {
                FUnit.Release();
            }

            FUnit = U;
            if (FUnit != null)
            {
                FUnit.AddRef();
                var call__1 = FUnit.GetUnitGFXResourceIndex();
                SetUnitGFXResourceIndex(call__1);
            }
        }

        // intf_P2_03.inc:74

    }

    public static partial class OverviewUnit
    {
        // OverviewUnit.pas:57
        public static void PlaceOverviewUnit(Aow.Units.AoWTC.TAoWCombatMap Map, Aow.Units.AoWE.TAbstractUnit U, int Position, int Index)
        {
            Aow.Units.OverviewUnit.TOverviewUnitHS HS = default;
            sbyte Overlay = default;
            Aow.Units.AoWTC.TTacticalCombatUnitHS Occupant = default;
            int X0 = default;
            int Y0 = default;
            int X = default;
            int Y = default;
            int I = default;
            Aow.Units.HSEngine.TXYL P = default;
            HS = new Aow.Units.OverviewUnit.TOverviewUnitHS(null);
            HS.SetU(U);
            var call__2 = Map.FUnitPositions.GetUnitPosition(Position, Index);
            P = call__2;
            X0 = P.X;
            Y0 = P.Y;
            I = 0;
            do
            {
                Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X0, Y0, I, ref X, ref Y);
                var call__3 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                var call__4 = call__3.Field[X, Y];
                Overlay = unchecked((sbyte)call__4.OverlayType);
                var call__5 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                var call__6 = call__5.Field[X, Y];
                var call__7 = call__6.FindOwnedChild(2228484);
                Occupant = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__7;
                I += 1;
            }
            while (!((Overlay != 8) && (Occupant == null)));
            if (!Map.PlaceHX(HS, X, Y, 0))
            {
                HS.Free();
            }
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
