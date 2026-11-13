// Translated from HSEngine.pas.
namespace Aow.Game.World.Map;

// A map location: hexagon column X, row Y and map level L, the item of TXYLList that PackXYL makes.
// The Delphi TXYL carried a fourth flag byte that each user borrowed (issue #191). Path steps keep
// it as PathStep.MoveCost, unit positions as UnitPosition.Facing, the AI target as
// AITargetLocation.Marker, and the stored slots as the owner's UnknownBytes through
// StoredCoordinateImage.
public readonly record struct MapCoordinate(sbyte X, sbyte Y, sbyte MapLevel)
{
    // The none value: X, Y and level -1, the Delphi $00FFFFFF.
    public static MapCoordinate None { get; } = new(X: -1, Y: -1, MapLevel: -1);
}
