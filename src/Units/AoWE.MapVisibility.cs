using Aow.Rtl;
using Aow.Units.HSEngine;

namespace Aow.Units.AoWE;

public partial class TAoWMapLevel
{
    private IEnumerable<TAoWMapField> Area(int x, int y, int radius)
    {
        THNWalkData walk = default;
        HSEngine.HSEngine.InitHNWalk(ref walk, x, y, 0);
        while (walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < FWidth && walk.Y < FHeight)
                yield return (TAoWMapField)GetField(walk.X, walk.Y);
            HSEngine.HSEngine.WalkToNextHN(ref walk);
        }
    }

    private static byte FogCount(TAoWMapField field) => unchecked((byte)((uint)field.FFieldFlags >> 16));
    public bool FogArea(int x, int y, int radius)
    {
        bool result = false;
        foreach (var field in Area(x, y, radius))
        {
            byte count = FogCount(field);
            if (count == 1) result = true;
            field.SetFogCount(unchecked((byte)(count - 1)));
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
            field.SetFogCount(unchecked((byte)(count + 1)));
        }
        return result;
    }

    private bool Explore(TAoWMapField field, sbyte player)
    {
        if (field.GetExplored(player)) return false;
        field.SetExplored(player, true);
        var map = (TAbstractAoWHSMap)FMap;
        if (map.FExplorationEnabled && map.FSeatedPlayer == player) FMap.InvalidateStaticScene(field.Xhx, field.Yhx, FLhx);
        return true;
    }

    public TSet<TFogExplorationChangedElement> UnFogExploreArea(int x, int y, sbyte player, int radius)
    {
        Rtl.Rtl.Assert(player >= 0, "Invalid Player Index", "AoWHSMap.PAS", 276);
        TSet<TFogExplorationChangedElement> result = default;
        foreach (var field in Area(x, y, radius))
        {
            byte count = FogCount(field);
            if (count == 0) result += TSet<TFogExplorationChangedElement>.Of(TFogExplorationChangedElement.feFog);
            field.SetFogCount(unchecked((byte)(count + 1)));
            if (Explore(field, player)) result += TSet<TFogExplorationChangedElement>.Of(TFogExplorationChangedElement.feExploration);
        }
        return result;
    }

    public bool ExploreArea(int x, int y, sbyte player, int radius)
    {
        bool result = false;
        foreach (var field in Area(x, y, radius)) result |= Explore(field, player);
        return result;
    }

    public void ResetFog() { for (int i = 0; i < Count; i++) ((TAoWMapField)GetChild(i)).SetFogCount(0); }
    public void ResetUnFog() { for (int i = 0; i < Count; i++) ((TAoWMapField)GetChild(i)).SetFogCount(1); }
    public void Reset() { for (int i = 0; i < Count; i++) ((TAoWMapField)GetChild(i)).FFieldFlags = 0; }
}
