
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

public partial class HeroInventory
{
    internal Hero Hero { get; set; }

    public override GameEngine GetEngine() => Hero != null ? Hero.GetEngine() : base.GetEngine();
    public override void GetLocation(out sbyte x, out sbyte y, out sbyte level) => Hero.GetLocation(out x, out y, out level);
}
