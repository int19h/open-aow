using Aow.Rtl;
using Aow.Units.EventLog;
using Aow.Units.HSEngine;

namespace Aow.Units.AoWE;

public sealed class AITargetQuery : TObject
{
    public TAITargetInfo Info;
    public TAITargetData Data;
}

public sealed class BattleFieldQuery : TObject
{
    public TBattleFieldInfo Info;
}

public sealed class DefenseRequirementsQuery : TObject
{
    public TDefenseRequirements Requirements;
}

public sealed class StormDamageQuery : TObject
{
    public TStormDamageInfo Info;
}

public sealed class CombatResultQuery : TObject
{
    public AnsiString Text;
}

public sealed class ShowMessageNotification : TObject
{
    public TShowMessageInfo Info;
}

public sealed class MapChangedNotification : TObject
{
    public TMapChangedInfo Info;
}

public sealed class ExecuteEventLogNotification : TObject
{
    public TExecuteEventLogInfo Info;
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
