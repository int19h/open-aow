// Translated from HSEngine.pas.

namespace Aow.Game.World.Map;

[Flags]
public enum HexSpriteEngineStatuses : byte
{
    None = 0,
    LoadingHsmSettings = 1 << 0,
    LoadingHsm = 1 << 1,
    SavingHsm = 1 << 2,
    LoadingHss = 1 << 3,
    SavingHss = 1 << 4,
}
