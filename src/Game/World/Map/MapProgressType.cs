// Translated from HSEngine.pas.

namespace Aow.Game.World.Map;

public enum MapProgressType : byte
{
    Loading = 0,
    Saving = 1,
    Initializing = 2,
    UpdatingTerrain = 3,
    Activating = 4,
    Destroying = 5
}
