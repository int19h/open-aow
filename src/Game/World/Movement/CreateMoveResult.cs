// Translated from AoWE.pas.

namespace Aow.Game.World.Movement;

public enum CreateMoveResult : byte
{
    OutOfMovePoints = 0,
    CannotMoveCloser = 1,
    Done = 2,
    Failed = 3
}
