using Aow.Game.World.Movement;

namespace Aow.Game.Magic.Abilities;

internal static class MovementTables
{
    // The overlay index of a move point table starts at -1, no overlay.
    internal const int FirstOverlay = -1;

    internal static MovePointTable[] MovePointTables { get; } = new MovePointTable[256];

}
