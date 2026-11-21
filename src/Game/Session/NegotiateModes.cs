// Translated from AoWE.pas.

namespace Aow.Game.Session;

[Flags]
public enum NegotiateModes : byte
{
    None = 0,
    AttackOnReject = 1 << 0,
    MoveOnAccept = 1 << 1,
    PrisonerJoin = 1 << 2,
}
