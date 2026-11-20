
using Aow.Game.Draw;
using Aow.Game.World.Map;

namespace Aow.Game.Units;

public sealed partial class ItemHSList
{
    internal ItemHexSprite Sprite { get; init; } = null!;

    public override GameEngine GetEngine() => Sprite is not null ? Sprite.GetEngine() : base.GetEngine();

    public override MapCoordinate GetLocation() => new(
        Sprite.HexX,
        Sprite.HexY,
        Sprite.HexLevel);
}
