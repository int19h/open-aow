// Translated from Animation.pas.

namespace Aow.Game.Draw.Animations;

[Flags]
public enum TwinkleModes : byte
{
    None = 0,
    Rotate = 1 << 0,
    Expand = 1 << 1,
    Explode = 1 << 2,
    Implode = 1 << 3,
}
