// Translated from AoWE.pas.

namespace Aow.Game.World.Movement;

public enum MoveStatus : byte
{
    Idle = 0,
    Moving = 1,
    Failed = 2,
    Done = 3,
    Finished = 4
}
