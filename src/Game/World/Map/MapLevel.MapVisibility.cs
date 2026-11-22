
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World.Map;

public partial class MapLevel
{
    private IEnumerable<MapField> Area(int x, int y, int radius)
    {
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, x, y, radius: 0);
        while (walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < Width && walk.Y < Height)
            {
                yield return GetField(walk.X, walk.Y);
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    public bool FogArea(int x, int y, int radius)
    {
        bool result = false;
        foreach (MapField field in Area(x, y, radius))
        {
            int count = field.FogCount;
            if (count == 1)
            {
                result = true;
            }

            field.SetFogCount((count - 1).TruncateToByte());
        }
        return result;
    }

    public bool UnFogArea(int x, int y, int radius)
    {
        bool result = false;
        foreach (MapField field in Area(x, y, radius))
        {
            int count = field.FogCount;
            if (count == 0)
            {
                result = true;
            }

            field.SetFogCount((count + 1).TruncateToByte());
        }
        return result;
    }

    private bool Explore(MapField field, sbyte player)
    {
        if (field.GetExplored(player))
        {
            return false;
        }

        field.SetExplored(player, value: true);
        GameMap map = Map;
        if (map.ExplorationEnabled && map.SeatedPlayer == player)
        {
            Map.InvalidateStaticScene(field.HexX, field.HexY, HexLevel);
        }

        return true;
    }

    public FogExplorationChanges UnFogExploreArea(int x, int y, sbyte player, int radius)
    {
        DelphiAssert.Assert(player >= 0, message: "Invalid Player Index", file: "AoWHSMap.PAS", line: 276);
        FogExplorationChanges result = default;
        foreach (MapField field in Area(x, y, radius))
        {
            int count = field.FogCount;
            if (count == 0)
            {
                result |= FogExplorationChanges.Fog;
            }

            field.SetFogCount((count + 1).TruncateToByte());
            if (Explore(field, player))
            {
                result |= FogExplorationChanges.Exploration;
            }
        }
        return result;
    }

    public bool ExploreArea(int x, int y, sbyte player, int radius)
    {
        bool result = false;
        foreach (MapField field in Area(x, y, radius))
        {
            result |= Explore(field, player);
        }

        return result;
    }

    public void ResetFog()
    {
        for (int i = 0; i < Count; i++)
        {
            GetChild<MapField>(i).SetFogCount(0);
        }
    }

    public void ResetUnFog()
    {
        for (int i = 0; i < Count; i++)
        {
            GetChild<MapField>(i).SetFogCount(1);
        }
    }

    public void Reset()
    {
        for (int i = 0; i < Count; i++)
        {
            GetChild<MapField>(i).FieldFlags = 0;
        }
    }
}
