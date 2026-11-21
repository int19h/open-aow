// Translated from AoWE.pas.

namespace Aow.Game.Combat;

[Flags]
public enum CombatFlowControlModes : byte
{
    None = 0,
    SetAutoSwitch = 1 << 0,
    LockAIExecutor = 1 << 1,
    UnlockAIExecutor = 1 << 2,
    ActivateTacticalBattle = 1 << 3,
    EnableTurnTimer = 1 << 4,
    DisableTurnTimer = 1 << 5,
}
