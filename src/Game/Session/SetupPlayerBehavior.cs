// Translated from AoWSetup.pas.

namespace Aow.Game.Session;

/// <summary>
/// Who plays one setup slot (Delphi <c>TSetupPlayerBehavior</c>): the player type and, for a human
/// player, the remote id in <see cref="Flags"/>. <see cref="UnknownBytes"/> holds the three stored
/// bytes after the player type (the Delphi record's padding) in its low 24 bits, the first byte
/// lowest. No code reads them. They are kept so a loaded record saves back unchanged.
/// <see cref="SetupPlayerBehaviorImage"/> is the stored record.
/// </summary>
public readonly record struct SetupPlayerBehavior(byte PlayerType, int Flags)
{
    public int UnknownBytes { get; init; }
}
