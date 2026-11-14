using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

/// <summary>
/// An item list, a hero's inventory, the items a hero wears or an item library, toward its items.
/// Code that locates the holder of an item calls these hooks through the interface.
/// </summary>
internal interface IItemHolder
{
    ItemCollection Items { get; }

    void GetLocation(out sbyte hexX, out sbyte hexY, out sbyte hexLevel);

    void Changed();
}
