
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World.Map;

public partial class MapLevel
{
    private IEnumerable<MapField> Area(int x, int y, int radius)
    {
        HexNumberWalkData walk = default;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, x, y, 0);
        while (walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < Width && walk.Y < Height)
                yield return GetField(walk.X, walk.Y);
            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    private static byte FogCount(MapField field) => (byte)((uint)field.FieldFlags >> 16);

    public bool FogArea(int x, int y, int radius)
    {
        bool result = false;
        foreach (var field in Area(x, y, radius))
        {
            byte count = FogCount(field);
            if (count == 1) result = true;
            field.SetFogCount((byte)(count - 1));
        }
        return result;
    }

    public bool UnFogArea(int x, int y, int radius)
    {
        bool result = false;
        foreach (var field in Area(x, y, radius))
        {
            byte count = FogCount(field);
            if (count == 0) result = true;
            field.SetFogCount((byte)(count + 1));
        }
        return result;
    }

    private bool Explore(MapField field, sbyte player)
    {
        if (field.GetExplored(player)) return false;
        field.SetExplored(player, true);
        var map = Map;
        if (map.ExplorationEnabled && map.SeatedPlayer == player) Map.InvalidateStaticScene(field.HexX, field.HexY, HexLevel);
        return true;
    }

    public Set<FogExplorationChangedElement> UnFogExploreArea(int x, int y, sbyte player, int radius)
    {
        RtlUnit.Assert(player >= 0, "Invalid Player Index", "AoWHSMap.PAS", 276);
        Set<FogExplorationChangedElement> result = default;
        foreach (var field in Area(x, y, radius))
        {
            byte count = FogCount(field);
            if (count == 0) result += Set<FogExplorationChangedElement>.Of(FogExplorationChangedElement.Fog);
            field.SetFogCount((byte)(count + 1));
            if (Explore(field, player)) result += Set<FogExplorationChangedElement>.Of(FogExplorationChangedElement.Exploration);
        }
        return result;
    }

    public bool ExploreArea(int x, int y, sbyte player, int radius)
    {
        bool result = false;
        foreach (var field in Area(x, y, radius)) result |= Explore(field, player);
        return result;
    }

    public void ResetFog() { for (int i = 0; i < Count; i++) ((MapField)GetChild(i)).SetFogCount(0); }

    public void ResetUnFog() { for (int i = 0; i < Count; i++) ((MapField)GetChild(i)).SetFogCount(1); }

    public void Reset() { for (int i = 0; i < Count; i++) ((MapField)GetChild(i)).FieldFlags = 0; }
}
