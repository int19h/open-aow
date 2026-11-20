#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Draw;

// UnitHexSprite
public sealed class UnitShadowHexSprite(CustomNode owner) : HexSprite(owner)
{
    public UnitHexSprite UnitHexSprite { get; init; }

    // UnitHS.PAS:153
    public override int GetShowPriority() => (UnitHexSprite.GetShowPriority() & 0b0011_1111_1111_1111_1111_1111_1111) + 1_677_721_600;

    // UnitHS.PAS:157
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface) => UnitHexSprite.ShowShadow(pixelXSurface, pixelYSurface, lockedSurface);
}
