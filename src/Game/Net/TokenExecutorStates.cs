// Translated from NetworkE.pas.

namespace Aow.Game.Net;

[Flags]
public enum TokenExecutorStates : byte
{
    None = 0,
    Prefetching = 1 << 0,
    Executing = 1 << 1,
    TacticalBattleActivated = 1 << 2,
}
