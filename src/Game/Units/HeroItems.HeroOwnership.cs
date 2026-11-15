
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

namespace Aow.Game.Units;

public partial class HeroItems
{
    internal Hero Hero { get; set; }

    public override GameEngine GetEngine() => Hero != null ? Hero.GetEngine() : base.GetEngine();
    public MapCoordinate GetLocation() => Hero.GetLocation();
}
