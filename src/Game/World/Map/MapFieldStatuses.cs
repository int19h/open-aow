// Translated from HSEngine.pas.

namespace Aow.Game.World.Map;

[Flags]
public enum MapFieldStatuses : byte
{
    None = 0,
    TerrainLocked = 1 << 0,
    Visible = 1 << 1,
    Loading = 1 << 2,
    Comparing = 1 << 3,
}
