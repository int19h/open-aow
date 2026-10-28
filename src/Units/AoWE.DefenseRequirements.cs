using Aow.Rtl;
using Aow.Units.HSEngine;

namespace Aow.Units.AoWE;

public partial class TAoWMapField
{
    public bool GetAITarget(TAIGroupControl control, in TAITargetInfo info, ref TAITargetData data)
    {
        var query = new AITargetQuery { Info = info };
        var message = new TMapFieldMsg { MsgID = 0x1200, Result = 0 };
        message.Param[0] = query;
        message.Param[1] = query;
        message.Param[2] = control;
        try { SendMapFieldMsg(ref message); }
        finally { data = query.Data; }
        return data.Priority != 0;
    }

    public void GetDefenseRequirements(sbyte player, ref TDefenseRequirements requirements)
    {
        var query = new DefenseRequirementsQuery();
        var message = new TMapFieldMsg { MsgID = 0x1127, Result = 0 };
        message.Param[0] = player;
        message.Param[1] = query;
        try { SendMapFieldMsg(ref message); }
        finally { requirements = query.Requirements; }
    }
}

public partial class TAoWHSMap
{
    public void RetrieveBattleFieldInfo(int x, int y, int level, int radius, ref TBattleFieldInfo info)
    {
        var query = new BattleFieldQuery { Info = info };
        var message = new TMapFieldMsg { MsgID = 0x1108 };
        message.Param[0] = query;
        var mapLevel = MapContainer.GetMapLevel(level);
        THNWalkData walk = default;
        HSEngine.HSEngine.InitHNWalk(ref walk, x, y, 0);
        try
        {
            while (walk.Radius <= radius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    var field = (TAoWMapField)mapLevel.GetField(walk.X, walk.Y);
                    if ((field.FFieldFlags & 0x04000000) != 0)
                    {
                        int friendly = query.Info.FriendlyStrength, enemy = query.Info.Strength;
                        field.SendMapFieldMsg(ref message);
                        query.Info.FriendlyStrength = Math.Max(friendly, query.Info.FriendlyStrength);
                        query.Info.Strength = Math.Max(enemy, query.Info.Strength);
                    }
                }
                HSEngine.HSEngine.WalkToNextHN(ref walk);
            }
        }
        finally { info = query.Info; }
    }
}
