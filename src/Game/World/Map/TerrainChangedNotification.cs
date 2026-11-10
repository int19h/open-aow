// Translated from HSEngine.pas.
using Aow.Rtl;

namespace Aow.Game.World.Map;

public sealed class TerrainChangedNotification
{
    public TerrainChangedInfo Info;

    public struct TerrainChangedInfo
    {
        public Rect Rect;
        public int Level;
        public MapLevel MapLevel;
    }
}
