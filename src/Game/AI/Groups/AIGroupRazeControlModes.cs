// Translated from AGCPlugins.pas.

namespace Aow.Game.AI.Groups;

[Flags]
public enum AIGroupRazeControlModes : byte
{
    None = 0,
    CannotDefend = 1 << 0,
    OnMigrate = 1 << 1,
    Always = 1 << 2,
}
