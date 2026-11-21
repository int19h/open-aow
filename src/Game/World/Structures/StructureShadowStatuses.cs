// Translated from AoWE.pas.

namespace Aow.Game.World.Structures;

[Flags]
public enum StructureShadowStatuses : byte
{
    None = 0,
    Connected = 1 << 0,
    Visible = 1 << 1,
}
