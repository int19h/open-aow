// Translated from AoWSetup.pas.

namespace Aow.Game.Session;

/// <summary>
/// Who plays one setup slot (Delphi <c>TSetupPlayerBehavior</c>): the player type and, for a human
/// player, the remote id in <see cref="Flags"/>. <see cref="SetupPlayerBehaviorImage"/> is the
/// stored record, whose bytes 1 to 3 are padding.
/// </summary>
public readonly record struct SetupPlayerBehavior(byte PlayerType, int Flags);
