// Translated from OverviewUnit.pas.
using Aow.Rtl;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.AoWE;

namespace Aow.Units.OverviewUnit
{
    // FUnit
    public partial class TOverviewUnitHS : TUnitHS
    {
        public TOverviewUnitHS(TECustomNode owner) : base(owner)
        {}

        public TAbstractUnit FUnit;
        // OverviewUnit.pas:31
        public override void Destroy()
        {
            SetU(null);
            base.Destroy();
        }

        // OverviewUnit.pas:52
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            base.Show(XhpSurf, YhpSurf, SurfaceDesc);
        }

        // OverviewUnit.pas:47
        public override void ShowShadow(int X, int Y, in TSurfaceDesc SurfaceDesc)
        {
            base.ShowShadow(X, Y, SurfaceDesc);
        }

        // OverviewUnit.pas:37
        public void SetU(TAbstractUnit U)
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
        public static void PlaceOverviewUnit(TAoWCombatMap Map, TAbstractUnit U, int Position, int Index)
        {
            TOverviewUnitHS HS = default;
            sbyte Overlay = default;
            TTacticalCombatUnitHS Occupant = default;
            int X0 = default;
            int Y0 = default;
            int X = default;
            int Y = default;
            int I = default;
            TXYL P = default;
            HS = new TOverviewUnitHS(null);
            HS.SetU(U);
            var call__2 = Map.FUnitPositions.GetUnitPosition(Position, Index);
            P = call__2;
            X0 = P.X;
            Y0 = P.Y;
            I = 0;
            do
            {
                CenterHNtoHX(X0, Y0, I, ref X, ref Y);
                var call__3 = AoWCombatMap.MapContainer.MapLevels[0];
                var call__4 = call__3.Field[X, Y];
                Overlay = call__4.OverlayType;
                var call__5 = AoWCombatMap.MapContainer.MapLevels[0];
                var call__6 = call__5.Field[X, Y];
                var call__7 = call__6.FindOwnedChild(2228484);
                Occupant = (TTacticalCombatUnitHS)call__7;
                I += 1;
            }
            while (!(Overlay != 8 && Occupant == null));
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
