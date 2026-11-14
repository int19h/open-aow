using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

/// <summary>
/// An army or a unit list toward its troops: the hooks of the original's TUnitList, which an army
/// overrides. Code that fills or locates either kind of holder calls them through this interface.
/// </summary>
internal interface IUnitHolder
{
    UnitCollection Units { get; }

    bool CanAddUnit(Troop troop);

    bool AddUnit(Troop troop);

    void GetLocation(out sbyte hexX, out sbyte hexY, out sbyte hexLevel);

    void Changed();
}
