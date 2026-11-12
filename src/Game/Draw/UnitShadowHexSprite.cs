// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Draw;

// UnitHexSprite
public partial class UnitShadowHexSprite : HexSprite
{
    public UnitShadowHexSprite(ECustomNode owner) : base(owner)
    { }

    public UnitHexSprite UnitHexSprite;

    // UnitHS.PAS:153
    public override int GetShowPriority() => (UnitHexSprite.GetShowPriority() & 67108863) + 1677721600;

    // UnitHS.PAS:157
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        UnitHexSprite.ShowShadow(pixelXSurface, pixelYSurface, lockedSurface);
    }
}
