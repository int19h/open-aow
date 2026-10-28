using Aow.Units.NetworkE;

namespace Aow.Units.AoWE;

public partial class TSpellTE
{
    public override TExecuteResult Execute(TTokenExecuter executer)
    {
        base.Execute(executer);
        var source = AoWE.AoWHSMap.FUnitControl.FindUnit(FEventSourceID);
        if (source == null || executer.ExecuteCount == 0 && !Validate((THero)source)) return TExecuteResult.erDoneHalt;
        return Process(source, executer.ExecuteCount) ? TExecuteResult.erRetry : TExecuteResult.erDone;
    }
}
