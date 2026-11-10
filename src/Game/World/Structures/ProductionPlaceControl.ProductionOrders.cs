
using Aow.Game.Magic;

namespace Aow.Game.World.Structures;

public partial class ProductionPlaceControl
{
    public override MagicSphere MagicSphere() => FProductionPlace.GetMagicSphere();
}
