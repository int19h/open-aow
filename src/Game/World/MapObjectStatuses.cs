// Translated from HSEngine.pas.

namespace Aow.Game.World;

[Flags]
public enum MapObjectStatuses : byte
{
    None = 0,
    Visible = 1 << 0,
    ResourceLinked = 1 << 1,
    EditMode = 1 << 2,
    PlacedOnMap = 1 << 3,
    Selected = 1 << 4,
    Activated = 1 << 5,
}
