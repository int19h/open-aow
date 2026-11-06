// Translated from OverviewUnit.pas.
using Aow.Rtl;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTCUnit;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngineUnit;
using Aow.Units.AoWE;

namespace Aow.Units.OverviewUnit;

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
            SetUnitGFXResourceIndex(FUnit.GetUnitGFXResourceIndex());
        }
    }

    // intf_P2_03.inc:74

}

public static partial class OverviewUnitUnit
{
    // OverviewUnit.pas:57
    public static void PlaceOverviewUnit(TAoWCombatMap Map, TAbstractUnit U, int Position, int Index)
    {
        TOverviewUnitHS HS;
        sbyte Overlay;
        TTacticalCombatUnitHS Occupant;
        int X0;
        int Y0;
        int X = default;
        int Y = default;
        int I;
        TXYL P;
        HS = new TOverviewUnitHS(null);
        HS.SetU(U);
        P = Map.FUnitPositions.GetUnitPosition(Position, Index);
        X0 = P.X;
        Y0 = P.Y;
        I = 0;
        do
        {
            CenterHNtoHX(X0, Y0, I, ref X, ref Y);
            Overlay = AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].OverlayType;
            Occupant = (TTacticalCombatUnitHS)AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2228484);
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
