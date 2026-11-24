
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.Session;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Map;

public partial class MapField
{
    public bool GetAITarget(AIGroupControl control, in AITargetInfo info, out AITargetData data)
    {
        AITargetQuery query = new() { Info = info };
        MapFieldMessage message = new() { MessageId = 0x1200, Result = 0 };
        MapFieldMessageParameters param = message.Param;
        param[0] = query;
        message.Param = param;
        MapFieldMessageParameters param2 = message.Param;
        param2[1] = query;
        message.Param = param2;
        MapFieldMessageParameters param3 = message.Param;
        param3[2] = control;
        message.Param = param3;
        try
        {
            SendMapFieldMessage(ref message);
        }
        finally
        {
            data = query.Data;
        }
        return data.Priority != 0;
    }

    public void GetDefenseRequirements(PlayerId? player, out DefenseRequirements requirements)
    {
        DefenseRequirementsQuery query = new();
        MapFieldMessage message = new() { MessageId = 0x1127, Result = 0 };
        MapFieldMessageParameters param = message.Param;
        param[0] = Session.MsgArgIds.FromPlayer(player);
        message.Param = param;
        MapFieldMessageParameters param2 = message.Param;
        param2[1] = query;
        message.Param = param2;
        try
        {
            SendMapFieldMessage(ref message);
        }
        finally
        {
            requirements = query.Requirements;
        }
    }
}
