using Aow.Rtl;
using Aow.Units.HSEngine;

namespace Aow.Units.AoWE;

public static partial class AoWE
{
    public static void RetrieveStormDamageInfo(sbyte player, TStormType storm, sbyte x, sbyte y,
        sbyte level, sbyte innerRadius, sbyte outerRadius, ref TStormDamageInfo info)
    {
        var query = new StormDamageQuery { Info = new()
        {
            Player = player,
            FriendlyRelations = TSet<byte>.WithBounds(0, 7).With(0).With(2),
            EnemyRelations = TSet<byte>.WithBounds(0, 7).With(1).With(3)
        } };
        var mapLevel = AoWHSMap.MapContainer.GetMapLevel(level);
        var message = new TMapFieldMsg { MsgID = 0x1151 };
        message.Param[0] = (int)storm;
        message.Param[2] = query;
        THNWalkData walk = default;
        HSEngine.HSEngine.InitHNWalk(ref walk, x, y, innerRadius);
        try
        {
            while (walk.Radius <= outerRadius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    message.Param[1] = walk.Radius;
                    mapLevel.GetField(walk.X, walk.Y).SendMapFieldMsg(ref message);
                }
                HSEngine.HSEngine.WalkToNextHN(ref walk);
            }
        }
        finally { info = query.Info; }
    }
}
