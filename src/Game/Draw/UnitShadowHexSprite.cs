// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Draw;

// FUnitHS
public partial class UnitShadowHexSprite : HexSprite
{
    public UnitShadowHexSprite(ECustomNode owner) : base(owner)
    { }

    public UnitHexSprite FUnitHS;

    // UnitHS.PAS:153
    public override int GetShowPriority() => (FUnitHS.GetShowPriority() & 67108863) + 1677721600;

    // UnitHS.PAS:157
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        FUnitHS.ShowShadow(XhpSurf, YhpSurf, SurfaceDesc);
    }
}
