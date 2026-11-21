// Translated from HSEngine.pas.

namespace Aow.Game.World.Map;

[Flags]
public enum ControlModes : byte
{
    None = 0,
    MultiSelect = 1 << 0,
    ShowSelectionSelf = 1 << 1,
}
