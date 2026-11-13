using Aow.Game.World.Map;

namespace Aow.Game.Combat;

// A unit start position of a combat map (TUnitPositionControl): the location and the direction the
// unit faces there (the Delphi flag byte, UnitPositionHexSprite.Direction).
public readonly record struct UnitPosition(MapCoordinate Location, byte Facing)
{
    public static UnitPosition None { get; } = new(Location: MapCoordinate.None, Facing: 0);
}
