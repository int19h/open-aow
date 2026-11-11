
using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

public partial class MoveControlSettings
{
    public int GetMovePoints(int x, int y, int level) => FLevelData[level] is null ? 0xffffff : FLevelData[level][y * FWidth + x];
    public void SetMovePoints(int x, int y, int level, int value) => FLevelData[level][y * FWidth + x] = value;

    public int GetMovePointsXYL(int packed)
    {
        var location = new MapCoordinate { Packed = packed };
        return GetMovePoints(location.X, location.Y, location.L);
    }

    public void SetMovePointsXYL(int packed, int value)
    {
        var location = new MapCoordinate { Packed = packed };
        SetMovePoints(location.X, location.Y, location.L, value);
    }
}
