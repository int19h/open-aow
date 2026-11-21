// Translated from Animation.pas.

namespace Aow.Game.Draw.Animations;

[Flags]
public enum AnimationControlModes : byte
{
    None = 0,
    LinkToNewFrame = 1 << 0,
    FixedVisible = 1 << 1,
    Selectable = 1 << 2,
    Saveable = 1 << 3,
    OwnedByMap = 1 << 4,
    LinkToOnDestroy = 1 << 5,
}
