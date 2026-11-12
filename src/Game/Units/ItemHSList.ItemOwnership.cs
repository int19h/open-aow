
using Aow.Game.Draw;

namespace Aow.Game.Units;

public partial class ItemHSList
{
    internal ItemHexSprite Sprite;

    public override GameEngine GetEngine() => Sprite != null ? Sprite.GetEngine() : base.GetEngine();

    public override void GetLocation(ref sbyte x, ref sbyte y, ref sbyte level)
    {
        x = Sprite.GetHexX();
        y = Sprite.GetHexY();
        level = Sprite.GetHexLevel();
    }
}
