// Translated from HSEngine.pas.

namespace Aow.Game.World.Map;

[Flags]
public enum HexSpriteMapStatuses : byte
{
    None = 0,
    Modified = 1 << 0,
    Editting = 1 << 1,
    Activated = 1 << 2,
    Destroying = 1 << 3,
    PreviewMode = 1 << 4,
}
