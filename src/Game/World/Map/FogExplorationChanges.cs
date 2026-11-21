// Translated from AoWE.pas.

namespace Aow.Game.World.Map;

[Flags]
public enum FogExplorationChanges : byte
{
    None = 0,
    Fog = 1 << 0,
    Exploration = 1 << 1,
}
