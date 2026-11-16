using Aow.Game.Combat;

namespace Aow.Game.Magic.Abilities;

public partial class Troop
{
    // The predictor stores the wall it created. Any other troop throws InvalidCastException,
    // as the cast to a wall combatant did. A null troop fails at the call.
    public virtual void SetWallType(WallType value) => throw new InvalidCastException();
}
