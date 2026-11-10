
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

public partial class HeroItems
{
    internal Hero Hero;

    public override GameEngine GetEngine() => Hero != null ? Hero.GetEngine() : base.GetEngine();
    public override void GetLocation(ref sbyte x, ref sbyte y, ref sbyte level) => Hero.GetLocation(ref x, ref y, ref level);
}
