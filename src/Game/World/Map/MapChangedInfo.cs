// Translated from AoWE.pas.
using Aow.Rtl;

namespace Aow.Game.World.Map;

// current/seated player, message events, message text, message frame, FPS, map-event lock, map
// events, scanner events, scanner lock, fog/exploration, selectors, selector visibility
public struct MapChangedInfo
{
    public Rect Bounds;
    public int Level;
    public MapLevel MapLevel;
}
