// Translated from AoWE.pas.

namespace Aow.Game.AI.Groups;

public enum AIMoveExecutorUnitStatus : byte
{
    Idle = 0,
    Ready = 1,
    Queued = 2,
    Moving = 3,
    Done = 4,
    Lost = 5,
    Failed = 6
}
