using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

/// <summary>
/// An army, a unit list or an AI group toward its troops: the hooks of the original's TUnitList,
/// which an army and an AI group override. Code that fills or locates any of these holders calls
/// them through this interface.
/// </summary>
internal interface IUnitHolder
{
    UnitCollection Units { get; }

    bool CanAddUnit(Troop troop);

    bool AddUnit(Troop troop);

    void GetLocation(out sbyte hexX, out sbyte hexY, out sbyte hexLevel);

    void Changed();
}
