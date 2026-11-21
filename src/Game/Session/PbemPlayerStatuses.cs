// Translated from AoWE.pas.

namespace Aow.Game.Session;

[Flags]
public enum PbemPlayerStatuses : byte
{
    None = 0,
    NewTurn = 1 << 0,
    Playing = 1 << 1,
    TurnDone = 1 << 2,
}
