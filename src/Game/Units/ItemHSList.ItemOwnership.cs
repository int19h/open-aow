
using Aow.Game.Draw;

namespace Aow.Game.Units;

public partial class ItemHSList
{
    internal ItemHexSprite Sprite { get; set; }

    public override GameEngine GetEngine() => Sprite != null ? Sprite.GetEngine() : base.GetEngine();

    public override void GetLocation(out sbyte x, out sbyte y, out sbyte level)
    {
        x = Sprite.GetHexX();
        y = Sprite.GetHexY();
        level = Sprite.GetHexLevel();
    }
}
