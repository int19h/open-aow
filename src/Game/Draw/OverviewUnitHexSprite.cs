// Translated from OverviewUnit.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Draw;

// Troop
public partial class OverviewUnitHexSprite : UnitHexSprite
{
    public OverviewUnitHexSprite(ECustomNode owner) : base(owner)
    { }

    public Troop Troop;

    // OverviewUnit.pas:31
    public override void Dispose()
    {
        SetU(null);
        base.Dispose();
    }

    // OverviewUnit.pas:52
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        base.Show(pixelXSurface, pixelYSurface, lockedSurface);
    }

    // OverviewUnit.pas:47
    public override void ShowShadow(int X, int Y, in LockedSurface lockedSurface)
    {
        base.ShowShadow(X, Y, lockedSurface);
    }

    // OverviewUnit.pas:37
    public void SetU(Troop U)
    {
        if (Troop != null)
        {
            Troop.Release();
        }

        Troop = U;
        if (Troop != null)
        {
            Troop.AddRef();
            SetUnitGraphicsResourceIndex(Troop.GetUnitGraphicsResourceIndex());
        }
    }

    // intf_P2_03.inc:74

}
