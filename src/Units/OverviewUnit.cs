// Translated from OverviewUnit.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

namespace Aow.Game.Draw;

// FUnit
public partial class TOverviewUnitHS : TUnitHS
{
    public TOverviewUnitHS(TECustomNode owner) : base(owner)
    { }

    public TAbstractUnit FUnit;

    // OverviewUnit.pas:31
    public override void Dispose()
    {
        SetU(null);
        base.Dispose();
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
