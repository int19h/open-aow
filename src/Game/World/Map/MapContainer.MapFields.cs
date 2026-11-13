
using Aow.Game.Draw;

namespace Aow.Game.World.Map;

public partial class MapContainer
{
    public MapField GetField(int x, int y, int level) => GetMapLevel(level).GetField(x, y);
    public MapField GetFieldMapCoordinate(MapCoordinate location) => GetField(location.X, location.Y, location.MapLevel);

    public void RemoveHexSprite(int x, int y, int level, HexSprite sprite)
    {
        if (GetMapLevel(level).GetField(x, y).RemoveHexSprite(sprite) != -1 && sprite.GetLevel() <= 15)
            _map.InvalidateStaticScene(x, y, level);
    }

    public void InsertHexSprite(int x, int y, int level, HexSprite sprite, int hexNumber)
    {
        GetMapLevel(level).GetField(x, y).AddHexSprite(sprite, hexNumber);
        if (sprite.GetLevel() <= 15) _map.InvalidateStaticScene(x, y, level);
    }
}
