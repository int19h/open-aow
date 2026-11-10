// Translated from OverviewUnit.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

namespace Aow.Game.Draw;

// FUnit
public partial class OverviewUnitHS : UnitHS
{
    public OverviewUnitHS(ECustomNode owner) : base(owner)
    { }

    public AbstractUnit FUnit;

    // OverviewUnit.pas:31
    public override void Dispose()
    {
        SetU(null);
        base.Dispose();
    }

    // OverviewUnit.pas:52
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        base.Show(XhpSurf, YhpSurf, SurfaceDesc);
    }

    // OverviewUnit.pas:47
    public override void ShowShadow(int X, int Y, in SurfaceDesc SurfaceDesc)
    {
        base.ShowShadow(X, Y, SurfaceDesc);
    }

    // OverviewUnit.pas:37
    public void SetU(AbstractUnit U)
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
