// Translated from Animation.pas.

namespace Aow.Game.Draw.Animations;

public enum AnimationControlModeElement : byte
{
    LinkToNewFrame = 0,
    FixedVisible = 1,
    Selectable = 2,
    Saveable = 3,
    OwnedByMap = 4,
    LinkToOnDestroy = 5
}
