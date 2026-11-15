// Translated from HSEngine.pas.
using Aow.Rtl;

namespace Aow.Game.World.Map;

public sealed class TerrainChangedNotification
{
    public TerrainChangedInfo Info { get; internal set; }

    public struct TerrainChangedInfo
    {
        public Rect Rectangle { get; set; }
        public int Level { get; set; }
        public MapLevel MapLevel { get; set; }
    }
}
