// Translated from AoWE.pas.

namespace Aow.Game.Events;

[Flags]
public enum NewTurnEventLogDialogStyles : byte
{
    None = 0,
    UseOverlay = 1 << 0,
    Closeable = 1 << 1,
}
