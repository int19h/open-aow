#nullable disable warnings
// Translated from OverviewUnit.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Draw;

// Troop
public sealed class OverviewUnitHexSprite(CustomNode? owner) : UnitHexSprite(owner)
{
    private Troop? _troop;

    // OverviewUnit.pas:31
    public override void Dispose()
    {
        SetU(null);
        base.Dispose();
    }

    // OverviewUnit.pas:52
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface) => base.Show(pixelXSurface, pixelYSurface, lockedSurface);

    // OverviewUnit.pas:47
    public override void ShowShadow(int X, int Y, in LockedSurface lockedSurface) => base.ShowShadow(X, Y, lockedSurface);

    // OverviewUnit.pas:37
    public void SetU(Troop? U)
    {
        _ = (_troop?.Release());

        _troop = U;
        if (_troop is not null)
        {
            _ = _troop.AddRef();
            SetUnitGraphicsResourceIndex(_troop.UnitGraphicsResourceIndex);
        }
    }
}
