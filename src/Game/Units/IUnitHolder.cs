using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

namespace Aow.Game.Units;

/// <summary>
/// An army, a unit list, a unit user list or an AI group toward its troops: the hooks of the
/// original's TUnitList, which an army, a unit user list and an AI group carry on their own.
/// Code that fills or locates any of these holders calls them through this interface.
/// </summary>
internal interface IUnitHolder
{
    UnitCollection Units { get; }

    bool CanAddUnit(Troop troop);

    bool AddUnit(Troop troop);

    MapCoordinate GetLocation();

    void Changed();
}
