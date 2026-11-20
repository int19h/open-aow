
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

namespace Aow.Game.Units;

public sealed partial class HeroItems
{
    internal Hero Hero { get; init; }

    public override GameEngine GetEngine() => Hero is not null ? Hero.GetEngine() : base.GetEngine();
    public MapCoordinate GetLocation() => Hero.GetLocation();
}
