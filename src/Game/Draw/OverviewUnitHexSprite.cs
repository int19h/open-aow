// Translated from OverviewUnit.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Draw;

// FUnit
public partial class OverviewUnitHexSprite : UnitHexSprite
{
    public OverviewUnitHexSprite(ECustomNode owner) : base(owner)
    { }

    public Troop FUnit;

    // OverviewUnit.pas:31
    public override void Dispose()
    {
        SetU(null);
        base.Dispose();
    }

    // OverviewUnit.pas:52
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        base.Show(XhpSurf, YhpSurf, SurfaceDesc);
    }

    // OverviewUnit.pas:47
    public override void ShowShadow(int X, int Y, in LockedSurface SurfaceDesc)
    {
        base.ShowShadow(X, Y, SurfaceDesc);
    }

    // OverviewUnit.pas:37
    public void SetU(Troop U)
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
