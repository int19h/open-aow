using Aow.Game.Events;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Ai
{

    [LegacyObject]
    public sealed class AITargetQuery
    {
        public TAITargetInfo Info;
        public TAITargetData Data;

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
        public TBattleFieldInfo Info;

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
        public TDefenseRequirements Requirements;

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
        public TStormDamageInfo Info;

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
        public TShowMessageInfo Info;

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
        public TMapChangedInfo Info;

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
        public TExecuteEventLogInfo Info;

        public ExecuteEventLogNotification()
        {
            CaptureAddress.Assign(this);
        }
    }
}

namespace Aow.Game.Ai.Groups
{

    public partial class TAIMoveExecuterControl
    {
        public TAIMoveExecuterMove CreateMove(TAIMoveExecuterArmy army, byte selection, sbyte x, sbyte y,
            sbyte level, TSet<TCreateMoveModeElement> mode)
        {
            byte result = 0;
            return CreateMove(army, selection, x, y, level, mode, ref result);
        }
    }
}
