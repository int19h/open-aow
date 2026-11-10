using Aow.Game.Events;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Ai
{

    [LegacyObject]
    public sealed class AITargetQuery
    {
        public AITargetInfo Info;
        public AITargetData Data;

        public AITargetQuery()
        {
            CaptureAddress.Assign(this);
        }
    }
}

namespace Aow.Game.Combat
{

    [LegacyObject]
    public sealed class BattleFieldQuery
    {
        public BattleFieldInfo Info;

        public BattleFieldQuery()
        {
            CaptureAddress.Assign(this);
        }
    }
}

namespace Aow.Game.Engine
{

    [LegacyObject]
    public sealed class DefenseRequirementsQuery
    {
        public DefenseRequirements Requirements;

        public DefenseRequirementsQuery()
        {
            CaptureAddress.Assign(this);
        }
    }
}

namespace Aow.Game.Combat
{

    [LegacyObject]
    public sealed class StormDamageQuery
    {
        public StormDamageInfo Info;

        public StormDamageQuery()
        {
            CaptureAddress.Assign(this);
        }
    }

    [LegacyObject]
    public sealed class CombatResultQuery
    {
        public string Text = "";

        public CombatResultQuery()
        {
            CaptureAddress.Assign(this);
        }
    }
}

namespace Aow.Game.Engine
{

    [LegacyObject]
    public sealed class ShowMessageNotification
    {
        public ShowMessageInfo Info;

        public ShowMessageNotification()
        {
            CaptureAddress.Assign(this);
        }
    }
}

namespace Aow.Game.World.Map
{

    [LegacyObject]
    public sealed class MapChangedNotification
    {
        public MapChangedInfo Info;

        public MapChangedNotification()
        {
            CaptureAddress.Assign(this);
        }
    }
}

namespace Aow.Game.Events
{

    [LegacyObject]
    public sealed class ExecuteEventLogNotification
    {
        public ExecuteEventLogInfo Info;

        public ExecuteEventLogNotification()
        {
            CaptureAddress.Assign(this);
        }
    }
}

namespace Aow.Game.Ai.Groups
{

    public partial class AIMoveExecuterControl
    {
        public AIMoveExecuterMove CreateMove(AIMoveExecuterArmy army, byte selection, sbyte x, sbyte y,
            sbyte level, Set<CreateMoveModeElement> mode)
        {
            byte result = 0;
            return CreateMove(army, selection, x, y, level, mode, ref result);
        }
    }
}
