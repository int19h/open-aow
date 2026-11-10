
using Aow.Game.Magic;

namespace Aow.Game.World.Structures;

public partial class ProductionControl
{
    // The city this control produces for, null for a production place.
    public virtual City ProductionCity() => null;

    // The magic sphere of the production place, msNone for a city.
    public virtual MagicSphere MagicSphere() => Magic.MagicSphere.None;
}
