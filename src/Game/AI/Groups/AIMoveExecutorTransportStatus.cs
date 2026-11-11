// Translated from AoWE.pas.

namespace Aow.Game.AI.Groups;

public enum AIMoveExecutorTransportStatus : byte
{
    Idle = 0,
    Boarding = 1,
    Transporting = 2,
    Arriving = 3,
    Guarding = 4,
    Dumping = 5,
    Dumped = 6,
    Done = 7
}
