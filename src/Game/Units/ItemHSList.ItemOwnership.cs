
using Aow.Game.Draw;
using Aow.Game.World.Map;

namespace Aow.Game.Units;

public partial class ItemHSList
{
    internal ItemHexSprite Sprite { get; set; }

    public override GameEngine GetEngine() => Sprite != null ? Sprite.GetEngine() : base.GetEngine();

    public override MapCoordinate GetLocation() => new(
        Sprite.GetHexX(),
        Sprite.GetHexY(),
        Sprite.GetHexLevel());
}
