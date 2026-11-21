// Translated from DisplayC.pas.

namespace Aow.Game.Draw;

[Flags]
public enum DisplayStatuses : byte
{
    None = 0,
    FrameLoopActive = 1 << 0,
    DirectInputEnabled = 1 << 2,
    Asynchronous = 1 << 3,
    ExclusiveAccess = 1 << 4,
    FrameLoopPaused = 1 << 5,
}
