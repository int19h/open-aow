using Aow.Game.World.Map;

namespace Aow.Game.AI;

// The target of an AI group (Delphi TAIGroup.FTargetXYL): a location, the none value, or the cleared
// marker that TAIGroupTargetSelector.NoTargetsFound writes as -1, all four bytes $FF. The original
// compared the target as one integer, so the cleared marker differs from the none value
// ($00FFFFFF). Marker keeps the stored fourth byte and equality compares it, so every stored value
// behaves as in the original.
public readonly record struct AITargetLocation(MapCoordinate Location, byte Marker)
{
    public static AITargetLocation None { get; } = new(Location: MapCoordinate.None, Marker: 0);

    public static AITargetLocation Cleared { get; } = new(Location: MapCoordinate.None, Marker: 0xFF);

    public static AITargetLocation At(MapCoordinate location) => new(Location: location, Marker: 0);

    // The stored slot (AIGroup id 23): the four bytes of the original integer.
    public static AITargetLocation FromStored(StoredCoordinateImage image) => new(Location: image.Location, Marker: image.UnknownBytes);

    public StoredCoordinateImage ToStored() => StoredCoordinateImage.From(Location, unknownBytes: Marker);
}
