using Aow.Rtl;
using Aow.Units.EventLog;

namespace Aow.Units.AoWE;

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

[LegacyObject]
public sealed class BattleFieldQuery
{
    public TBattleFieldInfo Info;

    public BattleFieldQuery()
    {
        CaptureAddress.Assign(this);
    }
}

[LegacyObject]
public sealed class DefenseRequirementsQuery
{
    public TDefenseRequirements Requirements;

    public DefenseRequirementsQuery()
    {
        CaptureAddress.Assign(this);
    }
}

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

[LegacyObject]
public sealed class ShowMessageNotification
{
    public TShowMessageInfo Info;

    public ShowMessageNotification()
    {
        CaptureAddress.Assign(this);
    }
}

[LegacyObject]
public sealed class MapChangedNotification
{
    public TMapChangedInfo Info;

    public MapChangedNotification()
    {
        CaptureAddress.Assign(this);
    }
}

[LegacyObject]
public sealed class ExecuteEventLogNotification
{
    public TExecuteEventLogInfo Info;

    public ExecuteEventLogNotification()
    {
        CaptureAddress.Assign(this);
    }
}

public partial class TAIMoveExecuterControl
{
    public TAIMoveExecuterMove CreateMove(TAIMoveExecuterArmy army, byte selection, sbyte x, sbyte y,
        sbyte level, TSet<TCreateMoveModeElement> mode)
    {
        byte result = 0;
        return CreateMove(army, selection, x, y, level, mode, ref result);
    }
}
