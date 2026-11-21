// Translated from EventLog.pas.

namespace Aow.Game.Events;

[Flags]
public enum SaveGameEventLogModes : byte
{
    None = 0,
    DisableCancel = 1 << 0,
}
