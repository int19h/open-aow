
using Aow.Game.Magic;

namespace Aow.Game.World.Structures;

public sealed partial class ProductionPlaceControl
{
    public override MagicSphere MagicSphere() => ProductionPlace.GetMagicSphere();
}
