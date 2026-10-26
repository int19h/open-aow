using Aow.Rtl;
using Aow.Units.HSEngine;

namespace Aow.Units.AoWE;

public partial class TAoWMapField
{
    public bool GetAITarget(TAIGroupControl control, in TAITargetInfo info, ref TAITargetData data)
    {
        TAITargetInfo[] input = [info];
        TAITargetData[] output = [default];
        var message = new TMapFieldMsg { MsgID = 0x1200, Result = 0 };
        message.Param[0] = Ptr<TAITargetInfo>.To(input, 0).Box();
        message.Param[1] = Ptr<TAITargetData>.To(output, 0).Box();
        message.Param[2] = control;
        try { SendMapFieldMsg(ref message); }
        finally { data = output[0]; }
        return data.Priority != 0;
    }

    public void GetDefenseRequirements(sbyte player, ref TDefenseRequirements requirements)
    {
        TDefenseRequirements[] cell = [default];
        var message = new TMapFieldMsg { MsgID = 0x1127, Result = 0 };
        message.Param[0] = player;
        message.Param[1] = Ptr<TDefenseRequirements>.To(cell, 0).Box();
        try { SendMapFieldMsg(ref message); }
        finally { requirements = cell[0]; }
    }
}

public partial class TAoWHSMap
{
    public void RetrieveBattleFieldInfo(int x, int y, int level, int radius, ref TBattleFieldInfo info)
    {
        TBattleFieldInfo[] cell = [info];
        var message = new TMapFieldMsg { MsgID = 0x1108 };
        message.Param[0] = Ptr<TBattleFieldInfo>.To(cell, 0).Box();
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
                        int friendly = cell[0].FriendlyStrength, enemy = cell[0].Strength;
                        field.SendMapFieldMsg(ref message);
                        cell[0].FriendlyStrength = Math.Max(friendly, cell[0].FriendlyStrength);
                        cell[0].Strength = Math.Max(enemy, cell[0].Strength);
                    }
                }
                HSEngine.HSEngine.WalkToNextHN(ref walk);
            }
        }
        finally { info = cell[0]; }
    }
}
