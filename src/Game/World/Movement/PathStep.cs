using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

// A step of a path: the location it enters and the move points spent entering it (the Delphi flag
// byte of a TXYL path item). Equality covers both, as the Delphi four-byte comparisons did.
public readonly record struct PathStep(MapCoordinate Location, sbyte MoveCost)
{
    // The default item of a path list: the none location with no cost.
    public static PathStep None { get; } = new(Location: MapCoordinate.None, MoveCost: 0);
}
