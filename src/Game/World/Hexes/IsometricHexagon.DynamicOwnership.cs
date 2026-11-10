using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World.Hexes;

public partial class IsometricHexagon
{
    protected void UpdateTransitionCore(Type peer, bool water, Action<int, int> shore = null)
    {
        FOpenSides = 0;
        FSides &= 0xC0;
        var field = (MapField)Owner;
        var level = (MapLevel)field.Owner;
        HNWalkData walk = default;
        HNWalkData.InitHNWalk(ref walk, field.Xhx, field.Yhx, 1);
        while (walk.HN <= 6)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                var neighbour = level.GetField(walk.X, walk.Y);
                var sprite = neighbour.FindOwnedHS(peer);
                bool peerFound = sprite != null;
                sprite ??= neighbour.FindOwnedHS(typeof(BorderHexagon));
                if (sprite == null)
                {
                    FOpenSides |= (byte)(1 << (walk.HN - 1));
                    if (water) shore(walk.HN, neighbour.TerrainType switch { 3 => 40, 5 => 80, _ => 0 });
                }
                else if ((!water || peerFound) && neighbour.TerrainType != GetTerrain(0))
                    FSides |= (byte)(1 << (walk.HN - 1));
            }
            HNWalkData.WalkToNextHN(ref walk);
        }
        if (water || FOpenSides != 0)
        {
            if (FDynamic == null) FDynamic = new DynamicIsometricHexagon(this);
        }
        else if (FDynamic != null)
        {
            FDynamic?.Dispose();
            FDynamic = null;
            FSides = (byte)(RtlUnit.Random(4) << 6);
            FResource.Engine.HSMap.InvalidateStaticScene(field.Xhx, field.Yhx, field.Lhx);
        }
    }
}
