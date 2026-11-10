// Translated from AoWE.pas.

namespace Aow.Game.Ai.Groups;

public enum AIMoveExecuterUnitStatus : byte
{
    Idle = 0,
    Ready = 1,
    Queued = 2,
    Moving = 3,
    Done = 4,
    Lost = 5,
    Failed = 6
}
