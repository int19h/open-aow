
using Aow.Game.Draw;

namespace Aow.Game.World.Map;

public partial class MapContainer
{
    public MapField GetField(int x, int y, int level) => GetMapLevel(level).GetField(x, y);
    public MapField GetFieldXYL(int location) => GetField((sbyte)location, (sbyte)(location >> 8), (sbyte)(location >> 16));

    public void RemoveHS(int x, int y, int level, HexagonSprite sprite)
    {
        if (GetMapLevel(level).GetField(x, y).RemoveHS(sprite) != -1 && sprite.GetLevel() <= 15)
            FMap.InvalidateStaticScene(x, y, level);
    }

    public void InsertHS(int x, int y, int level, HexagonSprite sprite, int hn)
    {
        GetMapLevel(level).GetField(x, y).AddHS(sprite, hn);
        if (sprite.GetLevel() <= 15) FMap.InvalidateStaticScene(x, y, level);
    }
}
