// Translated from DisplayC.pas.

namespace Aow.Game.Draw;

[Flags]
public enum DisplayControllerStatuses : byte
{
    None = 0,
    Visible = 1 << 0,
    Activated = 1 << 1,
    Windowed = 1 << 2,
}
